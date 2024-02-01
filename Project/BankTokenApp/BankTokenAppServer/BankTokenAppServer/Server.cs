using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BankTokenAppServer
{
    /// <summary>
    /// Represents the server entity that will be doing all of the back-end work
    /// </summary>
    public class Server
    {
        #region Properties
        private const int Port = 55555; // Default port on which to initialize socket

        private const string ServerAddress = "127.0.0.1"; // localaddress

        private const string UsersFile = "users.xml"; // users DB file

        private const string TokensFile = "tokens.xml"; // tokens DB file

        private TextBox logBox;

        private List<User> users;

        private List<CardTokenPair> tokens;

        private XmlSerializer usersSerializer;

        private XmlSerializer tokensSerializer;

        private object syncLock;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializing constructor for the server entity
        /// Uses dependency injection for the log box to which data will be output.
        /// </summary>
        /// <param name="logger"></param>
        public Server(TextBox logger)
        {
            users = new List<User>();
            tokens = new List<CardTokenPair>();
            usersSerializer = new XmlSerializer(typeof(List<User>));
            tokensSerializer = new XmlSerializer(typeof(List<CardTokenPair>));
            syncLock = new object();
            logBox = logger;
            LoadUsers();
            LoadTokens();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Helper method to display data to the logbox
        /// </summary>
        /// <param name="message">Message which will be displayed</param>
        private void DisplayMessage(string message)
        {
            if (!logBox.Dispatcher.CheckAccess())
            {
                logBox.Dispatcher.InvokeAsync(new Action(() => logBox.Text += message));
            }
            else
            {
                logBox.Text += message;
            }
        }

        /// <summary>
        /// Helper method used to load the users in memory on startup
        /// Uses path from the UsersFile property
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                using (var reader = new StreamReader(UsersFile))
                {
                    users = (List<User>)usersSerializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading users", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Helper method used to load the card-tokens pairs in memory on startup
        /// Uses path from the UsersFile property
        /// </summary>
        private void LoadTokens()
        {
            if (File.Exists("tokens.xml"))
            {
                using (FileStream stream = new FileStream("tokens.xml", FileMode.Open))
                {
                    if (stream.Length > 0)
                    {
                        tokens = (List<CardTokenPair>)tokensSerializer.Deserialize(stream);

                    }
                    else
                    {
                        tokens = new List<CardTokenPair>();
                    }
                }
            }
            else
            {
                tokens = new List<CardTokenPair>();
            }
        }

        /// <summary>
        /// Helper method used to save the current state of tokens list
        /// to a file (namely TokensFile property)30.
        /// </summary>
        private void SaveTokens()
        {
            try
            {
                if (tokens.Count == 0)
                    return;

                using (var writer = new StreamWriter(TokensFile))
                {
                    tokensSerializer.Serialize(writer, tokens);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving tokens", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Initiates a listener on the local addres with the default port.
        /// Awaits client connections and handles each of them separately in
        /// a different thread
        /// </summary>
        public void Start()
        {
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                TcpListener? listener = new TcpListener(localAddr, Port);
                listener.Start();

                while (true)
                {
                    TcpClient? client = listener.AcceptTcpClient();
                    DisplayMessage($"Client {client} has connected...\n");
                    Thread? clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error starting server", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Helper method that checks if a given string is a valid card number
        /// </summary>
        /// <param name="cardNumber">the string which will be checked</param>
        /// <returns></returns>
        private bool IsValidCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                return false;
            }

            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (cardNumber.Length != 16)
            {
                return false;
            }

            if (!cardNumber.StartsWith("3") && !cardNumber.StartsWith("4") && !cardNumber.StartsWith("5") && !cardNumber.StartsWith("6"))
            {
                return false;
            }

            int sum = 0;
            bool odd = true;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());
                if (odd)
                {
                    sum += digit;
                }
                else
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                    sum += digit;
                }
                odd = !odd;
            }
            return sum % 10 == 0;
        }

        /// <summary>
        /// Helper method used to generate a random token based on 
        /// a given card number
        /// </summary>
        /// <param name="cardNumber">The card number that will be used to generate token upon</param>
        /// <returns></returns>
        private string GenerateToken(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            StringBuilder token = new StringBuilder();
            Random random = new Random();
            int firstDigit = random.Next(0, 10);
            while (firstDigit == 3 || firstDigit == 4 || firstDigit == 5 || firstDigit == 6)
            {
                firstDigit = random.Next(0, 10);
            }
            token.Append(firstDigit);

            for (int i = 1; i < 12; i++)
            {
                int digit = random.Next(0, 10);
                while (digit == int.Parse(cardNumber[i].ToString()))
                {
                    digit = random.Next(0, 10);
                }
                token.Append(digit);
            }

            token.Append(cardNumber.Substring(12));

            return token.ToString();
        }

        /// <summary>
        /// Helper method used to export Card-Token pairs to a file
        /// in the format Token-CardNumber, where the data is sorted by Token
        /// </summary>
        /// <param name="filePath">Path to file in which pairs will be written</param>
        /// <returns></returns>
        public bool ExportTokensByToken(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Token           \tCard Number");

                    var sortedTokens = tokens.OrderBy(pair => pair.Token);

                    foreach (var pair in sortedTokens)
                    {
                        writer.WriteLine($"{pair.Token}\t{pair.CardNumber}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// /// Helper method used to export Card-Token pairs to a file
        /// in the format CardNumber-Token, where the data is sorted by CardNumber
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool ExportTokensByCard(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Card Number     \tToken");

                    var sortedTokens = tokens.OrderBy(pair => pair.CardNumber);

                    foreach (var pair in sortedTokens)
                    {
                        writer.WriteLine($"{pair.CardNumber}\t{pair.Token}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Handles the currently connected client by sending and receiving data upon request
        /// </summary>
        /// <param name="client"></param>
        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream? stream = client.GetStream();
                StreamReader? reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter? writer = new StreamWriter(stream, Encoding.UTF8);
                bool isLoggedIn = false;

                string? username;
                string? password;
                User? user = null;

                while (!isLoggedIn)
                {
                    username = reader.ReadLine();
                    password = reader.ReadLine();

                    user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

                    if (user == null)
                    {
                        writer.WriteLine("Invalid username or password");
                        writer.Flush();
                    }
                    else
                    {
                        isLoggedIn = true;
                    }
                }

                writer.WriteLine("Login successful");
                writer.Flush();

                writer.WriteLine(user.CanRegisterToken);
                writer.WriteLine(user.CanRetrieveCardNumber);
                writer.Flush();

                while (true)
                {
                    string? request = reader.ReadLine();

                    if (request == null || request == "disconnect")
                    {
                        DisplayMessage($"Client {client} has disconnected.\n");
                        break;
                    }

                    if (request == "register")
                    {
                        if (user.CanRegisterToken)
                        {
                            string? cardNumber = reader.ReadLine();

                            if (IsValidCardNumber(cardNumber))
                            {
                                string? token = GenerateToken(cardNumber);

                                CardTokenPair? pair = new CardTokenPair(cardNumber, token);
                                lock (syncLock)
                                {
                                    tokens.Add(pair);
                                }

                                SaveTokens();
                                writer.WriteLine(token);
                                writer.Flush();
                            }
                            else
                            {
                                writer.WriteLine("Invalid card number");
                                writer.Flush();
                            }
                        }
                        else
                        {
                            writer.WriteLine("You do not have permission to register tokens");
                            writer.Flush();
                        }
                    }

                    if (request == "retrieve")
                    {
                        if (user.CanRetrieveCardNumber)
                        {
                            string? token = reader.ReadLine();

                            CardTokenPair? pair = tokens.FirstOrDefault(p => p.Token == token);

                            if (pair != null)
                            {
                                writer.WriteLine(pair.CardNumber);
                                writer.Flush();
                            }
                            else
                            {
                                writer.WriteLine("Token not registered");
                                writer.Flush();
                            }
                        }
                        else
                        {
                            writer.WriteLine("You do not have permission to retrieve card numbers");
                            writer.Flush();
                        }
                    }
                }

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error handling client", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}

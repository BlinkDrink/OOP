using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BankTokenAppServer
{
    public class Server
    {
        private const int Port = 55555;

        private const string ServerAddress = "127.0.0.1";

        private const string UsersFile = "users.xml";

        private const string TokensFile = "tokens.xml";

        private TextBox logBox;

        private List<User> users;

        private List<CardTokenPair> tokens;

        private XmlSerializer usersSerializer;
        private XmlSerializer tokensSerializer;

        private object syncLock;

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

        private void SaveUsers()
        {
            try
            {
                using (var writer = new StreamWriter(UsersFile))
                {
                    usersSerializer.Serialize(writer, users);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving users", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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
    }
}

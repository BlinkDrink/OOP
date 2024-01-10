using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace BankTokenServer
{
    public partial class MainWindow : Window
    {

        int port = 55000;
        TcpListener listener;
        private NetworkStream? stream;
        private XDocument usersDB;
        private XDocument tokensDB;

        public MainWindow()
        {
            InitializeComponent();
            InitializeServer();
        }

        private void InitializeServer()
        {
            usersDB = XDocument.Load("users.xml"); // Зареждане на файл с потребители
            tokensDB = XDocument.Load("tokens.xml"); // Зареждане на файл със съответствия между номер на карта и токен
            Task.Run(() => StartServer());
        }

        private async Task StartServer()
        {
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                listener = new TcpListener(localAddr, port);
                listener.Start();

                DisplayMessage("Server started...\r\n");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    DisplayMessage("Client successfully connected");
                    stream = client.GetStream();
                    _ = HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage($"Server error: {ex.Message}\r\n");
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on exit: " + ex.Message);
            }

            System.Environment.Exit(System.Environment.ExitCode);
        }

        public async Task HandleClient(TcpClient client)
        {
            bool isLoggedIn = false;
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string[] requestData = dataReceived.Split('|');

                        if (requestData.Length >= 2)
                        {
                            if (!isLoggedIn)
                            {
                                string command = requestData[0];
                                string username = requestData[1];
                                string password = requestData[2];

                                isLoggedIn = CheckCredentials(username, password);

                                await SendToClient(isLoggedIn);

                                if (!isLoggedIn)
                                {
                                    MessageBox.Show("Invalid credentials, try again.", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                string command = requestData[0];
                                if (command == "REGISTER_TOKEN")
                                {
                                    string cardNumber = requestData[1];
                                    bool isTokenRegistered = await RegisterToken(cardNumber);

                                    string response = isTokenRegistered ? "TOKEN_REGISTERED" : "TOKEN_REGISTRATION_FAILED";
                                    byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
                                    await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                                }
                                else if (command == "GET_CARD_NUMBER")
                                {
                                    string token = requestData[3];
                                    string cardNumber = await GetCardNumber(token);

                                    byte[] responseBuffer = Encoding.UTF8.GetBytes(cardNumber);
                                    await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                                }
                                else
                                {
                                    byte[] responseBuffer = Encoding.UTF8.GetBytes("INVALID_COMMAND");
                                    await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage($"Client error: {ex.Message}\r\n");
            }
            finally
            {
                client.Close();
            }
        }

        private async Task SendToClient(bool isAuthenticated)
        {
            try
            {
                string message = isAuthenticated ? "AUTHENTICATED" : "INVALID_CREDENTIALS";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending data to client: {ex.Message}");
            }
        }

        private async Task<bool> RegisterToken(string cardNumber)
        {
            bool isTokenRegistered = IsCardRegistered(cardNumber);

            if (!isTokenRegistered)
            {
                string token = TokenizeCard(cardNumber);
                SaveTokenToXML(cardNumber, token);
            }

            return !isTokenRegistered;
        }

        private bool IsCardRegistered(string cardNumber)
        {
            var existingCard = tokensDB.Descendants("Token")
                                        .FirstOrDefault(t => t.Element("CardNumber")?.Value == cardNumber);
            return existingCard != null;
        }

        private async Task<string> GetCardNumber(string token)
        {
            return "1234 5678 9012 3456";
        }

        private async void DisplayMessage(string message)
        {
            if (!LogBox.Dispatcher.CheckAccess())
            {
                await LogBox.Dispatcher.InvokeAsync(new Action(() => LogBox.Text += message));
            }
            else
            {
                LogBox.Text += message;
            }
        }

        public bool CheckCredentials(string username, string password)
        {
            var user = usersDB.Descendants("User")
                .FirstOrDefault(u =>
                    u.Element("Username")?.Value == username &&
                    u.Element("Password")?.Value == password);

            return user != null;
        }

        private string TokenizeCard(string cardNumber)
        {
            Random rand = new Random();
            string token = String.Empty;
            int digitSum = 0;

            while (digitSum % 10 == 0)
            {
                StringBuilder tokenAttempt = new StringBuilder(cardNumber);

                // Generating the first number of the token
                HashSet<int> forbiddenNums = new HashSet<int>() { 3, 4, 5, 6 };
                int firstDigit = rand.Next(0, 10);
                while (forbiddenNums.Contains(firstDigit))
                    firstDigit = rand.Next(0, 10);

                tokenAttempt[0] = char.Parse(firstDigit.ToString());

                // Generating the next 11 numbers
                for (int i = 1; i < 12; i++)
                {
                    int newDigit = rand.Next(0, 10);

                    while (newDigit == int.Parse(tokenAttempt[i].ToString()))
                    {
                        newDigit = rand.Next(0, 10);
                    }

                    tokenAttempt[i] = char.Parse(newDigit.ToString());
                }

                digitSum = tokenAttempt.ToString().Sum(c => c - '0');
                token = tokenAttempt.ToString();
            }

            return token;
        }

        private void SaveTokenToXML(string cardNumber, string token)
        {
            XElement newToken = new XElement("Token",
                                    new XElement("CardNumber", cardNumber),
                                    new XElement("TokenValue", token));

            tokensDB.Element("Tokens")?.Add(newToken);

            tokensDB.Save("tokens.xml");
        }
    }
}
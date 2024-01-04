using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace BankTokenServer
{
    public partial class MainWindow : Window
    {
        int port = 55000;
        TcpListener listener;
        private NetworkStream? stream;
        private XDocument userCredentials;
        private XDocument tokenData;

        public MainWindow()
        {
            InitializeComponent();
            InitializeServer();
        }

        private void InitializeServer()
        {
            userCredentials = XDocument.Load("users.xml"); // Зареждане на файл с потребители
            //tokenData = XDocument.Load("tokens.xml"); // Зареждане на файл със съответствия между номер на карта и токен

            // Запускане на асинхронен метод за стартиране на сървъра
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
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string[] requestData = dataReceived.Split('|'); // Допустимо разделяне на заявките

                        if (requestData.Length >= 2)
                        {
                            string command = requestData[0];
                            string username = requestData[1];
                            string password = requestData[2]; // Предполагайки, че имаме команда, потребителско име и парола

                            bool isAuthenticated = CheckCredentials(username, password);

                            await SendToClient(isAuthenticated);

                            if (isAuthenticated)
                            {
                                // Проверка на командата
                                if (command == "REGISTER_TOKEN")
                                {
                                    string cardNumber = requestData[3]; // Вземане на номер на карта от заявката
                                    bool isTokenRegistered = await RegisterToken(cardNumber); // Метод за регистрация на токена

                                    // Изпращане на отговор към клиента
                                    string response = isTokenRegistered ? "TOKEN_REGISTERED" : "TOKEN_REGISTRATION_FAILED";
                                    byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
                                    await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                                }
                                else if (command == "GET_CARD_NUMBER")
                                {
                                    string token = requestData[3]; // Вземане на токен от заявката
                                    string cardNumber = await GetCardNumber(token); // Метод за връщане на номер на карта

                                    // Изпращане на отговор към клиента
                                    byte[] responseBuffer = Encoding.UTF8.GetBytes(cardNumber);
                                    await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                                }
                                else
                                {
                                    // Невалидна команда
                                    byte[] responseBuffer = Encoding.UTF8.GetBytes("INVALID_COMMAND");
                                    await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid credentials, try again.", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            // TODO: Логика за регистрация на токена (токенизация, валидация и запис в XML)
            return true; // Примерен отговор
        }

        private async Task<string> GetCardNumber(string token)
        {
            // TODO: Логика за връщане на номер на карта по токен (проверка в XML и връщане на стойност)
            return "1234 5678 9012 3456"; // Примерен отговор
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
            var user = userCredentials.Descendants("User")
                .FirstOrDefault(u =>
                    u.Element("Username")?.Value == username &&
                    u.Element("Password")?.Value == password);

            return user != null;
        }

        private string TokenizeCard(string cardNumber)
        {
            // Генериране на токен
            Random rand = new Random();
            string token = rand.Next(1000, 9999).ToString(); // Генериране на първите 4 цифри на токена
            token += cardNumber.Substring(4, 12); // Добавяне на следващите 12 цифри от номера на картата
            return token;
        }

        private void SaveTokenToXML(string cardNumber, string token)
        {
            // Запазване на съответствието между токен и номер на картата в XML
            // Примерно използване на System.Xml:
            // Създаване на XML документ и добавяне на елементи за токена и номера на картата
            XmlDocument doc = new XmlDocument();
            doc.Load("tokens.xml"); // Зареждане на съществуващия XML файл
            XmlElement root = doc.DocumentElement;

            XmlElement tokenElement = doc.CreateElement("Token");
            XmlElement cardNumberElement = doc.CreateElement("CardNumber");

            tokenElement.InnerText = token;
            cardNumberElement.InnerText = cardNumber;

            root.AppendChild(tokenElement);
            tokenElement.AppendChild(cardNumberElement);

            doc.Save("tokens.xml"); // Запазване на обновения XML файл
        }
    }
}
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Xml;

namespace BankTokenServer
{
    public partial class MainWindow : Window
    {
        private TcpListener serverSocket;
        private TcpClient clientSocket;
        private NetworkStream networkStream;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serverSocket = new TcpListener(IPAddress.Any, 8888);
                serverSocket.Start();
                LogBox.Text += "Сървърът е стартиран.\n";

                clientSocket = await serverSocket.AcceptTcpClientAsync();
                LogBox.Text += "Клиентът се е свързал.\n";

                await Task.Run(() => HandleClientCommunication());
            }
            catch (Exception ex)
            {
                LogBox.Text += "Грешка: " + ex.Message + "\n";
            }
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

        private void HandleClientCommunication()
        {
            try
            {
                string message = dataFromClient.Trim(); // Полученото съобщение от клиента

                if (message.StartsWith("REGISTER_TOKEN")) // Ако съобщението е за регистрация на токен
                {
                    string[] parts = message.Split('|'); // Формат: REGISTER_TOKEN|cardNumber
                    if (parts.Length == 2)
                    {
                        string cardNumber = parts[1];

                        // Проверки за валидност на номера на кредитната карта и други валидации

                        // Токенизация
                        string token = TokenizeCard(cardNumber);

                        // Запазване на токена и номера на картата в XML
                        SaveTokenToXML(cardNumber, token);

                        // Отговор до клиента
                        string serverResponse = "Токенът е регистриран успешно: " + token;
                        byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                    }
                }
                else if (message.StartsWith("GET_CARD_NUMBER")) // Ако съобщението е за извличане на номер на карта по токен
                {
                    // Логика за извличане на номера на картата по токен и изпращане към клиента
                }

                // ... (останала логика)
            }
            catch (Exception ex)
            {
                LogBox.Text += "Грешка: " + ex.Message + "\n";
            }
        }


        //private void HandleClientCommunication()
        //{
        //    try
        //    {
        //        NetworkStream networkStream = clientSocket.GetStream();
        //        byte[] bytesFrom = new byte[10025];
        //        int bytesRead = networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
        //        string dataFromClient = Encoding.ASCII.GetString(bytesFrom, 0, bytesRead);
        //        LogBox.Text += "Съобщение от клиента: " + dataFromClient + "\n";

        //        string serverResponse = "Сървърът е получил съобщението.";
        //        byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
        //        networkStream.Write(sendBytes, 0, sendBytes.Length);
        //        networkStream.Flush();

        //        clientSocket.Close();
        //        serverSocket.Stop();
        //        LogBox.Text += "Изход от сървъра.\n";
        //    }
        //    catch (Exception ex)
        //    {
        //        LogBox.Text += "Грешка: " + ex.Message + "\n";
        //    }
        //}
    }
}
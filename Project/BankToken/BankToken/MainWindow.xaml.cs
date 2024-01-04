using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace BankTokenServer
{
    public partial class MainWindow : Window
    {
        private Thread readThread;
        int port = 55000;
        TcpListener listener;
        private List<TcpClient> connections;
        private NetworkStream? stream;
        private BinaryWriter? writer;
        private BinaryReader? reader;
        private XDocument userCredentials;

        public MainWindow()
        {
            InitializeComponent();
            connections = new List<TcpClient>();
            userCredentials = XDocument.Load("users.xml");
            RunServer();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                // Clean up resources
                writer?.Close();
                reader?.Close();

                foreach (TcpClient client in connections)
                {
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on exit: " + ex.Message);
            }

            System.Environment.Exit(System.Environment.ExitCode);
        }

        private async void RunServer()
        {
            int counter = 1;
            try
            {
                IPAddress local = IPAddress.Parse("127.0.0.1");
                listener = new TcpListener(local, port);
                listener.Start();

                DisplayMessage("Server started...\r\n");

                while (true)
                {
                    DisplayMessage("Waiting for connection\r\n");
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    DisplayMessage("Connection " + counter + " received.\r\n");
                    connections.Add(client);
                    _ = HandleClient(client);
                    counter++;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }

        public async Task HandleClient(TcpClient client)
        {
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            string username, password;
            bool credentialsMatch;
            try
            {
                do
                {
                    username = reader.ReadString();
                    password = reader.ReadString();
                    credentialsMatch = CheckCredentials(username, password);
                    writer.Write(credentialsMatch);
                } while (!credentialsMatch);

                string clientMessage;
                do
                {
                    clientMessage = reader.ReadString();

                } while (clientMessage != "EXIT");

                DisplayMessage("\r\nUser terminated connection\r\n");
                await writer.DisposeAsync();
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                writer.Close();
                reader.Close();
                stream.Close();
                client.Close();
            }
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
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

        TcpListener listener;
        private TcpClient? connection;
        private NetworkStream? stream;
        private BinaryWriter? writer;
        private BinaryReader? reader;

        public MainWindow()
        {
            InitializeComponent();
            readThread = new Thread(new ThreadStart(RunServer));
            readThread.Start();
        }

        private async void RunServer()
        {
            int counter = 1;

            // wait for a client connection and display the text
            // that the client sends
            try
            {
                // Step 1: create TcpListener                    
                IPAddress local = IPAddress.Parse("127.0.0.1");
                listener = new TcpListener(local, 50000);

                // Step 2: TcpListener waits for connection request
                listener.Start();

                // Step 3: establish connection upon client request
                while (true)
                {
                    DisplayMessage("Waiting for connection\r\n");

                    // accept an incoming connection     
                    //connection = listener.AcceptSocket();
                    connection = listener.AcceptTcpClient();

                    DisplayMessage("Connection " + counter + " received.\r\n");

                    Thread clientThread = new Thread(() => HandleClient(connection));
                    clientThread.Start();

                    DisplayMessage("\r\nUser terminated connection\r\n");
                    connection?.Close();
                    counter++;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }

        public void HandleClient(TcpClient client)
        {
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);

            try
            {
                string username = reader.ReadString();
                string password = reader.ReadString();

                // Check username and password against XML data
                bool credentialsMatch = CheckCredentials(username, password);

                // Send response back to client
                writer.Write(credentialsMatch);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        private void DisplayMessage(string message)
        {
            if (!LogBox.Dispatcher.CheckAccess())
            {
                LogBox.Dispatcher.Invoke(new Action(() => LogBox.Text += message));
            }
            else
            {
                LogBox.Text += message;
            }
        }

        public bool CheckCredentials(string username, string password)
        {
            // Load XML file with user credentials
            XDocument doc = XDocument.Load("Users.xml");

            // Search for the provided username and password in the XML
            var user = doc.Descendants("User")
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
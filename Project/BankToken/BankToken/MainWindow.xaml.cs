using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Xml;

namespace BankTokenServer
{
    public partial class MainWindow : Window
    {
        private Thread readThread;

        private Socket? connection;
        private NetworkStream? socketStream;
        private BinaryWriter? writer;
        private BinaryReader? reader;

        //private TcpListener connection;
        //private TcpClient clientSocket;
        //private NetworkStream networkStream;

        public MainWindow()
        {
            InitializeComponent();
            readThread = new Thread(new ThreadStart(RunServer));
            readThread.Start();
        }

        private async void RunServer()
        {
            TcpListener listener;
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
                    connection = listener.AcceptSocket();

                    // create NetworkStream object associated with socket
                    socketStream = new NetworkStream(connection);

                    // create objects for transferring data across stream
                    writer = new BinaryWriter(socketStream);
                    reader = new BinaryReader(socketStream);

                    DisplayMessage("Connection " + counter + " received.\r\n");

                    // inform client that connection was successfull  
                    writer.Write("SERVER>>> Connection successful");

                    string theReply = "";

                    // Step 4: read string data sent from client
                    do
                    {
                        try
                        {
                            // read the string sent to the server
                            theReply = reader.ReadString();

                            // display the message
                            DisplayMessage("\r\n" + theReply);
                        } // end try
                        catch (Exception)
                        {
                            // handle exception if error reading data
                            break;
                        } // end catch
                    } while (theReply != "CLIENT>>> TERMINATE" &&
                       connection.Connected);

                    DisplayMessage("\r\nUser terminated connection\r\n");

                    writer?.Close();
                    reader?.Close();
                    socketStream?.Close();
                    connection?.Close();

                    counter++;
                } // end while
            } // end try
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            } // end catch
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
    }
}
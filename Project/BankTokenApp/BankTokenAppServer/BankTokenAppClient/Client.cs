using BankTokenAppServer;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace BankTokenAppClient
{
    /// <summary>
    /// Represents the client entity that will connect to the server and send different requests
    /// </summary>
    public class Client
    {
        #region Properties
        private const int Port = 55555;

        private const string ServerAddress = "127.0.0.1";

        private TcpClient? client;

        public User? LoggedUser { get; private set; }

        private NetworkStream? stream;
        private StreamReader? reader;
        private StreamWriter? writer;
        #endregion

        #region Constructors
        public Client()
        {
            try
            {
                client = new TcpClient(ServerAddress, Port);
                stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8);
                LoggedUser = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error connecting to server", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region HelperMethods
        /// <summary>
        /// Logs the current client in. Sends username and password to the server
        /// after which the server responds with an according answer (Login successful or Login failed)
        /// </summary>
        /// <param name="username">clients username</param>
        /// <param name="password">clients password</param>
        /// <returns>true if the login was succcessful, false otherwise</returns>
        public bool Login(string username, string password)
        {
            try
            {
                writer.WriteLine(username);
                writer.WriteLine(password);
                writer.Flush();

                string result = reader.ReadLine();

                if (result == "Login successful")
                {
                    string register = reader.ReadLine();
                    string retrieve = reader.ReadLine();

                    bool canRegister = bool.Parse(register);
                    bool canRetrieve = bool.Parse(retrieve);
                    LoggedUser = new User(username, password, canRegister, canRetrieve);
                    return true;
                }
                else
                {
                    MessageBox.Show(result, "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error logging in", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Simple method used to check whether a given card number is in valid format
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        private bool ValidateCardNumber(string cardNumber)
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
        /// Simple method used to check whether a given token is in valid format
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            token = token.Replace(" ", "").Replace("-", "");

            if (token.Length != 16)
            {
                return false;
            }

            if (token.StartsWith("3") || token.StartsWith("4") || token.StartsWith("5") || token.StartsWith("6"))
            {
                return false;
            }

            int sum = 0;
            for (int i = 0; i < token.Length; i++)
            {
                int digit = int.Parse(token[i].ToString());
                sum += digit;
            }
            return sum % 10 != 0;
        }

        /// <summary>
        /// Method used to register token by card number
        /// Sends the card number to the server and the server responds
        /// with the newly generated token
        /// </summary>
        /// <param name="cardNumber">Card number by which the registration of token will be done</param>
        /// <returns></returns>
        public string RegisterToken(string cardNumber)
        {
            try
            {
                if (ValidateCardNumber(cardNumber))
                {
                    writer.WriteLine("register");
                    writer.WriteLine(cardNumber);
                    writer.Flush();

                    string result = reader.ReadLine();

                    if (ValidateToken(result))
                    {
                        return result;
                    }
                    else
                    {
                        MessageBox.Show(result, "Register token failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid card number format", "Register token failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error registering token", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Method used to retrieve card number by token 
        /// Requests it from the server and returns it to the view
        /// </summary>
        /// <param name="token">Token number by which the server will search for card number</param>
        /// <returns>The card number (if found), null otherwise</returns>
        public string RetrieveCardNumber(string token)
        {
            try
            {
                if (ValidateToken(token))
                {
                    writer.WriteLine("retrieve");
                    writer.WriteLine(token);
                    writer.Flush();

                    string result = reader.ReadLine();

                    if (ValidateCardNumber(result))
                    {
                        return result;
                    }
                    else
                    {
                        MessageBox.Show(result, "Retrieve card number failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid token format", "Retrieve card number failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error retrieving card number", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Closes the existing connection with the server if there is one
        /// </summary>
        public void Close()
        {
            try
            {
                if (client != null && client.Connected)
                {
                    StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8);

                    writer.WriteLine("disconnect");
                    writer.Flush();

                    client?.Close();
                    reader?.Close();
                    writer?.Close();
                    stream?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error closing connection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks whether the current client instance is connected to the server
        /// </summary>
        /// <returns>True if connected, false otherwise</returns>
        public bool Connected()
        {
            return client != null && client.Connected;
        }
        #endregion
    }
}

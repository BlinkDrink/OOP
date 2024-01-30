using BankTokenAppServer;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace BankTokenAppClient
{
    // Клас, който представлява клиент, който се свързва със сървъра и изпраща заявки за токенизация
    public class Client
    {
        private const int Port = 55555;

        private const string ServerAddress = "127.0.0.1";

        private const string UsernamePattern = @"^[a-zA-Z0-9_]{3,20}$";

        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";

        private const string CardNumberPattern = @"^([3-6]\d{3}[\s-]?){3}\d{4}$";

        private const string TokenPattern = @"^([0-2,7-9]\d{3}[\s-]?){3}\d{4}$";

        private TcpClient? client;

        public User LoggedUser { get; private set; }

        private NetworkStream? stream;
        private StreamReader? reader;
        private StreamWriter? writer;

        public Client()
        {
            try
            {
                client = new TcpClient(ServerAddress, Port);

                stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error connecting to server", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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

        private bool ValidatePassword(string password)
        {
            // Проверява дали паролата е null или празна
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Проверява дали паролата отговаря на регулярния израз
            Regex regex = new Regex(PasswordPattern);
            return regex.IsMatch(password);
        }

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

        public bool Connected()
        {
            return client != null && client.Connected;
        }

        private bool ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            Regex regex = new Regex(UsernamePattern);
            return regex.IsMatch(username);
        }
    }
}

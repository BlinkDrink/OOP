using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankTokenClient
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private LoginForm _loginFormInstance;
        private NetworkStream _stream;
        private TcpClient _client;
        private string _creditCardNumber;
        private bool _isValidCreditCard;

        public event PropertyChangedEventHandler? PropertyChanged;
        public static readonly DependencyProperty IsValidBankCardProperty =
            DependencyProperty.Register("IsValidBankCard", typeof(bool), typeof(LoginForm), new PropertyMetadata(false));

        public string CreditCardNumber
        {
            get { return _creditCardNumber; }
            set
            {
                _creditCardNumber = value;
                IsValidCreditCard = IsCreditCardValid(_creditCardNumber);
                OnPropertyChanged(nameof(CreditCardNumber));
            }
        }

        public bool IsValidBankCard
        {
            get { return (bool)GetValue(IsValidBankCardProperty); }
            set { SetValue(IsValidBankCardProperty, value); }
        }

        public bool IsValidCreditCard
        {
            get { return _isValidCreditCard; }
            set
            {
                _isValidCreditCard = value;
                OnPropertyChanged(nameof(IsValidCreditCard));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            messageLabel.Visibility = Visibility.Hidden;
            inputTextBox.Visibility = Visibility.Hidden;
            getButton.Visibility = Visibility.Hidden;
            registerButton.Visibility = Visibility.Hidden;
            _loginFormInstance = loginForm;
            _loginFormInstance.LoginStatus += LoginHandler;
            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                _stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on exit: " + ex.Message);
            }

            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void LoginHandler(object sender, LoginEventArgs e)
        {
            _client = e.Client;
            _stream = e.Stream;
            loginForm.Visibility = Visibility.Hidden;
            inputTextBox.Visibility = Visibility.Visible;
            messageLabel.Visibility = Visibility.Visible;
            getButton.Visibility = Visibility.Visible;
            registerButton.Visibility = Visibility.Visible;
        }



        public static bool IsCreditCardValid(string creditCardNumber)
        {
            int sum = 0;
            bool alternate = true;

            for (int i = creditCardNumber.Length - 1; i >= 0; i--)
            {
                int digit = creditCardNumber[i] - '0';

                if (alternate)
                {
                    digit *= 2;

                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }
                sum += digit;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }

        private async Task SendData(string data)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                await _stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<string> ReceiveResponse()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return string.Empty;
            }
        }

        private void RegisterCardButton_Click(object sender, RoutedEventArgs e)
        {
            string cardNumber = inputTextBox.Text;

            if (IsCreditCardValid(cardNumber))
            {
                IsValidBankCard = true; // Промяна на стойността на Dependency property
            }
            else
            {
                IsValidBankCard = false; // Промяна на стойността на Dependency property
            }
        }

        private void GetCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                CreditCardNumber = textBox.Text;
            }
        }
    }
}
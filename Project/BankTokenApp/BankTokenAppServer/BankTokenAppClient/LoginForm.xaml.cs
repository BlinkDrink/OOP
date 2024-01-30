using System.Windows;
using System.Windows.Controls;

namespace BankTokenAppClient
{
    public partial class LoginForm : UserControl
    {
        public event EventHandler<LoginEventArgs> LoginStatus;
        private Client client;

        private const int Port = 55555;

        public LoginForm()
        {
            InitializeComponent();
            client = new Client();
        }

        protected virtual void OnLogin(LoginEventArgs e)
        {
            LoginStatus?.Invoke(this, e);
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (client.Connected())
            {
                if (client.Login(usernameTextBox.Text, passwordBox.Password))
                {
                    MessageBox.Show("You have logged in successfully", "Login", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (LoginStatus != null)
                    {
                        LoginEventArgs loginEventArgs = new LoginEventArgs(client);
                        OnLogin(loginEventArgs);
                    }
                }
                else
                {
                    messageTextBlock.Text = "Login Failed!";
                }
            }
            else
            {
                messageTextBlock.Text = "Connection problem";
            }
        }
    }
}
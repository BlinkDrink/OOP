using System.Windows;
using System.Windows.Controls;

namespace BankTokenAppClient
{
    public partial class LoginForm : UserControl
    {
        #region EventArgs
        public event EventHandler<LoginEventArgs> LoginStatus;
        #endregion

        #region Properties
        private Client client; // The client entity that will be used to send/receive data to/from the user
        #endregion

        #region Constructors
        public LoginForm()
        {
            InitializeComponent();
            client = new Client();
        }
        #endregion

        #region EventHandlerMethods
        /// <summary>
        /// Method used to send data to MainWindow upon successful login attempt
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLogin(LoginEventArgs e)
        {
            LoginStatus?.Invoke(this, e);
        }
        #endregion

        #region Button Clicks
        /// <summary>
        /// Button click method handling the login action
        /// Upon successful login attempt it notifies the user with a message box
        /// Upon failed login attempt it notifies the user through a text block with red foreground
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    messageTextBlock.Text = "Invalid username or password";
                }
            }
            else
            {
                messageTextBlock.Text = "Connection problem";
            }
        }
        #endregion
    }
}
using System.ComponentModel;
using System.Windows;

namespace BankTokenAppClient
{
    public partial class MainWindow : Window
    {
        #region Properties
        private Client client;
        private LoginForm loginFormInstance;
        private CardTokenMenu cardTokenMenuInstance;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            loginFormInstance = loginForm;
            loginFormInstance.Visibility = Visibility.Visible;

            cardTokenMenuInstance = cardTokenMenu;
            cardTokenMenuInstance.Visibility = Visibility.Hidden;

            loginFormInstance.LoginStatus += LoginHandler;
        }

        #region EventHandlers
        /// <summary>
        /// Upon successful login attempt, LoginHandler gets triggered
        /// and retrieves the data from the LoginEventArgs
        /// Sets the view accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginHandler(object sender, LoginEventArgs e)
        {
            client = e.ConnectedClient;
            cardTokenMenuInstance.setClient(e.ConnectedClient);
            loginForm.Visibility = Visibility.Hidden;
            cardTokenMenuInstance.Visibility = Visibility.Visible;
        }
        #endregion

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on exit: " + ex.Message);
            }

            System.Environment.Exit(System.Environment.ExitCode);
        }
    }
}
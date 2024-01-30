using System.ComponentModel;
using System.Windows;

namespace BankTokenAppClient
{
    public partial class MainWindow : Window
    {
        private Client client;
        private LoginForm loginFormInstance;
        private CardTokenMenu cardTokenMenuInstance;

        public MainWindow()
        {
            InitializeComponent();
            loginFormInstance = loginForm;
            loginFormInstance.Visibility = Visibility.Visible;

            cardTokenMenuInstance = cardTokenMenu;
            cardTokenMenuInstance.Visibility = Visibility.Hidden;

            loginFormInstance.LoginStatus += LoginHandler;
        }
        private void LoginHandler(object sender, LoginEventArgs e)
        {
            client = e.ConnectedClient;
            cardTokenMenuInstance.setClient(e.ConnectedClient);
            loginForm.Visibility = Visibility.Hidden;
            cardTokenMenuInstance.Visibility = Visibility.Visible;
        }

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
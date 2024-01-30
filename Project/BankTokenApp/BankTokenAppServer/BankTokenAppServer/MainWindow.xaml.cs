using System.ComponentModel;
using System.Windows;

namespace BankTokenAppServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server server;

        public MainWindow()
        {
            InitializeComponent();
            server = new Server(logTextBox);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            if (server != null)
            {
                Thread serverThread = new Thread(server.Start);

                serverThread.Start();
            }

            StartServerButton.IsEnabled = false;
            logTextBox.Text += "Server is listening for incoming connections...\n";
        }

    }
}
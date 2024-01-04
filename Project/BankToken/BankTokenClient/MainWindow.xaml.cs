using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace BankTokenClient
{
    public partial class MainWindow : Window
    {
        int port = 55000;
        private NetworkStream? output; // stream for receiving data           
        private BinaryWriter? writer; // facilitates writing to the stream    
        private BinaryReader? reader; // facilitates reading from the stream  
        private Thread readThread; // Thread for processing incoming messages
        private TcpClient clientSocket;

        public MainWindow()
        {
            InitializeComponent();
            InitializeClient();
        }

        private async void InitializeClient()
        {
            inputTextBox.Visibility = Visibility.Hidden;
            getButton.Visibility = Visibility.Hidden;
            registerButton.Visibility = Visibility.Hidden;

            try
            {
                await RunClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(Environment.ExitCode);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                // Inform the server about the disconnection
                writer?.Write("EXIT");

                // Clean up resources
                writer.Close();
                reader?.Close();
                output?.Close();
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on exit: " + ex.Message);
            }

            System.Environment.Exit(System.Environment.ExitCode);
        }

        public async Task RunClient()
        {
            try
            {
                clientSocket = new TcpClient();
                IPAddress local = IPAddress.Parse("127.0.0.1");
                await clientSocket.ConnectAsync(local, port);

                output = clientSocket.GetStream();
                writer = new BinaryWriter(output);
                reader = new BinaryReader(output);
            } // end try
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordTextBox.Password;

            try
            {
                writer?.Write(username);
                writer?.Write(password);

                bool? response = reader?.ReadBoolean();

                // Process the response (e.g., show login success/failure)
                if ((bool)response)
                {
                    MessageBox.Show("Login successful!");
                    inputTextBox.Visibility = Visibility.Visible;
                    getButton.Visibility = Visibility.Visible;
                    registerButton.Visibility = Visibility.Visible;
                    loginGrid.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RegisterCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetCardButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
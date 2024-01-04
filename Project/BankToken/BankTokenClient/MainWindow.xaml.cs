using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace BankTokenClient
{
    public partial class MainWindow : Window
    {
        private NetworkStream? output; // stream for receiving data           
        private BinaryWriter? writer; // facilitates writing to the stream    
        private BinaryReader? reader; // facilitates reading from the stream  
        private readonly Thread readThread; // Thread for processing incoming messages
        private TcpClient clientSocket;

        public MainWindow()
        {
            InitializeComponent();
            inputTextBox.Visibility = Visibility.Hidden;
            getButton.Visibility = Visibility.Hidden;
            registerButton.Visibility = Visibility.Hidden;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordTextBox.Password;

            TcpClient client = null;
            try
            {
                IPAddress local = IPAddress.Parse("127.0.0.1");
                client = new TcpClient();
                client.Connect(local, 50000);

                output = clientSocket.GetStream();
                writer = new BinaryWriter(output);
                reader = new BinaryReader(output);

                writer.Write(username);
                writer.Write(password);

                string response = reader.ReadString();

                // Process the response (e.g., show login success/failure)
                if (response == "True")
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
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
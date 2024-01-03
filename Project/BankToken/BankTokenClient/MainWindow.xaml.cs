using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace BankTokenClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient clientSocket;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clientSocket = new TcpClient();
                clientSocket.Connect("127.0.0.1", 8888);

                string message = MessageTextBox.Text;
                NetworkStream networkStream = clientSocket.GetStream();
                byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();

                byte[] bytesFrom = new byte[10025];
                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                string dataFromServer = Encoding.ASCII.GetString(bytesFrom);
                LogBox.Text += "Отговор от сървъра: " + dataFromServer + "\n";

                clientSocket.Close();
            }
            catch (Exception ex)
            {
                LogBox.Text += "Грешка: " + ex.Message + "\n";
            }
        }
    }
}
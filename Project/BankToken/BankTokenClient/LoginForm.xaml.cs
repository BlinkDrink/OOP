using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankTokenClient
{
    public partial class LoginForm : UserControl
    {
        public event EventHandler<LoginEventArgs> LoginStatus;
        private TcpClient client;
        private NetworkStream stream;
        private const int port = 55000;

        public LoginForm()
        {
            InitializeComponent();
            client = new TcpClient();
            Task.Run(() => ConnectToServer());
        }

        protected virtual void OnLogin(LoginEventArgs e)
        {
            LoginStatus?.Invoke(this, e);
        }


        private async Task ConnectToServer()
        {
            try
            {
                await client.ConnectAsync("127.0.0.1", port);
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task SendData(string data)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                await stream.WriteAsync(buffer, 0, buffer.Length);
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
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return string.Empty;
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await SendData("USER_AUTHENTICATION|" + UserNameTextBox.Text + "|" + PasswordTextBox.Password);
            string response = await ReceiveResponse();

            if (response == "INVALID_CREDENTIALS")
            {
                MessageBox.Show("Invalid username or password.Try again.\r\n");
            }
            else
            {
                MessageBox.Show("Login successful!");
                if (LoginStatus != null)
                {
                    LoginEventArgs loginEventArgs = new LoginEventArgs(client, stream);
                    OnLogin(loginEventArgs);
                }
            }
        }
    }
}

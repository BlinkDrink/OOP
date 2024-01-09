using System.Net.Sockets;

namespace BankTokenClient
{
    public class LoginEventArgs : EventArgs
    {
        public TcpClient Client { get; init; }

        public NetworkStream Stream { get; init; }

        public LoginEventArgs(TcpClient client, NetworkStream stream)
        {
            Client = client;
            Stream = stream;
        }
    }
}

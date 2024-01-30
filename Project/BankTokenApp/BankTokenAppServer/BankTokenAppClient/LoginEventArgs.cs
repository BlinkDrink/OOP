namespace BankTokenAppClient
{
    public class LoginEventArgs : EventArgs
    {
        public Client ConnectedClient { get; init; }

        public LoginEventArgs(Client client)
        {
            ConnectedClient = client;
        }
    }
}
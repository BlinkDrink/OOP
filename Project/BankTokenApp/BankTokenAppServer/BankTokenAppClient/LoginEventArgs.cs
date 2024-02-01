namespace BankTokenAppClient
{
    /// <summary>
    /// Used to store login event args which is the client that initiated 
    /// the connection to the server
    /// </summary>
    public class LoginEventArgs : EventArgs
    {
        public Client ConnectedClient { get; init; }

        public LoginEventArgs(Client client)
        {
            ConnectedClient = client;
        }
    }
}
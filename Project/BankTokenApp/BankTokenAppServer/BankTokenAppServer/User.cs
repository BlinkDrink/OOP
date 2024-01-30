namespace BankTokenAppServer
{
    [Serializable]
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool CanRegisterToken { get; set; }
        public bool CanRetrieveCardNumber { get; set; }

        public User()
        {

        }

        public User(string username, string password, bool canRegisterToken, bool canRetrieveCardNumber)
        {
            Username = username;
            Password = password;
            CanRegisterToken = canRegisterToken;
            CanRetrieveCardNumber = canRetrieveCardNumber;
        }
    }
}

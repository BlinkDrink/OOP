namespace Homework3
{
    internal class Program
    {

        static void Main(string[] args)
        {
            string cipherText = "abort the mission, you have been spotted";
            char[] cipherTextChars = cipherText.ToCharArray(); // to char array
            string text = new String(cipherTextChars); // to string
            RouteCipher routeCipher = new RouteCipher(-5);
            routeCipher.Encrypt(text);
        }
    }
}
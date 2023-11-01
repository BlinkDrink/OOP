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
            string encrypted = routeCipher.Encrypt(text);
            Console.WriteLine($"The encrypted form of the text is: {encrypted}");

            RouteCipher routeCipher2 = new RouteCipher(5);
            Console.WriteLine($"The decrypted form of the text is: {routeCipher2.Decrpyt("ATSYVNTEDXXTEANITROBHSOESPOEHOMEIUB")}");
        }
    }
}
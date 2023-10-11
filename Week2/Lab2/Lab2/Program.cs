namespace Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestAccount();
            TestTime();
        }

        static void TestAccount()
        {
            Account account = new Account(0.05m, 1000, "1234");
            account.Deposit(1500);
            account.Withdraw(750);
            Console.WriteLine(account);
            account.AnualInterestRate = 0.05m;
            Console.WriteLine(account);
        }

        static void TestTime()
        {
            Time time = new Time();
            Console.WriteLine(time.ToString());
            time.Hour = DateTime.Now.Hour;
            time.Minute = DateTime.Now.Minute;
            time.Second = DateTime.Now.Second;
            Console.WriteLine(time);
        }
    }
}
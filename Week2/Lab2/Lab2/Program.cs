namespace Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestAccount();
            TestTime();
            TestInvoice();

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

        static void TestInvoice()
        {
            Invoice invoice = new Invoice("1234", "Car engine", 7, 300);
            invoice.Quantity = 10;
            Console.WriteLine($"InvoiceAmount for partNumber {invoice.PartNumber} with part description: {invoice.PartDescription} - {invoice.GetInvoiceAmount()}");
            invoice.PricePerItem = 350;
            Console.WriteLine(invoice.GetInvoiceAmount());

        }
    }
}
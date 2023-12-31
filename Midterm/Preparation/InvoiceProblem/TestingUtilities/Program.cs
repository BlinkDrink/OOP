using InvoiceProblem;

namespace TestingUtilities
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const long itemsToCreate = 10;
            List<InvoiceDetail> details = new List<InvoiceDetail>();
            Random r = new Random();
            for (int i = 0; i < itemsToCreate; i++)
            {
                decimal randomValue = (decimal)(r.NextDouble() * 50);
                details.Add(new InvoiceDetail(randomValue));
            }

            List<InvoiceDetail> itemsForIn1 = new List<InvoiceDetail> { details[0] };
            Invoice in1 = new Invoice(itemsForIn1);

            List<InvoiceDetail> itemsForIn2 = new List<InvoiceDetail> { details[1] };
            Invoice in2 = new Invoice(itemsForIn2);

            in1.AddAllInvoice(details);

            List<Invoice> myInvoices = new List<Invoice> { in1, in2 };

            Invoice.PrintInvoices(myInvoices);

            bool areEqual = in1.Equals(in2);
            Console.WriteLine($"Are in1 and in2 equal? {areEqual}");

            Console.WriteLine("in1 description:");
            Console.WriteLine(in1.ToString());

            Console.WriteLine("in2 description:");
            Console.WriteLine(in2.ToString());
        }
    }
}
using InvoiceProblem.Interfaces;
using System.Text;

namespace InvoiceProblem
{
    public class Invoice : IReceivable, IOutgoing
    {
        #region Properties
        private static long INVOICE_NUMBER = 0;

        public long InvoiceNumber { get => INVOICE_NUMBER; }

        private List<InvoiceDetail> invoiceItems;

        public List<InvoiceDetail> InvoiceItems
        {
            get { return invoiceItems; }
            set { invoiceItems = value; }
        }
        #endregion

        #region Constructors
        public Invoice(List<InvoiceDetail> invoiceItems)
        {
            INVOICE_NUMBER++;
            invoiceItems = new List<InvoiceDetail>(invoiceItems);
            this.invoiceItems = invoiceItems;
        }
        #endregion


        #region Methods
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Invoice number: {InvoiceNumber}\n");

            if (invoiceItems != null)
            {
                foreach (var item in invoiceItems)
                {
                    sb.Append(item.ToString() + "\n");
                }
            }

            return sb.ToString();
        }

        public decimal InvoiceTotal()
        {
            decimal total = 0;

            foreach (var item in invoiceItems)
            {
                total += item.DblLineTotal;
            }

            return total;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!(obj is Invoice)) return false;

            return InvoiceTotal() == ((Invoice)obj).InvoiceTotal();
        }

        decimal IReceivable.InvoiceTotal => InvoiceTotal();
        decimal IOutgoing.InvoiceTotal => -InvoiceTotal();

        public static void PrintInvoices(List<Invoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                Console.WriteLine($"INVOICE_NUMBER: {invoice.InvoiceNumber}");

                var sortedItems = invoice.InvoiceItems.OrderByDescending(item => item.DblLineTotal);

                foreach (var item in sortedItems)
                {
                    Console.WriteLine($"DblLineTotal: {item.ToString()}");
                }

                if (invoice is IReceivable)
                {
                    Console.WriteLine($"InvoiceTotal (IReceivable): {((IReceivable)invoice).InvoiceTotal.ToString("C2")}");
                }
                else if (invoice is IOutgoing)
                {
                    Console.WriteLine($"InvoiceTotal (IOutgoing): {((IOutgoing)invoice).InvoiceTotal.ToString("C2")}");
                }

                Console.WriteLine();
            }
        }
        #endregion
    }
}

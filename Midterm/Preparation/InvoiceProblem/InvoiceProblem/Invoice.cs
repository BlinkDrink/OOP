using InvoiceProblem.Interfaces;
using System.Text;

namespace InvoiceProblem
{
    public class Invoice : IReceivable, IOutgoing
    {
        private static long INVOICE_NUMBER = 0;
        InvoiceDetail[] invoiceItems;

        public long InvoiceNumber { get => INVOICE_NUMBER; }

        public Invoice(InvoiceDetail[] invoiceItems)
        {
            INVOICE_NUMBER++;
            this.invoiceItems = invoiceItems;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Invoice number: {InvoiceNumber}");

            if (invoiceItems != null)
            {
                foreach (var item in invoiceItems)
                {
                    sb.Append(item.ToString());
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
    }
}

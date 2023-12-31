namespace InvoiceProblem
{
    public static class InvoiceExtensions
    {
        public static void AddAllInvoice(this Invoice invoice, List<InvoiceDetail> details)
        {
            invoice.InvoiceItems.AddRange(details);
        }
    }
}

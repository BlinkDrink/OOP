namespace InvoiceProblem
{
    public class InvoiceDetail
    {
        private decimal dblLineTotal;

        public decimal DblLineTotal
        {
            get { return dblLineTotal; }
            set { dblLineTotal = value; }
        }


        public InvoiceDetail()
        {

        }

        public override string ToString()
        {
            return $"{dblLineTotal:F2}";
        }
    }
}

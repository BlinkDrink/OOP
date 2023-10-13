namespace Lab2
{
    public class Invoice
    {
        #region Properties
        private string partNumber;

        public string PartNumber
        {
            get { return partNumber; }
            set
            {
                partNumber = value;
            }
        }

        private string partDescription;

        public string PartDescription
        {
            get { return partDescription; }
            set
            {
                partDescription = value;
            }
        }

        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value >= 0 ? value : quantity;
            }
        }

        private decimal pricePerItem;

        public decimal PricePerItem
        {
            get { return pricePerItem; }
            set
            {
                pricePerItem = value >= 0 ? value : quantity;
            }
        }
        #endregion

        #region Constructors

        public Invoice(string partNumber, string partDescription, int quantity, decimal pricePerItem)
        {
            PartNumber = partNumber;
            PartDescription = partDescription;
            if (quantity >= 0)
                Quantity = quantity;
            else
                Console.WriteLine("Cannot have negative quantity");

            if (pricePerItem >= 0)
                PricePerItem = pricePerItem;
            else
                Console.WriteLine("Cannot have negative price per item");
        }
        #endregion

        #region Methods
        public decimal GetInvoiceAmount()
        {
            return quantity * pricePerItem;
        }

        #endregion
    }
}

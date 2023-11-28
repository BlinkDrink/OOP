namespace Homework6.Concretes
{
    public class Product
    {
        #region Properties
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        #endregion

        public Product(string description, int quantity)
        {
            Description = description;
            Quantity = quantity;
        }

        #region Methods
        public override string ToString() { return $"{Description} {Quantity}"; }
        #endregion
    }
}

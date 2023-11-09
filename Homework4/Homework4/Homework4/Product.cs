namespace Homework4
{
    public enum YearlyQuarter
    {
        First = 1,
        Second,
        Third,
        Fourth
    }

    public enum Type
    {
        M = 'M',
        F = 'F'
    }

    public class Product
    {
        #region Properties
        private long cnt;
        private string ID;
        private static Random rnd;
        private List<int> WeeklyPurchases;

        public Type Category { get; init; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public YearlyQuarter Quarter { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// General purpose constructor for initializing description, category, weeklyPurchases and price
        /// </summary>
        /// <param name="description">Initializer of Description</param>
        /// <param name="category">Initializer of Category</param>
        /// <param name="weeklyPurchases">Initializer of WeeklyPurchases</param>
        /// <param name="price">Initializer of Price</param>
        public Product(string description, Type category, List<int> weeklyPurchases, decimal price)
        {
            Quarter = (YearlyQuarter)(rnd.Next(3) + 1);
            Description = description;
            Category = category;
            WeeklyPurchases = weeklyPurchases;
            Price = price;
            ID = $"{Category}{rnd.Next(100001, 999999)}";
        }
        #endregion    

        public override string ToString()
        {
            return $"{Description} {Category} {WeeklyPurchases} {Price}";
        }
    }
}

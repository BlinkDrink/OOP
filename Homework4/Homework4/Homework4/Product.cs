namespace Homework4
{
    public enum YearlyQuarter
    {
        First,
        Second,
        Third,
        Fourth
    }

    public enum Type
    {
        M,
        F
    }

    public class Product
    {
        #region Properties
        private long cnt;
        private string ID;
        private Random rnd;
        private List<int> WeeklyPurchases;

        public Type Category { get; init; }
        #endregion



    }
}

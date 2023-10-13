namespace Homework1
{
    public class TableTest
    {
        /// <summary>
        /// Static void function Test. Used for testing
        /// the implemented functionality of Table.
        /// </summary>
        public void Test()
        {
            double start, end, step;

            Console.WriteLine("Enter left bound of interval: ");
            start = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter right bound of interval: ");
            end = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter the step for iteration over the interval:");
            step = Convert.ToDouble(Console.ReadLine());

            if (start > end)
            {
                Console.WriteLine("IntervalStart is greater than IntervalEnd. Perfoming swap...");
                (start, end) = (end, start);
            }

            Table table = new Table(start, end, step);
            table.MakeTable();
        }
    }
}

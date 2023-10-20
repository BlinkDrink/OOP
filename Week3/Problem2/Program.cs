using System.Collections.Specialized;

namespace Problem2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter x:");
            double x;
            if (!double.TryParse(Console.ReadLine(), out x))
            {
                Console.WriteLine("Wrong input.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine($"Approximate cos({x}): {ComputeCos(x)}");
            Console.WriteLine($"Accurate cos({x}): {Math.Cos(x)}");
        }

        static double ComputeCos(double x)
        {
            // Declaration of vars
            double accuracy = 0.00000001; // accuracy of computation
            double cosValue = 0;
            double term = 1.0;
            int count = 0;

            // processing
            do
            {
                count += 2;
                term = -term * x * x / count * (count - 1);
                cosValue += term;
            } while (Math.Abs(term) > accuracy);

            return cosValue;
        }

    }
}
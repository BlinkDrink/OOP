using VectorImplementation;

namespace TestingVector
{
    class Program
    {
        static void Main()
        {
            // Примери за създаване на обекти от класа Vector
            Vector v1 = new Vector(0, 3);
            v1[0] = 0;
            v1[1] = 1;
            v1[2] = 2;

            Console.WriteLine(Vector.Count); // Извежда броя на активните обекти

            // Пример за използване на обект от класа Vector
            Console.WriteLine(v1[0]); // Достъпване на компонент от вектора

            Vector v1Diff = (Vector)v1.Differentiate();

            Console.WriteLine(v1Diff.Evaluate(2));

        }
    }
}
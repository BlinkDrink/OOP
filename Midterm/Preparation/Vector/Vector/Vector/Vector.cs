using System.Text;

namespace VectorImplementation
{
    public class Vector : Polynomial
    {
        protected static int count = 0;
        protected int[] p;
        protected int i1;
        protected readonly int num;

        public int[] P
        {
            get => p;
            private set
            {
                p = value;
            }
        }

        public int First => i1;

        public int Last => i1 + num - 1;

        public int Length => num;

        public static int Count => count;

        public Vector()
        {
            count++;
            num = 0;
            i1 = 0;
        }

        public Vector(int first_index, int number)
        {
            count++;
            i1 = first_index < 0 ? 0 : first_index;
            num = number;
            p = new int[number + i1];
        }

        public Vector(Vector v) : this(v.i1, v.num)
        {
            count++;
            Array.Copy(v.p, p, num);
        }

        ~Vector()
        {
            count--;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (int i in p)
            {
                sb.Append(i.ToString() + "\n");
            }
            return sb.ToString();
        }

        //public double Evaluate(double x)
        //{
        //    double result = 0;
        //    for (int i = First; i <= Last; i++)
        //    {
        //        result += p[i] * Math.Pow(x, i);
        //    }
        //    return result;
        //}

        //public Polynomial Differentiate()
        //{
        //    Vector derivative = new Vector(i1 - 1, num);
        //    for (int i = 0; i < derivative.Length; i++)
        //    {
        //        derivative[i] = p[i];
        //    }

        //    for (int i = derivative.First; i <= derivative.Last; i++)
        //    {
        //        derivative[i] = i * derivative[i];
        //    }

        //    return derivative;
        //}

        public double Evaluate(double x)
        {
            double result = 0;
            for (int i = First; i <= Last; i++)
            {
                result += p[i - i1] * Math.Pow(x, i);
            }
            return result;
        }

        public Polynomial Differentiate()
        {
            Vector derivative = new Vector(i1 + 1, num);

            for (int i = First + 1; i <= Last; i++)
            {
                derivative.p[i - i1 - 1] = p[i - i1] * i;
            }

            return derivative;
        }

        public int this[int index]
        {
            get
            {
                if (index < i1 || index >= i1 + num)
                {
                    Console.WriteLine("Error: Index out of range");
                    return -1;
                }
                return p[index + i1];
            }
            set
            {
                if (index < i1 || index >= i1 + num)
                {
                    Console.WriteLine("Error: Index out of range");
                    return;
                }
                p[index + i1] = value;
            }
        }

        public static Vector? operator +(Vector vector1, Vector vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new ArgumentException("Error: Cannot add vectors with different lengths");
            }

            Vector result = new Vector(vector1);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] += vector2[i];
            }
            return result;
        }

        public static int operator *(Vector vector1, Vector vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new ArgumentException("Error: Cannot apply dot product to vectors with different lengths");
            }

            int result = 0;
            for (int i = 0; i < vector1.Length; i++)
            {
                result += vector1[i] * vector2[i];
            }
            return result;
        }
    }
}


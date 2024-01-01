using System.Text;

namespace VectorImplementation
{
    public class Vector : Polynomial
    {
        protected static int count = 0;
        protected int[] p;
        protected int i1;
        protected readonly int num;

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
            i1 = first_index;
            num = number;
            p = new int[num];
        }

        public Vector(Vector v)
        {
            count++;
            i1 = v.i1;
            num = v.num;
            p = new int[num];
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
            Vector derivative = new Vector(First - 1, num);
            for (int i = First; i <= Last; i++)
            {
                derivative[i - 1] = this[i] * i;
            }
            return derivative;
        }

        public int this[int index]
        {
            get
            {
                if (index < First || index > Last)
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }
                return p[index - First];
            }
            set
            {
                if (index < First || index > Last)
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }
                p[index - First] = value;
            }
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.num != v2.num || v1.First != v2.i1)
            {
                throw new InvalidOperationException("Vectors are incompatible for addition.");
            }

            Vector result = new Vector(v1.First, v1.num);
            for (int i = v1.First; i <= v1.Last; i++)
            {
                result[i] = v1[i] + v2[i];
            }
            return result;
        }

        public static int operator *(Vector v1, Vector v2)
        {
            if (v1.num != v2.num || v1.i1 != v2.First)
            {
                throw new InvalidOperationException("Vectors are incompatible for multiplication.");
            }

            int result = 0;
            for (int i = v1.First; i < v1.i1 + v1.num; i++)
            {
                result += v1[i] * v2[i];
            }
            return result;
        }
    }
}


using static VectorImplementation.VectorNorms;

namespace VectorImplementation
{
    public static class VectorExtensions
    {
        public static double Norm(this Vector vector, ComputeNorm f)
        {
            return f(vector);
        }
    }
}

using System.Linq;
namespace Dagger.Utils.Math
{
    public static class Distance
    {
        public static decimal CalculateEuclidean(int[] x, int[] y)
        {
            int sum = 0;

            for(int i = 0; i < x.Count(); i++)
            {
                sum += (x[i] - y[i]) * (x[i] * y[i]);
            }

            return (decimal)System.Math.Sqrt(sum);
        }
    }
}
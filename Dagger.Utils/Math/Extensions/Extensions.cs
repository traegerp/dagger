using System.Linq;
using System.Collections.Generic;

namespace Dagger.Utils.Math.Extensions
{
    /// <summary>
    /// Mathematical Operations 
    /// </summary>
    public static class MathExtensions
    {

        public static IEnumerable<int> Normalize(this IEnumerable<int> values)
        {

            int max = values.Max();
            int min = values.Min();

            return values.Select(x => (int)((decimal)(x - min) / (decimal)max - min) * 100);

        }

        public static IEnumerable<int> Normalize(this IEnumerable<int> values, int max, int min)
        {
            return values.Select(x => (int)((decimal)(x - min) / (decimal)max - min) * 100);
        }


        public static IEnumerable<int> Normalize(this IEnumerable<decimal> values, decimal max, decimal min)
        {
            return values.Select(x => (int)((decimal)(x - min) / (decimal)max - min) * 100);
        }

        public static IEnumerable<int> Normalize(this IEnumerable<decimal> values)
        {

            decimal max = values.Max();
            decimal min = values.Min();

            return values.Select(x => (int)((decimal)(x - min) / (decimal)max - min) * 100);
        }

    }
}
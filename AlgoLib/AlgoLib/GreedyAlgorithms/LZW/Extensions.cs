using System.Collections.Generic;

namespace AlgoLib.GreedyAlgorithms
{
    public static class Extensions
    {
        public static string FillWithZero(this string value, int len)
        {
            while (value.Length < len)
            {
                value = "0" + value;
            }

            return value;
        }

        public static string FindKey(this IDictionary<string, int> lookup, int value)
        {
            foreach (var pair in lookup)
            {
                if (pair.Value == value)
                {
                    return pair.Key;
                }
            }

            return null;
        }
    }
}

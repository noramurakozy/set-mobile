using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public static class Utils
    {
        public static readonly Random Rng = new();
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
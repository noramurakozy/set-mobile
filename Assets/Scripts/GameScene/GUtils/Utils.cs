using System;
using System.Collections.Generic;

namespace GameScene.GUtils
{
    public static class Utils
    {
        private static readonly Random Rng = new();
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

        public static string GetTimeSpanString(TimeSpan timeSpan)
        {
            if (timeSpan.Hours == 0)
            {
                return timeSpan.ToString(@"mm\:ss");
            }

            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
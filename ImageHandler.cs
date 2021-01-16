using System;
using System.Collections.Generic;

namespace Memory_Game
{
    internal static class ImageListHandler
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random r = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
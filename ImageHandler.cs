using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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

    internal static class ImageHandler
    {
        public static Image BytesToImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
                return Image.FromStream(ms);
        }

        public static byte[] ImageToBytes(Image image, ImageFormat format = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format ?? image.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
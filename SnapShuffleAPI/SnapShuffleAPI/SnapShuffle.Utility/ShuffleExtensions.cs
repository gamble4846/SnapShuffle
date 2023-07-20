using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.Utility
{
    public static class ShuffleExtensions
    {
        public static int[] GetShuffleExchanges(int size, int key)
        {
            int[] exchanges = new int[size - 1];
            var rand = new Random(key);
            for (int i = size - 1; i > 0; i--)
            {
                int n = rand.Next(i + 1);
                exchanges[size - 1 - i] = n;
            }
            return exchanges;
        }

        public static List<T> Shuffle<T>(List<T> toShuffle, int key)
        {
            int size = toShuffle.Count;
            T[] chars = toShuffle.ToArray();
            var exchanges = GetShuffleExchanges(size, key);
            for (int i = size - 1; i > 0; i--)
            {
                int n = exchanges[size - 1 - i];
                T tmp = chars[i];
                chars[i] = chars[n];
                chars[n] = tmp;
            }
            return chars.ToList();
        }

        public static List<T> DeShuffle<T>(List<T> shuffled, int key)
        {
            int size = shuffled.Count;
            T[] chars = shuffled.ToArray();
            var exchanges = GetShuffleExchanges(size, key);
            for (int i = 1; i < size; i++)
            {
                int n = exchanges[size - i - 1];
                T tmp = chars[i];
                chars[i] = chars[n];
                chars[n] = tmp;
            }
            return chars.ToList();
        }
    }
}

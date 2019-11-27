using System;
using System.Collections.Generic;
using System.Linq;

namespace PU.MissionGen.Core
{
    public static class Extensions
    {
        public static T Choose<T>(this Random rnd, IEnumerable<T> set)
        {
            var array = (set as T[]) ?? set.ToArray();

            if (array.Length < 1)
            {
                throw new ArgumentException("Cannot pick item from empty set.");
            }

            var r = rnd.Next(array.Length);

            return array[r];
        }

        // Adapted from http://stackoverflow.com/a/1262619
        public static IEnumerable<T> Shuffle<T>(this Random rnd, IEnumerable<T> list)
        {
            var output = list.ToArray();
            var n = output.Length;
            while (n > 1)
            {
                n--;
                var k = rnd.Next(n + 1);
                var value = output[k];
                output[k] = output[n];
                output[n] = value;
            }

            return output;
        }
    }
}

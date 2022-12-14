using System;
using System.Collections.Generic;
using System.Linq;

namespace Drinks4Us.Extensions
{
    public static class CollectionExt
    {
        private static readonly Random Random = new();

        public static T GetRandom<T>(this ICollection<T> source)
        {
            var index = Random.Next(source.Count);
            return source.ElementAt(index);
        }
    }
}
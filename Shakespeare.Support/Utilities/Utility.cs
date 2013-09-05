using System;
using System.Collections.Generic;
using System.Linq;

namespace Shakespeare.Support.Utilities
{
    static class Utility
    {
        public static TTarget KeyedFirstOrDefault<TTarget, TKey>(this IEnumerable<TTarget> col, TKey key)
            where TTarget : IKeySearchable<TKey>
            where TKey : IEquatable<TKey>
        {
            return col.FirstOrDefault(item => item.Key.Equals(key));
        }

        public static TTarget KeyedSingle<TTarget, TKey>(this IEnumerable<TTarget> col, TKey key)
            where TTarget : IKeySearchable<TKey>
            where TKey : IEquatable<TKey>
        {
            return col.Single(item => item.Key.Equals(key));
        }
    }
}
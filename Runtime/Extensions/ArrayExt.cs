using System.Collections.Generic;
using System.Linq;

namespace OmiLAXR.Extensions
{
    public static class ArrayExt
    {
        public static bool AreValuesEqual<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            if (ReferenceEquals(source, other)) return true;
            if (source == null || other == null) return false;

            return source.SequenceEqual(other);
        }
    }
}
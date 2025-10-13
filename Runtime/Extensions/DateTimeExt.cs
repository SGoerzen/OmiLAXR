using System;
using System.Linq;

namespace OmiLAXR.Extensions
{
    public static class DateTimeExt 
    {
        public static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;
        public static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;
        public static DateTime Min(params DateTime[] values) => values.Aggregate(Min);
        public static DateTime Max(params DateTime[] values) => values.Aggregate(Max);

        public static DateTime? GetFirstNotNull(params DateTime?[] values)
            => values.FirstOrDefault(v => v.HasValue);
    }
}
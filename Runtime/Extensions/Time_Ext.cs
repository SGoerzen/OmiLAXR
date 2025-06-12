using System;
using System.Text;

namespace OmiLAXR.Extensions
{
    public static class Time_Ext
    {
        /// <summary>
        /// Formats a DateTimeOffset as an xAPI-compliant ISO 8601 UTC timestamp.
        /// </summary>
        public static string ToRfc3339(this DateTimeOffset dto)
        {
            return dto.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
        
        /// <summary>
        /// Formats a DateTime as an xAPI-compliant ISO 8601 UTC timestamp.
        /// </summary>
        public static string ToRfc3339(this DateTime dateTime)
        {
            // Ensure UTC, then format with milliseconds
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
        
        /// <summary>
        /// Converts a TimeSpan to ISO 8601 duration format (e.g., "PT1H2M3S").
        /// </summary>
        public static string ToIso8601(this TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero)
                return "PT0S";

            var sb = new StringBuilder();
            sb.Append("P");

            if (timeSpan.Days > 0)
                sb.Append($"{timeSpan.Days}D");

            if (timeSpan.Hours > 0 || timeSpan.Minutes > 0 || timeSpan.Seconds > 0 || timeSpan.Milliseconds > 0)
            {
                sb.Append("T");

                if (timeSpan.Hours > 0)
                    sb.Append($"{timeSpan.Hours}H");

                if (timeSpan.Minutes > 0)
                    sb.Append($"{timeSpan.Minutes}M");

                if (timeSpan.Seconds > 0 || timeSpan.Milliseconds > 0)
                {
                    var seconds = timeSpan.Seconds + timeSpan.Milliseconds / 1000.0;
                    sb.Append($"{seconds:0.###}S");
                }
            }

            return sb.ToString();
        }
    }
}
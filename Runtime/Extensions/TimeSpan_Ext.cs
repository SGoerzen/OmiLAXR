using System;

namespace OmiLAXR.Extensions
{
    public static class TimeSpan_Ext
    {
        /// <summary>
        /// Formats a TimeSpan object to ISO 8601 duration format.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to format.</param>
        /// <returns>A string representing the duration in ISO 8601 format.</returns>
        public static string ToIso8601(this TimeSpan timeSpan)
        {
            var formattedDuration = "P";
        
            if (timeSpan.Days > 0) formattedDuration += $"{timeSpan.Days}D";

            if (timeSpan.Hours <= 0 && timeSpan.Minutes <= 0 && timeSpan.Seconds <= 0 && timeSpan.Milliseconds <= 0)
                return formattedDuration;
            
            formattedDuration += "T";
            if (timeSpan.Hours > 0) formattedDuration += $"{timeSpan.Hours}H";
            if (timeSpan.Minutes > 0) formattedDuration += $"{timeSpan.Minutes}M";
            
            // Include seconds and milliseconds if they exist
            if (timeSpan.Seconds <= 0 && timeSpan.Milliseconds <= 0) 
                return formattedDuration;
                
            var totalSeconds = timeSpan.Seconds + timeSpan.Milliseconds / 1000.0;
            formattedDuration += $"{totalSeconds:0.###}S";

            return formattedDuration;
        }
    }
}
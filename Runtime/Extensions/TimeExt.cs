/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Text;

namespace OmiLAXR.Extensions
{
    /// <summary>
    /// Extension methods for working with time-related data types.
    /// Provides standardized formatting methods for timestamps and durations
    /// that comply with ISO 8601 standards used in learning analytics.
    /// </summary>
    public static class TimeExt
    {
        /// <summary>
        /// Formats a DateTimeOffset as an ISO 8601 UTC timestamp.
        /// Ensures consistent timestamp formatting across all analytics statements.
        /// </summary>
        /// <param name="dto">The DateTimeOffset to format</param>
        /// <returns>ISO 8601 formatted timestamp string with milliseconds and Z suffix</returns>
        /// <example>Returns: "2023-12-25T10:30:15.123Z"</example>
        public static string ToRfc3339(this DateTimeOffset dto)
        {
            return dto.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
        
        /// <summary>
        /// Formats a DateTime as an ISO 8601 UTC timestamp.
        /// Automatically converts to UTC before formatting to ensure consistency.
        /// </summary>
        /// <param name="dateTime">The DateTime to format</param>
        /// <returns>ISO 8601 formatted UTC timestamp string with milliseconds and Z suffix</returns>
        /// <example>Returns: "2023-12-25T10:30:15.123Z"</example>
        public static string ToRfc3339(this DateTime dateTime)
        {
            // Ensure UTC conversion, then format with milliseconds
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
        
        /// <summary>
        /// Converts a TimeSpan to ISO 8601 duration format.
        /// Formats duration components (days, hours, minutes, seconds) according to ISO 8601 standard.
        /// Used for representing learning session durations and time-based measurements.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to format</param>
        /// <returns>ISO 8601 duration string (e.g., "PT1H2M3S" for 1 hour, 2 minutes, 3 seconds)</returns>
        /// <example>
        /// TimeSpan(1, 2, 3) returns "PT1H2M3S"
        /// TimeSpan(0, 0, 0) returns "PT0S"
        /// TimeSpan(2, 1, 30, 45, 500) returns "P2DT1H30M45.5S"
        /// </example>
        public static string ToIso8601(this TimeSpan timeSpan)
        {
            // Handle zero duration as a special case
            if (timeSpan == TimeSpan.Zero)
                return "PT0S";

            var sb = new StringBuilder();
            sb.Append("P"); // Period designator

            // Add days component if present
            if (timeSpan.Days > 0)
                sb.Append($"{timeSpan.Days}D");

            // Add time components if any are present
            if (timeSpan.Hours > 0 || timeSpan.Minutes > 0 || timeSpan.Seconds > 0 || timeSpan.Milliseconds > 0)
            {
                sb.Append("T"); // Time designator

                // Add hours component
                if (timeSpan.Hours > 0)
                    sb.Append($"{timeSpan.Hours}H");

                // Add minutes component
                if (timeSpan.Minutes > 0)
                    sb.Append($"{timeSpan.Minutes}M");

                // Add seconds component (including fractional seconds from milliseconds)
                if (timeSpan.Seconds > 0 || timeSpan.Milliseconds > 0)
                {
                    var seconds = timeSpan.Seconds + timeSpan.Milliseconds / 1000.0;
                    sb.Append($"{seconds:0.###}S"); // Format with up to 3 decimal places
                }
            }

            return sb.ToString();
        }
    }
}
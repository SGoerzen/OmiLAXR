using System;
using OmiLAXR.Extensions;

namespace OmiLAXR
{
    public struct Duration
    {
        public enum DurationUnit
        {
            Milliseconds,
            Seconds,
            Minutes,
            Hours,
            Days
        }

        public long Value { get; private set; }
        public DurationUnit Unit { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Duration"/> class.
        /// </summary>
        /// <param name="value">The duration value.</param>
        /// <param name="unit">The unit of the duration (e.g., seconds, minutes).</param>
        public Duration(long value, DurationUnit unit)
        {
            Value = value;
            Unit = unit;
        }
        
        /// <summary>
        /// Creates a Duration from a given number of milliseconds.
        /// </summary>
        /// <param name="ms">The duration in milliseconds.</param>
        /// <returns>A new Duration representing the given milliseconds.</returns>
        public static Duration FromMilliseconds(long ms) 
            => new Duration(ms, DurationUnit.Milliseconds);
        
        /// <summary>
        /// Creates a Duration from a given number of seconds.
        /// </summary>
        /// <param name="seconds">The duration in seconds.</param>
        /// <returns>A new Duration representing the given seconds.</returns>
        public static Duration FromSeconds(long seconds) 
            => new Duration(seconds, DurationUnit.Seconds);
        
        /// <summary>
        /// Creates a Duration from a given number of minutes.
        /// </summary>
        /// <param name="minutes">The duration in minutes.</param>
        /// <returns>A new Duration representing the given minutes.</returns>
        public static Duration FromMinutes(long minutes) 
            => new Duration(minutes, DurationUnit.Minutes);
        
        /// <summary>
        /// Creates a Duration from a given number of hours.
        /// </summary>
        /// <param name="hours">The duration in hours.</param>
        /// <returns>A new Duration representing the given hours.</returns>
        public static Duration FromHours(long hours) 
            => new Duration(hours, DurationUnit.Hours);
        
        /// <summary>
        /// Creates a Duration from a given number of days.
        /// </summary>
        /// <param name="days">The duration in days.</param>
        /// <returns>A new Duration representing the given days.</returns>
        public static Duration FromDays(long days) 
            => new Duration(days, DurationUnit.Days);

        /// <summary>
        /// Converts the duration to an ISO 8601 formatted string.
        /// </summary>
        /// <returns>A string representing the duration in ISO 8601 format.</returns>
        public override string ToString()
        {
            var timeSpan = Unit switch
            {
                DurationUnit.Milliseconds => TimeSpan.FromMilliseconds(Value),
                DurationUnit.Seconds => TimeSpan.FromSeconds(Value),
                DurationUnit.Minutes => TimeSpan.FromMinutes(Value),
                DurationUnit.Hours => TimeSpan.FromHours(Value),
                DurationUnit.Days => TimeSpan.FromDays(Value),
                _ => throw new ArgumentOutOfRangeException()
            };

            return timeSpan.ToIso8601();
        }
    }
}
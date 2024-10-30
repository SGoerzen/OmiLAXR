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
        /// Converts the duration to an ISO 8601 formatted string.
        /// </summary>
        /// <returns>A string representing the duration in ISO 8601 format.</returns>
        public override string ToString()
        {
            TimeSpan timeSpan = Unit switch
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
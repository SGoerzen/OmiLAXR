using System;
using OmiLAXR.Extensions;

namespace OmiLAXR
{
    public struct Duration
    {
        public enum DurationUnit
        {
            Millisecond,
            Second,
            Minute,
            Hour,
            Day
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
                DurationUnit.Millisecond => TimeSpan.FromMilliseconds(Value),
                DurationUnit.Second => TimeSpan.FromSeconds(Value),
                DurationUnit.Minute => TimeSpan.FromMinutes(Value),
                DurationUnit.Hour => TimeSpan.FromHours(Value),
                DurationUnit.Day => TimeSpan.FromDays(Value),
                _ => throw new ArgumentOutOfRangeException()
            };

            return timeSpan.ToIso8601();
        }
    }
}
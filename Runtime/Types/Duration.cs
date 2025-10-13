using System;
using OmiLAXR.Extensions;

namespace OmiLAXR.Types
{
    public struct Duration : IComparable<Duration>
    {
        private const double ComparisonTolerance = 0.00000001;

        public enum DurationUnit
        {
            Milliseconds,
            Seconds,
            Minutes,
            Hours,
            Days
        }

        /// <summary>The duration value.</summary>
        public double Value { get; private set; }

        /// <summary>The unit of the duration (e.g., seconds, minutes).</summary>
        public DurationUnit Unit { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Duration"/> class.
        /// </summary>
        /// <param name="value">The duration value.</param>
        /// <param name="unit">The unit of the duration (e.g., seconds, minutes).</param>
        public Duration(double value, DurationUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Creates a Duration from a given number of milliseconds.
        /// </summary>
        /// <param name="ms">The duration in milliseconds.</param>
        /// <returns>A new Duration representing the given milliseconds.</returns>
        public static Duration FromMilliseconds(double ms)
            => new Duration(ms, DurationUnit.Milliseconds);

        /// <summary>
        /// Creates a Duration from a given number of seconds.
        /// </summary>
        /// <param name="seconds">The duration in seconds.</param>
        /// <returns>A new Duration representing the given seconds.</returns>
        public static Duration FromSeconds(double seconds)
            => new Duration(seconds, DurationUnit.Seconds);

        /// <summary>
        /// Creates a Duration from a given number of minutes.
        /// </summary>
        /// <param name="minutes">The duration in minutes.</param>
        /// <returns>A new Duration representing the given minutes.</returns>
        public static Duration FromMinutes(double minutes)
            => new Duration(minutes, DurationUnit.Minutes);

        /// <summary>
        /// Creates a Duration from a given number of hours.
        /// </summary>
        /// <param name="hours">The duration in hours.</param>
        /// <returns>A new Duration representing the given hours.</returns>
        public static Duration FromHours(double hours)
            => new Duration(hours, DurationUnit.Hours);

        /// <summary>
        /// Creates a Duration from a given number of days.
        /// </summary>
        /// <param name="days">The duration in days.</param>
        /// <returns>A new Duration representing the given days.</returns>
        public static Duration FromDays(double days)
            => new Duration(days, DurationUnit.Days);

        /// <summary>
        /// Converts the duration to an ISO 8601 formatted string.
        /// </summary>
        /// <returns>A string representing the duration in ISO 8601 format.</returns>
        public override string ToString() => ToTimeSpan().ToIso8601();

        public TimeSpan ToTimeSpan() 
            => Unit switch
            {
                DurationUnit.Milliseconds => TimeSpan.FromMilliseconds(Value),
                DurationUnit.Seconds => TimeSpan.FromSeconds(Value),
                DurationUnit.Minutes => TimeSpan.FromMinutes(Value),
                DurationUnit.Hours => TimeSpan.FromHours(Value),
                DurationUnit.Days => TimeSpan.FromDays(Value),
                _ => throw new ArgumentOutOfRangeException()
            };
        /// <summary>
        /// Converts the duration to milliseconds (as double) to allow unit-agnostic comparison.
        /// </summary>
        /// <returns>The total duration in milliseconds.</returns>
        public double ToMilliseconds()
            => Unit switch
            {
                DurationUnit.Milliseconds => Value,
                DurationUnit.Seconds => Value * 1000.0,
                DurationUnit.Minutes => Value * 60_000.0,
                DurationUnit.Hours => Value * 3_600_000.0,
                DurationUnit.Days => Value * 86_400_000.0,
                _ => throw new ArgumentOutOfRangeException()
            };

        public double ToSeconds() => ToMilliseconds() / 1000.0;
        public double ToMinutes() => ToMilliseconds() / 60_000.0;
        public double ToHours() => ToMilliseconds() / 3_600_000.0;
        public double ToDays() => ToMilliseconds() / 86_400_000.0;
        public double ToWeeks() => ToMilliseconds() / 604_800_000.0;
        public double ToMonths() => ToMilliseconds() / 2_629_746_000.0;
        public double ToYears() => ToMilliseconds() / 31_557_600_000.0;

        // Comparison operators for Duration vs Duration

        /// <summary>
        /// Checks if one duration is less than another.
        /// </summary>
        public static bool operator <(Duration a, Duration b)
            => a.ToMilliseconds() < b.ToMilliseconds() - ComparisonTolerance;

        /// <summary>
        /// Checks if one duration is less than or equal to another.
        /// </summary>
        public static bool operator <=(Duration a, Duration b)
            => a.ToMilliseconds() <= b.ToMilliseconds() + ComparisonTolerance;

        /// <summary>
        /// Checks if one duration is greater than another.
        /// </summary>
        public static bool operator >(Duration a, Duration b)
            => a.ToMilliseconds() > b.ToMilliseconds() + ComparisonTolerance;

        /// <summary>
        /// Checks if one duration is greater than or equal to another.
        /// </summary>
        public static bool operator >=(Duration a, Duration b)
            => a.ToMilliseconds() >= b.ToMilliseconds() - ComparisonTolerance;

        /// <summary>
        /// Checks if two durations are equal, allowing for floating-point tolerance.
        /// </summary>
        public static bool operator ==(Duration a, Duration b)
            => Math.Abs(a.ToMilliseconds() - b.ToMilliseconds()) < ComparisonTolerance;

        /// <summary>
        /// Checks if two durations are not equal.
        /// </summary>
        public static bool operator !=(Duration a, Duration b)
            => !(a == b);

        /// <summary>
        /// Checks for value equality with another object.
        /// </summary>
        public override bool Equals(object obj)
            => obj is Duration other && this == other;

        /// <summary>
        /// Returns a hash code based on the millisecond representation.
        /// </summary>
        public override int GetHashCode()
            => ToMilliseconds().GetHashCode();

        /// <summary>
        /// Compares this duration with another for sorting purposes.
        /// </summary>
        public int CompareTo(Duration other)
        {
            var diff = ToMilliseconds() - other.ToMilliseconds();
            if (Math.Abs(diff) < ComparisonTolerance) return 0;
            return diff < 0 ? -1 : 1;
        }

        /// <summary>
        /// Explicitly converts the duration to a double (in milliseconds).
        /// </summary>
        /// <param name="d">The duration to convert.</param>
        public static explicit operator double(Duration d)
            => d.ToMilliseconds();

        /// <summary>
        /// Explicitly converts the duration to a float (in milliseconds).
        /// </summary>
        /// <param name="d">The duration to convert.</param>
        public static explicit operator float(Duration d)
            => (float)d.ToMilliseconds();

        /// <summary>
        /// Implicitly creates a duration from a double interpreted as milliseconds.
        /// </summary>
        /// <param name="ms">The millisecond value to convert.</param>
        public static implicit operator Duration(double ms)
            => FromMilliseconds(ms);

        /// <summary>
        /// Implicitly creates a duration from a float interpreted as milliseconds.
        /// </summary>
        /// <param name="ms">The millisecond value to convert.</param>
        public static implicit operator Duration(float ms)
            => FromMilliseconds(ms);
    }
}

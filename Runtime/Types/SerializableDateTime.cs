/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;

namespace OmiLAXR.Types
{
    /// <summary>
    /// Unity-serializable wrapper for DateTime that can be configured in the Inspector.
    /// Provides conversion utilities and implicit operators for seamless DateTime interoperability.
    /// All dates are treated as UTC to ensure consistency across different time zones.
    /// </summary>
    [Serializable]
    public class SerializableDateTime
    {
        /// <summary>Year component (1-9999). Defaults to 2025.</summary>
        public int year = 2025;
        
        /// <summary>Month component (1-12). Defaults to 1 (January).</summary>
        public int month = 1;
        
        /// <summary>Day component (1-31). Defaults to 1.</summary>
        public int day = 1;
        
        /// <summary>Hour component (0-23). Defaults to 0.</summary>
        public int hour;
        
        /// <summary>Minute component (0-59). Defaults to 0.</summary>
        public int minute;
        
        /// <summary>Second component (0-59). Defaults to 0.</summary>
        public int second;
        
        /// <summary>Millisecond component (0-999). Defaults to 0.</summary>
        public int millisecond;

        /// <summary>
        /// Converts this SerializableDateTime to a standard DateTime object.
        /// Always creates UTC DateTime to ensure consistency across time zones.
        /// </summary>
        /// <returns>DateTime representation of this SerializableDateTime in UTC</returns>
        public DateTime ToDateTime() =>
            new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);

        /// <summary>
        /// Updates this SerializableDateTime with values from a standard DateTime.
        /// Extracts all time components and stores them in the individual fields.
        /// </summary>
        /// <param name="dt">DateTime to copy values from</param>
        public void FromDateTime(DateTime dt)
        {
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            hour = dt.Hour;
            minute = dt.Minute;
            second = dt.Second;
            millisecond = dt.Millisecond;
        }
        
        /// <summary>
        /// Creates a new SerializableDateTime representing the current UTC time.
        /// Uses current system time converted to UTC.
        /// </summary>
        public static SerializableDateTime Now => new SerializableDateTime();
        
        /// <summary>
        /// Implicit conversion from SerializableDateTime to DateTime.
        /// Enables seamless use in DateTime contexts without explicit conversion.
        /// </summary>
        /// <param name="sdt">SerializableDateTime to convert</param>
        /// <returns>DateTime representation</returns>
        public static implicit operator DateTime(SerializableDateTime sdt) => sdt.ToDateTime();
        
        /// <summary>
        /// Implicit conversion from DateTime to SerializableDateTime.
        /// Enables automatic wrapping of DateTime objects for serialization.
        /// </summary>
        /// <param name="dt">DateTime to convert</param>
        /// <returns>SerializableDateTime representation</returns>
        public static implicit operator SerializableDateTime(DateTime dt)
        {
            var sdt = new SerializableDateTime();
            sdt.FromDateTime(dt);
            return sdt;
        }
    }
}
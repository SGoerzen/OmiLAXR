/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.ComponentModel;

namespace OmiLAXR.Enums
{
    /// <summary>
    /// Utility class for working with enumeration values and their associated metadata.
    /// Provides helper methods for extracting human-readable descriptions from enum values
    /// using DescriptionAttribute annotations.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Retrieves the description attribute value for an enum value, or the enum name if no description exists.
        /// Uses reflection to access DescriptionAttribute metadata applied to enum members.
        /// </summary>
        /// <param name="value">The enum value to get the description for</param>
        /// <returns>The description string from DescriptionAttribute, or the enum's ToString() if no attribute exists</returns>
        public static string GetEnumDescription(Enum value)
        {
            // Get the field info for this enum value
            var field = value.GetType().GetField(value.ToString());
            
            // Look for a DescriptionAttribute on this field
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            // Return the description if it exists, otherwise return the enum name
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OmiLAXR.Extensions
{
    /// <summary>
    /// Extension methods for working with JSON.NET JToken objects.
    /// Provides utility methods for flattening nested JSON structures into flat key-value dictionaries.
    /// Useful for converting complex JSON data into tabular formats or simplified data structures.
    /// </summary>
    public static class JsonExt
    {
        /// <summary>
        /// Recursively flattens a JSON token into a flat dictionary structure.
        /// Converts nested objects and arrays into a single-level dictionary with composite keys.
        /// Array indices and object property names are combined with underscores to create unique keys.
        /// </summary>
        /// <param name="token">The JSON token to flatten (can be JObject, JArray, or JValue)</param>
        /// <param name="prefix">Optional prefix to prepend to all keys (used for recursion)</param>
        /// <returns>A dictionary with flattened key-value pairs</returns>
        /// <example>
        /// Input: {"user": {"name": "John", "scores": [85, 92]}}
        /// Output: {"user_name": "John", "user_scores_0": 85, "user_scores_1": 92}
        /// </example>
        public static Dictionary<string, object> Flatten(this JToken token, string prefix = "")
        {
            var result = new Dictionary<string, object>();

            // Handle JSON objects by recursively flattening each property
            if (token is JObject obj)
            {
                foreach (var prop in obj.Properties())
                {
                    // Recursively flatten the property value with an extended prefix
                    var nested = Flatten(prop.Value, $"{prefix}{prop.Name}_");
                    foreach (var kvp in nested)
                        result[kvp.Key] = kvp.Value;
                }
            }
            // Handle JSON arrays by flattening each element with index-based keys
            else if (token is JArray array)
            {
                for (var i = 0; i < array.Count; i++)
                {
                    // Recursively flatten each array element with index in the prefix
                    var nested = Flatten(array[i], $"{prefix}{i}_");
                    foreach (var kvp in nested)
                        result[kvp.Key] = kvp.Value;
                }
            }
            // Handle primitive values by adding them directly to the result
            else if (token is JValue val)
            {
                // Remove trailing underscore from the prefix and use as final key
                result[prefix.TrimEnd('_')] = val.Value;
            }

            return result;
        }
    }
}
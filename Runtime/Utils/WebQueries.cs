/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.Collections.Generic;

namespace OmiLAXR.Utils
{
    /// <summary>
    /// Utility class for parsing and managing URL query parameters.
    /// Provides easy access to query string key-value pairs from a given URL.
    /// </summary>
    public class WebQueries
    {
        /// <summary>
        /// Dictionary storing parsed query parameters.
        /// Keys represent parameter names, values represent parameter values.
        /// </summary>
        public Dictionary<string, string> Queries { get; private set; }

        /// <summary>
        /// Constructs a WebQueries instance by parsing query parameters from a URL.
        /// </summary>
        /// <param name="url">The full URL containing query parameters</param>
        public WebQueries(string url)
        {
            // Initialize empty dictionary to store query parameters
            Queries = new Dictionary<string, string>();

            // Create a URI object to safely parse the URL
            var uri = new System.Uri(url);

            // Extract the query string (part after '?')
            var query = uri.Query;

            // Remove leading '?' and split parameters by '&'
            var queryParams = query.TrimStart('?').Split('&');

            // Parse each query parameter
            foreach (var param in queryParams)
            {
                // Split each parameter into key and value
                var keyValue = param.Split('=');

                // Add to dictionary, handling potential index out of range
                if (keyValue.Length == 2)
                {
                    Queries.Add(keyValue[0], keyValue[1]);
                }
            }
        }

        /// <summary>
        /// Retrieves the value of a specific query parameter.
        /// Returns null if the parameter does not exist.
        /// </summary>
        /// <param name="key">The name of the query parameter</param>
        /// <param name="decode">If true, it will do URL unescape.</param>
        /// <returns>The value of the query parameter, or null if not found</returns>
        public string Get(string key, bool decode = false)
        {
            #if UNITY_2021_1_OR_NEWER
            // Use GetValueOrDefault to safely return null if key doesn't exist
            var value = Queries.GetValueOrDefault(key);
            return decode ? System.Web.HttpUtility.UrlDecode(value) : value;
            #else
            // Unity 2020 and earlier don't have GetValueOrDefault method
            Queries.TryGetValue(key, out var value);
            return decode ? System.Web.HttpUtility.UrlDecode(value) : value;           
            #endif
        }

        /// <summary>
        /// Sets or updates a query parameter value.
        /// </summary>
        /// <param name="key">The name of the query parameter</param>
        /// <param name="value">The value to set for the parameter</param>
        /// <param name="encode">If true, the value will be escaped for URL.</param>
        public void Set(string key, string value, bool encode = false)
        {
            // Add or update the parameter in the dictionary
            Queries[key] = encode ? System.Web.HttpUtility.UrlEncode(value) : value;
        }
    }
}
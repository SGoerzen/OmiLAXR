/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using OmiLAXR.Utils;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Abstract base class for web-based endpoints.
    /// Provides utility methods for web-related operations in Unity.
    /// </summary>
    public abstract class WebEndpoint : Endpoint
    {
        /// <summary>
        /// Retrieves the absolute URL of the current application.
        /// Useful for web-based context and configuration.
        /// </summary>
        /// <returns>The full URL of the current application</returns>
        public static string GetAbsoluteUrl() => Application.absoluteURL;

        /// <summary>
        /// Lazy-initialized web queries utility.
        /// Creates a new WebQueries instance if not already existing.
        /// </summary>
        private WebQueries _queries;

        /// <summary>
        /// Provides access to web-related query utilities.
        /// Initializes with the current application's absolute URL.
        /// </summary>
        public WebQueries Queries => _queries ??= new WebQueries(GetAbsoluteUrl());
    }
}
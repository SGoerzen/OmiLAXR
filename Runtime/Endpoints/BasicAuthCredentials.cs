/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Represents credentials for Basic Authentication.
    /// Stores endpoint, username, and password for authentication purposes.
    /// </summary>
    [System.Serializable]
    public struct BasicAuthCredentials
    {
        /// <summary>
        /// The base URL or endpoint for the authentication service.
        /// </summary>
        public string endpoint;

        /// <summary>
        /// The username for authentication (alternatively called key).
        /// </summary>
        [Tooltip("Alternatively called key.")]
        public string username;

        /// <summary>
        /// The password for authentication (alternatively called secret).
        /// </summary>
        [Tooltip("Alternatively called secret.")]
        public string password;

        /// <summary>
        /// Constructs a new BasicAuthCredentials instance with the specified parameters.
        /// </summary>
        /// <param name="endpoint">The authentication endpoint URL</param>
        /// <param name="username">The authentication username</param>
        /// <param name="password">The authentication password</param>
        public BasicAuthCredentials(string endpoint, string username, string password)
        {
            this.endpoint = endpoint;
            this.username = username;
            this.password = password;
        }
        
        /// <summary>
        /// Checks if the credentials are valid by ensuring none of the fields are null or empty.
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(endpoint) 
                               && !string.IsNullOrEmpty(username) 
                               && !string.IsNullOrEmpty(password);

        /// <summary>
        /// Provides a string representation of the credentials for logging or debugging.
        /// Masks the actual password for security.
        /// </summary>
        /// <returns>A formatted string with credential information</returns>
        public override string ToString()
        {
            return $"[BasicAuthCredentials endpoint={endpoint}, username={username}, password={password}]";
        }
    }
}
/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Represents configuration for Bearer token authentication.
    /// Stores endpoint URL and authentication token.
    /// </summary>
    [Serializable]
    public struct BearerAuth
    {
        // Stores the endpoint URL for the authentication service
        public string endpoint;

        // Stores the authentication bearer token
        public string token;

        /// <summary>
        /// Initializes a new instance of BearerConfig with the specified endpoint and token.
        /// </summary>
        /// <param name="endpoint">The URL of the authentication endpoint</param>
        /// <param name="token">The bearer authentication token</param>
        public BearerAuth(string endpoint, string token)
        {
            this.endpoint = endpoint;
            this.token = token;
        }
        public override string ToString()
        {
            return $"[BearerAuth endpoint={endpoint}, token={token}]";
        }
    }
}
/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Loads authentication credentials from a JSON file in the StreamingAssets folder.
    /// Automatically configures a BasicAuthEndpoint with loaded credentials.
    /// </summary>
    [RequireComponent(typeof(BasicAuthEndpoint))]
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Basic Auth Credentials Loader")]
    [DefaultExecutionOrder(-1001)] // Ensure this runs early in the initialization process
    [Description(
        "Loads bearer token authentication configuration using the following strategy:\n\n" +
        "▶ WebGL builds:\n" +
        "  1. If the URL contains '?${urlQuery}=https://...' → load JSON config from that remote URL.\n" +
        "  2. Otherwise, expect '?endpoint=...', '?username=...', and '?password=...' parameters in the URL query.\n\n" +
        "▶ Non-WebGL builds:\n" +
        "  1. From command line arguments: -endpoint=..., -token=...\n" +
        "  2. From bearer.json next to the executable.\n" +
        "  3. From bearer.json in Application.persistentDataPath.\n" +
        "  4. From bearer.json in Application.streamingAssetsPath.\n\n" +
        "The first valid source in this order is used to configure the BearerAuthEndpoint."
    )]
    public class BasicAuthCredentialsLoader : AuthLoader<BasicAuthEndpoint, BasicAuthCredentials>
    {
        /// <summary>
        /// Provides the default filename (including extension) used when searching for
        /// Basic authentication credentials on disk or remote sources.
        /// </summary>
        protected override string DefaultAuthFileName => "credentials.json";

        /// <summary>
        /// Attempts to construct a BasicAuthCredentials configuration object from key/value pairs.
        /// Expects the following keys to be present:
        /// - "endpoint": the base API endpoint to authenticate against
        /// - "username": the username used for HTTP Basic authentication
        /// - "password": the password used for HTTP Basic authentication
        /// </summary>
        /// <param name="values">Key/value store sourced from CLI, URL query, or JSON file.</param>
        /// <param name="config">Output configuration populated on success; default on failure.</param>
        /// <returns>True if all required keys are present and a config was created; otherwise false.</returns>
        protected override bool TryBuildConfig(IDictionary<string, string> values, out BasicAuthCredentials config)
        {
            if (!values.ContainsKey("endpoint") || !values.ContainsKey("username") || !values.ContainsKey("password"))
            {
                config = default;
                return false;
            }
            config = new BasicAuthCredentials()
            {
                endpoint = values["endpoint"],
                username = values["username"],
                password = values["password"]
            };
            return true;
        }

        /// <summary>
        /// Applies the validated Basic authentication configuration to the target endpoint.
        /// Delegates to the endpoint's SetAuthConfig method to store and activate credentials.
        /// </summary>
        /// <param name="endpoint">The BasicAuthEndpoint instance to configure.</param>
        /// <param name="config">The parsed and validated BasicAuthCredentials.</param>
        protected override void ApplyConfig(BasicAuthEndpoint endpoint, BasicAuthCredentials config)
        {
            endpoint.SetAuthConfig(config);
        }
    }
}
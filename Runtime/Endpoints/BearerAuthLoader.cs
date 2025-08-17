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
    /// Loads Bearer token authentication configuration from a JSON file in StreamingAssets.
    /// Automatically configures a BearerEndpoint with loaded credentials.
    /// </summary>
    /// <summary>
    /// Loads Bearer token authentication configuration from a JSON file.
    /// 
    /// 
    /// Loading priority:
    /// - WebGL builds:
    ///   1. If URL contains "?bearer=https://..." (or analogous key), 
    ///      load the JSON config from that remote URL.
    ///   2. Otherwise expect "?endpoint=..." and "?token=..." in the URL.
    /// 
    /// - Non-WebGL builds:
    ///   1. Command line arguments (-endpoint=..., -token=...).
    ///   2. JSON file next to the executable (AppDomain.CurrentDomain.BaseDirectory).
    ///   3. JSON file in Application.persistentDataPath.
    ///   4. JSON file in Application.streamingAssetsPath.
    /// 
    /// Subclasses implement ApplyConfig(TC, TE) to actually configure the endpoint.
    /// </summary>
    [RequireComponent(typeof(BearerAuthEndpoint))]
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Bearer Auth Loader")]
    [DefaultExecutionOrder(-1001)] // Ensure early initialization in the component lifecycle
    [Description(
        "Loads bearer token authentication configuration using the following strategy:\n\n" +
        "▶ WebGL builds:\n" +
        "  1. If the URL contains '?${urlQuery}=https://...' → load JSON config from that remote URL.\n" +
        "  2. Otherwise, expect '?endpoint=...' and '?token=...' parameters in the URL query.\n\n" +
        "▶ Non-WebGL builds:\n" +
        "  1. From command line arguments: -endpoint=..., -token=...\n" +
        "  2. From bearer.json next to the executable.\n" +
        "  3. From bearer.json in Application.persistentDataPath.\n" +
        "  4. From bearer.json in Application.streamingAssetsPath.\n\n" +
        "The first valid source in this order is used to configure the BearerAuthEndpoint."
    )]
    public class BearerAuthLoader : AuthLoader<BearerAuthEndpoint, BearerAuth>
    {
        /// <summary>
        /// Provides the default filename (including extension) used to discover
        /// bearer token configuration when loading from disk or remote sources.
        /// </summary>
        protected override string DefaultAuthFileName => "bearer.json";
        /// <summary>
        /// Attempts to create a BearerAuth configuration from key/value pairs.
        /// Expects the following keys:
        /// - "endpoint": the API base URL or service endpoint
        /// - "token": the bearer token string to be used for authorization
        /// Returns true when both keys are present and a BearerAuth instance was created.
        /// </summary>
        /// <param name="values">Source dictionary (from CLI args, URL query, or JSON).</param>
        /// <param name="config">Output BearerAuth configuration when successful; default otherwise.</param>
        /// <returns>True on success; false when required keys are missing.</returns>
        protected override bool TryBuildConfig(IDictionary<string, string> values, out BearerAuth config)
        {
            if (!values.ContainsKey("endpoint") || !values.ContainsKey("token"))
            {
                config = default;
                return false;
            }
            config = new BearerAuth(values["endpoint"], values["token"]);
            return true;
        }
        /// <summary>
        /// Applies the parsed bearer token configuration to the target endpoint.
        /// Delegates to the endpoint to store and activate the provided credentials.
        /// </summary>
        /// <param name="endpoint">The BearerAuthEndpoint to configure.</param>
        /// <param name="config">The validated BearerAuth configuration.</param>
        protected override void ApplyConfig(BearerAuthEndpoint endpoint, BearerAuth config)
        {
            endpoint.SetAuthConfig(config);
        }
    }
}
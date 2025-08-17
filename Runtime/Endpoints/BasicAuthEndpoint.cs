/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using System;
using System.Text;
using OmiLAXR.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Abstract base class for endpoints using Basic Authentication.
    /// Provides credential management and validation for sending statements.
    /// </summary>
    public abstract class BasicAuthEndpoint : Endpoint
    {
        /// <summary>
        /// Generates a basic authentication header string from the provided credentials.
        /// </summary>
        /// <param name="username">The username to be used for authentication</param>
        /// <param name="password">The password to be used for authentication</param>
        /// <returns>A basic authentication header string</returns>
        public static string GetAuth(string username, string password)
            => "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        /// <summary>
        /// Stores the basic authentication credentials.
        /// Uses [FormerlySerializedAs] to maintain compatibility with previous serialization.
        /// Defaults to a preset LRS endpoint if no credentials are provided.
        /// </summary>
        [FormerlySerializedAs("_credentials")] 
        [SerializeField]
        private BasicAuthCredentials credentials = DefaultCredentials;
        
        public static BasicAuthCredentials DefaultCredentials => new BasicAuthCredentials(
            "https://lrs.elearn.rwth-aachen.de/data/xAPI", // Default LRS endpoint
            "", // Default empty username 
            "" // Default empty password
        );
        
        /// <summary>
        /// Overrides the base HandleQueue method to first validate credentials.
        /// Prevents statement sending if credentials are invalid.
        /// </summary>
        /// <returns>
        /// TransferCode.InvalidCredentials if credentials are not valid,
        /// otherwise delegates to base class implementation
        /// </returns>
        protected override TransferCode HandleQueue()
        {
            // Check if credentials are valid before processing queue
            return !credentials.IsValid ? TransferCode.InvalidCredentials : base.HandleQueue();
        }

        /// <summary>
        /// Provides get and set access to the authentication credentials.
        /// Allows dynamic configuration of endpoint credentials.
        /// </summary>
        public BasicAuthCredentials Credentials => credentials;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public void SetAuthConfig(BasicAuthCredentials c)
        {
            credentials = c;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void SetAuthConfig(string endpoint, string username, string password)
        {
            credentials = new BasicAuthCredentials(endpoint, username, password);
        }

        public override void ConsumeDataMap(DataMap map)
        {
            credentials.endpoint = map["endpoint"] as string;
            credentials.username = map["username"] as string;
            credentials.password = map["password"] as string;
        }

        public override DataMap ProvideDataMap()
        {
            return new DataMap()
            {
                ["endpoint"] = credentials.endpoint,
                ["username"] = credentials.username,
                ["password"] = credentials.password
            };
        }
    }
}
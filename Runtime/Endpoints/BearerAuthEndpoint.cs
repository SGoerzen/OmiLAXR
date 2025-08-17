/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using OmiLAXR.Composers;
using Newtonsoft.Json;
using OmiLAXR.Utils;
using UnityEngine.Serialization;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Endpoint implementation for sending xAPI statements using Bearer token authentication.
    /// Supports sending individual and batch statements via HTTP POST.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 6) Endpoints/ Bearer Endpoint")]
    [Description("Sends xAPI statements using Bearer token authentication to a http endpoint.")]
    public class BearerAuthEndpoint : AsyncEndpoint
    {
        /// <summary>
        /// Bearer authentication configuration (endpoint URL and token).
        /// </summary>
        [FormerlySerializedAs("authData")] [SerializeField]
        private BearerAuth auth = DefaultAuth;
        
        public static BearerAuth DefaultAuth => new BearerAuth("https://lrs.elearn.rwth-aachen.de/data/xAPI", "");

        /// <summary>
        /// Sends a single xAPI statement asynchronously using UnityWebRequest.
        /// </summary>
        protected override async Task HandleSendingAsync(IStatement statement)
        {
            using (var request = new UnityWebRequest(auth.endpoint, UnityWebRequest.kHttpVerbPOST))
            {
                var json = JsonConvert.SerializeObject(statement.ToJsonString());
                var bodyRaw = Encoding.UTF8.GetBytes(json);

                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + auth.token);

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

#if UNITY_2020_1_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
#else
                if (request.isHttpError || request.isNetworkError)
#endif
                {
                    var errorMessage = request.error;
                    var resultCode = request.result.ToString();
                    Dispatch(() => { DebugLog.OmiLAXR.Error($"Request failed: {resultCode} {errorMessage}"); });
                }
                else
                {
                    Dispatch(() => TriggerSentStatement(statement));
                }
            }
        }

        /// <summary>
        /// Sends a batch of statements as a single JSON array.
        /// UnityWebRequest must be created on the main thread.
        /// </summary>
        protected override async Task HandleSendingBatchAsync(List<IStatement> batch)
        {
            // create request directly (no MainThreadAsync)
            var jsonArray = "[" + string.Join(",", batch.Select(b => b.ToJsonString())) + "]";
            var bodyRaw = Encoding.UTF8.GetBytes(jsonArray);

            using (var request = new UnityWebRequest(auth.endpoint, UnityWebRequest.kHttpVerbPOST))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + auth.token);

                var op = request.SendWebRequest();
                while (!op.isDone) await Task.Yield();
                
#if UNITY_2020_1_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
#else
                if (request.isHttpError || request.isNetworkError)
#endif
                {
                    var errorMessage = request.error;
                    var resultCode = request.result.ToString();
                    Dispatch(() => { DebugLog.OmiLAXR.Error($"Batch send failed: {resultCode} {errorMessage}"); });
                }
                else
                {
                    // let the base/endpoint trigger success (batch or per-item, not both)
                    Dispatch(() =>
                    {
                        foreach (var s in batch)
                            TriggerSentStatement(s);
                        TriggerSentBatch(batch);
                    });
                }
            }

            
        }

        /// <summary>
        /// Configures this endpoint with a new URL and token at runtime.
        /// </summary>
        public void SetAuthConfig(string endpoint, string token)
        {
            auth = new BearerAuth(endpoint, token);
        }
        public void SetAuthConfig(BearerAuth a)
        {
            auth = a;
        }
        
        public override void ConsumeDataMap(DataMap map)
        {
            auth.endpoint = map["endpoint"] as string;
            auth.token = map["token"] as string;
        }

        public override DataMap ProvideDataMap()
        {
            return new DataMap()
            {
                ["endpoint"] = auth.endpoint,
                ["token"] = auth.token,
            };
        }
    }
}
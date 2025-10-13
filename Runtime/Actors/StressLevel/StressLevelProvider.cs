/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using System.Collections.Generic;
using OmiLAXR.Actors.StressLevel;
using UnityEngine;

namespace OmiLAXR.StressLevel
{
    /// <summary>
    /// Abstract base class for components that manage stress level data providers.
    /// Maintains a collection of data sources and provides unified access to calculated stress levels.
    /// </summary>
    public abstract class StressLevelProvider : ActorDataProvider
    {
        /// <summary>
        /// Current computed stress level value (0-1 range)
        /// </summary>
        [ReadOnly, SerializeField]
        private float stressLevel;
        
        /// <summary>
        /// Collection of registered stress data providers that contribute to the final calculation
        /// </summary>
        public List<IStressLevelDataProvider> providers = new List<IStressLevelDataProvider>();

        /// <summary>
        /// Adds a stress data provider to the collection if not already present
        /// </summary>
        /// <param name="provider">Provider to register</param>
        public void RegisterProvider(IStressLevelDataProvider provider)
        {
            if (!providers.Contains(provider)) providers.Add(provider);
        }
        
        /// <summary>
        /// Removes a stress data provider from the collection
        /// </summary>
        /// <param name="provider">Provider to unregister</param>
        public void UnregisterProvider(IStressLevelDataProvider provider)
        {
            if (providers.Contains(provider)) providers.Remove(provider);
        }
        
        /// <summary>
        /// Clears all registered stress data providers
        /// </summary>
        public void UnregisterAllProviders() => providers.Clear();
        
        /// <summary>
        /// Indicates if the provider has any registered data sources
        /// </summary>
        public bool IsActive => providers.Count > 0;

        /// <summary>
        /// Gets the current calculated stress level
        /// </summary>
        /// <returns>Stress level value between 0 (relaxed) and 1 (high stress)</returns>
        public float GetStressLevel() => stressLevel;
        
        /// <summary>
        /// Sets the current stress level value
        /// </summary>
        /// <param name="level">New stress level to store</param>
        public void SetStressLevel(float level) => stressLevel = level;
    }
}
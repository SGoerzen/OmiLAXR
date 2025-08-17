/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.Collections.Generic;
using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
using OmiLAXR.Hooks;
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Generic extension system for DataProvider components that allows modular enhancement
    /// of data provider functionality without modifying the core implementation.
    /// Automatically discovers and registers child components (Composers, Hooks, Endpoints)
    /// with the target DataProvider during initialization.
    /// </summary>
    /// <typeparam name="T">Type of DataProvider this extension targets</typeparam>
    [DefaultExecutionOrder(-1)] // Execute early to ensure extensions are applied before other components
    public abstract class DataProviderExtension<T> : PipelineComponent, IDataProviderExtension
        where T : DataProvider
    {
        /// <summary>
        /// Collection of Composer components found in child objects.
        /// Composers handle data formatting and transformation for analytics output.
        /// </summary>
        public readonly List<IComposer> Composers = new List<IComposer>();
        
        /// <summary>
        /// Collection of Hook components found in child objects.
        /// Hooks provide event interception and custom processing capabilities.
        /// </summary>
        public readonly List<Hook> Hooks = new List<Hook>();
        
        /// <summary>
        /// Collection of Endpoint components found in child objects.
        /// Endpoints define where and how analytics data should be sent.
        /// </summary>
        public readonly List<Endpoint> Endpoints = new List<Endpoint>();
        
        /// <summary>
        /// Reference to the specific DataProvider instance this extension is attached to.
        /// Strongly typed to the generic parameter for type-safe access.
        /// </summary>
        public T DataProvider { get; protected set; }
        
        /// <summary>
        /// Gets the DataProvider as the base DataProvider type for interface compliance.
        /// Required by IDataProviderExtension interface.
        /// </summary>
        /// <returns>The DataProvider instance cast to base type</returns>
        public DataProvider GetDataProvider() => DataProvider;

        /// <summary>
        /// Unity Awake callback that automatically discovers and extends the target DataProvider.
        /// Searches for the DataProvider and initiates the extension process.
        /// </summary>
        private void Awake()
        {
            // Find the target DataProvider instance in the scene
            var pipeline = FindObject<T>();
            Extend(pipeline);
        }

        /// <summary>
        /// Extends the specified DataProvider with components found in this extension's hierarchy.
        /// Automatically discovers and registers all child Composers, Hooks, and Endpoints.
        /// </summary>
        /// <param name="dataProvider">The DataProvider instance to extend</param>
        public void Extend(DataProvider dataProvider)
        {
            // Cache the strongly-typed reference
            DataProvider = (T)dataProvider;

            // Discover all extension components in child objects
            var composers = gameObject.GetComponentsInChildren<IComposer>();
            var hooks = gameObject.GetComponentsInChildren<Hook>();
            var endpoints = gameObject.GetComponentsInChildren<Endpoint>();
            
            // Store references locally for management
            Composers.AddRange(composers);
            Hooks.AddRange(hooks);
            Endpoints.AddRange(endpoints);
            
            // Register components with the target DataProvider
            DataProvider.Composers.AddRange(composers);
            DataProvider.Hooks.AddRange(hooks);
            DataProvider.Endpoints.AddRange(endpoints);
            DataProvider.Extensions.Add(this);

            // Log successful extension for debugging
            DebugLog.OmiLAXR.Print("Extended data provider " + typeof(T));
        }
    }
}
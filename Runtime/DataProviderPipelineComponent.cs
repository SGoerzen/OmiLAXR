/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR
{
    /// <summary>
    /// Abstract base class for pipeline components that require access to a DataProvider.
    /// Automatically locates and caches the associated DataProvider component during initialization.
    /// Provides a standardized way for pipeline components to access data provider functionality.
    /// </summary>
    public abstract class DataProviderPipelineComponent : PipelineComponent
    {
        /// <summary>
        /// Reference to the associated DataProvider component found in the parent hierarchy.
        /// Cached after the first access to avoid repeated component searches.
        /// </summary>
        protected DataProvider DataProvider { get; private set; }
        
        /// <summary>
        /// Initializes the component by locating the DataProvider in the parent hierarchy.
        /// Uses Unity version-specific methods to ensure compatibility across different Unity versions.
        /// </summary>
        protected override void OnEnable()
        {
            // Use version-appropriate method for finding DataProvider in parent hierarchy
            #if UNITY_2019
            // Unity 2019 doesn't support includeInactive parameter
            DataProvider = GetComponentInParent<DataProvider>();
            #else 
            // Unity 2020+ supports includeInactive parameter for better component discovery
            DataProvider = GetComponentInParent<DataProvider>(true);
            #endif
        }
    }
}
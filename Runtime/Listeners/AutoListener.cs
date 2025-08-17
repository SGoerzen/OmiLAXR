/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using UnityEngine;

namespace OmiLAXR.Listeners
{
    /// <summary>
    /// Generic listener that automatically discovers and provides objects of a specific type to the pipeline.
    /// Serves as a base class for creating listeners that need to find all instances of a particular
    /// Unity Object type in the scene and make them available for analytics tracking.
    /// Simplifies the creation of type-specific listeners by handling the detection logic automatically.
    /// </summary>
    /// <typeparam name="T">The Unity Object type to automatically detect and listen for</typeparam>
    public class AutoListener<T> : Listener where T : Object
    {
        /// <summary>
        /// Initiates automatic detection of all objects of type T in the scene.
        /// Called when the listener starts and immediately searches for all instances
        /// of the specified type, making them available to the pipeline for tracking.
        /// </summary>
        public override void StartListening()
        {
            // Use the generic Detect method to find all objects of type T
            Detect<T>();
        }
    }
}
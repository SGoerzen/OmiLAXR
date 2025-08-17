/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Components;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    /// <summary>
    /// Specialized listener for detecting and providing TransformWatcher components to the pipeline.
    /// TransformWatcher components monitor position, rotation, and scale changes of GameObjects,
    /// making this listener essential for tracking object movement and spatial analytics.
    /// Used in scenarios involving object manipulation, physics interactions, or spatial learning.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 1) Listeners / <TransformWatcher> Objects Listener")]
    [Description("Provides all <TransformWatcher> components to pipeline.")]
    public class TransformWatcherListener : Listener
    {
        /// <summary>
        /// Initiates detection of all TransformWatcher components in the scene.
        /// Finds all existing TransformWatcher components and reports them to the pipeline
        /// for spatial tracking and movement analytics.
        /// </summary>
        public override void StartListening()
        {
            // Find all TransformWatcher components and report them to the pipeline
            Found(FindObjects<TransformWatcher>());
        }
    }
}
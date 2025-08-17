/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Components;
using UnityEngine;

namespace OmiLAXR.Listeners.Learner
{
    /// <summary>
    /// Specialized listener that monitors the main camera's transform for position, rotation, and scale changes.
    /// Automatically adds a TransformWatcher component to Camera.main if it doesn't already exist,
    /// then configures it with specified thresholds for movement detection. Essential for tracking
    /// player movement, camera behavior, and spatial navigation in first-person or camera-based experiences.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 1) Listeners / Main <Camera> Transform Listener")]
    [Description("Provides <TransformWatcher> of Camera.main if it exists and add the component if not.")]
    public class MainCameraTransformListener : Listener
    {
        /// <summary>
        /// Minimum position change (in world units) required to trigger position change events.
        /// Lower values increase sensitivity to small movements, higher values reduce noise
        /// from minor camera adjustments. Typical values range from 0.1 (sensitive) to 1.0 (less sensitive).
        /// </summary>
        [Tooltip("Minimum position change (in units) required to trigger events")]
        public float positionThreshold = .5f;
        
        /// <summary>
        /// Minimum rotation change (in degrees) required to trigger rotation change events.
        /// Lower values capture subtle head movements or camera adjustments, higher values
        /// focus on significant directional changes. Typical values range from 0.5 to 5.0 degrees.
        /// </summary>
        [Tooltip("Minimum rotation change (in degrees) required to trigger events")]
        public float rotationThreshold = 1.0f;
        
        /// <summary>
        /// Minimum scale change required to trigger scale change events.
        /// Rarely used for cameras but available for scenarios involving camera zoom
        /// or field-of-view changes represented as scale. Typical values range from 0.01 to 0.1.
        /// </summary>
        [Tooltip("Minimum scale change required to trigger events")]
        public float scaleThreshold = 0.1f;
        
        /// <summary>
        /// Initiates monitoring of the main camera's transform.
        /// Checks for the existence of Camera.main, adds or retrieves a TransformWatcher component,
        /// configures the sensitivity thresholds, and reports it to the pipeline for tracking.
        /// </summary>
        public override void StartListening()
        {
            // Exit early if no main camera exists in the scene
            if (!Camera.main)
                return;
                
            // Get the main camera's GameObject
            var go = Camera.main.gameObject;
            
            // Get existing TransformWatcher or add a new one
            var tw = go.GetComponent<TransformWatcher>() ?? go.AddComponent<TransformWatcher>();
            
            // Configure the TransformWatcher with our specified thresholds
            tw.positionThreshold = positionThreshold;
            tw.rotationThreshold = rotationThreshold;
            tw.scaleThreshold = scaleThreshold;
            
            // Report the configured TransformWatcher to the pipeline
            Found(tw);
        }
    }
}
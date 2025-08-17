/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Components;
using UnityEngine;
using UnityEngine.UI;

namespace OmiLAXR.Listeners
{
    /// <summary>
    /// Specialized listener for Unity UI Selectable components (buttons, toggles, sliders, etc.).
    /// Automatically detects UI elements and optionally adds interaction tracking components.
    /// Provides comprehensive support for tracking user interactions with UI elements,
    /// including clicks, selections, and other user interface events.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 1) Listeners / <Selectable> Objects Listener")]
    [Description("Provides all <Selectable> components to pipeline.")]
    public class SelectableObjectsListener : Listener
    {
        /// <summary>
        /// Whether to include inactive/disabled selectable objects in the detection.
        /// When true, finds selectables even if their GameObjects are currently inactive.
        /// Useful for tracking all potential UI interactions, not just currently active ones.
        /// </summary>
        public bool includeInactive = true;
        
        /// <summary>
        /// Whether to automatically add InteractionEventHandler components to detected selectables.
        /// When true, ensures each selectable has the necessary tracking component for analytics.
        /// Automatically enhances UI elements with interaction tracking capabilities.
        /// </summary>
        public bool addInteractionEventHandler = true;

        /// <summary>
        /// Initiates detection of all Selectable UI components in the scene.
        /// Optionally adds InteractionEventHandler components for comprehensive interaction tracking.
        /// Provides flexibility in handling both active and inactive UI elements.
        /// </summary>
        public override void StartListening()
        {
            // Find all selectable components, respecting the includeInactive setting
            var selectables = FindObjects<Selectable>(includeInactive);
            
            // Optionally add interaction event handlers for tracking
            if (addInteractionEventHandler)
            {
                foreach (var selectable in selectables)
                {
                    // Only add the handler if it doesn't already exist
                    if (!selectable.GetComponent<InteractionEventHandler>())
                        selectable.gameObject.AddComponent<InteractionEventHandler>();
                }
            }
            
            // Report the found selectables to the pipeline
            Found(selectables);
        }
    }
}
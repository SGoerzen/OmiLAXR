/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace OmiLAXR.Filters
{
    /// <summary>
    /// Filter component that excludes Unity system components and OmiLAXR framework components
    /// from analytics tracking. Prevents system-level components like UI elements, lighting,
    /// and pipeline components from being tracked as learning interactions.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 2) Filters / Unity Components Filter")]
    [Description("Filters out system relevant components that do need to be tracked.")]
    public class UnityComponentsFilter : Filter
    {
        /// <summary>
        /// Array of component types that should be excluded from tracking.
        /// Includes OmiLAXR pipeline components, Unity UI components, lighting, and system components
        /// that are not relevant for learning analytics.
        /// </summary>
        private static readonly Type[] ForbiddenTypes = new []
        {
            typeof(PipelineComponent),  // OmiLAXR pipeline infrastructure
            typeof(Pipeline),           // OmiLAXR pipeline instances
            typeof(Light),              // Unity lighting system
            typeof(EventSystem),        // Unity UI event handling
            typeof(StandaloneInputModule), // Unity input handling
            typeof(Canvas),             // Unity UI canvas
            typeof(TextMeshPro),        // TextMeshPro text rendering
            typeof(TextMeshProUGUI),    // TextMeshPro UI text rendering
        };
        
        /// <summary>
        /// Filters objects by excluding those that contain forbidden component types.
        /// Applies filtering logic to both individual components and GameObjects with forbidden components.
        /// </summary>
        /// <param name="gos">Array of Unity Objects to filter</param>
        /// <returns>Array of objects that don't contain forbidden components</returns>
        public override Object[] Pass(Object[] gos)
        {
            return gos.Where(IsAllowedObject).ToArray();
        }

        /// <summary>
        /// Determines if a component type is allowed for tracking.
        /// Checks if the type is in the forbidden list or inherits from a forbidden type.
        /// </summary>
        /// <param name="type">The component type to check</param>
        /// <returns>True if the type is allowed for tracking, false if forbidden</returns>
        public static bool IsAllowedType(Type type)
        {
            // Check if the type matches or inherits from any forbidden type
            return !ForbiddenTypes.Any(t => type == t || type.IsSubclassOf(t));
        }

        /// <summary>
        /// Determines if a Unity Object is allowed for tracking.
        /// For GameObjects, checks all attached components for forbidden types.
        /// For Components, checks the component type directly.
        /// </summary>
        /// <param name="obj">The Unity Object to check</param>
        /// <returns>True if the object is allowed for tracking, false if it contains forbidden components</returns>
        public static bool IsAllowedObject(Object obj)
        {
            var type = obj.GetType();

            // If it's not a GameObject, check the component type directly
            if (typeof(GameObject) != type) 
                return IsAllowedType(type);
            
            // For GameObjects, check all attached components
            var go = obj as GameObject;

            if (!go)
                return false;
            
            // Get all components and verify none are forbidden
            var components = go.GetComponents<Component>();
            return components.All(c => c != null && IsAllowedType(c.GetType()));
        }
    }
}
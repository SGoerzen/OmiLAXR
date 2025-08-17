/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using OmiLAXR.Extensions;
using UnityEngine;

namespace OmiLAXR.Components
{
    /// <summary>
    /// Component that allows setting a custom tracking name for a GameObject.
    /// This overrides the default tracking name behavior.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Game Objects / Custom Tracking Name")]
    [DisallowMultipleComponent]
    public class CustomTrackingName : MonoBehaviour
    {
        /// <summary>
        /// The custom tracking name to use for this GameObject.
        /// If empty, falls back to default tracking name behavior.
        /// </summary>
        [Tooltip("Alternative tracking name.")]
        public string customTrackingName = "";
    }

    /// <summary>
    /// Extension methods for handling custom tracking names on Unity Objects.
    /// </summary>
    public static class ObjectExtCustomTrackingName
    {
        /// <summary>
        /// Gets the CustomTrackingName component from a Unity Object.
        /// </summary>
        /// <param name="obj">The Unity Object to get the component from.</param>
        /// <returns>The CustomTrackingName component if found; null otherwise.</returns>
        public static CustomTrackingName GetCustomTrackingNameComponent(this Object obj)
        {
            if (obj is Component comp)
            {
                return comp.GetComponent<CustomTrackingName>();
            }
            return ((GameObject)obj).GetComponent<CustomTrackingName>();
        }
        
        /// <summary>
        /// Gets the tracking name for a Unity Object, considering custom tracking names and global settings.
        /// </summary>
        /// <param name="obj">The Unity Object to get the tracking name for.</param>
        /// <returns>
        /// The custom tracking name if set; otherwise returns either the full hierarchy path
        /// or the object's name based on global settings.
        /// </returns>
        public static string GetTrackingName(this Object obj)
        {
            var customTrackingNameComp = GetCustomTrackingNameComponent(obj);

            if (customTrackingNameComp)
                return customTrackingNameComp.customTrackingName;
            
            var trackingNameBehaviour = GlobalSettings.Instance.trackingNameBehaviour;
            return trackingNameBehaviour == GlobalSettings.TrackingNameBehaviour.HierarchyTreePath 
                ? obj.GetFullHierarchyPath() 
                : obj.name;
        }
    }
}
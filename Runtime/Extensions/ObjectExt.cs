/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.Linq;
using UnityEngine;

namespace OmiLAXR.Extensions
{
    /// <summary>
    /// Extension methods for Unity Object types, providing utility functions
    /// for object identification, hierarchy navigation, and filtering operations
    /// commonly used in analytics tracking and pipeline processing.
    /// </summary>
    public static class ObjectExt
    {
        /// <summary>
        /// Constructs the full hierarchy path of a Unity Object within the scene.
        /// Creates a forward-slash delimited path from root to the specific object,
        /// including component type names for Component objects.
        /// Used for unique object identification in analytics statements.
        /// </summary>
        /// <param name="obj">The Unity Object to get the path for (GameObject or Component)</param>
        /// <returns>Full hierarchy path string, or empty string if object is invalid</returns>
        /// <example>
        /// GameObject: "Scene/Player/Hand" 
        /// Component: "Scene/Player/Hand/BoxCollider"
        /// </example>
        public static string GetFullHierarchyPath(this Object obj)
        {
            GameObject gameObject;

            // Determine if the object is a component and extract the GameObject
            var isComponent = obj is Component comp;
            if (isComponent)
            {
                gameObject = ((Component)obj).gameObject;
            }
            else
            {
                gameObject = obj as GameObject;
            }
            
            // Return empty string if we can't get a valid GameObject
            if (gameObject == null)
                return string.Empty;
            
            // Build the hierarchy path by walking up the transform tree
            var path = gameObject.name;
            var current = gameObject.transform.parent;

            while (current != null)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }

            // Append component type name if this is a component
            if (isComponent)
            {
                path += "/" + ((Component)obj).GetType().Name;
            }

            return path;
        }
        
        /// <summary>
        /// Filters an array of Unity Objects to exclude those of a specific type or containing components of that type.
        /// Performs comprehensive type checking including direct type matches, inheritance relationships,
        /// and component composition for GameObjects.
        /// Used in pipeline filtering to remove unwanted object types from processing.
        /// </summary>
        /// <param name="objects">Array of Unity Objects to filter</param>
        /// <typeparam name="T">Type to exclude from the results</typeparam>
        /// <returns>Filtered array without objects of type T or objects containing components of type T</returns>
        /// <example>
        /// objects.Exclude&lt;Light&gt;() removes all Light components and GameObjects with Light components
        /// </example>
        public static Object[] Exclude<T>(this Object[] objects) where T : Object
        {
            var gameObjectType = typeof(GameObject);
            var componentType = typeof(Component);
            var targetType = typeof(T);

            return objects.Where(o =>
            {
                var objectType = o.GetType();

                // Exclude if object type directly matches or inherits from target type
                if (objectType == targetType || objectType.IsSubclassOf(targetType))
                    return false;

                // Check components if this is a Component object
                if (objectType == componentType || objectType.IsSubclassOf(componentType))
                {
                    var component = o as Component;
                    if (component && component.GetComponent<T>())
                        return false;
                }
                // Check components if this is a GameObject
                else if (objectType == gameObjectType || objectType.IsSubclassOf(gameObjectType))
                {
                    var gameObject = o as GameObject;
                    if (gameObject && gameObject.GetComponent<T>())
                        return false;
                }

                // Include the object if none of the exclusion criteria match
                return true;
            }).ToArray();
        }
    }
}
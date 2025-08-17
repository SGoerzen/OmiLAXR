/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR

using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using OmiLAXR.Endpoints;
using OmiLAXR.Hooks;

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Custom Unity Editor for DataProvider components that displays pipeline component collections.
    /// Provides a read-only view of registered Composers, Hooks, and Endpoints in collapsible sections,
    /// helping developers understand the current DataProvider configuration and connected components.
    /// </summary>
    [CustomEditor(typeof(DataProvider), true)] // Apply to DataProvider and all derived classes
    public class DataProviderEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Reference to the DataProvider component being inspected.
        /// Cached during OnEnable for efficient access throughout the editor lifecycle.
        /// </summary>
        private DataProvider _dataProvider;
        
        /// <summary>
        /// Dictionary tracking the fold-out state of each component collection section.
        /// Preserves user's expand/collapse preferences during Inspector updates.
        /// </summary>
        private Dictionary<string, bool> _foldouts;

        /// <summary>
        /// Initializes the custom editor by caching the target component and setting up foldout states.
        /// Called when the Inspector is opened or refreshed for a DataProvider component.
        /// </summary>
        private void OnEnable()
        {
            _dataProvider = (DataProvider)target;
            
            // Initialize foldout states with all sections expanded by default
            _foldouts = new Dictionary<string, bool>
            {
                { "Composers", true },
                { "Hooks", true },
                { "Endpoints", true }
            };
        }

        /// <summary>
        /// Renders the custom Inspector GUI with default properties and component collection views.
        /// Displays the standard DataProvider properties followed by organized, read-only lists
        /// of all registered pipeline components.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Render the default Inspector properties first
            DrawDefaultInspector();

            // Add visual separation before component sections
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Pipeline Components", EditorStyles.boldLabel);

            // Display each component collection in collapsible sections
            DrawReadonlyObjectList("Composers", _dataProvider.Composers, typeof(Object));
            DrawReadonlyObjectList("Hooks", _dataProvider.Hooks, typeof(Hook));
            DrawReadonlyObjectList("Endpoints", _dataProvider.Endpoints, typeof(Endpoint));
        }

        /// <summary>
        /// Draws a collapsible, read-only list of objects with count information and object field previews.
        /// Provides an organized view of component collections with proper type constraints
        /// and non-interactive object field displays for easy component identification.
        /// </summary>
        /// <param name="label">Display name for the collection section</param>
        /// <param name="list">IList containing the objects to display</param>
        /// <param name="type">System.Type for object field type constraints</param>
        private void DrawReadonlyObjectList(string label, IList list, System.Type type)
        {
            // Ensure foldout state exists for this section
            if (!_foldouts.ContainsKey(label))
                _foldouts[label] = false;

            // Draw collapsible header with count information
            _foldouts[label] = EditorGUILayout.Foldout(_foldouts[label], $"{label} ({list?.Count ?? 0})", true);
            
            if (_foldouts[label])
            {
                EditorGUI.indentLevel++; // Indent the content for visual hierarchy
                
                if (list == null || list.Count == 0)
                {
                    // Show placeholder when no items exist
                    EditorGUILayout.LabelField("None");
                }
                else
                {
                    // Display each object as a read-only object field
                    for (int i = 0; i < list.Count; i++)
                    {
                        var obj = list[i] as Object;
                        
                        // Use disabled group to prevent editing while showing the object reference
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.ObjectField($"[{i}]", obj, type, true);
                        EditorGUI.EndDisabledGroup();
                    }
                }
                
                EditorGUI.indentLevel--; // Restore indentation level
            }
        }
    }
}

#endif
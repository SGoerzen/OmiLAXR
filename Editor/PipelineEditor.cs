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
using OmiLAXR.Filters;
using OmiLAXR.Listeners;

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Custom Unity Editor for Pipeline components that displays all registered pipeline component collections.
    /// Provides a comprehensive, read-only view of Listeners, TrackingBehaviours, Filters, and DataProviders
    /// in organized collapsible sections, helping developers understand the complete pipeline configuration.
    /// </summary>
    [CustomEditor(typeof(Pipeline), true)] // Apply to Pipeline and all derived classes
    public class PipelineEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Reference to the Pipeline component being inspected.
        /// Cached during OnEnable for efficient access throughout the editor lifecycle.
        /// </summary>
        private Pipeline _pipeline;
        
        /// <summary>
        /// Dictionary tracking the fold-out state of each component collection section.
        /// Preserves user's expand/collapse preferences during Inspector updates.
        /// </summary>
        private Dictionary<string, bool> _foldouts;

        /// <summary>
        /// Initializes the custom editor by caching the target component and setting up foldout states.
        /// Called when the Inspector is opened or refreshed for a Pipeline component.
        /// </summary>
        private void OnEnable()
        {
            _pipeline = (Pipeline)target;
            
            // Initialize foldout states with all sections expanded by default
            _foldouts = new Dictionary<string, bool>
            {
                { "Listeners", true },
                { "TrackingBehaviours", true },
                { "Filters", true },
                { "DataProviders", true },
                //{ "Extensions", true } // Extensions section commented out - could be enabled if needed
            };
        }

        /// <summary>
        /// Renders the custom Inspector GUI with default properties and comprehensive component collection views.
        /// Displays the standard Pipeline properties followed by organized, read-only lists
        /// of all registered pipeline components across all categories.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Render the default Inspector properties first
            DrawDefaultInspector();

            // Add visual separation before component sections
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Pipeline Components", EditorStyles.boldLabel);

            // Display each component collection in collapsible sections
            DrawReadonlyObjectList("Listeners", _pipeline.Listeners, typeof(Listener));
            DrawReadonlyObjectList("TrackingBehaviours", _pipeline.TrackingBehaviours, typeof(Object));
            DrawReadonlyObjectList("Filters", _pipeline.Filters, typeof(Filter));
            DrawReadonlyObjectList("DataProviders", _pipeline.DataProviders, typeof(DataProvider));
            //DrawReadonlyObjectList("Extensions", _pipeline.Extensions, typeof(Object)); // Extensions display disabled
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
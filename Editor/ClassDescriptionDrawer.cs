/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR

using System;
using System.ComponentModel;
using UnityEditor;

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Custom Unity Editor for PipelineComponent classes that displays class-level DescriptionAttribute content.
    /// Enhances the Inspector by showing descriptive help text for components that have DescriptionAttributes,
    /// improving developer understanding of component purposes and usage. Supports multi-object editing.
    /// </summary>
    [CustomEditor(typeof(PipelineComponent), true)] // Apply to PipelineComponent and all derived classes
    [CanEditMultipleObjects] // Enable multi-object editing capabilities
    public class ClassDescriptionDrawer : UnityEditor.Editor
    {
        // Tracks foldout state for showing/hiding the description
        private bool _showDescription = true;

        /// <summary>
        /// Renders the Inspector GUI with class description information and default property fields.
        /// Extracts DescriptionAttribute from the component class and displays it as a help box,
        /// then renders the standard Inspector interface with full multi-object editing support.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Update the serialized object to handle multi-object editing properly
            serializedObject.Update();
            
            // Get the component's type from the first selected target object
            var componentType = target.GetType();

            // Use reflection to retrieve the DescriptionAttribute from the class
            var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(componentType, typeof(DescriptionAttribute));

            // Display the description with a foldout if the attribute exists
            if (descriptionAttribute != null)
            {
                // Add visual spacing for better layout
                EditorGUILayout.Space();

                // Render a foldout arrow labeled "Description"
                _showDescription = EditorGUILayout.Foldout(_showDescription, "Description", true);

                if (_showDescription)
                {
                    // Slight indent so the HelpBox lines up nicely
                    // EditorGUI.indentLevel++;
                    // Render the description text in an informational help box
                    EditorGUILayout.HelpBox(descriptionAttribute.Description, MessageType.None);
                    // EditorGUI.indentLevel--;
                }
            }

            // Apply any property modifications made during GUI rendering
            serializedObject.ApplyModifiedProperties();
            
            // Render the default Inspector interface (handles multi-object editing automatically)
            DrawDefaultInspector();
        }
    }
}

#endif
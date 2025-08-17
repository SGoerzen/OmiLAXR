/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Custom property drawer for ReadOnlyAttribute that renders fields as non-editable in the Inspector.
    /// Displays the property with its current value but prevents user modification,
    /// useful for showing calculated or runtime values that shouldn't be changed in the editor.
    /// </summary>
    [CustomPropertyDrawer( typeof( ReadOnlyAttribute ) )]
    public class ReadOnlyDrawer : PropertyDrawer {

        /// <summary>
        /// Calculates the height required to display the read-only property.
        /// Uses the standard property height calculation to maintain consistent spacing.
        /// </summary>
        /// <param name="property">SerializedProperty to calculate height for</param>
        /// <param name="label">GUIContent label for the property</param>
        /// <returns>Height in pixels required to display this property</returns>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
            return EditorGUI.GetPropertyHeight( property, label, true );
        }

        /// <summary>
        /// Renders the property field in a disabled state to prevent editing.
        /// Uses EditorGUI.DisabledGroupScope to create a non-interactive property display
        /// while maintaining the standard appearance and layout.
        /// </summary>
        /// <param name="position">Rectangle position for drawing the property</param>
        /// <param name="property">SerializedProperty to render</param>
        /// <param name="label">GUIContent label for the property field</param>
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
            // Use disabled group scope to prevent user interaction
            using (var scope = new EditorGUI.DisabledGroupScope(true)) {
                EditorGUI.PropertyField( position, property, label, true );
            }
        }

    }

    /// <summary>
    /// Decorator drawer for BeginReadOnlyGroupAttribute that starts a disabled GUI group.
    /// Begins a section where all subsequent GUI elements will be rendered as read-only
    /// until the corresponding EndReadOnlyGroupAttribute is encountered.
    /// </summary>
    [CustomPropertyDrawer( typeof( BeginReadOnlyGroupAttribute ) )]
    public class BeginReadOnlyGroupDrawer : DecoratorDrawer {

        /// <summary>
        /// Returns zero height as this decorator doesn't render visible content.
        /// The decorator only affects the GUI state for subsequent elements.
        /// </summary>
        /// <returns>Zero height since this decorator is invisible</returns>
        public override float GetHeight() { return 0; }

        /// <summary>
        /// Begins a disabled GUI group to make subsequent properties read-only.
        /// This affects all GUI elements until EditorGUI.EndDisabledGroup is called.
        /// </summary>
        /// <param name="position">Rectangle position (unused for this decorator)</param>
        public override void OnGUI( Rect position ) {
            EditorGUI.BeginDisabledGroup( true );
        }

    }

    /// <summary>
    /// Decorator drawer for EndReadOnlyGroupAttribute that ends a disabled GUI group.
    /// Terminates a read-only section started by BeginReadOnlyGroupAttribute,
    /// restoring normal GUI interactivity for subsequent elements.
    /// </summary>
    [CustomPropertyDrawer( typeof( EndReadOnlyGroupAttribute ) )]
    public class EndReadOnlyGroupDrawer : DecoratorDrawer {

        /// <summary>
        /// Returns zero height as this decorator doesn't render visible content.
        /// The decorator only affects the GUI state for subsequent elements.
        /// </summary>
        /// <returns>Zero height since this decorator is invisible</returns>
        public override float GetHeight() { return 0; }

        /// <summary>
        /// Ends the disabled GUI group to restore normal GUI interactivity.
        /// Must be paired with a corresponding BeginReadOnlyGroupAttribute.
        /// </summary>
        /// <param name="position">Rectangle position (unused for this decorator)</param>
        public override void OnGUI( Rect position ) {
            EditorGUI.EndDisabledGroup();
        }

    }
}

#endif
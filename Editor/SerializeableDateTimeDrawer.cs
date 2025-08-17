/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR
using System;
using OmiLAXR.Types;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Custom property drawer for SerializableDateTime that provides a user-friendly Inspector interface.
    /// Displays a formatted date/time preview alongside the standard property fields,
    /// helping developers visualize the actual DateTime value while editing individual components.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableDateTime))]
    public class SerializableDateTimeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Renders the SerializableDateTime property with a formatted preview in the Inspector.
        /// Extracts individual date/time components, constructs a DateTime for preview formatting,
        /// and displays the property with an enhanced label showing the formatted date/time.
        /// </summary>
        /// <param name="position">Rectangle position for drawing the property</param>
        /// <param name="property">SerializedProperty representing the SerializableDateTime</param>
        /// <param name="label">GUIContent label for the property field</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Extract individual date/time component properties
            var year = property.FindPropertyRelative("year");
            var month = property.FindPropertyRelative("month");
            var day = property.FindPropertyRelative("day");
            var hour = property.FindPropertyRelative("hour");
            var minute = property.FindPropertyRelative("minute");
            var second = property.FindPropertyRelative("second");
            var millisecond = property.FindPropertyRelative("millisecond");

            // Generate a formatted preview of the current date/time value
            var previewText = "Invalid date";
            try
            {
                // Construct DateTime from individual components for preview
                var dt = new DateTime(
                    year.intValue, month.intValue, day.intValue,
                    hour.intValue, minute.intValue, second.intValue,
                    millisecond != null ? millisecond.intValue : 0, // Handle potential null millisecond
                    DateTimeKind.Utc
                );
                // Format as user-friendly string with UTC indicator
                previewText = dt.ToString("yyyy-MM-dd HH:mm:ss 'UTC'");
            }
            catch
            {
                // Silently handle invalid date combinations (e.g., Feb 30)
                // Preview will show "Invalid date" for impossible date/time values
            }
            
            // Render the property with enhanced label including formatted preview
            var propertyRect = new Rect(position.x, position.y, position.width,
                EditorGUI.GetPropertyHeight(property, label, true));
            EditorGUI.PropertyField(propertyRect, property, new GUIContent($"{label.text}: {previewText}"), true); // Maintain foldout functionality

        }

        /// <summary>
        /// Calculates the height required to display the SerializableDateTime property.
        /// Uses the standard property height calculation to ensure proper spacing and layout,
        /// including space for all nested fields when the property is expanded.
        /// </summary>
        /// <param name="property">SerializedProperty to calculate height for</param>
        /// <param name="label">GUIContent label for the property</param>
        /// <returns>Height in pixels required to display this property</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif
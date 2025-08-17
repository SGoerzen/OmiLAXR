/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR
using System.ComponentModel;
using System.Reflection;
using OmiLAXR.Enums;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Custom property drawer for the Languages enum that displays description attributes in the Unity Inspector.
    /// Uses reflection to extract DescriptionAttribute values and show them as friendly display names
    /// instead of the raw enum value names, improving the editor user experience.
    /// </summary>
    [CustomPropertyDrawer(typeof(Languages))]
    public class LanguagesEnumDescriptionDrawer : PropertyDrawer
    {
        /// <summary>
        /// Renders the Languages enum property with description-based display names in the Inspector.
        /// Extracts the DescriptionAttribute from enum values and uses it as the display text,
        /// falling back to the enum name if no description is available.
        /// </summary>
        /// <param name="position">Rectangle position for drawing the property</param>
        /// <param name="property">SerializedProperty representing the Languages enum</param>
        /// <param name="label">GUIContent label for the property field</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the current enum value from the serialized property
            var enumValue = (Languages)property.enumValueIndex;

            // Use reflection to get the field info for the current enum value
            var field = enumValue.GetType().GetField(enumValue.ToString());
            // Extract the DescriptionAttribute if it exists on this enum field
            var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();

            // Use the description as display name, or fall back to the enum name
            var displayName = descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();
            
            // Render the property field with the custom display name
            EditorGUI.PropertyField(position, property, new GUIContent(displayName), true);
        }
    }
}
#endif
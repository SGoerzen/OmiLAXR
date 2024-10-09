#if UNITY_EDITOR
using System.ComponentModel;
using System.Reflection;
using OmiLAXR.Enums;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.Editor
{
    [CustomPropertyDrawer(typeof(Languages))]
    public class LanguagesEnumDescriptionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the current enum value
            var enumValue = (Languages)property.enumValueIndex;

            // Get the description attribute from the enum field
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();

            // Display the description in the Unity Inspector, fallback to enum name if no description exists
            var displayName = descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();
            EditorGUI.PropertyField(position, property, new GUIContent(displayName), true);
        }
    }
}
#endif
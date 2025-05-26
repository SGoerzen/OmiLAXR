#if UNITY_EDITOR
using System;
using OmiLAXR.Types;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDateTime))]
    public class SerializableDateTimeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Extract fields
            var year = property.FindPropertyRelative("year");
            var month = property.FindPropertyRelative("month");
            var day = property.FindPropertyRelative("day");
            var hour = property.FindPropertyRelative("hour");
            var minute = property.FindPropertyRelative("minute");
            var second = property.FindPropertyRelative("second");
            var millisecond = property.FindPropertyRelative("millisecond");

            // Render preview above the foldout
            var previewText = "Invalid date";
            try
            {
                var dt = new DateTime(
                    year.intValue, month.intValue, day.intValue,
                    hour.intValue, minute.intValue, second.intValue,
                    millisecond != null ? millisecond.intValue : 0,
                    DateTimeKind.Utc
                );
                previewText = dt.ToString("yyyy-MM-dd HH:mm:ss 'UTC'");
            }
            catch
            {
            }
            
            // Draw the actual property below
            var propertyRect = new Rect(position.x, position.y, position.width,
                EditorGUI.GetPropertyHeight(property, label, true));
            EditorGUI.PropertyField(propertyRect, property, new GUIContent($"{label.text}: {previewText}"), true); // keep foldout

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif
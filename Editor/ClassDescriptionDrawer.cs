#if UNITY_EDITOR

using System;
using System.ComponentModel;
using UnityEditor;

namespace OmiLAXR.Editor
{
    [CustomEditor(typeof(PipelineComponent), true)]
    [CanEditMultipleObjects] // Enable multi-object editing
    public class ClassDescriptionDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Update the serialized object for multi-object editing
            serializedObject.Update();
            
            // Get the component's type (for the first selected object)
            var componentType = target.GetType();

            // Retrieve the Description attribute if it exists
            var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(componentType, typeof(DescriptionAttribute));

            // If the Description attribute is present, draw it in the Inspector below the fields
            if (descriptionAttribute != null)
            {
                // Add some space for clarity
                EditorGUILayout.Space();
            
                // Draw the description in an info box
                EditorGUILayout.HelpBox(descriptionAttribute.Description, MessageType.None);
            }

            // Apply property modifications
            serializedObject.ApplyModifiedProperties();
            
            // Draw the default Inspector (handles multi-object editing automatically)
            DrawDefaultInspector();
        }
    }
}

#endif
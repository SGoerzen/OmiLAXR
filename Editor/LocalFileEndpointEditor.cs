#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using OmiLAXR.Endpoints;

[CustomEditor(typeof(LocalFileEndpoint))]
public class LocalFileEndpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var endpoint = (LocalFileEndpoint)target;

        // Start change check
        EditorGUI.BeginChangeCheck();

        // Draw default inspector
        DrawDefaultInspector();

        // Folder selection if Custom is selected
        if (endpoint.defaultFolder == LocalFileEndpoint.DefaultFolderPaths.Custom)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Storage Location", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Path", endpoint.customLocation);
            if (GUILayout.Button("Select Folder", GUILayout.MaxWidth(100)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Choose Folder for Local Storage", "", "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    Undo.RecordObject(endpoint, "Change Custom Location");
                    endpoint.customLocation = selectedPath;
                    EditorUtility.SetDirty(endpoint);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        // If anything changed, update preview path
        if (EditorGUI.EndChangeCheck())
        {
            endpoint.UpdateFileLocationPreview();
            EditorUtility.SetDirty(endpoint);
        }
    }
}

#endif
#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using OmiLAXR.Endpoints;

[CustomEditor(typeof(LocalFileEndpoint), true)]
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
                var selectedPath = EditorUtility.OpenFolderPanel("Choose Folder for Local Storage", "", "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    Undo.RecordObject(endpoint, "Change Custom Location");
                    endpoint.customLocation = selectedPath;
                    EditorUtility.SetDirty(endpoint);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        // Open Folder / Open File for preview path
        if (!string.IsNullOrEmpty(endpoint.EditorPreviewFilePath))
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Example File Location Preview", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = true
            };
            EditorGUILayout.TextArea(endpoint.EditorPreviewFilePath, textAreaStyle, GUILayout.MinHeight(40));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open Statements Folder"))
            {
                var directory = endpoint.ResolvedStatementsFolderPath;
                if (Directory.Exists(directory))
                {
                    EditorUtility.RevealInFinder(directory);
                }
                else
                {
                    Debug.LogWarning("Directory does not exist: " + directory);
                }
            }

            if (GUILayout.Button("Copy path in clipboard"))
            {
                var directory = endpoint.ResolvedStatementsFolderPath;
                if (Directory.Exists(directory))
                {
                    EditorGUIUtility.systemCopyBuffer = directory;
                }
                else
                {
                    Debug.LogWarning("Directory does not exist: " + directory);
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
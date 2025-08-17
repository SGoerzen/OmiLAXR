/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR
using System.IO;
using OmiLAXR;
using UnityEditor;
using UnityEngine;
using OmiLAXR.Endpoints;

/// <summary>
/// Custom Unity Editor for LocalFileEndpoint components that provides enhanced file path management.
/// Offers custom folder selection UI, path preview functionality, and convenient file system navigation
/// for configuring local file storage endpoints in the OmiLAXR analytics pipeline.
/// </summary>
[CustomEditor(typeof(LocalFileEndpoint), true)]
public class LocalFileEndpointEditor : Editor
{
    /// <summary>
    /// Renders the custom Inspector GUI with enhanced file path management features.
    /// Provides folder selection dialog for custom paths, file location preview,
    /// and convenient buttons for opening folders and copying paths to clipboard.
    /// </summary>
    public override void OnInspectorGUI()
    {
        var endpoint = (LocalFileEndpoint)target;

        // Begin change detection to update preview when properties change
        EditorGUI.BeginChangeCheck();

        // Render the standard LocalFileEndpoint properties
        DrawDefaultInspector();

        // Show custom folder selection UI when Custom folder option is selected
        if (endpoint.defaultFolder == LocalFileEndpoint.DefaultFolderPaths.Custom)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Storage Location", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            // Display current custom path in a read-only text field
            EditorGUILayout.TextField("Path", endpoint.customLocation);
            
            // Provide folder selection button for easy path configuration
            if (GUILayout.Button("Select Folder", GUILayout.MaxWidth(100)))
            {
                var selectedPath = EditorUtility.OpenFolderPanel("Choose Folder for Local Storage", "", "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    // Record undo operation and update the custom location
                    Undo.RecordObject(endpoint, "Change Custom Location");
                    endpoint.customLocation = selectedPath;
                    EditorUtility.SetDirty(endpoint); // Mark object as modified for saving
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        // Display file location preview and navigation tools if a preview path exists
        if (!string.IsNullOrEmpty(endpoint.EditorPreviewFilePath))
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Example File Location Preview", EditorStyles.boldLabel);

            // Show the complete file path in a read-only, word-wrapped text area
            EditorGUI.BeginDisabledGroup(true);
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = true // Enable word wrapping for long paths
            };
            EditorGUILayout.TextArea(endpoint.EditorPreviewFilePath, textAreaStyle, GUILayout.MinHeight(40));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginHorizontal();
            
            // Button to open the statements folder in the system file explorer
            if (GUILayout.Button("Open Statements Folder"))
            {
                var directory = endpoint.ResolvedStatementsFolderPath;
                if (Directory.Exists(directory))
                {
                    EditorUtility.RevealInFinder(directory); // Open folder in Finder/Explorer
                }
                else
                {
                    DebugLog.OmiLAXR.Warning("Directory does not exist: " + directory);
                }
            }

            // Button to copy the folder path to the system clipboard
            if (GUILayout.Button("Copy path in clipboard"))
            {
                var directory = endpoint.ResolvedStatementsFolderPath;
                if (Directory.Exists(directory))
                {
                    EditorGUIUtility.systemCopyBuffer = directory; // Copy path to clipboard
                }
                else
                {
                    DebugLog.OmiLAXR.Warning("Directory does not exist: " + directory);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        // Update the file location preview if any properties changed
        if (EditorGUI.EndChangeCheck())
        {
            endpoint.UpdateFileLocationPreview(); // Refresh the preview path
            EditorUtility.SetDirty(endpoint); // Mark object as modified
        }
    }
}

#endif
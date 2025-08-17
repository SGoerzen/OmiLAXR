/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Utility class for adding OmiLAXR prefabs to the current scene through Editor menu items.
    /// Provides convenient menu-driven prefab instantiation with proper undo support
    /// and automatic scene marking for save detection.
    /// </summary>
    internal static class AddPrefabToScene
    {
        /// <summary>
        /// Path to the main OmiLAXR prefab within the package resources.
        /// Contains the core Actor Pipeline components needed for basic OmiLAXR functionality.
        /// </summary>
        private const string PrefabPath = "Packages/com.rwth.unity.omilaxr/Resources/Prefabs/OmiLAXR.prefab";

        /// <summary>
        /// Adds the main OmiLAXR Actor Pipeline prefab to the current scene.
        /// Loads the prefab from the package, instantiates it at the scene origin,
        /// and sets up proper undo support and scene dirty marking for save detection.
        /// </summary>
        [MenuItem("OmiLAXR / Prefabs / 1) Add OmiLAXR Actor Pipeline components to Scene")]
        public static void AddPrefab()
        {
            // Load the prefab asset from the package resources
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (prefab == null)
            {
                DebugLog.OmiLAXR.Error($"Prefab not found at path: {PrefabPath}");
                return;
            }

            // Instantiate the prefab as a prefab instance (maintains prefab connection)
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            
            // Register the instantiation with the undo system for easy reversal
            Undo.RegisterCreatedObjectUndo(instance, "Add xAPI Prefab");

            // Position the instance at the scene center for consistent placement
            instance.transform.position = Vector3.zero;

            // Mark the scene as dirty to prompt save when closing Unity
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            DebugLog.OmiLAXR.Print($"Added prefab '{prefab.name}' to scene.");
        }
    }
}

#endif
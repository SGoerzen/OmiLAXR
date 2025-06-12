#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace OmiLAXR.Editor
{
    internal static class AddPrefabToScene
    {
        // Adjust the path to your actual prefab location inside your package
        private const string PrefabPath = "Packages/com.rwth.unity.omilaxr/Resources/Prefabs/OmiLAXR.prefab";

        [MenuItem("OmiLAXR / Prefabs / 1) Add OmiLAXR Actor Pipeline components to Scene")]
        public static void AddPrefab()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found at path: {PrefabPath}");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(instance, "Add xAPI Prefab");

            // Position it at the center of the scene or any custom default position
            instance.transform.position = Vector3.zero;

            // Mark scene dirty for saving
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            Debug.Log($"Added prefab '{prefab.name}' to scene.");
        }
    }
}

#endif
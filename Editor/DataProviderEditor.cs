#if UNITY_EDITOR

using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using OmiLAXR.Endpoints;
using OmiLAXR.Hooks;

namespace OmiLAXR.Editor
{
    [CustomEditor(typeof(DataProvider), true)]
    public class DataProviderEditor : UnityEditor.Editor
    {
        private DataProvider _dataProvider;
        private Dictionary<string, bool> _foldouts;

        private void OnEnable()
        {
            _dataProvider = (DataProvider)target;
            _foldouts = new Dictionary<string, bool>
            {
                { "Composers", true },
                { "Hooks", true },
                { "Endpoints", true }
            };
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Pipeline Components", EditorStyles.boldLabel);

            DrawReadonlyObjectList("Composers", _dataProvider.Composers, typeof(Object));
            DrawReadonlyObjectList("Hooks", _dataProvider.Hooks, typeof(Hook));
            DrawReadonlyObjectList("Endpoints", _dataProvider.Endpoints, typeof(Endpoint));
        }

        private void DrawReadonlyObjectList(string label, IList list, System.Type type)
        {
            if (!_foldouts.ContainsKey(label))
                _foldouts[label] = false;

            _foldouts[label] = EditorGUILayout.Foldout(_foldouts[label], $"{label} ({list?.Count ?? 0})", true);
            if (_foldouts[label])
            {
                EditorGUI.indentLevel++;
                if (list == null || list.Count == 0)
                {
                    EditorGUILayout.LabelField("None");
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var obj = list[i] as Object;
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.ObjectField($"[{i}]", obj, type, true);
                        EditorGUI.EndDisabledGroup();
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}

#endif
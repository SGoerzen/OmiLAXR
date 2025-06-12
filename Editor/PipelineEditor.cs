#if UNITY_EDITOR

using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using OmiLAXR.Filters;
using OmiLAXR.Listeners;

namespace OmiLAXR.Editor
{
    [CustomEditor(typeof(Pipeline), true)]
    public class PipelineEditor : UnityEditor.Editor
    {
        private Pipeline _pipeline;
        private Dictionary<string, bool> _foldouts;

        private void OnEnable()
        {
            _pipeline = (Pipeline)target;
            _foldouts = new Dictionary<string, bool>
            {
                { "Listeners", true },
                { "TrackingBehaviours", true },
                { "Filters", true },
                { "DataProviders", true },
                //{ "Extensions", true }
            };
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Pipeline Components", EditorStyles.boldLabel);

            DrawReadonlyObjectList("Listeners", _pipeline.Listeners, typeof(Listener));
            DrawReadonlyObjectList("TrackingBehaviours", _pipeline.TrackingBehaviours, typeof(Object));
            DrawReadonlyObjectList("Filters", _pipeline.Filters, typeof(Filter));
            DrawReadonlyObjectList("DataProviders", _pipeline.DataProviders, typeof(DataProvider));
            //DrawReadonlyObjectList("Extensions", _pipeline.Extensions, typeof(Object));
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
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace OmiLAXR.Editor
{
    [EditorTool("Statistics Toolbar")]
    internal class StatisticsToolbar : EditorTool
    {
        internal const int ID = 23082024;
        GUIContent m_IconContent;
        Rect windowRect = new Rect(10, 10, 200, 70);
        
        private void OnEnable()
        {
            // Load an icon for the toolbar button
            m_IconContent = new GUIContent()
            {
                image = EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow").image,
                text = "Stats",
                tooltip = "Show component statistics"
            };
            
            // Set the initial position of the window in the lower right corner
            SceneView.duringSceneGui += InitializeWindowPosition;
        }

        public override GUIContent toolbarIcon => m_IconContent;

        private void InitializeWindowPosition(SceneView sceneView)
        {
            if (windowRect.width == 0 && windowRect.height == 0) // Ensure it's only calculated once
            {
                const float width = 200f;
                const float height = 70f;
                windowRect = new Rect(sceneView.position.width - width - 10, sceneView.position.height - height - 10, width, height);
            }
            SceneView.duringSceneGui -= InitializeWindowPosition; // Unsubscribe after positioning
        }
        
        public override void OnToolGUI(EditorWindow window)
        {
            if (!(window.GetType() == typeof(SceneView)))
                return;

            Handles.BeginGUI();

            windowRect = GUILayout.Window(ID, windowRect, DrawWindow, "OmiLAXR");

            Handles.EndGUI();
        }

        private void DrawWindow(int windowID)
        {
            var pipelines = Resources.FindObjectsOfTypeAll(typeof(Pipeline)) as Pipeline[];

            if (pipelines != null)
            {
                foreach (var pipeline in pipelines)
                {
                    GUILayout.Label(pipeline.name);

                }
            }
            
          
            
            GUILayout.Label("Number of Pipelines: " + GetNumberOfComponents());

            // Make the window draggable by the title bar
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        int GetNumberOfComponents()
        {
            if (Selection.activeGameObject != null)
            {
                // Count components in the selected GameObject
                return Selection.activeGameObject.GetComponents<Component>().Length;
            }
            return 0;
        }
    }
}
#endif
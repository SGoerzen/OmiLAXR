/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Custom Unity Editor Tool that provides a floating statistics window in the Scene View.
    /// Displays real-time information about OmiLAXR pipelines and component counts,
    /// helping developers monitor system state during scene editing and debugging.
    /// </summary>
    [EditorTool("Statistics Toolbar")]
    internal class StatisticsToolbar : EditorTool
    {
        /// <summary>
        /// Unique identifier for this tool's GUI window to prevent conflicts with other tools.
        /// </summary>
        internal const int ID = 23082024;
        
        /// <summary>
        /// Icon content displayed in the Unity toolbar for this tool.
        /// Cached for performance to avoid repeated icon loading.
        /// </summary>
        GUIContent m_IconContent;
        
        /// <summary>
        /// Rectangle defining the position and size of the floating statistics window.
        /// Initially positioned in the lower right corner of the Scene View.
        /// </summary>
        Rect windowRect = new Rect(10, 10, 200, 70);
        
        /// <summary>
        /// Initializes the tool by setting up the toolbar icon and positioning the statistics window.
        /// Called when the tool is enabled or first accessed in the Unity Editor.
        /// </summary>
        private void OnEnable()
        {
            // Configure the toolbar button appearance with icon and tooltip
            m_IconContent = new GUIContent()
            {
                image = EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow").image, // Use console window icon
                text = "Stats",
                tooltip = "Show component statistics"
            };
            
            // Subscribe to Scene View GUI events to position window correctly
            SceneView.duringSceneGui += InitializeWindowPosition;
        }

        /// <summary>
        /// Gets the toolbar icon content displayed in the Unity Editor toolbar.
        /// Used by Unity's EditorTool system to show this tool in the toolbar.
        /// </summary>
        public override GUIContent toolbarIcon => m_IconContent;

        /// <summary>
        /// Positions the statistics window in the lower right corner of the Scene View.
        /// Called once during initialization to set the optimal window position,
        /// then automatically unsubscribes to avoid repeated positioning calls.
        /// </summary>
        /// <param name="sceneView">Scene View instance for positioning calculations</param>
        private void InitializeWindowPosition(SceneView sceneView)
        {
            // Only calculate position once to avoid continuous repositioning
            if (windowRect.width == 0 && windowRect.height == 0)
            {
                const float width = 200f;
                const float height = 70f;
                // Position in lower right corner with margin
                windowRect = new Rect(sceneView.position.width - width - 10, sceneView.position.height - height - 10, width, height);
            }
            
            // Unsubscribe after positioning to prevent repeated calls
            SceneView.duringSceneGui -= InitializeWindowPosition;
        }
        
        /// <summary>
        /// Renders the tool's GUI elements when the tool is active.
        /// Only displays the statistics window when the Scene View is the active editor window,
        /// ensuring the overlay appears in the correct context.
        /// </summary>
        /// <param name="window">Editor window where the tool GUI should be rendered</param>
        public override void OnToolGUI(EditorWindow window)
        {
            // Only display in Scene View windows
            if (!(window.GetType() == typeof(SceneView)))
                return;

            Handles.BeginGUI(); // Begin GUI rendering in Scene View coordinates

            // Render the draggable statistics window
            windowRect = GUILayout.Window(ID, windowRect, DrawWindow, "OmiLAXR");

            Handles.EndGUI(); // End GUI rendering
        }

        /// <summary>
        /// Draws the content of the statistics window with pipeline information and component counts.
        /// Displays all active pipelines by name and shows total pipeline count,
        /// with a draggable title bar for user repositioning.
        /// </summary>
        /// <param name="windowID">Unique window identifier for GUI system</param>
        private void DrawWindow(int windowID)
        {
            // Find all Pipeline instances in the scene, including inactive ones
            var pipelines = Resources.FindObjectsOfTypeAll(typeof(Pipeline)) as Pipeline[];

            if (pipelines != null)
            {
                // Display each pipeline name for quick identification
                foreach (var pipeline in pipelines)
                {
                    GUILayout.Label(pipeline.name);
                }
            }
            
            // Display total count of pipelines for system overview
            GUILayout.Label("Number of Pipelines: " + GetNumberOfComponents());

            // Make the entire window draggable by the title bar area
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        /// <summary>
        /// Gets the number of components on the currently selected GameObject.
        /// Used for component counting statistics in the statistics display.
        /// Returns zero if no GameObject is selected.
        /// </summary>
        /// <returns>Number of components on selected GameObject, or 0 if none selected</returns>
        int GetNumberOfComponents()
        {
            if (Selection.activeGameObject != null)
            {
                // Count all components attached to the selected GameObject
                return Selection.activeGameObject.GetComponents<Component>().Length;
            }
            return 0;
        }
    }
}
#endif
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.Editor
{
    [CustomEditor(typeof(OmiLAXR.Components.EyeCalibrator), true)]
    public class EyeCalibratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            using (new EditorGUI.DisabledScope(!Application.isPlaying))
            {
                var calibrator = (OmiLAXR.Components.EyeCalibrator)target;
                if (GUILayout.Button("Start Calibration")) calibrator.StartCalibration();
                if (GUILayout.Button("Stop Calibration")) calibrator.StopCalibration();
            }
        }
    }
}
#endif
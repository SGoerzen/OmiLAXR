using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Context
{
    [AddComponentMenu("OmiLAXR / 0) Scenario Context / FPS Monitor")]
    [DisallowMultipleComponent]
    [Description("Tracks Frames per Seconds.")]
    public class FpsMonitor : LearningContext
    {
        private static FpsMonitor _instance;
        public static FpsMonitor Instance
            => _instance ??= FindObjectOfType<FpsMonitor>();
        
        [ReadOnly] public int fps = 0;
        
        // Variables for calculating FPS
        private float _deltaTime = 0.0f;

        // Update is called once per frame
        private void Update()
        {
            // Update the time between frames
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            fps = (int)(1.0f / _deltaTime);
        }
    }
}
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Context
{
    [AddComponentMenu("OmiLAXR / Scenario Context / FPS Monitor")]
    [DisallowMultipleComponent]
    [Description("Tracks Frames per Seconds.")]
    public class FpsMonitor : LearningContext
    {
        private static FpsMonitor _instance;
        public static FpsMonitor Instance => GetInstance(ref _instance);

        [SerializeField, InspectorName("FPS"), ReadOnly]
        private int currentFPS = 0;
        
        [SerializeField, InspectorName("Average FPS"), ReadOnly]
        private float averageFPS = 0.0f;
        
        public int CurrentFPS => currentFPS;
        public float CurrentAverageFPS => averageFPS;
        
        // Variables for calculating FPS
        private float _deltaTime = 0.0f;
        
        // Variables for calculating average FPS
        private float _fpsUpdateInterval = 0.5f; // Update average every 0.5 seconds
        private float _fpsTimer = 0.0f;
        
        // Using an exponential moving average to avoid overflow issues
        private bool _isFirstAverage = true;
        private float _smoothingFactor = 0.1f; // Adjust this value between 0 and 1

        // Update is called once per frame
        private void Update()
        {
            // Update the time between frames
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            currentFPS = (int)(1.0f / _deltaTime);
            
            // Update average FPS using exponential moving average
            _fpsTimer += Time.unscaledDeltaTime;
            
            if (_fpsTimer >= _fpsUpdateInterval)
            {
                if (_isFirstAverage)
                {
                    averageFPS = currentFPS;
                    _isFirstAverage = false;
                }
                else
                {
                    // Exponential moving average: newAvg = currentValue * alpha + oldAvg * (1 - alpha)
                    averageFPS = currentFPS * _smoothingFactor + averageFPS * (1 - _smoothingFactor);
                }
                
                _fpsTimer = 0.0f;
            }
        }
    }
}
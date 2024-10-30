using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Context.HeartRate
{
    [AddComponentMenu("OmiLAXR / 0) Scenario Context / Heart Rate Monitor")]
    [DisallowMultipleComponent]
    [Description("Monitors heart rate value provided by a Heart Rate Provider.")]
    public class HeartRateMonitor : LearningContext
    {
        private static HeartRateMonitor _instance;
        public static HeartRateMonitor Instance
            => _instance ??= FindObjectOfType<HeartRateMonitor>();

        public HeartRateProvider provider;

        private void OnEnable()
        {
            
        }
    }
}
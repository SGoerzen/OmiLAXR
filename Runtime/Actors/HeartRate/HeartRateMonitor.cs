using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Actors.HeartRate
{
    [AddComponentMenu("OmiLAXR / 0) Learner / Heart Rate Monitor")]
    [DisallowMultipleComponent]
    [Description("Monitors heart rate value provided by a Heart Rate Provider.")]
    public class HeartRateMonitor : PipelineComponent
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
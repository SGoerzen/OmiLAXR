using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Actors.HeartRate
{
    [DisallowMultipleComponent]
    [Description("Monitors heart rate value provided by a Heart Rate Provider.")]
    public abstract class HeartRateProvider : ActorDataProvider
    {
        public abstract int GetHeartRate();
    }
}
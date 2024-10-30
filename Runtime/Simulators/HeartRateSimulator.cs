using OmiLAXR.Context.HeartRate;
using UnityEngine;
using Random = System.Random;

namespace OmiLAXR.Simulators
{
    public class HeartRateSimulator : HeartRateProvider
    {
        private Random random = new Random();

        private float _elapsedTime = 0f;
        
        [ReadOnly]
        public int heartRate;
        
        public override int GetHeartRate()
            => heartRate;
        
        private void FixedUpdate()
        {
            _elapsedTime += Time.fixedDeltaTime;

            if (_elapsedTime < 1.0f)
                return;

            heartRate = random.Next(50, 110);
            _elapsedTime = 0f;
        }
    }
}
using System.ComponentModel;
using OmiLAXR.TrackingBehaviours.Learner.EyeTracking;
using UnityEngine;

namespace OmiLAXR.Simulators
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Eye Tracking Behaviour (Simulator)"),
     Description("Simulates eye tracking behaviour by mouse and triggers the events.")]
    public class EyeTrackingBehaviourSimulator : EyeTrackingBehaviour
    {
        public double exampleViewingAngle = 45.0;

        private float _nextBlinkTime = 0f; // Time for the next blink
        public float minBlinkInterval = 3f; // Minimum interval between blinks in seconds
        public float maxBlinkInterval = 5f; // Maximum interval between blinks in seconds

        private bool _enabledEyeFixation = false;
        
        private void Start()
        {
            // Set the initial time for the first blink
            ScheduleNextBlink();
        }

        private void FixedUpdate()
        {
            // Check if it's time to simulate a blink
            if (Time.time >= _nextBlinkTime)
            {
                SimulateBlinkEvent();
                ScheduleNextBlink(); // Schedule the next blink
            }
        }

        private void SimulateBlinkEvent()
        {
            var randomEye = Random.Range(0, 2);
            var eye = new[] { BlinkData.BlinkEye.Both, BlinkData.BlinkEye.Left, BlinkData.BlinkEye.Right };
            var durationInMilliseconds = Random.Range(100, 300); // Typical blink duration in ms
            var blinkData = new BlinkData(eye[randomEye],
                new Duration(durationInMilliseconds, Duration.DurationUnit.Milliseconds));

            OnBlinked.Invoke(this, blinkData);
            Debug.Log($"Simulated Blink Event: Duration = {durationInMilliseconds} ms");
        }

        private void ScheduleNextBlink()
        {
            // Set the time for the next blink using a random interval
            _nextBlinkTime = Time.time + Random.Range(minBlinkInterval, maxBlinkInterval);
        }

        public override PupilDilationData? GetPupilDilationData()
        {
            throw new System.NotImplementedException();
        }

        public override double? GetViewingAngle() => exampleViewingAngle;
        
        // Detect collisions to simulate a fixation event on collision
        private void OnCollisionEnter(Collision collision)
        {
            if (!_enabledEyeFixation)
                return;
            // Get collision point data
            var collisionPoint = collision.contacts[0].point;
            SimulateFixationEvent(collision.gameObject, collisionPoint, Random.Range(500, 1500)); // Random duration for fixation
        }

        private void SimulateFixationEvent(GameObject target, Vector3 fixationPoint, int durationMilliseconds)
        {
            var fixationData = new FixationData(target, fixationPoint, Duration.FromMilliseconds(durationMilliseconds));

            OnFixated.Invoke(this, fixationData);
            Debug.Log($"Simulated Fixation Event: Position = {fixationPoint}, Duration = {durationMilliseconds} ms");
        }

        private void OnGUI()
        {
            // Set button dimensions
            const float buttonWidth = 150f;
            const float buttonHeight = 50f;
            const float padding = 10f;
            const float startX = 10f;
            const float startY = 10f;

            // Fixation Button
            if (GUI.Button(new Rect(startX, startY, buttonWidth, buttonHeight), (_enabledEyeFixation ? "Start" : "Stop") + " Simulation Eye Fixation"))
            {
                _enabledEyeFixation = !_enabledEyeFixation;
            }
        }
    }
}
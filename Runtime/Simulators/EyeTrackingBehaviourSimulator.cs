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
        
        private void Start()
        {
            
        }

        public override PupilDilationData? GetPupilDilationData()
        {
            throw new System.NotImplementedException();
        }

        public override double? GetViewingAngle() => exampleViewingAngle;

        public EyeTrackingBehaviour eyeTrackingBehaviour;

    private void OnGUI()
    {
        // Set button dimensions
        const float buttonWidth = 150f;
        const float buttonHeight = 50f;
        const float padding = 10f;
        const float startX = 10f;
        const float startY = 10f;

        // Fixation Button
        if (GUI.Button(new Rect(startX, startY, buttonWidth, buttonHeight), "Simulate Fixation"))
        {
            // var fixationData = new FixationData
            // {
            //     GazeCoordinates = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0),
            //     Duration = Random.Range(500, 1500) // Duration in ms
            // };
            // eyeTrackingBehaviour.OnFixated.Invoke(fixationData);
            Debug.Log("Simulated Fixation Event");
        }

        // Saccade Button
        if (GUI.Button(new Rect(startX, startY + (buttonHeight + padding), buttonWidth, buttonHeight), "Simulate Saccade"))
        {
            // var saccadeData = new SaccadeData
            // {
            //     StartGazeCoordinates = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0),
            //     EndGazeCoordinates = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0),
            //     SaccadeAmplitudeDegrees = Random.Range(5.0f, 20.0f)
            // };
            // eyeTrackingBehaviour.OnSaccaded.Invoke(saccadeData);
            Debug.Log("Simulated Saccade Event");
        }

        // Micro Saccade Button
        if (GUI.Button(new Rect(startX, startY + 2 * (buttonHeight + padding), buttonWidth, buttonHeight), "Simulate Micro Saccade"))
        {
            // var microSaccadeData = new MicroSaccadeData
            // {
            //     AmplitudeInDegrees = Random.Range(0.1f, 0.5f),
            //     DirectionInDegrees = Random.Range(0.0f, 360.0f),
            //     DurationInMilliseconds = Random.Range(10, 50)
            // };
            // eyeTrackingBehaviour.OnMicroSaccaded.Invoke(microSaccadeData);
            Debug.Log("Simulated Micro Saccade Event");
        }

        // Blink Button
        if (GUI.Button(new Rect(startX, startY + 3 * (buttonHeight + padding), buttonWidth, buttonHeight), "Simulate Blink"))
        {
            // var blinkData = new BlinkData
            // {
            //     DurationInMilliseconds = Random.Range(100, 300)
            // };
            // eyeTrackingBehaviour.OnBlinked.Invoke(blinkData);
            Debug.Log("Simulated Blink Event");
        }

        // Pupil Dilation Button
        if (GUI.Button(new Rect(startX, startY + 4 * (buttonHeight + padding), buttonWidth, buttonHeight), "Simulate Pupil Dilation"))
        {
            // var pupilDilationData = new PupilDilationData
            // {
            //     PupilDiameterStart = 3.1f,
            //     PupilDiameterEnd = Random.Range(3.5f, 4.5f),
            //     DilationChange = pupilDilationData.PupilDiameterEnd - pupilDilationData.PupilDiameterStart,
            //     DurationInMilliseconds = Random.Range(500, 1500)
            // };
            // eyeTrackingBehaviour.OnPupilDilation.Invoke(pupilDilationData);
            Debug.Log("Simulated Pupil Dilation Event");
        }
    }
    }
}
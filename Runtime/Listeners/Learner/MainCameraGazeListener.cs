using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners.Learner
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / Main <Camera> Gaze Listener")]
    [Description("Prepares the main <Camera> to be used for gaze tracking.")]
    public class MainCameraGazeListener : GazeDetectorListener<Camera>
    {
        protected override bool IsCorrectComponent(Camera component) => Camera.main == component;
    }
}
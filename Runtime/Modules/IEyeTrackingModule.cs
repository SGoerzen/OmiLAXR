using System;

namespace OmiLAXR.Modules
{
    public interface IEyeTrackingModule
    {
        void StartCalibration();
        void StopCalibration();
        bool IsCalibrated();
        event Action OnCalibrationStarted;
        event Action OnCalibrationStopped;
    }
}
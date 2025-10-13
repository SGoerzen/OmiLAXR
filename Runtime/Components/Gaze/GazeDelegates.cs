using OmiLAXR.Components.Gaze.Fixation;

namespace OmiLAXR.Components.Gaze
{
    public delegate void GazeHitHandler(GazeHit gazeHit);
    public delegate void FixationStartedHandler(GazeHit gazeHit, FixationData data);
    public delegate void FixationEndedHandler(GazeHit gazeHit, FixationData data);
    public delegate void SaccadeStartedHandler(GazeHit gazeHit, SaccadeData data);
    public delegate void SaccadeEndedHandler(GazeHit gazeHit, SaccadeData data);
}
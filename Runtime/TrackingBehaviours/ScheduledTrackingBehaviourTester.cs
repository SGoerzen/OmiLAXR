using System;

namespace OmiLAXR.TrackingBehaviours
{
    public class ScheduledTrackingBehaviourTester : ScheduledTrackingBehaviour
    {
        protected override void OnStartedPipeline(Pipeline pipeline)
        {
            base.OnStartedPipeline(pipeline);
        }

        protected override void Run()
        {
            DebugLog.OmiLAXR.Print(DateTime.Now + " RUN SCHEDULE");
        }
    }
}
namespace OmiLAXR.Actors.HeartRate
{
    public abstract class HeartRateProvider : ActorPipelineComponent
    {
        public abstract int GetHeartRate();
    }
}
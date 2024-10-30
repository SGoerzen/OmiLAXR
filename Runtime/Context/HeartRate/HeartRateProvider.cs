namespace OmiLAXR.Context.HeartRate
{
    public abstract class HeartRateProvider : PipelineComponent
    {
        public abstract int GetHeartRate();
    }
}
namespace OmiLAXR
{
    public abstract class ActorPipelineComponent : PipelineComponent
    {
        public Pipeline Pipeline { get; private set; }
        public Actor GetActor() => Pipeline.actor;
        public Instructor GetInstructor() => Pipeline.instructor;
        
        protected virtual void Awake()
        {
#if UNITY_2019
            Pipeline = GetComponent<Pipeline>() ?? GetComponentInParent<Pipeline>();
#else
            Pipeline = GetComponent<Pipeline>() ?? GetComponentInParent<Pipeline>(true);
#endif

            // cannot find a pipeline. Look for a Pipeline Extension
            if (!Pipeline)
            {
                var pipelineExt = GetComponentInParent<IPipelineExtension>();
                Pipeline = pipelineExt.GetPipeline();
            }
        }
        
        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"(Pipeline '{Pipeline.name}') " + message);
    }
}
namespace OmiLAXR
{
    public abstract class ActorPipelineComponent : PipelineComponent
    {
        protected Pipeline pipeline { get; private set; }
        public Actor GetActor() => pipeline.actor;
        public Instructor GetInstructor() => pipeline.instructor;
        
        protected virtual void Awake()
        {
            pipeline = GetComponent<Pipeline>() ?? GetComponentInParent<Pipeline>(true);

            // cannot find a pipeline. Look for a Pipeline Extension
            if (!pipeline)
            {
                var pipelineExt = GetComponentInParent<IPipelineExtension>();
                pipeline = pipelineExt.GetPipeline();
            }
        }
        
        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"(Pipeline '{pipeline.name}') " + message);
    }
}
namespace OmiLAXR
{
    public abstract class ActorPipelineComponent : PipelineComponent
    {
        private Pipeline _pipeline;

        public Pipeline Pipeline
        {
            get
            {
                if (!_pipeline)
                {
#if UNITY_2019
                    _pipeline = GetComponent<Pipeline>() ?? GetComponentInParent<Pipeline>();
#else
                    _pipeline = GetComponent<Pipeline>() ?? GetComponentInParent<Pipeline>(true);
#endif
                }

                if (_pipeline) 
                    return _pipeline;
                
                // cannot find a pipeline. Look for a Pipeline Extension
                var pipelineExt = GetComponentInParent<IPipelineExtension>();
                _pipeline = pipelineExt.GetPipeline();

                return _pipeline;
            }
        }
        
        public Actor GetActor() => Pipeline.actor;
        public Instructor GetInstructor() => Pipeline.instructor;
        
        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"(Pipeline '{Pipeline.name}') " + message);
    }
}
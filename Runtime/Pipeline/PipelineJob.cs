using System.Collections.Generic;

namespace OmiLAXR.Pipeline
{
    public abstract class PipelineJob
    {
        public readonly List<PipelineJob> jobs = new List<PipelineJob>();

        public abstract PipelineData Pass(PipelineData data);
    }
}
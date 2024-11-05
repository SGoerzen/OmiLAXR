using System;

namespace OmiLAXR.Composers
{
    public struct PipelineInfo
    {
        public readonly string Name;
        public readonly Type Type;
        public readonly ActorDataProvider[] ActorDataProviders;

        public PipelineInfo(Pipeline pipeline)
        {
            Name = pipeline.name;
            Type = pipeline.GetType();
            ActorDataProviders = pipeline.ActorDataProviders;
        }
    }
}
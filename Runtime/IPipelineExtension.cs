namespace OmiLAXR
{
    public interface IPipelineExtension : IPipelineComponent
    {
        Pipeline GetPipeline();
        void Extend(Pipeline pipeline);
    }
}
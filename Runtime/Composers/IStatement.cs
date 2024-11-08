namespace OmiLAXR.Composers
{
    public interface IStatement
    {
        bool IsDiscarded();
        void Discard();

        string ToDataStandardString();
        PipelineInfo GetSenderPipelineInfo();
    }
}
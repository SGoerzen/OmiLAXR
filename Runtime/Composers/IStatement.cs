namespace OmiLAXR.Composers
{
    public interface IStatement
    {
        void SetComposer(IComposer composer);
        IComposer GetComposer();
        bool IsDiscarded();
        void Discard();

        string ToDataStandardString();
        PipelineInfo GetSenderPipelineInfo();
    }
}
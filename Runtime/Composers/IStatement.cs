
using OmiLAXR.TrackingBehaviours;

namespace OmiLAXR.Composers
{
    public interface IStatement
    {
        string GetOrigin();
        void SetOrigin(string origin);
        void SetComposer(IComposer composer);
        void SetOwner(ITrackingBehaviour trackingBehaviour);
        IComposer GetComposer();
        bool IsDiscarded();
        void Discard();

        [System.Obsolete("Use ToJsonString(bool pretty = false) instead.", true)]
        string ToDataStandardString();
        string ToJsonString(bool pretty = false);
        CsvFormat ToCsvFormat(bool flat = false);
        PipelineInfo GetSenderPipelineInfo();
    }
}
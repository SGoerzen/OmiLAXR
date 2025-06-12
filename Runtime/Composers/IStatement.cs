using OmiLAXR.TrackingBehaviours;
using OmiLAXR.Utils;

namespace OmiLAXR.Composers
{
    public interface IStatement
    {
        string GetOrigin();
        void SetOrigin(string origin);
        void SetComposer(IComposer composer);
        public bool IsFromComposer<T>() where T : IComposer;
        IComposer GetComposer();
        void SetOwner(ITrackingBehaviour trackingBehaviour);
        bool IsDiscarded();
        void Discard();
        IStatement Clone();
        [System.Obsolete("Use ToJsonString(bool pretty = false) instead.", true)]
        string ToDataStandardString();
        string ToJsonString(bool pretty = false);
        string ToShortString();
        CsvFormat ToCsvFormat(bool flatten = false);
        string GetTimestampString();
        PipelineInfo GetSenderPipelineInfo();
    }
}
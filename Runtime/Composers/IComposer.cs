using OmiLAXR.TrackingBehaviours;

namespace OmiLAXR.Composers
{
    public interface IComposer
    {
        event ComposerAction<IStatement, bool> AfterComposed;
        bool IsHigherComposer { get; }
        bool IsEnabled { get; }
        Author GetAuthor();
        string GetName();
        void SendStatement(ITrackingBehaviour statementOwner, IStatement statement, bool immediate = false);
    }
}
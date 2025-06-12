
namespace OmiLAXR.Composers
{
    public interface IComposer
    {
        event ComposerAction<IStatement> AfterComposed;
        bool IsHigherComposer { get; }
        Author GetAuthor();
        string GetName();
    }
}
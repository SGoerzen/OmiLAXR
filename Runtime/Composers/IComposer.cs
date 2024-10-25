namespace OmiLAXR.Composers
{
    public interface IComposer
    {
        event ComposerAction<IStatement, bool> AfterComposed;
        bool IsHigherComposer { get; }
        bool IsEnabled { get; }
        Author GetAuthor();
    }
}
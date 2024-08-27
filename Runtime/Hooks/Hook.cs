using OmiLAXR.Composers;

namespace OmiLAXR.Hooks
{
    public abstract class Hook : PipelineComponent
    {
        public abstract IStatement AfterCompose(IStatement statement);
    }
}
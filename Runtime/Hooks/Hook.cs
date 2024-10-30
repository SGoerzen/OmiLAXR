using OmiLAXR.Composers;

namespace OmiLAXR.Hooks
{
    public abstract class Hook : PipelineComponent
    {
        protected virtual void OnEnable()
        {
            
        }
        public abstract IStatement AfterCompose(IStatement statement);
    }
}
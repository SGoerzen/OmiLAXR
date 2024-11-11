using System.Linq;
using OmiLAXR.Composers;

namespace OmiLAXR.Hooks
{
    public abstract class Hook : PipelineComponent, IHook
    {
        protected virtual void OnEnable()
        {
            
        }
        public abstract IStatement AfterCompose(IStatement statement);
        
        protected TS Get<TS>(IStatement statement) where TS : ActorDataProvider
            => statement.GetSenderPipelineInfo().ActorDataProviders
                .FirstOrDefault(o => o.GetType() == typeof(TS) || o.GetType().IsSubclassOf(typeof(TS))) as TS;
    }
}
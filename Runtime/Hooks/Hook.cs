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
        
        protected TS GetProvider<TS>(IStatement statement, bool includeInactive = false) where TS : ActorDataProvider
            => statement.GetSenderPipelineInfo().ActorDataProviders?
                .FirstOrDefault(o => (includeInactive && !o.enabled) && (o.GetType() == typeof(TS) || o.GetType().IsSubclassOf(typeof(TS)))) as TS;
    }
}
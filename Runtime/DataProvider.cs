using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.Composers.HigherComposers;
using OmiLAXR.Endpoints;
using OmiLAXR.Hooks;
using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / 0) Pipelines / Data Provider")]
    [DefaultExecutionOrder(-1)]
    public class DataProvider : MonoBehaviour
    {
        public StatementComposer[] Composers { get; private set; }   
        public HigherStatementComposer<IStatement>[] HigherComposers { get; private set; }   
        public StatementHook[] Hooks { get; private set; }   
        public DataEndpoint[] DataEndpoints { get; private set; }   
        
        public T GetComposer<T>() where T : StatementComposer
            => Composers.OfType<T>().Select(composer => composer as T).FirstOrDefault();
        
        protected virtual void Awake()
        {
            // Find available composers
            Composers = GetComponentsInChildren<StatementComposer>().Where(c => c.enabled).ToArray();
            
            // Find available higher composers
            HigherComposers = Composers.Where(c => c.IsHigherComposer).Select(c => c as HigherStatementComposer<IStatement>).ToArray();
            
            // Find available hooks
            Hooks = GetComponentsInChildren<StatementHook>().Where(c => c.enabled).ToArray();
            
            // Find available data endpoints
            DataEndpoints = GetComponentsInChildren<DataEndpoint>();
        }

        protected void Start()
        {
            // 4 & 4.1) Start listening for composers
            foreach (var composer in Composers)
            {
                composer.afterComposed += HandleStatement;
            }
        }

        private void HandleStatement(IStatement statement)
        {
            // 4.1) Start listening for higher composers
            foreach (var composer in HigherComposers)
            {
                composer.LookFor(statement);   
            }
            
            foreach (var hook in Hooks.Where(hook => hook.enabled))
            {
                statement = hook.AfterCompose(statement);
                if (statement.IsDiscarded())
                    return;
            }

            foreach (var dp in DataEndpoints)
            {
                dp.SendStatement(statement);
            }
        }

        public static DataProvider GetAll() => FindObjectOfType<DataProvider>();

      
    }
}
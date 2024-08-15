using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
using OmiLAXR.Hooks;
using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / 0) Pipelines / Data Provider")]
    [DefaultExecutionOrder(-1)]
    public class DataProvider : MonoBehaviour
    {
        [HideInInspector] public List<StatementComposer> composers;
        [HideInInspector] public List<StatementHook> hooks;
        [HideInInspector] public List<DataEndpoint> dataEndpoints;

        public T GetComposer<T>() where T : StatementComposer
            => composers.OfType<T>().Select(composer => composer as T).FirstOrDefault();
        
        protected virtual void Awake()
        {
            // Find available composers
            composers = GetComponentsInChildren<StatementComposer>().ToList();
            
            // Find available hooks
            hooks = GetComponentsInChildren<StatementHook>().ToList();
            
            // Find available data endpoints
            dataEndpoints = GetComponentsInChildren<DataEndpoint>().ToList();
        }

        protected void Start()
        {
            // 4) Start listening for composers
            foreach (var composer in composers)
            {
                composer.afterComposed += statement =>
                {
                    statement = hooks.Where(h => h.enabled).Aggregate(statement, (current, hook) => hook.AfterCompose(current));
                    foreach (var dp in dataEndpoints)
                    {
                        dp.SendStatement(statement);
                    }
                };
            }
        }

        public static DataProvider GetAll() => FindObjectOfType<DataProvider>();

      
    }
}
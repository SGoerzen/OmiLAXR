using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.Composers.HigherComposers;
using OmiLAXR.Endpoints;
using OmiLAXR.Hooks;
using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / 0) Data Providers / Data Provider")]
    [DefaultExecutionOrder(-1)]
    public class DataProvider : MonoBehaviour
    {
        public readonly List<IComposer> Composers = new List<IComposer>();

        public readonly List<HigherComposer<IStatement>> HigherComposers =
            new List<HigherComposer<IStatement>>();

        public readonly List<Hook> Hooks = new List<Hook>();
        public readonly List<Endpoint> Endpoints = new List<Endpoint>();   
        
        public T GetComposer<T>() where T : IComposer
            => Composers.OfType<T>().FirstOrDefault();
        
        protected virtual void Awake()
        {
            // Find available composers
            var composers = GetComponentsInChildren<IComposer>().Where(c => c.IsEnabled);
            Composers.AddRange(composers);
            
            // Find available higher composers
            HigherComposers.AddRange(Composers.Where(c => c.IsHigherComposer)
                .Select(c => c as HigherComposer<IStatement>));
            
            // Find available hooks
            Hooks.AddRange(GetComponentsInChildren<Hook>().Where(c => c.enabled));
            
            // Find available data endpoints
            Endpoints.AddRange(GetComponentsInChildren<Endpoint>());
        }

        protected void Start()
        {
            // 4 & 4.1) Start listening for composers
            foreach (var composer in Composers)
            {
                composer.AfterComposed += HandleStatement;
            }
        }

        private void HandleStatement(IComposer sender, IStatement statement, bool sendImmediate)
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
            
            foreach (var dp in Endpoints)
            {
                if (sendImmediate)
                {
                    dp.SendStatementImmediate(statement);
                    continue;
                }
                dp.SendStatement(statement);
            }
            
            Debug.Log($"Sent to {Endpoints.Count} endpoints");
        }

        public static DataProvider GetAll() => FindObjectOfType<DataProvider>();

      
    }
}
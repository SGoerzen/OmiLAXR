using System.Collections.Generic;
using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
using OmiLAXR.Hooks;
using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-1)]
    public abstract class DataProviderExtension<T> : PipelineComponent, IDataProviderExtension
        where T : DataProvider
    {
        public readonly List<IComposer> Composers = new List<IComposer>();
        public readonly List<Hook> Hooks = new List<Hook>();
        public readonly List<Endpoint> Endpoints = new List<Endpoint>();
        
        public T DataProvider { get; protected set; }
        public DataProvider GetDataProvider() => DataProvider;

        private void Awake()
        {
            var pipeline = FindObject<T>();
            Extend(pipeline);
        }

        public void Extend(DataProvider dataProvider)
        {
            DataProvider = (T)dataProvider;

            var composers = gameObject.GetComponentsInChildren<IComposer>();
            var hooks = gameObject.GetComponentsInChildren<Hook>();
            var endpoints = gameObject.GetComponentsInChildren<Endpoint>();
            
            Composers.AddRange(composers);
            Hooks.AddRange(hooks);
            Endpoints.AddRange(endpoints);
            
            DataProvider.Composers.AddRange(composers);
            DataProvider.Hooks.AddRange(hooks);
            DataProvider.Endpoints.AddRange(endpoints);
            DataProvider.Extensions.Add(this);

            DebugLog.OmiLAXR.Print("Extended data provider " + typeof(T));
        }
    }
}
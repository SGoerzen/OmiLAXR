using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
using OmiLAXR.Hooks;
using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-1)]
    public abstract class DataProviderExtension<T> : MonoBehaviour
        where T : DataProvider
    {
        protected T DataProvider;
        private void Awake()
        {
            DataProvider = FindObjectOfType<T>();
            Extend(DataProvider);
            DebugLog.OmiLAXR.Print("Extended data provider " + typeof(T));
        }

        protected void Add(Composer composer)  
            => this.DataProvider.Composers.Add(composer);
        
        protected void Add(Hook hook)
            => this.DataProvider.Hooks.Add(hook);

        protected void Add(Endpoint endpoint)
            => this.DataProvider.Endpoints.Add(endpoint);
        
        protected abstract void Extend(T pipeline);
    }
}
using System.Linq;
using OmiLAXR.Data;
using OmiLAXR.Listeners;
using OmiLAXR.Pipelines;
using UnityEngine;

namespace OmiLAXR
{
    [DisallowMultipleComponent]
    [AddComponentMenu("OmiLAXR / Tracking / Main Tracking Behaviour")]
    public class MainTrackingBehaviour : MonoBehaviour
    {
        private static MainTrackingBehaviour _instance;
        //[Description("Collection of tracking subsystems.")]
        //public readonly TrackingSubSystems SubSystems = new TrackingSubSystems();

        /// <summary>
        /// Singleton instance of the MainTrackingBehavior. Only one can exist at a time.
        /// </summary>
        public static MainTrackingBehaviour Instance
            => _instance ??= FindObjectOfType<MainTrackingBehaviour>();
        
        [HideInInspector]
        public Pipeline[] pipelines;
        [HideInInspector]
        public Listener[] listeners;
        [HideInInspector]
        public DataProvider[] dataProviders;
        [HideInInspector]
        public TrackingBehaviour[] trackingBehaviours;

        public T GetListener<T>() where T : Listener
            => listeners.OfType<T>().Select(listener => listener as T).FirstOrDefault();

        public T GetPipeline<T>() where T : Pipeline
            => pipelines.OfType<T>().Select(pipeline => pipeline as T).FirstOrDefault();

        public T GetDataProvider<T>() where T : DataProvider
            => dataProviders.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetTrackingBehaviour<T>() where T : TrackingBehaviour
            => trackingBehaviours.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        private void Awake()
        {
            // Find available pipelines
            pipelines = FindObjectsOfType<Pipeline>();
            Debug.Log("Found " + pipelines.Length + " pipelines.");

            // Find available listeners
            listeners = FindObjectsOfType<Listener>();
            Debug.Log("Found " + listeners.Length + " listeners.");
            
            // Find available data providers
            dataProviders = FindObjectsOfType<DataProvider>();
            Debug.Log("Found " + dataProviders.Length + " data providers.");
            
            // Start listening for events
            foreach (var listener in listeners)
            {
                listener.StartListening();
            }
        }

        public void Register(params GameObject[] gameObjects)
        {

        }

        public void SendToDataProviders()
        {
            
        }
    }

}
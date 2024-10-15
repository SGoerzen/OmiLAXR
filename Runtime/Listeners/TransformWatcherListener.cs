using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / <Transform Watcher> Objects Listener")]
    [Description("Provides all <TransformWatcher> components to pipeline.")]
    public class TransformWatcherListener : Listener
    {
        public override void StartListening()
        {
            var tws = FindObjectsOfType<TransformWatcher>();
            Found(tws);
        }
    }
}
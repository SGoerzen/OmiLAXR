using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / <TransformWatcher> Objects Listener")]
    [Description("Provides all <TransformWatcher> components to pipeline.")]
    public class TransformWatcherListener : Listener
    {
        public override void StartListening()
        {
            Found(FindObjects<TransformWatcher>());
        }
    }
}
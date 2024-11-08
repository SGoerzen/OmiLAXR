using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / <GameObjectStateWatcher> Objects Listener")]
    [Description("Provides all <GameObjectStateWatcher> components to pipeline.")]
    public class GameObjectStateWatcherListener : Listener
    {
        public override void StartListening()
        {
            Found(FindObjects<GameObjectStateWatcher>());
        }
    }
}
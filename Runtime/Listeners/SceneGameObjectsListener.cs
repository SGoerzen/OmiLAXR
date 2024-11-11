using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / Scene GameObjects Listener")]
    [Description("Provides all GameObjects to pipeline.")]
    public class SceneGameObjectsListener : Listener
    {
        public override void StartListening()
        {
            Found(FindObjects<GameObject>());
        }
    }
}
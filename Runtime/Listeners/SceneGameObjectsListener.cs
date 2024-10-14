using UnityEngine;

namespace OmiLAXR.Listeners
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / Scene GameObjects Listener")]
    public class SceneGameObjectsListener : Listener
    {
        public override void StartListening()
        {
            Found(FindObjectsOfType<GameObject>());
        }
    }
}
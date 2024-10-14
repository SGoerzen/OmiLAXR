using UnityEngine;

namespace OmiLAXR.Listeners.Learner
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / Main <Camera> Transform Listener")]
    public class MainCameraTransformListener : Listener
    {
        public override void StartListening()
        {
            if (!Camera.main)
                return;
            var go = Camera.main.gameObject;
            var tw = go.GetComponent<TransformWatcher>() ?? go.AddComponent<TransformWatcher>();
            Found(tw);
        }
    }
}
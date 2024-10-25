using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners.Learner
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / Main <Camera> Transform Listener")]
    [Description("Provides <TransformWatcher> of Camera.main if it exists and add the component if not.")]
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
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners.Learner
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / Main <Camera> Transform Listener")]
    [Description("Provides <TransformWatcher> of Camera.main if it exists and add the component if not.")]
    public class MainCameraTransformListener : Listener
    {
        /// <summary>
        /// Minimum position change (in units) required to trigger the position change event.
        /// </summary>
        [Tooltip("Minimum position change (in units) required to trigger events")]
        public float positionThreshold = .5f;
        
        /// <summary>
        /// Minimum rotation change (in degrees) required to trigger the rotation change event.
        /// </summary>
        [Tooltip("Minimum rotation change (in degrees) required to trigger events")]
        public float rotationThreshold = 1.0f;
        
        /// <summary>
        /// Minimum scale change required to trigger the scale change event.
        /// </summary>
        [Tooltip("Minimum scale change required to trigger events")]
        public float scaleThreshold = 0.1f;
        
        public override void StartListening()
        {
            if (!Camera.main)
                return;
            var go = Camera.main.gameObject;
            var tw = go.GetComponent<TransformWatcher>() ?? go.AddComponent<TransformWatcher>();
            tw.positionThreshold = positionThreshold;
            tw.rotationThreshold = rotationThreshold;
            tw.scaleThreshold = scaleThreshold;
            Found(tw);
        }
    }
}
using UnityEngine;

namespace OmiLAXR.Listeners
{
    public class AutoListener<T> : Listener where T : Object
    {
        public override void StartListening()
        {
            Detect<T>();
        }
    }
}
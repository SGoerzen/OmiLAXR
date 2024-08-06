using UnityEngine.UI;

namespace OmiLAXR.Listeners
{
    public class SelectableObjectsListener : Listener
    {
        public bool includeInactive = true;

        public override void StartListening()
        {
            var selectables = FindObjectsOfType<Selectable>(includeInactive);
            Pipe(selectables);
        }
    }
}
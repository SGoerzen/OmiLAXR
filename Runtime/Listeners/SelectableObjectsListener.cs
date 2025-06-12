using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace OmiLAXR.Listeners
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / <Selectable> Objects Listener")]
    [Description("Provides all <Selectable> components to pipeline.")]
    public class SelectableObjectsListener : Listener
    {
        public bool includeInactive = true;
        public bool addInteractionEventHandler = true;

        public override void StartListening()
        {
            var selectables = FindObjects<Selectable>(includeInactive);
            if (addInteractionEventHandler)
            {
                foreach (var selectable in selectables)
                {
                    if (!selectable.GetComponent<InteractionEventHandler>())
                        selectable.gameObject.AddComponent<InteractionEventHandler>();
                }
            }
            Found(selectables);
        }
    }
}
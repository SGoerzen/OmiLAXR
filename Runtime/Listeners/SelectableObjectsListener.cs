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

        public override void StartListening()
        {
            var selectables = FindObjectsOfType<Selectable>(includeInactive);
            Found(selectables);
        }
    }
}
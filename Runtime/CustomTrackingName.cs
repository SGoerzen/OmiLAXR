using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Custom Tracking Name")]
    public class CustomTrackingName : MonoBehaviour
    {
        [Tooltip("Alternative tracking name.")]
        public string customTrackingName = "";
    }

    public static class Object_Ext_CustomTrackingName
    {
        public static string GetTrackingName(this Object obj)
        {
            if (obj.GetType() == typeof(GameObject))
            {
                var go = obj as GameObject;
                var customTrackingNameComp = go.GetComponent<CustomTrackingName>();
                return customTrackingNameComp ? customTrackingNameComp.customTrackingName : go.name;
            }
            else if (obj.GetType() == typeof(Component))
            {
                var comp = obj as Component;
                var customTrackingNameComp = comp.GetComponent<CustomTrackingName>();
                return customTrackingNameComp ? customTrackingNameComp.customTrackingName : comp.gameObject.name;
            }

            return obj.name;
        }
    }
}
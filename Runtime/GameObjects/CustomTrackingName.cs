using OmiLAXR.Extensions;
using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Game Objects / Custom Tracking Name")]
    [DisallowMultipleComponent]
    public class CustomTrackingName : MonoBehaviour
    {
        [Tooltip("Alternative tracking name.")]
        public string customTrackingName = "";
    }

    public static class Object_Ext_CustomTrackingName
    {
        public static CustomTrackingName GetCustomTrackingNameComponent(this Object obj)
        {
            if (obj is Component comp)
            {
                return comp.GetComponent<CustomTrackingName>();
            }
            return ((GameObject)obj).GetComponent<CustomTrackingName>();
        }
        
        public static string GetTrackingName(this Object obj)
        {
            var customTrackingNameComp = GetCustomTrackingNameComponent(obj);

            if (customTrackingNameComp)
                return customTrackingNameComp.customTrackingName;
            
            var trackingNameBehaviour = GlobalSettings.Instance.trackingNameBehaviour;
            return trackingNameBehaviour == GlobalSettings.TrackingNameBehaviour.HierarchyTreePath ? obj.GetFullHierarchyPath() : obj.name;
        }
    }
}
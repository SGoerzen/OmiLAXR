using System;
using System.Linq;
using OmiLAXR.Filters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace OmiLAXR.Tests.Filters
{
    [AddComponentMenu("OmiLAXR / 2) Filters / Unity Components Filter")]
    public class UnityComponentsFilter : Filter
    {
        private static readonly Type[] ForbiddenTypes = new []
        {
            typeof(PipelineComponent),
            typeof(Pipeline),
            typeof(Light),
            typeof(EventSystem),
            typeof(StandaloneInputModule),
            typeof(Canvas),
            typeof(TextMeshPro),
            typeof(TextMeshProUGUI),
        };
        public override Object[] Pass(Object[] gos)
        {
            return gos.Where(IsAllowedObject).ToArray();
        }

        public static bool IsAllowedType(Type type)
        {
            return !ForbiddenTypes.Any(t => type == t || type.IsSubclassOf(t));
        }

        public static bool IsAllowedObject(Object obj)
        {
            var type = obj.GetType();

            if (typeof(GameObject) != type) 
                return IsAllowedType(type);
            
            var go = obj as GameObject;

            if (!go)
                return false;
            
            var components = go.GetComponents<Component>();
            return components.All(c => c != null && IsAllowedType(c.GetType()));
        }
    }
}
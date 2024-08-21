using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace OmiLAXR.Filters
{
    [AddComponentMenu("OmiLAXR / 2) Filters / Unity Components Filter")]
    public class UnityComponentsFilter : Filter
    {
        private static readonly Type[] ForbiddenTypes = new []
        {
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
            => !ForbiddenTypes.Contains(type);

        public static bool IsAllowedObject(Object obj)
        {
            var type = obj.GetType();

            if (typeof(GameObject) != type) 
                return IsAllowedType(type);
            
            var go = obj as GameObject;

            if (!go)
                return false;
            
            var components = go.GetComponents<Component>();
            return components.Any(c => IsAllowedType(c.GetType()));

        }
    }
}
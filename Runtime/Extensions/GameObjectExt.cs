using UnityEngine;

namespace OmiLAXR.Extensions
{
    public static class GameObjectExt
    {
        public static T EnsureComponent<T>(this GameObject gameObject)
            where T : Component
            => gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }
}
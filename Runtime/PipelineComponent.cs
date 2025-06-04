using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR
{
    public abstract class PipelineComponent : MonoBehaviour, IPipelineComponent
    {
        /// <summary>
        /// Uses FindObjectOfType(includeInactive) or FindFirstObjectByType() depending on Unity version.
        /// </summary>
        /// <param name="includeInactive">Includes inactive GameObjects.</param>
        /// <typeparam name="T">Type of target object.</typeparam>
        /// <returns>Object found by type.</returns>
        protected static T FindObject<T>(bool includeInactive = false) where T : Object 
#if UNITY_2023_1_OR_NEWER
            => FindFirstObjectByType<T>(includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude);
#else 
            => FindObjectOfType<T>(includeInactive);
#endif

        /// <summary>
        /// Uses FindObjectOfType(includeInactive) or FindFirstObjectByType() depending on Unity version.
        /// </summary>
        /// <param name="includeInactive">Includes inactive GameObjects.</param>
        /// <typeparam name="T">Type of target object.</typeparam>
        /// <returns>Object found by type.</returns>
        protected static T[] FindObjects<T>(bool includeInactive = false) where T : Object
        {
#if UNITY_2023_1_OR_NEWER
    return FindObjectsByType<T>(
        includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude,
        FindObjectsSortMode.None
    );
#else
            return FindObjectsOfType<T>()
                .Where(o =>
                {
                    if (!o) return false;

                    if (includeInactive)
                        return true;

                    if (o is GameObject go)
                        return go.activeSelf;

                    if (o is Component c)
                        return c.gameObject.activeSelf;

                    return true; // fallback if neither GameObject nor Component
                })
                .ToArray();
#endif
        }

        /// <summary>
        /// Returns singleton by different code style depending on Unity version.
        /// </summary>
        /// <param name="instance">private field of instance.</param>
        /// <typeparam name="T">Type of the singleton.</typeparam>
        /// <returns>Reference to the singleton instance.</returns>
        protected static T GetInstance<T>(ref T instance) where T : Object
        {
#if UNITY_2023_1_OR_NEWER
            return instance ??= FindFirstObjectByType<T>();
#else 
            if (instance == null)
                instance = FindObjectOfType<T>();
            return instance;
#endif
        }
    
    }
}
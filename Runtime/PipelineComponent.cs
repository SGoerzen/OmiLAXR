using System.Linq;
using UnityEngine;

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
#if UNITY_2019
             => FindObjectOfType<T>();
#elif UNITY_6000
            => FindFirstObjectByType<T>();
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
#if UNITY_2019
            return FindObjectsOfType<T>().Where(o => o && ((o as GameObject).activeSelf || includeInactive)).ToArray();
#elif UNITY_6000
            return FindObjectsByType<T>(includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            return FindObjectsOfType<T>(includeInactive);
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
#if UNITY_2019
            if (instance == null)
                instance = FindObjectOfType<T>();
            return instance;
#elif UNITY_6000
            return instance ??= FindFirstObjectByType<T>();
#else 
            return instance ??= FindObjectOfType<T>();
#endif
        }
    
    }
}
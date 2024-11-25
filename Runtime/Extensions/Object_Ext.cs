using System.Linq;
using UnityEngine;

namespace OmiLAXR.Extensions
{
    public static class Object_Ext
    {
        /// <summary>
        /// Gets the full hierarchy path of the GameObject.
        /// </summary>
        public static string GetFullHierarchyPath(this Object obj)
        {
            GameObject gameObject;

            var isComponent = obj is Component comp;
            if (isComponent)
            {
                gameObject = ((Component)obj).gameObject;
            }
            else
            {
                gameObject = obj as GameObject;
            }
            
            if (gameObject == null)
                return string.Empty;
            
            var path = gameObject.name;
            var current = gameObject.transform.parent;

            while (current != null)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }

            if (isComponent)
            {
                path += "/" + ((Component)obj).GetType().Name;
            }

            return path;
        }
        /// <summary>
        /// Removes a specific type T from objects. It checks if the type is equal, if it's a subclass, or if a component of the type exists.
        /// </summary>
        /// <param name="objects">Array of objects to filter</param>
        /// <typeparam name="T">Target type to exclude</typeparam>
        /// <returns>An array without objects of type T</returns>
        public static Object[] Exclude<T>(this Object[] objects) where T : Object
        {
            var gameObjectType = typeof(GameObject);
            var componentType = typeof(Component);
            var targetType = typeof(T);

            return objects.Where(o =>
            {
                var objectType = o.GetType();

                // Direct match or subclass of T
                if (objectType == targetType || objectType.IsSubclassOf(targetType))
                    return false;

                // Check if the object is a Component and has the target type as a component
                if (objectType == componentType || objectType.IsSubclassOf(componentType))
                {
                    var component = o as Component;
                    if (component && component.GetComponent<T>())
                        return false;
                }
                // Check if the object is a GameObject and has the target type as a component
                else if (objectType == gameObjectType || objectType.IsSubclassOf(gameObjectType))
                {
                    var gameObject = o as GameObject;
                    if (gameObject && gameObject.GetComponent<T>())
                        return false;
                }

                return true;
            }).ToArray();
        }
    }
}
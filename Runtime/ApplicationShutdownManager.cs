/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Manages the orderly shutdown of components when the application quits.
    /// This singleton manager ensures that all registered MonoBehaviour components
    /// have their OnAppQuit methods called in the correct order during application shutdown.
    /// </summary>
    /// <remarks>
    /// Components can register themselves to receive shutdown notifications and specify
    /// their shutdown order using the ShutdownOrderAttribute. This is particularly useful
    /// for cleaning up resources, saving state, or performing other cleanup operations
    /// in a controlled manner when the application terminates.
    /// </remarks>
    public class ApplicationShutdownManager : MonoBehaviour
    {
        /// <summary>
        /// Static list containing all MonoBehaviour components that have registered
        /// for shutdown notifications. These components will have their OnAppQuit
        /// methods called when the application terminates.
        /// </summary>
        private static readonly List<MonoBehaviour> QuitHandlers = new List<MonoBehaviour>();

        /// <summary>
        /// Registers a MonoBehaviour component to receive shutdown notifications.
        /// The component must implement a private OnAppQuit method to be eligible for registration.
        /// </summary>
        /// <param name="target">The MonoBehaviour component to register for shutdown notifications</param>
        /// <remarks>
        /// If the target component doesn't implement an OnAppQuit method, a warning is logged
        /// and the component is not registered. Duplicate registrations are automatically prevented.
        /// </remarks>
        public static void Register(MonoBehaviour target)
        {
            var type = target.GetType();
            var onAppQuitMethod = GetShutdownMethod(type);
            
            // Verify that the component implements the required OnAppQuit method
            if (onAppQuitMethod == null)
            {
                DebugLog.OmiLAXR.Warning($"[ApplicationShutdownManager] OnAppQuitMethod is not implemented in <{type.Name}>.");
                return;
            }
            
            // Prevent duplicate registrations
            if (!QuitHandlers.Contains(target))
                QuitHandlers.Add(target);
        }

        /// <summary>
        /// Unregisters a MonoBehaviour component from receiving shutdown notifications.
        /// This should be called when a component no longer needs shutdown notifications,
        /// typically in the component's OnDestroy method.
        /// </summary>
        /// <param name="target">The MonoBehaviour component to unregister</param>
        public static void Unregister(MonoBehaviour target)
        {
            if (QuitHandlers.Contains(target)) 
                QuitHandlers.Remove(target);
        }

        /// <summary>
        /// Retrieves the shutdown order for a given type by examining its ShutdownOrderAttribute.
        /// Components with lower order values will be shut down before those with higher values.
        /// </summary>
        /// <param name="type">The type to examine for shutdown order information</param>
        /// <returns>The shutdown order value, or 0 if no ShutdownOrderAttribute is present</returns>
        private static int GetShutdownOrder(Type type)
        {
            var attr = (ShutdownOrderAttribute)Attribute.GetCustomAttribute(type, typeof(ShutdownOrderAttribute));
            return attr?.Order ?? 0; // Default order is 0 if no attribute is found
        }

        /// <summary>
        /// Uses reflection to find the private OnAppQuit method in the specified type.
        /// This method is expected to handle the component's shutdown logic.
        /// </summary>
        /// <param name="type">The type to search for the OnAppQuit method</param>
        /// <returns>MethodInfo for the OnAppQuit method, or null if not found</returns>
        private static MethodInfo GetShutdownMethod(Type type)
        {
            return type.GetMethod("OnAppQuit", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        
        /// <summary>
        /// Unity callback invoked when the application is about to quit.
        /// This method orchestrates the orderly shutdown of all registered components
        /// by calling their OnAppQuit methods in the order specified by their ShutdownOrderAttribute.
        /// </summary>
        /// <remarks>
        /// Components are sorted by their shutdown order (ascending), and each OnAppQuit method
        /// is invoked using reflection. Any exceptions during shutdown are caught and logged
        /// to prevent one failing component from blocking the shutdown of others.
        /// </remarks>
        private void OnApplicationQuit()
        {
            // Create an ordered list of shutdown handlers with their metadata
            var ordered = QuitHandlers
                .Select(t => new
                {
                    Target = t,                           // The component instance
                    Order = GetShutdownOrder(t.GetType()), // Shutdown order priority
                    Method = GetShutdownMethod(t.GetType()) // The OnAppQuit method to invoke
                })
                .Where(x => x.Method != null) // Only include components with valid OnAppQuit methods
                .OrderBy(x => x.Order)        // Sort by shutdown order (lowest first)
                .ToList();

            // Execute shutdown methods in the determined order
            foreach (var entry in ordered)
            {
                try
                {
                    // Invoke the OnAppQuit method on the target component
                    entry.Method.Invoke(entry.Target, null);
                }
                catch (Exception ex)
                {
                    // Log errors but continue with remaining shutdowns to ensure system stability
                    DebugLog.OmiLAXR.Error($"[ApplicationShutdownManager] {entry.Target.GetType().Name} failed: {ex.Message}.");
                }
            }
        }
    }
}
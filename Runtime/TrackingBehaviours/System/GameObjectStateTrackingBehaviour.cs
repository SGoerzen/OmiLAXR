/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Components;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.System
{
    /// <summary>
    /// Monitors GameObject state changes such as destruction, enabling, and disabling.
    /// Works in conjunction with GameObjectStateWatcher components to provide real-time notifications
    /// of GameObject lifecycle events throughout the application.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Game Objects State Tracking Behaviour")]
    [Description("Tracks if a game object state is changed (e.g. is destroyed).")]
    public class GameObjectStateTrackingBehaviour : TrackingBehaviour<GameObjectStateWatcher>
    {
        /// <summary>
        /// Enable tracking of GameObject destruction events.
        /// When true, OnDestroyedGameObject events will be triggered when watched objects are destroyed.
        /// </summary>
        public bool watchOnDestroyed = true;
        
        /// <summary>
        /// Enable tracking of GameObject enable events.
        /// When true, OnEnabledGameObject events will be triggered when watched objects become active.
        /// </summary>
        public bool watchOnEnabled = true;
        
        /// <summary>
        /// Enable tracking of GameObject disable events.
        /// When true, OnDisabledGameObject events will be triggered when watched objects become inactive.
        /// </summary>
        public bool watchOnDisabled = true;
        
        /// <summary>
        /// Event triggered when a watched GameObject is destroyed.
        /// Provides both the GameObjectStateWatcher and the destroyed GameObject reference.
        /// </summary>
        public readonly TrackingBehaviourEvent<GameObjectStateWatcher, GameObject> OnDestroyedGameObject = new TrackingBehaviourEvent<GameObjectStateWatcher, GameObject>(); 
        
        /// <summary>
        /// Event triggered when a watched GameObject becomes enabled/active.
        /// Includes the watcher component and the newly enabled GameObject.
        /// </summary>
        public readonly TrackingBehaviourEvent<GameObjectStateWatcher, GameObject> OnEnabledGameObject = new TrackingBehaviourEvent<GameObjectStateWatcher, GameObject>(); 
        
        /// <summary>
        /// Event triggered when a watched GameObject becomes disabled/inactive.
        /// Provides access to both the watcher and the disabled GameObject.
        /// </summary>
        public readonly TrackingBehaviourEvent<GameObjectStateWatcher, GameObject> OnDisabledGameObject = new TrackingBehaviourEvent<GameObjectStateWatcher, GameObject>(); 
        
        /// <summary>
        /// Called after GameObjectStateWatcher components are filtered and ready for monitoring.
        /// Sets up event bindings based on the enabled tracking options.
        /// </summary>
        /// <param name="gameObjects">Array of GameObjectStateWatcher components to monitor</param>
        protected override void AfterFilteredObjects(GameObjectStateWatcher[] gameObjects)
        {
            // Set up bindings for each detected GameObject state watcher
            foreach (var go in gameObjects)
            {
                // Bind to destruction events if enabled
                if (watchOnDestroyed)
                {
                    OnDestroyedGameObject.Bind(go.onDestroyed, g =>
                    {
                        // Forward the destruction event with watcher and GameObject references
                        OnDestroyedGameObject.Invoke(this, go, g);
                    });
                }

                // Bind to enable events if enabled
                if (watchOnEnabled)
                {
                    OnEnabledGameObject.Bind(go.onEnabled, g =>
                    {
                        // Forward the enable event with context
                        OnEnabledGameObject.Invoke(this, go, g);
                    });
                }

                // Bind to disable events if enabled
                if (watchOnDisabled)
                {
                    OnDisabledGameObject.Bind(go.onDisabled, g =>
                    {
                        // Forward the disable event with context
                        OnDisabledGameObject.Invoke(this, go, g);
                    });
                }
            }
        }
    }
}
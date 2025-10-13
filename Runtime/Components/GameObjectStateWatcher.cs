/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace OmiLAXR.Components
{
    /// <summary>
    /// Component that monitors and broadcasts GameObject state changes.
    /// Provides UnityEvents that can be subscribed to for handling destruction, enabling, and disabling events.
    /// </summary>
    public sealed class GameObjectStateWatcher : MonoBehaviour
    {
        /// <summary>
        /// UnityEvent invoked when the GameObject is destroyed.
        /// Passes the GameObject reference as a parameter.
        /// </summary>
        [FormerlySerializedAs("OnDestroyed")] 
        public UnityEvent<GameObject> onDestroyed = new UnityEvent<GameObject>();
        
        /// <summary>
        /// UnityEvent invoked when the GameObject is enabled.
        /// Passes the GameObject reference as a parameter.
        /// </summary>
        [FormerlySerializedAs("OnEnabled")] 
        public UnityEvent<GameObject> onEnabled = new UnityEvent<GameObject>();
        
        /// <summary>
        /// UnityEvent invoked when the GameObject is disabled.
        /// Passes the GameObject reference as a parameter.
        /// </summary>
        [FormerlySerializedAs("OnDisabled")] 
        public UnityEvent<GameObject> onDisabled = new UnityEvent<GameObject>();
        
        /// <summary>
        /// Called when the GameObject is destroyed.
        /// Triggers the onDestroyed event.
        /// </summary>
        private void OnDestroy()
        {
            onDestroyed?.Invoke(gameObject);
        }

        /// <summary>
        /// Called when the GameObject becomes enabled and active.
        /// Triggers the onEnabled event.
        /// </summary>
        private void OnEnable()
        {
            onEnabled?.Invoke(gameObject);
        }

        /// <summary>
        /// Called when the GameObject becomes disabled.
        /// Triggers the onDisabled event.
        /// </summary>
        private void OnDisable()
        {
            onDisabled?.Invoke(gameObject);
        }
    }
}
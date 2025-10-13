/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Components;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    /// <summary>
    /// Specialized listener that automatically detects and provides all GameObjectStateWatcher components in the scene.
    /// Used for tracking state changes and lifecycle events of GameObjects throughout the learning experience.
    /// GameObjectStateWatcher components monitor activation, deactivation, and other state transitions
    /// that may be relevant for learning analytics and user behavior tracking.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 1) Listeners / <GameObjectStateWatcher> Objects Listener")]
    [Description("Provides all <GameObjectStateWatcher> components to pipeline.")]
    public sealed class GameObjectStateWatcherListener : AutoListener<GameObjectStateWatcher>
    {
        // Implementation inherited from AutoListener<GameObjectStateWatcher>
        // Automatically detects all GameObjectStateWatcher components in the scene
    }
}
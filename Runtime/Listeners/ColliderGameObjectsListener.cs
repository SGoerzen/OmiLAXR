/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    /// <summary>
    /// Specialized listener that automatically detects and provides all Collider components in the scene.
    /// Used for tracking physics-based interactions, collision events, and spatial analytics.
    /// Inherits from AutoListener to automatically find all Collider components when listening starts.
    /// Useful for scenarios involving physics simulations, collision detection, and spatial learning analytics.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 1) Listeners / <Collider> Objects Listener")]
    [Description("Provides all <Collider> components to pipeline.")]
    public class ColliderGameObjectsListener : AutoListener<Collider>
    {
        // Implementation inherited from AutoListener<Collider>
        // Automatically detects all Collider components in the scene
    }
}
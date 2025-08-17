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
    /// Listener that automatically detects and provides all GameObjects in the current scene.
    /// This is a broad-scope listener that captures every GameObject, making it useful for
    /// comprehensive scene analysis, debugging, or scenarios where all scene objects need tracking.
    /// Inherits from AutoListener to automatically find all GameObjects when listening starts.
    /// Note: This listener may capture a large number of objects, so filtering is typically recommended.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 1) Listeners / Scene GameObjects Listener")]
    [Description("Provides all GameObjects to pipeline.")]
    public class SceneGameObjectsListener : AutoListener<GameObject>
    {
        // Implementation inherited from AutoListener<GameObject>
        // Automatically detects all GameObject instances in the scene
        // Use with appropriate filters to manage the potentially large number of results
    }
}
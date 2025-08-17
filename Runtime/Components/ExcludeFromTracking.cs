/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using UnityEngine;

namespace OmiLAXR.Components
{
    /// <summary>
    /// Component that marks a GameObject to be excluded from tracking operations.
    /// When attached to a GameObject, it signals that this object should not be
    /// included in tracking systems or data collection processes.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Game Objects / Exclude From Tracking")]
    [DisallowMultipleComponent]
    public class ExcludeFromTracking : MonoBehaviour
    {
        // This component acts as a marker - no additional implementation needed.
        // Its presence on a GameObject indicates exclusion from tracking.
    }
}
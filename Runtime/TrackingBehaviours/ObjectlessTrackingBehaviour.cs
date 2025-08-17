/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Deprecated tracking behavior for scenarios not requiring object filtering.
    /// Use TrackingBehaviour instead for new implementations.
    /// </summary>
    [Obsolete("Please use 'TrackingBehaviour' instead. This class will be removed in future version.")]
    public class ObjectlessTrackingBehaviour : TrackingBehaviour<Object>
    {
        /// <summary>
        /// Empty implementation as no object processing is required.
        /// </summary>
        /// <param name="objects">Array of objects (unused)</param>
        protected override void AfterFilteredObjects(Object[] objects)
        {
            // do nothing
        }
    }
}
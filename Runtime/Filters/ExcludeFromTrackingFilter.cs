/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Components;
using OmiLAXR.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Filters
{
    /// <summary>
    /// A filter that removes objects with the ExcludeFromTracking component from a collection of objects.
    /// </summary>
    /// <remarks>
    /// This filter is used to exclude specific objects from tracking based on their components.
    /// It can be added as a component in the Unity Inspector under the OmiLAXR / Filters category.
    /// </remarks>
    [AddComponentMenu("OmiLAXR / 2) Filters / <Exclude From Tracking> Filter")]
    [Description("Filters found objects that have the component <Exclude From Tracking>.")]
    public sealed class ExcludeFromTrackingFilter : Filter
    {
        /// <summary>
        /// Filters out objects that have the ExcludeFromTracking component.
        /// </summary>
        /// <param name="objects">The collection of objects to filter</param>
        /// <returns>A filtered collection of objects without ExcludeFromTracking components</returns>
        public override Object[] Pass(Object[] objects)
        {
            // Use the Exclude extension method to remove objects with ExcludeFromTracking component
            return objects.Exclude<ExcludeFromTracking>();
        }
    }
}
/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OmiLAXR.Components;
using UnityEngine;

namespace OmiLAXR.Filters
{
    /// <summary>
    /// Filter component that excludes objects based on a blacklist of tracking names.
    /// Uses the GetTrackingName() extension method to identify objects and filters out
    /// any objects whose tracking names appear in the configured blacklist.
    /// Useful for excluding specific objects from analytics tracking by name.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 2) Filters / Blacklist Filter")]
    [Description("Filters found objects by a blacklist of tracking names. The names are get with go.GetTrackingName().")]
    public class BlacklistFilter : Filter
    {
        /// <summary>
        /// List of tracking names to exclude from the filtered results.
        /// Objects whose GetTrackingName() value matches any entry in this list
        /// will be filtered out and not passed through to subsequent pipeline stages.
        /// </summary>
        public List<string> blacklist;
        
        /// <summary>
        /// Filters the input objects by excluding those whose tracking names appear in the blacklist.
        /// </summary>
        /// <param name="objects">Array of objects to filter</param>
        /// <returns>Array of objects that are not blacklisted</returns>
        public override Object[] Pass(Object[] objects)
        {
            // Filter out objects whose tracking name appears in the blacklist
            return objects.Where(go => !blacklist.Contains(go.GetTrackingName())).ToArray();
        }
    }
}
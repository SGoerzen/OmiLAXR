/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using UnityEngine.UI;

namespace OmiLAXR.Extensions
{
    /// <summary>
    /// Extension methods for Unity's Selectable UI components.
    /// Provides convenient methods for common UI interaction patterns.
    /// </summary>
    public static class SelectableExt
    {
        /// <summary>
        /// Enables or disables a selectable UI component with proper deselection.
        /// Ensures the component is properly deselected before changing its interactive state.
        /// </summary>
        /// <param name="selectable">The selectable component to modify</param>
        /// <param name="flag">True to disable the component, false to enable it</param>
        public static void SetDisabled(this Selectable selectable, bool flag)
        {
            selectable.OnDeselect(null);
            selectable.interactable = !flag;
        }
    }

}
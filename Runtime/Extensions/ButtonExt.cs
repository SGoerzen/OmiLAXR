/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using TMPro;
using UnityEngine.UI;

namespace OmiLAXR.Extensions
{
    /// <summary>
    /// Extension methods specifically for Unity's Button components.
    /// Provides utility methods for button state management and text retrieval.
    /// </summary>
    public static class ButtonExt
    {
        /// <summary>
        /// Enables or disables a button with proper deselection.
        /// Ensures the button loses focus before changing its interactive state.
        /// </summary>
        /// <param name="button">The button to modify</param>
        /// <param name="flag">True to disable the button, false to enable it</param>
        public static void SetDisabled(this Button button, bool flag)
        {
            button.OnDeselect(null);
            button.interactable = !flag;
        }
        
        /// <summary>
        /// Retrieves the text content from a button's TextMeshProUGUI component.
        /// Searches for a TextMeshProUGUI component in the button's children and returns its text.
        /// </summary>
        /// <param name="button">The button to get text from</param>
        /// <param name="defaultText">Text to return if no TextMeshProUGUI component is found</param>
        /// <returns>The button's text content, or the default text if no text component exists</returns>
        public static string GetTextOrDefault(this Button button, string defaultText = "")
        {
            var textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
            return !textMesh ? defaultText : textMesh.text;
        }
    }
}
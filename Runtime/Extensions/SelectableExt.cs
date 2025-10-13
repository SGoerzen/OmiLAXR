/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using OmiLAXR.Types;
using TMPro;
using UnityEngine;
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
        
        /// <summary>
        /// Retrieves the text content from a button's TextMeshProUGUI component.
        /// Searches for a TextMeshProUGUI component in the button's children and returns its text.
        /// </summary>
        /// <param name="selectable">The Selectable to get text from</param>
        /// <param name="defaultText">Text to return if no TextMeshProUGUI component is found</param>
        /// <returns>The button's text content, or the default text if no text component exists</returns>
        public static string GetTextOrDefault(this Selectable selectable, string defaultText = "")
        {
            var textMesh = selectable.GetComponentInChildren<TextMeshProUGUI>();
            return !textMesh ? defaultText : textMesh.text;
        }
        
        public static UiElementTypes GetUiElementType(this Selectable selectable)
        {
            switch (selectable)
            {
                case Button _:
                    return UiElementTypes.Button;
                case Toggle _:
                    return UiElementTypes.ToggleButton;
                case Slider _:
                    return UiElementTypes.Slider;
                case Dropdown _:
                    return UiElementTypes.Dropdown;
                case Scrollbar _:
                    return UiElementTypes.Scrollbar;
                case InputField inputField:
                    // Heuristik: PasswordField oder TextField?
                    if (inputField.contentType == InputField.ContentType.Password)
                        return UiElementTypes.PasswordField;
                    return UiElementTypes.TextField;
                default:
                    Debug.Log($"Unknown Selectable type: {selectable.GetType().Name}");
                    return UiElementTypes.Custom;
            }
        }
    }

}
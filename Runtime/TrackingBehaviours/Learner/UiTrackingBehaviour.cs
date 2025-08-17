/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using System.Linq;
using OmiLAXR.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    /// <summary>
    /// Tracks user interactions with UI components including buttons, sliders, dropdowns, toggles, input fields, and scrollbars.
    /// Supports both legacy and TextMeshPro UI components.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / UI Tracking Behaviour")]
    [Description("Tracks interaction with <Button>, <Slider>, <Dropdown>, <TMP_Dropdown>, <Toggle>, <InputField>, <TMP_InputField> and <Scrollbar> components.")]
    public class UiTrackingBehaviour : TrackingBehaviour<Selectable>
    {
        /// <summary>
        /// Event for button click interactions.
        /// </summary>
        [Gesture("UI"), Action("Click")]
        public readonly TrackingBehaviourEvent<Button> OnClickedButton = new TrackingBehaviourEvent<Button>();

        /// <summary>
        /// Event for slider value changes.
        /// </summary>
        [Gesture("UI"), Action("Change")] 
        public readonly TrackingBehaviourEvent<Slider, float> OnChangedSlider = new TrackingBehaviourEvent<Slider, float>();

        /// <summary>
        /// Event for dropdown selection changes.
        /// </summary>
        [Gesture("UI"), Action("Change")]
        public readonly TrackingBehaviourEvent<Selectable, int, string[]> OnChangedDropdown = new TrackingBehaviourEvent<Selectable, int, string[]>();

        /// <summary>
        /// Event for toggle state changes.
        /// </summary>
        [Gesture("UI"), Action("Change")] 
        public readonly TrackingBehaviourEvent<Toggle, bool> OnChangedToggle = new TrackingBehaviourEvent<Toggle, bool>();

        /// <summary>
        /// Event for input field text changes.
        /// </summary>
        [Gesture("UI"), Action("Change")] 
        public readonly TrackingBehaviourEvent<Selectable, string> OnChangedInputField = new TrackingBehaviourEvent<Selectable, string>();

        /// <summary>
        /// Event for scrollbar value changes.
        /// </summary>
        [Gesture("UI"), Action("Change")] 
        public readonly TrackingBehaviourEvent<Scrollbar, float> OnChangedScrollbar = new TrackingBehaviourEvent<Scrollbar, float>();

        /// <summary>
        /// Binds event handlers to filtered UI components based on their type.
        /// </summary>
        protected override void AfterFilteredObjects(Selectable[] selectables)
        {
            foreach (var selectable in selectables)
            {
                var type = selectable.GetType();
                var ieh = selectable.GetComponent<InteractionEventHandler>();
                
                if (type == typeof(Button))
                {
                    var button = (Button)selectable;
                    OnClickedButton.Bind(button.onClick, () =>
                    {
                        if (!ieh || ieh.IsHovering)
                            OnClickedButton.Invoke(this, button);
                    });
                }
                else if (type == typeof(Slider))
                {
                    var slider = (Slider)selectable;
                    OnChangedSlider.Bind(slider.onValueChanged, value =>
                    {
                        if (!ieh || ieh.IsPressing)
                            OnChangedSlider.Invoke(this, slider, value);
                    });
                }
                else if (type == typeof(Dropdown))
                {
                    var dropdown = (Dropdown)selectable;
                    var options = dropdown.options.Select(o => o.text).ToArray();
                    OnChangedDropdown.Bind(dropdown.onValueChanged, value =>
                    {
                        if (!ieh || ieh.IsHovering)
                            OnChangedDropdown.Invoke(this, dropdown, value, options);
                    });
                }
                else if (type == typeof(TMP_Dropdown))
                {
                    var dropdown = (TMP_Dropdown)selectable;
                    var options = dropdown.options.Select(o => o.text).ToArray();
                    OnChangedDropdown.Bind(dropdown.onValueChanged, value =>
                    {
                        if (!ieh || ieh.IsHovering)
                            OnChangedDropdown.Invoke(this, dropdown, value, options);
                    });
                }
                else if (type == typeof(Toggle))
                {
                    var toggle = (Toggle)selectable;
                    OnChangedToggle.Bind(toggle.onValueChanged, value =>
                    {
                        if (ieh || ieh.IsHovering)
                            OnChangedToggle.Invoke(this, toggle, value);
                    });
                }
                else if (type == typeof(InputField))
                {
                    var inputField = (InputField)selectable;
                    OnChangedInputField.Bind(inputField.onValueChanged, value =>
                    {
                        if (inputField.isFocused)
                            OnChangedInputField.Invoke(this, inputField, value);
                    });
                }
                else if (type == typeof(TMP_InputField))
                {
                    var inputField = (TMP_InputField)selectable;
                    OnChangedInputField.Bind(inputField.onValueChanged, value =>
                    {
                        if (inputField.isFocused)
                            OnChangedInputField.Invoke(this, inputField, value);
                    });
                }
                else if (type == typeof(Scrollbar))
                {
                    var scrollbar = (Scrollbar)selectable;
                    OnChangedScrollbar.Bind(scrollbar.onValueChanged, value =>
                    {
                        if (!ieh || ieh.IsHovering)
                            OnChangedScrollbar.Invoke(this, scrollbar, value);
                    });
                }
            }
        }
    }
}
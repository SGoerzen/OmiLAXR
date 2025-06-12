using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / UI Tracking Behaviour")]
    [Description("Tracks interaction with <Button>, <Slider>, <Dropdown>, <TMP_Dropdown>, <Toggle>, <InputField>, <TMP_InputField> and <Scrollbar> components.")]
    public class UiTrackingBehaviour : TrackingBehaviour<Selectable>
    {
        [Gesture("UI"), Action("Click")]
        public readonly TrackingBehaviourEvent<Button> OnClickedButton = new TrackingBehaviourEvent<Button>();

        [Gesture("UI"), Action("Change")] public readonly TrackingBehaviourEvent<Slider, float> OnChangedSlider =
            new TrackingBehaviourEvent<Slider, float>();

        [Gesture("UI"), Action("Change")]
        public readonly TrackingBehaviourEvent<Selectable, int, string[]> OnChangedDropdown =
            new TrackingBehaviourEvent<Selectable, int, string[]>();

        [Gesture("UI"), Action("Change")] public readonly TrackingBehaviourEvent<Toggle, bool> OnChangedToggle =
            new TrackingBehaviourEvent<Toggle, bool>();

        [Gesture("UI"), Action("Change")] public readonly TrackingBehaviourEvent<Selectable, string> OnChangedInputField =
            new TrackingBehaviourEvent<Selectable, string>();

        [Gesture("UI"), Action("Change")] public readonly TrackingBehaviourEvent<Scrollbar, float> OnChangedScrollbar =
            new TrackingBehaviourEvent<Scrollbar, float>();

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
                    OnChangedSlider.Bind(slider.onValueChanged,
                        value =>
                        {
                            if (!ieh || ieh.IsPressing)
                                OnChangedSlider.Invoke(this, slider, value);
                        });
                }
                else if (type == typeof(Dropdown))
                {
                    var dropdown = (Dropdown)selectable;
                    var options = dropdown.options.Select(o => o.text).ToArray();
                    OnChangedDropdown.Bind(dropdown.onValueChanged,
                        value =>
                        {
                            if (!ieh || ieh.IsHovering)
                                OnChangedDropdown.Invoke(this, dropdown, value, options);
                        });
                }
                else if (type == typeof(TMP_Dropdown))
                {
                    var dropdown = (TMP_Dropdown)selectable;
                    var options = dropdown.options.Select(o => o.text).ToArray();
                    OnChangedDropdown.Bind(dropdown.onValueChanged,
                        value =>
                        {
                            if (!ieh || ieh.IsHovering)
                                OnChangedDropdown.Invoke(this, dropdown, value, options);
                        });
                }
                else if (type == typeof(Toggle))
                {
                    var toggle = (Toggle)selectable;
                    OnChangedToggle.Bind(toggle.onValueChanged,
                        value =>
                        {
                            if (ieh || ieh.IsHovering)
                                OnChangedToggle.Invoke(this, toggle, value);
                        });
                }
                else if (type == typeof(InputField))
                {
                    var inputField = (InputField)selectable;
                    OnChangedInputField.Bind(inputField.onValueChanged,
                        value =>
                        {
                            if (inputField.isFocused)
                                OnChangedInputField.Invoke(this, inputField, value);
                        });
                }
                else if (type == typeof(TMP_InputField))
                {
                    var inputField = (TMP_InputField)selectable;
                    OnChangedInputField.Bind(inputField.onValueChanged,
                        value =>
                        {
                            if (inputField.isFocused)
                                OnChangedInputField.Invoke(this, inputField, value);
                        });
                }
                else if (type == typeof(Scrollbar))
                {
                    var scrollbar = (Scrollbar)selectable;
                    OnChangedScrollbar.Bind(scrollbar.onValueChanged,
                        value =>
                        {
                            if (!ieh || ieh.IsHovering)
                                OnChangedScrollbar.Invoke(this, scrollbar, value);
                        });
                }
            }
        }
    }
}
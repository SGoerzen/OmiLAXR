using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Action = Unity.Plastic.Antlr3.Runtime.Misc.Action;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / UI Tracking Behaviour")]
    public class UiTrackingBehaviour : TrackingBehaviour
    {
        [Gesture("UI"), Action("Click")]
        public TrackingBehaviourEvent<Button> OnClickedButton = new TrackingBehaviourEvent<Button>();

        [Gesture("UI"), Action("Change")] public TrackingBehaviourEvent<Slider, float> OnChangedSlider =
            new TrackingBehaviourEvent<Slider, float>();

        [Gesture("UI"), Action("Change")] public TrackingBehaviourEvent<Dropdown, int> OnChangedDropdown =
            new TrackingBehaviourEvent<Dropdown, int>();

        [Gesture("UI"), Action("Change")] public TrackingBehaviourEvent<Toggle, bool> OnChangedToggle =
            new TrackingBehaviourEvent<Toggle, bool>();

        [Gesture("UI"), Action("Change")] public TrackingBehaviourEvent<Selectable, string> OnChangedInputField =
            new TrackingBehaviourEvent<Selectable, string>();

        [Gesture("UI"), Action("Change")] public TrackingBehaviourEvent<Scrollbar, float> OnChangedScrollbar =
            new TrackingBehaviourEvent<Scrollbar, float>();

        protected override void AfterFilteredObjects(Object[] objects)
        {
            var selectables = Select<Selectable>(objects);

            foreach (var selectable in selectables)
            {
                var type = selectable.GetType();

                if (type == typeof(Button))
                {
                    var button = (Button)selectable;
                    OnClickedButton.Bind(button.onClick, () => { OnClickedButton.Invoke(this, button); });
                }
                else if (type == typeof(Slider))
                {
                    var slider = (Slider)selectable;
                    OnChangedSlider.Bind(slider.onValueChanged,
                        value => { OnChangedSlider.Invoke(this, slider, value); });
                }
                else if (type == typeof(Dropdown))
                {
                    var dropdown = (Dropdown)selectable;
                    OnChangedDropdown.Bind(dropdown.onValueChanged,
                        value => { OnChangedDropdown.Invoke(this, dropdown, value); });
                }
                else if (type == typeof(Toggle))
                {
                    var toggle = (Toggle)selectable;
                    OnChangedToggle.Bind(toggle.onValueChanged,
                        value => { OnChangedToggle.Invoke(this, toggle, value); });
                }
                else if (type == typeof(InputField))
                {
                    var inputField = (InputField)selectable;
                    OnChangedInputField.Bind(inputField.onValueChanged,
                        value => { OnChangedInputField.Invoke(this, inputField, value); });
                }
                else if (type == typeof(TMP_InputField))
                {
                    var inputField = (TMP_InputField)selectable;
                    OnChangedInputField.Bind(inputField.onValueChanged,
                        value => { OnChangedInputField.Invoke(this, inputField, value); });
                }
                else if (type == typeof(Scrollbar))
                {
                    var scrollbar = (Scrollbar)selectable;
                    OnChangedScrollbar.Bind(scrollbar.onValueChanged,
                        value => { OnChangedScrollbar.Invoke(this, scrollbar, value); });
                }
            }
        }
    }
}
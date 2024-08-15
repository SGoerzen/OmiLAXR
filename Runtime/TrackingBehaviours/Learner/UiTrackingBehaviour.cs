using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviors / UI Tracking Behavior")]
    public class UiTrackingBehaviour : TrackingBehaviour
    {
        public event TrackingBehaviourAction<Button> OnClickedButton;
        public event TrackingBehaviourAction<Slider, float> OnChangedSlider;

        protected override void AfterFilteredObjects(Object[] objects)
        {
            var selectables = Select<Selectable>(objects);
            
            foreach (var selectable in selectables)
            {
                var type = selectable.GetType();

                if (type == typeof(Button))
                {
                    var button = (Button)selectable;
                    button.onClick.AddListener(() =>
                    {
                        OnClickedButton?.Invoke(this, button);
                    });
                }
                else if (type == typeof(Slider))
                {
                    var slider = (Slider)selectable;
                    slider.onValueChanged.AddListener((value) =>
                    {
                        OnChangedSlider?.Invoke(this, slider, value);
                    });
                }
            }
        }
    }
}
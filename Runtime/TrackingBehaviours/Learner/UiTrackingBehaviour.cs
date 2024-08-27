using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / UI Tracking Behaviour")]
    public class UiTrackingBehaviour : TrackingBehaviour
    {
        [Gesture("UI"), Action("Click")]
        public TrackingBehaviourEvent<Button> OnClickedButton = new TrackingBehaviourEvent<Button>();

        [Gesture("UI"), Action("Change")]
        public TrackingBehaviourEvent<Slider, float> OnChangedSlider =
            new TrackingBehaviourEvent<Slider, float>();
        
        protected override void AfterFilteredObjects(Object[] objects)
        {
            var selectables = Select<Selectable>(objects);
            
            foreach (var selectable in selectables)
            {
                var type = selectable.GetType();

                if (type == typeof(Button))
                {
                    var button = (Button)selectable;
                    OnClickedButton.Bind(button.onClick, () =>
                    {
                        OnClickedButton?.Invoke(this, button);
                    });
                }
                else if (type == typeof(Slider))
                {
                    var slider = (Slider)selectable;
                    OnChangedSlider.Bind(slider.onValueChanged, value =>
                    {
                        OnChangedSlider.Invoke(this, slider, value);
                    });
                }
            }
        }
    }
}
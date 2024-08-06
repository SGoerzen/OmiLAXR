using System;
using System.Linq;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace OmiLAXR.Pipelines.Learner
{
    public class UiTrackingBehaviour : TrackingBehaviour
    {
        public event Action<Button> OnClickedButton;
        public event Action<Slider, float> OnChangedSlider;

        protected override void AfterFilteredObjects(Object[] objects)
        {
            var selectables = Select<Selectable>(objects);
            
            foreach (var selectable in selectables)
            {
                var type = selectable!.GetType();

                if (type == typeof(Button))
                {
                    var button = (Button)selectable;
                    button.onClick.AddListener(() =>
                    {
                        print("hello");
                        OnClickedButton?.Invoke(button);
                    });
                }
                else if (type == typeof(Slider))
                {
                    var slider = (Slider)selectable;
                    slider.onValueChanged.AddListener((value) =>
                    {
                        OnChangedSlider?.Invoke(slider, value);
                    });
                }
            }
        }
    }
}
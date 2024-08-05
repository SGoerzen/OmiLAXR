using UnityEngine;
using UnityEngine.UI;

namespace OmiLAXR.Pipelines.Learner
{
    public class UiTrackingBehaviour : TrackingBehaviour
    {
        public override void Listen(GameObject go)
        {
            var selectable = go.GetComponent<Selectable>();
            
            // Ignore if it is not a control
            if (!selectable)
                return;
            
            var type = selectable.GetType();

            if (type == typeof(Button))
            {
                var button = (Button)selectable;
                //button.onClick.AddListener(Button_OnClick);
            }
            else if (type == typeof(Slider))
            {
                var slider = (Slider)selectable;
                //slider.onValueChanged.AddListener(Slider_OnValueChanged);
            }
        }
    }
}
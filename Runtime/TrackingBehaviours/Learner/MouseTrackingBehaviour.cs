using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Pipelines.Learner
{
    public class MouseTrackingBehaviour : TrackingBehaviour
    {
        public event Action<string> OnClicked;
        public event Action<string> OnPressedDown;
        public event Action<float> OnScrolledWheel;
        
        private bool _isLeftDown;
        private bool _isRightDown;
        private bool _isWheelDown;
        
        private float _mouseWheel = 0;
        private static readonly string[] ButtonNames = { "left", "right", "middle" };
        
        private void HandleMouseClick(int index, ref bool wasDown)
        {
            var isDown = Input.GetMouseButton(index);
            var name = ButtonNames[index];

            switch (wasDown)
            {
                case false when isDown:
                    OnPressedDown?.Invoke(name);
                    break;
                case true when !isDown:
                    OnClicked?.Invoke(name);
                    break;
            }

            wasDown = isDown;
        }

        private void Update()
        {
            HandleMouseClick(0, ref _isLeftDown);
            HandleMouseClick(1, ref _isRightDown);
            HandleMouseClick(2, ref _isWheelDown);

            var curMouseWheel = Input.mouseScrollDelta.y;
            if (!Mathf.Approximately(_mouseWheel, curMouseWheel))
            {
                _mouseWheel = curMouseWheel;
                OnScrolledWheel?.Invoke(curMouseWheel);
            }
        }
    }
}
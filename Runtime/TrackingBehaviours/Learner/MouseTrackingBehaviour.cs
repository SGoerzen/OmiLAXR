using System;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviors /  Mouse Tracking Behavior")]
    public class MouseTrackingBehaviour : TrackingBehaviour
    {
        public struct MouseTrackingBehaviourArgs
        {
            public string mouseButton;
            public Vector3 mousePosition;

            public MouseTrackingBehaviourArgs(string name, Vector3 position)
            {
                mouseButton = name;
                mousePosition = position;
            }
        }
        
        public event TrackingBehaviourAction<MouseTrackingBehaviourArgs> OnClicked;
        public event TrackingBehaviourAction<MouseTrackingBehaviourArgs> OnPressedDown;
        public event TrackingBehaviourAction<MouseTrackingBehaviourArgs, float> OnScrolledWheel;
        
        private bool _isLeftDown;
        private bool _isRightDown;
        private bool _isWheelDown;
        
        private float _mouseWheel = 0;
        private static readonly string[] ButtonNames = { "left", "right", "middle" };
        
        private void HandleMouseClick(int index, ref bool wasDown, Vector3 position)
        {
            var isDown = Input.GetMouseButton(index);
            var n = ButtonNames[index];

            switch (wasDown)
            {
                case false when isDown:
                    OnPressedDown?.Invoke(this, new MouseTrackingBehaviourArgs(n, position));
                    break;
                case true when !isDown:
                    OnClicked?.Invoke(this, new MouseTrackingBehaviourArgs(n, position));
                    break;
            }

            wasDown = isDown;
        }

        private void Update()
        {
            var mousePos = Input.mousePosition;
            HandleMouseClick(0, ref _isLeftDown, mousePos);
            HandleMouseClick(1, ref _isRightDown, mousePos);
            HandleMouseClick(2, ref _isWheelDown, mousePos);
            
            var curMouseWheel = Input.mouseScrollDelta.y;
            if (!Mathf.Approximately(_mouseWheel, curMouseWheel))
            {
                _mouseWheel = curMouseWheel;
                OnScrolledWheel?.Invoke(this, new MouseTrackingBehaviourArgs("wheel", mousePos), curMouseWheel);
            }
        }
    }
}
using System.ComponentModel;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Mouse Tracking Behaviour")]
    [Description("Tracks mouse clicks and wheel.")]
    public class MouseTrackingBehaviour : EventTrackingBehaviour
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

        [Gesture("Mouse"), Action("Click")]
        public TrackingBehaviourEvent<MouseTrackingBehaviourArgs> OnClicked =
            new TrackingBehaviourEvent<MouseTrackingBehaviourArgs>();

        [Gesture("Mouse"), Action("Press")]
        public TrackingBehaviourEvent<MouseTrackingBehaviourArgs> OnPressedDown =
            new TrackingBehaviourEvent<MouseTrackingBehaviourArgs>();

        [Gesture("Mouse"), Action("Scroll")]
        public TrackingBehaviourEvent<MouseTrackingBehaviourArgs, float> OnScrolledWheel =
            new TrackingBehaviourEvent<MouseTrackingBehaviourArgs, float>();

        [Gesture("Mouse"), Action("Move")]
        public TrackingBehaviourEvent<Vector3, Vector3> OnMousePositionChanged =
            new TrackingBehaviourEvent<Vector3, Vector3>();

        private bool _isLeftDown;
        private bool _isRightDown;
        private bool _isWheelDown;

        public float movementThreshold = 3.0f; // Threshold in pixels
        private Vector3 _lastMousePosition;

        private float _mouseWheel = 0;
        public float mouseWheelThreshold = 0.5f;

        private static readonly string[] ButtonNames = { "left", "right", "middle" };

        private void Start()
        {
            _lastMousePosition = GetMousePosition();
        }

        private void HandleMouseClick(int index, ref bool wasDown, Vector3 position)
        {
            bool isDown = false;
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                switch (index)
                {
                    case 0: isDown = Mouse.current.leftButton.isPressed; break;
                    case 1: isDown = Mouse.current.rightButton.isPressed; break;
                    case 2: isDown = Mouse.current.middleButton.isPressed; break;
                }
            }
#else
            isDown = Input.GetMouseButton(index);
#endif

            var name = ButtonNames[index];

            switch (wasDown)
            {
                case false when isDown:
                    OnPressedDown?.Invoke(this, new MouseTrackingBehaviourArgs(name, position));
                    break;
                case true when !isDown:
                    OnClicked?.Invoke(this, new MouseTrackingBehaviourArgs(name, position));
                    break;
            }

            wasDown = isDown;
        }

        private void Update()
        {
            var mousePos = GetMousePosition();
            HandleMouseClick(0, ref _isLeftDown, mousePos);
            HandleMouseClick(1, ref _isRightDown, mousePos);
            HandleMouseClick(2, ref _isWheelDown, mousePos);

            // Detect mouse wheel
            float curMouseWheel = GetMouseScroll();
            float mouseWheelDis = Mathf.Abs(_mouseWheel - curMouseWheel);
            if (mouseWheelDis > mouseWheelThreshold)
            {
                _mouseWheel = curMouseWheel;
                OnScrolledWheel?.Invoke(this, new MouseTrackingBehaviourArgs("wheel", mousePos), curMouseWheel);
            }

            // Detect mouse move
            float distance = Vector3.Distance(mousePos, _lastMousePosition);
            if (distance > movementThreshold)
            {
                OnMousePositionChanged?.Invoke(this, mousePos, _lastMousePosition);
                _lastMousePosition = mousePos;
            }
        }

        private Vector3 GetMousePosition()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                var pos = Mouse.current.position.ReadValue();
                return new Vector3(pos.x, pos.y, 0f);
            }
            return Vector3.zero;
#else
            return Input.mousePosition;
#endif
        }

        private float GetMouseScroll()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current?.scroll.ReadValue().y ?? 0f;
#else
            return Input.mouseScrollDelta.y;
#endif
        }
    }
}
using System.ComponentModel;
using UnityEngine;

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
        public TrackingBehaviourEvent<Vector3> OnMousePositionChanged =
            new TrackingBehaviourEvent<Vector3>();
        
        private bool _isLeftDown;
        private bool _isRightDown;
        private bool _isWheelDown;

        public float movementThreshold = 3.0f; // Threshold in pixels
        private Vector3 _lastMousePosition;
        
        private float _mouseWheel = 0;
        public float mouseWheelThreshold = .5f; 

        private static readonly string[] ButtonNames = { "left", "right", "middle" };

        private void Start()
        {
            _lastMousePosition = Input.mousePosition;
        }

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
            
            // Detect mouse wheel
            var curMouseWheel = Input.mouseScrollDelta.y;
            var mouseWheelDis = Mathf.Abs(_mouseWheel - curMouseWheel);
            if (mouseWheelDis > mouseWheelThreshold)
            {
                _mouseWheel = curMouseWheel;
                OnScrolledWheel?.Invoke(this, new MouseTrackingBehaviourArgs("wheel", mousePos), curMouseWheel);
            }
            
            // Detect Mouse move
            // Calculate the distance between the last and current mouse positions
            var distance = Vector3.Distance(Input.mousePosition, _lastMousePosition);

            // Check if the distance exceeds the threshold
            if (distance > movementThreshold)
            {
                // Update last mouse position
                _lastMousePosition = Input.mousePosition;
                OnMousePositionChanged?.Invoke(this, Input.mousePosition);
            }
        }
    }
}
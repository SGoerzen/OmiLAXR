using System;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Keyboard Tracking Behaviour"),
     Description("Tracks keyboard presses and releases."),
    ]
    public class KeyboardTrackingBehaviour : TrackingBehaviour
    {
        public struct KeyboardTrackingBehaviourArgs
        {
            public readonly bool isDown;
            public readonly string key;

            public KeyboardTrackingBehaviourArgs(bool isDown, string key)
            {
                this.isDown = isDown;
                this.key = key;
            }
        }
        
        [Gesture("Keyboard"), Action("Pressed")]
        public TrackingBehaviourEvent<KeyboardTrackingBehaviourArgs> OnPressed =
            new TrackingBehaviourEvent<KeyboardTrackingBehaviourArgs>();
        
        private readonly Dictionary<KeyCode, bool> _wasDown = new Dictionary<KeyCode, bool>();

        private List<KeyCode> _keys = new List<KeyCode>();
        
        protected override void Awake()
        {
            base.Awake();
            
            // init all key states
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                // Only proceed if the keyCode is between KeyCode.A and KeyCode.Z or other relevant ranges
                if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z ||
                    keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9 ||
                    keyCode >= KeyCode.F1 && keyCode <= KeyCode.F15 ||
                    keyCode >= KeyCode.Keypad0 && keyCode <= KeyCode.Keypad9 ||
                    keyCode >= KeyCode.UpArrow && keyCode <= KeyCode.RightArrow ||
                    keyCode == KeyCode.Space || keyCode == KeyCode.Return ||
                    keyCode == KeyCode.Backspace || keyCode == KeyCode.Tab ||
                    keyCode == KeyCode.Escape || keyCode == KeyCode.Delete)
                {
                    _wasDown.TryAdd(keyCode, false);
                }
            }
            
            _keys = new List<KeyCode>(_wasDown.Keys);
        }

        private void Start()
        {
            
        }
        
        private void HandleKey(string name, bool isDown, ref bool wasDown)
        {
            switch (wasDown)
            {
                case false when isDown:
                    OnPressed?.Invoke(this, new KeyboardTrackingBehaviourArgs(true, name));
                    break;
                case true when !isDown:
                    OnPressed?.Invoke(this, new KeyboardTrackingBehaviourArgs(false, name));
                    break;
            }
            

            wasDown = isDown;
        }

        // Update is called once per frame
        private void Update()
        {
            foreach (var key in _keys)
            {
                var wasDown = _wasDown[key];
                HandleKey(key.ToString(), Input.GetKey(key), ref wasDown);
                _wasDown[key] = wasDown;
            }
        }
        
        protected override void AfterFilteredObjects(Object[] objects)
        {
            
        }
    }
}
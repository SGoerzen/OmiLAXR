using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

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
        
        private void Start()
        {
            // init all key states
            _keyCodes.ForEach(key => _wasDown.Add(key, false));
        }
        
        private readonly List<KeyCode> _keyCodes = new List<KeyCode>() {
            KeyCode.Escape,
            KeyCode.Return,
            KeyCode.W,
            KeyCode.S,
            KeyCode.D,
            KeyCode.A,
            KeyCode.N,
            KeyCode.P,
            KeyCode.R,
            KeyCode.M,
            KeyCode.Space,
            KeyCode.LeftShift
        };
        
        
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
            // handle at first any key
            //HandleKey("any", Input.anyKey, ref wasAnyKeyDown);

            _keyCodes.ForEach(key => {
                var wasDown = _wasDown[key];
                HandleKey(key.ToString(), Input.GetKey(key), ref wasDown);
                _wasDown[key] = wasDown;
            });
        }
        
        protected override void AfterFilteredObjects(Object[] objects)
        {
            
        }
    }
}
using System;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Keyboard Tracking Behaviour"),
     Description("Tracks keyboard presses and releases.")]
    public class KeyboardTrackingBehaviour : ObjectlessTrackingBehaviour
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

            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z ||
                    keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9 ||
                    keyCode >= KeyCode.F1 && keyCode <= KeyCode.F15 ||
                    keyCode >= KeyCode.Keypad0 && keyCode <= KeyCode.Keypad9 ||
                    keyCode >= KeyCode.UpArrow && keyCode <= KeyCode.RightArrow ||
                    keyCode == KeyCode.Space || keyCode == KeyCode.Return ||
                    keyCode == KeyCode.Backspace || keyCode == KeyCode.Tab ||
                    keyCode == KeyCode.Escape || keyCode == KeyCode.Delete)
                {
#if UNITY_2019 || UNITY_2020
                    if (!_wasDown.ContainsKey(keyCode))
                        _wasDown.Add(keyCode, false);
#else
                    _wasDown.TryAdd(keyCode, false);
#endif
                }
            }

            _keys = new List<KeyCode>(_wasDown.Keys);
        }

        private void HandleKey(string name, bool isDown, ref bool wasDown)
        {
            if (!wasDown && isDown)
                OnPressed?.Invoke(this, new KeyboardTrackingBehaviourArgs(true, name));
            else if (wasDown && !isDown)
                OnPressed?.Invoke(this, new KeyboardTrackingBehaviourArgs(false, name));

            wasDown = isDown;
        }

        protected virtual void Update()
        {
            foreach (var key in _keys)
            {
                bool isDown = false;

#if ENABLE_INPUT_SYSTEM
                // Convert KeyCode to Key (InputSystem) where possible
                var inputKey = ConvertToInputSystemKey(key);
                if (inputKey != null)
                {
                    isDown = inputKey.isPressed;
                }
#else
                isDown = Input.GetKey(key);
#endif
                var wasDown = _wasDown[key];
                HandleKey(key.ToString(), isDown, ref wasDown);
                _wasDown[key] = wasDown;
            }
        }

#if ENABLE_INPUT_SYSTEM
        private KeyControl ConvertToInputSystemKey(KeyCode keyCode)
        {
            if (Keyboard.current == null) return null;

            return keyCode switch
            {
                KeyCode.A => Keyboard.current.aKey,
                KeyCode.B => Keyboard.current.bKey,
                KeyCode.C => Keyboard.current.cKey,
                KeyCode.D => Keyboard.current.dKey,
                KeyCode.E => Keyboard.current.eKey,
                KeyCode.F => Keyboard.current.fKey,
                KeyCode.G => Keyboard.current.gKey,
                KeyCode.H => Keyboard.current.hKey,
                KeyCode.I => Keyboard.current.iKey,
                KeyCode.J => Keyboard.current.jKey,
                KeyCode.K => Keyboard.current.kKey,
                KeyCode.L => Keyboard.current.lKey,
                KeyCode.M => Keyboard.current.mKey,
                KeyCode.N => Keyboard.current.nKey,
                KeyCode.O => Keyboard.current.oKey,
                KeyCode.P => Keyboard.current.pKey,
                KeyCode.Q => Keyboard.current.qKey,
                KeyCode.R => Keyboard.current.rKey,
                KeyCode.S => Keyboard.current.sKey,
                KeyCode.T => Keyboard.current.tKey,
                KeyCode.U => Keyboard.current.uKey,
                KeyCode.V => Keyboard.current.vKey,
                KeyCode.W => Keyboard.current.wKey,
                KeyCode.X => Keyboard.current.xKey,
                KeyCode.Y => Keyboard.current.yKey,
                KeyCode.Z => Keyboard.current.zKey,

                KeyCode.Space => Keyboard.current.spaceKey,
                KeyCode.Return => Keyboard.current.enterKey,
                KeyCode.Backspace => Keyboard.current.backspaceKey,
                KeyCode.Tab => Keyboard.current.tabKey,
                KeyCode.Escape => Keyboard.current.escapeKey,
                KeyCode.Delete => Keyboard.current.deleteKey,

                KeyCode.UpArrow => Keyboard.current.upArrowKey,
                KeyCode.DownArrow => Keyboard.current.downArrowKey,
                KeyCode.LeftArrow => Keyboard.current.leftArrowKey,
                KeyCode.RightArrow => Keyboard.current.rightArrowKey,

                KeyCode.Alpha0 => Keyboard.current.digit0Key,
                KeyCode.Alpha1 => Keyboard.current.digit1Key,
                KeyCode.Alpha2 => Keyboard.current.digit2Key,
                KeyCode.Alpha3 => Keyboard.current.digit3Key,
                KeyCode.Alpha4 => Keyboard.current.digit4Key,
                KeyCode.Alpha5 => Keyboard.current.digit5Key,
                KeyCode.Alpha6 => Keyboard.current.digit6Key,
                KeyCode.Alpha7 => Keyboard.current.digit7Key,
                KeyCode.Alpha8 => Keyboard.current.digit8Key,
                KeyCode.Alpha9 => Keyboard.current.digit9Key,

                _ => null
            };
        }
#endif
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace OmiLAXR
{
    public class IntervalTimer
    {
        private readonly MonoBehaviour _owner;
        private Coroutine _coroutine;
        private readonly float _intervalSeconds;
        private readonly Action _callback;
        
        public IntervalTimer(MonoBehaviour owner, float intervalSeconds, Action callback)
        {
            this._owner = owner;
            this._intervalSeconds = intervalSeconds;
            this._callback = callback;
        }

        public void Start()
        {
            if (_coroutine == null && _intervalSeconds > 0f)
            {
                _coroutine = _owner.StartCoroutine(TimerLoop());
            }
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                _owner.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator TimerLoop()
        {
            while (true)
            {
                _callback?.Invoke();
                yield return new WaitForSeconds(_intervalSeconds);
            }
        }
    }

}
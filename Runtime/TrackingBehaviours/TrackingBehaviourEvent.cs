using System.Collections.Generic;
using UnityEngine.Events;

namespace OmiLAXR.TrackingBehaviours
{
    public interface ITrackingBehaviourEvent
    {
        void UnbindAll();
        void ClearActions();
        void Dispose();
        void Mute();
        bool IsDisabled { get; set; }
    }
    public class TrackingBehaviourEvent : ITrackingBehaviourEvent
    {
        public readonly List<TrackingBehaviourAction> Actions = new List<TrackingBehaviourAction>();
        public event TrackingBehaviourAction Action;
        private Dictionary<UnityEvent, UnityAction> _unityBinds =
            new Dictionary<UnityEvent, UnityAction>();
        public bool IsDisabled { get; set; } = false;

        public void AddHandler(TrackingBehaviourAction action)
        {
            Action += action;
            Actions.Add(action);
        }

        public void Invoke(ITrackingBehaviour owner)
        {
            if (IsDisabled)
                return;
            Action?.Invoke(owner);
        }

        public void ClearActions()
        {
            foreach (var a in Actions)
            {
                Action -= a;
            }

            Actions.Clear();
        }

        public void Dispose()
        {
            UnbindAll();
            ClearActions();
        }
        public void Mute()
        {
            IsDisabled = true;
        }

        public void Unmute()
        {
            IsDisabled = false;
        }
        
        public void Bind(UnityEvent unityEvent, ITrackingBehaviour owner)
            => Bind(unityEvent, () => Invoke(owner));
        
        public void Bind(UnityEvent unityEvent, UnityAction invoker)
        {
#if UNITY_2020 || UNITY_2019
            if (!_unityBinds.ContainsKey(unityEvent))
                _unityBinds.Add(unityEvent, invoker);
#else
            if (!_unityBinds.TryAdd(unityEvent, invoker))
                return;
#endif
            unityEvent.AddListener(invoker);
        }

        public void Unbind(UnityEvent unityEvent)
        {
            unityEvent.RemoveListener(_unityBinds[unityEvent]);
            _unityBinds.Remove(unityEvent);
        }

        public void UnbindAll()
        {
            foreach (var ev in _unityBinds.Keys)
            {
                ev.RemoveListener(_unityBinds[ev]);
            }
            _unityBinds.Clear();
        }
    }
    public class TrackingBehaviourEvent<T> : ITrackingBehaviourEvent
    {
        public readonly List<TrackingBehaviourAction<T>> Actions = new List<TrackingBehaviourAction<T>>();
        public event TrackingBehaviourAction<T> Action;
        private Dictionary<UnityEvent, UnityAction> _unityBinds =
            new Dictionary<UnityEvent, UnityAction>();
        public bool IsDisabled { get; set; } = false;

        public void AddHandler(TrackingBehaviourAction<T> action)
        {
            Action += action;
            Actions.Add(action);
        }

        public void Invoke(ITrackingBehaviour owner, T arg)
        {
            if (IsDisabled)
                return;
            Action?.Invoke(owner, arg);
        }

        public void ClearActions()
        {
            foreach (var a in Actions)
            {
                Action -= a;
            }

            Actions.Clear();
        }

        public void Dispose()
        {
            ClearActions();
            UnbindAll();
        }

        public void Bind(UnityEvent unityEvent, ITrackingBehaviour owner, T arg)
         => Bind(unityEvent, () => Invoke(owner, arg));

        public void Bind(UnityEvent unityEvent, UnityAction invoker)
        {
#if UNITY_2020 || UNITY_2019
            if (!_unityBinds.ContainsKey(unityEvent))
                _unityBinds.Add(unityEvent, invoker);
#else
            if (!_unityBinds.TryAdd(unityEvent, invoker))
                return;
#endif
            unityEvent.AddListener(invoker);
        }

        public void Unbind(UnityEvent unityEvent)
        {
            unityEvent.RemoveListener(_unityBinds[unityEvent]);
            _unityBinds.Remove(unityEvent);
        }

        public void Mute()
        {
            IsDisabled = true;
        }

        public void Unmute()
        {
            IsDisabled = false;
        }

        public void UnbindAll()
        {
            foreach (var ev in _unityBinds.Keys)
            {
                ev.RemoveListener(_unityBinds[ev]);
            }
            _unityBinds.Clear();
        }
    }
    public class TrackingBehaviourEvent<TSender, TValue> : ITrackingBehaviourEvent
    {
        public readonly List<TrackingBehaviourAction<TSender, TValue>> Actions = new List<TrackingBehaviourAction<TSender, TValue>>();
        public event TrackingBehaviourAction<TSender, TValue> Action;

        private Dictionary<UnityEvent<TValue>, UnityAction<TValue>> _unityBinds =
            new Dictionary<UnityEvent<TValue>, UnityAction<TValue>>();
        public bool IsDisabled { get; set; } = false;

        public void AddHandler(TrackingBehaviourAction<TSender, TValue> action)
        {
            Action += action;
            Actions.Add(action);
        }

        public void Invoke(ITrackingBehaviour owner, TSender sender, TValue value)
        {
            if (IsDisabled)
                return;
            Action?.Invoke(owner, sender, value);
        }
        public void Mute()
        {
            IsDisabled = true;
        }

        public void Unmute()
        {
            IsDisabled = false;
        }
        public void ClearActions()
        {
            foreach (var a in Actions)
            {
                Action -= a;
            }

            Actions.Clear();
        }

        public void Dispose()
        {
            UnbindAll();
            ClearActions();
        }
        
        public void Bind(UnityEvent<TValue> unityEvent, UnityAction<TValue> invoker)
        {
#if UNITY_2020 || UNITY_2019
            if (!_unityBinds.ContainsKey(unityEvent))
                _unityBinds.Add(unityEvent, invoker);
#else
            if (!_unityBinds.TryAdd(unityEvent, invoker))
                return;
#endif
            unityEvent.AddListener(invoker);
        }

        public void Unbind(UnityEvent<TValue> unityEvent)
        {
           unityEvent.RemoveListener(_unityBinds[unityEvent]);
           _unityBinds.Remove(unityEvent);
        }

        public void UnbindAll()
        {
            foreach (var ev in _unityBinds.Keys)
            {
                ev.RemoveListener(_unityBinds[ev]);
            }
            _unityBinds.Clear();
        }
    }
    public class TrackingBehaviourEvent<TSender, TValue, TArgs> : ITrackingBehaviourEvent
    {
        public readonly List<TrackingBehaviourAction<TSender, TValue, TArgs>> Actions = new List<TrackingBehaviourAction<TSender, TValue, TArgs>>();
        public event TrackingBehaviourAction<TSender, TValue, TArgs> Action;

        private Dictionary<UnityEvent<TValue>, UnityAction<TValue>> _unityBinds =
            new Dictionary<UnityEvent<TValue>, UnityAction<TValue>>();
        public bool IsDisabled { get; set; } = false;

        public void AddHandler(TrackingBehaviourAction<TSender, TValue, TArgs> action)
        {
            Action += action;
            Actions.Add(action);
        }

        public void Invoke(ITrackingBehaviour owner, TSender sender, TValue value, TArgs args)
        {
            if (IsDisabled)
                return;
            Action?.Invoke(owner, sender, value, args);
        }
        public void Mute()
        {
            IsDisabled = true;
        }

        public void Unmute()
        {
            IsDisabled = false;
        }
        public void ClearActions()
        {
            foreach (var a in Actions)
            {
                Action -= a;
            }

            Actions.Clear();
        }

        public void Dispose()
        {
            UnbindAll();
            ClearActions();
        }

        public void Bind(UnityEvent<TValue> unityEvent, UnityAction<TValue> invoker)
        {
            #if UNITY_2020 || UNITY_2019
            if (!_unityBinds.ContainsKey(unityEvent))
                _unityBinds.Add(unityEvent, invoker);
            #else
            if (!_unityBinds.TryAdd(unityEvent, invoker))
                return;
            #endif
            unityEvent.AddListener(invoker);
        }

        public void Unbind(UnityEvent<TValue> unityEvent)
        {
            unityEvent.RemoveListener(_unityBinds[unityEvent]);
            _unityBinds.Remove(unityEvent);
        }

        public void UnbindAll()
        {
            foreach (var ev in _unityBinds.Keys)
            {
                ev.RemoveListener(_unityBinds[ev]);
            }
            _unityBinds.Clear();
        }
    }
}
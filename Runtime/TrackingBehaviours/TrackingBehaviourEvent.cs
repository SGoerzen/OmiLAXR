using System;
using System.Collections.Generic;
using UnityEngine;
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

        public void Invoke(TrackingBehaviour owner)
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
        public void Bind(UnityEvent unityEvent, UnityAction invoker)
        {
            if (!_unityBinds.TryAdd(unityEvent, invoker))
                return;
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

        public void Invoke(TrackingBehaviour owner, T arg)
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

        public void Bind(UnityEvent unityEvent, UnityAction invoker)
        {
            if (!_unityBinds.TryAdd(unityEvent, invoker))
                return;
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
    public class TrackingBehaviourEvent<TSender, TArgs> : ITrackingBehaviourEvent
    {
        public readonly List<TrackingBehaviourAction<TSender, TArgs>> Actions = new List<TrackingBehaviourAction<TSender, TArgs>>();
        public event TrackingBehaviourAction<TSender, TArgs> Action;

        private Dictionary<UnityEvent<TArgs>, UnityAction<TArgs>> _unityBinds =
            new Dictionary<UnityEvent<TArgs>, UnityAction<TArgs>>();
        public bool IsDisabled { get; set; } = false;

        public void AddHandler(TrackingBehaviourAction<TSender, TArgs> action)
        {
            Action += action;
            Actions.Add(action);
        }

        public void Invoke(TrackingBehaviour owner, TSender sender, TArgs args)
        {
            if (IsDisabled)
                return;
            Action?.Invoke(owner, sender, args);
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

        public void Bind(UnityEvent<TArgs> unityEvent, UnityAction<TArgs> invoker)
        {
            if (!_unityBinds.TryAdd(unityEvent, invoker))
                return;
            unityEvent.AddListener(invoker);
        }

        public void Unbind(UnityEvent<TArgs> unityEvent)
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
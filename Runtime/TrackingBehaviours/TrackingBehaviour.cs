using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OmiLAXR.Schedules;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Non-generic base class for tracking behaviors that work with general Unity Objects.
    /// Provides a simplified API by using Object as the tracking type.
    /// </summary>
    public abstract class TrackingBehaviour : TrackingBehaviour<Object>
    {
        /// <summary>
        /// Empty implementation to be overridden by derived classes if needed.
        /// Called after objects have been filtered through the pipeline. Can be called multiple times.
        /// </summary>
        /// <param name="objects">Array of filtered objects to track</param>
        protected override void AfterFilteredObjects(Object[] objects)
        {
            // Base implementation is empty - derived classes should override if needed
        }
    }
    
    /// <summary>
    /// Generic base class for all tracking behaviors in the OmiLAXR system.
    /// Provides functionality to track specific types of objects through the pipeline.
    /// Executes early in Unity's order to ensure tracking is set up before other components.
    /// </summary>
    /// <typeparam name="T">Type of objects to track</typeparam>
    [DefaultExecutionOrder(-1)]
    public abstract class TrackingBehaviour<T> : ActorPipelineComponent, ITrackingBehaviour
    where T : Object
    {
        /// <summary>
        /// Collection of schedulers that control when this tracking behavior executes.
        /// </summary>
        protected readonly List<Scheduler> Schedulers = new List<Scheduler>();

        protected IntervalTicker SetInterval(Action onTick, IntervalTicker.Settings settings, Action onTickStart = null, Action onTickEnd = null)
        {
            var it = new IntervalTicker(this, settings, onTick, onTickStart, onTickEnd);
            Schedulers.Add(it);
            if (settings.runImmediate)
                it.Start();
            return it;
        }

        protected IntervalTicker SetInterval(Action onTick, float intervalSeconds = 1.0f)
            => SetInterval(onTick, new IntervalTicker.Settings() { intervalSeconds = intervalSeconds });

        protected TimeoutTicker SetTimeout(Action onTick, TimeoutTicker.Settings settings, Action onTickStart = null, Action onTickEnd = null)
        {
            var to = new TimeoutTicker(this, settings, onTick, onTickStart, onTickEnd);
            Schedulers.Add(to);
            if (settings.runImmediate)
                to.Start();
            return to;
        }

        protected TimeoutTicker SetTimeout(Action onTick, float timeoutSeconds = 1.0f)
            => SetTimeout(onTick, new TimeoutTicker.Settings() { timeoutSeconds = timeoutSeconds });
       
        /// <summary>
        /// Initializes the tracking behavior by setting up schedulers and event subscriptions.
        /// Connects to pipeline events to receive objects for tracking.
        /// </summary>
        protected virtual void OnEnable()
        {
            try
            {
                Pipeline.BeforeStartedPipeline += _ =>
                {
                    if (!enabled)
                        return;
                    BeforeStartedPipeline(Pipeline);
                };
                // Subscribe to pipeline events
                Pipeline.AfterStartedPipeline += _ =>
                {
                    if (!enabled)
                        return;
                    OnStartedPipeline(Pipeline);
                    StartSchedules();
                };
                Pipeline.BeforeStoppedPipeline += _ =>
                {
                    if (!enabled)
                        return;
                    BeforeStoppedPipeline(Pipeline);
                };
                Pipeline.AfterStoppedPipeline += _ =>
                {
                    if (!enabled)
                        return;
                    OnStoppedPipeline(Pipeline);
                    ClearSchedules();
                    Dispose(AllFilteredObjects.Select(o => o as Object).ToArray());
                    ;
                };

                // Handle objects found by the pipeline
                Pipeline.AfterFoundObjects += (objects) =>
                {
                    if (!enabled)
                        return;
                    // Skip Select<T> if not needed (optimization)
                    AfterFoundObjects(typeof(T) == typeof(Object) ? objects as T[] : Select<T>(objects));
                };

                // Handle objects after they've been filtered by the pipeline
                Pipeline.AfterFilteredObjects += (objects) =>
                {
                    if (!enabled)
                        return;
                    // Skip Select<T> if not needed (optimization)

                    if (typeof(T) == typeof(Object) || objects.Length == 0)
                        AfterFilteredObjects(objects as T[]);
                    else
                    {
                        var selectedObjects = Select<T>(objects);
                        AllFilteredObjects.AddRange(selectedObjects);
                        AfterFilteredObjects(selectedObjects);
                    }
                };
            }
            catch (Exception ex)
            {
                DebugLog.OmiLAXR.Error($"Error initializing {GetType().Name}: {ex.Message}");
            }
        }

        protected virtual void BeforeStartedPipeline(Pipeline pipeline) {}
        protected virtual void BeforeStoppedPipeline(Pipeline pipeline) {}
        /// <summary>
        /// Called when the pipeline starts. Override to implement custom start behavior.
        /// </summary>
        /// <param name="pipeline">Reference to the started pipeline</param>
        protected virtual void OnStartedPipeline(Pipeline pipeline) {}
        
        /// <summary>
        /// Called before the pipeline stops. Override to implement custom stop behavior.
        /// </summary>
        /// <param name="pipeline">Reference to the stopping pipeline</param>
        protected virtual void OnStoppedPipeline(Pipeline pipeline) {}
        
        /// <summary>
        /// Called after objects are found by the pipeline but before filtering.
        /// Override to implement custom processing on found objects.
        /// </summary>
        /// <param name="objects">Array of found objects of type T</param>
        protected virtual void AfterFoundObjects(T[] objects) {}
        
        /// <summary>
        /// Called after objects have been filtered by the pipeline.
        /// Must be implemented by derived classes to process the filtered objects. Can be called multiple times.
        /// </summary>
        /// <param name="objects">Array of filtered objects of type T</param>
        protected abstract void AfterFilteredObjects(T[] objects);

        /// <summary>
        /// Stores the currently selected objects for tracking.
        /// </summary>
        protected readonly List<T> AllFilteredObjects = new List<T>();

        protected void StartSchedules()
        {
            foreach (var scheduler in Schedulers)
                scheduler.Start();
        }

        protected void StopSchedules()
        {
            foreach (var scheduler in Schedulers)
                scheduler.Stop();
        }

        protected void ClearSchedules()
        {
            StopSchedules();
            Schedulers.Clear();
        }

        protected virtual void OnDisable()
        {
            StopSchedules();
            DisposeAllTrackingEvents();
        }

        /// <summary>
        /// Unbinds all tracking events to prevent memory leaks and unexpected behavior.
        /// </summary>
        protected void DisposeAllTrackingEvents()
        {
            // Get all fields of type ITrackingBehaviourEvent
            var fields = GetTrackingBehaviourEvents();

            foreach (var field in fields)
            {
                // Get the value of the field from the current instance
                var fieldValue = field.GetValue(this) as ITrackingBehaviourEvent;

                // Call UnbindAll if the field is not null
                fieldValue?.UnbindAll();
            }
        }
        
        /// <summary>
        /// Cleans up resources when tracking stops.
        /// Unbinds all tracking events by default.
        /// </summary>
        /// <param name="objects">Array of objects that were being tracked</param>
        protected virtual void Dispose(Object[] objects)
        {
            DisposeAllTrackingEvents();
        }
        
        /// <summary>
        /// Retrieves all fields in this class that implement ITrackingBehaviourEvent.
        /// Used for automatic event management.
        /// </summary>
        /// <returns>Array of FieldInfo for fields of type ITrackingBehaviourEvent</returns>
        public FieldInfo[] GetTrackingBehaviourEvents()
        {
            return GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(f => typeof(ITrackingBehaviourEvent).IsAssignableFrom(f.FieldType))
                .ToArray();
        }
        
        /// <summary>
        /// Filters an array of objects to only include those of the specified type.
        /// </summary>
        /// <typeparam name="TS">Type to filter for</typeparam>
        /// <param name="objects">Array of objects to filter</param>
        /// <returns>Array of objects of type TS</returns>
        protected TS[] Select<TS>(Object[] objects) where TS : Object
            => objects
                .Where(o => o.GetType() == typeof(TS) || o.GetType().IsSubclassOf(typeof(TS)))
                .Select(o => o as TS).ToArray();
        
        /// <summary>
        /// Gets the first object of the specified type from an array of objects.
        /// </summary>
        /// <typeparam name="TS">Type to find</typeparam>
        /// <param name="objects">Array of objects to search</param>
        /// <returns>The first object of type TS, or null if none exists</returns>
        protected TS First<TS>(Object[] objects) where TS : Object
            => (TS)objects
                .FirstOrDefault(o => o.GetType() == typeof(TS) || o.GetType().IsSubclassOf(typeof(TS)));
    }
}
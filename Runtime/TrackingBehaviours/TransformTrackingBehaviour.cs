/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Components;
using OmiLAXR.Schedules;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Monitors transform changes (position, rotation, scale) in GameObjects with TransformWatcher components.
    /// Supports both interval-based polling and real-time change detection modes.
    /// Can selectively ignore specific transform properties based on configuration.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Transform Tracking Behaviour")]
    [Description("Tracks position, rotation and scale changes in a game object holding <TransformWatcher> component.")]
    public class TransformTrackingBehaviour : TrackingBehaviour<TransformWatcher>
    {
        /// <summary>
        /// Enable real-time change detection by binding to TransformWatcher events.
        /// When false, only interval-based checking is performed.
        /// </summary>
        public bool detectOnChange = true;
        
        /// <summary>
        /// Configuration for interval-based transform checking.
        /// Defines timing settings for periodic transform state evaluation.
        /// </summary>
        public IntervalTicker.Settings intervalSettings;
        
        /// <summary>
        /// Flags to ignore specific transform components during tracking.
        /// Allows selective monitoring of only position, rotation, or scale changes.
        /// </summary>
        public TransformWatcher.TransformIgnore ignore;
        
        /// <summary>
        /// Event triggered when a tracked object's position changes.
        /// Provides both the TransformWatcher and detailed change information.
        /// </summary>
        [Gesture("Movement"), Action("Translation")]
        public readonly TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedPosition =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
            
        /// <summary>
        /// Event triggered when a tracked object's rotation changes.
        /// Includes old and new rotation values in the change data.
        /// </summary>
        [Gesture("Movement"), Action("Rotation")]
        public readonly TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedRotation =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
            
        /// <summary>
        /// Event triggered when a tracked object's scale changes.
        /// Captures scale modifications with before/after values.
        /// </summary>
        [Gesture("Movement"), Action("Scale")]
        public readonly TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedScale =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();

        /// <summary>
        /// Initializes interval-based transform monitoring when the pipeline starts.
        /// Creates a scheduled task that polls all tracked transforms at regular intervals.
        /// </summary>
        /// <param name="pipeline">The pipeline that owns this tracking behavior</param>
        protected override void OnStartedPipeline(Pipeline pipeline)
        {
            // Set up interval-based polling of transform states
            SetInterval(() =>
            {
                // Check each tracked transform watcher
                foreach (var tw in AllFilteredObjects)
                {
                    // Monitor position changes if not ignored
                    if (!ignore.position)
                    {
                        var posState = new TransformWatcher.TransformChange()
                        {
                            OldValue = tw.PreviousPosition,
                            NewValue = tw.CurrentPosition,
                        };
                        OnChangedPosition.Invoke(this, tw, posState);
                    }

                    // Monitor rotation changes if not ignored
                    if (!ignore.rotation)
                    {
                        var rotState = new TransformWatcher.TransformChange()
                        {
                            OldValue = tw.PreviousRotation,
                            NewValue = tw.CurrentRotation,
                        };
                        OnChangedRotation.Invoke(this, tw, rotState);
                    }

                    // Monitor scale changes if not ignored
                    if (!ignore.scale)
                    {
                        var scaleState = new TransformWatcher.TransformChange()
                        {
                            OldValue = tw.PreviousScale,
                            NewValue = tw.CurrentScale,
                        };
                        OnChangedScale.Invoke(this, tw, scaleState);
                    }
                }        
            }, intervalSettings);
        }
        
        /// <summary>
        /// Sets up real-time event bindings for detected TransformWatcher components.
        /// Binds to each watcher's change events for immediate notification of transform modifications.
        /// </summary>
        /// <param name="transformWatchers">Array of TransformWatcher components to monitor</param>
        protected override void AfterFilteredObjects(TransformWatcher[] transformWatchers)
        {
            // Skip real-time binding if change detection is disabled
            if (!detectOnChange)
                return;
                
            // Bind to each transform watcher's events
            foreach (var tw in transformWatchers)
            {
                // Bind to position change events
                OnChangedPosition.Bind(tw.onChangedPosition, tc =>
                {
                    if (!ignore.position)
                        OnChangedPosition.Invoke(this, tw, tc);
                });
                
                // Bind to rotation change events
                OnChangedRotation.Bind(tw.onChangedRotation, tc =>
                {
                    if (!ignore.rotation)
                        OnChangedRotation.Invoke(this, tw, tc);
                });
                
                // Bind to scale change events
                OnChangedScale.Bind(tw.onChangedScale, tc =>
                {
                    if (!ignore.scale)
                        OnChangedScale.Invoke(this, tw, tc);
                });
            }
        }
    }
}
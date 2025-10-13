using System.Collections.Generic;
using OmiLAXR.Components;
using OmiLAXR.Components.Gaze;
using OmiLAXR.Components.Gaze.Fixation;
using OmiLAXR.Extensions;
using OmiLAXR.TrackingBehaviours.Learner.Gaze;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    public abstract class GazeDetectorListener<T> : Listener
    where T : Component
    {
        [Tooltip("Add <TransformWatcher> to gaze game objects.")]
        public bool addTransformWatcher = true;

        public bool enableFixation = true;
        
        public GazeDetectorDebug.GazeDetectorDebugSettings
            debugConfig;
        
        protected virtual void Reset()
        {
            debugConfig = GazeDetectorDebug.GazeDetectorDebugSettings.Default;
        }

        protected abstract bool IsCorrectComponent(T component);

        protected virtual void Map(GazeDetector gazeDetector)
        {
            // Do nothing
        }
        
        public override void StartListening()
        {
            var gazeComponents = FindObjects<T>();
            var transformWatchers = new List<TransformWatcher>();
            var gazeDetectors = new List<GazeDetector>();
            foreach (var gazeComponent in gazeComponents)
            {
                if (!IsCorrectComponent(gazeComponent)) 
                    continue;

                if (addTransformWatcher)
                {
                    var tw = gazeComponent.gameObject.EnsureComponent<TransformWatcher>();
                    transformWatchers.Add(tw);
                }
                
                var gd = gazeComponent.gameObject.EnsureComponent<GazeDetector>();
                gazeDetectors.Add(gd);

                if (enableFixation)
                    gazeComponent.gameObject.EnsureComponent<FixationDetector>();
                
                Map(gd);
                
                if (debugConfig.enabled)
                {
                    var debug = gazeComponent.gameObject
                        .EnsureComponent<GazeDetectorDebug>();
                    debug.Setup(debugConfig, gd.rayDistance);
                }
            }
            Found(transformWatchers.ToArray());
            Found(gazeDetectors.ToArray());
        }
    }
}
using System;
using System.Collections;
using System.Linq;
using OmiLAXR.Pipelines.Stages;
using UnityEngine;

namespace OmiLAXR.Pipelines
{
    /// <summary>
    /// Pipeline System containing many stages.
    /// </summary>
    public abstract class Pipeline : MonoBehaviour
    {
        public PipelineTrigger[] triggers;
        public GameObjectProcessStage[] gameObjectProcessStages;
        public StatementComposerStage[] statementComposerStages;
        public TrackingBehaviour[] trackingBehaviours;
        private void Awake()
        {
            trackingBehaviours = GetComponentsInChildren<TrackingBehaviour>();
            triggers = gameObject.GetComponentsInChildren<PipelineTrigger>(false);
            gameObjectProcessStages = gameObject.GetComponentsInChildren<GameObjectProcessStage>(false);

            StartCoroutine(WaitForTriggerCondition(() => 
                triggers.Aggregate(false, (b, trigger) => b || trigger.TriggerCondition())));
        }
        
        private IEnumerator WaitForTriggerCondition(Func<bool> condition)
        {
            // Wait until the condition is true
            yield return new WaitUntil(() => condition());

            StartPipeline();
        }

        private void StartPipeline()
        {
            // Process Game Objects
            var gameObjects = gameObjectProcessStages
                .Aggregate(PipelineData<GameObject>.Empty, (data, stage) => stage.Pass(data));
            
            // Generate statements
            
        }
    }
}
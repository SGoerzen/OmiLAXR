using UnityEngine;

namespace OmiLAXR.Pipeline.Stages.ObjectDetection
{
    [RequireComponent(typeof(ObjectDetectorsStage))]
    [DisallowMultipleComponent]
    [AddComponentMenu("OmiLAXR / Pipeline / Stages / Object Detectors / Jobs / Detect Scene Objects Job")]
    public class DetectSceneGameObjectsJob : PipelineJob<GameObject, GameObject>
    {
        public override PipelineData<GameObject> Pass(PipelineData<GameObject> data)
        {
            var gameObjects = Object.FindObjectsOfType<GameObject>();
            return PipelineData<GameObject>.From(gameObjects);
        }
    }
}
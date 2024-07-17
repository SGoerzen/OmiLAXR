using UnityEngine;

namespace OmiLAXR.Pipeline.Stages.ObjectDetection
{
    public class DetectSceneGameObjectsJob : PipelineJob
    {
        public override PipelineData Pass(PipelineData data)
        {
            var gameObjects = Object.FindObjectsOfType<GameObject>();
            return PipelineData.From(gameObjects);
        }
    }
}
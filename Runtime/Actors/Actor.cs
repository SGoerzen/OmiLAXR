using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / 0) Pipelines / Actor")]
    public class Actor : PipelineComponent
    {
        public ActorGroup group;
        public string actorName = "Anonymous";
        public string actorEmail = "anonymous@omilaxr.dev";
    }
}
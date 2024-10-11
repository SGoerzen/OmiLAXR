using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Actors / Actor")]
    public class Actor : PipelineComponent
    {
        public ActorGroup group;
        public string actorName = "Anonymous";
        public string actorEmail = "anonymous@omilaxr.dev";
    }
}
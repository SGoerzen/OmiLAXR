using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Actors / Actor")]
    public class Actor : PipelineComponent
    {
        public Team team;
        public string actorName = "Anonymous";
        public string actorEmail = "anonymous@omilaxr.dev";
        public virtual bool IsGroup => false;
    }
}
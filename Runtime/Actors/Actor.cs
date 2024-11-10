using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Actors / Actor")]
    public class Actor : ActorPipelineComponent
    {
        public Team team;
        public bool HasTeam => team != null;
        public string actorName = "Anonymous";
        public string actorEmail = "anonymous@omilaxr.dev";
        public virtual bool IsGroupActor => false;
    }
}
using System.Linq;
using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Actors / Actor Group")]
    public class ActorGroup : PipelineComponent
    {
        public string groupName = "Group";
        public string groupEmail = "group@omilaxr.dev";
        
        public Actor[] GetMembers() => FindObjectsByType<Actor>(FindObjectsSortMode.InstanceID)
            .Where(actor => this.Equals(actor.group)).ToArray();

        public void AddMember(Actor actor)
        {
            actor.group = this;
        }

        public void AddMembers(params Actor[] actors)
        {
            foreach (var a in actors)
                a.group = this;
        }
    }
}
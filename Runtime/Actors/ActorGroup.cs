using System.Linq;
using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Actors / Actor Group")]
    [RequireComponent(typeof(Actor))]
    public class ActorGroup : Actor
    {
        public Actor[] GetMembers() => GetComponents<Actor>()
            .Where(a => a.GetType() != typeof(ActorGroup)).ToArray();

        public override bool IsGroup => true;

        private void Reset()
        {
            actorName = "Group";
            actorEmail = "group@omilaxr.dev";
        }
    }
}
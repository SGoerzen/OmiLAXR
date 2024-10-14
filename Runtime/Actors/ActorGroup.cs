using System;
using System.Linq;
using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Actors / Actor Group")]
    [RequireComponent(typeof(Actor))]
    public class ActorGroup : Actor
    {
        private Actor[] _members;
        public Actor[] GetMembers() => _members;

        private void Start()
        {
            UpdateMemberList();
        }

        public void UpdateMemberList()
        {
            _members = GetComponents<Actor>()
                .Where(a => a.GetType() != typeof(ActorGroup)).ToArray();
        }

        public override bool IsGroupActor => true;

        private void Reset()
        {
            actorName = "Group";
            actorEmail = "group@omilaxr.dev";
        }
    }
}
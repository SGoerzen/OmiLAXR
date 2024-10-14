using System.Linq;
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Agents or Groups can also be included in the ‘context’ of a statement as an ‘instructor,’ leading to statements of the form “Brian (actor) learned xAPI from Ben (instructor).” Context can also include a ‘team’ property but it must be a Group.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Actors / Team")]
    public class Team : PipelineComponent
    {
        public string teamName = "Team";
        public string teamEmail = "team@omilaxr.dev";
        public Actor[] GetMembers() => _members;
        private Actor[] _members;
        
        private void Start()
        {
            UpdateMemberList();
        }

        public void UpdateMemberList()
        {
            _members = FindObjectsByType<Actor>(FindObjectsSortMode.InstanceID)
                .Where(actor => Equals(actor.team)).ToArray();
        }
        
        public void AddMember(Actor actor)
        {
            actor.team = this;
            UpdateMemberList();
        }

        public void AddMembers(params Actor[] actors)
        {
            foreach (var a in actors)
                a.team = this;
            UpdateMemberList();
        }
    }
}
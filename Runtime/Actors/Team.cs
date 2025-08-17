/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.Linq;
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Represents a team that can contain multiple actors as members.
    /// Teams can be used to group actors together and manage their relationships.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Actors / Team")]
    public class Team : PipelineComponent
    {
        /// <summary>
        /// The display name of the team.
        /// </summary>
        public string teamName = "Team";

        /// <summary>
        /// The contact email address for the team.
        /// </summary>
        public string teamEmail = "team@omilaxr.dev";

        /// <summary>
        /// Returns the current array of team members.
        /// </summary>
        /// <returns>Array of Actor components that are members of this team.</returns>
        public Actor[] GetMembers() => _members;

        /// <summary>
        /// Private cache of team members.
        /// </summary>
        private Actor[] _members;
        
        /// <summary>
        /// Initializes the team by updating the member list when the component starts.
        /// </summary>
        private void Start()
        {
            UpdateMemberList();
        }

        /// <summary>
        /// Refreshes the internal list of team members by finding all Actors in the scene
        /// that belong to this team.
        /// </summary>
        public void UpdateMemberList()
        {
#if UNITY_2022_3_OR_NEWER
            // Uses the newer, more efficient FindObjectsByType method in Unity 2022.3+
            _members = FindObjectsByType<Actor>(FindObjectsSortMode.InstanceID)
                .Where(actor => Equals(actor.team)).ToArray();
#else
            // Falls back to FindObjectsOfType for older Unity versions
            _members = FindObjectsOfType<Actor>()
                .Where(actor => Equals(actor.team)).ToArray();
#endif
        }
        
        /// <summary>
        /// Adds a single actor to the team and updates the member list.
        /// </summary>
        /// <param name="actor">The Actor component to add to the team.</param>
        public void AddMember(Actor actor)
        {
            actor.team = this;
            UpdateMemberList();
        }

        /// <summary>
        /// Adds multiple actors to the team and updates the member list.
        /// </summary>
        /// <param name="actors">Array of Actor components to add to the team.</param>
        public void AddMembers(params Actor[] actors)
        {
            foreach (var a in actors)
                a.team = this;
            UpdateMemberList();
        }
    }
}
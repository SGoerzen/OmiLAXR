/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Represents an actor in the OmiLAXR system.
    /// This class extends ActorPipelineComponent and provides basic actor properties.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Actors / Actor")]
    public class Actor : ActorPipelineComponent
    {
        /// <summary>
        /// Reference to the team this actor belongs to.
        /// </summary>
        public Team team;

        /// <summary>
        /// Checks if the actor is assigned to a team.
        /// </summary>
        public bool HasTeam => team != null;

        /// <summary>
        /// The name of the actor. Defaults to "Anonymous".
        /// </summary>
        public string actorName = "Anonymous";

        /// <summary>
        /// The email address of the actor. Defaults to "anonymous@omilaxr.dev".
        /// </summary>
        public string actorEmail = "anonymous@omilaxr.dev";

        /// <summary>
        /// Indicates whether this actor is a group actor.
        /// Can be overridden by derived classes.
        /// </summary>
        public virtual bool IsGroupActor => false;
    }
}
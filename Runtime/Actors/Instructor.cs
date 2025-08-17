/*
* This file is part of OmiLAXR.
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* Website: https://omilaxr.dev
*
* OmiLAXR is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* OmiLAXR is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Affero General Public License for more details.
*
* You should have received a copy of the GNU Affero General Public License
* along with OmiLAXR.  If not, see <https://www.gnu.org/licenses/>.
*/
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Represents an instructor who can guide or teach other actors in the system.
    /// This component allows defining basic instructor information and identity.
    /// </summary>
    /// <remarks>
    /// This component can be attached to GameObjects to define instructor properties
    /// and manage instructor-related functionality.
    /// </remarks>
    [AddComponentMenu("OmiLAXR / Actors / Instructor")]
    public class Instructor : PipelineComponent
    {
        /// <summary>
        /// The display name of the instructor.
        /// Used to identify the instructor within the system.
        /// </summary>
        public string instructorName = "Instructor";

        /// <summary>
        /// The contact email address for the instructor.
        /// Serves as a unique identifier for the instructor.
        /// </summary>
        public string instructorEmail = "instructor@omilaxr.dev";
    }
}
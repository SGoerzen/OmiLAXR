/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR
{
    /// <summary>
    /// Attribute used to mark fields with action names.
    /// Can be applied multiple times to the same field.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ActionAttribute : System.Attribute
    {
        /// <summary>
        /// The name of the action associated with this attribute.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Initializes a new instance of the ActionAttribute class.
        /// </summary>
        /// <param name="name">The name of the action to be associated with the marked field.</param>
        public ActionAttribute(string name)
        {
            Name = name;
        }
    }
}
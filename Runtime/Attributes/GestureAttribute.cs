/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR
{
    /// <summary>
    /// Custom attribute used to mark fields with gesture information.
    /// Can be applied multiple times to the same field.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public sealed class GestureAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the name of the gesture.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Initializes a new instance of the GestureAttribute class.
        /// </summary>
        /// <param name="name">The name identifier for the gesture.</param>
        public GestureAttribute(string name)
        {
            Name = name;
        }
    }
}
/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Composers
{
    /// <summary>
    /// Represents an author with basic information.
    /// </summary>
    public struct Author
    {
        /// <summary>
        /// Gets the name of the author.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the email address of the author.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Author struct.
        /// </summary>
        /// <param name="name">The name of the author.</param>
        /// <param name="email">The email address of the author.</param>
        public Author(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
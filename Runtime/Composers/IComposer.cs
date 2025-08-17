/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Composers
{
    /// <summary>
    /// Interface defining the contract for statement composers in the OmiLAXR analytics pipeline.
    /// Composers are responsible for transforming tracking behavior events into structured statements
    /// that can be processed by endpoints and analytics systems.
    /// </summary>
    public interface IComposer
    {
        /// <summary>
        /// Event fired after a statement has been composed and is ready for delivery to endpoints.
        /// Subscribers (typically endpoints) receive the composer instance and the composed statement.
        /// </summary>
        event ComposerAction<IStatement> AfterComposed;
        
        /// <summary>
        /// Indicates whether this composer operates at a higher abstraction level,
        /// processing output from other composers rather than raw tracking behavior events.
        /// Higher composers typically perform aggregation, analysis, or meta-processing.
        /// </summary>
        bool IsHigherComposer { get; }
        
        /// <summary>
        /// Provides author information for statements created by this composer.
        /// Used for attribution and metadata in the generated statements.
        /// </summary>
        /// <returns>Author details including name and contact information</returns>
        Author GetAuthor();
        
        /// <summary>
        /// Returns the display name of this composer, typically derived from the class name.
        /// Used for identification in logs, UI, and debugging scenarios.
        /// </summary>
        /// <returns>Human-readable name of the composer</returns>
        string GetName();
        
        /// <summary>
        /// Returns the functional category that this composer belongs to.
        /// Used for organizing, filtering, and processing statements by domain area.
        /// </summary>
        /// <returns>The composer group classification</returns>
        ComposerGroup GetGroup();
    }
}
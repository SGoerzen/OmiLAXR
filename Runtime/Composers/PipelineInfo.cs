/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;

namespace OmiLAXR.Composers
{
    /// <summary>
    /// Immutable data structure containing metadata about a pipeline instance.
    /// Used for statement attribution and tracking the source pipeline of analytics data.
    /// Provides information needed for proper statement contextualization and routing.
    /// </summary>
    public struct PipelineInfo
    {
        /// <summary>
        /// Human-readable name of the pipeline instance.
        /// Typically the GameObject name in Unity.
        /// </summary>
        public readonly string Name;
        
        /// <summary>
        /// Runtime type of the pipeline class.
        /// Used for type-based filtering and processing decisions.
        /// </summary>
        public readonly Type Type;
        
        /// <summary>
        /// Array of actor data providers associated with this pipeline.
        /// Provides access to user identification and actor information for statements.
        /// </summary>
        public readonly ActorDataProvider[] ActorDataProviders;

        /// <summary>
        /// Constructs pipeline information from a pipeline instance.
        /// Extracts relevant metadata for use in statement processing.
        /// </summary>
        /// <param name="pipeline">The pipeline to extract information from</param>
        public PipelineInfo(Pipeline pipeline)
        {
            Name = pipeline.name;
            Type = pipeline.GetType();
            ActorDataProviders = pipeline.ActorDataProviders;
        }
    }
}
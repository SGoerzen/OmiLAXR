/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR
{
    /// <summary>
    /// Interface for components that extend Pipeline functionality through composition.
    /// Inherits from IPipelineComponent to participate in the pipeline component system.
    /// Enables modular enhancement of pipelines by adding listeners, filters, and tracking behaviors.
    /// </summary>
    public interface IPipelineExtension : IPipelineComponent
    {
        /// <summary>
        /// Gets the Pipeline instance that this extension is associated with.
        /// Used to access the extended Pipeline's functionality, state, and event system.
        /// </summary>
        /// <returns>The Pipeline instance being extended</returns>
        Pipeline GetPipeline();
        
        /// <summary>
        /// Extends the specified Pipeline with additional components and functionality.
        /// Called during pipeline initialization to register extension components.
        /// Typically adds listeners, filters, and tracking behaviors from child components.
        /// </summary>
        /// <param name="pipeline">The Pipeline to extend with additional capabilities</param>
        void Extend(Pipeline pipeline);
    }
}
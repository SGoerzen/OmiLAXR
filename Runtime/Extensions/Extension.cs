/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Extensions
{
    /// <summary>
    /// Component that provides extensibility mechanisms for the OmiLAXR pipeline system.
    /// Allows attachment of additional components to extend or modify pipeline behavior
    /// without directly modifying the core pipeline implementation.
    /// </summary>
    public class Extension : PipelineComponent
    {
        /// <summary>
        /// Reference to a pipeline component that provides extension functionality.
        /// This component extends or modifies the behavior of the pipeline it's attached to.
        /// </summary>
        public PipelineComponent extensionComponent;
        
        /// <summary>
        /// Reference to a pipeline component that serves as the target for extension.
        /// This is the pipeline component whose behavior will be extended or modified.
        /// </summary>
        public PipelineComponent pipelineExtension;
    }
}
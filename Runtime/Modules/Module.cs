/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using System;

namespace OmiLAXR.Modules
{
    /// <summary>
    /// Abstract base class for all modules within the OmiLAXR framework.
    /// This class serves as the foundation for modular components that can be
    /// integrated into the OmiLAXR pipeline system for VR/AR applications.
    /// </summary>
    /// <remarks>
    /// Modules are specialized pipeline components that provide specific functionality
    /// such as eye tracking, hand tracking, or other XR features. All concrete modules
    /// must inherit from this class and implement the required pipeline behavior.
    /// </remarks>
    [Obsolete("A specific class is not neede anymore. Just use PipelineComponent directly if needed.", true)]
    public abstract class Module : PipelineComponent
    {
       // TODO: Add module-specific properties and methods
       // Common functionality for all modules should be implemented here
    }
}
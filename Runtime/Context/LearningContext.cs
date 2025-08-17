/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Context
{
    /// <summary>
    /// Abstract base class for components that provide contextual information about the learning environment.
    /// Learning contexts supply metadata and configuration data that enrich analytics statements
    /// with information about the scenario, platform, participants, and environment.
    /// </summary>
    public abstract class LearningContext : PipelineComponent
    {
        // Abstract base class - implementations provide specific context types
        // such as language settings, platform info, registration data, etc.
    }
}
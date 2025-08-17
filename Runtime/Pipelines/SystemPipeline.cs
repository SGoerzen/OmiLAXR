/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using UnityEngine;

namespace OmiLAXR.Pipelines
{
    /// <summary>
    /// Specialized pipeline for system-level analytics and logging.
    /// Handles framework-level events, configuration data, and administrative information
    /// that is not specific to individual learners or learning interactions.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Core / System Pipeline")]
    [ShutdownOrder(int.MaxValue)]
    public class SystemPipeline : Pipeline
    {
        // Implementation inherits from base Pipeline class
        // This pipeline specializes in system-wide events and metadata
    }
}
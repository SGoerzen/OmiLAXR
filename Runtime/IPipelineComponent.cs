/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR
{
    /// <summary>
    /// Marker interface for all components that can be part of the OmiLAXR pipeline system.
    /// Provides a common type for pipeline component management and extension discovery.
    /// Used for type safety when working with collections of pipeline components.
    /// </summary>
    public interface IPipelineComponent
    {
        // This interface serves as a marker - no methods required
        // Implementations will provide specific pipeline functionality
    }
}
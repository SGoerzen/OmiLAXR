/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR
{
    /// <summary>
    /// Generic delegate for handling pipeline objects of a specific Unity Object type.
    /// This delegate is typically used for callbacks that need to process collections
    /// of Unity objects within the pipeline system.
    /// </summary>
    /// <typeparam name="T">The type of Unity Object being handled, must inherit from UnityEngine.Object</typeparam>
    /// <param name="objects">An array of Unity objects to be processed by the handler</param>
    public delegate void PipelineObjectsHandler<in T>(T[] objects) where T : UnityEngine.Object;

    /// <summary>
    /// Delegate for handling pipeline initialization events.
    /// This delegate is invoked when a pipeline is being initialized and configured
    /// but before it starts processing data.
    /// </summary>
    /// <param name="pipeline">The pipeline instance that is being initialized</param>
    public delegate void PipelineInitHandler(Pipeline pipeline);

    /// <summary>
    /// Delegate for handling pipeline start events.
    /// This delegate is invoked when a pipeline transitions from an initialized
    /// or stopped state to an active running state.
    /// </summary>
    /// <param name="pipeline">The pipeline instance that has started</param>
    public delegate void PipelineStartedHandler(Pipeline pipeline);

    /// <summary>
    /// Delegate for handling pipeline stop events.
    /// This delegate is invoked when a pipeline transitions from a running state
    /// to a stopped state, either through normal shutdown or error conditions.
    /// </summary>
    /// <param name="pipeline">The pipeline instance that has stopped</param>
    public delegate void PipelineStoppedHandler(Pipeline pipeline);
}
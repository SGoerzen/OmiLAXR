/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using OmiLAXR.Composers;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Defines the contract for statement transfer endpoints.
    /// Provides methods for managing statement sending lifecycle.
    /// </summary>
    public interface IEndpoint
    {
        /// <summary>
        /// Indicates whether the endpoint is currently in the process of sending statements.
        /// </summary>
        bool IsSending { get; }

        /// <summary>
        /// Indicates whether a transfer operation is currently in progress.
        /// </summary>
        bool IsTransferring { get; }

        /// <summary>
        /// Begins the statement sending process.
        /// </summary>
        /// <param name="resetQueue">Optional flag to reset the statement queue before starting</param>
        void StartSending(bool resetQueue = false);

        /// <summary>
        /// Temporarily pauses the statement sending process.
        /// </summary>
        void PauseSending();

        /// <summary>
        /// Completely stops the statement sending process.
        /// </summary>
        void StopSending();

        /// <summary>
        /// Adds a single statement to the sending queue.
        /// </summary>
        /// <param name="statement">The statement to be sent</param>
        void SendStatement(IStatement statement);
    }
}
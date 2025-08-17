/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Base class for endpoints with async/await support.
    /// Ignores the synchronous HandleSending() method and processes everything asynchronously.
    /// </summary>
    public abstract class AsyncEndpoint : Endpoint
    {
        // Always use coroutines instead of threads in async endpoint
        protected override bool useThreads => false;

        /// <summary>
        /// Enqueues and starts async sending for a single statement.
        /// Return value only indicates that processing has been scheduled.
        /// </summary>
        protected sealed override TransferCode HandleSending(IStatement statement)
        {
            _ = HandleSendingInternalAsync(statement);
            return TransferCode.Queued;
        }

        /// <summary>
        /// Internal handler for processing single statements asynchronously.
        /// Wraps error handling and event triggering on the main thread.
        /// </summary>
        private async Task HandleSendingInternalAsync(IStatement statement)
        {
            try
            {
                await HandleSendingAsync(statement);
                Dispatch(() => TriggerSentStatement(statement));
            }
            catch (Exception ex)
            {
                DebugLog.OmiLAXR?.Error($"[AsyncEndpoint] Failed to send: {ex.Message}");
                Dispatch(() => TriggerFailedStatement(statement));
            }
        }

        /// <summary>
        /// Must be implemented by derived classes to send a statement asynchronously.
        /// </summary>
        protected abstract Task HandleSendingAsync(IStatement statement);

        /// <summary>
        /// Default batch handling: process all statements in parallel using Task.WhenAll.
        /// Can be overridden for native batch support.
        /// </summary>
        protected override TransferCode HandleSending(List<IStatement> batch)
        {
            _ = HandleSendingBatchAsync(batch);
            return TransferCode.Queued;
        }

        /// <summary>
        /// Default asynchronous batch handler using parallel statement sending.
        /// Includes event triggers and error fallback.
        /// </summary>
        protected virtual async Task HandleSendingBatchAsync(List<IStatement> batch)
        {
            try
            {
                BeforeHandleSendingBatch(batch);

                var tasks = new List<Task>();
                foreach (var s in batch)
                {
                    TriggerSendingStatement(s);
                    tasks.Add(HandleSendingAsync(s));
                }

                await Task.WhenAll(tasks);

                AfterHandleSendingBatch(batch);
                
                Dispatch(() => TriggerSentBatch(batch));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);

                foreach (var s in batch)
                {
                    Dispatch(() => TriggerFailedStatement(s));
                    QueuedStatements.Enqueue(s); // Optional requeue
                }

                Dispatch(() => TriggerFailedBatch(batch));
            }
        }

        /// <summary>
        /// Runs the given action on the Unity main thread.
        /// Useful for creating Unity objects or accessing Unity APIs.
        /// </summary>
        protected Task MainThreadAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();
            Dispatch(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        /// Runs the given function on the Unity main thread and returns the result.
        /// </summary>
        protected Task<T> MainThreadAsync<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            Dispatch(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }
    }
}
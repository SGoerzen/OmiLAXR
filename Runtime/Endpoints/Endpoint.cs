using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [DefaultExecutionOrder(-1000)]
    public abstract class Endpoint : DataProviderPipelineComponent
    {
        [ReadOnly]
        public ulong recordedStatements = 0;

        public T GetDataProvider<T>() where T : DataProvider => GetComponentInParent<T>();

        public event EndpointAction OnStartedSending;
        public event EndpointAction OnStoppedSending;
        public event EndpointAction OnPausedSending;
        public event EndpointAction<IStatement> OnSendingStatement;
        public event EndpointAction<IStatement> OnSentStatement;
        public event EndpointAction<List<IStatement>> OnSentBatch;
        public event EndpointAction<IStatement> OnFailedSendingStatement;
        public event EndpointAction<List<IStatement>> OnFailedSendingBatch;

        public bool IsSending { get; private set; }
        public bool IsTransferring { get; private set; }

        private Thread _sendThread;
        protected readonly ConcurrentQueue<IStatement> QueuedStatements = new ConcurrentQueue<IStatement>();
        private readonly object _sendLock = new object();
        private bool _shuttingDown = false;
        private readonly AutoResetEvent _signal = new AutoResetEvent(false);

        protected virtual int MaxBatchSize => 50; // Configurable by subclass

        private void SendWorkerLoop()
        {
            try
            {
                while (!_shuttingDown)
                {
                    var result = HandleQueue();

                    if (_shuttingDown) break;

                    if (result != TransferCode.Success && result != TransferCode.NoStatements)
                    {
                        DebugLog.OmiLAXR?.Error($"Failed to send statements. Error code: " + result);
                    }

                    if (result == TransferCode.InvalidCredentials)
                        break;

                    _signal.WaitOne(result == TransferCode.NoStatements ? 100 : 10);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public virtual void StartSending(bool resetQueue = false)
        {
            try
            {
                if (!enabled || IsSending || _shuttingDown)
                    return;

                lock (_sendLock)
                {
                    DebugLog.OmiLAXR.Print($"🚀({GetType().Name}) started writing statements.");

                    _sendThread = new Thread(SendWorkerLoop)
                    {
                        IsBackground = true,
                        Name = $"EndpointThread-{GetType().Name}"
                    };
                    _sendThread.Start();
                    
                    OnStartedSending?.Invoke(this);
                    
                    if (!resetQueue)
                        FlushQueue();
                    else
                    {
                        #if UNITY_2021_1_OR_NEWER
                        QueuedStatements.Clear();
                        #else 
                        while (QueuedStatements.TryDequeue(out var statement)) {}
                        #endif
                    }
                    
                    IsSending = true;
                    _shuttingDown = false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public virtual void PauseSending()
        {
            lock (_sendLock)
            {
                if (!IsSending)
                    return;

                IsSending = false;
                _shuttingDown = true;
                _signal.Set();
                _sendThread?.Join();
                OnPausedSending?.Invoke(this);
            }
        }

        public virtual void StopSending()
        {
            lock (_sendLock)
            {
                FlushQueue();

                if (!IsSending)
                    return;

                DebugLog.OmiLAXR.Print($"⛔({GetType().Name}) stopped writing statements. {recordedStatements} statements were sent.");

                _shuttingDown = true;
                _signal.Set();
                _sendThread?.Join();
                _sendThread = null;
                IsSending = false;

                OnStoppedSending?.Invoke(this);
            }
        }

        protected override void OnEnable() => StartSending();
        protected virtual void OnDisable() => StopSending();
        private void OnDestroy() => StopSending();

        public virtual void SendStatement(IStatement statement)
        {
            QueuedStatements.Enqueue(statement);
            _signal.Set();
        }

        [Obsolete("Don't use it anymore.", true)]
        public virtual void SendStatementImmediate(IStatement statement)
        {
            QueuedStatements.Enqueue(statement);
        }
        
        public void FlushQueue()
        {
            var count = QueuedStatements.Count;
            var batch = new List<IStatement>();
            while (QueuedStatements.TryDequeue(out var statement))
            {
                batch.Add(statement);
                if (batch.Count >= MaxBatchSize)
                {
                    TransferStatements(batch);
                    batch.Clear();
                }
            }
            
            DebugLog.OmiLAXR.Print($"🪄({GetType().Name}) flushed " + count + " statements.");

            // Send remaining
            if (batch.Count > 0)
                TransferStatements(batch);
        }


        protected abstract TransferCode HandleSending(IStatement statement);

        /// <summary>
        /// Optional batch handling method. Override for actual batch sending.
        /// </summary>
        protected virtual TransferCode HandleSending(List<IStatement> batch)
        {
            if (batch == null || batch.Count == 0)
                return TransferCode.Success;

            try
            {
                BeforeHandleSendingBatch(batch);
                foreach (var statement in batch)
                {
                    var result = HandleSending(statement);
                    if (result == TransferCode.Success)
                        TriggerSentStatement(statement);
                }
                AfterHandleSendingBatch(batch);
                TriggerSentBatch(batch);

                return TransferCode.Success;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);

                foreach (var statement in batch)
                {
                    TriggerFailedStatement(statement);
                    QueuedStatements.Enqueue(statement);
                }

                TriggerFailedBatch(batch);
                return TransferCode.Error;
            }
        }

        protected virtual void BeforeHandleSendingBatch(List<IStatement> batch) { }
        protected virtual void AfterHandleSendingBatch(List<IStatement> batch) { }

        private TransferCode TransferStatements(List<IStatement> batch)
        {
            try
            {
                IsTransferring = true;

                foreach (var statement in batch)
                    TriggerSendingStatement(statement);

                return HandleSending(batch);
            }
            catch (Exception ex)
            {
                if (Application.isPlaying)
                    Debug.LogException(ex);

                return TransferCode.Error;
            }
            finally
            {
                IsTransferring = false;
            }
        }

        protected void TriggerSentStatement(IStatement statement)
        {
#if UNITY_EDITOR
            recordedStatements++;
#endif
            OnSentStatement?.Invoke(this, statement);
        }

        protected void TriggerFailedStatement(IStatement statement)
        {
            OnFailedSendingStatement?.Invoke(this, statement);
        }

        protected void TriggerSentBatch(List<IStatement> batch)
            => OnSentBatch?.Invoke(this, batch);

        protected void TriggerFailedBatch(List<IStatement> batch)
            => OnFailedSendingBatch?.Invoke(this, batch);

        protected void TriggerSendingStatement(IStatement statement)
            => OnSendingStatement?.Invoke(this, statement);

        protected virtual TransferCode HandleQueue()
        {
            var batch = new List<IStatement>();
            while (batch.Count < MaxBatchSize && QueuedStatements.TryDequeue(out var item))
            {
                batch.Add(item);
            }

            return batch.Count > 0
                ? TransferStatements(batch)
                : TransferCode.NoStatements;
        }

        #region MainThreadDispatcher
        private readonly Queue<Action> _executionQueue = new Queue<Action>();

        private void Update()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue()?.Invoke();
                }
            }
        }

        protected void Dispatch(Action action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(action);
            }
        }
        #endregion
    }
}

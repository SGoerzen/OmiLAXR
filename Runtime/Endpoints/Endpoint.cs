using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    public abstract class Endpoint : PipelineComponent
    {
        public event EndpointAction OnStartedSending;
        public event EndpointAction OnStoppedSending;
        public event EndpointAction OnPausedSending;
        public event EndpointAction<IStatement> OnSendingStatement;
        public event EndpointAction<IStatement> OnSentStatement;
        public event EndpointAction<IStatement> OnFailedSendingStatement;
        
        public bool IsSending { get; private set; }
        public bool IsTransferring { get; private set; }
        
        private BackgroundWorker _sendWorker;
        protected readonly ConcurrentQueue<IStatement> QueuedStatements = new ConcurrentQueue<IStatement>();
        
        private void SendWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            while (!_sendWorker.CancellationPending)
            {
                var result = HandleQueue();

                if (result != TransferCode.Success && result != TransferCode.NoStatements)
                {
                    DebugLog.OmiLAXR.Error("Failed to send statement. Error code: " + result);
                }
                
                // handle codes
                switch (result)
                {
                    case TransferCode.NoStatements:
                        break;
                    case TransferCode.Success:
                        break;
                    case TransferCode.InvalidCredentials:
                        StopSending();
                        break;
                    case TransferCode.Error:
                        break;
                    case TransferCode.Busy:
                        break;
                    default:
                        break;
                }
                
                
            }
        }

        private void OnApplicationQuit()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {
            // do nothing
        }

        public void StartSending()
        {
            if (IsSending)
                return;
            
            // Reset tracking time
            IsSending = true;

            _sendWorker = new BackgroundWorker();
            _sendWorker.DoWork += SendWorkerOnDoWork;
            _sendWorker.WorkerSupportsCancellation = true;
            _sendWorker.RunWorkerAsync();
            OnStartedSending?.Invoke(this);
        }

        public void PauseSending()
        {
            if (!IsSending)
                return;
            IsSending = false;
            _sendWorker.CancelAsync();
            OnPausedSending?.Invoke(this);
        }

        public void StopSending()
        {
            if (!IsSending)
                return;
            
            if (_sendWorker != null)
            {
                _sendWorker.CancelAsync();
                _sendWorker.Dispose();
                _sendWorker = null;
            }

            IsSending = false;
            OnStoppedSending?.Invoke(this);
        }

        protected virtual void OnEnable()
        {
            StartSending();
        }
        
        protected virtual void OnDisable()
        {
            StopSending();
        }

        private void OnDestroy()
        {
            StopSending();
        }

        /// <summary>
        /// Enqueue statement to asynchronous sending queue
        /// </summary>
        /// <param name="statement"></param>
        public virtual void SendStatement(IStatement statement)
        {
            // Debug.Log("Enqueue " + statement, this);
            QueuedStatements.Enqueue(statement);
        }
        /// <summary>
        /// Send statement immediate without using queue system.
        /// </summary>
        /// <param name="statement"></param>
        public virtual void SendStatementImmediate(IStatement statement)
        {
            var code = TransferStatement(statement);
            if (code != TransferCode.Success && code != TransferCode.NoStatements)
            {
                DebugLog.OmiLAXR.Error("Failed to send statement. Error code: " + code);
            }
        }

        protected abstract TransferCode HandleSending(IStatement statement);

        protected virtual TransferCode TransferStatement(IStatement statement)
        {
            try
            {
                IsTransferring = true;
                OnSendingStatement?.Invoke(this, statement);
                var result = HandleSending(statement);
                IsTransferring = false;

                if (result != TransferCode.Success)
                {
                    OnFailedSendingStatement?.Invoke(this, statement);
                    // enqueue again
                    QueuedStatements.Enqueue(statement);
                }
                else
                {
                    OnSentStatement?.Invoke(this, statement);
                }
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            return TransferCode.Error;
        }
        
        protected virtual TransferCode HandleQueue()
        {
            if (!QueuedStatements.TryDequeue(out var statement))
                return TransferCode.NoStatements;

            return TransferStatement(statement);
        }

        
        #region MainThreadDispatcher
        private readonly Queue<Action> _executionQueue = new Queue<Action>();
        private void Update()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    Action action = null;
                    action = _executionQueue.Dequeue();
                    action?.Invoke();
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
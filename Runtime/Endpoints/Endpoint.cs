using System;
using System.Collections.Generic;
using System.ComponentModel;
using OmiLAXR.Composers;

namespace OmiLAXR.Endpoints
{
    public abstract class Endpoint : PipelineComponent
    {
        public EndpointAction onStartedSending;
        public EndpointAction onStoppedSending;
        public EndpointAction onPausedSending;
        public EndpointAction<IStatement> onSentStatement;
        public EndpointAction<IStatement> onFailedSendingStatement;

        private bool _isSending;
        
        private BackgroundWorker _sendWorker;
        private readonly Queue<IStatement> _queuedStatements = new Queue<IStatement>();
        
        private void SendWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            while (!_sendWorker.CancellationPending)
            {
                var result = TransferStatement();

                if (result != TransferCode.Success && result != TransferCode.NoStatements)
                {
                    DebugLog.OmiLAXR.Error("Failed to send statements. Error code: " + result);
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
                    default:
                        break;
                }
                
                
            }
        }

        public void StartSending()
        {
            // Reset tracking time
            _isSending = true;

            _sendWorker = new BackgroundWorker();
            _sendWorker.DoWork += SendWorkerOnDoWork;
            _sendWorker.WorkerSupportsCancellation = true;
            _sendWorker.RunWorkerAsync();
            onStartedSending?.Invoke(this);
        }

        public void PauseSending()
        {
            _isSending = false;
            _sendWorker.CancelAsync();
            onPausedSending.Invoke(this);
        }

        public void StopSending()
        {
            if (_sendWorker != null)
            {
                _sendWorker.CancelAsync();
                _sendWorker.Dispose();
                _sendWorker = null;
            }

            _isSending = false;
            onStoppedSending?.Invoke(this);
        }

        private void OnEnable()
        {
            StartSending();
        }
        
        private void OnDisable()
        {
            StopSending();
        }

        private void OnDestroy()
        {
            StopSending();
        }

        public virtual void SendStatement(IStatement statement)
        {
            lock (_queuedStatements)
            {
                _queuedStatements.Enqueue(statement);
            }
        }

        protected abstract TransferCode HandleSending(IStatement statement);
        
        protected virtual TransferCode TransferStatement()
        {
            lock (_queuedStatements)
            {
                if (_queuedStatements.Count < 1)
                    return TransferCode.NoStatements;

                var statement = _queuedStatements.Dequeue();

                var result = HandleSending(statement);

                if (result != TransferCode.Success)
                {
                    onFailedSendingStatement?.Invoke(this, statement);
                    // enqueue again
                    _queuedStatements.Enqueue(statement);
                }
                else
                {
                    onSentStatement?.Invoke(this, statement);
                }

                return result;
            }
        }

    }
}
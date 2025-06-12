using UnityEngine;

namespace OmiLAXR.Endpoints
{
    public abstract class BasicAuthEndpoint : Endpoint
    {
        [SerializeField]
        private BasicAuthCredentials _credentials = new BasicAuthCredentials("https://lrs.elearn.rwth-aachen.de/data/xAPI", "", "");
        
        protected override TransferCode HandleQueue()
        {
            // Check credentials
            return !_credentials.IsValid ? TransferCode.InvalidCredentials : base.HandleQueue();
        }

        public BasicAuthCredentials Credentials
        {
            get => _credentials;
            set => _credentials = value;
        }
    }
}
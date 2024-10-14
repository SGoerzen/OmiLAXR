namespace OmiLAXR.Endpoints
{
    public abstract class BasicAuthEndpoint : Endpoint
    {
        public BasicAuthCredentials credentials = new BasicAuthCredentials("https://lrs.elearn.rwth-aachen.de/data/xAPI", "", "");
        
        protected override TransferCode HandleQueue()
        {
            // Check credentials
            if (!credentials.IsValid)
                return TransferCode.InvalidCredentials;
            
            return base.HandleQueue();
        }
    }
}
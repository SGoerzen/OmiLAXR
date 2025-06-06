using OmiLAXR.Composers;

namespace OmiLAXR.Endpoints
{
    public abstract class BasicAuthEndpoint : BasicAuthEndpoint<IStatement> {}
    public abstract class BasicAuthEndpoint<TStatement> : Endpoint<TStatement>
    where TStatement : IStatement
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
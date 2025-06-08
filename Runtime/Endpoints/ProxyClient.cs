using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    public class ProxyClient<TStatement> : Endpoint<TStatement>
    where TStatement : IStatement
    {
        [SerializeField] private string proxyUrl = "https://your-api.com/xapi/statements";
        public string token = "<your_auth_token>";

        void Start()
        {
            token = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/token.txt");
        }
        protected override TransferCode HandleSending(TStatement statement)
        {
            
        }
    }
}
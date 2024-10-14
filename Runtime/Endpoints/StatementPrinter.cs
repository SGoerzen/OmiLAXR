using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Statement Printer")]
    public class StatementPrinter : Endpoint
    {
        protected override TransferCode HandleSending(IStatement statement)
        {
            DebugLog.OmiLAXR.Print("Sent statement: " + statement);
            return TransferCode.Success;
        }
    }
}
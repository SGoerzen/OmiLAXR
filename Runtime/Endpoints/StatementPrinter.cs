using System.ComponentModel;
using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Statement Printer")]
    [Description("Prints all received statements to Unity Editor console. May be used for testing purposes.")]
    public class StatementPrinter : Endpoint
    {
        protected override TransferCode HandleSending(IStatement statement)
        {
            DebugLog.OmiLAXR.Print("Sent statement: " + statement);
            return TransferCode.Success;
        }
    }
}
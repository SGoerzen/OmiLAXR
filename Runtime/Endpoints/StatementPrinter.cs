using System.ComponentModel;
using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Statement Printer")]
    [Description("Prints all received statements to Unity Editor console. May be used for testing purposes.")]
    public class StatementPrinter : Endpoint
    {
        public bool printAsCsv = false;
        protected override TransferCode HandleSending(IStatement statement)
        {
            if (printAsCsv)
            {
                var csvFormat = statement.ToCsvFormat(false);
                DebugLog.OmiLAXR.Print("Sent statement: " + csvFormat);
            }
            else
            {
                DebugLog.OmiLAXR.Print("Sent statement: " + statement);
            }
            return TransferCode.Success;
        }
    }
}
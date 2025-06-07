using System;
using System.ComponentModel;
using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Statement Printer")]
    [Description("Prints all received statements to Unity Editor console. May be used for testing purposes.")]
    public class StatementPrinter : Endpoint
    {
        public bool includeOrigin = true;
        [Serializable]
        public enum PrintType
        {
            Default,
            Json,
            Csv,
            CsvFlat
        }
        public PrintType printType = PrintType.Default;
        protected override TransferCode HandleSending(IStatement statement)
        {
            if (!includeOrigin)
                statement.SetOrigin("/");
            
            switch (printType)
            {
                case PrintType.Json:
                    PrintStatement(statement.ToJsonString());
                    break;
                case PrintType.Csv:
                {
                    var csvFormat = statement.ToCsvFormat() ;
                    PrintStatement(csvFormat.ToString());
                    break;
                }
                case PrintType.CsvFlat:
                {
                    var csvFormat = statement.ToCsvFormat(true);
                    DebugLog.OmiLAXR.Print(csvFormat.ToString());
                    break;
                }
                default:
                    PrintStatement(statement.ToString());
                    break;
            }

            return TransferCode.Success;
        }
        
        private static void PrintStatement(string message)
            => DebugLog.OmiLAXR.Print("Sent statement: " + message);
    }
}
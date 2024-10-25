using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [Description("Stores statements on local path.")]
    public class LocalEndpoint : Endpoint
    {
        [Header("Set a path where the files get stored. If no path is specified, the path will be stored in a Temp directory.")]
        public string storeLocation;

        [Header("Set a filename where the statements are stored. If no filename is specified, the filename will be generated automatically.")]
        public string fileName;
        
        private string _tempFilePath = "";
        
        private StreamWriter _streamWriter;
        
        private void Awake()
        {
            if (string.IsNullOrEmpty(storeLocation))
                storeLocation = TempFolder;

            if (!Directory.Exists(storeLocation))
                Directory.CreateDirectory(storeLocation);

            var now = DateTime.Now;
            var filename = $"{now.Year}{now.Month:00}{now.Day:00}{now.Hour:00}{now.Minute:00}{now.Second:00}";

            fileName = string.IsNullOrEmpty(fileName) ? filename + ".txt" : fileName;
            
            _tempFilePath = Path.Combine(storeLocation, fileName);

            _streamWriter = new StreamWriter(_tempFilePath, true);
            _streamWriter.AutoFlush = true;
            
            DebugLog.OmiLAXR.Print($"Started writing to local endpoint in '{_tempFilePath}'.");
        }

        public static string TempFolder => Path.Combine(Path.GetTempPath(), "OmiLAXR");
        
        protected override TransferCode HandleSending(IStatement statement)
        {
            try
            {
                _streamWriter.WriteLine(statement.ToDataStandardString());
            }
            catch (IOException ex)
            {
                Debug.LogException(ex);
                return TransferCode.Error;
            }
            return TransferCode.Success;
        }
    }
}
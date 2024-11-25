using System;
using System.ComponentModel;
using System.IO;
using OmiLAXR.Composers;
using UnityEngine;
using UnityEngine.Serialization;

namespace OmiLAXR.Endpoints
{
    [Description("Stores statements on local path.")]
    public class LocalEndpoint : Endpoint
    {
        public enum DefaultFolderPaths
        {
            PersistantDataPath,
            TempFolder,
            Custom
        }

        public DefaultFolderPaths defaultFolder = DefaultFolderPaths.PersistantDataPath;
        
        [FormerlySerializedAs("storeLocation")] 
        [Header("Set a path where the files get stored. Will be applied if 'Location Target' is 'Custom'.")]
        public string customLocation;

        [ReadOnly] 
        public string location;
        
        [Header("Set a filename where the statements are stored. If no filename is specified, the filename will be generated automatically.")]
        public string fileName;
        
        private string _tempFilePath = "";
        
        private StreamWriter _streamWriter;
        
        protected override void Awake()
        {
            if (string.IsNullOrEmpty(customLocation))
                customLocation = DefaultFolderPath;

            if (!Directory.Exists(customLocation))
                Directory.CreateDirectory(customLocation);

            var now = DateTime.Now;
            var filename = $"{now.Year}{now.Month:00}{now.Day:00}{now.Hour:00}{now.Minute:00}{now.Second:00}";

            fileName = string.IsNullOrEmpty(fileName) ? filename + ".txt" : fileName;
            
            location = _tempFilePath = Path.Combine(customLocation, fileName);

            _streamWriter = new StreamWriter(_tempFilePath, true);
            _streamWriter.AutoFlush = true;
            
            DebugLog.OmiLAXR.Print($"Started writing to local endpoint in '{_tempFilePath}'.");
        }

        public string GetDefaultFolder()
        {
            switch (defaultFolder)
            {
                case DefaultFolderPaths.PersistantDataPath:
                    return Application.persistentDataPath;
                case DefaultFolderPaths.TempFolder:
                    return Path.GetTempPath();
                case DefaultFolderPaths.Custom:
                    return customLocation;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string DefaultFolderPath => Path.Combine(GetDefaultFolder(), "OmiLAXR");
        
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
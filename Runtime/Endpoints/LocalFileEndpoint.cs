using System;
using System.ComponentModel;
using System.IO;
using OmiLAXR.Composers;
using UnityEngine;
using UnityEngine.Serialization;

namespace OmiLAXR.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Local File Endpoint")]
    [Description("Stores statements on local path.")]
    public class LocalFileEndpoint : Endpoint
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
        [HideInInspector]
        public string customLocation;

        [ReadOnly, SerializeField, TextArea] 
        private string fileLocationPreview = "yyyymmddhhmmss.txt";
        
        [Header("Default: Generated automatically based on current time.")]
        public string fileName;
        
        private string _tempFilePath = "";
        
        private StreamWriter _streamWriter;

        private static string GenerateFileName(DateTime now)
            => GenerateFileName(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

        private static string GenerateFileName(int year, int month, int day, int hour, int minute, int second)
            => $"{year}{month:00}{day:00}{hour:00}{minute:00}{second:00}";

        protected override void Awake()
        {
            if (string.IsNullOrEmpty(customLocation))
                customLocation = DefaultFolderPath;

            if (!Directory.Exists(customLocation))
                Directory.CreateDirectory(customLocation);

            var now = DateTime.Now;
            var filename = GenerateFileName(now);

            fileName = string.IsNullOrEmpty(fileName) ? filename + ".txt" : fileName;
            
             _tempFilePath = Path.Combine(customLocation, fileName);
             
             fileLocationPreview = Path.GetFullPath(_tempFilePath);

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

        private string GetFileLocationPreview()
        {
            var folder = GetDefaultFolder();
            var n = string.IsNullOrEmpty(fileName) ? "yyyymmddhhmmss.txt" : fileName;
            return Path.Combine(folder, n);
        }
        
        public void UpdateFileLocationPreview()
        {
            fileLocationPreview = GetFileLocationPreview();
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
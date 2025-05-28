using System;
using System.ComponentModel;
using System.IO;
using OmiLAXR.Composers;
using UnityEngine;

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

        [Header("Relative to project root (only used when 'Custom' is selected)")]
        [HideInInspector]
        public string customLocation;
        
        public string fileExtension = "txt";
        
        [Header("If true, the statements will be split into multiple files organized by composers.")]
        public bool splitByComposers = false;
        
        [ReadOnly, SerializeField, TextArea]
        private string exampleFileLocationPreview = "yyyymmddhhmmss.txt";

        [Header("Default: Generated automatically based on current time.")]
        public string fileName;

        private string _resolvedFilePath;
        private StreamWriter _streamWriter;

        private string GenerateFileName(DateTime now)
        {
            return string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}.{6}",
                now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, fileExtension);
        }
        
        private void OnValidate()
        {
#if UNITY_EDITOR
            var absPath = GetResolvedFilePath();
            var relPath = MakeRelativeToProject(absPath);
            exampleFileLocationPreview = relPath;
#endif
        }


        private string MakeRelativeToProject(string absolutePath)
        {
            var projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            Uri projectUri = new Uri(projectRoot + Path.DirectorySeparatorChar);
            Uri fileUri = new Uri(absolutePath);
            string relative = Uri.UnescapeDataString(projectUri.MakeRelativeUri(fileUri).ToString());
            return relative.Replace('/', Path.DirectorySeparatorChar);
        }


        private static string ProjectRoot
        {
            get { return Path.GetFullPath(Path.Combine(Application.dataPath, "..")); }
        }

        public static string ResolveRelativeToProject(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(ProjectRoot, relativePath ?? ""));
        }

        private string GetResolvedFolder()
        {
            if (defaultFolder == DefaultFolderPaths.PersistantDataPath)
            {
                return Application.persistentDataPath;
            }
            else if (defaultFolder == DefaultFolderPaths.TempFolder)
            {
                return Path.GetTempPath();
            }
            else if (defaultFolder == DefaultFolderPaths.Custom)
            {
                return ResolveRelativeToProject(customLocation);
            }

            throw new ArgumentOutOfRangeException();
        }

        public string DefaultFolderPath
        {
            get { return Path.Combine(GetResolvedFolder(), "OmiLAXR"); }
        }

        private string GetResolvedFilePath()
        {
            var folder = GetResolvedFolder();
            var n = string.IsNullOrEmpty(fileName) ? GenerateFileName(DateTime.Now) : string.Format("{0}.{1}", fileName, fileExtension);
            return Path.Combine(folder, n);
        }

        public void UpdateFileLocationPreview()
        {
#if UNITY_EDITOR
            exampleFileLocationPreview = GetResolvedFilePath();
#endif
        }

        protected override void Awake()
        {
            var fullPath = GetResolvedFilePath();

            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            _resolvedFilePath = fullPath;
            exampleFileLocationPreview = _resolvedFilePath;

            _streamWriter = new StreamWriter(_resolvedFilePath, true);
            _streamWriter.AutoFlush = true;

            DebugLog.OmiLAXR.Print("Started writing to local endpoint in '" + _resolvedFilePath + "'.");
        }

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

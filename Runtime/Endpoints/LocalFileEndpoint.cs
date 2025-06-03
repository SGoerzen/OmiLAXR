using System;
using System.Collections.Generic;
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

        [Header("If true, the statements will be split into multiple files named by composers.")]
        public bool splitByComposers = false;
        
        [ReadOnly, SerializeField, TextArea]
        private string exampleFileLocationPreview = "yyyymmddhhmmss.txt";

        [Header("Default: Generated automatically based on current time.")]
        public string fileName;

        private string _resolvedFilePath;
        private StreamWriter _streamWriter;

        private Dictionary<string, StreamWriter> _composerWriters = new Dictionary<string, StreamWriter>();

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
            var projectUri = new Uri(projectRoot + Path.DirectorySeparatorChar);
            var fileUri = new Uri(absolutePath);
            var relative = Uri.UnescapeDataString(projectUri.MakeRelativeUri(fileUri).ToString());
            return relative.Replace('/', Path.DirectorySeparatorChar);
        }

        private static string ProjectRoot => Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        public static string ResolveRelativeToProject(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(ProjectRoot, relativePath ?? ""));
        }

        private string GetResolvedFolder()
        {
            if (defaultFolder == DefaultFolderPaths.PersistantDataPath)
                return Application.persistentDataPath;
            if (defaultFolder == DefaultFolderPaths.TempFolder)
                return Path.GetTempPath();
            if (defaultFolder == DefaultFolderPaths.Custom)
                return ResolveRelativeToProject(customLocation);

            throw new ArgumentOutOfRangeException();
        }

        public string DefaultFolderPath => Path.Combine(GetResolvedFolder(), "OmiLAXR");

        private string GetResolvedFilePath()
        {
            var folder = GetResolvedFolder();
            var name = string.IsNullOrEmpty(fileName)
                ? GenerateFileName(DateTime.Now)
                : string.Format("{0}.{1}", fileName, fileExtension);
            return Path.Combine(folder, name);
        }

        public void UpdateFileLocationPreview()
        {
#if UNITY_EDITOR
            var baseFolder = GetResolvedFolder();

            if (splitByComposers)
            {
                var folder = Path.Combine(baseFolder, fileName);
                var exampleComposer = "{composer_name}";
                var exampleFile = Path.Combine(folder, exampleComposer + "." + fileExtension);
                exampleFileLocationPreview = MakeRelativeToProject(exampleFile);
            }
            else
            {
                exampleFileLocationPreview = MakeRelativeToProject(GetResolvedFilePath());
            }
#endif
        }


        protected override void Awake()
        {
            var fullPath = GetResolvedFilePath();
            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _resolvedFilePath = fullPath;
            exampleFileLocationPreview = _resolvedFilePath;

            if (!splitByComposers)
            {
                _streamWriter = new StreamWriter(_resolvedFilePath, true)
                {
                    AutoFlush = true
                };

                DebugLog.OmiLAXR.Print("Started writing to local endpoint in '" + _resolvedFilePath + "'.");
            }
        }

        protected virtual string FormatLine(IStatement statement)
        {
            return statement.ToDataStandardString();
        }

        protected override TransferCode HandleSending(IStatement statement)
        {
            try
            {
                if (splitByComposers)
                {
                    var composerName = statement.GetComposer().GetName();
                    if (string.IsNullOrWhiteSpace(composerName))
                        composerName = "Unknown";

                    var folder = Path.Combine(GetResolvedFolder(), fileName);
                    var filePath = Path.Combine(folder, composerName + "." + fileExtension);

                    if (!_composerWriters.TryGetValue(composerName, out var writer))
                    {
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        writer = new StreamWriter(filePath, true)
                        {
                            AutoFlush = true
                        };
                        _composerWriters[composerName] = writer;

                        DebugLog.OmiLAXR.Print("Created writer for '" + composerName + "' in '" + filePath + "'");
                    }

                    writer.WriteLine(FormatLine(statement));
                }
                else
                {
                    _streamWriter.WriteLine(FormatLine(statement));
                }
            }
            catch (IOException ex)
            {
                Debug.LogException(ex);
                return TransferCode.Error;
            }

            return TransferCode.Success;
        }

        private void OnDestroy()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Close();
                _streamWriter.Dispose();
            }

            foreach (var writer in _composerWriters.Values)
            {
                writer?.Close();
                writer?.Dispose();
            }

            _composerWriters.Clear();
        }
    }
}

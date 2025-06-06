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

        protected virtual string GetDefaultExtension() => "txt";
        public string fileExtension = "";

        [Header("If true, the statements will be split into multiple files named by composers.")]
        public bool splitByComposers = false;
        
        [ReadOnly, SerializeField, TextArea]
        private string exampleFileLocationPreview = "yyyymmddhhmmss.txt";

        [Header("Default: Generated automatically based on current time.")]
        public string fileName;

        private string _resolvedFilePath;
        private StreamWriter _streamWriter;
        private bool _isFirstLine;

        private readonly Dictionary<int, StreamWriter> _composerWriters = new Dictionary<int, StreamWriter>();

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
            
            if (string.IsNullOrEmpty(fileExtension))
                fileExtension = GetDefaultExtension();
#endif
        }

        protected virtual void Reset()
        {
            if (string.IsNullOrEmpty(fileExtension))
                fileExtension = GetDefaultExtension();
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

        private (StreamWriter writer, bool hasContent) CreateWriter(string filePath)
        {
            var info = new FileInfo(filePath);
            var hasContent = info.Length > 0;
            var writer = new StreamWriter(filePath, true)
            {
                AutoFlush = false
            };
            return (writer, hasContent);
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
                var (writer, hasContent) = CreateWriter(_resolvedFilePath);
                _streamWriter = writer;
                _isFirstLine = !hasContent;

                DebugLog.OmiLAXR.Print("Started writing to local endpoint in '" + _resolvedFilePath + "'.");
            }
        }

        protected virtual string FormatLine(IStatement statement)
        {
            return statement.ToJsonString();
        }

        protected virtual void BeforeWrite(StreamWriter writer, IStatement statement, bool isFirstLine)
        {
            // do nothing
        }

        protected virtual void AfterWrite(StreamWriter writer, IStatement statement, bool isFirstLine)
        {
            // do nothing
        }

        protected override TransferCode HandleSending(IStatement statement)
        {
            try
            {
                var isFirstLine = false;
                if (splitByComposers)
                {
                    var composer = statement.GetComposer();
                    var composerHashId = composer.GetHashCode();
                    
                    if (!_composerWriters.TryGetValue(composerHashId, out var writer))
                    {
                        var composerName = statement.GetComposer().GetName();
                        if (string.IsNullOrWhiteSpace(composerName))
                            composerName = "Unknown";

                        var folder = Path.Combine(GetResolvedFolder(), fileName);
                        var filePath = Path.Combine(folder, composerName + "." + fileExtension);
                        
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        var (w, hasContent) = CreateWriter(filePath);
                        writer = w;
                        isFirstLine = !hasContent;
                        _composerWriters[composerHashId] = writer;

                        DebugLog.OmiLAXR.Print("Created writer for '" + composerName + "' in '" + filePath + "'");
                    }

                    BeforeWrite(writer, statement, isFirstLine);
                    writer.WriteLine(FormatLine(statement));
                    writer.Flush();
                    AfterWrite(writer, statement, isFirstLine);
                }
                else
                {
                    BeforeWrite(_streamWriter, statement, _isFirstLine);
                    _streamWriter.WriteLine(FormatLine(statement));
                    _streamWriter.Flush();
                    AfterWrite(_streamWriter, statement, _isFirstLine);
                    _isFirstLine = false;
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
                _streamWriter.Flush();
                _streamWriter.Close();
                _streamWriter.Dispose();
            }

            foreach (var writer in _composerWriters.Values)
            {
                writer?.Flush();
                writer?.Close();
                writer?.Dispose();
            }

            _composerWriters.Clear();
        }
    }
}

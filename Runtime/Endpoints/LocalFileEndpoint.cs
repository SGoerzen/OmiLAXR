using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using OmiLAXR.Composers;
using OmiLAXR.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace OmiLAXR.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / (JSONL) File Endpoint")]
    [Description("Stores statements as JSONL (JSON per Line) on local path.")]
    public class LocalFileEndpoint : Endpoint
    {
        public enum DefaultFolderPaths
        {
            PersistantDataPath,
            TempFolder,
            DesktopFolder,
            DocumentsFolder,
            HomeFolder,
            Custom
        }

        public DefaultFolderPaths defaultFolder = DefaultFolderPaths.DesktopFolder;

        public string statementsFolder = "omilaxr_statements";
        
        [Header("Relative to project root (only used when 'Custom' is selected)")] [HideInInspector]
        public string customLocation;

        protected virtual string GetExtension() => "jsonl";

        [Header("If true, the statements will be split into multiple files named by composers.")]
        public bool oneFilePerComposer = false;
        
        [SerializeField, FormerlySerializedAs("fileName")] [Header("Identification of the tracking session. You can use '{yyyyMMddHHmmss}' (default) to format current timestamp format.")]
        private string identifier = "{yyyyMMddHHmmss}";
        
        private string _resolvedIdentifier;

        public string ResolvedIdentifier
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_resolvedIdentifier))
                    return _resolvedIdentifier;
                if (string.IsNullOrWhiteSpace(identifier))
                    identifier = "{yyyyMMddHHmmss}";
                return _resolvedIdentifier = IsInTimeFormat(identifier) ? GenerateIdentifier(DateTime.Now) : _resolvedIdentifier;
            }
        }
        
        protected readonly FileBufferManager FileBufferManager = new FileBufferManager();

        protected int ComposersCount { get; private set; }
        
        protected string GenerateIdentifier(DateTime now)
        {
            var format = identifier.Substring(1, identifier.Length - 2);
            return now.ToString(format);
        }

        private static string ProjectRoot => Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        public static string ResolveRelativeToProject(string relativePath)
            => Path.GetFullPath(Path.Combine(ProjectRoot, relativePath ?? ""));

        protected string GetResolvedFolder()
        {
            return defaultFolder switch
            {
                DefaultFolderPaths.PersistantDataPath => Application.persistentDataPath,
                DefaultFolderPaths.TempFolder => Path.GetTempPath(),
                DefaultFolderPaths.DesktopFolder => Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                DefaultFolderPaths.DocumentsFolder => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DefaultFolderPaths.HomeFolder => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                DefaultFolderPaths.Custom => ResolveRelativeToProject(customLocation),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private bool IsInTimeFormat(string ident)
        #if UNITY_2021_1_OR_NEWER
            => ident.EndsWith('}') && ident.StartsWith("{");
        #else
            => ident.EndsWith("}") && ident.StartsWith("{");
        #endif

        private string GetResolvedSingleFilePath()
        {
            var folder = Path.Combine(GetResolvedFolder(), statementsFolder);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var n = $"{ResolvedIdentifier}.{GetExtension()}";
            return Path.Combine(folder, n);
        }

        public string ResolvedStatementsFolderPath => Path.Combine(GetResolvedFolder(), statementsFolder);
        
        protected (int, string) GetIdAndPath(IComposer composer)
        {
            if (!oneFilePerComposer)
                return (0, GetResolvedSingleFilePath());
            
            var composerHashId = composer.GetHashCode();
            var composerName = composer.GetName();
            if (string.IsNullOrWhiteSpace(composerName))
                composerName = "Unknown";
            
            var folder = Path.Combine(ResolvedStatementsFolderPath, ResolvedIdentifier);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            
            var filePath = Path.Combine(folder, composerName + "." + GetExtension());
            return (composerHashId, filePath);
        }

        protected override void AfterHandleSendingBatch(List<IStatement> batch)
        {
            FileBufferManager.FlushAll();
        }
        
        protected override TransferCode HandleSending(IStatement statement)
        {
            try
            {
                var (id, filePath) = GetIdAndPath(statement.GetComposer());
                var buffer = FileBufferManager.EnsureBuffer(id, filePath);
                buffer.Writer.WriteLine(FormatLine(statement));
                buffer.Writer.Flush();
            }
            catch (IOException ex)
            {
                Debug.LogException(ex);
                return TransferCode.Error;
            }

            return TransferCode.Success;
        }

        protected virtual void OnDestroy()
        {
            FileBufferManager.Dispose();
        }
        protected override void OnDisable()
        {
            FileBufferManager.Dispose();
        }
        protected virtual string FormatLine(IStatement statement) => statement.ToJsonString();

        protected override void OnEnable()
        {
            // ensure files
            var composers = GetDataProvider<DataProvider>().GetComponentsInChildren<IComposer>();
#if UNITY_2021_1_OR_NEWER
            FileBufferManager.FileBuffers.EnsureCapacity(composers.Length);
#endif
            ComposersCount = composers.Length;
            base.OnEnable();
        }

#if UNITY_EDITOR
        public string EditorPreviewFilePath { get; private set; } = "{yyyyMMddHHmmss}.jsonl";
        public void UpdateFileLocationPreview()
        {
            if (oneFilePerComposer)
            {
                var folder = Path.Combine(ResolvedStatementsFolderPath, identifier);
                var exampleComposer = "{composer_name}";
                var exampleFile = Path.Combine(folder, exampleComposer + "." + GetExtension());
                EditorPreviewFilePath = Path.GetFullPath(exampleFile);
            }
            else
            {
                var n = $"{identifier}.{GetExtension()}";
                EditorPreviewFilePath = Path.GetFullPath(Path.Combine(ResolvedStatementsFolderPath, n));
            }
        }
        private void OnValidate()
        {
            UpdateFileLocationPreview();
        }
        #endif
    }
}
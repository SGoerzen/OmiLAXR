using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using OmiLAXR.Composers;
using OmiLAXR.Utils;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / (CSV) File Endpoint"),
     Description("Stores statements as CSV on local path.")]
    public class CsvFileEndpoint : LocalFileEndpoint
    {
        [Serializable]
        public struct FilterOptions
        {
            [Header("Headers include Regex pattern. Keep empty or write '.*' to include all headers.")]
            public string includePattern;
            [Header("Headers exclude Regex pattern. Keep empty to keep all headers.")]
            public string excludePattern;
        }
        
        [Header("If true, deep structures will be flattened into a single row. May cost more performance.")]
        public bool flatten = true;
        
        [Header("Options for filtering headers.")]
        public FilterOptions filterOptions;
        
        protected override string GetExtension() => "csv";
        
        private Regex _includeRegex;
        private Regex _excludeRegex;
        
        protected override int MaxBatchSize => 100;
        
        private class CsvBuffer
        {
            public CsvFormat CsvFormat;
            public StreamWriter HeaderWriter;
            public int LastHeaderCount;
            public string FilePath;
        }
        private CsvFormat GetDefaultCsvFormat()
        {
            _includeRegex ??= string.IsNullOrEmpty(filterOptions.includePattern) ? null : new Regex(filterOptions.includePattern, RegexOptions.Compiled);
            _excludeRegex ??= string.IsNullOrEmpty(filterOptions.excludePattern) ? null : new Regex(filterOptions.excludePattern, RegexOptions.Compiled);
            return new CsvFormat(MaxBatchSize)
            {
                IncludedHeaderPattern = _includeRegex,
                ExcludedHeaderPattern = _excludeRegex
            };
        }

        private CsvBuffer GetDefaultCsvBuffer(string filePath)
        {
            return new CsvBuffer
            {
                CsvFormat = GetDefaultCsvFormat(),
                HeaderWriter = new StreamWriter(filePath) { AutoFlush = false },
                LastHeaderCount = 0,
                FilePath = filePath
            };
        }

        protected override void AfterHandleSendingBatch(List<IStatement> batch)
        {
            // write buffers to files
            foreach (var pair in FileBufferManager.FileBuffers)
            {
                WriteBufferToFile(pair.Value);
            }
        }

        protected override string FormatLine(IStatement statement)
        {
            throw new NotImplementedException("Don't use this method. Use 'ToCsvFormat' instead.");
        }

        protected override void OnDestroy()
        {
            MergeCsvHeaderAndData();
            base.OnDestroy();
        }

        protected override void OnDisable()
        {
            MergeCsvHeaderAndData();
            base.OnDisable();
        }

        private void MergeCsvHeaderAndData()
        {
            foreach (var pair in FileBufferManager.FileBuffers)
            {
                var csvBuffer = (CsvBuffer)pair.Value.Data;
                var mainFilePath = csvBuffer.FilePath;
                var dataFilePath = mainFilePath + ".data";
                
                // dispose all streams
                if (pair.Value.Writer != null)
                {
                    pair.Value.Writer.Flush();
                    pair.Value.Writer.Dispose();
                    pair.Value.Writer = null;
                }

                if (csvBuffer.HeaderWriter != null)
                {
                    csvBuffer.HeaderWriter.Flush();
                    csvBuffer.HeaderWriter.Dispose();
                    csvBuffer.HeaderWriter = null;
                }
                
                using (var targetStream = new FileStream(mainFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
                using (var sourceStream = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    sourceStream.CopyTo(targetStream);
                }
                
                File.Delete(dataFilePath);
            }
        }

        private void WriteBufferToFile(FileBufferManager.FileBuffer buffer)
        {
            var csvBuffer = (CsvBuffer)buffer.Data;
            var csv = csvBuffer.CsvFormat;
            // skip if no rows in buffer
            if (csv.Rows.Count == 0)
                return;

            if (csv.Headers.Count > csvBuffer.LastHeaderCount)
            {
                var headerWriter = csvBuffer.HeaderWriter;
                headerWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                headerWriter.WriteLine(csv.GetHeadersString());
                csvBuffer.LastHeaderCount = csv.Headers.Count;
                headerWriter.Flush();
            }
            
            // write new rows
            buffer.Writer.WriteLines(csv.GetRowStrings());
            buffer.Writer.Flush();
            // clear rows to have a batch in next round, but keeping headers
            csv.ClearRows();
        }
        
        protected override TransferCode HandleSending(IStatement statement)
        {
            try
            {
                var (id, filePath) = GetIdAndPath(statement.GetComposer());
                var fb = FileBufferManager.EnsureBuffer(id, filePath + ".data", () => GetDefaultCsvBuffer(filePath));
                // append statement to buffer
                fb.GetValue<CsvBuffer>().CsvFormat.Append(statement.ToCsvFormat(flatten));

                return TransferCode.Success;
            }
            catch (IOException ex)
            {
                Debug.LogException(ex);

                TriggerFailedStatement(statement);
                QueuedStatements.Enqueue(statement);
                
                return TransferCode.Error;
            }
        }
    }
}
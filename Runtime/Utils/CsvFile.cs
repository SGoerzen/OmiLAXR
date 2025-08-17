/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Utils
{
    /// <summary>
    /// Utility class for writing CSV files with automatic formatting and optional auto-flushing.
    /// Extends BufferedUtf8Writer to provide CSV-specific functionality like header management
    /// and row writing with proper CSV formatting and escaping.
    /// </summary>
    public class CsvFile : BufferedUtf8Writer
    {
        /// <summary>
        /// Array of column headers for the CSV file.
        /// Set during initialization or via SetHeaders method.
        /// </summary>
        public string[] Headers { get; private set; }
        
        /// <summary>
        /// Controls whether the file buffer is automatically flushed after each write operation.
        /// When true, ensures immediate disk writes but may impact performance for frequent writes.
        /// </summary>
        public bool AutoFlush { get; set; }
        
        /// <summary>
        /// Initializes a new CSV file with optional headers and configuration.
        /// </summary>
        /// <param name="path">File path where the CSV will be written</param>
        /// <param name="headers">Optional array of column headers to write immediately</param>
        /// <param name="autoFlush">Whether to flush after each write operation</param>
        /// <param name="bufferSize">Size of the internal buffer in bytes</param>
        public CsvFile(string path, string[] headers = null, bool autoFlush = false, int bufferSize = 8192) : base(path, bufferSize)
        {   
            // Write headers immediately if provided
            if (headers != null)
                SetHeaders(headers);
            AutoFlush = autoFlush;
        }

        /// <summary>
        /// Sets the column headers and writes them as the first row of the CSV file.
        /// Overwrites any existing headers and writes the header row immediately.
        /// </summary>
        /// <param name="headers">Array of header names to set and write</param>
        public void SetHeaders(params string[] headers)
        {
            Headers = headers;
            // Write headers as comma-separated values
            WriteLine(string.Join(",", headers));
            
            // Flush immediately if auto-flush is enabled
            if (AutoFlush)
                Flush();
        }

        /// <summary>
        /// Writes a data row to the CSV file with automatic comma separation and optional flushing.
        /// Converts all values to strings and joins them with commas.
        /// </summary>
        /// <param name="values">Array of values to write as a CSV row</param>
        public void WriteRow(params object[] values)
        {
            // Convert all values to strings and join with commas
            WriteLine(string.Join(",", values));
            
            // Flush immediately if auto-flush is enabled
            if (AutoFlush)
                Flush();       
        }
    }
}
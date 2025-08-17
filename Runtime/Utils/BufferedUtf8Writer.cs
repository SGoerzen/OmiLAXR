/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.IO;
using System.Text;

namespace OmiLAXR.Utils
{
    /// <summary>
    /// High-performance UTF-8 file writer with internal buffering for efficient text output.
    /// Provides thread-safe writing operations with automatic UTF-8 encoding.
    /// Optimized for scenarios with frequent small writes like logging and analytics data.
    /// </summary>
    public class BufferedUtf8Writer : IDisposable
    {
        /// <summary>
        /// Buffered stream wrapper providing write buffering for improved performance.
        /// Reduces the number of actual disk I/O operations.
        /// </summary>
        private readonly BufferedStream _bufferedStream;
        
        /// <summary>
        /// Underlying file stream for direct file system access.
        /// Managed by the BufferedStream for actual I/O operations.
        /// </summary>
        private readonly FileStream _fileStream;
        
        /// <summary>
        /// Reusable byte buffer for UTF-8 encoding operations.
        /// Prevents repeated memory allocations during string encoding.
        /// </summary>
        private readonly byte[] _writeBuffer;
        
        /// <summary>
        /// File path this writer is associated with.
        /// Stored for reference and debugging purposes.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Thread synchronization object for ensuring thread-safe operations.
        /// All public methods acquire this lock before performing I/O operations.
        /// </summary>
        private readonly object _lock = new object();
        
        /// <summary>
        /// Initializes a new BufferedUtf8Writer for the specified file path.
        /// Creates or opens the file and sets up buffering for optimal performance.
        /// </summary>
        /// <param name="path">File path to write to</param>
        /// <param name="bufferSize">Size of the internal buffer in bytes (default: 8192)</param>
        public BufferedUtf8Writer(string path, int bufferSize = 8192)
        {
            // Open file with optimized settings for sequential writing
            _fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 8192);
            _bufferedStream = new BufferedStream(_fileStream, bufferSize);
            _writeBuffer = new byte[bufferSize]; // Reusable encoding buffer
            FilePath = path;
        }

        /// <summary>
        /// Writes a single line of text followed by a newline character.
        /// Thread-safe operation with automatic UTF-8 encoding and buffering.
        /// </summary>
        /// <param name="line">Text line to write (newline will be appended automatically)</param>
        public void WriteLine(string line)
        {
            lock (_lock)
            {
                // Append platform-specific newline
                var full = line + Environment.NewLine;
                
                // Encode to UTF-8 using reusable buffer to avoid allocations
                var byteCount = Encoding.UTF8.GetBytes(full, 0, full.Length, _writeBuffer, 0);
                
                // Write encoded bytes to buffered stream
                _bufferedStream.Write(_writeBuffer, 0, byteCount);
            }
        }

        /// <summary>
        /// Writes multiple lines of text efficiently in a single thread-safe operation.
        /// More efficient than multiple WriteLine calls for bulk operations.
        /// </summary>
        /// <param name="lines">Array of text lines to write</param>
        public void WriteLines(string[] lines)
        {
            lock (_lock)
            {
                // Write each line within the same lock for better performance
                foreach (var line in lines)
                    WriteLine(line);
            }
        }

        /// <summary>
        /// Forces all buffered data to be written to the underlying file system.
        /// Thread-safe operation that ensures data persistence.
        /// </summary>
        public void Flush()
        {
            lock (_lock)
            {
                _bufferedStream.Flush();
            }
        }
        
        /// <summary>
        /// Gets the current position in the buffered stream.
        /// Thread-safe access to the stream position for debugging or state tracking.
        /// </summary>
        /// <returns>Current byte position in the stream</returns>
        public int GetPosition()
        {
            lock (_lock)
            {
                return (int)_bufferedStream.Position;
            }
        }

        /// <summary>
        /// Sets the position in the buffered stream for random access scenarios.
        /// Thread-safe positioning operation with bounds checking by the underlying stream.
        /// </summary>
        /// <param name="position">Byte position to seek to</param>
        public void SetPosition(int position)
        {
            lock (_lock)
            {
                _bufferedStream.Position = position;
            }
        }

        /// <summary>
        /// Disposes all resources including streams and buffers.
        /// Ensures final flush before disposal to prevent data loss.
        /// Thread-safe cleanup operation.
        /// </summary>
        public void Dispose()
        {
            lock (_lock)
            {
                // Ensure all data is written before disposing
                _bufferedStream.Flush();
                _bufferedStream.Dispose();
                _fileStream.Dispose();
            }
        }
    }
}
/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Collections.Generic;

namespace OmiLAXR.Utils
{
    /// <summary>
    /// Delegate for providing default values when creating new file buffers.
    /// Used to initialize buffer data when no existing data is available.
    /// </summary>
    /// <returns>Default value object for the new buffer</returns>
    public delegate object DefaultValueHandler();
    
    /// <summary>
    /// Thread-safe manager for multiple file buffers with associated data storage.
    /// Provides centralized management of file writing operations and buffer lifecycle.
    /// Each buffer combines a file writer with arbitrary data storage for complex scenarios.
    /// </summary>
    public class FileBufferManager : IDisposable
    {
        /// <summary>
        /// Dictionary mapping buffer IDs to their corresponding FileBuffer instances.
        /// Thread-safe access is ensured through locking mechanisms.
        /// </summary>
        public readonly Dictionary<int, FileBuffer> FileBuffers = new Dictionary<int, FileBuffer>(10);
        
        /// <summary>
        /// Represents a single file buffer with associated data and writer.
        /// Combines file I/O capabilities with flexible data storage.
        /// </summary>
        public class FileBuffer
        {
            /// <summary>
            /// Arbitrary data object associated with this buffer.
            /// Can store any type of data needed alongside file operations.
            /// </summary>
            public object Data;
            
            /// <summary>
            /// BufferedUtf8Writer instance for efficient file writing operations.
            /// Handles the actual file I/O with buffering for performance.
            /// </summary>
            public BufferedUtf8Writer Writer;
            
            /// <summary>
            /// Gets the stored data as a specific type with type safety.
            /// </summary>
            /// <typeparam name="T">Type to cast the data to</typeparam>
            /// <returns>Data cast to the specified type</returns>
            public T GetValue<T>() => (T)Data;
            
            /// <summary>
            /// Sets the stored data with type safety.
            /// </summary>
            /// <typeparam name="T">Type of the value being stored</typeparam>
            /// <param name="value">Value to store in this buffer</param>
            public void SetValue<T>(T value) => Data = value;
        }

        /// <summary>
        /// Flushes all managed file buffers to ensure data is written to disk.
        /// Thread-safe operation that processes all buffers atomically.
        /// </summary>
        public void FlushAll()
        {
            lock (FileBuffers)
            {
                // Iterate through all buffers and flush their writers
                foreach (var bufferId in FileBuffers.Keys)
                {
                    FileBuffers[bufferId].Writer.Flush();
                }
            }
        }

        /// <summary>
        /// Flushes a specific file buffer identified by its buffer ID.
        /// Ensures the specified buffer's data is written to disk immediately.
        /// </summary>
        /// <param name="bufferId">ID of the buffer to flush</param>
        public void Flush(int bufferId)
        {
            lock (FileBuffers)
            {
                FileBuffers[bufferId].Writer.Flush();
            }
        }
        
        private readonly Dictionary<string, BufferedUtf8Writer> WritersByPath = new Dictionary<string, BufferedUtf8Writer>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Ensures a file buffer exists for the given ID, creating it if necessary.
        /// Thread-safe creation and retrieval of buffers with optional default data initialization.
        /// </summary>
        /// <param name="bufferId">Unique identifier for the buffer</param>
        /// <param name="filePath">File path for the buffer's writer (used only if creating new buffer)</param>
        /// <param name="fallbackValue">Optional delegate to provide default data value for new buffers</param>
        /// <returns>The FileBuffer instance (existing or newly created)</returns>
        public FileBuffer EnsureBuffer(int bufferId, string filePath, DefaultValueHandler fallbackValue = null)
        {
            lock (FileBuffers)
            {
                // Falls bereits ein Buffer mit dieser ID existiert, zurückgeben
                if (FileBuffers.TryGetValue(bufferId, out var existingBuffer))
                    return existingBuffer;

                // Gibt es bereits einen Writer für diesen Pfad?
                if (!WritersByPath.TryGetValue(filePath, out var writer))
                {
                    writer = new BufferedUtf8Writer(filePath);
                    WritersByPath[filePath] = writer;
                }

                var buffer = new FileBuffer
                {
                    Data = fallbackValue?.Invoke(),
                    Writer = writer
                };

                FileBuffers.Add(bufferId, buffer);
                return buffer;
            }
        }

        public void DisposeAll()
        {
            lock (FileBuffers)
            {
                foreach (var writer in WritersByPath.Values)
                {
                    writer?.Dispose();
                }

                WritersByPath.Clear();
                FileBuffers.Clear();
            }
        }

        /// <summary>
        /// Disposes all managed resources including file writers and buffers.
        /// Ensures all data is flushed before disposing writers to prevent data loss.
        /// Thread-safe cleanup operation.
        /// </summary>
        public void Dispose()
        {
            DisposeAll();
            lock (FileBuffers)
            {
                // Clean up each buffer individually
                foreach (var buffer in FileBuffers.Values)
                {
                    // Skip if writer is already disposed
                    if (buffer.Writer == null)
                        continue;
                    
                    // Flush remaining data and dispose writer
                    buffer.Writer.Flush();
                    buffer.Writer.Dispose();
                    buffer.Writer = null;
                }
                
                // Clear the dictionary after disposing all buffers
                FileBuffers.Clear();
            }
        }
    }
}
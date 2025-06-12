using System;
using System.Collections.Generic;

namespace OmiLAXR.Utils
{
    public delegate object DefaultValueHandler();
    public class FileBufferManager : IDisposable
    {
        public readonly Dictionary<int, FileBuffer> FileBuffers = new Dictionary<int, FileBuffer>(10);
        
        public class FileBuffer
        {
            public object Data;
            public BufferedUtf8Writer Writer;
            
            public T GetValue<T>() => (T)Data;
            public void SetValue<T>(T value) => Data = value;
        }

        public void FlushAll()
        {
            lock (FileBuffers)
            {
                foreach (var bufferId in FileBuffers.Keys)
                {
                    FileBuffers[bufferId].Writer.Flush();
                }
            }
        }

        public void Flush(int bufferId)
        {
            lock (FileBuffers)
            {
                FileBuffers[bufferId].Writer.Flush();
            }
        }
        
        public FileBuffer EnsureBuffer(int bufferId, string filePath, DefaultValueHandler fallbackValue = null)
        {
            lock (FileBuffers)
            {
                // return existing buffer if exists
                if (FileBuffers.TryGetValue(bufferId, out var buffer))
                {
                    return buffer;
                }
            
                // create new buffer and open file if not exists
                var stream = new BufferedUtf8Writer(filePath);
                buffer = new FileBuffer() { Data = fallbackValue?.Invoke(), Writer = stream };
                // add to dict
                FileBuffers.Add(bufferId, buffer);
                return buffer;
            }
        }

        public void Dispose()
        {
            lock (FileBuffers)
            {
                foreach (var buffer in FileBuffers.Values)
                {
                    if (buffer.Writer == null)
                        continue;
                    
                    buffer.Writer.Flush();
                    buffer.Writer.Dispose();
                    buffer.Writer = null;
                }
                FileBuffers.Clear();
            }
        }
    }
}
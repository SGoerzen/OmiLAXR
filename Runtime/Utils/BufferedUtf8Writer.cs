using System;
using System.IO;
using System.Text;

namespace OmiLAXR.Utils
{
    public class BufferedUtf8Writer : IDisposable
    {
        private readonly BufferedStream _bufferedStream;
        private readonly FileStream _fileStream;
        private readonly byte[] _writeBuffer;
        public string FilePath { get; private set; }

        private readonly object _lock = new object();
        
        public BufferedUtf8Writer(string path, int bufferSize = 8192)
        {
            _fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 8192);
            _bufferedStream = new BufferedStream(_fileStream, bufferSize);
            _writeBuffer = new byte[bufferSize];
            FilePath = path;
        }

        public void WriteLine(string line)
        {
            lock (_lock)
            {
                var full = line + Environment.NewLine;
                var byteCount = Encoding.UTF8.GetBytes(full, 0, full.Length, _writeBuffer, 0);
                _bufferedStream.Write(_writeBuffer, 0, byteCount);
            }
        }

        public void WriteLines(string[] lines)
        {
            lock (_lock)
            {
                foreach (var line in lines)
                    WriteLine(line);
            }
        }

        public void Flush()
        {
            lock (_lock)
            {
                _bufferedStream.Flush();
            }
        }
        
        public int GetPosition()
        {
            lock (_lock)
            {
                return (int)_bufferedStream.Position;
            }
        }

        public void SetPosition(int position)
        {
            lock (_lock)
            {
                _bufferedStream.Position = position;
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _bufferedStream.Flush();
                _bufferedStream.Dispose();
                _fileStream.Dispose();
            }
        }
    }
}
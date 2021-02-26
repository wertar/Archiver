using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    class FileReader : IFileReader
    {
        private readonly FileStream _file;
        private bool _disposed = false;

        public FileReader(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception($"File [{filePath} was not found.]");
            _file = File.OpenRead(filePath);
        }

        public void Dispose()
        {
            if (_disposed)
                return;            
            _file.Close();
            _disposed = true;
        }

        public int ReadBuffer(byte[] buffer)
        {
            return _file.Read(buffer, 0, buffer.Length);
        }

        public int ReadBuffer(byte[] buffer, int count)
        {
            return _file.Read(buffer, 0, count);
        }
    }
}

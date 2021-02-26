using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    public class FileWriter : IFileWriter
    {
        private readonly FileStream _file;
        private bool _disposed = false;

        public FileWriter(string filePath)
        {
            _file = File.Create(filePath);
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _file.Close();
            _disposed = true;
        }

        public void WriteBuffer(byte[] buffer)
        {
            _file.Write(buffer, 0, buffer.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    public class ArchiveFileWriter : IFileWriter
    {
        private readonly IFileWriter _file;
        private bool _disposed = false;

        public ArchiveFileWriter(IFileWriter filewriter)
        {
            _file = filewriter;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _file.Dispose();
            _disposed = true;
        }

        public void WriteBuffer(byte[] buffer)
        {
            var header = BitConverter.GetBytes(buffer.Length);
            _file.WriteBuffer(header);
            _file.WriteBuffer(buffer);
        }
    }
}

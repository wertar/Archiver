using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    class ArchiveFileReader : IFileReader
    {
        private readonly IFileReader fileReader;
        private byte[] blockLength = new byte[4];
        private bool _disposed = false;

        public ArchiveFileReader(IFileReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            fileReader.Dispose();
            _disposed = true;
        }

        public int ReadBuffer(byte[] buffer)
        {
            var result = fileReader.ReadBuffer(blockLength);
            if (result <= 0) return 0;
            var lengthOfBlock = BitConverter.ToInt32(blockLength, 0);
            return fileReader.ReadBuffer(buffer, lengthOfBlock);
        }

        public int ReadBuffer(byte[] buffer, int count)
        {
            throw new NotImplementedException();
        }
    }
}

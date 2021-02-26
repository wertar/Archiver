using System;

namespace archiver
{
    public interface IFileWriter : IDisposable
    {        
        void WriteBuffer(byte[] buffer);
    }
}
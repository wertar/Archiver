using System;

namespace archiver
{
    interface IFileReader: IDisposable
    {
        int ReadBuffer(byte[] buffer);
        int ReadBuffer(byte[] buffer, int count);
    }
}
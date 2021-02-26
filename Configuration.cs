using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    public static class Configuration
    {
        public static int BlockSize { get; set; } = 8192;
        public static int QueueBufferLength { get; set; } = 10000;
        public static int CountOfCompressors { get; set; } = Environment.ProcessorCount - 2;
        public static int CountOfDecompressors { get; set; } = Environment.ProcessorCount - 2;
    }
}

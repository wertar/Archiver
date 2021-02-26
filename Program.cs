using archiver;
using archiver.Providers;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 3 || false)
            {                  
                "Please specify all input arguments.".PrintOnConsole();                
                return 1;
            }
           // args = new[] { "compress", "G:\\Win10.vhd", "G:\\ArchivedVHD.arh" };
           // args = new[] { "decompress", "G:\\ArchivedVHD.arh", "K:\\DecompressedVHD.vhd" };


            // args = new[] { "compress", "G:\\Input.txt", "G:\\InputArc.txt" };
            //args = new[] { "decompress", "G:\\InputArc.txt", "G:\\DecompressedInput.txt" };
            switch (args[0])
            {
                case "compress":
                    {
                        var fileToArchive = args[1];
                        var archiveName = args[2];

                        var fileReader = new FileReader(fileToArchive);

                        var inpuQueues = QueueProvider.GetQueues(Configuration.CountOfCompressors, Configuration.QueueBufferLength);

                        var reader = FileReaderProvider.StartReader(fileReader, inpuQueues, Configuration.BlockSize);

                        var outputQueues = QueueProvider.GetQueues(Configuration.CountOfCompressors, Configuration.QueueBufferLength);

                        var compressors = Enumerable.Zip(inpuQueues, outputQueues, (a, b) => (inQueue: a, outQueue: b))
                            .Select(i => CompressorProvider.StartCompressor(i.inQueue, i.outQueue, CompressionLevel.Optimal))
                            .ToArray();

                        var fileWriter = new ArchiveFileWriter(new FileWriter(archiveName));
                        var writer = FileWriterProvider.StartWriting(fileWriter, outputQueues);

                        var monitor = MonitorProvider.StartMonitor(inpuQueues, outputQueues);

                        writer.Join();

                        break;
                    }
                case "decompress":
                    {
                        var archiveName = args[1];
                        var outputFileName = args[2];

                        var inpuCompressedQueues = QueueProvider.GetQueues(Configuration.CountOfDecompressors, Configuration.QueueBufferLength);
                        var archiveReader = FileReaderProvider.StartReader(new ArchiveFileReader(new FileReader(archiveName)), inpuCompressedQueues, Configuration.BlockSize + 100);
                        var outputDecompressedQueues = QueueProvider.GetQueues(Configuration.CountOfDecompressors, Configuration.QueueBufferLength);


                        var decompressors = Enumerable.Zip(inpuCompressedQueues, outputDecompressedQueues, (a, b) => (inQueue: a, outQueue: b))
                            .Select(i => DecompressorProvider.StartDecompressor(i.inQueue, i.outQueue))
                            .ToArray();

                        var decompressedWriter = FileWriterProvider.StartWriting(new FileWriter(outputFileName), outputDecompressedQueues);

                        var monitor = MonitorProvider.StartMonitor(inpuCompressedQueues, outputDecompressedQueues);

                        decompressedWriter.Join();
                        break;
                    }

                default:
                    break;
            }

            return 0;
        }
    }
}

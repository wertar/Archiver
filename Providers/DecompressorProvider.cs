using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace archiver
{
    public class DecompressorProvider
    {
        public static Thread StartDecompressor(IQueue<ProcessingBlock> inQueue, IQueue<ProcessingBlock> outQueue)
        {
            var thread = new Thread(() =>
            {
                try
                {

                
                while (true)
                {
                    if (!inQueue.HasElements)
                    {
                        if (inQueue.WriterFinished) break;
                        continue;
                    }

                    while (inQueue.AwaitableTryDequeue(out var processingBlock))
                    {
                        var stream = new MemoryStream(processingBlock.Value);
                        using (var zip = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            var buffer = new byte[processingBlock.Value.Length * 2];
                            var readCount = zip.Read(buffer, 0, buffer.Length);
                            outQueue.EnqueWhenNoOverflow(new ProcessingBlock() { BlockId = processingBlock.BlockId, Value = buffer.GetSubArray(readCount) });
                        }
                    }
                }
                }
                catch (Exception e)
                {
                    e.Message.PrintOnConsole();
                    Environment.Exit(1);
                }

                outQueue.WriterFinished = true;
            });
            thread.Start();
            return thread;
        }
    }
}


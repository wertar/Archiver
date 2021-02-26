using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace archiver
{
    public class CompressorProvider
    {
        public static Thread StartCompressor(IQueue<ProcessingBlock> inQueue, IQueue<ProcessingBlock> outQueue, CompressionLevel compressionLevel)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    using (var str = new StreamedBuffer(outQueue))
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
                                using (var zip = new GZipStream(str, compressionLevel, true))
                                {
                                    str.BlockId = processingBlock.BlockId;
                                    zip.Write(processingBlock.Value, 0, processingBlock.Value.Length);
                                }
                            }
                        }
                    }
                    outQueue.WriterFinished = true;
                }
                catch (Exception e)
                {
                    e.Message.PrintOnConsole();
                    Environment.Exit(1);                   
                }
            });
            thread.Start();
            return thread;
        }
    }
}

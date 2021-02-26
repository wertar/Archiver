using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace archiver
{
    public class FileWriterProvider
    {
        public static Thread StartWriting(IFileWriter fileWriter, IEnumerable<IQueue<ProcessingBlock>> queues)
        {            
            var thread = new Thread(() =>
            {
                try
                {
                    using (fileWriter)
                    {
                        var id = 0;
                        while (true)
                        {                            
                            if (queues.All(q => !q.HasElements))
                            {
                                if (queues.All(q => q.WriterFinished)) break;
                                continue;
                            }

                            foreach (var queue in queues)
                            {
                                if (!queue.HasElements) continue;
                                if (!queue.TryPeek(out var processingBlock)) continue;

                                if (processingBlock.BlockId == id)
                                {
                                    queue.AwaitableTryDequeue(out _);
                                    fileWriter.WriteBuffer(processingBlock.Value);
                                    id++;                                    
                                }
                            }
                            
                        }
                        $"Wrote {id} blocks.".PrintOnConsole();
                    }
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

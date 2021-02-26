using System;
using System.Collections.Generic;
using System.Threading;

namespace archiver
{
    class FileReaderProvider
    {
        public static Thread StartReader(IFileReader fileReader, IEnumerable<IQueue<ProcessingBlock>> outputQueues, int sizeOfBlock)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    var id = 0;
                    var queues = outputQueues.GetEnumerator();
                    using (fileReader)
                    {
                        while (true)
                        {
                            var buffer = new byte[sizeOfBlock];
                            var readBytes = fileReader.ReadBuffer(buffer);
                            if (readBytes == 0) break;

                            if (!queues.MoveNext())
                            {
                                queues.Reset();
                                queues.MoveNext();
                            }

                            queues.Current.EnqueWhenNoOverflow(new ProcessingBlock() { BlockId = id, Value = (readBytes != buffer.Length) ? buffer.GetSubArray(readBytes) : buffer });
                            id++;
                        }
                    }

                    foreach (var item in outputQueues)
                    {
                        item.WriterFinished = true;
                    }
                }
                catch (System.Exception e)
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

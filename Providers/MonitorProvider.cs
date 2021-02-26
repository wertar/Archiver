using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace archiver.Providers
{
    public static class MonitorProvider
    {
        public static Thread StartMonitor(IEnumerable<IQueue<ProcessingBlock>> inQueues, IEnumerable<IQueue<ProcessingBlock>> outQueues)
        {
            var thread = new Thread(() =>
            {
                var pairs = Enumerable.Zip(inQueues, outQueues, (a, b) => (inQueue: a, outQueue: b)).ToArray();
                while (true)
                {
                    foreach (var item in pairs)
                    {
                        $"In Queue:{item.inQueue.Counter}   ".PrintInLine();
                        $"Out Queue:{item.outQueue.Counter}  at :".PrintInLine();
                        DateTime.Now.ToString().PrintOnConsole();                        
                    }
                    Thread.Sleep(30000);
                }
            });
            thread.IsBackground = true;
            thread.Start();
            return thread;
        }
    }
}

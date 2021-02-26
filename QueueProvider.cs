using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    public static class QueueProvider
    {
        public static IEnumerable<SingnallingQueue<ProcessingBlock>> GetQueues(int numberOfQueues, int lengthOfQueueBuffer)
        {
            var queues = Enumerable.Range(1, numberOfQueues)
                .Select(i => new SingnallingQueue<ProcessingBlock>(lengthOfQueueBuffer))
                .ToArray();

            return queues;
        }
    }
}

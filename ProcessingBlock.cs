using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    public struct ProcessingBlock
    {
        public int BlockId { get; set; }
        public byte[] Value { get; set; }
    }
}

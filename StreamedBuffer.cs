using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    class StreamedBuffer : Stream
    {
        private byte[] _internalBuffer;
        private readonly IQueue<ProcessingBlock> queue;

        public StreamedBuffer(IQueue<ProcessingBlock> queue)
        {
            this.queue = queue;
        }
        public int BlockId { get; set; }

        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => true;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer.Length == 10)
            {
                _internalBuffer = buffer;
                return;
            }

            var tmp = new byte[_internalBuffer.Length + count];
            _internalBuffer.CopyTo(tmp, 0);
            Array.Copy(buffer,0,tmp,_internalBuffer.Length,count);
            _internalBuffer = tmp;

            if (buffer.Length == 8)
            {
                queue.EnqueWhenNoOverflow(new ProcessingBlock() { BlockId = BlockId, Value = _internalBuffer });
                _internalBuffer = null;
            }
        }
    }
}

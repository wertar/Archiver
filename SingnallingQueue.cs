using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace archiver
{
    public class SingnallingQueue<T> : IQueue<T> where T : struct
    {
        private long _counter = 0;
        private LinkedListNode<T> _firstNode;
        private LinkedListNode<T> _lastNode;
        private readonly ManualResetEvent _waier = new ManualResetEvent(false);
        private readonly ManualResetEvent _stopIfOverflow = new ManualResetEvent(true);
        private readonly int _bufferLength;
        private readonly int _maxQueueSize;
        private bool _fnished;

        public bool WriterFinished { get => _fnished; set { _fnished = value; _waier.Set(); } }

        public SingnallingQueue(int bufferLength)
        {
            _bufferLength = bufferLength;
            _maxQueueSize = _bufferLength * 3;
        }

        public void Enqueue(T element)
        {
            var newNode = new LinkedListNode<T> { Value = element };

            if (_lastNode != null)
                _lastNode.Previous = newNode;

            if (_firstNode == null)
                _firstNode = newNode;

            _lastNode = newNode;
            lock (this)
            {
                _counter++;
                if (_counter > _bufferLength) _waier.Set();
                if (_counter > _maxQueueSize) _stopIfOverflow.Reset();
            }
        }

        public void EnqueWhenNoOverflow(T element)
        {
            _stopIfOverflow.WaitOne();
            Enqueue(element);
        }

        public bool AwaitableTryDequeue(out T element)
        {
            _waier.WaitOne();
            if (_firstNode == null)
            {
                element = default;
                return false;
            }
                        
            var nodeToReturn = _firstNode;
            _firstNode = nodeToReturn.Previous;

            element = nodeToReturn.Value;

            lock (this)
            {
                _counter--;
                if (_counter < 3 && !WriterFinished)
                {
                    _waier.Reset();                    
                }

                if (_counter < _bufferLength) _stopIfOverflow.Set();
            }

            return true;
        }

        public bool TryPeek(out T element)
        {
            if (_firstNode == null)
            {
                element = default;
                return false;
            }

            element = _firstNode.Value;
            return true;
        }
        public bool HasElements => _firstNode != null;

        public long Counter => _counter;
    }
    class LinkedListNode<T>
    {
        public LinkedListNode<T> Previous { get; set; }
        public T Value { get; set; }
    }
}

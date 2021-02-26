namespace archiver
{
    public interface IQueue<T>
    {
        bool HasElements { get; }
        bool WriterFinished { get; set; }
        long Counter { get; } 
        void Enqueue(T element);
        void EnqueWhenNoOverflow(T element);
        bool AwaitableTryDequeue(out T element);
        bool TryPeek(out T element);
    }
}
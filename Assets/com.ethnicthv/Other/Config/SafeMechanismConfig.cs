namespace com.ethnicthv.Other.Config
{
    public struct SafeMechanismConfig
    {
        
        public int QueueSize { get; set; }
        public int EnqueueTimeout { get; set; }
        public int DequeueTimeout { get; set; }
        
        public uint PoolSize { get; set; }
    }
}
namespace com.ethnicthv.Other.Config
{
    public class SafeMechanismConfig
    {
        
        public int QueueSize { get; set; } = 10;
        public int EnqueueTimeout { get; set; } = 10;
        public int DequeueTimeout { get; set; } = 0;
        
        public uint PoolSize { get; set; } = 0;
        
        public SafeMechanismConfig() { }
    }
}
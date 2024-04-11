using System.Collections.Generic;

namespace com.ethnicthv.Util.Config
{
    public class SafeMechanismConfig
    {
        
        public int QueueSize { get; set; } = 10;
        public int EnqueueTimeout { get; set; } = 10;
        public int DequeueTimeout { get; set; } = 0;
        
        public SafeMechanismConfig() { }
    }
}
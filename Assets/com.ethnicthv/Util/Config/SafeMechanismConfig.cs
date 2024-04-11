using System.Collections.Generic;

namespace com.ethnicthv.Util.Config
{
    public class SafeMechanismConfig
    {
        //private static SafeMechanismConfig _config;

        public static bool GetConfig(out SafeMechanismConfig config)
        {
            // if (_config != null)
            // {
            //     config = _config;
            //     return true;
            // }
            // config = null;
            config = null;
            return false;
        }
        
        public readonly int QueueSize = 10;
        public readonly int EnqueueTimeout = 10;
        public readonly int DequeueTimeout;
        
        public SafeMechanismConfig(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("queueSize", out var queueSize))
            {
                QueueSize = (int) queueSize;
            }
            if (dict.TryGetValue("enqueueTimeout", out var enqueueTimeout))
            {
                EnqueueTimeout = (int) enqueueTimeout;
            }
            if (dict.TryGetValue("dequeueTimeout", out var dequeueTimeout))
            {
                DequeueTimeout = (int) dequeueTimeout;
            }
        }
    }
}
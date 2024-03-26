namespace com.ethnicthv.Util
{
    public static class Debug
    {
        public static void Log(object message)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.Log(message);
            }
        }
        
        public static void Log(string message)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.Log(message);
            }
        }
        
        public static void LogError(string message)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.LogError(message);
            }
        }
        
        public static void LogWarning(string message)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.LogWarning(message);
            }
        }
        
        public static void LogException(System.Exception exception)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        public static void Assert(bool condition)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.Assert(condition);
            }
        }
        
        public static void Assert(bool condition, string message)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.Assert(condition, message);
            }
        }
        
        public static void Assert(bool condition, object message)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.Assert(condition, message);
            }
        }
        
        public static void Assert(bool condition, object message, UnityEngine.Object context)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.Assert(condition, message, context);
            }
        }
        
        public static void Assert(bool condition, string message, UnityEngine.Object context)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.Assert(condition, message, context);
            }
        }
        
        public static void Break()
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.Break();
            }
        }
        
        public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end)
        {
            if (GameManager.IsDebug)
            {
                UnityEngine.Debug.DrawLine(start, end);
            }
        }
    }
}
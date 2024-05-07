namespace com.ethnicthv.Other.Ev.Exception
{
    public class HandlerException : System.Exception
    {
        public HandlerException(string message) : base(message)
        {
        }
        
        public HandlerException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
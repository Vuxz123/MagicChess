namespace com.ethnicthv.Inner.Event.Exception
{
    public class NotMainThreadException: System.Exception
    {
        public NotMainThreadException(string message) : base(message)
        {
        }
    }
}
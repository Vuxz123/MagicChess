using com.ethnicthv.Other.Ev;

namespace Demo
{
    public class DemoEvent: Event
    {
        public string Message { get; set; }
        
        public DemoEvent(string message)
        {
            Message = message;
        }
    }
}
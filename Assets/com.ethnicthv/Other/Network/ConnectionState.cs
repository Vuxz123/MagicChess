using System.Net.Sockets;
using System.Threading;

namespace com.ethnicthv.Other.Network
{
    public class ConnectionState
    {
        public TcpClient client;

        // thread safe pipe to send messages from main thread to send thread
        public readonly SendStack sendPipe;

        // ManualResetEvent to wake up the send thread. better than Thread.Sleep
        // -> call Set() if everything was sent
        // -> call Reset() if there is something to send again
        // -> call WaitOne() to block until Reset was called
        public ManualResetEvent sendPending = new ManualResetEvent(false);

        public ConnectionState(TcpClient client, int MaxMessageSize)
        {
            this.client = client;

            // create send pipe with max message size for pooling
            sendPipe = new SendStack(MaxMessageSize);
        }
    }
}
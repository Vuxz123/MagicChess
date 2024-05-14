using System.Net.Sockets;
using System.Threading;

namespace com.ethnicthv.Other.Networking
{
    public class ConnectionState
    {
        public Thread receiveThread;
        public TcpClient client;
        
        public ManualResetEvent sendPending = new ManualResetEvent(false);
        public SendStack sendPipe;

        public bool Connected => client is { Client: { Connected: true } };
        
        public bool Connecting { get; set; }
        
        public int maxSendBufferSize { get; }
        public int maxReceiveBufferSize { get; }

        public ConnectionState(int maxSendBufferSize, int maxReceiveBufferSize)
        {
            client = new TcpClient();
            this.maxSendBufferSize = maxSendBufferSize;
            this.maxReceiveBufferSize = maxReceiveBufferSize;
        }

        public void Dispose()
        {
            client.Close();
            
            receiveThread?.Interrupt(); 
            
            Connecting = false;
            
            client = null;
        }
    }
}
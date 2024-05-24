using System.Net.Sockets;
using System.Threading;

namespace com.ethnicthv.Other.Network.Client
{
    public class ClientConnectionState : ConnectionState
    {
        public Thread receiveThread;
        
        public readonly ReceiveStack receivePipe;

        public bool Connected => client is { Client: { Connected: true } };
        
        public bool Connecting { get; set; }

        public ClientConnectionState(int maxMessageSize) : base(new TcpClient(), maxMessageSize)
        {
            receivePipe = new ReceiveStack(maxMessageSize);
        }

        public void Dispose()
        {
            client.Close();
            
            receiveThread?.Interrupt(); 
            
            Connecting = false;
            
            sendPipe.Clear();
            
            client = null;
        }
    }
}
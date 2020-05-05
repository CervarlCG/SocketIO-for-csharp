using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace SocketIO
{
    class SocketIO
    {
        private IPHostEntry hostEntry;
        private IPAddress ipAddr;
        IPEndPoint endPoint;
        Socket listener;

        public delegate void eventDelegate(object o);
        public eventDelegate m_onConnection;
        public eventDelegate m_onDisconnect;

        public SocketIO(string host, int port)
        {
            //Starting the server socket
            hostEntry = Dns.GetHostEntry(host);
            ipAddr = hostEntry.AddressList[0];
            endPoint = new IPEndPoint(ipAddr, port);

            //Staring the listener
            listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endPoint);
            listener.Listen(99);
        }

        public void Start()
        {
            Thread t = new Thread(Listen);
            t.Start();
        }

        public void Listen()
        {
            Socket client;
            SocketConnection clientManager;
            while (true)
            {
                client = listener.Accept();
                clientManager = new SocketConnection(client);
                clientManager.Start();
                m_onConnection(clientManager);
                
            }
        }

        public void onConnection(eventDelegate e)
        {
            this.m_onConnection = e;
        }

        public void onDisconnect(eventDelegate e)
        {
            this.m_onDisconnect = e;
        }


    }
}

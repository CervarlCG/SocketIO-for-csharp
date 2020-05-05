using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketIO_Client
{
    class SocketIO_Client
    {
        public static SocketConnection createConnection(string host, int port)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(host);
                IPAddress ipAddr = hostEntry.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(ipAddr, port);
                Socket client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(endPoint);
                SocketConnection socket = new SocketConnection(client);
                return socket;
            }catch(SocketException)
            {
                return null;
            }
        }

       
    }
}

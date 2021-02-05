using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Listener_Clone
    {
        Socket listenSocket;

        public void Init(IPEndPoint endPoint)
        {
            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(endPoint);
            listenSocket.Listen(10);//최대 몇명과 통신할것인가
        }

        public Socket Accept()
        {
            return listenSocket.Accept();
        }
    }
}

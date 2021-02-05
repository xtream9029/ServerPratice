using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DummyClient
{
    class Program
    {
        public static void Main()
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];//하나의 Domain에 여러개의 ip주소를 가지고 있을수 있기 때문에
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);//포트넘버 7777을 사용하고 있음

            while (true)
            {
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(endPoint);//서버와 통신하기 위해 연결
                    Console.WriteLine($"Connected To{ socket.RemoteEndPoint.ToString()}");

                    //서버에 데이터를 보내는 부분
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Hi");
                    int sendBytes = socket.Send(sendBuff);

                    //서버로부터 데이터를 받는 부분
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = socket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine($"[From Server]{recvData}");

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(100);
            }

          
        }
    }
}

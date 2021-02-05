using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program 
    {
        static Listener _listener = new Listener();
        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                //클라이언트로부터 데이터를 받음
                byte[] recvBuff = new byte[1024];
                int recvBytes = clientSocket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);//문자열을 받는다고 가정
                Console.WriteLine($"[From Client] {recvData}");

                //서버에서 클라이언트로 데이터를 보내는 부분
                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to Server!");
                clientSocket.Send(sendBuff);

                //클라이언트와 연결종료
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }//OnAcceptHandler

        public static void Main()
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr=ipHost.AddressList[0];//하나의 Domain에 여러개의 ip주소를 가지고 있을수 있기 때문에
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);//포트넘버 7777을 사용하고 있음

            _listener.Init(endPoint,OnAcceptHandler);
            Console.WriteLine("Listening");

            while (true)
            {
            }
        }//Main
    }//Class Program
}

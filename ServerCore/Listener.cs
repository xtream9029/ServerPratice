using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    //서버에서 가지고 있어야할 클래스지 클라이언트가 가지고 있어야할 클래스가 아님
    class Listener
    {
        Socket _listenSocket;
        Action<Socket> _onAcceptHandler;

        public void Init(IPEndPoint endPoint,Action<Socket> onAcceptHandler)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;

            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(10);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);//callback 방식
            RegisterAccept(args);
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            //이전 소켓은 깔끔하게 null로 밀어버림
            args.AcceptSocket = null;

            //비동기 방식으로 accept를 함 --non blocking
            bool pending = _listenSocket.AcceptAsync(args);//예약
            //클라이언트가 바로 온상황
            if (!pending)
                OnAcceptCompleted(null,args);
        }

        void OnAcceptCompleted(object sender,SocketAsyncEventArgs args)
        {
            //소켓 에러가 없다
            if (args.SocketError == SocketError.Success)
            {
                _onAcceptHandler.Invoke(args.AcceptSocket);//클라이언트 Accept 완료
            }
            //소켓 에러가 있는 경우
            else Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args);//다음 클라이언트를 위해 새롭게 등록
        }
    }
}

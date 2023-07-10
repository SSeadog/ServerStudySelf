using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Listener
    {
        Socket _listenSocket;
        Action<Socket> _onAccecptAction;

        public Listener(IPEndPoint endPoint, Action<Socket> onAccecptAction)
        {
            // 문지기
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // 문지기 교육
            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(10);

            _onAccecptAction = onAccecptAction;
        }

        public void Start()
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.Completed += new EventHandler<SocketAsyncEventArgs>(OnAccept);
            RegistAccept(_listenSocket, e);
        }

        void RegistAccept(Socket listenSocket, SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = listenSocket.AcceptAsync(args);
            if (pending == false)
            {
                OnAccept(null, args);
            }
        }

        void OnAccept(object? sender, SocketAsyncEventArgs args)
        {
            if (args.AcceptSocket != null)
            {
                Socket clientSocket = args.AcceptSocket;
                Console.WriteLine("클라 접속!");
                _onAccecptAction.Invoke(clientSocket);
            }

            RegistAccept(_listenSocket, args);
        }
    }
}


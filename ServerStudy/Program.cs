using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static Socket socket;

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);


            Listener listener = new Listener(endPoint, OnAccepted);
            listener.Start();

            while (true)
            {
                Console.WriteLine("서버 메인 쓰레드!");
                Thread.Sleep(250);
            }
        }

        public static void OnAccepted(Socket socket)
        {
            Program.socket = socket;

            // 듣기
            SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
            recvArgs.SetBuffer(new byte[1024]);
            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecv);
            RegistRecv(recvArgs);
            //byte[] recvBuff = new byte[1024];
            //socket.Receive(recvBuff);
            //Console.WriteLine(Encoding.UTF8.GetString(recvBuff));

            // 전송
            socket.Send(Encoding.UTF8.GetBytes("Welcome!"));

            // 내쫓기
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Close();
        }

        public static void RegistRecv(SocketAsyncEventArgs args)
        {
            bool pending = socket.ReceiveAsync(args);
            if (pending == false)
            {
                OnRecv(null, args);
            }
        }

        public static void OnRecv(object? sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0)
            {
                if (args.Buffer != null)
                    Console.WriteLine(Encoding.UTF8.GetString(args.Buffer));
                args.SetBuffer(new byte[1024]);
            }

            RegistRecv(args);
        }
    }
}
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 접속
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            for (int i = 0; i < 5; i++)
            {
                // 전송
                socket.Send(Encoding.UTF8.GetBytes("Hello!!" + i));
            }

            // 받기
            byte[] buffer = new byte[1024];
            socket.Receive(buffer);
            Console.WriteLine(Encoding.UTF8.GetString(buffer));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DemoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 55555);
            Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectionSocket.Bind(ipEndPoint);
            connectionSocket.Listen(10);

            try
            {
                Socket messageSocket = connectionSocket.Accept();
                string clientAddress = messageSocket.RemoteEndPoint.ToString();

                int numberOfBytesReceived;
                do
                {
                    byte[] buffer = new byte[1000];
                    numberOfBytesReceived = messageSocket.Receive(buffer);

                    string message = Encoding.UTF8.GetString(buffer, 0, numberOfBytesReceived);
                    Console.WriteLine(clientAddress + " said: " + message);
                } while (numberOfBytesReceived > 0);

                string response = "Congratulations! You have mastered socket programming.";
                messageSocket.Send(Encoding.UTF8.GetBytes(response));

                messageSocket.Shutdown(SocketShutdown.Both);
                messageSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.Read();
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

            Console.WriteLine(ipEndPoint.ToString() + ": Waiting for clients...");

            try
            {
                // The Socket.Accept() method will wait until a client initiates a connection to the socket.
                // The method will return a new socket instance
                Socket messageSocket = connectionSocket.Accept();
                string clientAddress = messageSocket.RemoteEndPoint.ToString();

                // Here we do a loop where we receive data from the socket until there are no available data left to be received
                do
                {
                    // These lines will receive the data from the socket and store them in the "buffer" variable
                    byte[] buffer = new byte[1000];
                    int numberOfBytesReceived = messageSocket.Receive(buffer);

                    // We then convert the raw bytes in the buffer into a string
                    string message = Encoding.UTF8.GetString(buffer, 0, numberOfBytesReceived);

                    Console.WriteLine(clientAddress + " said: " + message);
                } while (messageSocket.Available > 0);

                // After receiving the message, we send a response back to the client.
                string response = "Congratulations! You have mastered socket programming.";
                messageSocket.Send(Encoding.UTF8.GetBytes(response));

                // Sockets should be shutdown and closed after using so that they can be reused by other applications
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

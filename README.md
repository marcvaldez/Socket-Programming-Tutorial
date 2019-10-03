First draft
# Socket-Programming-Tutorial
This is a project for my Introduction to Networks class in NBCC.

What is a Socket?

A socket is one endpoint of a two-way communication link between two programs running on the network.

Types of Sockets

Stream Socket
- Also called connection-oriented sockets
- Commonly used for TCP

Datagram Socket
- Also called connectionless sockets
- Commonly used for UDP

Raw Socket
- Allows direct sending and receiving of IP packets without any protocol-specific transport layer formatting
- Can be used if you want to implement your own transport protocol
- Also used by network utility programs such as nmap and ping

Socket API
- A collection of functions/methods/commands usually provided by the operating system to enable programs to use sockets and communicate with other programs.

Where do sockets fit in the OSI or TCP/IP model?
(show diagram)

Berkeley Sockets
- Originally used as an application programming interface (API) for sockets in the BSD operating system
- Also known as BSD Socket API
- Created by a group of researchers in University of California, Berkeley in the 1980s
- Became the de facto standard for socket APIs and also the basis for most modern socket API implementations such as Linux sockets, POSIX sockets, and Windows sockets (Winsock)

List of Common Socket API Functions
socket( )
bind( )
connect( )
listen( ) and accept( )
read( ), recv( ), recvfrom( ), or recvmsg( )
write( ), send( ), sendto( ), or sendmsg( )
close( )
gethostbyname( ) and gethostbyaddr( )
select( )
poll( )

Socket Programming Tutorial using C# and the .Net Framework

In this tutorial, we will create a simple server and a simple client that will communicate with each other using TCP.

(Show TCP Socket Flow diagram)

Part 1: Creating the server program

1. Create a new Console application project in Visual Studio.
2. Open the Program.cs file and add the following lines near the top: 

using System.Net;
using System.Net.Sockets;

3. Add the following lines of code inside the Main() method

Part 2: Creating the client application

Part 3: Putting It All Together

1. Run both the server app and the client app at the same time.
2. Click on the connect button on the client app.
3. If everything goes well, you should be able to see the client's message on the server app and the server's response in the output textbox on the client app. 


Sources:
https://en.wikipedia.org/wiki/Network_socket
https://docs.oracle.com/javase/tutorial/networking/sockets/definition.html
http://homepage.smc.edu/morgan_david/cs70/sockets.htm
http://ijcsit.com/docs/Volume%205/vol5issue03/ijcsit20140503462.pdf
https://docs.microsoft.com/en-us/dotnet/framework/network-programming/

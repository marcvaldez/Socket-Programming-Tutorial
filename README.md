![License](https://img.shields.io/github/license/marcvaldez/socket-programming-tutorial) ![Code Size](https://img.shields.io/github/languages/code-size/marcvaldez/socket-programming-tutorial) ![Repo Size](https://img.shields.io/github/repo-size/marcvaldez/socket-programming-tutorial) ![Last Commit](https://img.shields.io/github/last-commit/marcvaldez/socket-programming-tutorial)
# Socket Programming
This is a project for my Introduction to Networks class in NBCC.

## Table of Contents

#### [What is a Socket?](#what-is-a-socket-1)
#### [Types of Sockets](#types-of-sockets-1)
#### [Socket API](#socket-api-1)
#### [Where do sockets fit in the OSI or TCP/IP model?](#where-do-sockets-fit-in-the-osi-or-tcpip-model-1)
#### [Berkeley Sockets](#berkeley-sockets-1)
#### [List of Common Berkeley Socket API Functions](#list-of-common-berkeley-socket-api-functions-1)
#### [Diagrams of Typical Socket API Call Sequences](#diagrams-of-typical-socket-api-call-sequences-1)
#### [Socket Protocols](#socket-protocols-1)
#### [Domains (aka Protocol Family/Address Family) in Berkeley Sockets](#domains-aka-protocol-familyaddress-family-in-berkeley-sockets-1)
#### [Socket Operating Modes](#socket-operating-modes-1)
#### [Microsoft .NET Socket API Implementation](#microsoft-net-socket-api-implementation-1)
#### [Socket Programming Tutorial using C# and the .Net Framework](#socket-programming-tutorial-using-c-and-the-net-framework-1)
#### [Challenge](#challenge-1)

## What is a Socket?

A socket is one endpoint of a two-way communication link between two programs running on the network.

## Types of Sockets

### Stream Socket
- Also called connection-oriented sockets
- Sends and receives messages through streams (much like reading/writing data from files)
- Doesn't care about message boundaries
- Commonly used for TCP

### Datagram Socket
- Also called connectionless sockets
- Sends and receives messages as independent datagrams with defined boundaries
- Commonly used for UDP

### Raw Socket
- Allows direct sending and receiving of IP packets without any protocol-specific transport layer formatting (basically skips the transport layer)
- Has no concept of ports
- Can be used if you want to implement your own transport protocol
- Also used by network utility programs such as nmap, ping, etc.

## Socket API
- A collection of functions/methods/commands usually provided by the operating system to enable programs to use sockets and communicate with other programs.

## Where do sockets fit in the OSI or TCP/IP model?
![Sockets in the OSI Model](/docs/SocketsontheOSIModel.png)

## Berkeley Sockets
- Used as an application programming interface (API) for sockets in the BSD operating system
- Created by a group of researchers in University of California, Berkeley in the 1980s
- Became the de facto standard for socket APIs and also the basis for most modern socket API implementations such as Linux sockets, POSIX sockets, and Winsock

## List of Common Berkeley Socket API Functions

### socket()
Instantiates a socket object

### bind()
Binds an IP address and port number to a socket object

### connect()
Establishes a connection to a remote host

### listen()
Used by servers to listen for incoming connections

### accept()
Accepts a connection request from the listen() function and returns a new socket associated with the new connection

### send()
Sends data through the socket

### recv()
Receives data from a socket

### close()
Closes the socket

### Other common socket functions include:
- read(), recvfrom(), or recvmsg()
- write(), sendto(), or sendmsg()
- gethostbyname() and gethostbyaddr()
- select()
- poll()

## Diagrams of Typical Socket API Call Sequences

### TCP Call Sequence
![TCP Socket Flow Diagram](/docs/SocketFlowDiagram.png)

### UDP Call Sequence

Note the absense of connection-specific socket calls such as connect(), listen(), and accept() in the UDP diagram.
![UDP Socket Flow Diagram](/docs/SocketFlowDiagramUDP.png)

## Socket Protocols
- IPPROTO_TCP for TCP
- IPPROTO_UDP for UDP
- Other values include IPPROTO_SCTP and IPPROTO_DCCP.

Note that you can use the same port number for different protocols.

## Domains (aka Protocol Family/Address Family) in Berkeley Sockets

### AF_INET/PF_INET
- Corresponds to IPv4
- `AddressFamily.InterNetwork` in the .NET implementation

### AF_INET6/PF_INET6
- Corresponds to IPv6
- `AddressFamily.InterNetworkV6` in the .NET implementation

### AF_UNIX/PF_UNIX
- Corresponds to local sockets (using a file)
- Used in UNIX for inter-process communication (IPC)

## Socket Operating Modes

### Blocking Mode
- When operating in Blocking mode, some socket functions such as accept(), send(), and recv() will block execution until the operation is successful. The accept() function for instance, will wait until a client initiates a connection to the server.
- In .NET, this can be set by assigning `true` to the `Socket.Blocking` property. This is the default value in .NET.

### Non-Blocking Mode
- In non-blocking mode, socket functions will not block execution.
- In .NET, this can be set by assigning `false` to the `Socket.Blocking` property.

Sources:

https://en.wikipedia.org/wiki/Network_socket

https://docs.oracle.com/javase/tutorial/networking/sockets/definition.html

http://homepage.smc.edu/morgan_david/cs70/sockets.htm

http://ijcsit.com/docs/Volume%205/vol5issue03/ijcsit20140503462.pdf

https://docs.microsoft.com/en-us/dotnet/framework/network-programming/

# Microsoft .NET Socket API Implementation

## System.Net.Sockets

- The `System.Net.Sockets.Socket` class is an implementation of the Berkeley Sockets API in the .NET framework.
- In addition to the standard Berkeley Socket functions, it also features methods for asynchronous sending and receiving of data.

## List of Commonly Used Methods in the Socket Class

#### Socket() (Constructor)
#### Bind()
#### Listen()
#### Accept()
#### Connect()
#### Send()
#### Receive()
#### Shutdown()
#### Close()

## Commonly Used Asynchronous Methods

#### AcceptAsync(), BeginAccept(), and EndAccept()
#### SendAsync(), BeginSend(), and EndSend()
#### ReceiveAsync(), BeginReceive(), and EndReceive()

Asynchronous methods are helpful when you do not want your application to block while waiting for operations such as sending and receiving to complete. These are useful for programs that are event-driven or user-facing such as Windows Forms apps.

When using the asynchronous methods, you may no longer need to set the `Blocking` mode of the socket to `false`.

The &ast;Async versions of the methods are used if you prefer to code using the [Task-based asynchronous pattern (TAP)](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap) which is a style of programming asynchronous code.

The Begin&ast; and End&ast; versions can be used if you prefer to code using the more traditional [APM pattern](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/asynchronous-programming-model-apm).

## Commonly Used Properties of the Socket Class

#### Available
Gets the amount of data that has been received from the network and is available to be read.

#### Blocking
Gets or sets the blocking mode of the socket. `true` by default.

#### Connected
Is `true` if the socket is connected.

#### DontFragment
Used if you don't want the packets to be fragmented.

#### RemoteEndPoint
Allows you to get the IP address and port you are connected to on the other end.

#### Ttl
Gets or sets a value that specifies the Time To Live (TTL) value of Internet Protocol (IP) packets sent by the Socket.

Source:

https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=netframework-4.8


# Socket Programming Tutorial using C# and the .Net Framework

In this tutorial, we will create a simple server app and client app that will communicate with each other using TCP.

To keep our apps simple, the client and server apps we are going to create will follow a slightly different process flow from the diagram shown in the previous article. Our process flow will use the following steps:

1. The server will run and start listening for connections.
2. The client will connect to the server and send a message.
3. The server will receive and display the message from client.
4. The server will respond with its own message and then close the connection.
5. The client will receive the server's reponse and display it on the UI.

## Part 1: Creating the Server Application

### Step 1

Create a new Console App (.NET Framework) project in Visual Studio.

![Creating a console application](/docs/step1.png)

### Step 2

Open the Program.cs file and add the following lines near the top.

```csharp
using System.Net;
using System.Net.Sockets;
```

This will let us use the `System.Net` and `System.Net.Sockets` classes in such a way that we don't have to type the namespaces in our code.

### Step 3

Add the following lines of code inside the `static void Main()` method.

```csharp
IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 55555);
```

These lines will create an `IPAddress` object and `IPEndpoint` object with an IP address of 127.0.0.1 and a port number of 55555. For now, we will be using the loopback address as the server address to make it easy for us to test the application later. You can change this later if you want to test this on a real network. Also note that the 55555 is just an arbitrary port number. Feel free to use any unused port number on your machine.

### Step 4

Add the following lines after the lines in Step 3.

```csharp
Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
connectionSocket.Bind(ipEndPoint);
connectionSocket.Listen(10);

Console.WriteLine(ipEndPoint.ToString() + ": Waiting for clients...");
```

The first line in this section instantiates a new socket. The new socket is specified to be a Stream socket that uses TCP as the transport protocol and uses the Internet (InterNetwork) as the network protocol.

The second line binds the IP address and port we specified in Step 3 to the socket we instantiated.

The third line tells the socket to listen for any connection attempts and have up to 10 connections in the listening queue.

Note that the socket we created will only be used for listening to connection requests, hence the name "connectionSocket". We will use a different socket for sending and receiving messages and it will be created on the next step.

### Step 5

Create the try-catch block in the code below after the code in Step 4.

```csharp
try
{
    Socket messageSocket = connectionSocket.Accept();
    string clientAddress = messageSocket.RemoteEndPoint.ToString();

}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
```

The above code will accept the first connection in the listening queue of the connection socket and instantiate a brand new socket that we can use for communicating with the client on the connection we accepted.

We can get the address of the client by accessing the RemoteEndpoint property of the newly created socket.

### Step 6

Inside the try-catch block right at the end, add the following code.

```csharp
do
{
    byte[] buffer = new byte[1000];
    int numberOfBytesReceived = messageSocket.Receive(buffer);

    string message = Encoding.UTF8.GetString(buffer, 0, numberOfBytesReceived);

    Console.WriteLine(clientAddress + " said: " + message);
} while (messageSocket.Available > 0);
```

The code above uses a do-while loop to process messages from the socket until we can no longer read messages from the stream. In the first step of the process, we create a byte array called buffer and then use that to receive the data from the socket. We then convert the data from the buffer into a UTF-8 encoded string and display it on the screen.

### Step 7

Add the following code after the do-while loop.

```csharp
string response = "Congratulations! You have mastered socket programming.";
messageSocket.Send(Encoding.UTF8.GetBytes(response));

messageSocket.Shutdown(SocketShutdown.Both);
messageSocket.Close();
```

In this code, we create a string response message and convert it to a byte array and use the send() method of our socket to send it to the client. To simplify the program, we immediately shutdown and close the socket after sending the message.

### Step 8

Finally, add the following lines at the end, after the catch block.

```csharp
Console.WriteLine();
Console.WriteLine("Press ENTER to continue...");
Console.Read();
```

This code will prevent the console window from immediately closing after the program ends.

## Part 2: Creating the client application

### Step 1

In the same solution as our server application, create a new Windows Forms application with a user interface similar to the image below.

![Client user interface](/docs/democlientui.png)

Name the "Connect" button as `btnConnect` and the textboxes as `txtIpAddress`, `txtPort`, `txtMessage` and `txtResponse`.

### Step 2

Add the following code to the click event handler of btnConnect.

```csharp
if (!IPAddress.TryParse(txtIpAddress.Text, out IPAddress ipAddress))
{
    MessageBox.Show("Please enter a valid IP Address.");
}
else if (!int.TryParse(txtPort.Text, out int port) || port < 1 || port > 65535)
{
    MessageBox.Show("Please enter a valid port number.");
}
else if (txtMessage.Text == string.Empty)
{
    MessageBox.Show("Please enter a message to send to the server.");
}
else
{
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    socket.Connect(new IPEndPoint(ipAddress, port));

    byte[] buffer = Encoding.UTF8.GetBytes(txtMessage.Text);

    socket.Send(buffer);

    do
    {
        int numberOfBytesReceived = socket.Receive(buffer);

        string message = Encoding.UTF8.GetString(buffer, 0, numberOfBytesReceived);

        txtResponse.AppendText(message);

    } while (socket.Available > 0);

    txtResponse.AppendText(Environment.NewLine);

}
```

The above code first validates user input and then creates a socket similar to how we did it in the server application. Instead of binding into an IP address and port, we directly call the socket's connect() method and specify the IP address and port number we want to connect to. The connect() method also automatically assigns the local machine's IP address to the socket and finds an unused ethereal port for the socket to use.

After connecting to the server, we send a message the same way we did in the server application. Then we also create a loop similar to what we did in the server app for reading data from the socket and converting the data into a UTF-8 encoded string. We then display the string containing the server's response by appending it to the contents of the txtResponse textbox.


## Part 3: Putting It All Together

### Step 1

Run both the server app and the client app at the same time. If the client and server apps are in the same solution, you can easily do this by setting multiple startup projects in your solution.

![Multiple startup projects in Visual Studio](/docs/multiplestartups.png)

To get to the Solution Property Pages dialog, right-click on the solution in the Solution Explorer and click on "Set Startup Projects..."

![Solution context menu](/docs/rightclicksolution.png)

### Step 2

Fill in the server IP address and port number on the client app. Enter your message in the textbox then click on the "Connect" button.

![Running client application](/docs/democlientrun.png)

### Step 3

If everything goes well, you should be able to see the client's message on the server app and the server's response in the output textbox on the client app.


## Challenge

Try implementing the following improvements to the client and server apps to practice your socket programming skills:

- Add a "Disconnect" button to client app and make the client and server continue exchanging messages until the "Disconnect" button is clicked.
- Modify the server app to make it able to connect to multiple clients.
- Convert the client app into a peer-to-peer chat program by adding some of the server code to the client. (A working peer-to-peer chat program is included in this tutorial. You can download it from https://github.com/marcvaldez/Socket-Programming-Tutorial.)

First draft
# Socket Programming
This is a project for my Introduction to Networks class in NBCC.

## What is a Socket?

A socket is one endpoint of a two-way communication link between two programs running on the network.

## Types of Sockets

### Stream Socket
- Also called connection-oriented sockets
- Sends messages through streams (i.e. continue sending and receiving messages until someone closes the stream)
- Doesn't care about message boundaries
- Commonly used for TCP

### Datagram Socket
- Also called connectionless sockets
- Sends messages as independent packets with defined boundaries (i.e. one read = one message)
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
- Originally used as an application programming interface (API) for sockets in the BSD operating system
- Created by a group of researchers in University of California, Berkeley in the 1980s
- Became the de facto standard for socket APIs and also the basis for most modern socket API implementations such as Linux sockets, POSIX sockets, and Windows sockets

## List of Common Socket API Functions

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
- close()
- gethostbyname() and gethostbyaddr()
- select()
- poll()


Sources:

https://en.wikipedia.org/wiki/Network_socket
https://docs.oracle.com/javase/tutorial/networking/sockets/definition.html
http://homepage.smc.edu/morgan_david/cs70/sockets.htm
http://ijcsit.com/docs/Volume%205/vol5issue03/ijcsit20140503462.pdf
https://docs.microsoft.com/en-us/dotnet/framework/network-programming/



# Socket Programming Tutorial using C# and the .Net Framework

In this tutorial, we will create a simple server app and client app that will communicate with each other using TCP.

The diagram below shows a typical process flow for a TCP client-server application

![Socket Flow Diagram](/docs/SocketFlowDiagram.png)

## Part 1: Creating the server program

### Step 1

Create a new Console app (.NET Framework) project in Visual Studio.

![Creating a console application](/docs/step1.png)

### Step 2

Open the Program.cs file and add the following lines near the top.

```CSharp
using System.Net;
using System.Net.Sockets;
```

This will let us use the System.Net and System.Net.Sockets classes is such a way that we don't have to type the namespaces in our code.

### Step 3

Add the following lines of code inside the static void Main() method.

```CSharp
IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 55555);
```

These lines will create an IPAddress object and IPEndpoint object with an IP address of 127.0.0.1 and a port number of 55555. For now, we will be using the loopback address as the server address to make it easy for us to test the application later. You can change this later if you want to test this on a real network. Also note that the 55555 is just an arbitrary port number. Feel free to use any unused port number on your machine.

### Step 4

Add the following lines after the lines in Step 3.

```CSharp
Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
connectionSocket.Bind(ipEndPoint);
connectionSocket.Listen(10);

Console.WriteLine(ipEndPoint.ToString() + ": Waiting for clients...");
```

The first line in this section instantiates a new socket. The new socket is specified to be a Stream socket that uses TCP as the transport protocol and uses the Internet (InterNetwork) as the network protocol.

The second line binds the IP address and port we specified in Step 3 to the socket we instantiated.

The third line tells the socket to listen for any connection attempts and have up to 10 connections in the listening queue.

Note that the socket we created will only be used for listening to connections, hence the name "connectionSocket". We will use a different socket for sending and receiving messages and it will be created on the next step.

### Step 5

Create the try-catch block in the code below after the code in Step 3.

```CSharp
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

```CSharp
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

```CSharp
string response = "Congratulations! You have mastered socket programming.";
messageSocket.Send(Encoding.UTF8.GetBytes(response));

messageSocket.Shutdown(SocketShutdown.Both);
messageSocket.Close();
```

In this code, we create a string response message and convert it to a byte array and use the send() method of our socket to send it to the client. To simplify the program, we immediately shutdown and close the socket after sending the message.

### Step 8

Finally, add the following lines at the end, after the catch block.

```CSharp
Console.WriteLine();
Console.WriteLine("Press ENTER to continue...");
Console.Read();
```

This code will prevent the console window from immediately closing after the program ends.

## Part 2: Creating the client application

### Step 1

In the same solution as our server application, create a new Windows Forms application with a user interface similar to the image below.

![Client user interface](/docs/democlientui.png)

Name the "Connect" button as btnConnect and the textboxes as txtIpAddress, txtPort, txtMessage and txtResponse.

### Step 2

Add the following code to the click event handler of btnConnect.

```CSharp
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

The above code first validates user input then creates a socket similar to how we did it in the server application. Instead of binding into an IP address and port, we directly call the socket's connect() method and specify the IP address and port number we want to connect to. The connect() method also automatically assigns the local machine's IP address to the socket and finds an unused ethereal port for the socket to use.

After connecting the socket, we send a message the same way we did in the server application. Then we also create a loop similar to what we did in the server app for reading data from the socket and converting the data into a UTF-8 encoded string. We then display the string containing the server's response by appending it to the contents of the txtResponse textbox.


## Part 3: Putting It All Together

### Step 1

Run both the server app and the client app at the same time. If the client and server apps are in the same solutions, you can easily do this by setting multiple startup projects in your solution.

![Multiple startup projects in Visual Studio](/docs/multiplestartups.png)

To get to the Solution Property Pages dialog, right-click on the solution in the Solution Explorer and click on "Set Startup Projects..."

### Step 2

Fill in the server IP address and port number on the client app. Enter your message in the textbox then click on the "Connect" button.

![Running client application](/docs/democlientrun.png)

### Step 3

If everything goes well, you should be able to see the client's message on the server app and the server's response in the output textbox on the client app.


using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Peer2PeerChat
{
    public partial class Form1 : Form
    {
        // We declare two sockets: one for listening for incoming connection requests 
        // and another for handling the actual sending and receiving of messages
        Socket connectionSocket = null;
        Socket messageSocket = null;

        // We want to stop listening for incoming connections when we are already connected to someone or if we initiate the connection ourselves
        bool stopListening = false;

        // These two lines of code will get the local ip address and port number from the config file and store them into global variables
        string ipAddress = ConfigurationManager.AppSettings["ipaddress"];
        int port = int.Parse(ConfigurationManager.AppSettings["port"]);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // When the form loads, create a socket and wait for incoming connections
            IPAddress localAddress = IPAddress.Parse(ipAddress);
            IPEndPoint endpoint = new IPEndPoint(localAddress, port);
            connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectionSocket.Bind(endpoint);
            connectionSocket.Listen(1);

            // Start the timer for listening to connection requests
            tmrListenTimer.Interval = 1000;
            tmrListenTimer.Start();
            tmrListenTimer.Tick += tmrListenTimer_Tick;

            // Start the timer for receiving messages
            tmrReceiveMessageTimer.Interval = 200;
            tmrReceiveMessageTimer.Start();
            tmrReceiveMessageTimer.Tick += TmrReceiveMessageTimer_Tick;

            // Here we display the ip address and the port number on the top of the form
            Text = "P2P Chat - " + ipAddress + ":" + port;
        }

        // This method is for accepting any pending connection requests to our socket.
        private void tmrListenTimer_Tick(object sender, EventArgs e)
        {
            // We only want to listen to connection requests if we aren't connected yet, hence this flag
            if (!stopListening)
            {
                Output("Waiting for connection...");

                // The code inside the curly braces below will run after the socket accepts the connection.
                connectionSocket.BeginAccept((ar) =>
                {
                    if (messageSocket == null)
                    {
                        // Once a connection is accepted, a new socket will be created and that socket is 
                        // what we need to use for communication. We assign this socket to the messageSocket variable
                        messageSocket = connectionSocket.EndAccept(ar);
                        Output("Connected...");

                        // We should stop listening once we've already accepted a connection.
                        stopListening = true;
                    }
                }, connectionSocket);
            }
        }

        // This method will allow us to accept any pending connection requests to our socket.
        private void TmrReceiveMessageTimer_Tick(object sender, EventArgs e)
        {
            // Before receiving a message, let's check first if we have a functioning communication socket.
            if (messageSocket != null && messageSocket.Connected)
            {
                // We create a buffer for storing the raw data we will receive.
                byte[] buffer = new byte[1000];

                // We tell the socket to check if there are any messages and if there are, 
                // put the data in the buffer and then run the ReceiveCallback method.
                messageSocket.BeginReceive(buffer, 0, 1000, SocketFlags.None, new AsyncCallback(ReceiveCallback), buffer);
            }
        }

        // This method is run when the communication socket receives a message.
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            // At this point, the data from the message is stored in asyncResult.AsyncState. Let's store in in buffer.
            byte[] buffer = (byte[])asyncResult.AsyncState;
            int bytesRead = messageSocket.EndReceive(asyncResult);
            if (bytesRead > 0)
            {
                // We convert the data in the buffer into a string
                string msg = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Output(msg);

                // We check again for more messages and repeat the process to get the rest of the data.  
                buffer = new byte[1000];
                messageSocket.BeginReceive(buffer, 0, 1000, 0, new AsyncCallback(ReceiveCallback), buffer);
            }
        }

        // If the connect button is clicked, we stop listening to connection attempts and initiate a connection
        // to the ip address and port specified in the form textboxes.
        private void BtnConnect_Click(object sender, EventArgs e)
        {
            // We stop listening because we want to initiate the connection ourselves
            stopListening = true;
            connectionSocket.Close();

            // create a socket for sending messages
            IPAddress localAddress = IPAddress.Parse(ipAddress);
            IPEndPoint endpoint = new IPEndPoint(localAddress, port);
            messageSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            messageSocket.Bind(endpoint);

            // connect to the socket specified in the IP address textbox and Port textbox
            IPAddress buddyAddress = IPAddress.Parse(txtIpAddress.Text);
            int buddyPort = int.Parse(txtPort.Text);
            messageSocket.Connect(buddyAddress, buddyPort);

            Output("Connected...");

            // disable the connect button
            btnConnect.Enabled = false;

            // set the focus on the chat input textbox
            txtMessage.Focus();
        }

        // This method will send a message through the connected socket
        private void BtnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text;
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            messageSocket.Send(buffer);

            // We output our own message to our own screen to make it more chat-like. Another way to do this is to 
            // echo every message we receive back to the sender but that would be too complicated for our purposes.
            Output(message);

            // clear the chat input
            txtMessage.Clear();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        delegate void SetTextCallback(string text);

        // This method displays text to the screen
        private void Output(string text)
        {
            // What all these complicated code does is just to be able to add text to txtOutput 
            // from different threads. We actually use threads when executing code through timers
            if (this.txtOutput.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Output);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtOutput.AppendText(text + Environment.NewLine);
            }
        }
    }
}

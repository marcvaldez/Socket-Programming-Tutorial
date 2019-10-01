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
        // and another for handling the actual communication (i.e. sending and receiving messages)
        Socket connectionSocket = null;
        Socket messageSocket = null;

        // We want to stop listening for incoming connections when we are already connected to someone or if we initiate the connection ourself
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
            // when the form loads, create a socket and wait for incoming connections
            IPAddress localAddress = IPAddress.Parse(ipAddress);
            IPEndPoint endpoint = new IPEndPoint(localAddress, port);
            connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectionSocket.Bind(endpoint);
            connectionSocket.Listen(1);

            // start the timer for listening to connection requests
            tmrListenTimer.Interval = 1000;
            tmrListenTimer.Start();
            tmrListenTimer.Tick += tmrListenTimer_Tick;

            // start the timer for receiving messages
            tmrReceiveMessageTimer.Interval = 200;
            tmrReceiveMessageTimer.Start();
            tmrReceiveMessageTimer.Tick += TmrReceiveMessageTimer_Tick;

            // here we display the ip address and the port number on the top of the form
            Text = "P2P Chat - " + ipAddress + ":" + port;
        }

        // This method is executed every second. Running this code will accept any pending connection requests to our socket.
        private void tmrListenTimer_Tick(object sender, EventArgs e)
        {
            // we only want to listen to connection requests if we aren't connected yet, hence this flag
            if (!stopListening)
            {
                Output("Waiting for connection...");

                // the code inside the curly braces below will run after the socket accepts the connection.
                connectionSocket.BeginAccept((ar) =>
                {
                    if (messageSocket == null)
                    {
                        messageSocket = connectionSocket.EndAccept(ar);
                        Output("Connected...");

                        stopListening = true;
                    }
                }, connectionSocket);
            }
        }

        private void TmrReceiveMessageTimer_Tick(object sender, EventArgs e)
        {
            if (messageSocket != null && messageSocket.Connected)
            {
                // receive incoming message
                byte[] buffer = new byte[1000];

                messageSocket.BeginReceive(buffer, 0, 1000, SocketFlags.None, new AsyncCallback(ReceiveCallback), buffer);
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            byte[] buffer = (byte[])asyncResult.AsyncState;
            int bytesRead = messageSocket.EndReceive(asyncResult);
            if (bytesRead > 0)
            {
                string msg = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Output(msg);

                // Get the rest of the data.  
                buffer = new byte[1000];
                messageSocket.BeginReceive(buffer, 0, 1000, 0, new AsyncCallback(ReceiveCallback), buffer);
            }
        }


        private void BtnConnect_Click(object sender, EventArgs e)
        {
            // stop listening
            stopListening = true;
            connectionSocket.Close();

            // create a socket for sending messages
            IPAddress localAddress = IPAddress.Parse(ipAddress);
            IPEndPoint endpoint = new IPEndPoint(localAddress, port);
            messageSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            messageSocket.Bind(endpoint);

            // connect to your buddy's device
            IPAddress buddyAddress = IPAddress.Parse(txtIpAddress.Text);
            int buddyPort = int.Parse(txtPort.Text);
            messageSocket.Connect(buddyAddress, buddyPort);

            Output("Connected...");

            // disable the connect button
            btnConnect.Enabled = false;

            // set the focus on the chat input textbox
            txtMessage.Focus();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text;
            Output(message);
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            messageSocket.Send(buffer);

            // clear the chat input
            txtMessage.Clear();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        delegate void SetTextCallback(string text);

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

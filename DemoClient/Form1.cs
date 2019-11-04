using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace DemoClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Validate all the input before sending the message to the server.
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
                // Here we create a new Socket instance named "socket" using an Internet address and using TCP as the transport protocol
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(ipAddress, port));

                // We convert our string into raw bytes before we send them through the network
                byte[] buffer = Encoding.UTF8.GetBytes(txtMessage.Text);

                // This line will send to message
                socket.Send(buffer);

                // Here we do a loop where we receive data from the socket until there are no available data left to be received
                do
                {
                    // This line will receive the data and store them in the "buffer" variable
                    int numberOfBytesReceived = socket.Receive(buffer);

                    // Here we convert the buffer to a string called message
                    string message = Encoding.UTF8.GetString(buffer, 0, numberOfBytesReceived);

                    // Then we add the message to the output textbox txtResponse
                    txtResponse.AppendText(message);

                    // Loop while there is data available to be received in the socket
                } while (socket.Available > 0);

                txtResponse.AppendText(Environment.NewLine);

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

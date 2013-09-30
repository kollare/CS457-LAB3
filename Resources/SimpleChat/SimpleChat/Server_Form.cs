using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace SimpleChat
{
    public partial class Server_Form : Form
    {
        Socket serverSocket;
       // private Socket clientSocket;
        private Socket sending;

        byte[] byteData = new byte[1024];
        public Server_Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine +textBox2.Text;

            try
            {

                byte[] bytes = Encoding.ASCII.GetBytes(textBox2.Text);

                //Send it to the server
               sending.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(OnSend), sending);
                bytes = null;

                textBox2.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to send message to the server.", "Client ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Server_Form_Load(object sender, EventArgs e)
        {
            try
            {

                serverSocket = new Socket(AddressFamily.InterNetwork,
                                          SocketType.Stream,
                                          ProtocolType.Tcp);


                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 1000);

                //Bind and listen on the given address
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(4);

                //Accept the incoming clients
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  

        }
        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = serverSocket.EndAccept(ar);
                sending = clientSocket;
                
                //Start listening for more clients
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);

                //Once the client connects then start receiving the commands from her
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                    new AsyncCallback(OnReceive), clientSocket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
              Socket clientSocket = (Socket)ar.AsyncState;
                clientSocket.EndReceive(ar);
                ASCIIEncoding enc = new ASCIIEncoding();
                string data = enc.GetString(byteData);
                textBox1.Text = textBox1.Text + Environment.NewLine + data;

              //  byte[] bytes = Encoding.ASCII.GetBytes(data);
              //clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None,
              //                  new AsyncCallback(OnSend), clientSocket);
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

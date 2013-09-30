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
    public partial class Client_Form : Form
    {
        public Socket clientSocket; 
        private byte[] byteData = new byte[1024];
        public Client_Form()
        {
            InitializeComponent();
        }

        private void Client_Form_Load(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                //Server is listening on port 1000
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1000);

                //Connect to the server
                clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SGSclient", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
           
           

            byteData = new byte[1024];
            //Start listening to the data asynchronously
            clientSocket.BeginReceive(byteData,
                                       0,
                                       byteData.Length,
                                       SocketFlags.None,
                                       new AsyncCallback(OnReceive),
                                       null);
        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndReceive(ar);

                ASCIIEncoding enc = new ASCIIEncoding();
                string data = enc.GetString(byteData);
                textBox1.Text = textBox1.Text + Environment.NewLine + data;
                //Accordingly process the message received
               
                clientSocket.BeginReceive(byteData,
                                          0,
                                          byteData.Length,
                                          SocketFlags.None,
                                          new AsyncCallback(OnReceive),
                                          null);

            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "clientTCP: " , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                byte[] bytes = Encoding.ASCII.GetBytes(textBox2.Text);

                //Send it to the server
                clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                textBox1.Text = textBox1.Text + Environment.NewLine + textBox2.Text;
                textBox2.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to send message to the server.", "Client " , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Client " , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

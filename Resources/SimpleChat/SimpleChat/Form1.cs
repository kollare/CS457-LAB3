using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server_Form f=new Server_Form();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           Client_Form f=new Client_Form();
            f.Show();
        }
    }
}

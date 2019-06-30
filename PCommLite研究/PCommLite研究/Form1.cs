using FIRC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PCommLite研究
{
    public partial class Form1 : Form
    {
        SerialPortManger sp = new SerialPortManger();
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!sp.IsOpen)
            {
                sp.PortIndex=1;
                sp.Open(1);
                sp.OnReceiveData += sp_OnReceiveData;
                sp.SetPort(1,9600);
               
            }
                
        }

        void sp_OnReceiveData(byte[] data)
        {
            textBox1.AppendText(Encoding.Default.GetString(data)+"\r\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sp.Close();
               
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sp.WriteLine("hello tomorrow");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace FireflyExtensionLights
{
    public partial class Form1 : Form
    {
        protected BackgroundWorker _serialReader = new BackgroundWorker();
        protected SerialPort _serialPort;

        public Form1()
        {
            InitializeComponent();
        }

        public void DrawIt(FireflySerialMessage m)
        {
            Graphics graphics = CreateGraphics();

            var leftValid = new Rectangle(20, 20, 50, 50);
            var leftInvalid = new Rectangle(80, 20, 50, 50);
            var rightInvalid = new Rectangle(180, 20, 50, 50);
            var rightValid = new Rectangle(240, 20, 50, 50);
            var leftGround = new Rectangle(20, 100, 10, 10);
            var rightGround = new Rectangle(280, 100, 10, 10);

            graphics.FillRectangle(Brushes.Gray, leftValid);
            if (m.LeftValid)
                graphics.FillRectangle(Brushes.Red, leftValid);
            graphics.DrawRectangle(Pens.Black, leftValid);

            graphics.FillRectangle(Brushes.Gray, leftInvalid);
            if (m.LeftInvalid)
                graphics.FillRectangle(Brushes.White, leftInvalid);
            graphics.DrawRectangle(Pens.Black, leftInvalid);

            graphics.FillRectangle(Brushes.Gray, rightValid);
            if (m.RightValid)
                graphics.FillRectangle(Brushes.Green, rightValid);
            graphics.DrawRectangle(Pens.Black, rightValid);

            graphics.FillRectangle(Brushes.Gray, rightInvalid);
            if (m.RightInvalid)
                graphics.FillRectangle(Brushes.White, rightInvalid);
            graphics.DrawRectangle(Pens.Black, rightInvalid);

            graphics.FillEllipse(Brushes.Gray, leftGround);
            if (m.LeftGround)
                graphics.FillEllipse(Brushes.Yellow, leftGround);
            graphics.DrawEllipse(Pens.Black, leftGround);

            graphics.FillEllipse(Brushes.Gray, rightGround);
            if (m.RightGround)
                graphics.FillEllipse(Brushes.Yellow, rightGround);
            graphics.DrawEllipse(Pens.Black, rightGround);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawIt(new FireflySerialMessage());
            Console.WriteLine("Starting serial port...");
            _serialPort = new SerialPort(comboBox1.Text, 4800, Parity.None, 8, StopBits.One);
            _serialPort.Open();
            _serialReader.DoWork += SerialReader_DoWork;
            _serialReader.RunWorkerAsync();

        }

        private void SerialReader_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                var b = _serialPort.ReadByte();
                var msg = new FireflySerialMessage();
                if( (b & 0x80) > 0)
                {
                    // basic message
                    msg.Buzzer = (b & 0x40) > 0;
                    msg.RightGround = (b & 0x20) > 0;
                    msg.LeftGround = (b & 0x10) > 0;
                    msg.RightInvalid = (b & 0x08) > 0;
                    msg.LeftInvalid = (b & 0x04) > 0;
                    msg.RightValid = (b & 0x02) > 0;
                    msg.LeftValid = (b & 0x01) > 0;
                    DrawIt(msg);
                }
                else
                {
                    // extended message
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = System.IO.Ports.SerialPort.GetPortNames();
        }
    }
}

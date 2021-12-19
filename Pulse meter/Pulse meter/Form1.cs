using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO.Ports;
using rtChart;

namespace Pulse_meter
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

          
                materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue400, Primary.Blue500,
                Primary.Blue500, Accent.LightBlue200,
                TextShade.WHITE);

           getports();
        }
        kayChart serialDataChart;
        private DateTime datetime;
        void getports()
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "" || comboBox2.Text == "")
                {
                    textBox1.Text = "እባክዎ ፖርት መሙያውን በትክክል ይሙሉ";
                }
                else
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;
                    textBox1.Text = "ሲስትሙ ተጀምራል";
                    progressBar1.Value = 100;
                    serialPort1.Open();
                    serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceivedEventHandler);
                    if (!serialPort1.IsOpen)
                    {
                        serialPort1.Open();
                    }
                    serialPort1.Write("a");
                }
            }

            catch (UnauthorizedAccessException)
            {
                textBox1.Text = "ያልተፈቀደ ትእዛዝ";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            textBox1.Text = "ሲስተሙ ተዘግቷል";
            progressBar1.Value = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialDataChart = new kayChart(chart1, 60);

            serialDataChart.serieName = "Pulse meter";
        }
        private void SerialDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
        {

            datetime = DateTime.Now;
            SerialPort output = sender as SerialPort;
            string input = output.ReadLine();

            textBox2.Invoke((MethodInvoker)delegate { textBox2.AppendText( datetime + "Pulse meter" + input + "\n"); });
            // initiaization of chart
            double data;
            bool result = Double.TryParse(input, out data);
            if (result)
            {
                serialDataChart.TriggeredUpdate(data);
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}

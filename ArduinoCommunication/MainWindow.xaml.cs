using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArduinoLedScreen74hc595;
using System.Management;
using System.Diagnostics;
using System.Windows.Threading;
using CpuInfoLib;

namespace ArduinoCommunication
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }
        public ArduinoLedScreen74hc595.Class1 Arduino = new Class1();
        CpuInfoLib.CPUINFO CPUinfo = new CPUINFO();
        DispatcherTimer timer = new DispatcherTimer();
        private string _Port;
        private int _BaudRate;
        private float _Delay;

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            _Port = TextBoxCom.Text;
            try
            {
                Int32.TryParse(TextBoxRate.Text, out _BaudRate);
                float.TryParse(TextBoxDelay.Text, out _Delay);

                timer.Interval = TimeSpan.FromSeconds(_Delay);

                if(Arduino.serialPort.IsOpen)
                {
                    timer.Start();        
                }
                else if(!Arduino.serialPort.IsOpen)
                {
                    Arduino.OpenPort(_Port, _BaudRate);
                    timer.Start();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error: " + Ex.Message);
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            
            Arduino.serialPort.Write("CT:" + (int)CPUinfo.CPUTemp() + ";"); //CPU Temp
            Arduino.serialPort.Write("GT:" + (int)CPUinfo.GPUTemp() + ";"); //GPU Temp
            Arduino.serialPort.Write("CU:" + (int)CPUinfo.CPUusage() + ";"); // CPU Usage
            Arduino.serialPort.Write("GU:" + (int)CPUinfo.GPUusage() + ";"); //GPU Usage
            Arduino.serialPort.Write("RU:" + (int)CPUinfo.RamUsage() + ";"); //Ram Usage
            //Arduino.serialPort.Write("TR:" + (int)CPUinfo.RamTemp() + ";"); //Ram Temp*/

            //Arduino.SetNumber((int)CPUinfo.CPUTemp());

            LabelCPUTemp.Content = "CPU Temp: " + CPUinfo.CPUTemp() + "°C";//
            LabelGPUTemp.Content = "GPU Temp" + CPUinfo.GPUTemp() + "°C";//
            CPUusage.Content = "CPU Usage" + (int)CPUinfo.CPUusage();//
            GPUusage.Content = "GPU Usage" + CPUinfo.GPUusage();//
            RamUsage.Content = "Ram Usage" + (int)CPUinfo.RamUsage();//
            RamTemp.Content = "Ram Temp: " + CPUinfo.RamTemp();//
        }

    }
}

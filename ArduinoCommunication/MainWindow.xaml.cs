﻿using MahApps.Metro.Controls;
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
using System.ComponentModel;

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
            Port = Properties.Settings.Default.Port;
            BaudRate = Properties.Settings.Default.BaudRate;
            Delay = Properties.Settings.Default.Delay;
            _Sending = Properties.Settings.Default.SendAfterStart;

            CheckBoxSendAfterStart.IsChecked = Properties.Settings.Default.SendAfterStart;
            TextBoxDelay.Text = Delay.ToString();
            TextBoxRate.Text = BaudRate.ToString();
            TextBoxCom.Text = Port.ToString();
            Closing += OnClosing;

            if(_Sending == true)
            {
                timer.Start();
            }
        }
        public ArduinoLedScreen74hc595.Class1 Arduino = new Class1();
        CpuInfoLib.CPUINFO CPUinfo = new CPUINFO();
        DispatcherTimer timer = new DispatcherTimer();
        public string Port;
        public int BaudRate;
        public float Delay;
        private bool _Sending;

        internal void OnClosing(object sender, CancelEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Port = Port.ToString();
                Properties.Settings.Default.Delay = Delay;
                Properties.Settings.Default.BaudRate = BaudRate;

                //save all settings to file
                Properties.Settings.Default.Save();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error: " + Ex.Message);
            }
        } 

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            _Sending = true;
            Port = TextBoxCom.Text;
            try
            {
                Int32.TryParse(TextBoxRate.Text, out BaudRate);
                float.TryParse(TextBoxDelay.Text, out Delay);

                timer.Interval = TimeSpan.FromSeconds(Delay);

                if (Arduino.serialPort.IsOpen)
                {
                    timer.Start();
                }
                else if (!Arduino.serialPort.IsOpen)
                {
                    Arduino.OpenPort(Port, BaudRate);
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

            if (_Sending == true)
            {
                if (!Arduino.serialPort.IsOpen)
                {
                    try
                    {
                        Arduino.serialPort.Close();
                        Arduino.OpenPort(Port, BaudRate);
                    }
                    catch (Exception Ex)
                    {
                        //MessageBox.Show("Error: " + Ex);
                    }
                }

                if (Arduino.serialPort.IsOpen)
                {
                    /*Arduino.serialPort.Write("CT:" + (int)CPUinfo.CPUTemp() + ";"); //CPU Temp
                    Arduino.serialPort.Write("GT:" + (int)CPUinfo.GPUTemp() + ";"); //GPU Temp
                    Arduino.serialPort.Write("CU:" + (int)CPUinfo.CPUusage() + ";"); // CPU Usage
                    Arduino.serialPort.Write("GU:" + (int)CPUinfo.GPUusage() + ";"); //GPU Usage
                    Arduino.serialPort.Write("RU:" + (int)CPUinfo.RamUsage() + ";"); //Ram Usage*/
                    Arduino.SetNumber((int)CPUinfo.CPUTemp());
                }
            }
            else
            {
                if(Arduino.serialPort.IsOpen)
                {
                    Arduino.SetNumber(0);
                    Arduino.serialPort.Close();
                }
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _Sending = false;
            Arduino.serialPort.Close();
        }

        private void CheckBoxSendAfterStart_Checked(object sender, RoutedEventArgs e)
        {
            SaveCBsendAfterStart();
        }
        private void CheckBoxSendAfterStart_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveCBsendAfterStart();
        }

        private void SaveCBsendAfterStart()
        {
            Properties.Settings.Default.SendAfterStart = CheckBoxSendAfterStart.IsChecked.Value;
            Properties.Settings.Default.Save();
        }

    }
}

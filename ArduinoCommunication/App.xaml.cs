using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.ComponentModel;

namespace ArduinoCommunication
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow mainWindow = new MainWindow();
        Properties.Settings Settings;
        
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if(mainWindow.Arduino.serialPort.IsOpen)
            {
                mainWindow.Arduino.serialPort.Close();
            }

        }

        /*private void Application_Exit(object sender, ExitEventArgs e)
{
   MessageBox.Show("Exit Event Raised", "Exit");
}*/
    }
}

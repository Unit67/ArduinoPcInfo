using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    class Save
    {
        public void SaveProg(string Port,int BaudRate, float Delay, bool Sending)
        {
            Port = Properties.Settings.Default.Port;
            BaudRate = Properties.Settings.Default.BaudRate;
            Delay = Properties.Settings.Default.Delay;
            Sending = Properties.Settings.Default.SendAfterStart;
        }
    }
}

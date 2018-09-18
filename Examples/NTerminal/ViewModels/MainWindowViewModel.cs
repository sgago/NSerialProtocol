using System.Collections.Generic;
using System.IO.Ports;

namespace NTerminal.ViewModels
{
    public class MainWindowViewModel
    {
        public static IEnumerable<string> PortNames
        {
            get
            {
                return SerialPort.GetPortNames();
            }
        }
    }
}

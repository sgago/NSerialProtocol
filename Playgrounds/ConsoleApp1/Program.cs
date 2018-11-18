using NByteStuff.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Babs babs = new Babs(10, 16);

            //string convert1 = string.Concat(babs.Convert("\u0001\u0000", 256, 255, Encoding.UTF8).Reverse());
            //string convert2 = string.Concat(babs.Convert("\u0001\u0000", 255, 256, Encoding.UTF8).Reverse());

            // This converts 
            List<byte> c = babs.Convert(new List<byte> { 1, 5 }, 10, 16);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Критический_Путь_Экзамен_Болдин
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(File.CreateText("Log-file.txt")));
            Debug.AutoFlush = true;
            CrithWay crw = new CrithWay();
            crw.Work();
        }
    }
}

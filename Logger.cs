using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
   public static class Logger
    {
        private static readonly Lazy<TextWriter> file; 
        static Logger()
        {
            file = new Lazy<TextWriter>(() => File.CreateText("G:\\archLog.txt"),false);
        }

        public static void PrintOnConsole(this string message)
        {
            Console.WriteLine(message);
        }

        public static void PrintInLine(this string message)
        {            
            Console.Write(message);
        }

        public static void LogToFile(this string message)
        {
            lock (file)
            {
                file.Value.WriteLine(message);
            }
        }
    }
}

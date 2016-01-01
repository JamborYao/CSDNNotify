using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NotificationSystem.Common
{
    public class LogHelper
    {
       
        public static void LogMessage(string message)
        { 
            string filename = Environment.CurrentDirectory + "\\log.txt";
            using (FileStream stream = File.Open(filename, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);  
                stream.Write(bytes, 0, bytes.Length); 
            }

        }
    }
}

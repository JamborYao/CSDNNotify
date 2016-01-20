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
            string filename = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
            using (FileStream stream = File.Open(filename, FileMode.Append, FileAccess.Write, FileShare.None))   
            {
                string messageWithDate = DateTime.Now.ToString() + message+"\r\n";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(messageWithDate);     
                stream.Write(bytes, 0, bytes.Length);
                
            }

        }
    }
}


using NotificationSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft;


namespace NotificationSystem.Common
{
    public class TxtHelper
    {
        public static List<Dictionary<string, List<Thread>>> GetFromTxt(string filepath)
        {
            List<Dictionary<string, List<Thread>>> oldData = new List<Dictionary<string, List<Thread>>>();
            // string filename = Environment.CurrentDirectory + "\\" + ForumDataTemp;
            try
            {
                if (!File.Exists(filepath)) return null;
                using (FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    StreamReader reader = new StreamReader(stream);
                    string content = reader.ReadToEnd();
                    oldData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, List<Thread>>>>(content);
                }
            }
            catch
            {
                File.Delete(filepath);
            }
            return oldData;
        }

        public static void SaveToTxt(List<NotifyTask> tasks)
        {
            foreach (var item in tasks)
            {
                string jsonThreads = Newtonsoft.Json.JsonConvert.SerializeObject(item.WriteModel);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonThreads);
                //string filepath = AppDomain.CurrentDomain.BaseDirectory  + "\\" + ForumDataTemp;
                if (File.Exists(item.TempFile))
                {
                    File.Delete(item.TempFile);
                }
                using (FileStream file = File.Open(item.TempFile, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    file.Write(bytes, 0, bytes.Length);
                }
            }

        }
        public static void SaveToTxt(NotifyTask task)
        {
            string jsonThreads = Newtonsoft.Json.JsonConvert.SerializeObject(task.WriteModel);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonThreads);
            //string filepath = AppDomain.CurrentDomain.BaseDirectory  + "\\" + ForumDataTemp;
            if (File.Exists(task.TempFile))
            {
                File.Delete(task.TempFile);
            }
            using (FileStream file = File.Open(task.TempFile, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                file.Write(bytes, 0, bytes.Length);
            }


        }
    }
}

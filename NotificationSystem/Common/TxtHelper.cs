﻿using NotificationSystem.Interface;
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
    public class TxtHelper : ITxt
    {
        public string ForumDataTemp = ConfigurationManager.AppSettings["ForumDataTemp"].ToString();
        public string WspDataTemp = ConfigurationManager.AppSettings["WspDataTemp"].ToString();

        public void SaveToTxt(List<Dictionary<string, List<Thread>>> writeData,string filepath)
        {
            string jsonThreads = Newtonsoft.Json.JsonConvert.SerializeObject(writeData);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonThreads);
            //string filepath = AppDomain.CurrentDomain.BaseDirectory  + "\\" + ForumDataTemp;
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (FileStream file = File.Open(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                file.Write(bytes, 0, bytes.Length);
            }

        }

        public List<Dictionary<string, List<Thread>>> GetFromTxt(string filepath)
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
            catch {
                File.Delete(filepath);
            }
            return oldData;
        }
    }
}

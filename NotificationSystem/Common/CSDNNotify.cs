using NotificationSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotificationSystem.Common
{
    public class CSDNNotify
    {
        public static NotifyTask GetModel()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "TaskConfig.xml";
            XDocument xdoc = XDocument.Load(fileName);

            var models = from c in xdoc.Descendants("Task")
                         where c.Attribute("id").Value == "CSDN"
                         select new
                         {
                             ID = c.Attribute("id").Value,
                             Name = c.Element("Name").Value,
                             Subject = c.Element("Subject").Value,
                             Links = c.Element("Link").Value,
                             From = c.Element("From").Value,
                             To = c.Element("To").Value,
                             Cc = c.Element("Cc").Value,
                             TempFile = AppDomain.CurrentDomain.BaseDirectory + c.Element("TempFile").Value
                         };


            NotifyTask task = new NotifyTask();
            var item = models.FirstOrDefault();
            task.ID = item.ID;
            task.Name = item.Name;
            task.Subject = item.Subject;
            task.Links = XMLHelper.GetListFromString(item.Links);
            task.EmailFrom = item.From;
            task.EmailTo = XMLHelper.GetListFromString(item.To);
            task.EmailCc = XMLHelper.GetListFromString(item.Cc);
            task.TempFile = item.TempFile;
            CSDNWorkFlow(task);
            //SaveListNofifyToTempFile(tasks);
            return task;

        }
        public static void CSDNWorkFlow(NotifyTask model)
        {
            //get new data from http
            GetNewData(model);
            //get old data from temp file
            model.oldModel = TxtHelper.GetFromTxt(model.TempFile);
            //compare old data and new data, save the difference to notify model
            XMLHelper.CompareData(model);
            //save the new data to temp file
            if (model.NotifyModel.Count != 0)
            {
                TxtHelper.SaveToTxt(model);
            }
            List<NotifyTask> tasks = new List<NotifyTask>();
            tasks.Add(model);
            EmailHelper.SendEmail(tasks);
        }

        public static void GetNewData(NotifyTask model)
        {
            List<Dictionary<string, List<Thread>>> WriteData = new List<Dictionary<string, List<Thread>>>();
            foreach (var url in model.Links)
            {
                List<Thread> threads = new List<Thread>();
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";

                try
                {
                    using (Stream stream = request.GetResponse().GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string ResponseBody = reader.ReadToEnd();
                        Dictionary<string, List<Thread>> list = new Dictionary<string, List<Thread>>();
                        if (url.Contains("zone"))
                        {
                            threads = MSDNHelper.ConverToXMLFromHtml(ResponseBody, Forum.CSDNZone);
                            list[Forum.CSDNZone.ToString()] = threads;
                        }
                        else
                        {
                            threads = MSDNHelper.ConverToXMLFromHtml(ResponseBody, Forum.CSDNUrlAsk);
                            list[Forum.CSDNUrlAsk.ToString()] = threads;
                        }

                        WriteData.Add(list);

                    }
                }
                catch (Exception e)
                {
                    LogHelper.LogMessage(e.Message);
                }
            }
            model.WriteModel = WriteData;
        }

    }
}

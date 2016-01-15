using NotificationSystem.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotificationSystem.Common
{
    public class XMLHelper
    {
        public static List<NotifyTask> GetModel()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "TaskConfig.xml";
            XDocument xdoc = XDocument.Load(fileName);
           
            var models = from task in xdoc.Descendants("Task") where task.Attribute("id").Value!="CSDN"
                         select new
                         {
                             ID = task.Attribute("id").Value,
                             Name = task.Element("Name").Value,
                             Subject = task.Element("Subject").Value,
                             Links = task.Element("Link").Value,
                             From = task.Element("From").Value,
                             To = task.Element("To").Value,
                             Cc = task.Element("Cc").Value,
                             TempFile = AppDomain.CurrentDomain.BaseDirectory + task.Element("TempFile").Value
                         };
            List<NotifyTask> tasks = new List<NotifyTask>();
            foreach (var item in models)
            {
                NotifyTask task = new NotifyTask();
                task.ID = item.ID;
                task.Name = item.Name;
                task.Subject = item.Subject;
                task.Links = GetListFromString(item.Links);
                task.EmailFrom = item.From;
                task.EmailTo = GetListFromString(item.To);
                task.EmailCc = GetListFromString(item.Cc);
                task.TempFile = item.TempFile;
                tasks.Add(task);
            }

            WorkFlow(tasks);
            //SaveListNofifyToTempFile(tasks);
            return tasks;

        }

        public static void WorkFlow(List<NotifyTask> tasks)
        {
            //first add new data to NotifyTask
            AddWriteModelToListNotify(tasks);
            //get old data from temp file and compare old data with new data, save the change to notify data
            GetOldModelAndCompare(tasks);
            //save the new data to temp file as old data
            SaveTheNewData(tasks);
            //send email
            EmailHelper.SendEmail(tasks);
        }

        public static void AddWriteModelToListNotify(List<NotifyTask> tasks)
        {
            MSDNHelper.GetWriteModel(tasks);
        }

        public static void SaveListNofifyToTempFile(List<NotifyTask> tasks)
        {
            TxtHelper.SaveToTxt(tasks);
        }

        public static void GetOldModelAndCompare(List<NotifyTask> tasks)
        {
            foreach (var task in tasks)
            {
                task.oldModel = TxtHelper.GetFromTxt(task.TempFile);
                CompareData(task);
            }
        }

        public static void CompareData(NotifyTask task)
        {

            try
            {
                if (task.oldModel == null)
                {
                    TxtHelper.SaveToTxt(task);
                    return;
                };
                foreach (var item in task.WriteModel)
                {
                    Dictionary<string, List<Thread>> notifyItem = new Dictionary<string, List<Thread>>();
                    List<Thread> notifyItemList = new List<Thread>();
                    List<Thread> threadcoming = item.FirstOrDefault().Value;
                    var threadstore = (task.oldModel.Where(m => m.FirstOrDefault().Key == item.FirstOrDefault().Key)).FirstOrDefault().FirstOrDefault().Value;
                    foreach (var i in threadcoming)
                    {
                        var findResult = threadstore.Where(m => m.ThreadTitle == i.ThreadTitle);

                        //find result so this thread has been found in old data, it is an old data too.
                        if (findResult.Count() > 0)
                        {
                            break;
                        }
                        else
                        {
                            notifyItemList.Add(i);

                        }
                    }
                    if (notifyItemList.Count > 0)
                    {
                        notifyItem[item.FirstOrDefault().Key] = notifyItemList;
                        task.NotifyModel.Add(notifyItem);
                    }

                }
            }
            catch (Exception e)
            {
                LogHelper.LogMessage(e.Message);
            }
        }

        public static void SaveTheNewData(List<NotifyTask> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.NotifyModel.Count != 0)
                {
                    TxtHelper.SaveToTxt(task);
                }
            }
        }

        /// <summary>
        /// get list<string> from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> GetListFromString(string value)
        {
            List<string> list = new List<string>();
            try
            {
                string[] values = value.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    list.Add(values[i]);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }

        }


    }
}

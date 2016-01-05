using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NotificationSystem.Common;
using NotificationSystem.Model;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace NotificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            ThreadHandler handler = new ThreadHandler();
            if (handler.NotifiyData.Count == 0) return;
            SendEmail(handler.NotifiyData);
        }



        public static void SendEmail(List<Dictionary<string, List<Thread>>> threads)
        {

            string content = "";
            foreach (var item in threads)
            {
                switch (item.FirstOrDefault().Key)
                {
                    case "CSDNUrlAsk":
                        if (item.FirstOrDefault().Value.Count <= 0) break;
                        content += "New CSDN thread coming from CSDN ASK forum:<br>";
                        foreach (var thread in item.FirstOrDefault().Value)
                        {
                            content += thread.ThreadLink + "<br>";
                        }

                        break;
                    case "CSDNZone":
                        if (item.FirstOrDefault().Value.Count <= 0) break;
                        content += "New CSDN thread coming from CSDN Zone forum:<br>";                       
                        foreach (var thread in item.FirstOrDefault().Value)
                        {
                            content += thread.ThreadLink + "<br>";
                        }
                        break;
                    case "MSDNUrl":
                        if (item.FirstOrDefault().Value.Count <= 0) break;
                        content += "New MSDN thread coming:<br>";                       
                        foreach (var thread in item.FirstOrDefault().Value)
                        {
                            content += thread.ThreadLink + "<br>";
                        }
                        break;
                }
                //content += item.ThreadLink + "</br>";
            }
            EmailHelper emailHelper = new EmailHelper("Forum New Thread Coming", content);

            emailHelper.SendEmail();

        }

    }

}

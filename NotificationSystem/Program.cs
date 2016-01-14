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
            if (handler.NotifiyData.Count != 0)
            {
                SendEmail(handler.NotifiyData);
            }
            if (handler.NotifiyDataWsp.Count != 0)
            {
                SendEmailWsp(handler.NotifiyDataWsp);
            }
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
        public static void SendEmailWsp(List<Dictionary<string, List<Thread>>> threads)
        {

            string wspContent = "";
            string wfContent = "";
            foreach (var item in threads)
            {
                switch (item.FirstOrDefault().Key)
                {
                    case "wsp":
                        if (item.FirstOrDefault().Value.Count <= 0) break;
                        wspContent += "New WebsitePanel-Development thread coming from CSDN ASK forum:<br>";
                        foreach (var thread in item.FirstOrDefault().Value)
                        {
                            wspContent += thread.ThreadLink + "<br>";
                        }

                        break;
                    case "wspSupport":
                        if (item.FirstOrDefault().Value.Count <= 0) break;
                        wspContent += "New WebsitePanel-Support thread coming from CSDN Zone forum:<br>";
                        foreach (var thread in item.FirstOrDefault().Value)
                        {
                            wspContent += thread.ThreadLink + "<br>";
                        }
                        break;
                    case "wf":
                        if (item.FirstOrDefault().Value.Count <= 0) break;
                        wfContent += "New Windows Workflow Foundation  thread coming:<br>";
                        foreach (var thread in item.FirstOrDefault().Value)
                        {
                            wfContent += thread.ThreadLink + "<br>";
                        }
                        break;
                }
                //content += item.ThreadLink + "</br>";
            }
            if (wspContent != "")
            {
                EmailHelper emailHelper = new EmailHelper("Forum New Thread Coming", wspContent);
                emailHelper.SendEmailCustom(emailHelper.WspappEmailTos, emailHelper.WspappEmailCC);
            }
            if (wfContent != "")
            {
                EmailHelper emailHelper2 = new EmailHelper("Forum New Thread Coming", wfContent);
                emailHelper2.SendEmailCustom(emailHelper2.WfappEmailTos, emailHelper2.WfappEmailCC);
            }
        }
    }

}

using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NotificationSystem.Model;
using HtmlAgilityPack;

namespace NotificationSystem.Common
{
    public class MSDNHelper
    {
        public static void GetWriteModel(List<NotifyTask> models)
        {
            string driverFolder = ConfigurationManager.AppSettings["driveFolder"].ToString();
            IWebDriver driver = new InternetExplorerDriver(driverFolder);
            driver.CurrentWindowHandle.Min();
          
            foreach (var model in models)
            {
                int i = 0;
                List<string> contents = new List<string>();               
                foreach (var link in model.Links)
                {
                    driver.Navigate().GoToUrl(link);
                    string Html = driver.FindElement(By.Id("threadList")).GetAttribute("innerHTML");
                    List<Thread> threads = new List<Thread>();
                    threads = ConverToXMLFromHtml(Html, Forum.MSDNUrl);
                    Dictionary<string, List<Thread>> dic = new Dictionary<string, List<Thread>>();
                    dic[model.Name+i.ToString()] = threads;
                    model.WriteModel.Add(dic);
                    i++;
                    contents.Add(Html);
                }
             
                model.HtmlFromLinks = contents;
            }
            driver.Close();
            driver.Quit();

        }

        /// <summary>
        /// convert xml to hreads
        /// </summary>
        /// <param name="html"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static List<Thread> ConverToXMLFromHtml(string html, Forum root)
        {
            List<Thread> threads = new List<Thread>();
            HtmlDocument hdoc = new HtmlDocument();
            hdoc.LoadHtml(html);

            switch (root)
            {
                case Forum.CSDNZone:
                    var result = (from h3 in hdoc.DocumentNode.Descendants("h3") orderby h3.Line ascending select h3).Take(10);
                    foreach (var item in result)
                    {
                        Thread thread = new Thread();
                        thread.ThreadLink = item.InnerHtml;
                        thread.ThreadTitle = item.InnerText;
                        thread.Line = item.Line;
                        threads.Add(thread);
                    }
                    break;
                case Forum.CSDNUrlAsk:
                    var resultAsk = (from div in hdoc.DocumentNode.Descendants("div")
                                     where div.Attributes.Contains("class") && div.Attributes["class"].Value.Contains("questions_detail_con")
                                     orderby div.Line ascending
                                     select div).Take(10);
                    foreach (var item in resultAsk)
                    {
                        Thread thread = new Thread();
                        thread.ThreadLink = item.InnerHtml.Substring(item.InnerHtml.IndexOf("<dt>") + 4, (item.InnerHtml.LastIndexOf("</dt>") - item.InnerHtml.IndexOf("<dt>") + 4));
                        thread.ThreadTitle = item.InnerText;
                        thread.Line = item.Line;
                        threads.Add(thread);
                    }
                    break;

                case Forum.MSDNUrl:
                    var resultMSDN = (from h3 in hdoc.DocumentNode.Descendants("h3") orderby h3.Line ascending select h3).Take(10);
                    foreach (var item in resultMSDN)
                    {
                        Thread thread = new Thread();
                        thread.ThreadLink = item.InnerHtml;
                        thread.ThreadTitle = item.InnerText;
                        thread.Line = item.Line;
                        threads.Add(thread);
                    }
                    break;
            }

            return threads;

        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.IO;
using System.Xml;
using HtmlAgilityPack;
using NotificationSystem.Model;


namespace NotificationSystem.Common
{
    public class ThreadHandler
    {
        public string MSDNUrl = System.Web.HttpUtility.UrlDecode(ConfigurationManager.AppSettings["MSDNUrl"].ToString());
        public string CSDNUrlAsk = System.Web.HttpUtility.UrlDecode(ConfigurationManager.AppSettings["CSDNUrlAsk"].ToString());
        public string CSDNZone = System.Web.HttpUtility.UrlDecode(ConfigurationManager.AppSettings["CSDNZone"].ToString());
        public string ResponseBody { get; set; }

        TxtHelper txtHelper = new TxtHelper();
        public List<Dictionary<string, List<Thread>>> writeTxtData = new List<Dictionary<string, List<Thread>>>();
        public List<Thread> Notify = new List<Thread>();
        public List<Dictionary<string, List<Thread>>> NotifiyData = new List<Dictionary<string, List<Thread>>>();

        public ThreadHandler()
        {
            GetThreadsFromCSDNUrlAskUrl(CSDNUrlAsk, Forum.CSDNUrlAsk, 0);
            GetThreadsFromCSDNUrlAskUrl(CSDNZone, Forum.CSDNZone, 1);
            GetThreadsFromMSDN(Forum.MSDNUrl, 2);
            List<Dictionary<string, List<Thread>>> oldData = txtHelper.GetFromTxt();

            CompareData(writeTxtData, oldData);

            if (NotifiyData.Count != 0)
            {

                txtHelper.SaveToTxt(writeTxtData);
            }
        }

        public void CompareData(List<Dictionary<string, List<Thread>>> newData, List<Dictionary<string, List<Thread>>> oldData)
        {
           
            try
            {
                if (oldData == null) { txtHelper.SaveToTxt(writeTxtData); return; };
                foreach (var item in newData)
                {
                    Dictionary<string, List<Thread>> notifyItem = new Dictionary<string, List<Thread>>();
                    List<Thread> notifyItemList = new List<Thread>();
                    List<Thread> threadcoming = item.FirstOrDefault().Value;
                    var threadstore = (oldData.Where(m => m.FirstOrDefault().Key == item.FirstOrDefault().Key)).FirstOrDefault().FirstOrDefault().Value;
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
                    notifyItem[item.FirstOrDefault().Key] = notifyItemList;
                    NotifiyData.Add(notifyItem);
                }
            }
            catch (Exception e)
            {
                LogHelper.LogMessage(e.Message);
            }
        }
        public void GetThreadsFromMSDN(Forum root, int priority)
        {
            List<Thread> threads = new List<Thread>();
            string html = MSDNHelper.GetThreadBlockHTML();
            threads = ConverToXMLFromHtml(html, Forum.MSDNUrl);
            Dictionary<string, List<Thread>> list = new Dictionary<string, List<Thread>>();
            list[root.ToString()] = threads;
            writeTxtData.Add(list);
        }

        public void GetThreadsFromCSDNUrlAskUrl(string url, Forum root, int priority)
        {
            List<Thread> threads = new List<Thread>();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";

            try
            {
                using (Stream stream = request.GetResponse().GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    ResponseBody = reader.ReadToEnd();
                    threads = ConverToXMLFromHtml(ResponseBody, root);
                    Dictionary<string, List<Thread>> list = new Dictionary<string, List<Thread>>();
                    list[root.ToString()] = threads;
                    writeTxtData.Add(list);

                }
            }
            catch (Exception e)
            {
                LogHelper.LogMessage(e.Message);
            }


        }

        private List<Thread> ConverToXMLFromHtml(string html, Forum root)
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

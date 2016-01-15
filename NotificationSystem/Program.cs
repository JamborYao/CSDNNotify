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
            XMLHelper.GetModel();
            CSDNNotify.GetModel();

        }

    }

}

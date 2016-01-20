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

            //MailMessage mail = new MailMessage();
            ////SmtpClient SmtpServer = new SmtpClient("cloudmail.microsoft.com");
            //SmtpClient SmtpServer = new SmtpClient("smtp.live.com",587);
            //mail.From = new MailAddress("CSDN-Forums@hotmail.com");


            //    mail.To.Add("v-jayao@microsoft.com");


            //mail.Subject = "test";
            //mail.IsBodyHtml = true;

            //string content = "New thread coming from " + "task.Name" + ":<br>";

            //mail.Body = content;
            ////Attachment attachment = new Attachment(filename);
            ////mail.Attachments.Add(attachment);
            ////SmtpServer.Port = 25;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("CSDN-Forums@hotmail.com", "Change!89");
            ////SmtpServer.UseDefaultCredentials = false;
            //SmtpServer.EnableSsl = true;

            //SmtpServer.Send(mail);


        }

    }

}

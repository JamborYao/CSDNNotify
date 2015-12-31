using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Common
{
    public class EmailHelper
    {
        public string appEmailFrom = ConfigurationManager.AppSettings["EmailFrom"].ToString();
        public string appEmailTos = ConfigurationManager.AppSettings["EmailTos"].ToString();
        public string appEmailCC = ConfigurationManager.AppSettings["EmailCC"].ToString();
        string EmailFrom;
        List<String> EmailTos=new List<string>();
        string Subject;
        string Content;
        string SmtpServer = "cloudmail.microsoft.com";
        string EmailFromPwd = "Change!89";

       

        public EmailHelper(string subject, string content)
        {
            this.Subject = subject;
            this.Content = content;
        }

        public void SendEmail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(this.SmtpServer);
                if (this.EmailTos == null) return;
                mail.From = new MailAddress(appEmailFrom);
                string[] tos = appEmailTos.Split(',');
                string[] CCs = appEmailCC.Split(',');
                for (int i = 0; i < tos.Length; i++)
                {
                    mail.To.Add(tos[i]);
                }
                for (int i = 0; i < CCs.Length; i++)
                {
                    mail.CC.Add(CCs[i]);
                }
                mail.Subject = this.Subject;
                mail.IsBodyHtml = true;
                mail.Body = this.Content;
                //Attachment attachment = new Attachment(filename);
                //mail.Attachments.Add(attachment);
                //SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential(this.EmailFrom, this.EmailFromPwd);
                //SmtpServer.UseDefaultCredentials = true;
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch {

            }
        }
    }
}

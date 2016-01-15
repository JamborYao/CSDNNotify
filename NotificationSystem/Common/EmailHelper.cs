using NotificationSystem.Model;
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
        public EmailHelper(string subject, string content)
        {

        }


        public static void SendEmail(List<NotifyTask> tasks)
        {
            try
            {
                foreach (var task in tasks)
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("cloudmail.microsoft.com");
                    if (task.EmailTo.Count == 0) return;
                    mail.From = new MailAddress(task.EmailFrom);

                    foreach (var to in task.EmailTo)
                    {
                        mail.To.Add(to);
                    }
                    foreach (var cc in task.EmailCc)
                    {
                        mail.CC.Add(cc);
                    }
                    mail.Subject = task.Subject;
                    mail.IsBodyHtml = true;

                    string content = "New thread coming from " + task.Name + ":<br>";
                    foreach (var lists in task.NotifyModel)
                    {
                        foreach (var thread in lists)
                        {
                            List<Thread> threads = thread.Value;
                            foreach (var item in threads)
                            {
                                content += item.ThreadLink + "<br>";
                            }
                        }
                    }
                    mail.Body = content;
                    //Attachment attachment = new Attachment(filename);
                    //mail.Attachments.Add(attachment);
                    //SmtpServer.Port = 25;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(task.EmailFrom, "Change!89");
                    //SmtpServer.UseDefaultCredentials = true;
                    SmtpServer.EnableSsl = true;
                    if (task.NotifyModel.Count != 0)
                    {
                        SmtpServer.Send(mail);
                    }

                }
            }
            catch (Exception e)
            {
                LogHelper.LogMessage(e.Message);
            }
        }
    }
}

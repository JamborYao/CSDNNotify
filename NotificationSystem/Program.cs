using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NotificationSystem.Common;
using NotificationSystem.Model;

namespace NotificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            HistoryThread handler = new HistoryThread();
            if (handler.Notify.Count == 0) return;
            SendEmail(handler.Notify);


        }

        public static void SendEmail(List<Thread> threads)
        {
            List<string> emailTos = new List<string>();
           
            string content = "";
            foreach (var item in threads)
            {
                content += item.ThreadLink + "</br>";
            }
            EmailHelper emailHelper = new EmailHelper("Forum New Thread Coming", content);

            emailHelper.SendEmail();

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Model
{
    public class NotifyTask
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public List<string> Links { get; set; }
        public string EmailFrom { get; set; }
        public List<string> EmailTo { get; set; }
        public List<string> EmailCc { get; set; }
        public string TempFile { get; set; }
        public List<string> HtmlFromLinks { get; set; }
        public List<Dictionary<string, List<Thread>>> WriteModel = new List<Dictionary<string, List<Thread>>>();
        public List<Dictionary<string, List<Thread>>> oldModel = new List<Dictionary<string, List<Thread>>>();
        public List<Dictionary<string, List<Thread>>> NotifyModel = new List<Dictionary<string, List<Thread>>>();

    }
   
}

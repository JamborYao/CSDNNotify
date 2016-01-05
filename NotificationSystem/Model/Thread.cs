using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NotificationSystem.Common;

namespace NotificationSystem.Model
{
    [Serializable]
    public class Thread
    {
        public string ThreadTitle;

        public string ThreadLink;

        public int Line;
    }
    [Serializable]
    public class Threads
    {
        public Dictionary<string, List<Thread>> AllThreads { get; set; }
    }

    
}

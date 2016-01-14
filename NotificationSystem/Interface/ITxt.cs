using NotificationSystem.Common;
using NotificationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Interface
{
    interface ITxt
    {
        void SaveToTxt(List<Dictionary<string, List<Thread>>> writeData,string filepath);
        List<Dictionary<string, List<Thread>>> GetFromTxt(string filepath);
    }
}

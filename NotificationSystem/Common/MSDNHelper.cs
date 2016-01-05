using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NotificationSystem.Common
{
    public class MSDNHelper
    {
        public static string GetThreadBlockHTML()
        {
            string driverFolder = ConfigurationManager.AppSettings["driveFolder"].ToString();
            IWebDriver driver = new InternetExplorerDriver(driverFolder);
            driver.Navigate().GoToUrl("https://social.msdn.microsoft.com/Forums/zh-CN/home?forum=windowsazurezhchs&filter=alltypes&sort=firstpostdesc");
            driver.CurrentWindowHandle.Min();
            var msdnHtml = driver.FindElement(By.Id("threadList")).GetAttribute("innerHTML");
           
            driver.Close();
            driver.Quit();
            return msdnHtml;
        }
    }
}

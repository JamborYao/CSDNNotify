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
        
        public static void GetThreadBlockHTML(ref string msdnHtml,ref string wspHtml,ref string wspSupportHtml,ref string wfHtml)
        {
            string driverFolder = ConfigurationManager.AppSettings["driveFolder"].ToString();
            IWebDriver driver = new InternetExplorerDriver(driverFolder);
            driver.CurrentWindowHandle.Min();
            driver.Navigate().GoToUrl("https://social.msdn.microsoft.com/Forums/zh-CN/home?forum=windowsazurezhchs&filter=alltypes&sort=firstpostdesc");
            
            msdnHtml = driver.FindElement(By.Id("threadList")).GetAttribute("innerHTML");

            driver.Navigate().GoToUrl("https://social.msdn.microsoft.com/Forums/en-US/home?forum=wspdev&filter=alltypes&sort=firstpostdesc");
            wspHtml = driver.FindElement(By.Id("threadList")).GetAttribute("innerHTML");

            driver.Navigate().GoToUrl("https://social.msdn.microsoft.com/Forums/en-US/home?forum=wspsupport&filter=alltypes&sort=firstpostdesc");
            wspSupportHtml = driver.FindElement(By.Id("threadList")).GetAttribute("innerHTML");

            driver.Navigate().GoToUrl("https://social.msdn.microsoft.com/Forums/en-US/home?forum=wfprerelease&filter=alltypes&sort=firstpostdesc ");
            wfHtml = driver.FindElement(By.Id("threadList")).GetAttribute("innerHTML");

            driver.Close();
            driver.Quit();
           // return msdnHtml;
        }

    }
}

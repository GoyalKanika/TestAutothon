using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace Framework.SeleniumDriver
{
    public class DriverFactory
    {
       // public  IWebDriver driver = null;
        public static ThreadLocal<object> driver = new ThreadLocal<Object>();
        public static ChromeOptions chromeOptions = new ChromeOptions();
        public static string BrowserType;
        public static string url;
        public static string path;
        public static string RunOnDocker;
        public static string WebDriverUrl;

        /// <summary>
        /// Creates a webdriver instance and loads the application
        /// </summary>
        /// <param name="URL">Application URL</param>
        /// <returns></returns>
        public static IWebDriver GetDriver(string URL="")
        {
            if (driver.Value == null)
            {
                if (URL!="")
                {
                    DriverFactory.url=URL;
                }
                if (url == "")
                {
                    BaseClass.Reporter.Report("Specify URL in appconfig", "fail", false);
                    BaseClass.ExitTest(true);
                }
                switch (BrowserType.ToLower())
                {
                    case "chrome":
                        StartChrome();
                        break;
                    case "firefox":
                        StartFF();
                        break;
                    case "edge":
                        StartEdge();
                        break;
                    default : BaseClass.Reporter.Report("Browser Type is not valid ", "fail", false);
                        BaseClass.ExitTest();
                        break;
                 }


            }
            return (IWebDriver)driver.Value;
        }


        public static void StartChrome()
        {

            if (driver.Value == null)
            {
                if (RunOnDocker.ToLower() == "false")
                {
                    try
                    {

                       IWebDriver Chromedriver = new Driver(new ChromeDriver(path, chromeOptions)).GetDriver();
                        BaseClass.Reporter.Report("Browser opened successfully", "info", false);
                        Chromedriver.Url = url; 
                        driver.Value = Chromedriver;
                        BaseClass.Reporter.Report("Navigating to url : " + url, "Pass", true);
                    }

                    catch (Exception e)
                    {
                        BaseClass.Reporter.Report("Exception reported : <br/> " + e.Message, "Fail", false);
                        BaseClass.ExitTest();
                    }
                }
                else
                {
                    try
                    {

                        IWebDriver Chromedriver = new RemoteWebDriver(new Uri(WebDriverUrl), chromeOptions);
                        BaseClass.Reporter.Report("Browser opened successfully", "info", false);
                        Chromedriver.Url = url;
                        driver.Value = Chromedriver;
                        BaseClass.Reporter.Report("Navigating to url : " + url, "Pass", true);
                    }

                    catch(Exception e)
                    {
                        BaseClass.Reporter.Report("Exception reported : <br/> " + e.Message, "Fail", false);
                        BaseClass.ExitTest();
                    }
                }
            }
        }


        public static void StartFF()
        {
           // throw (new NotImplementedException());



            if (driver.Value == null)
            {
                if (RunOnDocker.ToLower() == "false")
                {
                    try
                    {

                        IWebDriver FirefoxDriver = new Driver(new FirefoxDriver()).GetDriver();
                        FirefoxDriver.Manage().Window.Maximize();
                        BaseClass.Reporter.Report("Firefox Browser opened successfully", "info", false);
                        FirefoxDriver.Url = url;
                        driver.Value=FirefoxDriver;
                        BaseClass.Reporter.Report("Navigating to url : " + url, "Pass", true);
                    }

                    catch (Exception e)
                    {
                        BaseClass.Reporter.Report("Exception reported : <br/> " + e.Message, "Fail", false);
                        BaseClass.ExitTest();
                    }
                }
                else
                {
                    try
                    {
                        var options = new FirefoxOptions();
                        IWebDriver FirefoxDriver = new RemoteWebDriver(new Uri(WebDriverUrl), options);
                        BaseClass.Reporter.Report("Browser opened successfully", "info", false);
                        FirefoxDriver.Url = url;
                        driver.Value = FirefoxDriver;
                        BaseClass.Reporter.Report("Navigating to url : " + url, "Pass", true);
                    }

                    catch (Exception e)
                    {
                        BaseClass.Reporter.Report("Exception reported : <br/> " + e.Message, "Fail", false);
                        BaseClass.ExitTest();
                    }

                }
            }
        }

        public static void StartEdge()
        {
            throw (new NotImplementedException());

        }
    }

   
}

using Framework.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Framework.SeleniumDriver
{
    public class ParallelDriver
    {
      public static IWebDriver driver { get; set; }

        public static  void LaunchParallelDriver(string url)
        {
                try
                {
                    driver = new Driver(new ChromeDriver(DriverFactory.path, DriverFactory.chromeOptions)).GetDriver();
                    BaseClass.Reporter.Report("Browser opened successfully", "info", false);
                    driver.Url = url;
                    BaseClass.Reporter.Report("Navigating to url : " + url, "Pass", true);
                }

                catch (Exception e)
                {
                    BaseClass.Reporter.Report("Exception reported : <br/> " + e.Message, "Fail", false);
                    BaseClass.ExitTest();
                }  
        }

        public static void SwapDrivers()
        {
            IWebDriver staging = driver;
            driver = DriverFactory.GetDriver();
            DriverFactory.driver.Value = staging;
        }
    }
}

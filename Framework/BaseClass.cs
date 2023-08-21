using Framework.Reporting;
using Framework.SeleniumDriver;
using Framework.Utilities;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Framework
{
    public class BaseClass
    {
        private string testname;
        public static Reporter Reporter;
        public static Connection Connection;
        private static string previoustest = "";
        public static string FilePath;
        public BaseClass()
        {

        }
        public BaseClass(string testname)
        {
            this.testname = testname;
        }

        public static void KillDrivers()
        {
            if (DriverFactory.RunOnDocker.ToLower() != "true")
            {
                if (DriverFactory.BrowserType.ToLower() == "chrome")
                {
                    var chromeDriverProcesses = Process.GetProcesses().Where(pr => pr.ProcessName == "chromedriver"); // without '.exe'
                    foreach (var process in chromeDriverProcesses)
                    {
                        process.Kill();
                    }
                    var chromeProcesses = Process.GetProcesses().Where(pr => pr.ProcessName == "chrome"); // without '.exe'
                    foreach (var process in chromeProcesses)
                    {
                        process.Kill();
                    }
                }

                if (DriverFactory.BrowserType.ToLower() == "firefox")
                {
                    var firefoxDriverProcesses = Process.GetProcesses().Where(pr => pr.ProcessName == "geckodriver"); // without '.exe'
                    foreach (var process in firefoxDriverProcesses)
                    {
                        process.Kill();
                    }
                    var firefoxProcesses = Process.GetProcesses().Where(pr => pr.ProcessName == "firefox"); // without '.exe'
                    foreach (var process in firefoxProcesses)
                    {
                        process.Kill();
                    }
                }
            }
        }

        public void SetUp()
        {
            ReporterSetup();
           
        }
        public void SetDownloadPath(string downloadpath)
        {
            Constant.downloadpath = downloadpath + @"\" + Constant.CurrentTestName;
        }
        public Reporter ReporterSetup()
        {
           // Constant = new Constant();
            //Constant.SetConstant(Constant);
            Constant.CurrentTestName = this.testname;
            Constant.CurrentTest = this.testname;
            Constant.RunLocally = true;

            if (Constant.Resultpath == null)
            {
                Constant.Resultpath = @".\Results\Run_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
                Directory.CreateDirectory(Constant.Resultpath);
            }
            Constant.Screenshotpath = Constant.Resultpath + @"\" + "Screenshots" + @"\" + this.testname;
            Directory.CreateDirectory(Constant.Screenshotpath);
            Reporter = new Reporter();
            //_Reporter = Reporter;
            Reporter.InitializeReporting();
            return Reporter;
        }

        //public static string  getAppConfigValue(string key)
        //{
        //    key = string.Format("{0}", key);
        //    return ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings[key].Value;
        //}

        /// <summary>
        /// Launches the browser and loads the application 
        /// </summary>
        public static void LaunchApplication(string url="")
        {
            if (DriverFactory.RunOnDocker.ToLower() == "false")
            {
                BaseClass.KillDrivers();
            }
            DriverFactory.GetDriver(url);         
        }

      
        public void CloseBrowser()
        {
            Quit();
        }

        public static void TearDownReport()
        {
            Reporter.extent.Flush();
        }

        public static void TearDown()
        {
            TearDownReport();
            ExitTest();
        }
        
        public static void ExitTest()
        {
            Quit();
            BaseClass.KillDrivers();
            Reporter.extent.Flush();
            
        }

        public static void ExitTest(bool hasfailure)
        {
            if (hasfailure)
            {
                Quit();
                Reporter.Report("Execution stopped due to error", "Fail", false);
                
                Reporter.extent.Flush();
                if (previoustest == "")
                {
                    Constant.failed = Constant.failed + 1;
                    previoustest = Constant.CurrentTest;
                }
                else if (!previoustest.ToLower().Equals(Constant.CurrentTest.ToLower()))
                {
                    Constant.failed = Constant.failed + 1;
                    previoustest = Constant.CurrentTest;
                }
                BaseClass.KillDrivers();
                throw new Exception("Execution stopped due to error");
            }
        }

        public static void ExitTest(string failurereason)
        {
           
            Quit();
            BaseClass.KillDrivers();
            Reporter.extent.Flush();
            Assert.True(false, failurereason);
        }

        public static void ExitTest(By by)
        {
            if (DriverFactory.GetDriver() != null)
            {
                Reporter.Report("Object Not Found : " + by, "fail", true);
                Quit();
                Reporter.Report("Exiting test", "info", false);
                Reporter = null;
            }
            BaseClass.KillDrivers();
            Reporter.extent.Flush();
            Assert.True(false, "Object Not Found : " + by);

        }


        public static string GetData(string name)
        {
            if(Constant.DataSource.ToLower() == "excel")
            {
                return Connection.GetData(name);
            }
            else if(Constant.DataSource.ToLower() == "xml")
            {
               return  XmlFunctions.GetData(name);
            }
            return "";
        }

      public static void Quit()
        {
            if (DriverFactory.driver.Value != null)
            {
                DriverFactory.GetDriver().Quit();
                DriverFactory.driver.Value = null;
                Reporter.Report("Browser closed successfully", "info", false);
            }

            if (ParallelDriver.driver != null)
            {
                ParallelDriver.driver.Quit();
                ParallelDriver.driver= null;
                Reporter.Report("Parallel Browser Closed Sucessfully", "info", false);
            }
        }
    }
}
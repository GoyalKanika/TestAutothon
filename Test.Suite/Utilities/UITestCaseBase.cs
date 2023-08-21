using Framework;
using Framework.SeleniumDriver;
using Framework.Utilities;
using System;
using System.IO;
using System.Reflection;

namespace Test.Suite.Utilities
{
    public class UITestCaseBase : IDisposable
    {
       

        protected UITestCaseBase()
        {
            #region Pre-requisite
            BaseClass baseclass = new BaseClass(this.GetType().Name);
            baseclass.SetUp();
            

            DriverFactory.url = AppConfig.getAppConfigValue("URL");
            DriverFactory.path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DriverFactory.BrowserType = AppConfig.getAppConfigValue("BrowserType");
            DriverFactory.RunOnDocker = AppConfig.getAppConfigValue("RunOnDocker");
            if(DriverFactory.BrowserType.ToLower() == "chrome")
            {
                InitialiseChromeOptions();
            }
            Constant.DataSource = AppConfig.getAppConfigValue("DataSource");
            if(Constant.DataSource.ToLower()=="excel")
            {
                BaseClass.FilePath = AppConfig.getAppConfigValue("FilePath");
                Constant.Iterations = new Connection().OpenConnection();
            }
            if(DriverFactory.RunOnDocker.ToLower() =="true")
            {
                DriverFactory.WebDriverUrl = AppConfig.getAppConfigValue("RemoteWebDriverUrl");
            }
            #endregion
        }
        public void Dispose()
        {
            #region CleanUp
            BaseClass.TearDown();
            #endregion
        }

        private void InitialiseChromeOptions()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string DownloadPath = path + AppConfig.getAppConfigValue("DownloadPath");
            string BrowserType = AppConfig.getAppConfigValue("BrowserType");

            if (AppConfig.getAppConfigValue("RunLocation").ToLower() == "pipeline")
            {
                DriverFactory.chromeOptions.AddArgument("--headless");
                DriverFactory.chromeOptions.AddArgument("--window-size=1920,1080");
            }
            else if (AppConfig.getAppConfigValue("RunTestHeadless").ToLower() == "true")
            {
                DriverFactory.chromeOptions.AddArgument("--headless");
                DriverFactory.chromeOptions.AddArgument("--window-size=1920,1080");
            }
            DriverFactory.chromeOptions.AddArgument("--start-maximized");
            DriverFactory.chromeOptions.AddUserProfilePreference("download.default_directory", Constant.downloadpath);
            DriverFactory.chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
            if (AppConfig.getAppConfigValue("UseBrowserCache").ToLower() == "true")
            {
                string userdir = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data";
                if (Directory.Exists(userdir))
                {
                    DriverFactory.chromeOptions.AddArgument("--user-data-dir=" + userdir);
                }
            }
            
        }

       


    }
}

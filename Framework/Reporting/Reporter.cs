using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Framework.SeleniumDriver;
using Framework.Utilities;
using OpenQA.Selenium;
using System;
using Xunit;

namespace Framework.Reporting
{
    public class Reporter 
    {
        public static AventStack.ExtentReports.ExtentReports extent;
        public ExtentTest test;
      
        public Reporter()
        {
           
        }

        //public void SetDriver(IWebDriver driver)
        //{
        //    this.driver = driver;
        //}

        public void InitializeReporting()
        {
            string resultpath = Constant.Projectbasepath + @"\Results\";            
            string resultfilename = Constant.Resultpath + @"\Report.html";
            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(resultfilename);
            //htmlReporter.AppendExisting = true;
            if (extent == null)
            {
                extent = new AventStack.ExtentReports.ExtentReports();
            }
            extent.AttachReporter(htmlReporter);
            test = extent.CreateTest(Constant.CurrentTestName, "Sample description");
        }

      

        public void Report(string description,string status,bool takescreenshot)
        {
            
            switch(status.ToLower()){
                case "pass":
                    {
                        if (takescreenshot) {
                            test.Pass(description, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenshot()).Build());
                        }                            
                        else
                            test.Pass(description);
                        break;
                    }
                    
                case "fail":
                    {
                        if (takescreenshot)
                        {
                            test.Fail(description, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenshot()).Build());
                        }
                        else
                            test.Fail(description);
                        break;
                    }
                case "info":
                    {
                        test.Info(description);
                        break;
                    }
            }
            extent.Flush();
     
        }

        public void Report(string description, string status, bool takescreenshot,bool exittest)
        {
            switch (status.ToLower())
            {
                case "pass":
                    {
                        if (takescreenshot)
                        {
                            test.Pass(description, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenshot()).Build());
                        }
                        else
                            test.Pass(description);
                        break;
                    }

                case "fail":
                    {
                        if (takescreenshot)
                        {
                            test.Fail(description, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenshot()).Build());
                        }
                        else
                            test.Fail(description);
                        if (exittest)
                        {
                            if (DriverFactory.GetDriver() != null)
                            {
                                DriverFactory.GetDriver().Quit();
                            }
                            Reporter.extent.Flush();
                            Assert.True(false, description);
                        }
                        break;
                    }
                case "info":
                    {
                        test.Info(description);
                        break;
                    }
            }
            extent.Flush();

        }

        public string TakeScreenshot()
        {
           //Constant.screenshotpath = Constant.projectbasepath + @"\Results\Screenshots";
            Screenshot screenshotFile = ((ITakesScreenshot)DriverFactory.GetDriver()).GetScreenshot();
            string imagepath = Constant.Screenshotpath + @"\" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss").Replace('/', '_').Replace(':', '_') + ".png";
            screenshotFile.SaveAsFile(imagepath);
            //driver = null;
            return System.AppContext.BaseDirectory + imagepath;
        }

    }
}

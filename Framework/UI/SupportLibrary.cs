using Framework.SeleniumDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using Xunit;

namespace Framework.UI
{
    public class SupportLibrary
    {

        private static WebDriverWait wait = null;
        private static IWebElement element = null;
        private static By by = null;
        private static ReadOnlyCollection<IWebElement> elements = null;

        /// <summary>
        /// Scrolls to the location of the element 
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if the element is not found. </param>
        /// <param name="timeout">Wait time in seconds</param>
        public static void ScrollIntoView(string locatorString, bool exitTestonFailure = false, double timeout = 5)
        {
            element = SupportLibrary.GetObject(locatorString, exitTestonFailure, timeout);
            IJavaScriptExecutor js = (IJavaScriptExecutor)DriverFactory.GetDriver();
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        /// <summary>
        /// Gets the IWebElement Object for the specified locator
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if the element is not found. </param>
        /// <param name="timeout">Wait time in seconds</param>
        /// <returns>This method returns the IWebElement Object for the specified locator </returns>
        public static IWebElement GetObject(string locatorString, bool exitTestonFailure = false, double timeout = 5, bool ScrollIntoView = true)
        {

            wait = new WebDriverWait(DriverFactory.GetDriver(), TimeSpan.FromSeconds(timeout));
            try
            {
                by = null;
                by = GetCustomLocator(locatorString);
                element = wait.Until<IWebElement>(d => d.FindElement(by));

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));

                if (ScrollIntoView)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)DriverFactory.GetDriver();
                    js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                }
            }
            catch (Exception e)
            {
                if (by == null)
                {
                    BaseClass.ExitTest("Invalid locator:" + locatorString + " Ensure the identifier is of the format '<<identifier>>=<<Element Locator>>'. " +
                        "Identifiers cab be Xpath, Id, Name, Tagname, Linktext, Partiallinktext ");
                    
                }
                if (!exitTestonFailure)
                {
                    BaseClass.ExitTest(by);
                }
                else
                {
                    return null;
                }
            }
            return element;
        }

        /// <summary>
        /// Gets the Collection of IWebElement Object for the specified locator
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if the element is not found. </param>
        /// <param name="timeout">Wait time in seconds</param>
        /// <returns>This method returns the Collection of IWebElement Object for the specified locator </returns>
        public static ReadOnlyCollection<IWebElement> GetObjects(string locatorString, bool exitTestonFailure = false, double timeout = 5)
        {
            wait = new WebDriverWait(DriverFactory.GetDriver(), TimeSpan.FromSeconds(timeout));
            try
            {
                by = null;
                by = GetCustomLocator(locatorString);
                elements = wait.Until<ReadOnlyCollection<IWebElement>>(d => d.FindElements(by));
            }
            catch (Exception e)
            {
                if (by == null)
                {
                    BaseClass.ExitTest("Invalid locator:" + locatorString + " Ensure the identifier is of the format '<<identifier>>=<<Element Locator>>'. " +
                                            "Identifiers cab be Xpath, Id, Name, Tagname, Linktext, Partiallinktext ");
                }
                if (!exitTestonFailure)
                {
                    BaseClass.ExitTest(by);
                    Assert.True(false, "Object not found");
                }
                else
                {
                    return elements;
                }
            }
            return elements;
        }
    
        /// <summary>
        /// Waits until the page loads
        /// </summary>
        /// <returns></returns>
        public static bool WaitForPageLoad()
        {
            var javaScriptExecutor = DriverFactory.GetDriver() as IJavaScriptExecutor;
            bool flag = false;
            var wait = new WebDriverWait(DriverFactory.GetDriver(), TimeSpan.FromSeconds(120));
            try
            {
                Func<IWebDriver, bool> readyCondition = webDriver => (bool)javaScriptExecutor.ExecuteScript("return (document.readyState == 'complete')");
                flag = wait.Until(readyCondition);
            }
            catch (InvalidOperationException)
            {
                wait.Until(wd => javaScriptExecutor.ExecuteScript("return document.readyState").ToString() == "complete");
            }
            return flag;
        }


        /// <summary>
        /// Waits for the element to be visible 
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if the element is not found. </param>
        /// <param name="timeout">Wait time in seconds</param>
        /// <returns></returns>
        public static void WaitForElementToBeVisible(string locatorString, double timeout = 5, bool exitTestonFailure = false)
        {
            bool IsVisible = false;
            wait = new WebDriverWait(DriverFactory.GetDriver(), TimeSpan.FromSeconds(timeout));
            by = null;
            by = GetCustomLocator(locatorString);
            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));

                element = wait.Until<IWebElement>(d => d.FindElement(by));
                IsVisible = true;
            }
            catch
            {
                if (by == null)
                {
                    BaseClass.ExitTest("Invalid locator:" + locatorString + " Ensure the identifier is of the format '<<identifier>>=<<Element Locator>>'. " +
                                            "Identifiers cab be Xpath, Id, Name, Tagname, Linktext, Partiallinktext ");
                }
                if (!exitTestonFailure)
                {
                    BaseClass.Reporter.Report("Element is not Visible even after time out", "fail", true);
                    BaseClass.ExitTest(by);
                }

            }
          //  return IsVisible;
        }

        /// <summary>
        /// Waits for the element to be clickable
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="timeout">Wait time in seconds</param>
        public static void WaitForElementToBeClickable(string locatorString, double timeout = 5, bool exitTestonFailure = false)
        {
            try
            {
                by = null;
                wait = new WebDriverWait(DriverFactory.GetDriver(), TimeSpan.FromSeconds(timeout));
                by = GetCustomLocator(locatorString);
                element = wait.Until<IWebElement>(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
            }
            catch (Exception e)
            {
                if (!exitTestonFailure)
                {
                    if (by == null)
                    {
                        BaseClass.ExitTest("Invalid locator:" + locatorString + " Ensure the identifier is of the format '<<identifier>>=<<Element Locator>>'. " +
                                                "Identifiers cab be Xpath, Id, Name, Tagname, Linktext, Partiallinktext ");
                    }
                    else
                    {
                        BaseClass.Reporter.Report("Element is not Visible even after time out", "fail", true);
                        BaseClass.ExitTest(by);
                    }
                }

            }

        }

        /// <summary>
        /// Waits for the element to be invisible
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="timeout">Wait time in seconds</param>
        public static void WaitForElementToBeInvisible(string locatorString, double timeout = 5)
        {
            wait = new WebDriverWait(DriverFactory.GetDriver(), TimeSpan.FromSeconds(timeout));
            try

            {
                by = null;
                by = GetCustomLocator(locatorString);
                var element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(by));
            }
            catch (Exception e)
            {
                if (by == null)
                {
                    BaseClass.ExitTest("Invalid locator:" + locatorString + " Ensure the identifier is of the format '<<identifier>>=<<Element Locator>>'. " +
                                            "Identifiers cab be Xpath, Id, Name, Tagname, Linktext, Partiallinktext ");
                }
                else
                {
                    BaseClass.ExitTest(by);
                    Assert.True(false, "Object not found");
                }
            }
           // return element;
        }

        /// <summary>
        /// Waits till the element exists
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="timeout">Wait time in seconds</param>
        public static void WaitForElementToExist(string locatorString, double timeout = 5)
        {
            wait = new WebDriverWait(DriverFactory.GetDriver(), TimeSpan.FromSeconds(timeout));
            try
            {
                by = null;
                by = GetCustomLocator(locatorString);
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
            }
            catch (Exception e)
            {
                if (by == null)
                {
                    BaseClass.ExitTest("Invalid locator:" + locatorString + " Ensure the identifier is of the format '<<identifier>>=<<Element Locator>>'. " +
                                            "Identifiers cab be Xpath, Id, Name, Tagname, Linktext, Partiallinktext ");
                }
                else
                {
                    BaseClass.ExitTest(by);
                    BaseClass.ExitTest(true);
                }
            }
            //return element;
        }

        /// <summary>
        /// Returns the Custom locator based on the element identifier
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <returns></returns>
        public static By GetCustomLocator(string locatorString)
        {
            try
            {
                string[] locator = locatorString.Split(new char[] { '=' }, 2);
                switch (locator[0].ToLower().Trim())
                {
                    case "id":
                        return By.Id(locator[1]);
                       
                    case "name":
                        return By.Name(locator[1]);

                    case "xpath":
                        return By.XPath(locator[1]);

                    case "tagname":
                        return By.TagName(locator[1]);

                    case "linktext":
                        return By.LinkText(locator[1]);

                    case "partiallinktext":
                        return By.PartialLinkText(locator[1]);

                    default:
                        throw new Exception("Incorrect object identification string - " + locator[0]);
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Incorrect object identification. Ensure the identifier is of the format '<<identifier>>=<<Element Locator>>' ", "fail", true);
                BaseClass.Reporter.Report("Identifiers cab be Xpath, Id, Name, Tagname, Linktext, Partiallinktext  ", "info", true);

                //throw new Exception("Incorrect object identification string - " + e);
                return null;
            }
        }
    }
}
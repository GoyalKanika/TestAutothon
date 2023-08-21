using AutoIt;
using Framework.SeleniumDriver;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Framework.UI
{
    public class CommonFunctions
    {
        static IWebElement element = null;
        static ReadOnlyCollection<IWebElement> elements = null;



        /// <summary>
        /// Clicks on the given element
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element to be used for reporting</param>
        /// <param name="useJavaScriptExec">This is an optional parameter to be used if we want to use js executor for click. Default value is false.</param>
        /// <param name="ignoreException">This is an optional parameter to ignore any excpetion . Default value is false .</param>
        ///  <param name="ScrollIntoView">Set to true if you want to scroll explicitly</param>
        public static void Click(string locatorString, string elementName, bool useJavaScriptExec = false, bool ignoreException = false, bool ScrollIntoView=true)
        {

            int i = 0;
            element = SupportLibrary.GetObject(locatorString,false,5,ScrollIntoView);
            if (ScrollIntoView)
            {
                SupportLibrary.ScrollIntoView(locatorString);
            }
            while (true)
            {
                try
                {
                    if (useJavaScriptExec)
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)DriverFactory.GetDriver();
                        js.ExecuteScript("arguments[0].click();", element);
                    }
                    else
                    {
                        element.Click();
                    }
                    if (elementName != "")
                    {
                        BaseClass.Reporter.Report("Clicked on " + elementName, "pass", true);
                    }
                    break;
                }
                catch (Exception e)
                {
                    if (!ignoreException)
                    {
                        if (i == 10)
                        {
                            if (elementName != "")
                            {
                                BaseClass.Reporter.Report("Exception occured on trying to click Element : " + elementName + " <br/> " + e.Message, "fail", true, true);
                            }
                            else
                            {
                                BaseClass.Reporter.Report("Exception occured on trying to click Element " + e.Message, "fail", true, true);
                            }

                        }
                        else
                        {
                            Thread.Sleep(1000);
                            i = i + 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Enters the text
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="text">Text to be enetered</param>
        /// <param name="elementName">Name of the element</param>
        /// <param name="useJavaScriptExec">Boolean value to specify if the click should be using js executor</param>
        public static void EnterText(string locatorString, string text, string elementName, bool useJavaScriptExec = false)
        {
            element = SupportLibrary.GetObject(locatorString);

            try
            {
                if (element.Displayed)
                {
                    if (useJavaScriptExec)
                    {
                        IJavaScriptExecutor jse = (IJavaScriptExecutor)DriverFactory.GetDriver();
                        jse.ExecuteScript("arguments[0].value='" + text + "';", element);
                    }
                    else
                    {
                        element.SendKeys(text);
                    }
                    if (text == "")
                    {
                        BaseClass.Reporter.Report(elementName + " was set as empty ", "pass", true);
                    }
                    else
                    {
                        BaseClass.Reporter.Report(elementName + " -  data entered '" + text + "'", "pass", true);
                    }
                }
                else
                {
                    BaseClass.Reporter.Report(elementName + " not displayed in the application", "fail", true, true);
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Error occured on trying to enter data to Element : " + elementName + " <br/> " + e.Message, "fail", true, true);
            }
        }

        /// <summary>
        /// Clears the text field and Enters the text
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="text">Text to be enetered</param>
        /// <param name="elementName">Name of the element</param>
        public static void ClearandEnterText(string locatorString, string text, string elementName)
        {
            element = SupportLibrary.GetObject(locatorString);

            try
            {
                if (element.Displayed)
                {

                    ClearText(locatorString, elementName);
                    element.SendKeys(text);
                    if (text == "")
                    {
                        BaseClass.Reporter.Report(elementName + " was set as empty ", "pass", true);
                    }
                    else
                    {
                        BaseClass.Reporter.Report(elementName + " -  data entered '" + text + "'", "pass", true);
                    }
                }
                else
                {
                    BaseClass.Reporter.Report(elementName + " not displayed in the application", "fail", true, true);
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Error occured on trying to enter data to Element : " + elementName + " <br/> " + e.Message, "fail", true, true);
            }
        }

        /// <summary>
        /// Clears the text from element
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element</param>
        public static void ClearText(string locatorString, string elementName)
        {
            element = SupportLibrary.GetObject(locatorString);

            try

            {
                if (element.Displayed)
                {
                    if (element.Text != "")
                    {
                        element.Clear();
                        BaseClass.Reporter.Report(elementName + " cleared ", "pass", true);
                    }
                    else if (element.GetAttribute("value") != "")
                    {
                        element.SendKeys(Keys.Control + "a");
                        element.SendKeys(Keys.Backspace);
                        BaseClass.Reporter.Report(elementName + "cleared ", "pass", true);
                    }
                }
                else
                {
                    BaseClass.Reporter.Report(elementName + " not displayed in the application", "fail", true, true);
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Error occured on trying to clear data of Element : " + elementName + " <br/> " + e.Message, "fail", true, true);
            }
        }

        /// <summary>
        /// Gets the Text of the element
        /// </summary>
        /// <param name="locatorString">Element Identifier</param>
        /// <returns></returns>
        public static string GetText(string locatorString)
        {
            string Text = null;
            element = SupportLibrary.GetObject(locatorString);
            Text = element.Text;
            return Text;
        }

        /// <summary>
        /// Checks if the element is displayed or not.
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if the element is not displayed. </param>

        /// <returns> This method returns true if element is displayed and false if element is not displayed</returns>
        public static bool IsElementDisplayed(string locatorString, string elementName, bool exitTestonFailure = false)
        {
            bool isDisplayed = false;
            element = SupportLibrary.GetObject(locatorString, !(exitTestonFailure));

            if (element == null)
            {
                return isDisplayed;
            }

            try
            {
                isDisplayed = element.Displayed;
                if (isDisplayed)
                {
                    BaseClass.Reporter.Report(elementName + " is displayed", "pass", true);
                }
                else
                {
                    if (exitTestonFailure)
                    {
                        BaseClass.Reporter.Report(elementName + " not displayed on the application", "fail", true, true);
                    }
                    else
                    {
                        BaseClass.Reporter.Report(elementName + " not displayed on the application", "info", true, true);
                    }
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Error occured on trying to display " + elementName + " <br/> " + e.Message, "fail", true, true);
            }
            return isDisplayed;
        }

        /// <summary>
        /// Checks if the element is displayed or not.
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if the element displayed. </param>
        /// <returns> This method returns true if element is not displayed and false if element is displayed</returns>
        public static bool IsElementNotDisplayed(string locatorString, string elementName, bool exitTestonFailure = false)
        {
            bool isDisplayed = false;
            int count = GetElementCount(locatorString, elementName);

            if (count > 0)
            {
                isDisplayed = true;
                BaseClass.Reporter.Report(elementName + " is displayed", "fail", true);
            }
            else
            {
                isDisplayed = false;
                BaseClass.Reporter.Report(elementName + " is not displayed", "pass", true);
            }
            return !isDisplayed;
        }

        /// <summary>
        /// Checks if the element is enabled or not
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if the element is not displayed.Default value is false. </param>
        /// <returns> This method returns true if element is enabled and false if element is not enabled</returns>
        public static bool IsElementEnabled(string locatorString, string elementName, bool exitTestonFailure = false)
        {
            bool isEnabled = false;
            element = SupportLibrary.GetObject(locatorString);

            try
            {
                isEnabled = element.Enabled;
                if (isEnabled)
                {
                    BaseClass.Reporter.Report(elementName + " is enabled", "pass", true);
                }
                else
                {
                    if (exitTestonFailure)
                    {
                        BaseClass.Reporter.Report(elementName + " not enabled mode in the application", "fail", true, true);
                    }
                    else
                    {
                        BaseClass.Reporter.Report(elementName + " not enabled mode in the application", "info", true, true);
                    }
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("MethodName: CommonFunctions.isElementEnabled - Error occured, <br/> " + e.Message, "fail", true, true);
            }
            return isEnabled;
        }

        /// <summary>
        /// Checks if the element is disabled or not
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if the element is not displayed.Default value is false. </param>
        /// <returns> This method returns true if element is disabled and false if element is not disabled</returns>
        public static bool IsElementDisabled(string locatorString, string elementName, bool exitTestonFailure = false)
        {
            bool isEnabled = false;
            element = SupportLibrary.GetObject(locatorString);

            try
            {
                isEnabled = element.Enabled;
                if (!isEnabled)
                {
                    BaseClass.Reporter.Report(elementName + " is disabled", "pass", true);
                }
                else
                {
                    if(exitTestonFailure)
                    {
                        BaseClass.Reporter.Report(elementName + "  enabled mode in the application", "fail", true, true);
                    }
                    else
                    {
                        BaseClass.Reporter.Report(elementName + "  enabled mode in the application", "info", true, true);
                    }
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("MethodName: CommonFunctions.isElementEnabled - Error occured, <br/> " + e.Message, "fail", true, true);
            }
            return !isEnabled;
        }

     

        /// <summary>
        /// Checks if the checkbox is enabled or not
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element</param>
        /// <returns>This method returns true if checkbox is checked else returns false</returns>
        public static bool IsCheckboxChecked(string locatorString, string elementName)
        {
            bool IsChecked = false;
            element = SupportLibrary.GetObject(locatorString);
            SupportLibrary.ScrollIntoView(locatorString);
            try
            {


                IsChecked = element.Selected;

            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Exception occured on trying to validate the Element : " + elementName + " <br/> " + e.Message, "fail", true, false);
            }

            return IsChecked;
        }

        /// <summary>
        /// Checks if the radio button is enabled or not
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element</param>
        /// <returns>This method returns true if radio button is enabled else returns false</returns>
        public static bool IsRadioButtonEnabled(string locatorString, string elementName)
        {
            bool IsEnabled = false;

            element = SupportLibrary.GetObject(locatorString);
            SupportLibrary.ScrollIntoView(locatorString);
            try
            {

                IsEnabled = element.Selected;

            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Exception occured on trying to access the Element : " + elementName + " <br/> " + e.Message, "fail", true, false);
            }

            return IsEnabled;
        }

        /// <summary>
        /// Selects the value from dropdown
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="option">Option to be selected</param>
        /// <param name="dropdownname">Name of the dropdown</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if match is not valid. Default value is false. </param> 
        /// <returns>This method returns true if the option is available else returns false</returns>
        public static bool SelectValueFromDropdown(string locatorString, string option, string dropdownname, bool exitTestonFailure = false)
        {
            bool found = false;
            element = SupportLibrary.GetObject(locatorString);

            try
            {
                SelectElement dropdown = new SelectElement(element);
                string textbefore = dropdown.AllSelectedOptions[0].Text;
                dropdown.SelectByText(option);
                string textafter = dropdown.AllSelectedOptions[0].Text;
                if (!textbefore.Equals(textafter))
                {
                    BaseClass.Reporter.Report(dropdownname + " dropdown value set to : " + option, "pass", true);
                    found = true;
                }
            }
            catch (Exception e)
            {
                if (!exitTestonFailure)
                {
                    BaseClass.Reporter.Report("Error occured while selecting value from dropdown <br/>" + e.Message, "fail", true);
                    BaseClass.ExitTest("Error occured : Dropdown value selection");
                }
            }
            return found;
        }


        /// <summary>
        /// Selects the value from dropdown
        /// </summary>
        /// <param name="options">option of tyoe IWebElement</param>
        /// <param name="text">text to be selected </param>
        /// <param name="dropdownname">Name of the dropdown</param>
        /// <param name="partialmatch">Boolean Value to specify if the match should be partial or exact</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if match is not valid. Default value is false. </param> 
        /// <returns>This method returns true if the option is available else returns false</returns>
        public static bool SelectValueFromDropdown(ReadOnlyCollection<IWebElement> options, string text, string dropdownname, bool partialmatch = false, bool exitTestonFailure = false)
        {
            bool found = false;
            if (partialmatch == true)
            {
                try
                {
                    foreach (IWebElement option in options)
                    {
                        if (option.Text.Trim().Contains(text) || text.Contains(option.Text))
                        {
                            option.Click();
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        BaseClass.Reporter.Report(dropdownname + " dropdown value set to : " + text, "pass", true);
                    }
                    else
                    {
                        if (!exitTestonFailure)
                        {
                            BaseClass.Reporter.Report(dropdownname + " dropdown does not have value : " + text, "fail", true, true);
                        }
                    }

                }
                catch (Exception e)
                {
                    BaseClass.Reporter.Report("Exception occured while selecting value from dropdown <br/>" + e.Message, "fail", true);
                    BaseClass.ExitTest("Exception occured : Dropdown value selection");
                }
            }
            else
            {
                try
                {
                    foreach (IWebElement option in options)
                    {
                        string temp = option.Text;
                        if (option.Text.Equals(text))
                        {
                            option.Click();
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        BaseClass.Reporter.Report(dropdownname + " dropdown value set to : " + text, "pass", true);
                    }
                    else
                    {
                        if (!exitTestonFailure)
                        {
                            BaseClass.Reporter.Report(dropdownname + " dropdown does not have value : " + text, "fail", true, true);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!exitTestonFailure)
                    {
                        BaseClass.Reporter.Report("Error occured while selecting value from dropdown <br/>" + e.Message, "fail", true);
                        BaseClass.ExitTest("Error occured : Dropdown value selection");
                    }
                }

            }
            return found;
        }


        public static void SelectValueFromDropdown(string locatorString, ReadOnlyCollection<IWebElement> options, string text, string dropdownname)
        {
            bool found = false;
            element = SupportLibrary.GetObject(locatorString);

            try
            {
                element.Click();
                foreach (IWebElement option in options)
                {
                    if (option.Text.Equals(text))
                    {
                        option.Click();
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    BaseClass.Reporter.Report(dropdownname + " dropdown value set to : " + text, "pass", true);
                }
                else
                {
                    BaseClass.Reporter.Report(dropdownname + " dropdown does not have value : " + text, "fail", true, true);
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Error occured while selecting value from dropdown <br/>" + e.Message, "fail", true);
                BaseClass.ExitTest("Error occured : Dropdown value selection");
            }
        }


        /// <summary>
        /// Gets the count of element with same locator
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element</param>
        /// <returns>This method returns the count of occurance of the element with the specified locator</returns>
        public static int GetElementCount(string locatorString, string elementName)
        {
            int count = 0;
            elements = SupportLibrary.GetObjects(locatorString);
            try
            {
                count = elements.Count;
            }

            catch (Exception e)
            {
                BaseClass.Reporter.Report("Exception occured on trying to access the Element : " + elementName + " <br/> " + e.Message, "fail", true, false);
            }

            return count;
        }

        /// <summary>
        /// Gets the specified attribute of the element
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="attributeName">Attribute name whose value needs to be fetched</param>
        /// <param name="elementName">Name of the element</param>
        /// <returns>This method gets the attribute value for the element </returns>
        public static string GetAttribute(string locatorString, string attributeName, string elementName)
        {
            element = SupportLibrary.GetObject(locatorString);
            string attribute = null;

            try
            {
                attribute = element.GetAttribute(attributeName);
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Exception occured on trying to access the Element : " + elementName + " <br/> " + e.Message, "fail", true, false);
            }
            return attribute;
        }

        /// <summary>
        /// Sets the value for the given element
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="value">Value to be set</param>
        /// <returns>This method returns true if value is set else returns false</returns>
        public static bool SetAttribute(string locatorString, string value)
        {
            element = SupportLibrary.GetObject(locatorString);

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)DriverFactory.GetDriver();
                js.ExecuteScript("arguments[0].value='" + value + "';", element);
                return true;
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Method : SetAttribute , Error occured on setting attribute value,<br/> " + e.Message, "fail", true);
                return false;
            }

        }


        /// <summary>
        /// Gets the specified CSS value of the specified property
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="propertyName">Name of the property whose value needs to be fetched</param>
        /// <param name="elementName">Name of the element</param>
        /// <returns>Returns the CSS value of specified propery  </returns>
        public static string GetCssValue(string locatorString, string propertyName, string elementName)
        {
            element = SupportLibrary.GetObject(locatorString);
            string CssValue = null;

            try
            {
                CssValue = element.GetCssValue(propertyName);
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Exception occured on trying to access the Element : " + elementName + " <br/> " + e.Message, "fail", true, false);
            }
            return CssValue;
        }

        /// <summary>
        /// Finds the text in a given string and verifies it against the actual text on the UI.
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element</param>
        /// <param name="alltext">Entire text as a string</param>
        /// <param name="delimiter">Delimiter to fetch the search text</param>
        /// <param name="ignorecase">Boolean value to specify if the match should be case sensitive or not</param>
        /// <returns>This method returns true if the text is present is valid and false if the text is not present </returns>
        public static bool FindandVerifyText(string locatorString, string elementName, string alltext, string delimiter, bool ignorecase)
        {
            bool flag = true;
            IsElementDisplayed(locatorString, elementName);
            element = SupportLibrary.GetObject(locatorString);
            if (alltext != "")
            {
                foreach (string text in alltext.Split(delimiter))
                {
                    if (ignorecase)
                    {
                        if (element.Text.ToLower().Contains(text.ToLower()))
                        {
                            BaseClass.Reporter.Report(elementName + " has text : " + text, "pass", true);
                        }
                        else
                        {
                            BaseClass.Reporter.Report(elementName + " does not have text : " + text, "fail", true);
                            flag = false;
                        }
                    }
                    else
                    {
                        if (element.Text.Contains(text))
                        {
                            BaseClass.Reporter.Report(elementName + " has text : " + text, "pass", true);
                        }
                        else
                        {
                            BaseClass.Reporter.Report(elementName + " does not have text : " + text, "fail", true);
                            flag = false;
                        }
                    }
                }
            }
            else
            {
                BaseClass.Reporter.Report("No Expected text found to verify", "fail", false);
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Verifies if the expected text from the string is same as text of the given element
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element</param>
        /// <param name="expectedtext">Expected text value</param>
        /// <param name="exactmatch">Boolean Value to specify if the match should be partial or exact </param>
        /// <param name="exitTestonFailure">If set to true the test will exit if match is not valid. Default value is false. </param> 
        /// <returns>This method returns true if the text is present and false if the text is not present </returns>
        public static bool VerifyText(string locatorString, string elementName, string expectedtext, bool exactmatch,bool exitTestonFailure = false)
        {
            bool flag = true;
            IsElementDisplayed(locatorString, elementName);
            element = SupportLibrary.GetObject(locatorString);
            if (elementName != "")
            {
                if (!exactmatch)
                {
                    if (expectedtext == "")
                    {
                        if (element.Text.Trim() == "")
                        {
                            BaseClass.Reporter.Report(elementName + " no has text", "pass", true);
                        }
                        else
                        {
                            BaseClass.Reporter.Report(elementName + " has text : " + element.Text + " <b>expected was element should have no text</b>", "fail", true);
                            flag = false;
                        }
                    }
                    else if (element.Text.ToLower().Contains(expectedtext.ToLower()) || expectedtext.ToLower().Contains(element.Text.ToLower()))
                    {
                        BaseClass.Reporter.Report(elementName + " has text : " + expectedtext, "pass", true);
                    }
                    else
                    {
                        BaseClass.Reporter.Report(elementName + " does not match expected text ", "fail", true);
                        BaseClass.Reporter.Report(elementName + " expected text : " + expectedtext, "info", true);
                        BaseClass.Reporter.Report(elementName + " actual text : " + element.Text, "info", true);
                        flag = false;
                    }
                }
                else
                {
                    if (element.Text.Equals(expectedtext))
                    {
                        BaseClass.Reporter.Report(elementName + " has text : " + expectedtext, "pass", true);
                    }
                    else
                    {
                        BaseClass.Reporter.Report(elementName + " does not match expected text ", "fail", true);
                        BaseClass.Reporter.Report(elementName + " expected text : " + expectedtext, "info", true);
                        BaseClass.Reporter.Report(elementName + " actual text : " + element.Text, "info", true);
                        flag = false;
                    }
                }
            }
            else
            {
                BaseClass.Reporter.Report("No Expected text found to verify", "fail", false);
                flag = false;
            }
            if(flag==false)
            {
                if(exitTestonFailure)
                {
                    BaseClass.Reporter.Report("Exiting the test case as the text is not matched", "fail", true, true);
                }
            }
            return flag;
        }

        /// <summary>
        /// Verifies if the expected text is same as text of the given element
        /// </summary>
        /// <param name="actualtext">Actual text</param>
        /// <param name="expectedtext">Expected text</param>
        /// <param name="exactmatch">Boolean Value to specify if the match should be partial or exact </param>
        /// <param name="elementName">Name of the element</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if match is not valid. Default value is false. </param> 
        /// <returns>This method returns true if actual text and expected text are same else returns false </returns>
        public static bool VerifyText(string actualtext, string expectedtext, bool exactmatch, string elementName, bool exitTestonFailure = false)
        {
            bool flag = true;
            if (actualtext == "" && expectedtext == "")
            {
                BaseClass.Reporter.Report(elementName + " has no text ", "pass", true);
            }
            else if (!exactmatch)
            {
                if (actualtext.ToLower().Contains(expectedtext.ToLower()) || expectedtext.ToLower().Contains(actualtext.ToLower()))
                {
                    BaseClass.Reporter.Report(elementName + " has expected text '" + actualtext + "'", "pass", true);
                }
                else
                {
                    BaseClass.Reporter.Report(elementName + " text does not match the expected text, Expected : " + expectedtext + " Actual : " + actualtext, "fail", true);
                    flag = false;
                }
            }
            else
            {
                if (actualtext.Equals(expectedtext))
                {
                    BaseClass.Reporter.Report(elementName + " has expected text '" + expectedtext + "'", "pass", true);
                }
                else
                {
                    BaseClass.Reporter.Report(elementName + " text does not match the expected text ", "fail", true);
                    BaseClass.Reporter.Report(elementName + " expected text : " + expectedtext, "info", true);
                    BaseClass.Reporter.Report(elementName + " actual text : " + actualtext, "info", true);
                    flag = false;
                }
            }
            if (flag == false)
            {
                if (exitTestonFailure)
                {
                    BaseClass.Reporter.Report("Exiting the test case as the text is not matched. Expected Text:" +expectedtext+"Actual Text:"+actualtext, "fail", true, true);
                   
                }
            }
            return flag;
        }

        /// <summary>
        /// Verifies if the expected text is not same as text of the given element
        /// </summary>
        /// <param name="actualtext">Actual text</param>
        /// <param name="expectedtext">Expected text</param>
        /// <param name="elementName">Name of the element</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if match is not valid. Default value is false. </param> 
        /// <returns>This method returns true if actual text and expected text are same else returns false </returns>
        public static bool VerifyTextNotEqual(string actualtext, string expectedtext, string elementName, bool exitTestonFailure = false)
        {
            bool flag = true;
            if (actualtext == "" && expectedtext == "")
            {
                BaseClass.Reporter.Report(elementName + " has no text ", "pass", true);
            }
            else
            {
                if (actualtext.Equals(expectedtext))
                {
                    BaseClass.Reporter.Report(elementName + " text matches the expected text ", "fail", true);
                    BaseClass.Reporter.Report(elementName + " expected text : " + expectedtext, "info", true);
                    BaseClass.Reporter.Report(elementName + " actual text : " + actualtext, "info", true);
                    flag = false;
                }
                else
                {
                    BaseClass.Reporter.Report(elementName + " has text '" + expectedtext + "'", "pass", true);
                }
            }

            return flag;
        }

      
        /// <summary>
        /// Verifies if the expected integer value is same as actual integer value
        /// </summary>
        /// <param name="actual">Actual Value</param>
        /// <param name="expected">EXpected Value</param>
        /// <param name="elementName">Name of the element</param>
        /// <param name="exitTestonFailure">If set to true the test will exit if match is not valid. Default value is false. </param> 
        /// <returns>This method returns true if actual  value and expected value are same else returns false </returns>
        public static bool VerifyText(int actual, int expected, string elementName, bool exitTestonFailure = false)
        {
            bool flag = true;
            if (actual.Equals(expected))
            {
                BaseClass.Reporter.Report(elementName + " has expected value -  '" + expected + "'", "pass", true);
            }
            else
            {
                BaseClass.Reporter.Report(elementName + " value does not match the expected ", "fail", true);
                BaseClass.Reporter.Report(elementName + " expected value : " + expected, "info", true);
                BaseClass.Reporter.Report(elementName + " actual value : " + actual, "info", true);
                flag = false;
            }
            if (flag == false)
            {
                if (exitTestonFailure)
                {
                    BaseClass.Reporter.Report("Exiting the test case as the text is not matched", "fail", true, true);
                }
            }
            return flag;
        }
       
        /// <summary>
        /// Checks if the value is true or not
        /// </summary>
        /// <param name="value">Value to be validated</param>
        public static void IsTrue(bool value)
        {
            if(value == true)
            {
                BaseClass.Reporter.Report("Value is true", "pass", true);
            }
            else
            {
                BaseClass.Reporter.Report("Expected value : true. Actual value :"+value, "fail", true);
            }
        }

        /// <summary>
        /// Checks if the value is false     or not
        /// </summary>
        /// <param name="value"></param>
        public static void IsFalse(bool value)
        {
            if (value == false)
            {
                BaseClass.Reporter.Report("Value is false", "pass", true);
            }
            else
            {
                BaseClass.Reporter.Report("Expected value : false. Actual value :" + value, "fail", true);
            }
        }
        /// <summary>
        /// Uploads a file 
        /// </summary>
        /// <param name="filepath">Path of the file to be uplaoded</param>
        public static void UploadFile(string filepath)
        {
            try
            {
                AutoItX.WinActivate("Open");
                Thread.Sleep(2000);
                // AutoItX.Send("^a");
                //AutoItX.WinActivate(popupname);
                Thread.Sleep(400);
                AutoItX.Send(filepath);
                BaseClass.Reporter.Report("Entered filepath to Windows Popup <br/> FilePath : " + filepath, "pass", true);
                Thread.Sleep(1000);
                AutoItX.WinActivate("Open");
                AutoItX.Send("{Enter}");
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Error uploading file <br/> " + e.Message, "fail", true, true);
            }
        }


       
       
        /// <summary>
        /// Gets the reference to Excel file
        /// </summary>
        /// <param name="filepath">Path of excel file</param>
        /// <returns>This Returns the ExcelPackage reference to the specified file </returns>
        public static ExcelPackage GetExcel(string filepath)
        {
            try
            {
                return new ExcelPackage(new FileInfo(filepath));
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Error Connecting to Excel <br/> " + e.Message, "fail", false, true);
                return null;
            }
        }

        /// <summary>
        /// Gets the reference to Excel sheet
        /// </summary>
        /// <param name="filepath">Path of excel file</param>
        /// <param name="sheetname">Name of excel sheet</param>
        /// <returns>Returns the ExcelWorksheet reference to the specified file</returns>
        public static ExcelWorksheet GetWorksheet(string filepath, string sheetname)
        {
            try
            {
                ExcelPackage excel = new ExcelPackage(new FileInfo(filepath));
                return excel.Workbook.Worksheets[sheetname];
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Error Connecting to Excel <br/> " + e.Message, "fail", false, true);
                return null;
            }
        }

        /// <summary>
        /// Gets the column number 
        /// </summary>
        /// <param name="worksheet">ExcelWorksheet reference of the excel sheet</param>
        /// <param name="colName">Column Name in excel</param>
        /// <returns>Returns the columnNumber of the given column</returns>
        public static int GetColNumber(ExcelWorksheet worksheet, string colName)
        {
            try
            {
                for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
                {
                    if (worksheet.Cells[1, i].Value.ToString().Trim().ToLower().Equals(colName.ToLower()))
                    {
                        return i;
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Method -'GetColNumber' , Error occured : " + e.Message, "fail", true, true);
                return -1;
            }
        }

        /// <summary>
        /// Gets the cell value
        /// </summary>
        /// <param name="worksheet">ExcelWorksheet reference of the excel sheet</param>
        /// <param name="row">Row Number</param>
        /// <param name="col">Column Number</param>
        /// <returns>This method</returns>
        public static string GetExcelData(ExcelWorksheet worksheet, int row, int col)
        {
            try
            {
                // string data = worksheet.Cells[row, col].Value.ToString();
                //data = CommonFunctions.RemoveNewLineFromString(data);
                if (worksheet.Cells[row, col].Value == null)
                {
                    return "";
                }
                else
                {
                    return worksheet.Cells[row, col].Value.ToString();
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Method : GetExcelData , Error occured<br/>" + e.Message, "fail", true, true);
                return "";
            }
        }

        /// <summary>
        /// Moves the mouse to the specified element
        /// </summary>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="elementName">Name of the element</param>
        public static void MouseOver(string locatorString, string elementName, bool ScrollIntoView=true)
        {
            element = SupportLibrary.GetObject(locatorString,false,5,ScrollIntoView);
            if (ScrollIntoView)
            {
                SupportLibrary.ScrollIntoView(locatorString);
            }
            try
            {
                Actions action = new Actions(DriverFactory.GetDriver());

                //Performing the mouse hover action on the target element.
                action.MoveToElement(element).Perform();

                BaseClass.Reporter.Report("Mouse over to the element " + elementName, "info", true);
            }
            catch (Exception ex)
            {
                BaseClass.Reporter.Report("Mouse over to the element " + elementName + "failed due to exception" + ex.Message, "info", true);
                BaseClass.ExitTest(ex.Message);
            }
        }


        /// <summary>
        /// Creates a Folder at the specified path
        /// </summary>
        /// <param name="path">Path of the folder </param>
        /// <param name="clearAllDownloads">Boolean value which specifies if the existing folder should be deleted or not</param>
        public static void CreateFolder(string path, bool clearAllDownloads = false)
        {

            int counter = 0;

            try
            {
                if (counter == 0)
                {

                    if (clearAllDownloads)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                            Directory.CreateDirectory(path);
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    counter = 1;
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Method : CreateDownloadFolder , Error occured trying to create download folder", "fail", false, true);
            }

        }

        /// <summary>
        /// Downloads the file in the specified path
        /// </summary>  
        /// <param name="downloadPath">Path of the folder</param>
        /// <param name="locatorString">Element identifier</param>
        /// <param name="file">Name of the file</param>
        /// <returns></returns>
        public static string DownloadFile(string downloadPath, string locatorString, string file)
        {
            CreateFolder(downloadPath);
            Click(locatorString, "");
            string[] downloadfiles = GetDownloadedFiles(downloadPath);
            if (downloadfiles.Length != 0)
            {
                foreach (string downloadfile in downloadfiles)
                {
                    if (downloadfile.Contains(file))
                    {
                        return downloadfile;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Gets the list of file in the specified path
        /// </summary>
        /// <param name="path">Path of the folder</param>
        /// <returns>This method returns the array of file names in the specified path </returns>
        public static string[] GetDownloadedFiles(string path)
        {
            string[] files = null;
            int i = 0;
            while (true)
            {
                try
                {
                    files = Directory.GetFiles(path);
                    if (files.Length == 0)
                    {
                        if (i * 10 == 120)
                        {
                            return files;
                        }
                        else
                        {
                            i = i + 1;
                            Thread.Sleep(100);
                        }
                    }
                    else if (files.Length != 0)
                    {
                        foreach (string file in files)
                        {
                            if (file.Contains(".crdownload") || file.Contains(".tmp"))
                            {
                                if (i * 10 == 120)
                                {
                                    return null;
                                }
                                else
                                {
                                    i = i + 1;
                                    Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                return files;
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

        }

        /// <summary>
        /// Drags and drops the element 
        /// </summary>
        /// <param name="fromLocatorString">From element identifier</param>
        /// <param name="toLocatorString">To element identifier</param>
        /// <returns></returns>
        public static bool DragandDrop(string fromLocatorString, string toLocatorString)
        {
            IWebElement elementfrom = SupportLibrary.GetObject(fromLocatorString);
            IWebElement elementto = SupportLibrary.GetObject(toLocatorString);
            try
            {

                IJavaScriptExecutor js = (IJavaScriptExecutor)DriverFactory.GetDriver();
                js.ExecuteScript("function createEvent(typeOfEvent) {\n" + "var event =document.createEvent(\"CustomEvent\");\n"
                        + "event.initCustomEvent(typeOfEvent,true, true, null);\n" + "event.dataTransfer = {\n" + "data: {},\n"
                        + "setData: function (key, value) {\n" + "this.data[key] = value;\n" + "},\n"
                        + "getData: function (key) {\n" + "return this.data[key];\n" + "}\n" + "};\n" + "return event;\n"
                        + "}\n" + "\n" + "function dispatchEvent(element, event,transferData) {\n"
                        + "if (transferData !== undefined) {\n" + "event.dataTransfer = transferData;\n" + "}\n"
                        + "if (element.dispatchEvent) {\n" + "element.dispatchEvent(event);\n"
                        + "} else if (element.fireEvent) {\n" + "element.fireEvent(\"on\" + event.type, event);\n" + "}\n"
                        + "}\n" + "\n" + "function simulateHTML5DragAndDrop(element, destination) {\n"
                        + "var dragStartEvent =createEvent('dragstart');\n" + "dispatchEvent(element, dragStartEvent);\n"
                        + "var dropEvent = createEvent('drop');\n"
                        + "dispatchEvent(destination, dropEvent,dragStartEvent.dataTransfer);\n"
                        + "var dragEndEvent = createEvent('dragend');\n"
                        + "dispatchEvent(element, dragEndEvent,dropEvent.dataTransfer);\n" + "}\n" + "\n"
                        + "var source = arguments[0];\n" + "var destination = arguments[1];\n"
                        + "simulateHTML5DragAndDrop(source,destination);", elementfrom, elementto);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Clears the cookies from the current browser
        /// </summary>
        public static void DeleteAllVisibleCookies()
        {
            DriverFactory.GetDriver().Manage().Cookies.DeleteAllCookies();
        }

       
        /// <summary>
        ///  Refreshes the browser page
        /// </summary>
        public static void RefreshPage()
        {
            DriverFactory.GetDriver().Navigate().Refresh();
        }

        /// <summary>
        /// Navigates back in browser
        /// </summary>
        public static void NavigateBack()
        {
            DriverFactory.GetDriver().Navigate().Back();
        }

        /// <summary>
        /// Navigates forward in browser
        /// </summary>
        public static void NavigateForward()
        {
            DriverFactory.GetDriver().Navigate().Forward();
        }

        /// <summary>
        /// Gets the Current URL
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentURL()
        {
            return DriverFactory.GetDriver().Url;
        }

        /// <summary>
        /// Sets the url of the current browser
        /// </summary>
        /// <param name="url">url to be initialised</param>
        public static void SetURL (string url)
        {
            DriverFactory.GetDriver().Url = url;
        }

        /// <summary>
        /// Gets the Title of the current Window
        /// </summary>
        /// <returns>This method returns the Window Title</returns>
        public static string GetWindowTitle()
        {
            return DriverFactory.GetDriver().Title;
        }

        /// <summary>
        /// Switches the control to Specified tab
        /// </summary>
        /// <param name="index">Tab number to be user wishes to switch to</param>
        public static void GotoTab(int index)
        {
            IWebDriver driver = DriverFactory.GetDriver();
            try
            {
                driver.SwitchTo().Window(driver.WindowHandles[index]);
                BaseClass.Reporter.Report("Switched to tab : " + (index + 1), "info", true);
            }
            catch (Exception ex)
            {
                BaseClass.Reporter.Report("Error on trying to switch to tab" + (index + 1), "fail", true, true);
            }
        }

        /// <summary>
        /// Switches to the last tab
        /// </summary>
        public static void GotoLastTab()
        {
            IWebDriver driver = DriverFactory.GetDriver();
            try
            {
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                BaseClass.Reporter.Report("Switched to last tab : ", "info", true);
            }
            catch (Exception ex)
            {
                BaseClass.Reporter.Report("Error on trying to switch to last tab", "fail", true, true);
            }
        }

        /// <summary>
        /// This method Appends time in h_mm_ss format.
        /// </summary>
        /// <param name="text">Text to which time should be appended</param>
        /// <returns>Text with appended time stamp in the format hh_mm_ss </returns>
        public static string AppendTimestamptoString(string text)
        {
            if (text != "" && text != null)
            {
                return (text + DateTime.Now.ToString("_hh_mm_ss"));
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// Generates number of specified length
        /// </summary>
        /// <param name="length">Length of the string </param>
        /// <returns>This method returns a string of random numbers of specified length</returns>
        public static string GetRandomNumbers(int length)
        {
            Random random = new Random();
            string chars = "0123456789";

            return new string((Enumerable.Repeat(chars, length)).Select(s => s[random.Next(s.Length)]).ToArray());

        }

        /// <summary>
        /// Generates a random alphanumeric string
        /// </summary>
        /// <param name="length">Length of the string</param>
        /// <returns>This method retuns string of random alpha numeric charecters of  specified length</returns>
        public static string GetRandomAlphaNumericString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrsqtuvwxyz";
            return new string((Enumerable.Repeat(chars, length)).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">Length of the string</param>
        /// <returns>This method returns string of random charecters of specified length</returns>
        public static string GetRandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrsqtuvwxyz";
            return new string((Enumerable.Repeat(chars, length)).Select(s => s[random.Next(s.Length)]).ToArray());
        }

       
        /// <summary>
        /// Gets the current date in YYYYMMDD format
        /// </summary>
        /// <returns>This method returns the date time in YYYYMMDD format </returns>
        public static string GetDate()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            return date;
        }

        /// <summary>
        /// Gets the current date time in YYYYMMDDHHMMSS format
        /// </summary>
        /// <returns>This method returns the date time in YYYYMMDDHHMMSS format </returns>
        public static string GetDateTime()
        {
            string dateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            return dateTime;
        }

        /// <summary>
        /// Gets the current time in HHMMSS format
        /// </summary>
        /// <returns>This method returns the time in HHMMSS format </returns>
        public static string GetTime()
        {
            string dateTime = DateTime.Now.ToString("hhmmss");
            return dateTime;
        }

        /// <summary>
        /// Removes the Newline charecters from the given string 
        /// </summary>
        /// <param name="text">String from which the newline charecter needs to be removed</param>
        /// <returns>This method returns string after removing new line charecter</returns>
        public static string RemoveNewLineFromString(string text)
        {
            try
            {
                if (text != "" && text != null)
                {
                    return text.Replace("\r", "").Replace("\n", "");
                }
                else
                {
                    return text;
                }
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report("Method - RemoveNewLineFromString , Error occured <br/> " + e.Message, "fail", false, true);
                return "";
            }
        }

        /// <summary>
        /// Converts the string to TitleCase
        /// </summary>
        /// <param name="text">Text to convert to TitleCase</param>
        /// <returns></returns>
        public static string ToTitleCase(string text)
        {
            if (text != "" || text != null)
            {
                return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text.ToLower());
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// Resizes the 2D Arrray
        /// </summary>
        /// <param name="original">Original array/param>
        /// <param name="rows">New array row count</param>
        /// <param name="cols">New array column count</param>
        /// <returns>This method returns resized 2D array</returns>
        public static string[,] ReSize2dArray(ref string[,] original, int rows, int cols)
        {
            //create a new 2 dimensional array with
            //the size we want
            string[,] newArray = new string[rows, cols];
            //copy the contents of the old array to the new one
            if (newArray.Length <= original.Length)
            {
                Array.Copy(original, newArray, newArray.Length);
            }
            else
            {
                Array.Copy(original, newArray, original.Length);
            }
            //set the original to the new array
            original = newArray;
            return newArray;
        }

      

    }
}


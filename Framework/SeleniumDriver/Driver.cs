using OpenQA.Selenium;

namespace Framework.SeleniumDriver
{
    class Driver
    {
        private IWebDriver driver = null;

        public Driver()
        {

        }
        public Driver(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebDriver GetDriver()
        {
            return driver;
        }
    }
}

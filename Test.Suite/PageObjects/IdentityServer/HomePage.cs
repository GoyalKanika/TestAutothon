using OpenQA.Selenium;

namespace Test.Suite.PageObjects.IdentityServer
{
    class HomePage
    {
        private static By by = null;

        public static string Link_UserName = "Id=popover";
        public static string Link_Logout = "Id=logoutBtn";
        public static string Btn_LogoutYes = "xpath=.//button[@class='logout-YesBtn']";

        public static string Label_WelcomeToBeatIdS = "xpath=.//span[text()='Welcome to BEAT Identity Server']";
        public static string Card_MyProfile = "xpath=(.//div[@class='card-deck']//a[contains(@href,'Manage')])[1]";

        public static string Card_ChangePassword = "xpath=.//div[@class='card-deck']//a[contains(@href,'ChangePassword')]";

        public static string Card_TwoFactorAuthentication = "xpath =(.//div[@class='card-deck']//a[contains(@href,'TwoFactorAuthentication')])[1]";
       


    }
}

using Framework;
using Framework.UI;
using Test.Suite.PageObjects.Login_Logout_ForgotPassword;
using Test.Suite.Utilities;

namespace Test.Suite.BusinessComponents.Login_LogOut_ForgotPassword
{
    public class LoginPageActions
    {
        

        public void LogintoIdswithDefaultUser()
        {
            CommonFunctions.EnterText(LoginPage.txtbox_Email, AppConfig.getAppConfigValue("Username"),"Username Textbox");
            CommonFunctions.EnterText(LoginPage.txtbox_Password, AppConfig.getAppConfigValue("Password"), "Password Textbox");
            CommonFunctions.Click(LoginPage.btn_Login, "Login button");
                     
         }

        public void LogintoIds()
        {
            CommonFunctions.EnterText(LoginPage.txtbox_Email, BaseClass.GetData("Username"), "Username Textbox");
            CommonFunctions.EnterText(LoginPage.txtbox_Password, BaseClass.GetData("Password"), "Password Textbox");
            CommonFunctions.Click(LoginPage.btn_Login, "Login button");
        }

    }
}

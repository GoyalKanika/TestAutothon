using Framework.UI;
using Test.Suite.PageObjects.IdentityServer;

namespace Test.Suite.BusinessComponents.IdentityServer
{
    public class HomePageActions
    {

        public void Logout()
        {
            SupportLibrary.WaitForPageLoad();
            SupportLibrary.WaitForElementToBeClickable(HomePage.Link_UserName, 10);
            CommonFunctions.Click(HomePage.Link_UserName,"Username Link");
      

            SupportLibrary.WaitForElementToBeClickable(HomePage.Link_Logout,10);
            CommonFunctions.Click(HomePage.Link_Logout, "Logout Link");
        
            SupportLibrary.WaitForElementToBeClickable(HomePage.Btn_LogoutYes, 10);
            CommonFunctions.Click(HomePage.Btn_LogoutYes, "Logout Yes button");

           
        }
    }
}

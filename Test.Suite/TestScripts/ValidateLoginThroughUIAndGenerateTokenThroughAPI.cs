
using Framework;
using Framework.API;
using Framework.UI;
using Framework.Utilities;
using Test.Suite.BusinessComponents.Login_LogOut_ForgotPassword;
using Test.Suite.PageObjects.IdentityServer;
using Test.Suite.Utilities;

namespace Test.Suite.TestScripts
{
    public class ValidateLoginThroughUIAndGenerateTokenThroughAPI : UITestCaseBase
    {
        [Xunit.Fact]
        public void Test()
        {


            #region Declarations 
            XmlFunctions.LoadXml(@"./DataXml/PF_478_VerifyLoginwithvalidUsernameAndPassword.xml");
            LoginPageActions Login = new LoginPageActions();
            APICommonFunctions api = new APICommonFunctions();

            #endregion


            for (int l = 1; l <= Constant.Iterations; l++)
            {
                BaseClass.LaunchApplication();


                Login.LogintoIds();

                CommonFunctions.IsElementDisplayed(HomePage.Link_UserName, "Username Link");
                CommonFunctions.IsElementDisplayed(HomePage.Label_WelcomeToBeatIdS, "Welcome to BEAT IDS Text");
                CommonFunctions.IsElementDisplayed(HomePage.Card_MyProfile, "My Profile Card");
                CommonFunctions.IsElementDisplayed(HomePage.Card_ChangePassword, "Change Password Card");
                CommonFunctions.IsElementDisplayed(HomePage.Card_TwoFactorAuthentication, "Two Factor Authentication Card");

                Constant.CurrentIteration += 1;


            }


            #region authentication
            string Payload;
           string EndPoint = "https://uat.beatapps.net/identity/uat/beat/sts/connect/token";
            api.RequestHeader.Add("ContentType", "application/x-www-form-urlencoded");


            // Payload = "{\"Email\": \"tanya.sharma@acuitykp.com\", \"Password\": \"Acuity@2020\",\"App\": \"CompanyProfileDEV\"}";
            // RequestPayload = new StringContent(Payload, Encoding.UTF8, "application/json");
            Payload = "{\"grant_type\": \"password\",\"client_id\": \"beat-banker-hub-test-pod-client-id\",\"username\": \"nitish.singh@acuitykp.com\",\"password\": \"P@ssw0rd\"}";



            api.POST(EndPoint, Payload);

            string token = api.GetValueFromJsonResponse("access_token");

            api.Dispose();
            #endregion



        }
    }
}


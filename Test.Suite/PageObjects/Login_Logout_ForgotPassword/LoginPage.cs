namespace Test.Suite.PageObjects.Login_Logout_ForgotPassword
{
    class LoginPage
    {


        public static string txtbox_Email = "Name=Username";
        public static string txtbox_Password = "Name=Password";

        public static string btn_Login = "Id=login-button";

        public static string Label_UsernameError = "Id=login-username-error";

        public static string Label_PasswordError = "Id=login-password-error";

        public static string Label_InvalidEmailorpassword = "Id=val-err-msg-style";

        public static string Label_LogoutConfirmationMessage = "xpath=.//div[contains(text(),'You are now logged out')]";
    }
}

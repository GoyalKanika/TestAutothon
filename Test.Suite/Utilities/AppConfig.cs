using System.Configuration;
using System.Reflection;

namespace Test.Suite.Utilities
{
    public class AppConfig
    {
        public static string getAppConfigValue(string key)
        {
            key = string.Format("{0}", key);
            return ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings[key].Value;
        }

        
    }
}

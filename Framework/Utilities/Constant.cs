using System.Xml;

namespace Framework.Utilities
{
    public class Constant
    {
        public static string CurrentTestName { get; set; }
        public static string CurrentTest { get; set; }
        public static string Projectbasepath { get; set; }
        public static string DataSource { get; set; }

        public static string Screenshotpath { get; set; }
     
        public static string Resultpath { get; set; }
        public static string downloadpath { get; set; }
        public static int passed { get; set; }
        public static int failed { get; set; }
        public static bool RunLocally { get; set; }

       // public static int TestRowNbr { get; set; }
        public static int Iterations { get; set; }
        public static int CurrentIteration { get; set; }

        public static XmlDocument xmlDocument = new XmlDocument();
    }
}
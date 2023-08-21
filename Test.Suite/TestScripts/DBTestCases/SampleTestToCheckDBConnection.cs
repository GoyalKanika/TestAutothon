using Framework;
using Framework.Utilities;
using POD.UIAutomation.Suite.Utilities;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Test.Suite.Utilities;
using Xunit;

namespace Test.Suite.TestScripts.DBTestCases
{
    public class SampleTestToCheckDBConnection: UITestCaseBase
    {
        [Xunit.Fact]
        public async Task TestDBCon()
        {;
            string conString = AppConfig.getAppConfigValue("ConnectionString");
            string AWSkey = AppConfig.getAppConfigValue("AWSKey");
            string region = AppConfig.getAppConfigValue("Region");

            string dbQuery1 = "select Top 10 * from userdetails;";

            AWSDBConnection Con = new AWSDBConnection();
            var datatable=await Con.ExecuteQuery(dbQuery1, conString, AWSkey, region);

            foreach (DataRow dataRow in datatable.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    BaseClass.Reporter.Report(item.ToString() + " ", "pass", false);
                

                }
            }

            Assert.True(datatable.Rows.Count > 0);


        }
    }
}

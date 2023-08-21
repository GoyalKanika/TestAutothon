using Framework;
using Framework.Utilities;
using OfficeOpenXml;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Test.Suite.Utilities;
using Xunit;


namespace POD.UIAutomation.Suite.Utilities
{
    public class DriverScript
    {
        private ExcelPackage package;
        private ExcelWorksheet worksheet;
        private HashSet<string> tests;
        //private HashSet<string> priority;

      
        [Trait("Category", "DriverScript")]
        public void test()
        {
            Constant.failed = 0;
            BaseClass.FilePath = AppConfig.getAppConfigValue("FilePath");
            try
            {
                package = new ExcelPackage(new FileInfo(BaseClass.FilePath));
                worksheet = package.Workbook.Worksheets[0];
                int rows = worksheet.Dimension.End.Row;
                int cols = worksheet.Dimension.End.Column;
                tests = new HashSet<string>();
                //priority = new HashSet<string>();
                string scriptnamespace = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["TestScriptNamespace"].Value;
                for (int row = 2; row <= rows; row++)
                {
                    if (worksheet.Cells[row, 5].Value != null)
                    {
                        string data = worksheet.Cells[row, 5].Value.ToString();
                        if (data != "")
                        {
                            if (data.ToLower().Equals("true"))
                            {
                                tests.Add(worksheet.Cells[row, 1].Value.ToString());
                                //priority.Add(worksheet.Cells[row, 4].Value.ToString());
                            }
                        }
                    }
                }
                //executing tests
                for (int i = 0; i < tests.Count; i++)
                {
                    try
                    {
                        Type t = Type.GetType(scriptnamespace + "." + tests.ElementAt<string>(i));
                        var methods = t.GetMethods().Where(m => m.Name.Contains("test")).Select(m => m.Name).ToList();
                        foreach (var method in methods)
                        {
                            try
                            {
                                t.GetMethod(method).Invoke(Activator.CreateInstance(t, null), null);
                            }
                            catch (Exception e)
                            {

                            }
                            //if (AppConfig.getAppConfigValue("TestType").ToLower() == "api")
                            //    BaseClass.TearDownReport();
                            //else
                                BaseClass.TearDown();
                        }
                    }
                    catch (Exception e)
                    {
                        new BaseClass(tests.ElementAt<string>(i)).ReporterSetup().Report("Test Case not found :" + tests.ElementAt<string>(i)
                            + ",<br/>Please note: The testcase name should be same in RunManager, Datatable and  TestScript Class name", "fail", false);
                        Constant.failed = Constant.failed + 1;
                    }
                }
            }
            catch (Exception e)
            {

            }
            string passthreshold = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["TestPassThreshold"].Value.Replace("%", "");
            string project = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["Project"].Value;
            string pipeline = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["Pipeline"].Value;
            string slacknotify = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["SlackNotification"].Value;
            string localrunslacknotify = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["LocalRunSlackNotification"].Value;
            string localpath = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["LocalTestResultPath"].Value;
            string awspath = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["AWSTestResultPath"].Value;
            string channel = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["SlackChannel"].Value;
            string channellocal = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["SlackChannelLocalRun"].Value;
            string resultpath = Constant.Resultpath;
            if (resultpath == null)
            {
                resultpath = @".\Results\Run_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
            }
            string resultlink = "";
            string buildstatus = "";
            if (Constant.RunLocally)
            {
                resultpath = localpath + resultpath.Replace(".", "") + @"\index.html";
                resultlink = resultpath;
            }
            else
            {
                resultpath = awspath + resultpath.Replace(".", "").Replace(@"\", "/") + @"\index.html";
                resultlink = "<" + resultpath + " | Click to view result>";
            }
            float percentpass;
            if (tests.Count == 0)
            {
                percentpass = (float)0;
            }
            else
            {
                percentpass = (float)(tests.Count - Constant.failed) / tests.Count * 100;
            }
            float threshold = float.Parse(passthreshold);
            string slackmsg = "";
            if (percentpass < threshold)
            {
                if (Constant.RunLocally)
                {
                    buildstatus = "_N/A , Test(s) Executed Locally_!!";
                }
                else
                {
                    buildstatus = "_BUILD NOT PROMOTED_";
                }
                Assert.True(false, "Test Pass percentage is below " + threshold + "%, Pass% is " + percentpass.ToString("0.00") + "%");
                slackmsg = ":rotating_light:*_ ATTENTION _*:rotating_light:\n " +
                           "Project : " + project + "\n" +
                           "Pipeline : " + pipeline + "\n" +
                           "Threshold(%) : " + threshold.ToString("0.00") + "%\n" +
                           "Test Pass(%): " + percentpass.ToString("0.00") + "%\n" +
                           "Test Executed : " + tests.Count + "\n" +
                           "Test Passed : " + (tests.Count - Constant.failed) + "\n" +
                           "Test Failed : " + Constant.failed + "\n" +
                           "Report Link : " + resultlink + "\n" +
                           ":red_circle:*BUILD PROMOTION STATUS : " + buildstatus + "*";
            }
            else if (Constant.failed > 0)
            {
                if (Constant.RunLocally)
                {
                    buildstatus = "_N/A , Tests Executed Locally_!!";
                }
                else
                {
                    buildstatus = "_BUILD PROMOTED_";
                }
                slackmsg = ":warning:*_ SUCCESS WITH WARNING _*:warning:\n" +
                           "Project : " + project + "\n" +
                           "Pipeline : " + pipeline + "\n" +
                           "Threshold(%) : " + threshold.ToString("0.00") + "%\n" +
                           "Test Pass(%) : " + percentpass.ToString("0.00") + "%\n" +
                           "Test Executed : " + tests.Count + "\n" +
                           "Test Passed : " + (tests.Count - Constant.failed) + "\n" +
                           "Test Failed : " + Constant.failed + "\n" +
                           "Report Link : " + resultlink + "\n" +
                           ":heavy_check_mark:*BUILD PROMOTION STATUS : " + buildstatus + "*";
            }
            else
            {
                if (Constant.RunLocally)
                {
                    buildstatus = "_N/A , Tests Executed Locally_!!";
                }
                else
                {
                    buildstatus = "_BUILD PROMOTED_";
                }
                slackmsg = ":trophy:*_ SUCCESS _*:trophy:\n" +
                           "Project : " + project + "\n" +
                           "Pipeline : " + pipeline + "\n" +
                           "Threshold(%) : " + threshold.ToString("0.00") + "%\n" +
                           "Test Pass(%) : " + percentpass.ToString("0.00") + "%\n" +
                           "Test Executed : " + tests.Count + "\n" +
                           "Report Link : " + resultlink + "\n" +
                           ":heavy_check_mark:*BUILD PROMOTION STATUS : " + buildstatus + "*";
            }
            if (Constant.RunLocally)
            {
                if (localrunslacknotify.ToLower().Equals("true"))
                {
                    var url = @"https://hooks.slack.com/services/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";
                    //var url = @" https://hooks.slack.com/services/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";
                    var slackClient = new SlackClient(url);
                    var slackMessage = new SlackMessage
                    {
                        Channel = channellocal,
                        Text = slackmsg,
                    };
                    slackClient.Post(slackMessage);
                }
            }
            else
            {
                if (slacknotify.ToLower().Equals("true"))
                {
                    var url = @"https://hooks.slack.com/services/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";
                    //var url = @" https://hooks.slack.com/services/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";
                    var slackClient = new SlackClient(url);
                    var slackMessage = new SlackMessage
                    {
                        Channel = channel,
                        Text = slackmsg,
                    };
                    slackClient.Post(slackMessage);
                }
            }
        }
    }
}

using DealerPro.Config;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DealerPro.Global.CommonMethods;

namespace DealerPro.Global
{
    class Base
    {

        #region To access Path from resource file

        public static int Browser = Int32.Parse(CSBResources.Browser);
        public static string ExcelPath = CSBResources.ExcelPath;
        public static string ScreenshotPath = CSBResources.ScreenShotPath;
        public static string ReportPath = CSBResources.ReportPath;
        public static string JsonRequestPath = CSBResources.JSonRequestPath;
        public static int DealerId = Convert.ToInt32(CSBResources.DealerId);
        public static string JsonResponsePath = CSBResources.JSonResponsePath;
        public static string bookingConfirmation = null;

        #endregion

        #region reports
        public static ExtentTest test;
        public static ExtentReports extent;
        #endregion

        #region CompareResults
        public static void CompareResults(string actualValue, string expectedValue, string validationText)
        {
            if (actualValue == expectedValue)
            {
                Base.test.Log(LogStatus.Pass, validationText + " validation is successful");
            }
            else
            {
                Base.test.Log(LogStatus.Fail, validationText + " validation is not successful");
                SaveScreenShotClass.SaveScreenshot(Global.Driver.driver, validationText);
            }
        }

        public static void LogPageValidation(string validationText,  string result)
        {
            String img = SaveScreenShotClass.SaveScreenshot(Global.Driver.driver, validationText);
            if (result == "Pass")
            {
                Base.test.Log(LogStatus.Pass, validationText + " validation is successful, Screenshot: " + img);
            }
            else
            {
                Base.test.Log(LogStatus.Fail, validationText + " validation is not successful, Screenshot: " + img);
            }
        }

        public static void LogPageValidation(string validationText, string result, Exception e)
        {
            Base.test.Log(LogStatus.Fail, validationText + " validation is not successful", e.Message);
        }

        #endregion

        #region setup and tear down
        [SetUp]
        public void Inititalize()
        {


            switch (Browser)
            {
                case 1:
                    Driver.driver = new FirefoxDriver();
                    break;

                case 2:
                    var options = new ChromeOptions();
                    options.AddArguments("--disable-extensions --disable-extensions-file-access-check --disable-extensions-http-throttling --disable-infobars --enable-automation --start-maximized");
                    options.AddUserProfilePreference("credentials_enable_service", false);
                    options.AddUserProfilePreference("profile.password_manager_enabled", false);
                    Driver.driver = new ChromeDriver(options);
                    break;

            }
            // advisasble to read this documentation before proceeding http://extentreports.relevantcodes.com/net/
            extent = new ExtentReports(ReportPath, false, DisplayOrder.OldestFirst);
            extent.LoadConfig(CSBResources.ReportXMLPath);

        }


        [TearDown]
        public void TearDown()
        {
            // Screenshot
            String img = SaveScreenShotClass.SaveScreenshot(Driver.driver, "TearDown");
            test.Log(LogStatus.Info, "Screen shot: " + img);

            // end test. (Reports)
            extent.EndTest(test);
            // calling Flush writes everything to the log file (Reports)
            extent.Flush();

            // Close the driver            
            Driver.driver.Close();

        }
        #endregion

    }
}
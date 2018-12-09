using DealerPro.Global;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DealerPro.Global.CommonMethods;

namespace DealerPro.Pages
{
    class LandingPage
    {

        public void VerifyLandingPageDetails(CustomerVehicle cv)
        {
            try
            {

                //SQL query to select DealerConfiguration details
                string sql = "select * from DealerConfiguration where DealerId = " + Global.Base.DealerId ;

                //Get Data from DB in a collection
                Global.CommonMethods.DBConnection.CollectionFromDB(sql);

                OpenInvitationLink(Global.CommonMethods.DBConnection.ReadDBData(2, "InvitationDomain",1), cv.CustomerNo, cv.VehicleNo);

                //Populate the Excel Sheet
                Global.CommonMethods.ExcelLib.CollectionFromExcel(Global.Base.ExcelPath, "XPathSheet");

                //Introduce Wait Time
                Global.Driver.IntroduceWait(Global.Driver.driver, 3000);

                //DealerName Validation
                string expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "Name");
                string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(2, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(2, "LocatorValue"));

                Global.Base.CompareResults(actualValue, expectedValue, "DealerName");

                //CustomerName validation
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(4, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(4, "LocatorValue"));
                expectedValue = Global.CommonMethods.ExcelLib.ReadExcelData(4, "InputValue") + cv.FirstName;
                Global.Base.CompareResults(actualValue, expectedValue, "CustomerName");

                //Click Start Now button
                if (Global.Driver.ElementVisible(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(5, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(5, "LocatorValue")))
                {
                    Global.Base.LogPageValidation("Landing Page","Pass");
                    Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(5, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(5, "LocatorValue"));
                }
            }
            catch (Exception e)
            {
                Global.Base.LogPageValidation("Test Failed, Landing Page", "Fail", e);
            }
        }

        internal void OpenInvitationLink(string InvitationDomain, string CustomerNo, string VehicleNo)
        {
            string url = InvitationDomain + "/?d=" + Global.Base.DealerId.ToString() + "&cno=" + CustomerNo + "&vno=" + VehicleNo;

            //Navigate to the URL
            Global.Driver.driver.Navigate().GoToUrl(url);

            //Maximize the screen
            Global.Driver.driver.Manage().Window.Maximize();
        }

    }
}
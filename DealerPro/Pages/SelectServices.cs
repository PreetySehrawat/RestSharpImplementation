using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DealerPro.Global.CommonMethods;

namespace DealerPro.Pages
{
    class SelectServices
    {
        public string recommendedServicename { set; get; }
        public List<string> additionalservices { set; get; }
        
        internal void VerifyServices(CustomerVehicle cv)
        {
            try
            {
                //Trigger GetRecommendedServices
                GetRecommendedServicesAPI grsapi = new GetRecommendedServicesAPI();
                bool APIstaus = grsapi.TriggerGetRecommendedServicesAPI(cv);

                if(APIstaus)
                { 
                    //Populate the Excel Sheet
                    Global.CommonMethods.ExcelLib.CollectionFromExcel(Global.Base.ExcelPath, "XPathSheet");

                    //Validate Page title
                    string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(12, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(12, "LocatorValue"));
                    string expectedValue = Global.CommonMethods.ExcelLib.ReadExcelData(12, "InputValue");
                    Global.Base.CompareResults(actualValue, expectedValue, "Select Service Title ");

                    //Introduce Wait
                    Global.Driver.IntroduceWait(Global.Driver.driver, 200);

                    //Validate one service is selected as Recommended Services
                    actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(21, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(21, "LocatorValue"));
                    expectedValue = Global.CommonMethods.ExcelLib.ReadExcelData(21, "InputValue");
                    Global.Base.CompareResults(actualValue, expectedValue, "Recommended Service is selected");

                    recommendedServicename = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(46, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(46, "LocatorValue"));

                    ((IJavaScriptExecutor)Global.Driver.driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                    //Click on Book Appointment button
                    if (Global.Driver.ElementVisible(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(20, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(20, "LocatorValue")))
                    {
                        Global.Base.LogPageValidation("Select Service Page", "Pass");
                        Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(20, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(20, "LocatorValue"));
                    }
                }
            }
            catch (Exception e)
            {
                Global.Base.LogPageValidation("Test Failed, Select Service Page", "Fail", e);
            }
        }
    }
}

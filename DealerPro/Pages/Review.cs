using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Pages
{
    class Review
    {
        public void VerifyReviewDetails(CustomerVehicle cv, Appointment app, SelectServices ss)
        {
            try
            {
                string fullXPath = null;

                //Validate Page title
                string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(32, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(32, "LocatorValue"));
                string expectedValue = Global.CommonMethods.ExcelLib.ReadExcelData(32, "InputValue");
                Global.Base.CompareResults(actualValue, expectedValue, "Review Title");

                //Validate Transport Option
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(40, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(40, "LocatorValue"));
                expectedValue = app.transportOption;
                Global.Base.CompareResults(actualValue, expectedValue, "Transport Option");

                //Validate Appointment Time
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(43, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(43, "LocatorValue"));
                expectedValue = app.appointmentTime;
                Global.Base.CompareResults(actualValue, expectedValue, "Appointment Time");

                //Validate Appointment Date and Day 
                //actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(43, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(43, "LocatorValue"));
                //expectedValue = app.appointmentDay + app.appointmentDate;
                //Global.Base.CompareResults(actualValue, expectedValue, "Appointment Day and Date");


                //Validate Model year and Description
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(47, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(47, "LocatorValue"));
                expectedValue = cv.ModelYear + " " +cv.ModelDescription;
                Global.Base.CompareResults(actualValue, expectedValue, "Model Year & Model Description on Review screen");

                 //Validate Selected Service
                fullXPath = Global.CommonMethods.ExcelLib.ReadExcelData(35, "LocatorValue") + "1" + Global.CommonMethods.ExcelLib.ReadExcelData(36, "LocatorValue");
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(35, "Locator"), fullXPath);
                expectedValue = ss.recommendedServicename;
                Global.Base.CompareResults(actualValue, expectedValue, "Recommended Service on Review screen");

                //Validate FirstName 
                fullXPath = Global.CommonMethods.ExcelLib.ReadExcelData(37, "LocatorValue") + "1" +Global.CommonMethods.ExcelLib.ReadExcelData(38, "LocatorValue");
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(43, "Locator"), fullXPath);
                expectedValue = cv.FirstName;
                Global.Base.CompareResults(actualValue, expectedValue, "First Name on Review screen");

                //Validate Last name 
                fullXPath = Global.CommonMethods.ExcelLib.ReadExcelData(37, "LocatorValue") + "2" + Global.CommonMethods.ExcelLib.ReadExcelData(38, "LocatorValue");
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(43, "Locator"), fullXPath);
                expectedValue = cv.Surname;
                Global.Base.CompareResults(actualValue, expectedValue, "Last Name on Review screen");

                //Validate Email
                fullXPath = Global.CommonMethods.ExcelLib.ReadExcelData(37, "LocatorValue") + "3" + Global.CommonMethods.ExcelLib.ReadExcelData(38, "LocatorValue");
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(43, "Locator"), fullXPath);
                expectedValue = cv.CustomerEmail;
                Global.Base.CompareResults(actualValue, expectedValue, "Email on Review screen");

                //Validate Phone Number
                fullXPath = Global.CommonMethods.ExcelLib.ReadExcelData(37, "LocatorValue") + "4" + Global.CommonMethods.ExcelLib.ReadExcelData(38, "LocatorValue");
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(43, "Locator"), fullXPath);
                expectedValue = cv.PhoneNumber;
                Global.Base.CompareResults(actualValue, expectedValue, "Phone Number on Review screen");

                //Click the Disclaimer
                Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(45, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(45, "LocatorValue"));

                ((IJavaScriptExecutor)Global.Driver.driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                //Log Page Validation
                Global.Base.LogPageValidation("Review screen ", "Pass");

                //Click Book Now Button
                Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(39, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(39, "LocatorValue"));

                //Introduce Wait Time
                Global.Driver.IntroduceWait(Global.Driver.driver, 2000);

                //Get Confirmation Number
                Global.Base.bookingConfirmation = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(49, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(49, "LocatorValue"));
            }
            catch(Exception e)
            {
                Global.Base.LogPageValidation("Test Failed, Review Page ", "Fail", e);
            }
        }

    }
}

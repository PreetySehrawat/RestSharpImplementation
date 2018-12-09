using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DealerPro.Global.CommonMethods;

namespace DealerPro.Pages
{
    class CarDetails
    {

        public void VerifyCarDetails(CustomerVehicle cv)
        {
            try
            {
                //Validate Page title
                string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(6, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(6, "LocatorValue"));
                string expectedValue = Global.CommonMethods.ExcelLib.ReadExcelData(6, "InputValue");
                Global.Base.CompareResults(actualValue, expectedValue, "Car Details Title");

                //Validate Registration Number
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(7, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(7, "LocatorValue"));
                expectedValue = cv.RegistrationNo;
                Global.Base.CompareResults(actualValue, expectedValue, "RegistrationNo");

                //Validate ModelYear and ModelDescription
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(8, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(8, "LocatorValue"));
                expectedValue = cv.ModelYear + " " + cv.ModelDescription;
                Global.Base.CompareResults(actualValue, expectedValue, "ModelYear&ModelDescription");

                if(Global.Base.DealerId != 8)
                { 
                    //Validate Odometer reading
                    actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(9, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(9, "LocatorValue"));
                    expectedValue = cv.NextServiceMileage;
                    expectedValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(9, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(9, "LocatorValue"));
                    Global.Base.CompareResults(actualValue, expectedValue, "EstOdometer");

                    //Validate Text on Confirm Card Details page
                    actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(10, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(10, "LocatorValue"));
                    expectedValue = Global.CommonMethods.ExcelLib.ReadExcelData(10, "InputValue");
                    Global.Base.CompareResults(actualValue, expectedValue, "TextForOdometer");
                }

                //Click on Book Service button
                if (Global.Driver.ElementVisible(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(11, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(11, "LocatorValue")))
                {
                    Global.Base.LogPageValidation("Confirm Your Car's Details Page", "Pass");
                    Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(11, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(11, "LocatorValue"));
                }
                    

            }
            catch (Exception e)
            {
                Global.Base.LogPageValidation("Test Failed, Confirm Your Car Details Page", "Fail", e);
            }
        }
    }
}

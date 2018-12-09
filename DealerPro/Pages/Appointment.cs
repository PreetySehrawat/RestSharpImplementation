using DealerPro.Global;
using OpenQA.Selenium;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Pages
{
    class Appointment
    {
        public string appointmentDate { set; get; }
        public string appointmentDay { set; get; }
        public string appointmentTime { set; get; }
        public string transportOption { set; get; }

        public void VerifyAppointmentDetails(CustomerVehicle cv)
        {
            try
            {

                //Validate Page title
                string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(22, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(22, "LocatorValue"));
                string expectedValue = Global.CommonMethods.ExcelLib.ReadExcelData(22, "InputValue");
                Global.Base.CompareResults(actualValue, expectedValue, "Appointment Title");

                //Trigger GetAppointmentSlots
                string aPIRspns = null;
                GetApppointmentSlotsAPI grsapi = new GetApppointmentSlotsAPI();
                bool aPIstatus = grsapi.TriggerGetApppointmentSlotsAPI(cv, out aPIRspns);

                //Populate the Excel Sheet
                Global.CommonMethods.ExcelLib.CollectionFromExcel(Global.Base.ExcelPath, "XPathSheet");

                //Verifying the GetAppointmentSlots response if APIstatus is OK
                if (aPIstatus)
                {
                    if(aPIRspns != null && aPIRspns != "" )
                    {

                        //Validate Section for Select Transport title
                        actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(23, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(23, "LocatorValue"));
                        expectedValue = Global.CommonMethods.ExcelLib.ReadExcelData(23, "InputValue");
                        Global.Base.CompareResults(actualValue, expectedValue, "Select Transportation Title");

                        int i = 1;

                        string option = null;
                        do
                        {
                            
                            string firsttransportOption = null;
                            option = getBetween(aPIRspns, "OptionID\":\"" + i.ToString(), ",{\"Date");
                            firsttransportOption = VerifyTransportOptiondetails(option, i);
                            if (i == 1)
                            {
                                appointmentDate = VerifyFirstAvailableDate(option);
                                transportOption = firsttransportOption;
                            }
                            i = i + 1;
                        } while (i == 2);

                        appointmentTime = Global.CommonMethods.ExcelLib.ReadExcelData(41, "InputValue");

                        ((IJavaScriptExecutor)Global.Driver.driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                        //Click Next button
                        Global.Base.LogPageValidation("Appointment Screen ", "Pass");
                        Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(31, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(31, "LocatorValue"));
                    }
                }
            }
            catch (Exception e)
            {
                Global.Base.LogPageValidation("Test Failed, Appointment Page ", "Fail", e);
            }
        }



        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }


        private string VerifyTransportOptiondetails(string option, int optionnumber)
        {
            string optionName = getBetween(option, "OptionDisplayName\":\"", "\",\"OptionDescription");
            string optionDescription = getBetween(option, "OptionDescription\":\"", "\",\"Slots");
            string actualValue = null;

            //Validate Transportation Option Name
            if (optionnumber == 1)
                actualValue = Global.CommonMethods.ExcelLib.ReadExcelData(24, "InputValue");
            else
                actualValue = Global.CommonMethods.ExcelLib.ReadExcelData(25, "InputValue");
            Global.Base.CompareResults(actualValue, optionName, optionName);

            //Validate Transportation Option Description
            if (optionnumber == 1)
                actualValue = Global.CommonMethods.ExcelLib.ReadExcelData(26, "InputValue");

            Global.Base.CompareResults(actualValue, optionDescription, optionName + "Description");



            return optionName;
        }


        private string VerifyFirstAvailableDate(string option)
        {
            string firstavailabledate = getBetween(option, "Date\":\"", "\",\"Slots");
            int monthNo = Int32.Parse(firstavailabledate.Substring(5, 2));
            string year = firstavailabledate.Substring(0, 4);
            string date = firstavailabledate.Substring(8, 2);

            DateTime firstDate = DateTime.Parse(firstavailabledate);

            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(monthNo).ToString();
            string expectedValue = strMonthName + " " + year;

            //Validate Month and Year in Select Date
            string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(28, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(28, "LocatorValue"));
            Global.Base.CompareResults(actualValue, expectedValue, "Month & Year in Select Date");

            //Log First available date from API
            Base.test.Log(LogStatus.Info, "First Available Date from API is " + firstavailabledate);

            //Validate First Available Day
            actualValue = Global.Driver.FindElementsUsingForLoop(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(29, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(29, "LocatorValue"));
            if(actualValue.Length == 1)
                actualValue = year + "-" + monthNo + "-" + "0"+ actualValue;
            else
                actualValue = year + "-" + monthNo + "-" + actualValue;

            //Log First available date from screen
            Base.test.Log(LogStatus.Info, "First Available Date from Screen is " + actualValue);

            return firstavailabledate;
        }



        //private string GetFirstAvailableDateFromScreen()
        //{

        //    string expectedValue = null;
        //    for (int i = 1; i < 7; i++)
        //    {
        //        for(int j = 1; j < 8; j++)
        //        {
        //            string daytext = Global.Driver.FindElementsUsingForLoop(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(29, "Locator"), "//div[@class='DayContent FullyAvailable']");
        //            string fullXPath = Global.CommonMethods.ExcelLib.ReadExcelData(29, "LocatorValue") + i.ToString() + "//td[" + j.ToString() + "]";
        //            bool enabled = Global.Driver.ElementVisible(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(29, "Locator"), fullXPath);
        //            if (enabled)
        //            {
        //                expectedValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(29, "Locator"), fullXPath);
        //                break;
        //            }
        //        }
        //    }
        //    return expectedValue;
        //}

        //private void VerifyTimeslotforFirstAvailableDate(string option)
        //{
        //    getBetween(option, "\"Slots\":[\"",)
        //    throw new NotImplementedException();
        //}


        //private void VerifyLastAvailableDate(string aPIRspns)
        //{
        //    //string lastAvailableDate = getBetween(option, "Date\":\"", "\",\"Slots");
        //    string strStart = "Date\":\"";
        //    string strEnd = "\",\"Slots";

        //    int start = aPIRspns.LastIndexOf(strStart, 0) + strStart.Length;
        //    int end = aPIRspns.IndexOf(strEnd, start);
        //    string lastAvailableDate = aPIRspns.Substring(start, end - start);


        //    //Navigate to next two months
        //    Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(30, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(30, "LocatorValue"));
        //    Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(30, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(30, "LocatorValue"));

        //    string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(28, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(28, "LocatorValue"));


        //    //Validate Last Available Date

        //    Global.Base.CompareResults(actualValue, lastAvailableDate, "Last Available Date");
        //}
    }
}

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
    class Email
    {

        public void LoginToYopMail(string emailid)
        {
            try
            { 
            string url = "http://www.yopmail.com/en/";

                //Navigate to the URL
                Global.Driver.driver.Navigate().GoToUrl(url);

                //Maximize the screen
                Global.Driver.driver.Manage().Window.Maximize();

                //Populate the XPath for YOPMail
                Global.CommonMethods.ExcelLib.CollectionFromExcel(Global.Base.ExcelPath, "YOPMailXPath");
        
                //Send Email Address
                Global.Driver.SendText(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(2, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(2, "LocatorValue"), emailid);

                //Click on Check Inbox Button
                Global.Driver.ClickButton(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(3, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(3, "LocatorValue"));

                Base.test.Log(LogStatus.Pass, "Login to  Yopmail is successful ");
            }
            catch (Exception e)
            {
                Base.test.Log(LogStatus.Fail, "Login to  Yopmail is not successful ", e.Message);
            }

        }


        public string VerifyInvitationEmail(CustomerVehicle cv)
        {
            try
            {
                //Switch to Frame with email details
                Global.Driver.driver.SwitchTo().Frame("ifinbox");

                //Get Email Date
                string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(8, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(8, "LocatorValue"));

                //string expectedValue = DateTime.Now.ToString("yyyy-mm-dd");
                string expectedValue = "today";

                //if email date is not of today, that means CSV upload is not successful
                if (!(actualValue.Equals(expectedValue)))
                {
                    Base.test.Log(LogStatus.Fail, "Email Date is not as of today");
                    return null;
                }
                else
                    Base.test.Log(LogStatus.Pass, "Email Date is as of today");

                //Switch to default frame
                Global.Driver.driver.SwitchTo().DefaultContent();

                //Switch to Frame with email details
                Global.Driver.driver.SwitchTo().Frame("ifmail");


                //Get Email address for Dealer 
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(6, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(6, "LocatorValue"));
                actualValue = actualValue.Replace("From: ", "");
                expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "EmailAddress", 1);

                if (actualValue.Contains(expectedValue))
                    Base.test.Log(LogStatus.Pass, "Dealer Email Address Validation is successful");
                else
                    Base.test.Log(LogStatus.Fail, "Dealer Email Address Validation failed for Dealer Id" + Global.Base.DealerId);

                //Create inivtation url by using domain from Dealer Configuration table
                string invitationUrl = Global.CommonMethods.DBConnection.ReadDBData(2, "InvitationDomain", 1) + "/?d=" + Global.Base.DealerId.ToString() + "&cno=" + cv.CustomerNo + "&vno=" + cv.VehicleNo;

                //Get Email Subject
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(5, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(5, "LocatorValue"));

                //SQL query to select DealerConfiguration details
                string sql = "select * from CommunicationTemplate where DealerId = " + Global.Base.DealerId;

                //Get Data from DB in a collection
                Global.CommonMethods.DBConnection.CollectionFromDB(sql);

                expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "InvitationEmailSubject");

                if (actualValue.Equals(expectedValue))
                    Base.test.Log(LogStatus.Pass, "Email Subject Validation is successful");
                else
                   Base.test.Log(LogStatus.Fail, "Email Subject Validation failed for Dealer Id" + Global.Base.DealerId);

                //Get Email content
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(7, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(7, "LocatorValue"));
                expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "InvitationEmailContent");
                expectedValue = expectedValue.Replace("{REGISTRATION-NUMBER}", cv.RegistrationNo);
                expectedValue = expectedValue.Replace("{SERVICE-BOOKING-INVITATION-URL}", invitationUrl);
                expectedValue = expectedValue.Replace("<br/><br/>just", "just");
                expectedValue = expectedValue.Replace("<br/>", "\r\n");

                if (actualValue.Equals(expectedValue))
                {
                    SaveScreenShotClass.SaveScreenshot(Global.Driver.driver, "VerifyInvitationEmailSuccessful");
                    Base.test.Log(LogStatus.Pass, "Email Content Validation is successful");
                }
                else
                    Base.test.Log(LogStatus.Fail, "Email Content Validation failed for Dealer Id" + Global.Base.DealerId);

                return invitationUrl;
            }
            catch (Exception e)
            {
                Base.test.Log(LogStatus.Fail, "Test Failed in VerifyInvitationEmail", e.Message);
                SaveScreenShotClass.SaveScreenshot(Global.Driver.driver, "VerifyInvitationEmailUnSuccessful");
                return null;
            }

        }

        internal void VerifyBookingConfirmationCustomerEmail(CustomerVehicle cv, Appointment app, DealerConfiguration dc)
        {
            try
            {
                //Switch to Frame with email details
                Global.Driver.driver.SwitchTo().Frame("ifinbox");

                //Get Email Date
                string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(8, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(8, "LocatorValue"));

                //string expectedValue = DateTime.Now.ToString("yyyy-mm-dd");
                string expectedValue = "today";

                //if email date is not of today, that means CSV upload is not successful
                if (!(actualValue.Equals(expectedValue)))
                {
                    Base.test.Log(LogStatus.Fail, "Customer Booking Confirmation Email Date is not as of today");
                }
                else
                    Base.test.Log(LogStatus.Pass, "Customer Booking Confirmation Email Date is as of today");

                //Switch to default frame
                Global.Driver.driver.SwitchTo().DefaultContent();

                //Switch to Frame with email details
                Global.Driver.driver.SwitchTo().Frame("ifmail");


                //Get Email address for Service Booking App 
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(6, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(6, "LocatorValue"));
                actualValue = actualValue.Replace("From: ", "");
                expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "EmailAddress", 1);

                if (actualValue.Contains(expectedValue))
                    Base.test.Log(LogStatus.Pass, "Customer Booking - Dealer Email Address Validation is successful");
                else
                    Base.test.Log(LogStatus.Fail, "Customer Booking - Dealer Email Address Validation failed for Dealer Id" + Global.Base.DealerId);

                 //Get Email Subject
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(5, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(5, "LocatorValue"));

                //SQL query to select DealerConfiguration details
                string sql = "select * from CommunicationTemplate where DealerId = " + Global.Base.DealerId;

                //Get Data from DB in a collection
                Global.CommonMethods.DBConnection.CollectionFromDB(sql);

                expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "CustomerConfirmationEmailSubject");

                if (actualValue.Equals(expectedValue))
                    Base.test.Log(LogStatus.Pass, "Customer Booking Email Subject Validation is successful");
                else
                    Base.test.Log(LogStatus.Fail, "Customer Booking Email Subject Validation failed for Dealer Id" + Global.Base.DealerId);

                //Get Email content
                actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(7, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(7, "LocatorValue"));
                expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "CustomerConfirmationEmailContent");
                expectedValue = expectedValue.Replace("{0}", cv.FirstName);
                expectedValue = expectedValue.Replace("{1}", cv.ModelDescription);
                expectedValue = expectedValue.Replace("{2}", cv.RegistrationNo);
                expectedValue = expectedValue.Replace("{3}", Global.Base.bookingConfirmation);
                expectedValue = expectedValue.Replace("{4}", app.appointmentDate);
                app.appointmentTime = app.appointmentTime.Replace(" AM", "-07:45");
                expectedValue = expectedValue.Replace("{5}", app.appointmentTime);
                expectedValue = expectedValue.Replace("{6}", dc.dealerPhone);
                expectedValue = expectedValue.Replace("{7}", dc.dealerEmailAddress);
                expectedValue = expectedValue.Replace("{8}", dc.dealerName);
                expectedValue = expectedValue.Replace("<br/>", "\r\n");

                if (actualValue.Equals(expectedValue))
                {
                    SaveScreenShotClass.SaveScreenshot(Global.Driver.driver, "VerifyInvitationEmailSuccessful");
                    Base.test.Log(LogStatus.Pass, "Customer Booking Email Content Validation is successful");
                }
                else
                    Base.test.Log(LogStatus.Fail, "Customer Booking Email Content Validation failed for Dealer Id" + Global.Base.DealerId);

            }
            catch (Exception e)
            {
                Base.test.Log(LogStatus.Fail, "Test Failed in VerifyInvitationEmail", e.Message);
                SaveScreenShotClass.SaveScreenshot(Global.Driver.driver, "VerifyInvitationEmailUnSuccessful");
            }
        }

        internal void VerifyBookingConfirmationDealerEmail(CustomerVehicle cv, Appointment app, DealerConfiguration dc)
        {
            try
            { 
            //Switch to Frame with email details
            Global.Driver.driver.SwitchTo().Frame("ifinbox");

            //Get Email Date
            string actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(8, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(8, "LocatorValue"));

            //string expectedValue = DateTime.Now.ToString("yyyy-mm-dd");
            string expectedValue = "today";

            //if email date is not of today, that means CSV upload is not successful
            if (!(actualValue.Equals(expectedValue)))
            {
                Base.test.Log(LogStatus.Fail, "Dealer Booking Confirmation Email Date is not as of today");
            }
            else
                Base.test.Log(LogStatus.Pass, "Dealer Booking Confirmation Email Date is as of today");

            //Switch to default frame
            Global.Driver.driver.SwitchTo().DefaultContent();

            //Switch to Frame with email details
            Global.Driver.driver.SwitchTo().Frame("ifmail");


            //Get Email address for Service Booking App 
            actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(6, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(6, "LocatorValue"));
            actualValue = actualValue.Replace("From: ", "");
            expectedValue = "servicebookingapp@experieco.com";

            if (actualValue.Contains(expectedValue))
                Base.test.Log(LogStatus.Pass, "Experieco Email Address Validation is successful");
            else
                Base.test.Log(LogStatus.Fail, "Experieco Email Address Validation failed for Dealer Id" + Global.Base.DealerId);

            //Create inivtation url by using domain from Dealer Configuration table
            string invitationUrl = Global.CommonMethods.DBConnection.ReadDBData(2, "InvitationDomain", 1) + "/?d=" + Global.Base.DealerId.ToString() + "&cno=" + cv.CustomerNo + "&vno=" + cv.VehicleNo;

            //Get Email Subject
            actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(5, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(5, "LocatorValue"));

            //SQL query to select DealerConfiguration details
            string sql = "select * from CommunicationTemplate where DealerId = " + Global.Base.DealerId;

            //Get Data from DB in a collection
            Global.CommonMethods.DBConnection.CollectionFromDB(sql);

            expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "DealerConfirmationEmailSubject");

            if (actualValue.Equals(expectedValue))
                Base.test.Log(LogStatus.Pass, "Dealer Booking Confirmation Email Subject Validation is successful");
            else
                Base.test.Log(LogStatus.Fail, "Dealer Booking Confirmation Email Subject Validation failed for Dealer Id" + Global.Base.DealerId);

            //Get Email content
            actualValue = Global.Driver.GetTextValue(Global.Driver.driver, Global.CommonMethods.ExcelLib.ReadExcelData(7, "Locator"), Global.CommonMethods.ExcelLib.ReadExcelData(7, "LocatorValue"));
            expectedValue = Global.CommonMethods.DBConnection.ReadDBData(2, "DealerConfirmationEmailContent");
            expectedValue = expectedValue.Replace("{0}", Global.Base.bookingConfirmation);
            expectedValue = expectedValue.Replace("{1}", cv.FirstName);
            expectedValue = expectedValue.Replace("{2}", cv.Surname);
            expectedValue = expectedValue.Replace("{3}", cv.PhoneNumber);
            expectedValue = expectedValue.Replace("{4}", cv.CustomerEmail);
            expectedValue = expectedValue.Replace("{5}", cv.VehicleNo);
            expectedValue = expectedValue.Replace("{6}", app.appointmentDate);
            app.appointmentTime = app.appointmentTime.Replace(" AM", "-07:45");
            expectedValue = expectedValue.Replace("{7}", app.appointmentTime);
            expectedValue = expectedValue.Replace("{8}", dc.dealerName);
            expectedValue = expectedValue.Replace("<br/>", "\r\n");

            if (actualValue.Equals(expectedValue))
            {
                SaveScreenShotClass.SaveScreenshot(Global.Driver.driver, "VerifyBookingConfirmationDealerEmail");
                Base.test.Log(LogStatus.Pass, "Dealer Booking Confirmation Email Content Validation is successful");
            }
            else
                Base.test.Log(LogStatus.Fail, "Dealer Booking Confirmation Email Content Validation failed for Dealer Id " + Global.Base.DealerId);

            }
            catch (Exception e)
            {
                Base.test.Log(LogStatus.Fail, "Test Failed in VerifyBookingConfirmationDealerEmail", e.Message);
                SaveScreenShotClass.SaveScreenshot(Global.Driver.driver, "VerifyBookingConfirmationDealerEmail");
            }
        }
    }
}

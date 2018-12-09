using DealerPro.Global;
using DealerPro.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Test
{
    public class CreateABooking
    {
        [TestFixture]
        class Booking : DealerPro.Global.Base
        {
            [Test, Description("Test to trigger SaveCustomervehicleAPI, generate Invitation, Make a booking")]
            public void CreateBookingWithCSVUploadAPI()
            {
                bool APIstaus = false;
                test = extent.StartTest("Make a booking by SaveCustomervehicleAPI using Standard transport");

                //Get Dealer Configuraton details and save it in a collection
                DealerConfiguration dc = new DealerConfiguration();
                dc.GetDealerConfigurationDBData();

                //Updload CSV Data
                CustomerVehicle cv = new CustomerVehicle();
                cv.UploadCSVDetails(2);

                //Trigger SaveCustomerVehicleAPI to generate Invitation link
                SaveCustomerVehicleAPI scvapi = new SaveCustomerVehicleAPI();
                APIstaus = scvapi.TriggerSaveCustomerVehicleAPI(cv);

                if(APIstaus)
                {
                    //Email login and verification
                    Email invurl = new Email();
                    invurl.LoginToYopMail(cv.CustomerEmail);
                    invurl.VerifyInvitationEmail(cv);

                    //Access the invitation link and verify Landing Page Details
                    LandingPage lp = new LandingPage();
                    lp.VerifyLandingPageDetails(cv);

                    //Verify Car Details on Confirm Your Car's Details page
                    CarDetails cd = new CarDetails();
                    cd.VerifyCarDetails(cv);

                    //Verify Select Services page details
                    SelectServices ss = new SelectServices();
                    ss.VerifyServices(cv);

                    //Verify Appointment page details
                    Appointment app = new Appointment();
                    app.VerifyAppointmentDetails(cv);

                    //Verify Review screen
                    Review vrd = new Review();
                    vrd.VerifyReviewDetails(cv, app, ss);

                    if(Global.Base.bookingConfirmation != null)
                    { 
                        //Verify Confirmation Email for Dealer
                        invurl.LoginToYopMail(dc.dealerEmailAddress);
                        invurl.VerifyBookingConfirmationDealerEmail(cv, app, dc);

                        //Verify Confirmation Email for Customer
                        invurl.LoginToYopMail(cv.CustomerEmail);
                        invurl.VerifyBookingConfirmationCustomerEmail(cv,app,dc);
                    }
                }
            }

            [Test, Description("Test to upload CSV manually, access the Invitation link and make a booking")]
            public void CreateBookingWithCSVUpload()
            {

                test = extent.StartTest("Validate Dealer Details");

                //2 means first record in excel file
                CustomerVehicle cv = new CustomerVehicle();
                cv.UploadCSVDetails(2);

                //Get Dealer Configuraton details and save it in a collection
                DealerConfiguration dc = new DealerConfiguration();
                dc.GetDealerConfigurationDBData();

                Email invurl = new Email();
                invurl.LoginToYopMail(cv.CustomerEmail);
                invurl.VerifyInvitationEmail(cv);

                LandingPage lp = new LandingPage();
                lp.VerifyLandingPageDetails(cv);
            }

        }
    }
}

using DealerPro.Global;
using DealerPro.Pages;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Test
{
    [TestFixture]
    class TestAPIs : DealerPro.Global.Base
    {
        [Test, Description("Test SaveCustomerVehicleAPI to generate Invitation Link")]
        public void SaveCustomerVehicleDetails()
        {
            test = extent.StartTest("API Test - SaveCustomerVehicleDetails");

            CustomerVehicle cv = new CustomerVehicle();
            cv.UploadCSVDetails(2);

            SaveCustomerVehicleAPI scvapi = new SaveCustomerVehicleAPI();
            bool aPIstaus = scvapi.TriggerSaveCustomerVehicleAPI(cv);

        }

        [Test]
        public void GetRecommendedServices()
        {
            test = extent.StartTest("API Test - GetRecommendedServices");

            CustomerVehicle cv = new CustomerVehicle();
            cv.UploadCSVDetails(2);

            GetRecommendedServicesAPI grsapi = new GetRecommendedServicesAPI();
            bool aPIstaus = grsapi.TriggerGetRecommendedServicesAPI(cv);

        }

        [Test]
        public void GetAppointmentSlots()
        {


        }

        [Test]
        public void CreateAppointment()
        {


        }
    }
}
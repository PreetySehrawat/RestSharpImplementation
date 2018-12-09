using DealerPro.Global;
using Newtonsoft.Json;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Pages
{
    class GetApppointmentSlotsAPI
    {
        public bool TriggerGetApppointmentSlotsAPI(CustomerVehicle cv, out string aPIResponse)
        {
            try
            {
                string aPIName = "GetAppointmentSlots";

                string aPIRequest = GenerateAPIRequest(cv);

                APIMethods api = new APIMethods();

                //Save API Request
                api.SaveAPIRequest(aPIName, aPIRequest);

                //Trigger API Request
                aPIResponse = api.TriggerAPI(aPIName, aPIRequest);

                bool status = api.CheckAPIRspnsStatus();
                return status;
            }
            catch (Exception e)
            {
                Global.Base.LogPageValidation("Test Failed, GetApppointmentSlots ", "Fail", e);
                aPIResponse = "";
                return false;
            }
        }

        internal string GenerateAPIRequest(CustomerVehicle cv)
        {
            string JobCode = "";
            string currentdate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime newDate = DateTime.Now.AddDays(60);

            string endDate = newDate.ToString("yyyy-MM-dd");

            //If Experico implementation, assigning a dummy job code, as API response is independent of Job Code.
            if (Global.Base.DealerId == 8)
                JobCode = "[100]"; 

            string JSONresult = "{\"DealerId\": \"" + Global.Base.DealerId + "\", \"CustomerNo\": \"" + cv.CustomerNo + "\" , \"JobCode\": [\"" + JobCode
                                + "\"] , \"EndDate\": \"" + endDate + "\" , \"InitialDate\": \"" + currentdate + "\"}";

            return JSONresult;
        }

    }
}

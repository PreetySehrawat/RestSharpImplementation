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
    class SaveCustomerVehicleAPI
    {
        public bool TriggerSaveCustomerVehicleAPI(CustomerVehicle cv)
        {
            try
            { 
            string aPIResponse = null;

            string aPIName = "SaveCustomerVehicle";

            string aPIRequest = GenerateAPIRequest(cv);

            APIMethods api = new APIMethods();

            //Save API Request
            api.SaveAPIRequest(aPIName, aPIRequest);
             
            //Trigger API Request
            aPIResponse = api.TriggerAPI(aPIName, aPIRequest);

            bool status = api.CheckAPIRspnsStatus();

            return status;
            }
            catch(Exception e)
            {
                Base.test.Log(LogStatus.Fail, "TriggerSaveCustomerVehicleAPI is unsuccessful" + e.Message);
                return false;
            }
        }

        internal string GenerateAPIRequest(CustomerVehicle cv)
        {
            string JSONresult = JsonConvert.SerializeObject(cv);
            return JSONresult;
        }


    }
}

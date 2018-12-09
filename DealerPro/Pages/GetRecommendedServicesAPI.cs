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
    class GetRecommendedServicesAPI
    {
        public bool TriggerGetRecommendedServicesAPI(CustomerVehicle cv)
        {
            try
            {
                string aPIResponse = null;

                string aPIName = "GetRecommendedServices";

                string aPIRequest = GenerateAPIRequest(cv);

                APIMethods api = new APIMethods();
                
                //Save API Request
                api.SaveAPIRequest(aPIName, aPIRequest);

                //Trigger API Request
                aPIResponse = api.TriggerAPI(aPIName, aPIRequest);

                bool status = api.CheckAPIRspnsStatus();

                //If API Response is OK, Compare JSON response
                if (status)
                {
                    Base.test.Log(LogStatus.Pass, "GetRecommendedServices is successful");
                    string fileName = Global.Base.JsonResponsePath;
                    fileName = fileName + "\\" + "GetRecommendedServices.json";
                    //Read API Response from Test Results folder
                    string actualRspns = api.ReadJson(fileName);
                    bool comparisonstatus = api.CompareJSonResponse(aPIResponse, actualRspns);
                    if(comparisonstatus)
                        Base.test.Log(LogStatus.Pass, "Comparison of GetRecommendedServices response is successful");
                }

                return status;
            }
            catch (Exception e)
            {
                Global.Base.LogPageValidation("Test Failed, GetRecommendedServices ", "Fail", e);
                return false;
            }
        }

        internal string GenerateAPIRequest(CustomerVehicle cv)
        {

            string JSONresult = "{\"DealerId\": \"" + Global.Base.DealerId + "\", \"MakeCode\": \"" + cv.MakeCode + "\" , \"ModelCode\": \"" + cv.ModelCode
                                + "\" , \"EstOdometer\": \"" + cv.NextServiceMileage + "\" , \"ModelYear\": \"" + cv.ModelYear + "\"}";

            return JSONresult;


        }

    }
}

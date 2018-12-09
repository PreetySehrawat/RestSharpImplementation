using DealerPro.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Global
{
    class APIMethods
    {
        IRestResponse response;

        public string TriggerAPI(string aPIName, string aPIRequest)
        {
            Global.CommonMethods.ExcelLib.CollectionFromExcel(Global.Base.ExcelPath, "APIDetails");

            string Method = Global.CommonMethods.ExcelLib.ReadExcelData(2, aPIName);
            string APIUrl = Global.CommonMethods.ExcelLib.ReadExcelData(3, aPIName);
            string APIKey = Global.CommonMethods.ExcelLib.ReadExcelData(4, aPIName);



            var client = new RestClient(APIUrl);
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("x-functions-key", APIKey);

            if (Method == "GET")
                request.Method = RestSharp.Method.GET;
            if (Method == "POST")
                request.Method = RestSharp.Method.POST;
            if (Method == "PUT")
                request.Method = RestSharp.Method.PUT;
            if (Method == "DELETE")
                request.Method = RestSharp.Method.DELETE;

            if (!String.IsNullOrEmpty(aPIRequest))
            {
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("undefined", aPIRequest, ParameterType.RequestBody);
            }

            response = client.Execute(request);

            return response.Content;

        }

        public bool CheckAPIRspnsStatus()
        {
            bool IsStatusOK = true;
            if(response.ResponseStatus.Equals("Completed"))
                IsStatusOK = response.StatusCode.Equals("OK");
            return IsStatusOK;
        }

        public bool CheckAPIRspnsContentType()
        {
            bool IsContentType = true;
            if (response.ResponseStatus.Equals("Completed"))
                IsContentType = response.ContentType.Equals("application/json; charset=utf-8")  ;

            return IsContentType;
        }

        public string ReadJson(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string str = r.ReadToEnd();
                return str;
            }
        }

        public void SaveAPIRequest(string aPIName, string aPIRequest)
        {
            string path = Global.Base.JsonRequestPath;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            var fileName = new StringBuilder(path);
            fileName.Append(aPIName);
            fileName.Append(DateTime.Now.ToString("_dd-mm-yyyy_mss"));
            fileName.Append(".json");

            using (var tw = new StreamWriter(fileName.ToString(), true))
            {
                tw.WriteLine(aPIRequest.ToString());
                tw.Close();
            }
        }

        public bool CompareJSonResponse(string expectedRspns , string actualRspns)
        {
            var expectedResponse = JObject.Parse(expectedRspns);
            var actualResponse = JObject.Parse(actualRspns);
            if (JToken.DeepEquals(expectedResponse, actualResponse))
            {
                return true;
            }
            else
                return false;
        }
    }
}

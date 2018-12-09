using Giltrap_DealerPro.Pages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giltrap_DealerPro.APIPages
{
    class CustomerVehicleServices
    {
        public JObject GenerateAPIRequest(CustomerVehicle cv)
        {
                JObject jsonrequest = null;
                FieldInfo[] fields = typeof(CustomerVehicle).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                
                }
                return jsonrequest;
        }

    }
}
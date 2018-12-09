using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Pages
{
    class DealerConfiguration
    {

        public string dealerEmailAddress { set; get; }
        public string dealerName { set; get; }

        public string dealerPhone { set; get; }
        public void GetDealerConfigurationDBData()
        {
            //SQL query to select DealerConfiguration details
            string sql = "select * from DealerConfiguration where DealerId = " + Global.Base.DealerId;

            //Get Data from DB in a collection
            Global.CommonMethods.DBConnection.CollectionFromDB(sql);

            //Populate the dealer Configuration DB data in dbcol1
            Global.CommonMethods.DBConnection.PopulateDealerConfigDBCol();
       
            dealerEmailAddress = Global.CommonMethods.DBConnection.ReadDBData(2, "EmailAddress", 1);
            dealerName = Global.CommonMethods.DBConnection.ReadDBData(2, "Name", 1);
            dealerPhone = Global.CommonMethods.DBConnection.ReadDBData(2, "PhoneNumber", 1);
        }
    }
}

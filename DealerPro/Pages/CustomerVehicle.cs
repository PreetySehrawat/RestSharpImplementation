using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Pages
{
    class CustomerVehicle
    {
        public string VehicleNo { set; get; }
        public string CustomerNo { set; get; }
        public string RooftopId { set; get; }
        public string CommunityId { set; get; }
        public string FirstName { set; get; }
        public string Surname { set; get; }
        public string CustomerEmail { set; get; }
        public string PhoneNumber { set; get; }
        public string MakeCode { set; get; }
        public string ModelYear { set; get; }
        public string ModelCode { set; get; }
        public string ModelDescription { set; get; }
        public string RegistrationNo { set; get; }
        public string VinNumber { set; get; }
        public string LastServiceDate { set; get; }
        public string LastKnownMileage { set; get; }
        public string NextServiceDate { set; get; }
        public string NextServiceMileage { set; get; }
        public string VariantCode { set; get; }

        public void UploadCSVDetails(int rownumber)
        {
            //Populate CommunityId and Rooftop Id from DB
            CommunityId = Global.CommonMethods.DBConnection.ReadDBData(2, "CommunityId");
            RooftopId = Global.CommonMethods.DBConnection.ReadDBData(2, "RooftopId");

            if (Global.Base.DealerId == 2)
            {
                //Customer and Vehicle Details from CSV file
                Global.CommonMethods.ExcelLib.CollectionFromExcel(Global.Base.ExcelPath, "EbbettCSVData");

                VehicleNo = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "VehicleNo");
                CustomerNo = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "CustomerNo");

                FirstName = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "FirstName");
                Surname = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Surname");
                CustomerEmail = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Email");
                PhoneNumber = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "PhoneNumber");
                MakeCode = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "MakeCode");
                ModelYear = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "ModelYear");
                ModelCode = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "ModelCode");
                ModelDescription = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "ModelDescription");
                RegistrationNo = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "RegistrationNo");
                VinNumber = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "VinNumber");
                LastServiceDate = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "LastServiceDate");
                LastKnownMileage = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "LastKnownMileage");
                NextServiceDate = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "NextServiceDate");
                NextServiceMileage = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "NextServiceMileage");
                VariantCode = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "VariantCode");
            }

            if (Global.Base.DealerId == 8)
            {
                //Customer and Vehicle Details from CSV file
                Global.CommonMethods.ExcelLib.CollectionFromExcel(Global.Base.ExcelPath, "DealerProCSVData");

                VehicleNo = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Registration No. (Originating Vehicle) (Vehicle)");
                CustomerNo = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Client ID (Parent Account for lead) (Client)");
                FirstName = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "First Name");
                Surname = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Last Name");
                CustomerEmail = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Email");
                PhoneNumber = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Mobile Phone (Parent Account for lead) (Client)");
                MakeCode = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Brand (Originating Vehicle) (Vehicle)");
                ModelYear = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Model Year (Originating Vehicle) (Vehicle)");
                ModelCode = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Vehicle Model (Originating Vehicle) (Vehicle)");
                ModelDescription = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Model Description (Originating Vehicle) (Vehicle)");
                RegistrationNo = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Registration No. (Originating Vehicle) (Vehicle)");
                VinNumber = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "VIN (Originating Vehicle) (Vehicle)");
                LastServiceDate = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Last Service Notification Date (Originating Vehicle) (Vehicle)");
                LastKnownMileage = "0";
                NextServiceDate = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Next Service Date (Originating Vehicle) (Vehicle)");
                NextServiceMileage = Global.CommonMethods.ExcelLib.ReadExcelData(rownumber, "Next Service KM (Originating Vehicle) (Vehicle)");
                VariantCode = VinNumber;
            }

        }



    }
}

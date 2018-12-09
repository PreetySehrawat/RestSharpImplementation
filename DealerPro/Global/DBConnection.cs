using DealerPro.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Global
{
    class DBConnection
    {

        static SqlConnection sqlConn;

        static string connectionstring = CBSResource.DBConnectionString;


        static List<Datacollection> dbdataCol = new List<Datacollection>();

        public class Datacollection
        {
            public int rowNumber { get; set; }
            public string colName { get; set; }
            public string colValue { get; set; }
        }

        public static void ClearData()
        {
            dbdataCol.Clear();
        }

        public static void CollectionFromDB(string sql)
        {
            try
            {
                int numberofrecords = 0;
                //string connectionstring = @"Data Source=servicebookingdbservtest.database.windows.net;Initial Catalog=ServiceBookingDb;User ID=ExperiecoAdmin;Password=9aJrtENEU6JV3h$";
                using (sqlConn = new SqlConnection(connectionstring))
                {
                    sqlConn.Open();

                    SqlCommand command;
                    SqlDataReader datareader;

                    command = new SqlCommand(sql, sqlConn);
                    datareader = command.ExecuteReader();

                    DBConnection.ClearData();

                    while (datareader.Read())
                    {
                        numberofrecords = numberofrecords + 1;
                        for (int j = 0; j < datareader.FieldCount; j++)
                        {
                            Datacollection dtTable = new Datacollection()
                            {
                                rowNumber = numberofrecords,
                                colName = datareader.GetName(j),
                                colValue = datareader.GetValue(j).ToString()
                            };

                            dbdataCol.Add(dtTable);
                        }

                    }

                    datareader.Close();
                    command.Dispose();

                    if (sqlConn != null) sqlConn.Close();
                }             
            }
            catch (Exception e)
            {
                if (sqlConn != null) sqlConn.Close();
                Console.WriteLine("Exception occurred while connecting to the database!" + Environment.NewLine + e.Message.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        public static string ReadDBData(int rowNumber, string columnName)
        {
            try
            {
                //Retriving Data using LINQ to reduce much of iterations

                rowNumber = rowNumber - 1;
                string data = (from colData in dbdataCol
                               where colData.colName == columnName && colData.rowNumber == rowNumber
                               select colData.colValue).SingleOrDefault();

                //var datas = dataCol.Where(x => x.colName == columnName && x.rowNumber == rowNumber).SingleOrDefault().colValue;


                return data.ToString();
            }

            catch (Exception e)
            {
                //Added by Kumar
                Console.WriteLine("Exception occurred in ExcelLib Class ReadData Method!" + Environment.NewLine + e.Message.ToString());
                return null;
            }
        }

    }
}

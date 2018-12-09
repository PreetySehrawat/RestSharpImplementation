using DealerPro.Config;
using ExcelDataReader;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerPro.Global
{
    class CommonMethods
    {
        #region Database
        public class DBConnection
        {

            static SqlConnection sqlConn;

            static string connectionstring = CSBResources.DBConnectionString;


            static List<Datacollection> dbdataCol = new List<Datacollection>();
            static List<Datacollection> dealerConfigCol = new List<Datacollection>();


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

            //int dc = 1, means dealer configuration db collection
            public static string ReadDBData(int rowNumber, string columnName, int dc = 0)
            {
                try
                {
                    string data = null;

                    //Retriving Data using LINQ to reduce much of iterations

                    rowNumber = rowNumber - 1;

                    if(dc == 1)
                    { 
                            data = (from colData in dealerConfigCol
                                    where colData.colName == columnName && colData.rowNumber == rowNumber
                                   select colData.colValue).SingleOrDefault();
                    }
                    else
                    {
                            data = (from colData in dbdataCol
                                       where colData.colName == columnName && colData.rowNumber == rowNumber
                                       select colData.colValue).SingleOrDefault();
                    }
                    //var datas = dataCol.Where(x => x.colName == columnName && x.rowNumber == rowNumber).SingleOrDefault().colValue;


                    return data.ToString();
                }

                catch (Exception e)
                {
                    Console.WriteLine("Exception occurred in ExcelLib Class ReadData Method!" + Environment.NewLine + e.Message.ToString());
                    return null;
                }
            }


            public static void PopulateDealerConfigDBCol()
            {
                dealerConfigCol = dbdataCol;
            }
        }
        #endregion

        #region Excel
        public class ExcelLib
        {
            static List<Datacollection> dataCol = new List<Datacollection>();

            public class Datacollection
            {
                public int rowNumber { get; set; }
                public string colName { get; set; }
                public string colValue { get; set; }
            }


            public static void ClearData()
            {
                dataCol.Clear();
            }


            private static DataTable ExcelToDataTable(string fileName, string SheetName)
            {
                // Open file and return as Stream
                using (System.IO.FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                    {
                        //To use first row as header
                        DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        //Get all the tables
                        DataTableCollection table = result.Tables;

                        // store it in data table
                        DataTable resultTable = table[SheetName];

                        return resultTable;
                    }
                }
            }

            public static string ReadExcelData(int rowNumber, string columnName)
            {
                try
                {
                    //Retriving Data using LINQ to reduce much of iterations

                    rowNumber = rowNumber - 1;
                    string data = (from colData in dataCol
                                   where colData.colName == columnName && colData.rowNumber == rowNumber
                                   select colData.colValue).SingleOrDefault();

                    //var datas = dataCol.Where(x => x.colName == columnName && x.rowNumber == rowNumber).SingleOrDefault().colValue;


                    return data.ToString();
                }

                catch (Exception e)
                {
                    Console.WriteLine("Exception occurred in ExcelLib Class ReadData Method!" + Environment.NewLine + e.Message.ToString());
                    return null;
                }
            }

            public static void CollectionFromExcel(string fileName, string SheetName)
            {
                ExcelLib.ClearData();
                DataTable table = ExcelToDataTable(fileName, SheetName);

                //Iterate through the rows and columns of the Table
                for (int row = 1; row <= table.Rows.Count; row++)
                {
                    for (int col = 0; col < table.Columns.Count; col++)
                    {
                        Datacollection dtTable = new Datacollection()
                        {
                            rowNumber = row,
                            colName = table.Columns[col].ColumnName,
                            colValue = table.Rows[row - 1][col].ToString()
                        };


                        //Add all the details for each row
                        dataCol.Add(dtTable);

                    }
                }

            }
        }

        #endregion

        #region screenshots
        public class SaveScreenShotClass
        {
            public static string SaveScreenshot(IWebDriver driver, string ScreenShotFileName) // Definition
            {
                var folderLocation = (Base.ScreenshotPath);

                if (!System.IO.Directory.Exists(folderLocation))
                {
                    System.IO.Directory.CreateDirectory(folderLocation);
                }

                var screenShot = ((ITakesScreenshot)driver).GetScreenshot();
                var fileName = new StringBuilder(folderLocation);

                fileName.Append(ScreenShotFileName);
                fileName.Append(DateTime.Now.ToString("_dd-mm-yyyy_mss"));

                fileName.Append(".jpeg");
                screenShot.SaveAsFile(fileName.ToString(), ScreenshotImageFormat.Jpeg);
                return fileName.ToString();
            }
        }
        #endregion

    }
}
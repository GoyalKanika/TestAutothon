using Framework.Reporting;
using OfficeOpenXml;
using System;
using System.IO;

namespace Framework.Utilities
{
    public class Connection
    {
        
      
        private static ExcelPackage package;
        private static ExcelWorksheet worksheet;
        private static int rowCount;
        private static int colCount;

        public Connection()
        {
            
       
        }

        

        public int OpenConnection()
        {
            Constant.CurrentIteration = -1;
            int count = 1;
            //string FilePath = @".\Datatable\RunManager.xlsx";
            package = new ExcelPackage(new FileInfo(BaseClass.FilePath));
            try
            {
                worksheet = package.Workbook.Worksheets[0];
            }
            catch (Exception e)
            {
                BaseClass.ExitTest(true);
            }
            colCount = worksheet.Dimension.End.Column;
            rowCount = worksheet.Dimension.End.Row;
            for (int row = 2; row <= rowCount; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    if (worksheet.Cells[row, 1].Value.ToString().Trim().Equals(Constant.CurrentTestName))
                    {
                        if(Constant.CurrentIteration == -1)
                        {
                            Constant.CurrentIteration = row;
                        }
                        else
                        {
                            count += 1;
                        }
                    }
                }
            }
            if (Constant.CurrentIteration == -1)
            {
                BaseClass.Reporter.Report("Test Case not found : " + Constant.CurrentTestName, "fail", false);
                BaseClass.ExitTest(true);
            }
            return count;
        }

        public  int OpenConnection(string FilePath)
        {
            Constant.CurrentIteration = -1;
            int count = 1;
            //string DatatableFilePath = @".\Datatable\Datatable.xlsx";
            package = new ExcelPackage(new FileInfo(FilePath));
            try
            {
                worksheet = package.Workbook.Worksheets[0];
            }
            catch (Exception e)
            {
                BaseClass.ExitTest(true);
            }
            colCount = worksheet.Dimension.End.Column;
            rowCount = worksheet.Dimension.End.Row;
            for (int row = 2; row <= rowCount; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    if (worksheet.Cells[row, 1].Value.ToString().Trim().Equals(Constant.CurrentTestName))
                    {
                        if (Constant.CurrentIteration == -1)
                        {
                            Constant.CurrentIteration = row;
                        }
                        else
                        {
                            count += 1;
                        }
                    }
                }
            }
            if (Constant.CurrentIteration == -1)
            {
                BaseClass.Reporter.Report("Test Case not found : " + Constant.CurrentTestName, "fail", false);
                BaseClass.ExitTest(true);
            }
            return count;
        }

        public void OpenAPiConnection()
        {
            Constant.CurrentIteration = -1;
            string FilePath = Constant.Projectbasepath + @"\Datatable\Datatable.xlsx";
            package = new ExcelPackage(new FileInfo(FilePath));
            worksheet = package.Workbook.Worksheets[0];
            colCount = worksheet.Dimension.End.Column;
            rowCount = worksheet.Dimension.End.Row;
            for (int row = 2; row <= rowCount; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    if (worksheet.Cells[row, 1].Value.ToString().Trim().Equals(Constant.CurrentTestName))
                    {
                        Constant.CurrentIteration = row;
                        break;
                    }
                }
            }
            if (Constant.CurrentIteration == -1)
            {
                BaseClass.Reporter.Report("Test Case not found : " + Constant.CurrentTestName, "fail", false);
                Reporter.extent.Flush();
                BaseClass.ExitTest(true);
            }
        }

        public int GetIterations()
        {
            int count = 1;
            for (int row = Constant.CurrentIteration + 1; row <= rowCount; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    if (worksheet.Cells[row, 1].Value.ToString().Trim().Equals(Constant.CurrentTestName))
                    {
                        count += 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return count;
        }

        public static string GetData(string column)
        {
            int col = GetColumnNbr(column);
            if(worksheet.Cells[Constant.CurrentIteration, col].Value != null)
            {
                return worksheet.Cells[Constant.CurrentIteration, col].Value.ToString().Trim();
            }
            else
            {
                return "";
            }
        }

        public string GetData(string column,bool exittestifdatanull)
        {
            int col = GetColumnNbr(column);
            if (exittestifdatanull)
            {
                if(worksheet.Cells[Constant.CurrentIteration, col].Value == null)
                {
                    BaseClass.Reporter.Report("Datatable column '" + column + "' has no data", "fail", false, true);                   
                }                
            }
            return worksheet.Cells[Constant.CurrentIteration, col].Value.ToString();
            
        }

        public static int GetColumnNbr(string column)
        {
            int colnbr = -1;
            for (int col = 3; col <= colCount; col++)
            {
                if (worksheet.Cells[1, col].Value != null)
                {
                    if (worksheet.Cells[1, col].Value.ToString().Trim().ToLower().Equals(column.ToLower()))
                    {
                        colnbr = col;
                        break;
                    }
                }
            }
            if(colnbr == -1)
            {
                BaseClass.Reporter.Report("Column not found : " + column, "fail", false);
                Reporter.extent.Flush();
                BaseClass.ExitTest(true);

            }
            return colnbr;
        }

        public void SetData(string columnname, string data)
        {
            int col = GetColumnNbr(columnname);
            worksheet.Cells[Constant.CurrentIteration, col].Value = data;
            BaseClass.Reporter.Report("Datatable updated ------ " + columnname + " : " + data,"pass",false);
           // package.Save();
        }
        public void SetData(string columnname, string appenddata,string delimiter)
        {
            int col = GetColumnNbr(columnname);
            try
            {
                if (worksheet.Cells[Constant.CurrentIteration, col].Value == null)
                {
                    worksheet.Cells[Constant.CurrentIteration, col].Value = appenddata;
                }
                else
                {
                    worksheet.Cells[Constant.CurrentIteration, col].Value = worksheet.Cells[Constant.CurrentIteration, col].Value + delimiter + appenddata;
                }
            }
            catch(Exception e)
            {
                worksheet.Cells[Constant.CurrentIteration, col].Value = appenddata;
            }

            BaseClass.Reporter.Report("Datatable updated ------ " + columnname + " : " + worksheet.Cells[Constant.CurrentIteration, col].Value, "pass", false);
             package.Save();
        }

        public void CloseConnection()
        {
            package.Save();
        }

        public void SaveExcel()
        {
            package.Save();
        }

    }
}

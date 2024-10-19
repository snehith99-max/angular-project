using ems.payroll.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using MySql.Data.MySqlClient;

using static OfficeOpenXml.ExcelErrorValue;
using static System.Collections.Specialized.BitVector32;


namespace ems.payroll.DataAccess
{
    public class DaEmployeeBankDetails
    {
        HttpPostedFile httpPostedFile;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, exemployee_code, lsemployeegid, lsbankname;
        int ErrorCount;


        // Module Master Summary
        public void DaGetEmployeeBankDetailsSummary(MdlPayTrnEmployeeBankDetails values)
        {
            try
            {
                
                msSQL = " Select distinct a.user_gid,c.useraccess, " +
                    " a.user_code,concat(a.user_firstname,a.user_lastname) as empname, " +
                    " d.designation_name,c.designation_gid,c.employee_gid, " +
                    " c.department_gid,c.branch_gid, g.department_name," +
                    " c.pf_no,c.esi_no,c.pan_no,c.uan_no," +
                    " CASE " +
                    " WHEN e.branch_name = 'Vcidex Solutions Pvt Ltd' THEN 'VCX - IN' " +
                    " ELSE e.branch_name " +
                    " END AS branch_name " +
                    " FROM adm_mst_tuser a " +
                    " left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                    " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                    " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                    " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                    " left join hrm_trn_temployeetypedtl h on c.employee_gid=h.employee_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeebankdetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeebankdetails_list
                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            empname = dt["empname"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            pf_no = dt["pf_no"].ToString(),
                            esi_no = dt["esi_no"].ToString(),
                            pan_no = dt["pan_no"].ToString(),
                            uan_no = dt["uan_no"].ToString(),

                        });
                        values.employeebankdetails_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee Bank details!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }


        public void DaPostEmployeeBankDetails(string user_gid, employeebankdetails_list values)
        {
            try
            {
                

                msSQL = "update hrm_mst_temployee set" +
                    " pf_no='" + values.pf_no + "'," +
                   " pan_no = '" + values.pan_no + "'," +
                   " esi_no = '" + values.esi_no + "'," +
                   " bank = '" + values.bank_name + "'," +
                   " uan_no = '" + values.uan_no + "'," +
                   " bank_code  = '" + values.bank_code + "'," +
                   " ac_no = '" + values.ac_no + "'," +
                   " updated_by = '" + user_gid + "'," +
                   " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                   "where employee_gid='" + values.employee_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Bank Details Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Update Bank Details";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while update Bank Details!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }



        public void DaGetBankDtl(MdlPayTrnEmployeeBankDetails values)
        {
            try
            {

                msSQL = " select concat(bank_prefix_code,' ','/',' ',bank_name) as bank_name From acc_mst_tallbank ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBankdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBankdropdown
                        {
                            bank_name = dt["bank_name"].ToString(),
                        });
                        values.GetBankDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank details!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetBankdetails(string employee_gid, MdlPayTrnEmployeeBankDetails values)
        {
            try
            {
                
                msSQL = "select bank,pf_no,esi_no,concat(bank_code,ac_no) as ac_no,pan_no,uan_no from hrm_mst_temployee where employee_gid = '" + employee_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBank>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBank
                        {

                            bank = dt["bank"].ToString(),
                            pf_no = dt["pf_no"].ToString(),
                            esi_no = dt["esi_no"].ToString(),
                            ac_no = dt["ac_no"].ToString(),
                            pan_no = dt["pan_no"].ToString(),
                            uan_no = dt["uan_no"].ToString(),
                            
                        });
                        values.GetBank = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank details!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaBankDtlImport(HttpRequest httpRequest, string user_gid, result1 objResult, employeebankdetails_list values)
        {
            try 
            { 
                        string lscompany_code;
            string excelRange, endRange;
            int rowCount, columnCount;


            try
            {
                int insertCount = 0;
                HttpFileCollection httpFileCollection;
                DataTable dt = null;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"];

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string lsfile_gid = msdocument_gid;
                FileExtension = Path.GetExtension(FileExtension).ToLower();
                lsfile_gid = lsfile_gid + FileExtension;
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                try
                {
                   // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    //using (ExcelPackage xlPackage = new ExcelPackage(ms))
                    //{
                    //    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Customer"];
                    //    rowCount = worksheet.Dimension.End.Row;
                    //    columnCount = worksheet.Dimension.End.Column;
                    //    endRange = worksheet.Dimension.End.Address;
                    //}
                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();
                    
                    objcmnfunctions.uploadFile(lspath, lsfile_gid);
                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.Message.ToString();
                    return;
                }

                //Excel To DataTable
                try
                {
                    DataTable dataTable = new DataTable();
                    int totalSheet = 1;
                    string connectionString = string.Empty;
                    string fileExtension = Path.GetExtension(lspath);

                    lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                    string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");
                    string sheetName;
                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                    }
                    catch (Exception ex)
                    {

                    }

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (schemaTable != null)
                        {
                            var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
                                                 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                 select dataRow).CopyToDataTable();

                            schemaTable = tempDataTable;
                            totalSheet = schemaTable.Rows.Count;
                            using (OleDbCommand command = new OleDbCommand())
                            {
                                // sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Replace("'", "").Trim();
                                command.Connection = connection;
                                command.CommandText = "SELECT * FROM [Sheet1$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                // Insert data into the database
                                //InsertDataIntoDatabase(dt);
                                foreach (DataRow row in dataTable.Rows)
                                {

                                    string exbankname = row["Bank Name"].ToString();
                                    string exbankaccountnumber = row["Bank Account Number"].ToString();
                                    string expfaccountnumber = row["PF Account Number"].ToString();
                                    string exesiaccountnumber = row["ESI Account Number"].ToString();
                                    string exemployeecode = row["Employee Code"].ToString();
                                    string exbankcode = row["Bank Code"].ToString();
                                    string exuannumber = row["UAN Number"].ToString();
                                    string expannumber = row["PAN Number"].ToString();

                                    if (exemployeecode == "")
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("CC");
                                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC' order by finyear asc limit 0,1 ";
                                        string exCode = objdbconn.GetExecuteScalar(msSQL);
                                        exemployee_code = "CC-" + "00" + exCode;

                                    }
                                    else
                                    {
                                        exemployee_code = exemployeecode;

                                    }

                                    msSQL = "select employee_gid from hrm_mst_temployee a " + " left join adm_mst_tuser b on a.user_gid=b.user_gid " + " where b.user_code='" + exemployeecode + "'";
                                    string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                                    if (lsemployee_gid == "")
                                    {
                                        values.message = "Please given the employee name";
                                    }
                                    else
                                    {
                                        lsemployeegid = lsemployee_gid;
                                    }

                                    msSQL = "select bank_name from acc_mst_tallbank where bank_name = '" + exbankname + "'";
                                    string lsbank_name = objdbconn.GetExecuteScalar(msSQL);

                                    if (!(expfaccountnumber.Length <= 22 && expfaccountnumber.All(char.IsDigit)))
                                    {
                                          string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                          msSQL = " insert into pay_trn_tbankuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " 'Invalid PF Number' ," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + exemployeecode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;

                                    }
                                     
                                        if (!(exesiaccountnumber.Length == 10 && exesiaccountnumber.All(char.IsDigit)))
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into pay_trn_tbankuploadexcelerrorlog (" +
                                                      " uploaderrorlog_gid ," +
                                                      " uploadexcellog_gid, " +
                                                      " remarks, " +
                                                      " created_by, " +
                                                      " created_date, " +
                                                      " user_code)" +
                                                      " values(" +
                                                      " '" + MstGid + "'," +
                                                      " '" + msdocument_gid + "'," +
                                                      " 'Invalid PAN Number' ," +
                                                      " '" + user_gid + "'," +
                                                      " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                      "'" + exemployeecode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;

                                        }

                                        if (!(exuannumber.Length == 12 && exuannumber.All(char.IsDigit)))
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into pay_trn_tbankuploadexcelerrorlog (" +
                                                      " uploaderrorlog_gid ," +
                                                      " uploadexcellog_gid, " +
                                                      " remarks, " +
                                                      " created_by, " +
                                                      " created_date, " +
                                                      " user_code)" +
                                                      " values(" +
                                                      " '" + MstGid + "'," +
                                                      " '" + msdocument_gid + "'," +
                                                      " 'Invalid UAN Number' ," +
                                                      " '" + user_gid + "'," +
                                                      " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                      "'" + exemployeecode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;

                                        }


                                        if (!(expannumber.Length <= 10 && expannumber.All(char.IsDigit)))
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into pay_trn_tbankuploadexcelerrorlog (" +
                                                      " uploaderrorlog_gid ," +
                                                      " uploadexcellog_gid, " +
                                                      " remarks, " +
                                                       " created_by, " +
                                                      " created_date, " +
                                                      " user_code)" +
                                                      " values(" +
                                                      " '" + MstGid + "'," +
                                                      " '" + msdocument_gid + "'," +
                                                      " 'Invalid PAN Number' ," +
                                                      " '" + user_gid + "'," +
                                                      " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                      "'" + exemployeecode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;

                                        }


                                        {
                                            msSQL = " update hrm_mst_temployee set" +
                                                " pf_no='" + expfaccountnumber + "'," +
                                                " pan_no = '" + expannumber + "'," +
                                                " esi_no = '" + exesiaccountnumber + "'," +
                                                " ac_no = '" + exbankaccountnumber + "'," +
                                                " bank = '" + exbankname + "'," +
                                                " uan_no = '" + exuannumber + "'," +
                                                " bank_code = '" + exbankcode + "'," +
                                                " updated_by = '" + user_gid + "'," +
                                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " where employee_gid='" + lsemployee_gid + "'  ";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        if (mnResult == 1)
                                        {
                                            values.status = true;
                                            values.message = "Bank Details Updated Successfully";
                                        }
                                        else
                                        {
                                            values.status = false;
                                            values.message = "Error While Update Bank Details";
                                        }

                                    }
                                }
                            }
                        }
                    }
                }


                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.Message.ToString();
                    return;
                }
                //  Nullable<DateTime> ldcodecreation_date;


            }
            catch (Exception ex)
            {
                objResult.status = false;
                objResult.message = ex.Message.ToString();
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Import Bank details!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           


        }
        public void DaGetDocumentDtllist(string document_gid, MdlPayTrnEmployeeBankDetails values)
        {
            try 
            { 
            
                msSQL = " select user_code,remarks from pay_trn_tbankuploadexcelerrorlog " +
                        " where uploadexcellog_gid = '" + document_gid + "'";
            
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<documentdtl_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new documentdtl_list
                    {
                        user_code = dt["user_code"].ToString(),
                        remarks = dt["remarks"].ToString(),
                    });
                    values.documentdtl_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank details!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetDocumentlist(string document_gid, MdlPayTrnEmployeeBankDetails values)
        {
            try { 
            msSQL = " select user_code,remarks,created_by,created_date from pay_trn_tbankuploadexcelerrorlog " +
                    " order by created_date ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<document_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new document_list
                    {
                        user_code = dt["user_code"].ToString(),
                        remarks = dt["remarks"].ToString()
                      
                    });
                    values.document_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank details!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
            

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.system.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using OfficeOpenXml;
using System.Configuration;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using System.Data.OleDb;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace ems.system.DataAccess
{
    public class DaSysMstDesignation
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        HttpPostedFile httpPostedFile;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsdesignation_name;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        int mnResult6, importcount;

        public void DaGetDesignationtSummary(MdlSysMstDesignation values)
        {
            try
            {
                msSQL = "select designation_gid, designation_code, designation_name,a.status as designation_status ," +
                   " case when a.designation_flag = 'Y' then 'Active' when a.designation_flag IS NULL OR a.designation_flag = '' OR a.designation_flag = 'N' then 'InActive' END AS designation_flag, a.designation_flag as designationStatus ," +
                       " status as designation_status ,concat(user_firstname,' ',user_lastname) as created_by," +
                        " date_format(a.created_date,'%d-%b-%Y') as created_date" +
                        " from  adm_mst_tdesignation a" +
                        " left join adm_mst_tuser b on a.created_by = b.user_gid" +
                        " where 1 = 1 order by designation_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Designation_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Designation_list
                        {
                            designation_gid = dt["designation_gid"].ToString(),
                            designation_code = dt["designation_code"].ToString(),
                            designation_status = dt["designation_status"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            designationStatus = dt["designation_flag"].ToString()

                        });
                        values.Designation_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostDesignationAdd(string user_gid, Designation_list values)
        {
            try
            {
                     string designation_name = values.designation_name.Replace("'", "\\'");

                msGetGid1 = objcmnfunctions.GetMasterGID("SDGM");
                // lsdesignation_code = objcmnfunctions.GetMasterGID("DESG");
                if (msGetGid1 == "E")
                {
                    values.status = false;
                    values.message = "Sequence code not generated for Gid";
                    return;

                }
                if (lsdesignation_code == "E")
                {
                    values.status = false;
                    values.message = "Sequence code not generated for Code";
                    return;

                }



                msSQL = "SELECT designation_code FROM adm_mst_tdesignation " +
                       "WHERE LOWER(designation_code) = LOWER('" + values.designation_code + "') " +
                       "OR UPPER(designation_code) = UPPER('" + values.designation_code + "')";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Designation Code already exist";
                    return;
                }


                //msSQL = " select designation_name from adm_mst_tdesignation where designation_name = '" + designation_name + "' ";
                //objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                msSQL = "SELECT designation_name FROM adm_mst_tdesignation " +
                        "WHERE LOWER(designation_name) = LOWER('" + designation_name + "') " +
                        "OR UPPER(designation_name) = UPPER('" + designation_name + "')";

                DataTable dt_datatable3 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable3.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Designation Name already exist";
                    return;
                }

                        msSQL = " insert into adm_mst_tdesignation (" +
                                " designation_gid," +
                                " designation_code," +
                                " designation_name," +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid1 + "'," +
                                " '" + values.designation_code + "'," +
                                "'" + designation_name + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Designation Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Designation";
                        }
                    
                   
                
              
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //public void DaPostDesignationAdd(string user_gid, Designation_list values)
        //{
        //    try
        //    {
        //        string designation_name = values.designation_name.Replace("'", "\\'");

        //        msGetGid1 = objcmnfunctions.GetMasterGID("SDGM");

        //        if (values.Code_Generation == "N")
        //        {
        //            msSQL = "select designation_code from adm_mst_tdesignation  where designation_code = '" + values.designation_code_manual + "'";
        //            DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

        //            if (dt_datatable1.Rows.Count > 0)
        //            {
        //                values.status = false;
        //                values.message = "Designation code already exists";
        //                return;
        //            }

        //            msSQL = "select designation_name from adm_mst_tdesignation  where designation_name = '" + designation_name + "'";
        //            DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

        //            if (dt_datatable2.Rows.Count > 0)
        //            {
        //                values.status = false;
        //                values.message = "Designation name already exists";
        //                return;
        //            }

        //            lsdesignation_code = values.designation_code_manual;
        //        }
        //        else
        //        {
        //            msSQL = "select designation_name from adm_mst_tdesignation  where designation_name = '" + designation_name + "'";
        //            DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

        //            if (dt_datatable2.Rows.Count > 0)
        //            {
        //                values.status = false;
        //                values.message = "Designation name already exists";
        //                return;
        //            }

        //            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='SDGM' order by finyear desc limit 0,1 ";
        //            string lsCode = objdbconn.GetExecuteScalar(msSQL);

        //            lsdesignation_code = "DC" + "000" + lsCode;

        //        }

        //        if(msGetGid1 == "E")
        //        {
        //            values.status = false;
        //            values.message = "Designation gid is not generated";
        //            return;
        //        }
        //        else
        //        {
        //            msSQL = " insert into adm_mst_tdesignation (" +
        //                    " designation_gid," +
        //                    " designation_code," +
        //                    " designation_name," +
        //                    " created_by, " +
        //                    " created_date)" +
        //                    " values(" +
        //                    " '" + msGetGid1 + "'," +
        //                    " '" + lsdesignation_code + "'," +
        //                    " '" + designation_name + "'," +
        //                    " '" + user_gid + "'," +
        //                    " '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
        //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //            if (mnResult != 0)
        //            {
        //                values.status = true;
        //                values.message = "Designation Added Successfully";
        //            }
        //            else
        //            {
        //                values.status = false;
        //                values.message = "Error While Adding Designation";
        //            }
        //        }                
        //    }

        //    catch (Exception ex)
        //    {
        //        values.status = false;

        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        //         "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }
        //}

        public void DaDesignationActivate(string designation_gid, MdlSysMstDesignation values)
        {
            try
            {
                msSQL = "UPDATE adm_mst_tdesignation SET " +
                        "designation_flag = 'Y' " +
                        "WHERE designation_gid = '" + designation_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Designation Active successfully";

                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error while Designation Activated";
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaDesignationInactivate(string designation_gid, MdlSysMstDesignation values)
        {
            try
            {
                msSQL = "UPDATE adm_mst_tdesignation SET " +
                        "designation_flag = 'N' " +
                        "WHERE designation_gid = '" + designation_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Designation Inactive succssfully";
                }
                else
                {
                    {
                        values.status = true;
                        values.message = "Error while Designation Inactivated";
                    }

                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }


        public void DaDeleteDesignation(string designation_gid, Designation_list values)
        {
            try
            {
                msSQL = "  delete from adm_mst_tdesignation where designation_gid='" + designation_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Designation Deleted Successfully";
                }
                else
                {

                    values.status = false;
                    values.message = "Error While Deleting Designation";

                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }





        public void DaPostUpdateDesignation(string user_gid, Designation_list values)
        {
            try
            {
                //msSQL = " select designation_name from adm_mst_tdesignation where designation_name = '" + values.designation_name + "' ";
                //DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable1.Rows.Count > 0)
                //{
                //    values.status = false;
                //    values.message = "Designation name already exist";
                //    return;
                //}
                string designation_name = values.designation_name.Replace("'", "\\'");
                msSQL = " SELECT designation_name  FROM " +
                        " adm_mst_tdesignation WHERE designation_name = '" + designation_name + "' and   designation_gid !='" + values.designation_gid + "' ";

                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Designation Name already exist";
                    return;
                }

                else
                {
                    msSQL = " update  adm_mst_tdesignation set " +
                     " designation_name = '" + designation_name + "'," +
                     " updated_by = '" + user_gid + "'," +
                     " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where designation_gid='" + values.designation_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Designation Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Designation";
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }



        public void DaPostDesignationStatus(string user_gid, Designation_list values)
        {
            try
            {
                msSQL = " update  adm_mst_tdesignation set " +
                 " status = '" + values.designation_status + "'," +
                 " updated_by = '" + user_gid + "'," +
                 " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where designation_gid='" + values.designation_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Designation Status Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Designation Status";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostDesignationImport(HttpRequest httpRequest, string user_gid, result objResult, Designation_list values)
        {
            string lscompany_code, msdocument_gid = "";
            string excelRange, endRange, lstotalshifthours, lshalfdaymaxhours, lshalfdayminhours, lsortminhours, lsotmaxhours;
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
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"] + lscompany_code + "/" + " Import_Excel/Hrm_Module/EmployeeExcels/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
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
                    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();

                    msSQL = " insert into hrm_trn_temployeeuploadexcellog(" +
                            " uploadexcellog_gid," +
                            " fileextenssion," +
                            " uploaded_by, " +
                            " uploaded_date)" +
                            " values(" +
                            " '" + msdocument_gid + "'," +
                            " '" + FileExtension + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.ToString();
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
                                command.Connection = connection;
                                command.CommandText = "select * from [Designation$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                // Upload document
                                // importcount = 0;
                                char[] charsToReplace = { '*', ' ', '/', '@', '$', '#', '!', '^', '%', '(', ')', '\'' };

                                // Get the header names from the CSV file
                                List<string> headers = dataTable.Columns.Cast<DataColumn>().Select(column =>
                                    string.Join("", column.ColumnName.Split(charsToReplace, StringSplitOptions.RemoveEmptyEntries))
                                        .ToLower()).ToList();
                                if (dataTable.Rows.Count == 0)
                                {
                                    objResult.message = "No data found ";
                                    objResult.status = false;
                                    return;
                                }
                                int DesignationCode_index = headers.IndexOf("designationcode");
                                int DesignationName_index = headers.IndexOf("designationname");
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    string DesignationCode = row[DesignationCode_index].ToString();
                                    string DesignationName = row[DesignationName_index].ToString();
                                    string lsdes_code = "", lsdes_name = "";
                                    if (DesignationCode == "" || DesignationCode == null)
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("SDEL");

                                        if (lsdesignation_code == "E")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Sequence code not generated for Code";
                                            continue;

                                        }
                                        msSQL = " insert into adm_mst_tdesignationerrorlog (" +
                                                " designation_gid," +
                                                " designation_code," +
                                                " designation_name," +
                                                " error_remarks," +
                                                " created_by, " +
                                                " created_date)" +
                                                " values(" +
                                                " '" + msGetGid + "'," +
                                                " '" + DesignationCode.Trim() + "'," +
                                                "'" + DesignationName.Trim() + "'," +
                                                "'Designation Code is Empty'," +
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        objResult.message = "Designation Code is Empty";
                                        objResult.status = false;
                                        continue;
                                    }
                                    if (DesignationName == "" || DesignationName == null)
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("SDEL");

                                        if (lsdesignation_code == "E")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Sequence code not generated for Code";
                                            continue;

                                        }
                                        msSQL = " insert into adm_mst_tdesignationerrorlog (" +
                                                " designation_gid," +
                                                " designation_code," +
                                                " designation_name," +
                                                " error_remarks," +
                                                " created_by, " +
                                                " created_date)" +
                                                " values(" +
                                                " '" + msGetGid + "'," +
                                                " '" + DesignationCode.Trim() + "'," +
                                                "'" + DesignationName.Trim() + "'," +
                                                "'Designation Name is Empty'," +
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        objResult.message = "Designation Name is Empty";
                                        objResult.status = false;
                                        continue;
                                    }
                                    msSQL = "select designation_code from adm_mst_tdesignation " +
                                            " where designation_code = '" + DesignationCode.Trim() + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("SDEL");

                                        if (msGetGid == "E")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Sequence code not generated for Code";
                                            continue;

                                        }
                                        msSQL = " insert into adm_mst_tdesignationerrorlog (" +
                                                " designation_gid," +
                                                " designation_code," +
                                                " designation_name," +
                                                " error_remarks," +
                                                " created_by, " +
                                                " created_date)" +
                                                " values(" +
                                                " '" + msGetGid + "'," +
                                                " '" + DesignationCode.Trim() + "'," +
                                                "'" + DesignationName.Trim() + "'," +
                                                "'Duplicate Designation Code'," +
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        objResult.message = "Duplicate Designation Code";
                                        objResult.status = false;
                                        continue;
                                    }
                                    msSQL = "select designation_name from adm_mst_tdesignation " +
                                            " where designation_name = '" + DesignationName.Trim() + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("SDEL");

                                        if (msGetGid == "E")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Sequence code not generated for Code";
                                            continue;

                                        }
                                        msSQL = " insert into adm_mst_tdesignationerrorlog (" +
                                                " designation_gid," +
                                                " designation_code," +
                                                " designation_name," +
                                                " error_remarks," +
                                                " created_by, " +
                                                " created_date)" +
                                                " values(" +
                                                " '" + msGetGid + "'," +
                                                " '" + DesignationCode.Trim() + "'," +
                                                "'" + DesignationName.Trim() + "'," +
                                                "'Duplicate Designation Name'," +
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        objResult.message = "Duplicate Designation Name";
                                        objResult.status = false; return;
                                    }
                                    msGetGid1 = objcmnfunctions.GetMasterGID("SDGM");

                                    if (msGetGid1 == "E")
                                    {
                                        objResult.status = false;
                                        objResult.message = "Sequence code not generated for Code";
                                        continue;

                                    }
                                    msSQL = " insert into adm_mst_tdesignation (" +
                                            " designation_gid," +
                                            " designation_code," +
                                            " designation_name," +
                                            " created_by, " +
                                            " created_date)" +
                                            " values(" +
                                            " '" + msGetGid1 + "'," +
                                            " '" + DesignationCode.Trim() + "'," +
                                            "'" + DesignationName.Trim() + "'," +
                                            "'" + user_gid + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        importcount++;
                                    }
                                }
                                if (mnResult != 0)
                                {
                                    objResult.status = true;
                                    objResult.message = "Designation Added " + importcount + " Out of " + dataTable.Rows.Count + " Successfully";
                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = "Error While Adding Designation";
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = "Error While Adding Designation";
                    return;
                }
            }

            catch (Exception ex)
            {
                objResult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

            msSQL = " update  hrm_trn_temployeeuploadexcellog set " +
                    " importcount = " + importcount + " " +
                    " where uploadexcellog_gid='" + msdocument_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (importcount == 0)
            {
                objResult.status = false;
                objResult.message = "No Designations has been imported so Please check the error log.";
            }
            else
            {
                objResult.status = true;
                objResult.message = objResult.message;
            }
        }
        public void DaGetDesignationtErrorSummary(MdlSysMstDesignation values)
        {
            try
            {
                //msSQL = "select designation_gid, designation_code, designation_name,error_remarks," +
                //        " status as designation_status ,concat(user_firstname,' ',user_lastname) as created_by," +
                //        " date_format(a.created_date,'%d-%b-%Y %H:%i:%s') as created_date" +
                //        " from  adm_mst_tdesignationerrorlog a" +
                //        " left join adm_mst_tuser b on a.created_by = b.user_gid" +
                //        "  order by created_date desc";

                msSQL = "SELECT designation_gid, designation_code, designation_name, error_remarks, status AS designation_status, " +
                        "CONCAT(b.user_firstname, ' ', b.user_lastname) AS created_by, " +
                        "DATE_FORMAT(a.created_date, '%d-%b-%Y %H:%i:%s') AS created_date " +
                        "FROM adm_mst_tdesignationerrorlog a " +
                        "LEFT JOIN adm_mst_tuser b ON a.created_by = b.user_gid " +
                        "ORDER BY a.created_date DESC";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Designation_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Designation_list
                        {
                            designation_gid = dt["designation_gid"].ToString(),
                            designation_code = dt["designation_code"].ToString(),
                            designation_remarks = dt["error_remarks"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString()

                        });
                        values.Designation_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
    }
}
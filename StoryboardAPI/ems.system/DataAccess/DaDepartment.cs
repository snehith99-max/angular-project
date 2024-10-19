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


using global::ems.utilities.Functions;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Web.Http.Results;
using System.Linq.Expressions;

namespace ems.system.DataAccess
{
    public class DaDepartment
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        HttpPostedFile httpPostedFile;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsdepartment_code, lsdepartment_name, lsdepartment_prefix, lsdepartment_name_edit;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        int mnResult6;

        public void DaGetDepartmentSummary(MdlDepartment values)
        {
            try
            {
                msSQL = " select  a.department_gid,a.department_code, a.department_prefix, a.department_name, a.status as department_status ,"+
                        " case when a.status = 'Y' then 'Active' when a.status IS NULL OR a.status = '' OR a.status = 'N' then 'InActive' END AS status,"+ 
                        " concat(c.user_firstname,' ',c.user_lastname) as created_by  ," +
                        " date_format(a.created_date,'%d-%b-%Y') as created_date " +
                        " from hrm_mst_tdepartment a  " +
                        " left join adm_mst_tuser c on a.created_by = c.user_gid " +
                        " order by a.department_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<department_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new department_list
                        {
                            department_gid = dt["department_gid"].ToString(),
                            department_code = dt["department_code"].ToString(),
                            department_prefix = dt["department_prefix"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            // department_manager = dt["department_manager"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            DepartmentStatus = dt["status"].ToString(), 

                        });
                        values.department_list = getModuleList;
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


        public void GetDepartmentAddDropdown(MdlDepartment values)
        {
            try
            {
                msSQL = "select b.employee_gid , concat(a.user_firstname,' ',a.user_lastname) as department_manager from adm_mst_tuser a " +
                    " left join hrm_mst_temployee b on b.user_gid=  a.user_gid  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDepartmentAddDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDepartmentAddDropdown
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            department_manager = dt["department_manager"].ToString(),
                        });
                        values.GetDepartmentAddDropdown = getModuleList;
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

        public void DaPostDepartment(string user_gid, department_list values)
        {
            try
            {
                string department_name = values.department_name.Replace("'", "\\'");

                msSQL = "SELECT department_code FROM hrm_mst_tdepartment " +
                      "WHERE LOWER(department_code) = LOWER('" + values.department_code + "') " +
                      "OR UPPER(department_code) = UPPER('" + values.department_code + "')";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Department Code already exist";
                    return;
                }

                msSQL = "SELECT department_name FROM hrm_mst_tdepartment " +
                     "WHERE LOWER(department_name) = LOWER('" + department_name + "') " +
                     "OR UPPER(department_name) = UPPER('" + department_name + "')";

                DataTable dt_datatable3 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable3.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Department Name already exist";
                    return;
                }

                msSQL = "SELECT department_prefix FROM hrm_mst_tdepartment " +
                    "WHERE LOWER(department_prefix) = LOWER('" + values.department_prefix.Replace("'", "\\'") + "') " +
                    "OR UPPER(department_prefix) = UPPER('" + values.department_prefix.Replace("'", "\\'") + "')";

                DataTable dt_datatable4 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable4.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Department Prefix already exist";
                    return;
                }


                    msGetGid = objcmnfunctions.GetMasterGID("HDPM");

                            msSQL = " insert into hrm_mst_tdepartment(" +
                                    " department_gid," +
                                    " department_code," +
                                    " department_prefix," +
                                    " department_name," +
                                    " created_by, " +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msGetGid + "'," +
                                    " '" + values.department_code + "'," +
                                    "'" + values.department_prefix.Replace("'", "\\'") + "'," +
                                    " '" + department_name + "',";


                            msSQL += "'" + user_gid + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Department Added Successfully";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Adding Department";
                            }
                         }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }



        public void DagetUpdatedDepartment(string user_gid, department_list values)
        {
            try
            {
                string department_name_edit = values.department_name_edit.Replace("'", "\\'");

                //msSQL = " select department_prefix from hrm_mst_tdepartment where department_prefix = '" + values.department_prefix_edit + "' ";
                //DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable1.Rows.Count > 0)
                //{
                //    values.status = false;
                //    values.message = "Department prefix already exists";
                //    return;
                //}

                //msSQL = " select department_name from hrm_mst_tdepartment where department_name = '" + values.department_name_edit + "' ";
                //DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable2.Rows.Count > 0)
                //{
                //    values.status = false;
                //    values.message = "Department name already exists";
                //    return;
                //}

                msSQL = " SELECT department_prefix  FROM " +
                        " hrm_mst_tdepartment WHERE department_prefix = '" + values.department_prefix_edit.Replace("'", "\\'") + "' and   department_gid !='" + values.department_gid + "' ";

                DataTable dt_datatable5 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable5.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Department Prefix already exist";
                    return;
                }


                msSQL = " SELECT department_name  FROM " +
                       " hrm_mst_tdepartment WHERE department_name = '" + department_name_edit + "' and   department_gid !='" + values.department_gid + "' ";

                DataTable dt_datatable6 = objdbconn.GetDataTable(msSQL); 
                if (dt_datatable6.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Department Name already exist";
                    return;
                }

                msSQL = " update  hrm_mst_tdepartment set " +
                             " department_gid = '" + values.department_gid + "'," +
                             " department_code = '" + values.department_code_edit + "'," +
                             " department_prefix = '" + values.department_prefix_edit.Replace("'", "\\'") + "'," +
                             " department_name = '" + department_name_edit + "'," +
                             " updated_by = '" + user_gid + "'," +
                             " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where department_gid='" + values.department_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Department Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Department";
                    }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaDepartmentActivate (string department_gid, MdlDepartment objresult)
        {
            try
            {
                msSQL = "UPDATE hrm_mst_tdepartment SET " +
                        "status = 'Y' " +
                        "WHERE department_gid = '" + department_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    objresult.status = true;
                    objresult.message = "Department Activated Successfully";

                }
                else 
                {
                    {
                        objresult.status = false;
                        objresult.message = "Error while Department Activated";
                    }
                }
            }
            catch (Exception ex)
            {
                objresult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        
        public void DaDepartmentInactivate(string department_gid, MdlDepartment objresult)
        {
            try
            {
                msSQL = "UPDATE hrm_mst_tdepartment SET " +
                        "status = 'N' " +
                        "WHERE department_gid = '" + department_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    objresult.status = true;
                    objresult.message = "Department Inactivated Successfully";
                }
                else
                {
                    {
                        objresult.status = true;
                        objresult.message = "Error while Department Inactivated";
                    }

                }
            }
            catch (Exception ex)
            {
                objresult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        
        public void DaDeleteDepartment(string department_gid, department_list values)
        {
            try
            {
                msSQL = "  delete from hrm_mst_tdepartment where department_gid='" + department_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Department Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Department";
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

        public void DaPostDepartmentImport(HttpRequest httpRequest, string user_gid, result objResult)
        {
            string lscompany_code, msdocument_gid = "";
            string excelRange, endRange, lstotalshifthours, lshalfdaymaxhours, lshalfdayminhours, lsortminhours, lsotmaxhours;
            int rowCount, columnCount, importcount=0;

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
                                command.CommandText = "select * from [Department$]";

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
                                int DepartmentCode_index = headers.IndexOf("departmentcode");
                                int DepartmentName_index = headers.IndexOf("departmentname");
                                int DepartmentPre_index = headers.IndexOf("departmentprefix");
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    if (row.ItemArray.All(item => item == null || item.ToString() == ""))
                                    {
                                        continue;
                                    }

                                    string DepartmentCode = row[DepartmentCode_index].ToString();
                                    string DepartmentName = row[DepartmentName_index].ToString();
                                    string DepartmentPre = row[DepartmentPre_index].ToString();
                                    if(DepartmentCode ==null || DepartmentCode=="")
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("HDDL");

                                        if (msGetGid == "E")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Sequence code not generated for Code";
                                            return;

                                        }
                                        msSQL = " insert into hrm_mst_tdepartmenterrorlog (" +
                                                " department_gid," +
                                                " department_code," +
                                                " department_name," +
                                                " department_prefix," +
                                                " department_remarks," +
                                                " created_by, " +
                                                " created_date)" +
                                                " values(" +
                                                " '" + msGetGid + "'," +
                                                " '" + DepartmentCode.Trim() + "'," +
                                                "'" + DepartmentName.Trim() + "'," +
                                                "'" + DepartmentPre + "'," +
                                                "'DepartmentCode is Empty '," +
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        objResult.message = "Department Code Should Not be Empty";
                                        objResult.status = false;
                                        continue;
                                        
                                    }
                                    if (DepartmentName == null || DepartmentName == "")
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("HDDL");

                                        if (msGetGid == "E")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Sequence code not generated for Code";
                                            return;

                                        }
                                        msSQL = " insert into hrm_mst_tdepartmenterrorlog (" +
                                                " department_gid," +
                                                " department_code," +
                                                " department_name," +
                                                " department_prefix," +
                                                " department_remarks," +
                                                " created_by, " +
                                                " created_date)" +
                                                " values(" +
                                                " '" + msGetGid + "'," +
                                                " '" + DepartmentCode.Trim() + "'," +
                                                "'" + DepartmentName.Trim() + "'," +
                                                "'" + DepartmentPre + "'," +
                                                "'DepartmentName is Empty',"+
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        objResult.message = "Department Name Should Not be Empty";
                                        objResult.status = false;
                                        continue;
                                    }
                                    msSQL = "select department_code from hrm_mst_tdepartment " +
                                            " where department_code = '" + DepartmentCode.Trim() + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("HDDL");

                                        if (msGetGid == "E")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Sequence code not generated for Code";
                                            return;

                                        }
                                        msSQL = " insert into hrm_mst_tdepartmenterrorlog (" +
                                                " department_gid," +
                                                " department_code," +
                                                " department_name," +
                                                " department_prefix," +
                                                " department_remarks," +
                                                " created_by, " +
                                                " created_date)" +
                                                " values(" +
                                                " '" + msGetGid + "'," +
                                                " '" + DepartmentCode.Trim() + "'," +
                                                "'" + DepartmentName.Trim() + "'," +
                                                "'" + DepartmentPre + "'," +
                                                "'Duplicate Department Code'," +
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        objResult.message = "Duplicate Department Code";
                                        objResult.status = false;
                                        continue;
                                    }
                                    msSQL = "select department_name from hrm_mst_tdepartment " +
                                            " where department_name = '" + DepartmentName.Trim() + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("HDDL");

                                        if (msGetGid == "E")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Sequence code not generated for Code";
                                            return;

                                        }
                                        msSQL = " insert into hrm_mst_tdepartmenterrorlog (" +
                                                " department_gid," +
                                                " department_code," +
                                                " department_name," +
                                                " department_prefix," +
                                                " department_remarks," +
                                                " created_by, " +
                                                " created_date)" +
                                                " values(" +
                                                " '" + msGetGid + "'," +
                                                " '" + DepartmentCode.Trim() + "'," +
                                                "'" + DepartmentName.Trim() + "'," +
                                                "'" + DepartmentPre + "'," +
                                                "'Duplicate Department Name'," +
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        objResult.message = "Duplicate Department Name";
                                        objResult.status = false; 
                                        continue;
                                    }
                                    msGetGid = objcmnfunctions.GetMasterGID("HDPM");

                                    if (msGetGid == "E")
                                    {
                                        objResult.status = false;
                                        objResult.message = "Sequence code not generated for Code";
                                        return;

                                    }
                                    msSQL = " insert into hrm_mst_tdepartment (" +
                                            " department_gid," +
                                            " department_code," +
                                            " department_name," +
                                            " department_prefix," +
                                            " created_by, " +
                                            " created_date)" +
                                            " values(" +
                                            " '" + msGetGid + "'," +
                                            " '" + DepartmentCode.Trim() + "'," +
                                            "'" + DepartmentName.Trim() + "'," +
                                            "'" + DepartmentPre + "'," +
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
                                    objResult.message = "Department Added " + importcount + " Out of " + dataTable.Rows.Count + " Successfully";
                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = "Error While Adding Department";
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = "Error While Adding Department";
                    return;
                }
            }

            catch (Exception ex)
            {
                objResult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

            msSQL = " update  hrm_trn_temployeeuploadexcellog set " +
                    " importcount = " + importcount + " " +
                    " where uploadexcellog_gid='" + msdocument_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (importcount == 0)
            {
                ;
                objResult.status = false;
                objResult.message = "No Department has been imported so Please check the error log";
            }
            else
            {
                objResult.status = true;
            }
        }
        public void DaGetDepartmentErrorlogSummary(MdlDepartment values)
        {
            try
            {

                msSQL = " select  a.department_gid,a.department_code, a.department_prefix, a.department_name,  " +
                        " concat(c.user_firstname,' ',c.user_lastname) as created_by  ," +
                        " date_format(a.created_date,'%d-%b-%Y %H:%i:%s') as created_date,department_remarks " +
                        " from hrm_mst_tdepartmenterrorlog a  " +
                        " left join adm_mst_tuser c on a.created_by = c.user_gid " +
                        " order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<department_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new department_list
                        {
                            department_gid = dt["department_gid"].ToString(),
                            department_code = dt["department_code"].ToString(),
                            department_prefix = dt["department_prefix"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            department_remarks = dt["department_remarks"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString()

                        });
                        values.department_list = getModuleList;
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


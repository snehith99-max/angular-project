using System;
using System.Collections.Generic;
using System.Linq;
using ems.hrm.Models;
using ems.utilities.Functions;
using System.Data;
using System.Configuration;
using System.Data.Odbc;

using System.IO;
using System.Web;
using OfficeOpenXml;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Globalization;
using MySql.Data.MySqlClient;
using static OfficeOpenXml.ExcelErrorValue;
using Microsoft.SqlServer.Server;
using System.Windows.Media.Animation;
using Google.Protobuf;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;
using System.Threading;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace ems.hrm.DataAccess
{
    public class DaHrmTrnAttendanceroll
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objMySqlDataReader, objMySqlDataReader1;
        DataTable dt_datatable, objtbl;
        int mnResult, mnResult1, importcount;
        string lsleavegrade_code, lsleavegrade_name, lsattendance_startdate, lsattendance_enddate, lsleavetype_gid, lsleavetype_name, lstotal_leavecount, lsavailable_leavecount, lsleave_limit, lsholidaygrade_gid, lsholiday_gid, lsholiday_date;
        string msUserGid, msEmployeeGID, msBiometricGID, msGetemployeetype, msTemporaryAddressGetGID, msPermanentAddressGetGID, usercode, lsuser_gid, lsemployee_gid, lsuser_code, lscountry_gid2, lscountry_gid, msGetGIDN;
        HttpPostedFile httpPostedFile;
        string lstemcountry_gid, msdocument_gid, lspcountry_gid, lsentity_gid, lsdepartment_gid, lsbranch_gid, uppercasedbvalue, lsdesignation_gid, lsshifttypegid;
        int ErrorCount;
        string shifttype_gid, grace_time, shift_fromhours, shift_tohours, lunchin_hours, lunchout_hours;



        public void DaGetEmployeeAttendanceSummary(string user_gid, MdlHrmTrnAttendanceroll values, string date)
        {
            try
            {
                if (date == null)
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    // Parse and format the date string
                    if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        date = parsedDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        // Handle invalid date format
                        date = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }



                msSQL = " select /*+ MAX_EXECUTION_TIME(300000) */ a.employee_gid,b.user_code,concat(b.user_firstname,' ',b.user_lastname) As empname,f.grace_time as grace," +
                " f.employee_attendance as emp_status," +
                " cast(concat(attendance_date,' ',sec_to_time((time_to_sec(login_time)))) as datetime) as login_time," +
                " cast(concat(attendance_date,' ',sec_to_time((time_to_sec(lunch_in)))) as datetime) as lunch_in," +
                " cast(concat(attendance_date,' ',sec_to_time((time_to_sec(lunch_out)))) as datetime) as lunch_out," +
                " cast(concat(attendance_date,' ',sec_to_time((time_to_sec(logout_time)))) as datetime) as logout_time," +
                " cast(concat(attendance_date,' ',sec_to_time((time_to_sec(OT_in)))) as datetime) as OT_in," +
                " cast(concat(attendance_date,' ',sec_to_time((time_to_sec(OT_out)))) as datetime) as OT_out," +
                " cast(concat(attendance_date,' ',sec_to_time((time_to_sec(OT_duration)))) as datetime) as OT_duration," +
                " (Select p.permission_fromhours from hrm_trn_tpermission p" +
                " where p.employee_gid=a.employee_gid and p.permission_date='" + date + "') as permission_fromhours," +
                " (Select p.permission_frommins from hrm_trn_tpermission p where p.employee_gid=a.employee_gid" +
                " and p.permission_date='" + date + "') as permission_frommins," +
                " (Select p.permission_tohours from hrm_trn_tpermission p where p.employee_gid=a.employee_gid" +
                " and p.permission_date='" + date + "') as permission_tohours," +
                " (Select p.permission_tomins from hrm_trn_tpermission p where p.employee_gid=a.employee_gid" +
                " and p.permission_date='" + date + "') as permission_tomins" +
                " from hrm_mst_temployee a" +
                " inner join adm_mst_tuser b on a.user_gid=b.user_gid" +
                " inner join hrm_trn_tattendance f on a.employee_gid=f.employee_gid" +
                " inner join hrm_trn_temployeetypedtl d on a.employee_gid=d.employee_gid" +
                " left join hrm_mst_tsectionassign2employee i on i.employee_gid=a.employee_gid" +
                " where  f.shift_date='" + date + "'" +
                " and a.attendance_flag='Y' and b.user_status='Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeeattendace_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeeattendace_list1
                        {

                            user_code = dt["user_code"].ToString(),
                            empname = dt["empname"].ToString(),
                            emp_status = dt["emp_status"].ToString(),
                            login_time = dt["login_time"].ToString(),
                            logout_time = dt["logout_time"].ToString(),
                            lunch_in = dt["lunch_in"].ToString(),
                            OT_in = dt["OT_in"].ToString(),
                            lunch_out = dt["lunch_out"].ToString(),
                            OT_out = dt["OT_out"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),

                        });
                        values.employeeattendace_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public result DaAttendanceImports(HttpRequest httpRequest, string user_gid, result objResult, employee_lists values)

        {

            result result = new result();

            try

            {

                HttpFileCollection httpFileCollection;

                HttpPostedFile httpPostedFile;


            

                    HttpContext ctx = HttpContext.Current;

                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>

                    {

                        HttpContext.Current = ctx;

                        DaAttendanceImport(httpRequest, user_gid, objResult, values);

                    }));

                    t.Start();

                

                objResult.status = true;

                objResult.message = " Attendence Imported Inprogress Kindly Check the Error log";

            }

            catch (Exception ex)

            {

                objResult.message = "Error while Sending Message";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;

        }
        public void DaAttendanceImport(HttpRequest httpRequest, string user_gid, result objResult, employee_lists values)
        {
            string lscompany_code, formattedDate, lsotintime, lsotouttime, oT_duration,lstotalduration ,lslogin,lslogout;
            DateTime ot_intime, ot_outtime,login_time,logout_time;
            string excelRange, endRange, lstotalshifthours, lshalfdaymaxhours,lshalfdayminhours,lsortminhours,lsotmaxhours,lsshifttype_gid ;
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

                    msSQL = " insert into hrm_trn_tattendanceuploadexcellog(" +
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
                                command.CommandText = "select * from [Attendance$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                // Upload document
                                importcount = 0;
                                char[] charsToReplace = { '*', ' ', '/', '@', '$', '#', '!', '^', '%', '(', ')', '\'' };

                                // Get the header names from the CSV file
                                List<string> headers = dataTable.Columns.Cast<DataColumn>().Select(column =>
                                    string.Join("", column.ColumnName.Split(charsToReplace, StringSplitOptions.RemoveEmptyEntries))
                                        .ToLower()).ToList();
                                if (dataTable.Rows.Count == 0)
                                {
                                    values.message = "No data found ";
                                    values.status = false;
                                    return;
                                }
                                int employeecode = headers.IndexOf("employeecode");
                                int attendancedate = headers.IndexOf("attendancedate");
                                int shiftname = headers.IndexOf("shiftname");
                                int logintime = headers.IndexOf("logintime");
                                int logouttime = headers.IndexOf("logouttime");

                                int attendancesource = headers.IndexOf("attendancesource");
                                int totalhours = headers.IndexOf("totalhours");
                                int otintime = headers.IndexOf("otintime");
                                int otouttime = headers.IndexOf("otouttime");
                                int otduration = headers.IndexOf("otduration");
                                int attendancetype = headers.IndexOf("attendancetype");


                                foreach (DataRow row in dataTable.Rows)
                                {


                                    string lsattendancetype;
                                    string employee_code = row[employeecode].ToString();
                                    string attendance_date = row[attendancedate].ToString();
                                    DateTime parsedDate;
                                    if (DateTime.TryParse(attendance_date, out parsedDate))
                                    {
                                        // Format the parsed date to "yyyy-MM-dd" format
                                        formattedDate = parsedDate.ToString("yyyy-MM-dd");



                                        string shift_name = row[shiftname].ToString();



                                        string lsattendance = row[attendancesource].ToString();
                                        string attendance_type = row[attendancetype].ToString();

                                        string  lstotalhours =row[totalhours].ToString();

                                       


                                        ErrorCount = 0;
                                        // getting country_gids
                                        {
                                            msSQL = "select b.employee_gid from adm_mst_Tuser a left join hrm_mst_temployee b on a.user_gid=b.user_gid where a.user_code='" + employee_code + "'";
                                            string lsemployeegid = objdbconn.GetExecuteScalar(msSQL);
                                            if(lsemployeegid == null||lsemployeegid=="" )
                                            {
                                                msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");

                                                msSQL = " insert into hrm_trn_tattendanceuploadexcellog(" +
                                                         " uploadexcellog_gid," +
                                                           " fileextenssion," +
                                                           " errorlog," +
                                                           " uploaded_by, " +
                                                         " uploaded_date)" +
                                                          " values(" +
                                                       " '" + msdocument_gid + "'," +
                                                        " '" + FileExtension + "'," +
                                                        " '" + employee_code + "-If the Employee does not exists in Employee Masters'," +
                                                          " '" + user_gid + "'," +
                                                     " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }

                                            //msSQL = "select attendance_type from hrm_trn_Tattendance where employee_attendance='" + lsattendance + "'";
                                            //string lsattendancetype = objdbconn.GetExecuteScalar(msSQL);


                                            msSQL = "select employee_gid from hrm_mst_Temployee where user_gid='" + user_gid + "'";
                                            string ls_employeegid = objdbconn.GetExecuteScalar(msSQL);

                                            msSQL = "select shifttype_gid from hrm_mst_tshifttype where shifttype_name='" + shift_name + "'";
                                            objMySqlDataReader1 = objdbconn.GetDataReader(msSQL);
                                            if (objMySqlDataReader1.HasRows == false)
                                            {

                                                lsshifttypegid = objcmnfunctions.GetMasterGID("HSPM");

                                                msSQL = " insert into hrm_mst_tshifttype (" +
                                               " shifttype_gid, " +
                                              " shifttype_name," +
                                               " grace_time," +
                                                 " email_list," +
                                                 " created_by," +
                                               " created_date )" +
                                                 " values (" +
                                                "'" + lsshifttypegid + "', " +
                                                "'" + shift_name + "'," +
                                                "null," +
                                                "null," +
                                                "'" + user_gid + "'," +
                                                  "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                            }
                                            else
                                            {

                                                lsshifttypegid = objMySqlDataReader1["shifttype_gid"].ToString();
                                            }






                                                msSQL = "select attendance_gid from hrm_trn_tattendance" +
                                                        " where attendance_date='" + formattedDate + "' " +
                                                        " and employee_gid='" + lsemployeegid + "' ";
                                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                                if (objMySqlDataReader.HasRows == false)
                                                {
                                                    string msGetGid = objcmnfunctions.GetMasterGID("HATP");
                                                    msSQL = " insert into hrm_trn_tattendance ( " +
                                                            " attendance_gid, " +
                                                            " employee_gid, " +
                                                            " shift_date, " +
                                                            " attendance_date, " +
                                                            " login_scheduled, " +
                                                            " logout_scheduled, " +
                                                            " day_mode, " +
                                                            " OT_mode, " +
                                                            " employee_attendance, " +
                                                            " attendance_type, " +
                                                            " halfdayabsent_flag," +
                                                            " halfdayleave_flag," +
                                                            " regulate_flag," +
                                                            " attendance_source," +
                                                            " login_time, " +
                                                            " logout_time, " +
                                                            " lunch_out, " +
                                                            " lunch_in , " +
                                                            " created_by, " +
                                                            " shifttype_gid," +
                                                            " created_date) " +
                                                            " values( " +
                                                            "'" + msGetGid + "', " +
                                                            "'" + lsemployeegid + "'," +
                                                            "'" + formattedDate + "'," +
                                                            "'" + formattedDate+ "'," +
                                                            " null," +
                                                            " null," +
                                                            "'S'," +
                                                            "'S'," +
                                                            "'Absent'," +
                                                            "'A'," +
                                                            "'N'," +
                                                            "'N'," +
                                                            "'C'," +
                                                            "'"+ lsattendance + "'," +
                                                            " null," +
                                                            " null," +
                                                            " null," +
                                                            " null," +
                                                            "'" + ls_employeegid + "'," +
                                                            " '" + lsshifttypegid + "'," +
                                                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (row[otintime].ToString() != ""|| row[otouttime].ToString()!="")
                                                    {
                                                        ot_intime = Convert.ToDateTime(row[otintime].ToString());
                                                        lsotintime = formattedDate + " " + ot_intime.ToString("HH:mm:ss");
                                                         ot_outtime = Convert.ToDateTime(row[otouttime].ToString());
                                                        lsotouttime = formattedDate + " " + ot_intime.ToString("HH:mm:ss");

                                                        oT_duration = row[otduration].ToString();
                                                        


                                                        msSQL = " update hrm_trn_Tattendance set" +
                                                               " OT_in='" + lsotintime + "'," +
                                                               " OT_out='" + lsotouttime + "'," +
                                                               " OT_duration='"+ oT_duration+"'," +
                                                               " updated_by='" + user_gid + "'," +
                                                               " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                              " where attendance_date='" + formattedDate + "' " +
                                                               " and employee_gid='" + lsemployeegid + "' ";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    }
                                                    importcount++;
                                                }
                                                else
                                                {
                                                    msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");

                                                    msSQL = " insert into hrm_trn_tattendanceuploadexcellog(" +
                                                             " uploadexcellog_gid," +
                                                               " fileextenssion," +
                                                               " errorlog," +
                                                               " uploaded_by, " +
                                                             " uploaded_date)" +
                                                              " values(" +
                                                           " '" + msdocument_gid + "'," +
                                                            " '" + FileExtension + "'," +
                                                            " '" + employee_code + "-Employee already exists in this attendanceDate '," +
                                                              " '" + user_gid + "'," +
                                                         " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                }


                                                switch (attendance_type)
                                                {
                                                    case "Absent":

                                                        break;

                                                    case "Present":
                                                        if (row[logintime].ToString() == "-" || (row[logintime].ToString()) == ""|| (row[logintime].ToString()) == null)
                                                        {
                                                            msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");

                                                            msSQL = " insert into hrm_trn_tattendanceuploadexcellog(" +
                                                                     " uploadexcellog_gid," +
                                                                       " fileextenssion," +
                                                                       " errorlog," +
                                                                       " uploaded_by, " +
                                                                     " uploaded_date)" +
                                                                      " values(" +
                                                                   " '" + msdocument_gid + "'," +
                                                                    " '" + FileExtension + "'," +
                                                                    " '"+ employee_code + "'If the Prsent Kindly Give the login time'," +
                                                                      " '" + user_gid + "'," +
                                                                 " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                        }
                                                        else if (row[logouttime].ToString() == "-" || (row[logouttime].ToString()) == "" || (row[logouttime].ToString()) == null)
                                                    {
                                                        msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");

                                                        msSQL = " insert into hrm_trn_tattendanceuploadexcellog(" +
                                                                 " uploadexcellog_gid," +
                                                                   " fileextenssion," +
                                                                   " errorlog," +
                                                                   " uploaded_by, " +
                                                                 " uploaded_date)" +
                                                                  " values(" +
                                                               " '" + msdocument_gid + "'," +
                                                                " '" + FileExtension + "'," +
                                                                " '" + employee_code + "-If the Prsent Kindly Give the logout time'," +
                                                                  " '" + user_gid + "'," +
                                                             " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                                    }
                                                    else
                                                        {

                                                        login_time = Convert.ToDateTime(row[logintime].ToString());
                                                        lslogin = formattedDate + " " + login_time.ToString("HH:mm:ss");

                                                         logout_time = Convert.ToDateTime(row[logouttime].ToString());
                                                        lslogout= formattedDate + " " + logout_time.ToString("HH:mm:ss");
                                                            msSQL = " update hrm_trn_Tattendance set" +
                                                             " login_time='" + lslogin + "'," +
                                                             " logout_time='" + lslogout + "'," +
                                                             " attendance_type='P'," +
                                                             " employee_attendance='"+attendance_type+"'," +
                                                             " working_hours='" + lstotalhours + "'," +
                                                             " updated_by='" + user_gid + "'," +
                                                             " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                            " where attendance_date='" + formattedDate + "' " +
                                                             " and employee_gid='" + lsemployeegid + "' ";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                           

                                                        }

                                                        // Splitting the time string into hours, minutes, and seconds
                                                        //string[] timeComponents = lstotal_hours.Split(':');
                                                        //int hours = int.Parse(timeComponents[0]);
                                                        //int minutes = int.Parse(timeComponents[1]);
                                                        //int seconds = int.Parse(timeComponents[2]);
                                                        //int totalHours = hours;
                                                        //string lshalfday_minhours = lshalfdaymin_hours.ToString("HH:mm:ss");

                                                        //// Splitting the time string into hours, minutes, and seconds
                                                        //string[] timeComponents1 = lshalfday_minhours.Split(':');
                                                        //int hours1 = int.Parse(timeComponents1[0]);

                                                        //int halfdayHours = hours1;


                                                        //// Calculating the total number of hours
                                                        //if (totalHours <= halfdayHours)
                                                        //{
                                                        //    msSQL = " update hrm_trn_Tattendance set" +
                                                        //           "  set halfdayabsent_flag='Y'" +
                                                        //            " where attendance_date='" + attendance_date.ToString("yyyy-MM-dd") + "' " +
                                                        //            " and employee_gid='" + lsemployeegid + "' ";
                                                        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        //}
                                                        break;
                                                    case "Holiday":
                                                        msSQL = " update hrm_trn_Tattendance set" +
                                                            " login_time=null," +
                                                            " logout_time=null," +
                                                            " attendance_type='NH'," +
                                                            " employee_attendance='Holiday'," +
                                                            " updated_by='" + user_gid + "'," +
                                                            " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                           " where attendance_date='" + formattedDate + "' " +
                                                            " and employee_gid='" + lsemployeegid + "' ";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        break;
                                                    case "Leave":
                                                        msSQL = " update hrm_trn_Tattendance set" +
                                                            " login_time=null," +
                                                            " logout_time=null," +
                                                            " attendance_type='Leave'," +
                                                            " employee_attendance='Leave'," +
                                                            " updated_by='" + user_gid + "'," +
                                                            " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                           " where attendance_date='" +formattedDate + "' " +
                                                            " and employee_gid='" + lsemployeegid + "' ";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        string msGetGID2 = objcmnfunctions.GetMasterGID("HLVP");

                                                        msSQL = " insert into hrm_trn_tleave " +
                                                                " ( leave_gid,  " +
                                                                " employee_gid , " +
                                                                " leave_applydate , " +
                                                                " leave_fromdate, " +
                                                                " leave_todate , " +
                                                                " leave_noofdays , " +
                                                                " leave_status ," +
                                                                " created_by, " +
                                                                " created_date) " +
                                                                " values ( " +
                                                                " '" + msGetGID2 + "', " +
                                                                " '" + lsemployeegid + "', " +
                                                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                                                " '" + formattedDate + "'," +
                                                                " '" + formattedDate + "', " +
                                                                " '1'," +
                                                                " 'Direct Approval'," +
                                                                " '" + user_gid + "'," +
                                                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                        break;
                                                    case "Weekoff":
                                                        msSQL = " update hrm_trn_Tattendance set" +
                                                            " login_time=null," +
                                                            " logout_time=null," +
                                                            " attendance_type='WH'," +
                                                            " employee_attendance='Weekoff'," +
                                                            " updated_by='" + user_gid + "'," +
                                                            " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                            " where attendance_date='" + formattedDate + "' " +
                                                            " and employee_gid='" + lsemployeegid + "' ";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        break;


                                                }
                                            











                                        }
                                    }
                                }
                            }
                            }
                    }
                    //msSQL = "select  totalshift_hours,halfmax_hours ,halfmin_hours ,otminhours,otmaxhours from hrm_mst_tattendanceconfig ";
                    //objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    //if (objMySqlDataReader.HasRows)
                    //{
                    //    objMySqlDataReader.Read();
                    //    lstotalshifthours = objMySqlDataReader["totalshift_hours"].ToString();
                    //    DateTime lstotalshift_hours = Convert.ToDateTime(objMySqlDataReader["totalshift_hours"].ToString());
                    //    lshalfdaymaxhours = objMySqlDataReader["halfmax_hours"].ToString();
                    //    lshalfdayminhours = objMySqlDataReader["halfmin_hours"].ToString();
                    //    DateTime lshalfdaymin_hours = Convert.ToDateTime(objMySqlDataReader["halfmin_hours"].ToString());

                    //    lsotmaxhours = objMySqlDataReader["otminhours"].ToString();
                    //    lsortminhours = objMySqlDataReader["otmaxhours"].ToString();
                    //    objMySqlDataReader.Close();
                    //}

                    }
                catch (Exception ex)
                 {
                    msSQL = " update  hrm_trn_tattendanceuploadexcellog set " +
                           " importcount = " + importcount + ", " +
                           " errorlog =' " + ex.Message.ToString()+ "' " +
                           " where uploadexcellog_gid='" + msdocument_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    objResult.status = false;
                    objResult.message = ex.ToString();
                    return;
                }
            }

            catch (Exception ex)
            {
                msSQL = " update  hrm_trn_tattendanceuploadexcellog set " +
                             " importcount = " + importcount + ", " +
                             " errorlog =' " + ex.Message.ToString()  + "' " +
                             " where uploadexcellog_gid='" + msdocument_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
             
            }

            msSQL = " update  hrm_trn_tattendanceuploadexcellog set " +
                    " importcount = " + importcount + ", " +
                    " errorlog = '" + importcount + "-Attendance imported successfully' " +
                    " where uploadexcellog_gid='" + msdocument_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (importcount == 0)
            {
                objResult.status = false;
                objResult.message = "No Attendance data has been imported so Please check the error log.";
             
            }
            else
            {
                objResult.status = true;
                objResult.message = importcount + "  Attendance data has been imported";
            }
        }

        public void DaGetBranch(MdlHrmTrnAttendanceroll values)
        {
            try
            {

                msSQL = " Select branch_name,branch_gid  " +
                    " from hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranch1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranch1
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetBranch1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetAttendnanceErorlogSummaryy(string user_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select date(a.uploaded_date) as uploaddate,a.errorlog,a.importcount, " +
                          " concat(c.user_firstname,' ',c.user_lastname) as created_by,a.uploaded_date   " +
                          " from hrm_trn_tattendanceuploadexcellog  a " +
                          " left  join adm_mst_Tuser c on a.uploaded_by=c.user_gid " +
                         " where 1=1";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employee_list10>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employee_list10
                        {
                            user_code = dt["user_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            remarks = dt["remarks"].ToString(),

                        });
                        values.employee_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void GetDepartment(string branch_gid, MdlHrmTrnAttendanceroll values)
        {
            try
            {

                if (branch_gid != "all")
                {
                    msSQL = "select distinct a.department_gid,a.department_name from hrm_mst_tdepartment a " +
                            "inner join hrm_mst_temployee b on a.department_gid = b.department_gid " +
                            "inner join hrm_mst_tbranch c on b.branch_gid = c.branch_gid " +
                            "where c.branch_gid ='" + branch_gid + "' ";
                }
                else
                {
                    msSQL = "select distinct a.department_gid,a.department_name from hrm_mst_tdepartment a " +
                            "inner join hrm_mst_temployee b on a.department_gid = b.department_gid " +
                            "inner join hrm_mst_tbranch c on b.branch_gid = c.branch_gid ";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDepartment1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDepartment1
                        {
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.GetDepartment1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Updatedtl(update_lists values, string user_gid)
        {
            string shiftdate="";
             int f = 0;
            try
            {
                msSQL = " SELECT distinct a.employee2shifttypedtl_gid,a.employee_gid,a.shifttype_gid, " +
                               " cast(concat('00:',c.grace_time,':00')as char) as grace_time, " +
                               " concat(a.shift_fromhours, ':',a.shift_fromminutes,':','00') as shift_fromhours," +
                               " concat(a.shift_tohours, ':',a.shift_tominutes,':','00') as shift_tohours, " +
                               " concat(a.lunchin_hours, ':',a.lunchin_minutes,':','00') as lunchin_hours, " +
                               " concat(a.lunchout_hours, ':',a.lunchout_minutes,':','00') as lunchout_hours " +
                               " from hrm_trn_temployee2shifttypedtl a " +
                               " left join hrm_trn_temployee2shifttype b on a.shifttype_gid= b.shifttype_gid " +
                               " left join hrm_mst_tshifttype c on c.shifttype_gid = a.shifttype_gid " +
                               " where a.employee_gid='" + values.employee_gid + "' and b.shift_status='Y' " +
                               " and a.employee2shifttype_name='monday'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    objMySqlDataReader.Read();
                     shifttype_gid = objMySqlDataReader["shifttype_gid"].ToString();
                     grace_time = objMySqlDataReader["grace_time"].ToString();
                     shift_fromhours = objMySqlDataReader["shift_fromhours"].ToString();
                     shift_tohours = objMySqlDataReader["shift_tohours"].ToString();
                     lunchin_hours = objMySqlDataReader["lunchin_hours"].ToString();
                     lunchout_hours = objMySqlDataReader["lunchout_hours"].ToString();
                    objMySqlDataReader.Close();
                }
                if (DateTime.TryParseExact(values.date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    shiftdate = parsedDate.ToString("yyyy-MM-dd");
                }
                msSQL = " select login_time,logout_time,employee_gid from hrm_trn_tattendance where employee_gid='" + values.employee_gid + "'" +
                        " and shift_date='" + shiftdate + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    string login_time = objMySqlDataReader["login_time"].ToString();
                    string logout_time = objMySqlDataReader["logout_time"].ToString();
                    if (string.IsNullOrEmpty(login_time))
                    {
                        login_time = $"{shiftdate} 09:00:00";
                    }

                    if (string.IsNullOrEmpty(logout_time))
                    {
                        logout_time = $"{shiftdate} 18:00:00";
                    }

                    string newLoginTime = values.login_time; // Replace with actual input                   
                    string newLogoutTime = values.logout_time; // Replace with actual input
                    string updatedLogoutTime,updatedLoginTime;
                    if(newLoginTime == "NaN:NaN:NaN") { newLoginTime = ""; }
                    if(newLogoutTime == "NaN:NaN:NaN") { newLogoutTime =""; } 
                    if (newLoginTime != "")
                    { 
                        DateTime updatedLoginDateTime = UpdateTime(DateTime.Parse(login_time), newLoginTime);
                         updatedLoginTime = updatedLoginDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else {  updatedLoginTime = null; }

                    if (newLogoutTime != "")
                    {
                        DateTime updatedLogoutDateTime = UpdateTime(DateTime.Parse(logout_time), newLogoutTime);
                         updatedLogoutTime = updatedLogoutDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else {  updatedLogoutTime = null; }
                    
                  if(updatedLoginTime!=null || updatedLogoutTime != null)
                    { 
                    msSQL = "update hrm_trn_tattendance set " +
                          " employee_attendance='Present',attendance_type = 'P'," +
                          " regulate_flag='C'," +
                          " update_flag='N', " +
                          " attendance_source='Manual',";
                          if(updatedLoginTime != null)
                          {
                          msSQL += " login_time='" + updatedLoginTime + "', ";
                          }
                         if (updatedLogoutTime != null)
                         {
                          msSQL += " logout_time='" + updatedLogoutTime + "', ";
                         }
                  msSQL +=" updated_by='" + user_gid + "', " +
                          " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                          " where shift_date='" + shiftdate + "' " +
                          " and employee_gid='" + values.employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                    else
                    {
                        f = 1;
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Attendance Updated successfully";
                    }
                    else if (f == 1)
                    {
                        values.status = false;
                        values.message = "No attendance is available on that day for this employee";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While Updating Attendance";
                    }

                }
                else
                {
                    string newLoginTime = values.login_time; // Replace with actual input                   
                    string newLogoutTime = values.logout_time; // Replace with actual input
                    if(newLoginTime != "NaN:NaN:NaN") {
                        newLoginTime = newLoginTime;
                        if(newLogoutTime != "NaN:NaN:NaN") { newLogoutTime = newLogoutTime; }
                    
                    msGetGIDN = objcmnfunctions.GetMasterGID("HATP");
                        msSQL = "Insert Into hrm_trn_tattendance" +
                                           "(attendance_gid," +
                                           " employee_gid," +
                                           " attendance_date," +
                                           " shift_date, " +
                                           " login_scheduled, " +
                                           " logout_scheduled, " +
                                           " lunch_in_scheduled, " +
                                           " lunch_out_scheduled, " +
                                           " grace_time, " +
                                           " day_mode, " +
                                           " OT_mode, " +
                                           " attendance_source," +
                                           " login_time,";
                        if (newLogoutTime != "NaN:NaN:NaN") { msSQL += " logout_time,"; }
                        msSQL += " login_time_audit," +
                              " employee_attendance," +
                              " regulate_flag, " +
                              " attendance_source, " +
                              " created_by," +
                              " created_date," +
                              " attendance_type)" +
                              " VALUES ( " +
                              "'" + msGetGIDN + "', " +
                              "'" + values.employee_gid + "'," +
                              "'" + shiftdate + "'," +
                              "'" + shiftdate + "'," +
                              "'" + shift_fromhours + "'," +
                              "'" + shift_tohours + "'," +
                              "'" + lunchin_hours + "'," +
                              "'" + lunchout_hours + "'," +
                              "'" + grace_time + "'," +
                              "'S'," +
                              "'S'," +
                              "'Manual'," +
                              "'" + shiftdate + " " + newLoginTime + "',";
                            if (newLogoutTime != "NaN:NaN:NaN")
                              { 
                         msSQL += "'" + shiftdate + " " + newLogoutTime + "',";
                              }
                    msSQL += "'" + shiftdate + " " + newLoginTime + "'," +
                             "'Present'," +
                             "'C'," +
                             "'Attendance Regulation'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                             "'P')";



                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Attendance Updated successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occured While Updating Attendance";
                        }
                    }
                }
                
                



                }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }



        DateTime UpdateTime(DateTime originalDateTime, string newTime)
        {
            TimeSpan newTimeSpan = TimeSpan.Parse(newTime);
            return new DateTime(
                originalDateTime.Year,
                originalDateTime.Month,
                originalDateTime.Day,
                newTimeSpan.Hours,
                newTimeSpan.Minutes,
                newTimeSpan.Seconds
            );
        }

        public void Dacleardtl(update_lists values, string user_gid)
        {
            string shiftdate = "";
            try
            {
                if (DateTime.TryParseExact(values.date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    shiftdate = parsedDate.ToString("yyyy-MM-dd");
                }

                msSQL = " update hrm_trn_tattendance set login_time=null,logout_time=null,total_duration=null,login_ip=null,logout_ip=null,lunch_out=null,lunch_in=null,OT_in=null,OT_out=null,OT_duration=null, " +
                   " login_time_audit=null,logout_time_audit=null,lunch_out_audit=null,lunch_in_audit=null,employee_attendance='Absent',attendance_type='A',update_flag='N' ,regulate_flag='C'," +
                   " updated_by='" + user_gid + "', " +
                   " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                   " where employee_gid='" + values.employee_gid + "' and shift_date='" + shiftdate + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Attendance cleared successfully";
                }
               
                else
                {
                    values.status = false;
                    values.message = "No Attendance Available on that Day for this employee";
                }


            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Exception  While clearing Attendance";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaUpdatepunchindtl(punchupdatedtl values, string user_gid)
        {
            int count = 0;
            string shiftdate = "";
            try
            {
                foreach (var data in values.update_lists)
                {
                    int f = 0;
                    msSQL = " SELECT distinct a.employee2shifttypedtl_gid,a.employee_gid,a.shifttype_gid, " +
                              " cast(concat('00:',c.grace_time,':00')as char) as grace_time, " +
                              " concat(a.shift_fromhours, ':',a.shift_fromminutes,':','00') as shift_fromhours," +
                              " concat(a.shift_tohours, ':',a.shift_tominutes,':','00') as shift_tohours, " +
                              " concat(a.lunchin_hours, ':',a.lunchin_minutes,':','00') as lunchin_hours, " +
                              " concat(a.lunchout_hours, ':',a.lunchout_minutes,':','00') as lunchout_hours " +
                              " from hrm_trn_temployee2shifttypedtl a " +
                              " left join hrm_trn_temployee2shifttype b on a.shifttype_gid= b.shifttype_gid " +
                              " left join hrm_mst_tshifttype c on c.shifttype_gid = a.shifttype_gid " +
                              " where a.employee_gid='" + data.employee_gid + "' and b.shift_status='Y' " +
                              " and a.employee2shifttype_name='monday'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                    if (objMySqlDataReader.HasRows == true)
                    {
                        objMySqlDataReader.Read();
                        shifttype_gid = objMySqlDataReader["shifttype_gid"].ToString();
                        grace_time = objMySqlDataReader["grace_time"].ToString();
                        shift_fromhours = objMySqlDataReader["shift_fromhours"].ToString();
                        shift_tohours = objMySqlDataReader["shift_tohours"].ToString();
                        lunchin_hours = objMySqlDataReader["lunchin_hours"].ToString();
                        lunchout_hours = objMySqlDataReader["lunchout_hours"].ToString();
                        objMySqlDataReader.Close();
                    }
                    if (DateTime.TryParseExact(values.date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        shiftdate = parsedDate.ToString("yyyy-MM-dd");
                    }
                    msSQL = " select login_time,logout_time,employee_gid from hrm_trn_tattendance where employee_gid='" + data.employee_gid + "'" +
                            " and shift_date='" + shiftdate + "' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        string login_time = objMySqlDataReader["login_time"].ToString();
                        if (string.IsNullOrEmpty(login_time))
                        {
                            login_time = $"{shiftdate} 09:00:00";
                        }
                        string newLoginTime = values.punchin_time; // Replace with actual input
                        string updatedLoginTime;
                        if (newLoginTime == "NaN:NaN:NaN") { newLoginTime = ""; }
                        if (newLoginTime != "")
                        {
                            DateTime updatedLoginDateTime = UpdateTime(DateTime.Parse(login_time), newLoginTime);
                            updatedLoginTime = updatedLoginDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else { updatedLoginTime = null; }
                        msSQL = "update hrm_trn_tattendance set " +
                                " employee_attendance='Present',attendance_type = 'P'," +
                                " regulate_flag='C'," +
                                " update_flag='N', " +
                                " attendance_source='Manual',";
                        msSQL += " login_time='" + updatedLoginTime + "', ";
                        msSQL += " updated_by='" + user_gid + "', " +
                                 " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                 " where shift_date='" + shiftdate + "' " +
                                 " and employee_gid='" + data.employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1) { count++; }

                    }
                
                    else
                    {
                        string newLoginTime = values.punchin_time; // Replace with actual input    
                        if (newLoginTime != "NaN:NaN:NaN")
                        {
                            newLoginTime = newLoginTime;

                            msGetGIDN = objcmnfunctions.GetMasterGID("HATP");
                            msSQL = "Insert Into hrm_trn_tattendance" +
                                               "(attendance_gid," +
                                               " employee_gid," +
                                               " attendance_date," +
                                               " shift_date, " +
                                               " login_scheduled, " +
                                               " logout_scheduled, " +
                                               " lunch_in_scheduled, " +
                                               " lunch_out_scheduled, " +
                                               " grace_time, " +
                                               " day_mode, " +
                                               " OT_mode, " +
                                               " attendance_source," +
                                               " login_time,";
                            msSQL += " login_time_audit," +
                                  " employee_attendance," +
                                  " regulate_flag, " +
                                  " attendance_source, " +
                                  " created_by," +
                                  " created_date," +
                                  " attendance_type)" +
                                  " VALUES ( " +
                                  "'" + msGetGIDN + "', " +
                                  "'" + data.employee_gid + "'," +
                                  "'" + shiftdate + "'," +
                                  "'" + shiftdate + "'," +
                                  "'" + shift_fromhours + "'," +
                                  "'" + shift_tohours + "'," +
                                  "'" + lunchin_hours + "'," +
                                  "'" + lunchout_hours + "'," +
                                  "'" + grace_time + "'," +
                                  "'S'," +
                                  "'S'," +
                                  "'Manual'," +
                                  "'" + shiftdate + " " + newLoginTime + "',";                          
                            msSQL += "'" + shiftdate + " " + newLoginTime + "'," +
                                     "'Present'," +
                                     "'C'," +
                                     "'Attendance Regulation'," +
                                     "'" + user_gid + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                     "'P')";



                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1) { count++; }

                        }
                    }


                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = count+" Attendance Updated successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Updating Attendance";
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaUpdatepunchoutdtl(punchupdatedtl values, string user_gid)
        {
            int count = 0;
            string shiftdate = "";
            try
            {
                foreach (var data in values.update_lists)
                {
                    int f = 0;
                    msSQL = " SELECT distinct a.employee2shifttypedtl_gid,a.employee_gid,a.shifttype_gid, " +
                              " cast(concat('00:',c.grace_time,':00')as char) as grace_time, " +
                              " concat(a.shift_fromhours, ':',a.shift_fromminutes,':','00') as shift_fromhours," +
                              " concat(a.shift_tohours, ':',a.shift_tominutes,':','00') as shift_tohours, " +
                              " concat(a.lunchin_hours, ':',a.lunchin_minutes,':','00') as lunchin_hours, " +
                              " concat(a.lunchout_hours, ':',a.lunchout_minutes,':','00') as lunchout_hours " +
                              " from hrm_trn_temployee2shifttypedtl a " +
                              " left join hrm_trn_temployee2shifttype b on a.shifttype_gid= b.shifttype_gid " +
                              " left join hrm_mst_tshifttype c on c.shifttype_gid = a.shifttype_gid " +
                              " where a.employee_gid='" + data.employee_gid + "' and b.shift_status='Y' " +
                              " and a.employee2shifttype_name='monday'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                    if (objMySqlDataReader.HasRows == true)
                    {
                        objMySqlDataReader.Read();
                        shifttype_gid = objMySqlDataReader["shifttype_gid"].ToString();
                        grace_time = objMySqlDataReader["grace_time"].ToString();
                        shift_fromhours = objMySqlDataReader["shift_fromhours"].ToString();
                        shift_tohours = objMySqlDataReader["shift_tohours"].ToString();
                        lunchin_hours = objMySqlDataReader["lunchin_hours"].ToString();
                        lunchout_hours = objMySqlDataReader["lunchout_hours"].ToString();
                        objMySqlDataReader.Close();
                    }
                    if (DateTime.TryParseExact(values.date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        shiftdate = parsedDate.ToString("yyyy-MM-dd");
                    }
                    msSQL = " select login_time,logout_time,employee_gid from hrm_trn_tattendance where employee_gid='" + data.employee_gid + "'" +
                            " and shift_date='" + shiftdate + "' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        string logout_time = objMySqlDataReader["login_time"].ToString();
                        if (string.IsNullOrEmpty(logout_time))
                        {
                            logout_time = $"{shiftdate} 18:00:00";
                        }
                        string newLogoutTime = values.punchout_time; // Replace with actual input
                        string updatedLogoutTime;
                        if (newLogoutTime == "NaN:NaN:NaN") { newLogoutTime = ""; }
                        if (newLogoutTime != "")
                        {
                            DateTime updatedLogoutDateTime = UpdateTime(DateTime.Parse(logout_time), newLogoutTime);
                            updatedLogoutTime = updatedLogoutDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else { updatedLogoutTime = null; }
                        msSQL = "update hrm_trn_tattendance set " +
                                " employee_attendance='Present',attendance_type = 'P'," +
                                " regulate_flag='C'," +
                                " update_flag='N', " +
                                " attendance_source='Manual',";
                        msSQL += " logout_time='" + updatedLogoutTime + "', ";
                        msSQL += " updated_by='" + user_gid + "', " +
                                 " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                 " where shift_date='" + shiftdate + "' " +
                                 " and employee_gid='" + data.employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1) { count++; }

                    }

                    else
                    {
                        string newLogoutTime = values.punchout_time; // Replace with actual input    
                        if (newLogoutTime != "NaN:NaN:NaN")
                        {
                            newLogoutTime = newLogoutTime ;

                            msGetGIDN = objcmnfunctions.GetMasterGID("HATP");
                            msSQL = "Insert Into hrm_trn_tattendance" +
                                               "(attendance_gid," +
                                               " employee_gid," +
                                               " attendance_date," +
                                               " shift_date, " +
                                               " login_scheduled, " +
                                               " logout_scheduled, " +
                                               " lunch_in_scheduled, " +
                                               " lunch_out_scheduled, " +
                                               " grace_time, " +
                                               " day_mode, " +
                                               " OT_mode, " +
                                               " attendance_source," +
                                               " logout_time,";
                            msSQL += " login_time_audit," +
                                  " employee_attendance," +
                                  " regulate_flag, " +
                                  " attendance_source, " +
                                  " created_by," +
                                  " created_date," +
                                  " attendance_type)" +
                                  " VALUES ( " +
                                  "'" + msGetGIDN + "', " +
                                  "'" + data.employee_gid + "'," +
                                  "'" + shiftdate + "'," +
                                  "'" + shiftdate + "'," +
                                  "'" + shift_fromhours + "'," +
                                  "'" + shift_tohours + "'," +
                                  "'" + lunchin_hours + "'," +
                                  "'" + lunchout_hours + "'," +
                                  "'" + grace_time + "'," +
                                  "'S'," +
                                  "'S'," +
                                  "'Manual'," +
                                  "'" + shiftdate + " " + newLogoutTime + "',";
                            msSQL += "'" + shiftdate + " " + newLogoutTime + "'," +
                                     "'Present'," +
                                     "'C'," +
                                     "'Attendance Regulation'," +
                                     "'" + user_gid + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                     "'P')";



                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1) { count++; }

                        }
                    }


                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = count + " Attendance Updated successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Updating Attendance";
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }




        public void GetShift(string branch_gid, MdlHrmTrnAttendanceroll values)
        {
            try
            {

                if (branch_gid != "all")
                {
                    msSQL = " select a.shifttype_name,a.shifttype_gid from hrm_mst_tshifttype a " +
                            " inner join hrm_mst_tshifttype2branch b on a.shifttype_gid=b.shifttype_gid "+
                            " where b.branch_gid ='" + branch_gid + "' ";
                }
                else
                {
                    msSQL = " select a.shifttype_name,a.shifttype_gid from hrm_mst_tshifttype a " +
                            " inner join hrm_mst_tshifttype2branch b on a.shifttype_gid=b.shifttype_gid ";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shiftList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shiftList
                        {
                            shift_gid = dt["shifttype_gid"].ToString(),
                            shift_name = dt["shifttype_name"].ToString(),
                        });
                        values.shiftList = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetAttendnanceErorlogSummary(string user_gid, MdlHrmTrnAttendanceroll values)
        {
            try
            {
                msSQL = " select date(a.uploaded_date) as uploaddate,a.errorlog,a.importcount ," +
                          " concat(c.user_firstname,' ',c.user_lastname) as created_by,a.uploaded_date   " +
                          " from hrm_trn_tattendanceuploadexcellog  a " +
                          " left  join adm_mst_Tuser c on a.uploaded_by=c.user_gid " +
                         " where 1=1";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<attendaceerror_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new attendaceerror_list
                        {
                            uploaddate = dt["uploaddate"].ToString(),
                            errorlog = dt["errorlog"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["uploaded_date"].ToString(),
                            importcount = dt["errorlog"].ToString(),

                        });
                        values.attendaceerror_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


    }
}
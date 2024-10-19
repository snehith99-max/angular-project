using ems.utilities.Functions;
using ems.utilities.Models;
using System.Collections.Generic;
using System.Data.Odbc;
using ems.subscription.Models;
using System.Data;
using System.Web;
using System;
using System.Web.Http;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using Stripe.Forwarding;
using System.Data.Common;
using System.Web.Http;
using System.Web.Http.Results;


namespace ems.subscription.DataAccess
{
    public class DaDynamicdb
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objODBCdatareader, objOdbcDataReader, objODBCDataReader, odbcDataReader;
        DataTable dt_datatable;
        int mnresult;
        string msGetGid;
        string msSQL1 = string.Empty;
        string domain = string.Empty;

        string[] createtables;
        int mnResult, ls_port;
        string  company_code, lsSQLpath, inserpath, Scripts, GetFilesinPath, lshostingconnectionstring, lsviewhostingconnectionstring, lshosting_details, lsserver_gid, lsserver_name;
        string ls_server, ls_password, ls_username,sub, cc_mailid, lsBccmail_id, lsto_mail ,lscompany_code;
        public string[] lsCCReceipients;
        public string[] lsToReceipients;
        public void DaGetModuledropdown(MdlDynamicdb values)
        {
            try
            {
                msSQL = "select module_name from sub_mst_tmodulemanagement";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Modulelists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Modulelists
                        {
                            module_name = dt["module_name"].ToString(),
                        });
                        values.Modulelists = getModuleList;

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
        //public bool DaInternalDynamicDBcreationInSQLFiles(string domain,string employee_gid, Mdlscriptmanagement values)
        public bool DaInternalDynamicDBcreationInSQLFiles(string domain, Mdlscriptmanagement values, string employee_gid)

        {

            string concatenatedModules = string.Join(",", values.productmodule_name);

            msSQL = "select server_name from vcxcontroller.adm_mst_tserver where server_gid='" + values.server_name + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {

                lsserver_name = objOdbcDataReader["server_name"].ToString();
            }
            objOdbcDataReader.Close();

            msGetGid = objcmnfunctions.GetMasterGID("DYNA");
            msSQL = " insert into sub_mst_tdynamicdbscriptmanagement(" +
                        " dynamicdbscriptmanagement_gid," +
                        " module_name," +
                        " company_code," +
                        " server_name ," +
                        " productmodule_name," +
                        " created_by," +
                        " created_date" +
                        " )values(" +
                        "'" + msGetGid + "'," +
                        "'" + values.module_name + "'," +
                        "'" + values.database_name + "'," +
                        "'" + lsserver_name + "'," +
                         "'" + concatenatedModules + "'," +
                        "'" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);


            if (values.module_name == "Kot")
            {
                company_code = values.database_name;

                msSQL = "select concat(hosting_details,'Database=sys;') as hosting_details,server_gid from vcxcontroller.adm_mst_tserver  where server_status='Open' and server_gid='" + values.server_name + "'";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {

                    lsserver_gid = objODBCDataReader["server_gid"].ToString();
                    lshosting_details = objODBCDataReader["hosting_details"].ToString();
                }

                string databaseconnectionString = lshosting_details;
                objODBCDataReader.Close();

                msSQL1 = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and server_gid='" + values.server_name + "'";
                //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                objODBCDataReader = objdbconn.GetDataReader(msSQL1);
                if (objODBCDataReader.HasRows == true)
                {

                    lsviewhostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                }
                objODBCDataReader.Close();

                using (OdbcConnection connection = new OdbcConnection(databaseconnectionString))
                {
                    try
                    {
                        connection.Open();

                        string msSQL = "CREATE DATABASE `" + company_code + "`;";

                        OdbcCommand command = new OdbcCommand(msSQL, connection);

                        // Execute the SQL command
                        int mnDBResult = command.ExecuteNonQuery();
                        mnDBResult = 1;

                        if (mnDBResult == 1)

                        {
                            string folderName = values.module_name;
                            lsSQLpath = ConfigurationManager.AppSettings["dynamicdbpath"].ToString();

                            // The directory where you want to search for the folder
                            string baseDirectory = lsSQLpath;

                            // Combine the base directory path with the folder name
                            string fullPath = Path.Combine(baseDirectory, folderName);
                            var SQLfiles = Directory.GetFiles(fullPath, "*.sql");
                            foreach (var createstatement in SQLfiles)
                            {
                                GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                                if (createstatement.EndsWith("kotSP.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "CREATE PROCEDURE`" + company_code + "`." + "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                                    createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

                                    //var combinedScripts = string.Join(";\r\n", createtables.Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p))) + ";";

                                    //if (!string.IsNullOrEmpty(combinedScripts))
                                    //{
                                    //    OdbcCommand commandsp = new OdbcCommand(combinedScripts, connection);
                                    //    mnresult = commandsp.ExecuteNonQuery();
                                    //}
                                }
                                else if (createstatement.EndsWith("Functions.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                                    createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                                }
                                else if (createstatement.EndsWith("View.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW", "CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW").Replace("VIEW `", "VIEW `");

                                    //Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                                    createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);



                                    using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                    {
                                        viewconnection.Open();
                                        foreach (var tables in createtables)
                                        {
                                            if (tables.Trim() != "")
                                            {
                                                msSQL = tables.Trim();
                                                OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                // Execute the SQL command
                                                mnresult = commands.ExecuteNonQuery();
                                                mnresult = 1;
                                            }
                                        }
                                        viewconnection.Close();

                                    }


                                }
                                else if (createstatement.EndsWith("kotTables.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                                    createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                }

                                else if (createstatement.EndsWith("TableAlter.sql"))
                                {
                                    string Scripts = GetFilesinPath.Replace("ALTER TABLE", "ALTER TABLE `" + company_code + "`." + "");

                                    createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                    using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                    {
                                        viewconnection.Open();
                                        foreach (var tables in createtables)
                                        {
                                            if (tables.Trim() != "")
                                            {
                                                try
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.Message.Contains("Duplicate column name"))
                                                    {

                                                    }
                                                    else if (ex.Message.Contains("Unknown column"))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        // Re-throw the exception if it's not a duplicate-related error
                                                        throw;
                                                    }
                                                    
                                                }

                                            }
                                        }
                                        viewconnection.Close();

                                    }


                                }
                                if (!createstatement.EndsWith("View.sql", StringComparison.OrdinalIgnoreCase) &&
                                     !createstatement.EndsWith("TableAlter.sql", StringComparison.OrdinalIgnoreCase))
                                {
                                    foreach (var tables in createtables)
                                    {
                                        if (tables.Trim() != "")
                                        {
                                            msSQL = tables.Trim();
                                            OdbcCommand commands = new OdbcCommand(msSQL, connection);
                                            // Execute the SQL command
                                            mnresult = commands.ExecuteNonQuery();
                                            mnresult = 1;
                                        }
                                    }
                                }

                            }


                            if (mnresult == 1)
                            {
                                msSQL = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and server_gid='" + values.server_name + "'";
                                //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                if (objODBCDataReader.HasRows == true)
                                {

                                    lshostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                                }
                                objODBCDataReader.Close();
                                //string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                                //    "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                                //    "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";

                                DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                                builder.ConnectionString = lshostingconnectionstring;

                                // Extract the components
                                string uid = builder.ContainsKey("UID") ? builder["UID"].ToString() : string.Empty;
                                string pwd = builder.ContainsKey("PWD") ? builder["PWD"].ToString() : string.Empty;
                                string server = builder.ContainsKey("Server") ? builder["Server"].ToString() : string.Empty;
                                string port = builder.ContainsKey("port") ? builder["port"].ToString() : string.Empty;


                                msSQL = "select company_code " +
                                    "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";

                                OdbcCommand commandreader = new OdbcCommand(msSQL, connection);

                                // Execute the command and obtain a data reader
                                OdbcDataReader objOdbcDataReader = commandreader.ExecuteReader();

                                if (!objOdbcDataReader.HasRows)
                                {
                                    msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                                            "company_code," +
                                            "server_name," +
                                            "db_name," +
                                            "user_name," +
                                            "password," +
                                            "connection_string," +
                                            "created_date, created_by) values (" +
                                            "'" + company_code + "'," +
                                            "'" + server + "'," +
                                            "'" + company_code + "'," +
                                            "'" + uid + "'," +
                                            "'" + pwd + "'," +
                                            "'" + lshostingconnectionstring + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                            "'U1')";

                                    OdbcCommand consumerdbcommand = new OdbcCommand(msSQL, connection);
                                    mnResult = consumerdbcommand.ExecuteNonQuery();
                                    mnResult = 1;

                                    if (mnResult == 1)
                                    {
                                        msSQL = " insert into `" + company_code + "`.adm_mst_tcompany (" +
                                                 "company_code," +
                                                 "company_name," +
                                                 "company_phone," +
                                                 //"company_address," +
                                                 "contact_person," +
                                                 "company_mail," +
                                                 "auth_code," +
                                                 "pop_password" +
                                                 " ) values ( " +
                                                 "'" + company_code + "'," +
                                                 "'" + company_code + "'," +
                                                 "'9042075030'," +
                                                 //"'" + company_address + "'," +
                                                 "'Vcidex'," +
                                                 "'yamini@vcidex.com'," +
                                                 "''," +
                                                 "'')";
                                        OdbcCommand companycommand = new OdbcCommand(msSQL, connection);
                                        mnResult = companycommand.ExecuteNonQuery();
                                        mnResult = 1;
                                        if (mnResult == 1)
                                        {
                                            msSQL = "insert into `" + company_code + "`.adm_mst_tuser(" +
                                                   "user_gid," +
                                                   "user_code," +
                                                   "user_firstname," +
                                                    "user_lastname," +
                                                   "user_password," +
                                                   "user_status" +
                                                   " ) values (" +
                                                   "'U1'," +
                                                   "'superadmin'," +
                                                   "'superadmin'," +
                                                   "'administrator'," +
                                                   "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                   "'Y')";

                                            OdbcCommand usercommand = new OdbcCommand(msSQL, connection);
                                            mnResult = usercommand.ExecuteNonQuery();
                                            mnResult = 1;

                                            msSQL = "insert into `" + company_code + "`.adm_mst_tuser(" +
                                                  "user_gid," +
                                                  "user_code," +
                                                  "user_firstname," +
                                                   "user_lastname," +
                                                  "user_password," +
                                                  "user_status" +
                                                  " ) values (" +
                                                  "'U2'," +
                                                  "'admin'," +
                                                  "'admin'," +
                                                  "'administrator'," +
                                                  "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                  "'Y')";

                                            OdbcCommand usercommand1 = new OdbcCommand(msSQL, connection);
                                            mnResult = usercommand1.ExecuteNonQuery();
                                            mnResult = 1;


                                            if (mnResult == 1)
                                            {
                                                msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                                        "employee_gid," +
                                                        "biometric_id," +
                                                        "user_gid" +
                                                        " )values(" +
                                                        "'E1'," +
                                                         "'1'," +
                                                        "'U1')";
                                                OdbcCommand employeecommand = new OdbcCommand(msSQL, connection);
                                                mnResult = employeecommand.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                                   "employee_gid," +
                                                   "biometric_id," +
                                                   "user_gid" +
                                                   " )values(" +
                                                   "'E2'," +
                                                   "'1'," +
                                                   "'U2')";
                                                OdbcCommand employeecommand1 = new OdbcCommand(msSQL, connection);
                                                mnResult = employeecommand1.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                    "module2employee_gid," +
                                                    "module_gid," +
                                                    "employee_gid," +
                                                    "branch_gid," +
                                                    "employeereporting_to," +
                                                    "hierarchy_level," +
                                                    "visible," +
                                                    "approval_hierarchy_removed," +
                                                    "created_by," +
                                                    "created_date" +
                                                    " ) values (" +
                                                    "'SMEM1611170112'," +
                                                    "'SYS'," +
                                                    "'E1'," +
                                                    "''," +
                                                    "'null'," +
                                                    "'1'," +
                                                    "'T'," +
                                                    "'N'," +
                                                    "'E1'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                OdbcCommand module2employee = new OdbcCommand(msSQL, connection);
                                                mnResult = module2employee.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                        "module2employee_gid," +
                                                        "module_gid," +
                                                        "employee_gid," +
                                                        "branch_gid," +
                                                        "employeereporting_to," +
                                                        "hierarchy_level," +
                                                        "visible," +
                                                        "approval_hierarchy_removed," +
                                                        "created_by," +
                                                        "created_date" +
                                                        " ) values (" +
                                                        "'SMEM1611170113'," +
                                                        "'SYS'," +
                                                        "'E2'," +
                                                        "''," +
                                                        "'null'," +
                                                        "'1'," +
                                                        "'T'," +
                                                        "'N'," +
                                                        "'E2'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                OdbcCommand module2employee1 = new OdbcCommand(msSQL, connection);
                                                mnResult = module2employee1.ExecuteNonQuery();
                                                mnResult = 1;

                                                if (mnResult == 1)
                                                {
                                                    string metadata = "MetaData"; string commonmetadata = "CommonMetadata";
                                                    string lsfullcommonpath = folderName + "/" + metadata + "/" + commonmetadata;

                                                  
                                                    string fullPathmetadata = Path.Combine(baseDirectory, lsfullcommonpath);
                                                    var InsertFiles = Directory.GetFiles(fullPathmetadata, "*.sql");

                                                    foreach (var insertquery in InsertFiles)
                                                    {
                                                        inserpath = System.IO.File.ReadAllText(insertquery);

                                                        string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`." + "");
                                                        string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var insert in metadatasforinsert)
                                                        {
                                                            if (insert.Trim() != "")
                                                            {
                                                                msSQL = insert.Trim();

                                                                try
                                                                {
                                                                    // Attempt to execute the insert query
                                                                    OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                                    mnResult = metacommand.ExecuteNonQuery();
                                                                    mnResult = 1; // Assuming the insert was successful
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    if (ex.Message.Contains("Duplicate entry"))
                                                                    {
                                                                        // If a duplicate is found, move it to the 'duplicate_records' table
                                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.duplicate_records (original_data, error_message) VALUES (@original_data, @error_message)";
                                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                                        duplicateCommand.Parameters.AddWithValue("@original_data", msSQL);
                                                                        duplicateCommand.Parameters.AddWithValue("@error_message", ex.Message);
                                                                        duplicateCommand.ExecuteNonQuery();
                                                                    }
                                                                    else
                                                                    {
                                                                        // Re-throw the exception if it's not a duplicate-related error
                                                                        throw;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //string lsfullmodulepath = folderName + "/" + metadata + "/" + values.productmodule_name;

                                                    //fullPathmetadata = Path.Combine(baseDirectory, lsfullmodulepath);
                                                    //var InsertFiles1 = Directory.GetFiles(fullPathmetadata, "*.sql");

                                                    //foreach (var insertquery in InsertFiles1)
                                                    //{
                                                    //    inserpath = System.IO.File.ReadAllText(insertquery);

                                                    //    string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`." + "");
                                                    //    string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);
                                                    //    foreach (var insert in metadatasforinsert)
                                                    //    {
                                                    //        if (insert.Trim() != "")
                                                    //        {
                                                    //            msSQL = insert.Trim();

                                                    //            try
                                                    //            {
                                                    //                // Attempt to execute the insert query
                                                    //                OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                    //                mnResult = metacommand.ExecuteNonQuery();
                                                    //                mnResult = 1; // Assuming the insert was successful
                                                    //            }
                                                    //            catch (Exception ex)
                                                    //            {
                                                    //                if (ex.Message.Contains("Duplicate entry"))
                                                    //                {
                                                    //                    // If a duplicate is found, move it to the 'duplicate_records' table
                                                    //                    string moveToDuplicateTable = "INSERT INTO vcxcontroller.duplicate_records (original_data, error_message) VALUES (@original_data, @error_message)";
                                                    //                    OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                    //                    duplicateCommand.Parameters.AddWithValue("@original_data", msSQL);
                                                    //                    duplicateCommand.Parameters.AddWithValue("@error_message", ex.Message);
                                                    //                    duplicateCommand.ExecuteNonQuery();
                                                    //                }
                                                    //                else
                                                    //                {
                                                    //                    // Re-throw the exception if it's not a duplicate-related error
                                                    //                    throw;
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}


                                                    if (mnResult == 1)
                                                    {
                                                        msSQL = "insert into vcxcontroller.adm_mst_tconsumer(" +
                                                                  "server_gid," +
                                                                  "company_code," +
                                                                  "consumer_url," +
                                                                  "subscription_details," +
                                                                  "created_by," +
                                                                  "created_date," +
                                                                  "status) values (" +
                                                                  "'" + lsserver_gid + "'," +
                                                                  "'" + company_code + "'," +                                                                 
                                                                  "'" + domain + "'," +
                                                                    "'Manual'," +
                                                                  "'" + employee_gid + "'," +
                                                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                  "'Y')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                        //objcmnfunctions.activationMailtrigger(values.database_name, "superadmin", "Welcome@123");
                                                        values.status = true;
                                                        values.message = " Successfully Dynamic DB has been created...";


                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = " Already this company code has used...";
                                }
                                objOdbcDataReader.Close();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        connection.Close();

                        LogForAudit(ex.Message.ToString());
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                                        "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }
                    connection.Close();
                }
            }
            else
            {
                company_code = values.database_name;

                msSQL = "select concat(hosting_details,'Database=sys;') as hosting_details,server_gid from vcxcontroller.adm_mst_tserver  where server_status='Open' and server_gid='" + values.server_name + "'";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {

                    lsserver_gid = objODBCDataReader["server_gid"].ToString();
                    lshosting_details = objODBCDataReader["hosting_details"].ToString();
                }


                string databaseconnectionString = lshosting_details;
                objODBCDataReader.Close();

                msSQL1 = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and server_gid='" + values.server_name + "'";
                //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                objODBCDataReader = objdbconn.GetDataReader(msSQL1);
                if (objODBCDataReader.HasRows == true)
                {

                    lsviewhostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                }
                objODBCDataReader.Close();
                using (OdbcConnection connection = new OdbcConnection(databaseconnectionString))
                {
                    try
                    {
                        connection.Open();

                        string msSQL = "CREATE DATABASE `" + company_code + "`;";

                        OdbcCommand command = new OdbcCommand(msSQL, connection);

                        // Execute the SQL command
                        int mnDBResult = command.ExecuteNonQuery();
                        mnDBResult = 1;

                        if (mnDBResult == 1)

                        {
                            string folderName = values.module_name;
                            lsSQLpath = ConfigurationManager.AppSettings["dynamicdbpath"].ToString();

                            // The directory where you want to search for the folder
                            string baseDirectory = lsSQLpath;

                            // Combine the base directory path with the folder name
                            string fullPath = Path.Combine(baseDirectory, folderName);
                            var SQLfiles = Directory.GetFiles(fullPath, "*.sql");
                            foreach (var createstatement in SQLfiles)
                            {
                                GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                                if (createstatement.EndsWith("SP.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "CREATE PROCEDURE`" + company_code + "`." + "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                                    createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

                                }
                                else if (createstatement.EndsWith("Functions.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                                    createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                                }
                                //else if (createstatement.EndsWith("View.sql"))
                                //{
                                //    Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                                //    createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                //}
                                else if (createstatement.EndsWith("View.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW", "CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW").Replace("VIEW `", "VIEW `");

                                    //Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                                    createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                    using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                    {
                                        viewconnection.Open();
                                        foreach (var tables in createtables)
                                        {
                                            if (tables.Trim() != "")
                                            {
                                                msSQL = tables.Trim();
                                                OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                // Execute the SQL command
                                                mnresult = commands.ExecuteNonQuery();
                                                mnresult = 1;
                                            }
                                        }
                                        viewconnection.Close();

                                    }
                                }
                                else if (createstatement.EndsWith("Tables.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                                    createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                    //var combinedScript = string.Join(";", createtables.Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)));

                                    //if (!string.IsNullOrEmpty(combinedScript))
                                    //{
                                    //    OdbcCommand commands = new OdbcCommand(combinedScript, connection);
                                    //    mnresult = commands.ExecuteNonQuery();
                                    //    mnresult = 1;
                                    //}
                                }
                                else if (createstatement.EndsWith("TableAlter.sql"))
                                {
                                    string Scripts = GetFilesinPath.Replace("ALTER TABLE", "ALTER TABLE `" + company_code + "`." + "");

                                    createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                    using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                    {
                                        viewconnection.Open();
                                        foreach (var tables in createtables)
                                        {
                                            if (tables.Trim() != "")
                                            {
                                                msSQL = tables.Trim();
                                                try
                                                {
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.Message.Contains("Duplicate column name"))
                                                    {

                                                    }
                                                    else if (ex.Message.Contains("Unknown column"))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        // Re-throw the exception if it's not a duplicate-related error
                                                        throw;
                                                    }
                                                }
                                            }
                                        }
                                        viewconnection.Close();

                                    }


                                }
                                if (!createstatement.EndsWith("View.sql", StringComparison.OrdinalIgnoreCase) &&
                                     !createstatement.EndsWith("TableAlter.sql", StringComparison.OrdinalIgnoreCase))
                                {
                                    foreach (var tables in createtables)
                                    {
                                        if (tables.Trim() != "")
                                        {
                                            msSQL = tables.Trim();
                                            OdbcCommand commands = new OdbcCommand(msSQL, connection);
                                            // Execute the SQL command
                                            mnresult = commands.ExecuteNonQuery();
                                            mnresult = 1;
                                        }
                                    }
                                }

                            }


                            if (mnresult == 1)
                            {
                                msSQL = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and server_gid='" + values.server_name + "'";
                                //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                if (objODBCDataReader.HasRows == true)
                                {

                                    lshostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                                }
                                objODBCDataReader.Close();

                                //string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                                //    "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                                //    "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";

                                DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                                builder.ConnectionString = lshostingconnectionstring;

                                // Extract the components
                                string uid = builder.ContainsKey("UID") ? builder["UID"].ToString() : string.Empty;
                                string pwd = builder.ContainsKey("PWD") ? builder["PWD"].ToString() : string.Empty;
                                string server = builder.ContainsKey("Server") ? builder["Server"].ToString() : string.Empty;
                                string port = builder.ContainsKey("port") ? builder["port"].ToString() : string.Empty;


                                msSQL = "select company_code " +
                                    "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";

                                OdbcCommand commandreader = new OdbcCommand(msSQL, connection);

                                // Execute the command and obtain a data reader
                                OdbcDataReader objOdbcDataReader = commandreader.ExecuteReader();

                                if (!objOdbcDataReader.HasRows)
                                {
                                    msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                                            "company_code," +
                                            "server_name," +
                                            "db_name," +
                                            "user_name," +
                                            "password," +
                                            "connection_string," +
                                            "created_date, created_by) values (" +
                                            "'" + company_code + "'," +
                                            "'" + server + "'," +
                                            "'" + company_code + "'," +
                                            "'" + uid + "'," +
                                            "'" + pwd + "'," +
                                            "'" + lshostingconnectionstring + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                            "'U1')";

                                    OdbcCommand consumerdbcommand = new OdbcCommand(msSQL, connection);
                                    mnResult = consumerdbcommand.ExecuteNonQuery();
                                    mnResult = 1;

                                    if (mnResult == 1)
                                    {
                                        msSQL = " insert into `" + company_code + "`.adm_mst_tcompany (" +
                                                "company_code," +
                                                "company_name," +
                                                "company_phone," +
                                                //"company_address," +
                                                "contact_person," +
                                                "company_mail," +
                                                "auth_code," +
                                                "pop_password" +
                                                " ) values ( " +
                                                "'" + company_code + "'," +
                                                "'" + company_code + "'," +
                                                "'9042075030'," +
                                                //"'" + company_address + "'," +
                                                "'Vcidex'," +
                                                "'yamini@vcidex.com'," +
                                                "''," +
                                                "'')";
                                        OdbcCommand companycommand = new OdbcCommand(msSQL, connection);
                                        mnResult = companycommand.ExecuteNonQuery();
                                        mnResult = 1;
                                        if (mnResult == 1)
                                        {
                                            msSQL = "insert into `" + company_code + "`.adm_mst_tuser(" +
                                                   "user_gid," +
                                                   "user_code," +
                                                   "user_firstname," +
                                                    "user_lastname," +
                                                   "user_password," +
                                                   "user_status" +
                                                   " ) values (" +
                                                   "'U1'," +
                                                   "'superadmin'," +
                                                   "'superadmin'," +
                                                   "'administrator'," +
                                                   "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                   "'Y')";

                                            OdbcCommand usercommand = new OdbcCommand(msSQL, connection);
                                            mnResult = usercommand.ExecuteNonQuery();
                                            mnResult = 1;

                                            msSQL = "insert into `" + company_code + "`.adm_mst_tuser(" +
                                                  "user_gid," +
                                                  "user_code," +
                                                  "user_firstname," +
                                                   "user_lastname," +
                                                  "user_password," +
                                                  "user_status" +
                                                  " ) values (" +
                                                  "'U2'," +
                                                  "'admin'," +
                                                  "'admin'," +
                                                  "'administrator'," +
                                                  "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                  "'Y')";

                                            OdbcCommand usercommand1 = new OdbcCommand(msSQL, connection);
                                            mnResult = usercommand1.ExecuteNonQuery();
                                            mnResult = 1;


                                            if (mnResult == 1)
                                            {
                                                msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                                        "employee_gid," +
                                                        "biometric_id," +
                                                        "user_gid" +
                                                        " )values(" +
                                                        "'E1'," +
                                                         "'1'," +
                                                        "'U1')";
                                                OdbcCommand employeecommand = new OdbcCommand(msSQL, connection);
                                                mnResult = employeecommand.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                                   "employee_gid," +
                                                   "biometric_id," +
                                                   "user_gid" +
                                                   " )values(" +
                                                   "'E2'," +
                                                   "'1'," +
                                                   "'U2')";
                                                OdbcCommand employeecommand1 = new OdbcCommand(msSQL, connection);
                                                mnResult = employeecommand1.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                    "module2employee_gid," +
                                                    "module_gid," +
                                                    "employee_gid," +
                                                    "branch_gid," +
                                                    "employeereporting_to," +
                                                    "hierarchy_level," +
                                                    "visible," +
                                                    "approval_hierarchy_removed," +
                                                    "created_by," +
                                                    "created_date" +
                                                    " ) values (" +
                                                    "'SMEM1611170112'," +
                                                    "'SYS'," +
                                                    "'E1'," +
                                                    "''," +
                                                    "'null'," +
                                                    "'1'," +
                                                    "'T'," +
                                                    "'N'," +
                                                    "'E1'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                OdbcCommand module2employee = new OdbcCommand(msSQL, connection);
                                                mnResult = module2employee.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                        "module2employee_gid," +
                                                        "module_gid," +
                                                        "employee_gid," +
                                                        "branch_gid," +
                                                        "employeereporting_to," +
                                                        "hierarchy_level," +
                                                        "visible," +
                                                        "approval_hierarchy_removed," +
                                                        "created_by," +
                                                        "created_date" +
                                                        " ) values (" +
                                                        "'SMEM1611170113'," +
                                                        "'SYS'," +
                                                        "'E2'," +
                                                        "''," +
                                                        "'null'," +
                                                        "'1'," +
                                                        "'T'," +
                                                        "'N'," +
                                                        "'E2'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                OdbcCommand module2employee1 = new OdbcCommand(msSQL, connection);
                                                mnResult = module2employee1.ExecuteNonQuery();
                                                mnResult = 1;

                                                if (mnResult == 1)
                                                {

                                                    string metadata = "MetaData"; string commonmetadata = "CommonMetadata";
                                                   
                                                    string lsfullmodulepath = folderName + "/" + metadata + "/" + commonmetadata;

                                                    string fullPathmetadata = Path.Combine(baseDirectory, lsfullmodulepath);
                                                    var InsertFiles = Directory.GetFiles(fullPathmetadata, "*.sql");
                                                    foreach (var insertquery in InsertFiles)
                                                    {
                                                        inserpath = System.IO.File.ReadAllText(insertquery);

                                                        string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`.");
                                                        string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);

                                                        foreach (var insert in metadatasforinsert)
                                                        {
                                                            if (insert.Trim() != "")
                                                            {
                                                                msSQL = insert.Trim();

                                                                try
                                                                {
                                                                    // Attempt to execute the insert query
                                                                    OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                                    mnResult = metacommand.ExecuteNonQuery();
                                                                    mnResult = 1; // Assuming the insert was successful
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    if (ex.Message.Contains("Duplicate entry"))
                                                                    {
                                                                        // If a duplicate is found, move it to the 'duplicate_records' table
                                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.duplicate_records (original_data, error_message) VALUES (@original_data, @error_message)";
                                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                                        duplicateCommand.Parameters.AddWithValue("@original_data", msSQL);
                                                                        duplicateCommand.Parameters.AddWithValue("@error_message", ex.Message);
                                                                        duplicateCommand.ExecuteNonQuery();
                                                                    }
                                                                    else
                                                                    {
                                                                        // Re-throw the exception if it's not a duplicate-related error
                                                                        throw;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    foreach (string dbcompany_code in values.productmodule_name)
                                                    {

                                                        string lsmodulepath = folderName + "/" + metadata + "/" + dbcompany_code;

                                                        string lsPathmetadata = Path.Combine(baseDirectory, lsmodulepath);
                                                        var InsertFiles1 = Directory.GetFiles(lsPathmetadata, "*.sql");
                                                        foreach (var insertquery in InsertFiles1)
                                                        {
                                                            inserpath = System.IO.File.ReadAllText(insertquery);

                                                            string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`.");
                                                            string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);

                                                            foreach (var insert in metadatasforinsert)
                                                            {
                                                                if (insert.Trim() != "")
                                                                {
                                                                    msSQL = insert.Trim();

                                                                    try
                                                                    {
                                                                        // Attempt to execute the insert query
                                                                        OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                                        mnResult = metacommand.ExecuteNonQuery();
                                                                        mnResult = 1; // Assuming the insert was successful
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        if (ex.Message.Contains("Duplicate entry"))
                                                                        {
                                                                            // If a duplicate is found, move it to the 'duplicate_records' table
                                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.duplicate_records (original_data, error_message) VALUES (@original_data, @error_message)";
                                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                                            duplicateCommand.Parameters.AddWithValue("@original_data", msSQL);
                                                                            duplicateCommand.Parameters.AddWithValue("@error_message", ex.Message);
                                                                            duplicateCommand.ExecuteNonQuery();
                                                                        }
                                                                        else
                                                                        {
                                                                            // Re-throw the exception if it's not a duplicate-related error
                                                                            throw;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                        

                                                    if (mnResult == 1)
                                                    {
                                                        msSQL = "insert into vcxcontroller.adm_mst_tconsumer(" +
                                                                  "server_gid," +
                                                                  "company_code," +
                                                                  "consumer_url," +
                                                                  "subscription_details," +
                                                                  "created_by," +
                                                                  "created_date," +
                                                                  "status) values (" +
                                                                  "'" + lsserver_gid + "'," +
                                                                  "'" + company_code + "'," +
                                                                  "'" + domain + "'," +
                                                                  "'Manual'," +
                                                                  "'" + employee_gid + "'," +
                                                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                  "'Y')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                       bool status = InternalDynamicDBMailtrigger(company_code, "superadmin", "Welcome@123");
                                                        values.status = true;

                                                        values.message = " Successfully Dynamic DB has been created...";

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = " Already this company has used...";
                                }
                                objOdbcDataReader.Close();

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        string lsConnectionString = ConfigurationManager.ConnectionStrings["AuthConn"].ConnectionString;

                        using (OdbcConnection dbconnection = new OdbcConnection(lsConnectionString))
                        {
                            dbconnection.Open();
                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdynamicscripterrormanagement (dynamicdbscriptmanagement_gid,company_code, server_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?)";
                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, dbconnection);
                            duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                            duplicateCommand.Parameters.AddWithValue("?", company_code);
                            duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                            duplicateCommand.Parameters.AddWithValue("?", ex.Message);
                            duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                            duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                            mnresult = duplicateCommand.ExecuteNonQuery();
                            mnresult = 1;

                            dbconnection.Close();

                        }

                        connection.Close();

                        LogForAudit(ex.Message.ToString());
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SUB/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }
                    connection.Close();
                }
            }


            return values.status;
        }
        public void LogForAudit(string strVal)
        {
            try
            {
                string lspath = ConfigurationManager.AppSettings["file_path"].ToString() + "/erp_documents/errorlog/" + DateTime.Now.Year + @"\" + DateTime.Now.Month;
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);

                lspath = lspath + @"\" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
                System.IO.StreamWriter sw = new System.IO.StreamWriter(lspath, true);
                sw.WriteLine(strVal);
                sw.Close();
            }
            catch (Exception ex)
            {
            }
        }
        public void DaGetdynamicdbSummary(MdlDynamicdb values)
        {
            try
            {
                msSQL = "select a.dynamicdbscriptmanagement_gid,a.module_name,a.company_code,a.server_name,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by from sub_mst_tdynamicdbscriptmanagement a " +
                    " left join hrm_mst_temployee b on a.created_by = b.employee_gid " +
                    "  left join adm_mst_tuser c on c.user_gid = b.user_gid order by a.dynamicdbscriptmanagement_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<dynamicdblists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new dynamicdblists
                        {
                            module_name = dt["module_name"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            server_name = dt["server_name"].ToString(),
                            dynamicdbscriptmanagement_gid = dt["dynamicdbscriptmanagement_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.dynamicdblists = getModuleList;
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
        public bool InternalDynamicDBMailtrigger(string company_code,string user_code,string password)
        {
            try
            {
                string body;
                msSQL = " SELECT pop_server, pop_port, pop_username, pop_password  FROM vcxcontroller.adm_mst_tcompany";
                objODBCdatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCdatareader.HasRows == true)
                {
                    ls_server = objODBCdatareader["pop_server"].ToString();
                    ls_port = Convert.ToInt32(objODBCdatareader["pop_port"]);
                    ls_username = objODBCdatareader["pop_username"].ToString();
                    ls_password = objODBCdatareader["pop_password"].ToString();
                }
                objODBCdatareader.Close();

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(ls_username);

                sub = "Database has been Successfully Created ";
                body = "Dear Infra Team, <br/>";
                body += "<br />";
                body += "Greetings,  <br/>";
                body += "<br />";
                body += "We are pleased to inform that database has been successfully created,Below are the details for login credentials<br/>";
                body += "<br />";
                body += "<b>Company Code: </b>" + company_code + "<br/>";
                body += "<br />";
                body += "<b>User Code: </b>" + user_code + "<br/>";
                body += "<br />";
                body += "<b>Password: </b>" + password + "<br/>";
                body += "<br />";
                body += "If this login was not initiated by the authorized user, please review the account's activity and contact our portal management team.<br/>";
                body += "<br />";
                body += "Best Regards,";
                body += "<br />";
                body += "Portal Management Team";
                body += "<br />";

                lsto_mail = ConfigurationManager.AppSettings["internaldbtomail_id"].ToString();
                cc_mailid = ConfigurationManager.AppSettings["internaldbccmail_id"].ToString();

                if (lsto_mail != null & lsto_mail != string.Empty & lsto_mail != "")
                {
                    lsToReceipients = lsto_mail.Split(',');
                    if (lsto_mail.Length == 0)
                    {
                        message.To.Add(new MailAddress(lsto_mail));
                    }
                    else
                    {
                        foreach (string ToEmail in lsToReceipients)
                        {
                            message.To.Add(new MailAddress(ToEmail)); //Adding Multiple CC email Id
                        }
                    }
                }

                if (cc_mailid != null & cc_mailid != string.Empty & cc_mailid != "")
                {
                    lsCCReceipients = cc_mailid.Split(',');
                    if (cc_mailid.Length == 0)
                    {
                        message.CC.Add(new MailAddress(cc_mailid));
                    }
                    else
                    {
                        foreach (string CCEmail in lsCCReceipients)
                        {
                            message.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                        }
                    }
                }

                message.Body = body;
                message.Subject = sub;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = body;
                smtp.Port = ls_port;
                smtp.Host = ls_server; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                smtp.Credentials = new NetworkCredential(ls_username, ls_password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void DaGetScriptViewSummary(Mdlscriptmanagement values, string dbscriptmanagementdocument_gid)
        {
            try
            {
                msSQL = "select a.company_code,a.server_name,a.script_name,a.execute_query,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by from sub_mst_tdbscriptmanagement a " +
                     " left join hrm_mst_temployee b on a.created_by = b.employee_gid " +
                    "  left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                    " where dbscriptmanagementdocument_gid='" + dbscriptmanagementdocument_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<serverlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new serverlists
                        {

                            company_code = dt["company_code"].ToString(),
                            server_name = dt["server_name"].ToString(),
                            script_name = dt["script_name"].ToString(),
                            execute_query = dt["execute_query"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),

                        });
                        values.serverlists = getModuleList;
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
        public void DaGetDynamicDBScriptViewSummary(Mdlscriptmanagement values, string dynamicdbscriptmanagement_gid)
        {
            try
            {
                msSQL = "select a.company_code,a.server_name,a.execute_query,DATE_FORMAT(a.created_date , '%d-%m-%Y %h:%i %p') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by from sub_mst_tdynamicscripterrormanagement a " +
                     " left join hrm_mst_temployee b on a.created_by = b.employee_gid " +
                    "  left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                    " where dynamicdbscriptmanagement_gid='" + dynamicdbscriptmanagement_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<serverlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new serverlists
                        {

                            company_code = dt["company_code"].ToString(),
                            server_name = dt["server_name"].ToString(),
                            execute_query = dt["execute_query"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),

                        });
                        values.serverlists = getModuleList;
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
        public void DaDeleteDatabase(Mdlscriptmanagement values,string employee_gid)
        {
            msSQL = "select concat(hosting_details,'Database=sys;') as hosting_details,server_gid from vcxcontroller.adm_mst_tserver  where server_status='Open' and server_gid='" + values.server_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {

                lsserver_gid = objODBCDataReader["server_gid"].ToString();
                lshosting_details = objODBCDataReader["hosting_details"].ToString();
            }
            string databaseconnectionString = lshosting_details;
            objODBCDataReader.Close();

            using (OdbcConnection connection = new OdbcConnection(databaseconnectionString))
            {
                connection.Open();
                
                    msSQL = "select company_code from adm_mst_tconsumer where company_code='" + values.database_name + "'";
                  string db_name = objdbconn.GetExecuteScalar(msSQL);

                if(db_name =="")
                {
                    values.status = false;
                    values.message = "Company Code Does Not Exist";
                    return;
                }

                    msSQL = "DROP DATABASE `" + values.database_name + "`;";

                    OdbcCommand command = new OdbcCommand(msSQL, connection);

                    // Execute the SQL command
                    int mnDBResult = command.ExecuteNonQuery();
                    mnDBResult = 1;
                    values.status = true;

                msSQL = "  delete from sub_mst_tdynamicdbscriptmanagement where dynamicdbscriptmanagement_gid = '" + values.dynamicdbscriptmanagement_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "  delete from storyboard.adm_mst_tconsumerdb where company_code = '" + values.database_name + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "  delete from vcxcontroller.adm_mst_tconsumer where company_code = '" + values.database_name + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Database Deleted Successfully";
                }


            }
            //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            //if (mnResult != 0)
            //{
            //    values.status = true;
            //    values.message = "Database exist";
            //}
            //else
            //{
            //    {
            //        values.status = false;
            //        values.message = "Error While Deleting Database";
            //    }
            //}

        }
    }
}
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
using Newtonsoft.Json;
using System.Data.SqlClient;
using Stripe;


namespace ems.subscription.DataAccess
{
    public class DaSubManagement
    {
    dbconn objdbconn = new dbconn();
    cmnfunctions objcmnfunctions = new cmnfunctions();
    Fnazurestorage objcmnstorage = new Fnazurestorage();
        string msSQL = string.Empty;
        OdbcDataReader objODBCdatareader, objOdbcDataReader;
        DataTable  dt_datatable;
        string[] createtables;

        int mnresult;
        string msGetGid, lspath, lsviewhostingconnectionstring, Scripts, lsserver_name, insertsqeriery;
        public void DaGetsubscriptionSummary(MdlSubManagement values)
        {
            try
            {


                msSQL = " select company_code,company_name,server_name,db_name,DATE_FORMAT(created_date , '%d-%m-%Y') as created_date,user_name from storyboard.adm_mst_tconsumerdb; ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<subscription_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new subscription_list
                        {

                            company_code = dt["company_code"].ToString(),
                            company_name = dt["company_name"].ToString(),
                            server_name = dt["server_name"].ToString(),
                            db_name = dt["db_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            user_name = dt["user_name"].ToString(),
                           

                        });
                        values.subscription_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting subscription summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/subscription/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }

        public void DaGetServerSummary(MdlSubManagement values)
        {
            try
            {

                msSQL = " select server_gid,server_name,hosting_details,token_no,server_ip,server_status,country_name, " +
                    " (select count(server_gid) from vcxcontroller.adm_mst_tconsumer b where b.server_gid = a.server_gid and b.active_status='Y')as total_count " +
                    " from vcxcontroller.adm_mst_tserver a";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<server_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new server_list
                        {

                            server_name = dt["server_name"].ToString(),
                            hosting_details = dt["hosting_details"].ToString(),
                            token_number = dt["token_no"].ToString(),
                            server_ipaddress = dt["server_ip"].ToString(),
                            server_gid = dt["server_gid"].ToString(),
                            server_status = dt["server_status"].ToString(),
                            total_count = dt["total_count"].ToString(),
                            country_name = dt["country_name"].ToString(),

                        });
                        values.server_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

               
            }
        }
        public void DaUpdateServer(string employee_gid,MdlSubManagement values)
        {
            
            msSQL = "update vcxcontroller.adm_mst_tserver set " +
                    " server_name='" + values.server_name + "'," +
                    "hosting_details='" + values.hosting_details + "'," +
                    "token_no='" + values.token_number + "'," +
                    "server_ip='" + values.server_ipaddress + "'," +
                    "server_status='" + values.server_status + "'," +
                     "country_name='" + values.cbopermanent_country + "'" +
                    "where server_gid='" + values.server_gid + "'";
            mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnresult == 1)
            {
               
                    values.message = "Server Details updated successfully";
                    values.status = true;
                
            }
            else
            {
                values.message = "Error occured while updating Server!";
            }
        }

        public void DaGetConsumerSummary(MdlSubManagement values)
        {
            try
            {

                msSQL = " select consumer_gid,server_gid,company_code,consumer_url,server_name,subscription_details,date_format(start_date,'%d-%m-%Y')as start_date,date_format(end_date,'%d-%m-%Y')as end_date,status,active_status from vcxcontroller.adm_mst_tconsumer order by consumer_gid desc; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<consumer_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new consumer_list
                        {

                            consumer_gid = dt["consumer_gid"].ToString(),
                            server_gid = dt["server_gid"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            consumer_url = dt["consumer_url"].ToString(),
                            server_name = dt["server_name"].ToString(),
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            subscription_details = dt["subscription_details"].ToString(),
                            active_status = dt["active_status"].ToString(),

                        });
                        values.consumer_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {


            }
        }
        public void DaUpdateConsumer(string employee_gid, MdlSubManagement values)
        {
            msSQL = "update vcxcontroller.adm_mst_tconsumer set " +
                    " company_code='" + values.company_code + "'," +
                    "consumer_url='" + values.consumer_url + "'," +
                    "subscription_details='" + values.subscription_details + "'," +
                    "start_date='" + Convert.ToDateTime(values.from).ToString("yyyy-MM-dd") + "'," +
                    "end_date='" + Convert.ToDateTime(values.to).ToString("yyyy-MM-dd") + "'" +
                    "where consumer_gid='" + values.consumer_gid + "'";
            mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnresult == 1)
            {

                values.message = "Consumer updated successfully";
                values.status = true;

            }
            else
            {
                values.message = "Error occured while updating Server!";
            }
        }
        public void DaPostServer(string employee_gid, MdlSubManagement values)
        {

            //msGetGid = objcmnfunctions.GetMasterGID("SERV");
            msSQL = "insert into vcxcontroller.adm_mst_tserver(" +
                        "server_name," +
                        "hosting_details," +
                        "token_no," +
                        "server_ip, " +
                         "country_name, " +
                           "server_status, " +
                        "created_by," +
                        "created_date)" +
                        "values(" +
                        "'" + values.server_name + "'," +
                        "'" + values.hosting_details + "'," +
                        "'" + values.token_number + "'," +
                        "'" + values.server_ipaddress + "'," +
                        "'" + values.cbopermanent_country + "'," +
                          "'" + values.server_status + "'," +
                        "'" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnresult != 0)
            {
                values.status = true;
                values.message = "Server Details Added Successfully";
            }
            else
            {
                values.message = "Error occured while onboarding!";
            }
        }
        public void DaGetServerView(string server_gid,MdlSubManagement values)
        {
            try
            {

                msSQL = " select consumer_gid,server_gid,company_code,consumer_url,subscription_details from vcxcontroller.adm_mst_tconsumer where server_gid= '" + server_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<server_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new server_list
                        {

                            consumer_gid = dt["consumer_gid"].ToString(),
                            server_gid = dt["server_gid"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            consumer_url = dt["consumer_url"].ToString(),
                            subscription_details = dt["subscription_details"].ToString(),

                        });
                        values.server_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {


            }

            msSQL = " Select server_gid,server_name,hosting_details,token_no,server_ip,server_status,country_name from " +
                       " vcxcontroller.adm_mst_tserver where " +
                       " server_gid = '" + server_gid + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {
                values.server_name = objOdbcDataReader["server_name"].ToString();
                values.hosting_details = objOdbcDataReader["hosting_details"].ToString();
                values.token_number = objOdbcDataReader["token_no"].ToString();
                values.server_ipaddress = objOdbcDataReader["server_ip"].ToString();
                values.server_status = objOdbcDataReader["server_status"].ToString();
                values.country_name = objOdbcDataReader["country_name"].ToString();
            }
            objOdbcDataReader.Close();
            values.status = true;

        }
        public void DaInactiveConsumer(consumerinactive values, string user_gid)
        {
            try
            {
                               
                    msSQL = " update vcxcontroller.adm_mst_tconsumer set active_status='" + values.rbo_status + "'," +
                    " remarks='" + values.remarks.Replace("'", "''") + "'" +
                    " where consumer_gid='" + values.consumer_gid + "' ";
                    mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnresult != 0)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("COLO");

                        msSQL = " insert into vcxcontroller.adm_mst_tconsumerlog (" +
                              " consumer_gid," +
                              " status," +
                              " remarks," +
                              " updated_by," +
                              " updated_date) " +
                              " values (" +
                              " '" + values.consumer_gid + "'," +
                              " '" + values.rbo_status + "'," +
                              " '" + values.remarks.Replace("'", "''") + "'," +
                              " '" + user_gid + "'," +
                              " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (values.rbo_status == "N")
                        {
                            values.status = true;
                            values.message = "Consumer Inactivated Successfully";
                        }
                        else
                        {
                            values.status = true;
                            values.message = "Consumer Activated Successfully";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Status Consumer";
                    }

                
            }
            catch (Exception ex)
            {
               
                values.status = false;
            }
        }

        public void DaInactiveConsumerHistory(consumerhistory values, string consumer_gid)
        {
            try
            {
                msSQL = " select a.remarks,date_format(a.updated_date,'%d-%m-%Y %h:%i %p') as updated_date, " +
                " concat(b.user_firstname,' ',b.user_lastname,' || ',b.user_code) as updated_by," +
                " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                " from vcxcontroller.adm_mst_tconsumerlog a " +
                 " left join vcxcontroller.adm_mst_tuser b on a.updated_by = b.user_gid " +
                " left join vcxcontroller.hrm_mst_temployee c on b.user_gid = c.user_gid" +
                " where a.consumer_gid='" + consumer_gid + "' order by a.consumerlog_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getinactivehistory_list = new List<consumerhistory_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getinactivehistory_list.Add(new consumerhistory_list
                        {
                            statuses = (dr_datarow["status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString())
                        });
                    }
                    values.consumerhistory_list = getinactivehistory_list;
                }

                values.status = true;
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                
                values.status = false;
            }
        }
        public void DaGetConsumerEdit(MdlSubManagement values, string consumer_gid)
        {
            try
            {
                msSQL = " SELECT consumer_gid, company_code,active_status" +
                        " FROM vcxcontroller.adm_mst_tconsumer" +
                        " WHERE consumer_gid='" + consumer_gid + "'";
                objODBCdatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCdatareader.HasRows == true)
                {
                    values.consumer_gid = objODBCdatareader["consumer_gid"].ToString();
                    values.company_code = objODBCdatareader["company_code"].ToString();
                    values.active_status = objODBCdatareader["active_status"].ToString();

                }
                objODBCdatareader.Close();

            }
            catch (Exception ex)
            {
               
                values.status = false;

            }
        }
        public bool DaPopCountry(MdlSubManagement values)
        {
            try
            {
                msSQL = " Select country_gid,country_code,country_name From vcxcontroller.adm_mst_tcountry ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_country = new List<country>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_row in dt_datatable.Rows)
                    {
                        get_country.Add(new country
                        {
                            country_gid = dr_row["country_gid"].ToString(),
                            country_name = dr_row["country_name"].ToString()
                        });
                    }
                    values.country = get_country;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                return false;
            }
        }
        public void DaPostScriptDocumentUpload(HttpRequest httpRequest, scriptuploaddocument objfilename, string employee_gid)
        {
            // upload_list objdocumentmodel = new upload_list();
            HttpFileCollection httpFileCollection;

            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;

            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            string pdfFilName = string.Empty;

            string lsdocumenttype_gid = string.Empty;
            //string scriptdocument_name = httpRequest.Form["file"];
            string server_name = httpRequest.Form["server_name"];
            string company_name = httpRequest.Form["company_code"];
            string filename_status = httpRequest.Form["filename_status"];

            //string scriptdocumentList = httpRequest.Form["scriptdocumentList"];
            //List<UploadDocument> scriptDocuments = JsonConvert.DeserializeObject<List<UploadDocument>>(scriptdocumentList);

            String path = lspath;
            HttpPostedFile httpPostedFile;



            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            path = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Subscription/ScriptDocument/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

            if ((!System.IO.Directory.Exists(path)))
                System.IO.Directory.CreateDirectory(path);

            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {

                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        string scriptdocument_name = httpPostedFile.FileName;
                        //var getdocumentdtl = scriptDocuments.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault();
                        //string lsfile_gid = msdocument_gid + FileExtension;
                        string lsfile_gid = msdocument_gid;
                        string project_flag = httpRequest.Form["project_flag"].ToString();
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(scriptdocument_name);
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        MemoryStream ms = new MemoryStream();
                        ls_readStream.CopyTo(ms);

                        byte[] bytes = ms.ToArray();
                        //if ((objcmnstorage.CheckIsValidfilename(FileExtension, project_flag) == false) || (objcmnstorage.CheckIsExecutable(bytes) == true))
                        //{
                        //    objfilename.status = false;
                        //    objfilename.message = "File format is not supported";
                        //    return;
                        //}

                        string status;
                        status = objcmnfunctions.uploadFile(path, lsfile_gid);
                        ms.Close();
                        lspath = "erpdocument" + "/" + lscompany_code + "/" + "Subscription/ScriptDocument/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        msSQL = "select server_name from vcxcontroller.adm_mst_tserver where server_gid='" + server_name + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {

                            lsserver_name = objOdbcDataReader["server_name"].ToString();
                        }
                        objOdbcDataReader.Close();

                        //if(scriptdocument_name.EndsWith(fileNameWithoutExtension) != filename_status)
                        //{
                        //    objfilename.status = false;
                        //    objfilename.message = "File name and upload file is mismatched";
                        //    return;
                        //}
                      
                        msGetGid = objcmnfunctions.GetMasterGID("SERV");
                        msSQL = " insert into sub_mst_tdbscriptmanagementdocument(" +
                                    " dbscriptmanagementdocument_gid," +
                                    " server_name," +
                                    " company_code," +
                                    " file_name ," +
                                    " file_path," +
                                    " created_by," +
                                    " created_date" +
                                    " )values(" +
                                    "'" + msGetGid + "'," +
                                   "'" + lsserver_name + "'," +
                                    "'" + company_name + "'," +
                                    "'" + scriptdocument_name + "'," +
                                    "'" + lspath + msdocument_gid + FileExtension + "'," +
                                    "'" + employee_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if(mnresult !=0)
                    {
                        string filepath = ConfigurationManager.AppSettings["file_path"] + "/" + lspath + msdocument_gid + FileExtension;

                        // Use them one by one
                        string[] companyNames = company_name.Split(',');

                        foreach (string dbcompany_code in companyNames)
                        {
                            string lsConnectionString = ConfigurationManager.ConnectionStrings["AuthConn"].ConnectionString;

                            msSQL = "select concat(hosting_details,'Database=" + dbcompany_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver where server_gid='" + server_name + "'";
                            //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows == true)
                            {

                                lsviewhostingconnectionstring = objOdbcDataReader["hosting_details"].ToString();
                            }
                            objOdbcDataReader.Close();

                            msSQL = "select server_name from vcxcontroller.adm_mst_tserver where server_gid='" + server_name + "'";
                            //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows == true)
                            {

                                lsserver_name = objOdbcDataReader["server_name"].ToString();
                            }
                            objOdbcDataReader.Close();

                            string GetFilesinPath = System.IO.File.ReadAllText(filepath);

                            if (scriptdocument_name.EndsWith("SP.sql"))
                            {
                                try
                                {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "CREATE PROCEDURE`" + dbcompany_code + "`." + "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + dbcompany_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

                                        //Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "DROP PROCEDURE IF EXISTS `" + dbcompany_code + "`.").Replace("CREATE PROCEDURE", "DROP PROCEDURE IF EXISTS `" + dbcompany_code + "`.");

                                        //// Splitting the script into individual statements
                                        //createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

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

                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscriptmanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "sp");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.Message.Contains("already exists"))
                                                    {
                                                        LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");


                                                        using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                        {
                                                            connection.Open();
                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                            duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                            duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                            duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                            duplicateCommand.Parameters.AddWithValue("?", "sp");
                                                            duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                            duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                            duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                            mnresult = duplicateCommand.ExecuteNonQuery();
                                                            mnresult = 1;

                                                            connection.Close();

                                                        }


                                                    }
                                                    else
                                                    {
                                                        // Handle other SQL exceptions
                                                        throw;
                                                    }
                                                }

                                            }

                                        }
                                        viewconnection.Close();

                                    }

                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Contains("already exists"))
                                    {
                                        LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                        // Handle the specific case where the table already exists
                                    }
                                    else
                                    {
                                        // Handle other SQL exceptions
                                        throw;
                                    }
                                }

                            }
                            else if (scriptdocument_name.EndsWith("Table.sql"))
                            {
                                Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + dbcompany_code + "`." + "");
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

                                                using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                {
                                                    connection.Open();
                                                    string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscriptmanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                    OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                    duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                    duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                    duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                    duplicateCommand.Parameters.AddWithValue("?", "Table");
                                                    duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                    duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                    duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                    mnresult = duplicateCommand.ExecuteNonQuery();
                                                    mnresult = 1;

                                                    connection.Close();

                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                if (ex.Message.Contains("already exists"))
                                                {
                                                    LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "Table");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }

                                                }
                                                else
                                                {
                                                    // Handle other SQL exceptions
                                                    throw;
                                                }
                                            }
                                        }
                                    }
                                    viewconnection.Close();

                                }


                            }
                                else if (scriptdocument_name.EndsWith("SPDrop.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("Drop Procedure", "Drop Procedure`" + dbcompany_code + "`." + "");
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

                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscriptmanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "SP");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.Message.Contains("already exists"))
                                                    {
                                                        LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                        using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                        {
                                                            connection.Open();
                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                            duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                            duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                            duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                            duplicateCommand.Parameters.AddWithValue("?", "SP");
                                                            duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                            duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                            duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                            mnresult = duplicateCommand.ExecuteNonQuery();
                                                            mnresult = 1;

                                                            connection.Close();

                                                        }

                                                    }
                                                   else if (ex.Message.Contains("does not exist"))
                                                    {
                                                        LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                        using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                        {
                                                            connection.Open();
                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                            duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                            duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                            duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                            duplicateCommand.Parameters.AddWithValue("?", "SP");
                                                            duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                            duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                            duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                            mnresult = duplicateCommand.ExecuteNonQuery();
                                                            mnresult = 1;

                                                            connection.Close();

                                                        }

                                                    }
                                                    else
                                                    {
                                                        // Handle other SQL exceptions
                                                        throw;
                                                    }
                                                }
                                            }
                                        }
                                        viewconnection.Close();

                                    }


                                }
                                else if (scriptdocument_name.EndsWith("TableDrop.sql"))
                                {
                                    Scripts = GetFilesinPath.Replace("Drop TABLE", "Drop TABLE `" + dbcompany_code + "`." + "");
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

                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscriptmanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "Table");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.Message.Contains("already exists"))
                                                    {
                                                        LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                        using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                        {
                                                            connection.Open();
                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                            duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                            duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                            duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                            duplicateCommand.Parameters.AddWithValue("?", "Table");
                                                            duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                            duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                            duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                            mnresult = duplicateCommand.ExecuteNonQuery();
                                                            mnresult = 1;

                                                            connection.Close();

                                                        }

                                                    }
                                                   else if (ex.Message.Contains("does not exist"))
                                                    {
                                                        LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                        using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                        {
                                                            connection.Open();
                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                            duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                            duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                            duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                            duplicateCommand.Parameters.AddWithValue("?", "Table");
                                                            duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                            duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                            duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                            mnresult = duplicateCommand.ExecuteNonQuery();
                                                            mnresult = 1;

                                                            connection.Close();

                                                        }

                                                    }
                                                    else
                                                    {
                                                        // Handle other SQL exceptions
                                                        throw;
                                                    }
                                                }
                                            }
                                        }
                                        viewconnection.Close();

                                    }


                                }
                                else if (scriptdocument_name.EndsWith("Alter.sql"))
                            {

                                string Scripts = GetFilesinPath.Replace("ALTER TABLE", "ALTER TABLE `" + dbcompany_code + "`." + "");

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

                                                using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                {
                                                    connection.Open();
                                                    string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscriptmanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                    OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                    duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                    duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                    duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                    duplicateCommand.Parameters.AddWithValue("?", "AlterTable");
                                                    duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                    duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                    duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                    mnresult = duplicateCommand.ExecuteNonQuery();
                                                    mnresult = 1;

                                                    connection.Close();

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (ex.Message.Contains("Duplicate column name"))
                                                {
                                                    LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "AlterTableDuplicateColumn");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }
                                                }
                                                else if (ex.Message.Contains("Unknown column"))
                                                {
                                                    // Handle unknown column issue
                                                    LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "AlterTableUnknownColumn");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }
                                                }
                                                else
                                                {
                                                    // Handle other SQL exceptions
                                                    throw;
                                                }
                                            }
                                        }
                                    }
                                    viewconnection.Close();
                                }


                            }
                            else if (scriptdocument_name.EndsWith("Update.sql"))
                            {
                                  string safeUpdatesOff = "SET SQL_SAFE_UPDATES = 0;";

                                    string Scripts = GetFilesinPath.Replace("Update", "Update `" + dbcompany_code + "`." + "");
                                    Scripts = safeUpdatesOff + Scripts;
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
                                                using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                {
                                                    connection.Open();
                                                    string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscriptmanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                    OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                    duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                    duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                    duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                    duplicateCommand.Parameters.AddWithValue("?", "Update");
                                                    duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                    duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                    duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                    mnresult = duplicateCommand.ExecuteNonQuery();
                                                    mnresult = 1;

                                                    connection.Close();

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (ex.Message.Contains("Duplicate column name"))
                                                {
                                                    // Handle duplicate column issue
                                                    LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");


                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "AlterTableDuplicateColumn");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }

                                                }
                                                else if (ex.Message.Contains("Unknown column"))
                                                {
                                                    // Handle unknown column issue
                                                    LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "AlterTableUnknownColumn");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }
                                                }
                                                else
                                                {
                                                    // Handle other SQL exceptions
                                                    throw;
                                                }
                                            }
                                        }
                                    }
                                    viewconnection.Close();
                                }


                            }

                                else if (scriptdocument_name.EndsWith("Delete.sql"))
                                {
                                    string safeUpdatesOff = "SET SQL_SAFE_UPDATES = 0;";

                                    string Scripts = GetFilesinPath.Replace("delete from", "Delete from `" + dbcompany_code + "`." + "");
                                    Scripts = safeUpdatesOff + Scripts;
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
                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscriptmanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "Delete");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.Message.Contains("Duplicate column name"))
                                                    {
                                                        // Handle duplicate column issue
                                                        LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");


                                                        using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                        {
                                                            connection.Open();
                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                            duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                            duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                            duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                            duplicateCommand.Parameters.AddWithValue("?", "Delete");
                                                            duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                            duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                            duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                            mnresult = duplicateCommand.ExecuteNonQuery();
                                                            mnresult = 1;

                                                            connection.Close();

                                                        }

                                                    }
                                                    else if (ex.Message.Contains("Unknown column"))
                                                    {
                                                        // Handle unknown column issue
                                                        LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");

                                                        using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                        {
                                                            connection.Open();
                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                            duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                            duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                            duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                            duplicateCommand.Parameters.AddWithValue("?", "Delete");
                                                            duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                            duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                            duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                            mnresult = duplicateCommand.ExecuteNonQuery();
                                                            mnresult = 1;

                                                            connection.Close();

                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Handle other SQL exceptions
                                                        throw;
                                                    }
                                                }
                                            }
                                        }
                                        viewconnection.Close();
                                    }


                                }

                                else if (scriptdocument_name.EndsWith("Metadata.sql"))
                            {
                                insertsqeriery = GetFilesinPath.Replace("INSERT INTO", "INSERT INTO `" + dbcompany_code + "`." + "");
                                string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);

                                using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                {
                                    foreach (var insert in metadatasforinsert)
                                    {
                                        if (insert.Trim() != "")
                                        {
                                            msSQL = insert.Trim();

                                            try
                                            {
                                                // Attempt to execute the insert query
                                                OdbcCommand metacommand = new OdbcCommand(msSQL, viewconnection);
                                                mnresult = metacommand.ExecuteNonQuery();
                                                mnresult = 1; // Assuming the insert was successful

                                                using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                {
                                                    connection.Open();
                                                    string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscriptmanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                    OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                    duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                    duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                    duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                    duplicateCommand.Parameters.AddWithValue("?", "Metadata");
                                                    duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                    duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                    duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                    mnresult = duplicateCommand.ExecuteNonQuery();
                                                    mnresult = 1;

                                                    connection.Close();

                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                if (ex.Message.Contains("Duplicate entry"))
                                                {
                                                    LogForAudit(dbcompany_code, "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");
                                                    using (OdbcConnection connection = new OdbcConnection(lsConnectionString))
                                                    {
                                                        connection.Open();
                                                        string moveToDuplicateTable = "INSERT INTO vcxcontroller.sub_mst_tdbscripterrormanagement (dbscriptmanagementdocument_gid,company_code, server_name,script_name,execute_query,created_by,created_date) VALUES (?,?,?,?,?,?,?)";
                                                        OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                        duplicateCommand.Parameters.AddWithValue("?", msGetGid);
                                                        duplicateCommand.Parameters.AddWithValue("?", dbcompany_code);
                                                        duplicateCommand.Parameters.AddWithValue("?", lsserver_name);
                                                        duplicateCommand.Parameters.AddWithValue("?", "MetaDuplicate");
                                                        duplicateCommand.Parameters.AddWithValue("?", msSQL);
                                                        duplicateCommand.Parameters.AddWithValue("?", employee_gid);
                                                        duplicateCommand.Parameters.AddWithValue("?", DateTime.Now.ToString("yyyy-MM-dd"));

                                                        mnresult = duplicateCommand.ExecuteNonQuery();
                                                        mnresult = 1;

                                                        connection.Close();

                                                    }
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

                            else
                            {
                                objfilename.status = false;
                                objfilename.message = "File Format does not supported";
                                    return;
                            }
                        }

                        if (mnresult != 0)
                        {
                            objfilename.status = true;
                            objfilename.message = "Document Uploaded Successfully";

                        }
                    }

                        else
                        {
                            objfilename.status = false;
                            objfilename.message = "Error Occured While Uploading the document";

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "subscription");
                ex.StackTrace.ToString();
                objfilename.message = "Error Occured While Uploading the document";
                objfilename.status = false;
            }
            Scripts = string.Empty;
            insertsqeriery = string.Empty;
        }
        public void LogForAudit(string company_code, string content, string path)
        {
            try
            {
                string lspath = ConfigurationManager.AppSettings["file_path"].ToString() + "/erp_documents/" + company_code + "/ErrorLogs";
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);

                lspath = lspath + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                System.IO.StreamWriter sw = new System.IO.StreamWriter(lspath, true);
                sw.WriteLine(content);
                sw.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void DaPostConsumer(string employee_gid, MdlSubManagement values)
        {

            //msGetGid = objcmnfunctions.GetMasterGID("SERV");
            msSQL = "insert into vcxcontroller.adm_mst_tconsumer(" +
                        "server_name," +
                        "company_code," +
                        "consumer_url," +
                        "start_date," +
                        "end_date, " +                         
                        "created_by," +
                        "created_date)" +
                        "values(" +
                        "'" + values.server_name + "'," +
                        "'" + values.company_name + "'," +
                        "'" + values.consumer_url + "'," +
                        "'" + Convert.ToDateTime(values.from).ToString("yyyy-MM-dd") + "'," +
                        "'" + Convert.ToDateTime(values.to).ToString("yyyy-MM-dd") + "'," +                       
                        "'" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnresult != 0)
            {
                values.status = true;
                values.message = "Consumer Details Added Successfully";
            }
            else
            {
                values.message = "Error occured while onboarding!";
            }
        }
    }
}
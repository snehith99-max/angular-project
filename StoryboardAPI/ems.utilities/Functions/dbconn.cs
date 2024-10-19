using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text.Json;
using System.Web;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Data.Odbc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
namespace ems.utilities.Functions
{
    public class dbconn
    {
        private string lsConnectionString = string.Empty;

        // Get Connection String 

        public string GetConnectionString(string companyCode = "")
        {
            try
            {

                if (HttpContext.Current.Request.Headers["Authorization"] == null || HttpContext.Current.Request.Headers["Authorization"] == "null")
                {
                    lsConnectionString = ConfigurationManager.ConnectionStrings["AuthConn"].ConnectionString;
                }
                else
                {
                    using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["AuthConn"].ToString()))
                    {
                        using (OdbcCommand cmd = new OdbcCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "CALL adm_mst_spgetconnectionstring('" + HttpContext.Current.Request.Headers["Authorization"].ToString() + "')";
                            cmd.Connection = conn;
                            conn.Open();
                            lsConnectionString = cmd.ExecuteScalar()?.ToString();
                            conn.Close();

                            if (string.IsNullOrEmpty(lsConnectionString))
                            {
                                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                                {
                                    ReasonPhrase = "Conflict: Connection string not found."
                                });
                            }
                        }
                    }
                }
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + e.Message.ToString() + "***********GetConnectionString");
                lsConnectionString = "error";
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = "Internal server error."
                });
            }
            return lsConnectionString;
        }

        public class MdlCmnConn
        {
            public string connection_string { get; set; }
            public string company_code { get; set; }
            public string company_dbname { get; set; }
        }

        // Open Connection 

        public OdbcConnection OpenConn(string companyCode = "")
        {
            try
            {
                OdbcConnection gs_ConnDB;
                gs_ConnDB = new OdbcConnection(GetConnectionString(companyCode));
                if (gs_ConnDB.State != ConnectionState.Open)
                {
                    gs_ConnDB.Open();
                }
                return gs_ConnDB;
            }
            catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
            catch (Exception e)
            {
                LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + e.Message.ToString() + "***********OpenConn");
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "UnAuthorized" };
                throw new HttpResponseException(msg);
            }

        }

        // Close Connection
        public void CloseConn()
        {
            try
            {
                if (OpenConn().State != ConnectionState.Closed)
                {
                    OpenConn().Dispose();
                    OpenConn().Close();
                }
            }
            
             catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
        }

        // Execute a Query

        public int ExecuteNonQuerySQL(string query, string user_gid = null, string module_reference = null, string module_name = "Log")
        {
            int mnResult = 0;
            try
            {
                OdbcConnection ObjOdbcConnection = OpenConn();
                try
                {
                    OdbcCommand cmd = new OdbcCommand(query, ObjOdbcConnection);
                    mnResult = cmd.ExecuteNonQuery();
                    mnResult = 1;
                }
                catch (Exception e)
                {
                    LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + e.Message.ToString() + "*****Query****" + query + "*******Apiref********" + module_reference);
                }
                ObjOdbcConnection.Close();
                return mnResult;
                }
            catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
        }
        public int ExecuteNonQuerySQLForgot(string query, string companyCode = "", string user_gid = null, string module_reference = null, string module_name = "Log")
        {
            int mnResult = 0;
            try
            {
                OdbcConnection ObjOdbcConnection = OpenConn(companyCode);
                try
                {
                    OdbcCommand cmd = new OdbcCommand(query, ObjOdbcConnection);
                    mnResult = cmd.ExecuteNonQuery();
                    mnResult = 1;
                }
                catch (Exception e)
                {
                    LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + e.Message.ToString() + "*****Query****" + query + "*******Apiref********" + module_reference);
                }
                ObjOdbcConnection.Close();
                return mnResult;
            }
            catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
        }

        // Get Scalar Value
        public string GetExecuteScalar(string query, string companyCode = "", string user_gid = null, string module_reference = null, string module_name = "Log")
        {
            string val="";
            try
            {
                OdbcConnection ObjOdbcConnection = OpenConn(companyCode);

                OdbcCommand cmd = new OdbcCommand(query, ObjOdbcConnection);
                var val1 = cmd.ExecuteScalar();
                val = val1 != null ? val1.ToString() : "";

                ObjOdbcConnection.Close();
            }
            catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
            catch (Exception e)
            {
                LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + e.Message.ToString() + "*****Query****" + query + "*******Apiref********" + module_reference);
            }
          
            return val;
        }

        // Get Data Reader
        public OdbcDataReader GetDataReader(string query, string companyCode = "", string user_gid = null, string module_reference = null, string module_name = "Log")
        {
            try
            {
                OdbcCommand cmd = new OdbcCommand(query, OpenConn(companyCode));
                OdbcDataReader rdr;
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //rdr.Read();
                return rdr;
            }
            catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
            catch (Exception e)
            {
                LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + e.Message.ToString() + "*****Query****" + query + "*******Apiref********" + module_reference);
                return null;
            }

        }

        // Get Data Table

        public DataTable GetDataTable(string query, string user_gid = null, string module_reference = null, string module_name = "Log")
        {
            try
            {
                OdbcConnection ObjOdbcConnection = OpenConn();
                DataTable dt = new DataTable();
                OdbcDataAdapter da = new OdbcDataAdapter(query, ObjOdbcConnection);
                da.Fill(dt);
                ObjOdbcConnection.Close();
                return dt;
            }
            catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
            catch (Exception e)
            {
                LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + e.Message.ToString() + "*****Query****" + query + "*******Apiref********" + module_reference);
                return null;
            }

        }

        // Get Data Set

        public DataSet GetDataSet(string query, string table, string user_gid = null, string module_reference = null, string module_name = "Log")
        {
            try
            {
                OdbcConnection ObjOdbcConnection = OpenConn();
                DataSet ds = new DataSet();
                OdbcDataAdapter da = new OdbcDataAdapter(query, ObjOdbcConnection);
                da.Fill(ds, table);
                ObjOdbcConnection.Close();
                return ds;
            }
            catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
            catch (Exception e)
            {
                LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + e.Message.ToString() + "*****Query****" + query + "*******Apiref********" + module_reference);
                return null;
            }

        }

        public DataTable GetDataTableSP(string storedProcedureName, params OdbcParameter[] parameters)
        {
            DataTable dt = new DataTable();

            try
            {
                using (OdbcConnection objOdbcConnection = OpenConn())
                {
                    using (OdbcCommand cmd = new OdbcCommand(storedProcedureName, objOdbcConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add the parameters to the command
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        using (OdbcDataAdapter adapter = new OdbcDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }

                return dt;
            }
            catch (HttpResponseException ex)
            {
                LogForAudit($"HTTP Response Exception: {ex.Response.ReasonPhrase.ToString()}");
                throw;
            }
            
        }

        public void LogForAudit(string content)
        {
            try
            {
                string company_code = HttpContext.Current.Request.Headers["c_code"];
                company_code = company_code == null ? "COMMON" : company_code.ToUpper();
                try
                {
                    string lspath = ConfigurationManager.AppSettings["log_path"].ToString() + "/erp_documents/" + company_code + "/ExceptionLog";
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
            catch
            {
            }
        }
    }
}
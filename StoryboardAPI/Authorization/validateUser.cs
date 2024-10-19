using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Runtime.Remoting.Contexts;
using Spire.Pdf.Barcode;
using System.Security.Cryptography.X509Certificates;

namespace StoryboardAPI.Authorization
{

    public class validateUser
    {
        dbconn objdbconn = new dbconn();
        OdbcDataReader objODBCDataReader;
        string mssql;
        public bool isvalid(string username, string password, string RouterPrefix = null,string companycode = null)
        {
            if (RouterPrefix == "api/InstituteLogin")
            {
                mssql = " SELECT institute_gid FROM " + companycode + ".law_mst_tinstitute " +
                    " WHERE institute_code='" + username + "' AND password='" + password + "'";
            }
            else if (RouterPrefix == "api/customerlogin")
            {
                mssql = " SELECT customer_gid FROM " + companycode + ".crm_mst_tcustomer " +
                   " WHERE eportal_emailid='" + username + "' AND eportal_password='" + password + "'";
            }
            else
            {
                mssql = " SELECT user_gid FROM " + companycode + ".adm_mst_tuser " +
                " WHERE user_code='" + username + "' AND user_password='" + password + "'";
            }           
            objODBCDataReader = objdbconn.GetDataReader(mssql, companycode);
            if (objODBCDataReader.HasRows)
            {
                objODBCDataReader.Close();
                return true;
            }
            else
            {
                objODBCDataReader.Close();
                return false;
            }
        }       
        public class MdlCmnConn
        {
            public string connection_string { get; set; }
            public string company_code { get; set; }
            public string company_dbname { get; set; }
        }
    }
}
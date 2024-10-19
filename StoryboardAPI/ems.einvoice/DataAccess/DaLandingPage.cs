﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;

using ems.einvoice.Models;

namespace ems.einvoice.DataAccess
{
    public class DaLandingPage
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid2, msGetGid, msGetGid3, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaLandingSummary(MdlLandingPage values)
        {
            try
            {
                msSQL = "select count(*) as total_raised_invoice, (select count(*)  from rbl_trn_tinvoice where irn is null) " +
                    " as 'irn_pending', (select count(*) from rbl_trn_tinvoice where irn is not null) as 'irn_generated' " +
                    " from rbl_trn_tinvoice ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getlandinglist = new List<LandingpageSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        getlandinglist.Add(new LandingpageSummary
                        {
                            total_raised_invoice = row["total_raised_invoice"].ToString(),
                            irn_generated = row["irn_generated"].ToString(),
                            irn_Pending = row["irn_pending"].ToString()

                        });
                        values.LandingpageSummary = getlandinglist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Land summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
         
        }
    }
}
﻿using ems.payroll.Models;

using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

using ems.payroll.Models;

using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Collections.Specialized.BitVector32;
using MySql.Data.MySqlClient;



namespace ems.payroll.DataAccess
{
    public class DaPayRptPaymentSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msGetloangid;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        public void DaGetPaymentSummary(MdlPayRptPaymentSummary values)
        {
            try
            {
                
                msSQL = " select sum(net_salary) as payment_amount,count(employee_gid) as employee_count, payment_month, payment_year " +
                    " from pay_trn_tpayment ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPaymentReportlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPaymentReportlist
                        {
                            employee_count = dt["employee_count"].ToString(),
                            payment_month = dt["payment_month"].ToString(),
                            payment_year = dt["payment_year"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),

                        });
                        values.paymentreportlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment Summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

    }
}

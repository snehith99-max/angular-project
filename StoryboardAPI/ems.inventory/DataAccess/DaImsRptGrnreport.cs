using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Diagnostics.Eventing.Reader;

namespace ems.inventory.DataAccess
{
    public class DaImsRptGrnreport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsstart_date, lsend_date, lsMainbranch_flag;

        public void DaGetBranch(MdlImsRptGrnReport values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select branch_name,branch_gid " +
                    " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branch>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branch
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.branch = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetVendor(MdlImsRptGrnReport values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select a.vendor_gid,a.vendor_companyname " +
                        " from acp_mst_tvendor a " +
                        " order by a.vendor_companyname asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<vendor_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new vendor_list
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                        });
                        values.vendor_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaGetImsRptGrnreport(MdlImsRptGrnReport values)
        {
            try
            {

                msSQL = " SELECT a.grn_gid,a.purchaseorder_gid, b.vendor_companyname, b.vendor_gid, a.grn_status, a.grn_flag, a.invoice_flag,g.branch_name,e.costcenter_name, " +
                " date_format(a.grn_date, '%d-%m-%Y') as grn_date,g.branch_gid,g.branch_prefix, " +
                " CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag " +
                " else a.grn_flag end as 'overall_status', " +
                " date_format(a.approved_date, '%d-%m-%Y') as approved_date, (c.user_firstname) as approved_by " +
                " FROM pmr_trn_tgrn a " +
                " left join acp_mst_tvendor b on b.vendor_gid = a.vendor_gid " +
                " left join adm_mst_tuser c on c.user_gid = a.approved_by " +
                " left join hrm_mst_tbranch g on g.branch_gid=a.branch_gid" +
                " left join pmr_trn_tpurchaseorder d on a.purchaseorder_gid=d.purchaseorder_gid " +
                " left join pmr_mst_tcostcenter e on d.costcenter_gid =e.costcenter_gid " +
                " WHERE a.vendor_gid = b.vendor_gid order by a.grn_date desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<grn_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new grn_list
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            grn_status = dt["grn_status"].ToString(),
                            grn_flag = dt["grn_flag"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            grn_date = dt["grn_date"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            approved_date = dt["approved_date"].ToString(),
                            approved_by = dt["approved_by"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.grn_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetImsRptGrnreportsearch(string from_date, string to_date, string branch_gid, string vendor_gid, MdlImsRptGrnReport values)
        {
            try
            {

                if (string.IsNullOrEmpty(from_date) || string.IsNullOrEmpty(to_date))
                {
                    lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");
                    lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime from_date1 = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");

                    DateTime lsDateto = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");
                }
                msSQL = "  select mainbranch_flag from hrm_mst_tbranch" +
                        " where branch_gid='"+branch_gid+"'";
                lsMainbranch_flag= objdbconn.GetExecuteScalar(msSQL);
                msSQL = "SELECT a.grn_gid, a.purchaseorder_gid, b.vendor_companyname, b.vendor_gid, a.grn_status, a.grn_flag, " +
                               "a.invoice_flag, g.branch_prefix, e.costcenter_name, date_format(a.grn_date, '%d-%m-%Y') as grn_date, g.branch_gid, " +
                               "CASE WHEN a.invoice_flag <> 'IV Pending' THEN a.invoice_flag ELSE a.grn_flag END as 'overall_status', " +
                               "date_format(a.approved_date, '%d-%m-%Y') as approved_date, c.user_firstname as approved_by " +
                               "FROM pmr_trn_tgrn a " +
                               "LEFT JOIN acp_mst_tvendor b ON b.vendor_gid = a.vendor_gid " +
                               "LEFT JOIN adm_mst_tuser c ON c.user_gid = a.approved_by " +
                               "LEFT JOIN hrm_mst_tbranch g ON g.branch_gid = a.branch_gid " +
                               "LEFT JOIN pmr_trn_tpurchaseorder d ON a.purchaseorder_gid = d.purchaseorder_gid " +
                               "LEFT JOIN pmr_mst_tcostcenter e ON d.costcenter_gid = e.costcenter_gid " +
                               "WHERE a.vendor_gid = b.vendor_gid ";

                if (lsMainbranch_flag == "Y")
                {
                    msSQL += "AND a.branch_gid = '" + branch_gid + "'";
                }
                if (!string.IsNullOrEmpty(lsstart_date))
                {
                    msSQL += "AND a.grn_date >= '" + lsstart_date + "'";
                }
                if (!string.IsNullOrEmpty(lsend_date))
                {
                    msSQL += "AND a.grn_date <= '" + lsend_date + "'";
                }
                if (branch_gid == "" || branch_gid == "null" || branch_gid == "undefined")
                {

                }
                else
                {
                    msSQL += "AND g.branch_gid = '" + branch_gid + "'";
                }
                if (vendor_gid == "" || vendor_gid == "null" || vendor_gid =="undefined")
                {

                }

                else
                {
                    msSQL += "AND b.vendor_gid = '" + vendor_gid + "'";
                }

                msSQL += " ORDER BY a.grn_date DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<grn_list>();

                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new grn_list
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            grn_status = dt["grn_status"].ToString(),
                            grn_flag = dt["grn_flag"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            grn_date = dt["grn_date"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            approved_date = dt["approved_date"].ToString(),
                            approved_by = dt["approved_by"].ToString(),
                        });
                    }
                    values.grn_list = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

    }
}
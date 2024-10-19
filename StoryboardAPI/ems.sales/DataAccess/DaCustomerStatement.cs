using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Configuration;
using CrystalDecisions.Shared;
using ems.sales.Models;

namespace ems.sales.DataAccess
{
    public class DaCustomerStatement
    {
        string msSQL = string.Empty;
        dbconn objdbconn = new dbconn();
        string path, full_path = string.Empty;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        public Dictionary<string, object> DaGetCustomerStatementPDF(string customer_gid, MdlCustomerStatement values)
        {
            var ls_response = new Dictionary<string, object>();
            try
            {
                OdbcConnection myConnection = new OdbcConnection();
                myConnection.ConnectionString = objdbconn.GetConnectionString();
                OdbcCommand MyCommand = new OdbcCommand();
                MyCommand.Connection = myConnection;
                DataSet myDS = new DataSet();
                OdbcDataAdapter MyDA = new OdbcDataAdapter();

                msSQL = " select customer_name from crm_mst_tcustomer where customer_gid='" + customer_gid +"'";
                string customet_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select distinct a.invoice_gid,concat('Sales Invoice',' ',a.invoice_refno) as invoice_refno,a.invoice_status,a.invoice_flag,a.invoice_date,a.invoice_type,c.branch_name, " +
                    " FORMAT(a.advance_amount,2) as advance_amount,invoice_from, e.address1, e.city, e.zip_code, e.customercontact_name," +
                    " ifnull(cast(payment_amount * exchange_rate as decimal),0.00) as payment_amount,a.payment_date, " +
                    " CASE WHEN a.payment_flag <>'PY Pending' then a.payment_flag else invoice_flag END as 'overall_status',  " +
                    " concat(case when a.payment_date < now() then DATEDIFF(NOW(), a.payment_date) end,' days') AS expiry_days," +
                    " DATE_FORMAT(a.payment_date,'%d-%m-%Y') as due_date, d.customer_name," +
                    " ifnull(cast(a.invoice_amount*a.exchange_rate as decimal),0.00) as invoice_amount,invoice_date,  " +
                    " CONCAT(a.customer_name,'/',a.customer_contactperson,'/',a.customer_contactnumber) as companydetails, " +
                    " a.invoice_from,ifnull(cast((a.invoice_amount*a.exchange_rate) - (a.payment_amount*a.exchange_rate) as decimal),0.00) as outstanding_amount  " +
                    " from rbl_trn_tinvoice a  " +
                    " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid  " +
                    " left join hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                    " left join crm_mst_tcustomer d on d.customer_gid = a.customer_gid" +
                    " left join crm_mst_tcustomercontact e on e.customer_gid= d.customer_gid" +
                    " where 1=1 and a.invoice_amount  <> a.payment_amount and a.invoice_status <> 'Invoice Cancelled' and a.invoice_flag <> 'Invoice Cancelled' " +
                    " and a.customer_gid='" + customer_gid + "' " +
                    " group by a.invoice_gid  order by date(invoice_date) desc,invoice_date asc, invoice_gid desc ";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");

                msSQL = "select sum(case when datediff(now(), a.payment_date) <= 30 then a.invoice_amount  ELSE 0 END) AS 'up_to_30_days', " +
                    " sum(case when datediff(now(), a.payment_date) BETWEEN 61 AND 90  then a.invoice_amount else 0 end) as '61_to_90_days', " +
                    " sum(case when datediff(now(), a.payment_date) BETWEEN 31 AND 60  then a.invoice_amount else 0 end) as '31_to_60_days', " +
                    " sum(case when datediff(now(), a.payment_date) > 90 then b.amount else 0 end) as 'more_than_90_days', " +
                    " CONCAT('£ ', CAST(FORMAT(SUM(a.invoice_amount * a.exchange_rate), 2) AS CHAR), ' ', a.currency_code) AS DataColumn5, " +
                    " CONCAT('£ ', CAST(FORMAT(SUM((a.invoice_amount * a.exchange_rate) - COALESCE(b.amount, 0)), 2) AS CHAR), ' ', a.currency_code) AS DataColumn6  from " +
                    " rbl_trn_tinvoice a \r\n left join rbl_trn_tpayment b on a.invoice_gid=b.invoice_gid " +
                    " where a.customer_gid='"+ customer_gid + "'";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable2");

                msSQL = " select template_content as notes from adm_mst_ttemplate where templatetype_gid='3'";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");
                
                msSQL = " select 'Receipt' as invoice_ref , concat('- ',a.amount) as amount, a.payment_date as DataColumn3  from rbl_trn_tpayment a " +
                    " left join rbl_trn_tinvoice b on a.invoice_gid=b.invoice_gid" +
                    " where b.customer_gid ='" + customer_gid + "'";
                MyCommand.CommandText = msSQL; 
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable4");

                try
                {
                    ReportDocument oRpt = new ReportDocument();
                    string base_pathOF_currentFILE = AppDomain.CurrentDomain.BaseDirectory;
                    string report_path = Path.Combine(base_pathOF_currentFILE, "ems.sales", "Reports", "smr_crp_customerstatement.rpt");

                    if (!File.Exists(report_path))
                    {
                        values.status = false;
                        values.message = "Your Rpt path not found !!";
                        ls_response = new Dictionary<string, object>
                        {
                            {"status",false },
                            {"message",values.message}
                        };
                    }
                    oRpt.Load(report_path);
                    oRpt.SetDataSource(myDS);

                    path = Path.Combine(ConfigurationManager.AppSettings["report_path"]?.ToString());

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string PDFfile_name = customet_name + " Statement.pdf";
                    full_path = Path.Combine(path, PDFfile_name);

                    oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, full_path);
                    myConnection.Close();
                    ls_response = objFnazurestorage.reportStreamDownload(full_path);
                    values.status = true;

                }
                catch (Exception Ex)
                {
                    values.status = false;
                    values.message = Ex.Message;
                    ls_response = new Dictionary<string, object>
                    {
                        { "status", false },
                        { "message", Ex.Message }
                    };
                }
                finally
                {
                    if (full_path != null)
                    {
                        try
                        {
                            File.Delete(full_path);
                        }
                        catch (Exception Ex)
                        {
                            values.message = Ex.Message;
                            ls_response = new Dictionary<string, object>
                            {
                                { "status", false },
                                { "message", Ex.Message }
                            };
                        }
                    }
                }
                return ls_response;
            }
            catch (Exception ex)
            {
                values.message = ex.Message;
                ls_response = new Dictionary<string, object>
                {
                   { "status", false },
                   { "message", ex.Message }
                };
                return ls_response;
            }
        }
    }
}
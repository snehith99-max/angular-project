//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace ems.sales.DataAccess
//{
//    public class DaSmrTrnSO2PO
//    {
//    }
//}

using ems.utilities.Functions;
using ems.utilities.Models;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using ems.sales.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Drawing.ImageConverter;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using CrystalDecisions.Shared.Json;
using System.Threading.Tasks;
using OfficeOpenXml.Utils;
using System.Web.UI.WebControls;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnSO2PO 
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string lspop_server, lspop_mail, lspop_password, lscompany, lscompany_code;
        string msINGetGID, msGet_att_Gid, msenquiryloggid;
        string lspath, lspath1, lspath2, mail_path, mail_filepath, pdf_name = "";
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable mail_datatable, dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, lstaxpercentage, msPOGetGID, msPO1GetGID, msGetGID, lsreceived, lsReference_gid, lstPR_PO_flag,
            lspurchaserequisition_status, lsbsccallocation, lstax_gid4,
            mspGetGID, msGetGid, msGetGid1, msGetGID1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lspop_port;
        MailMessage message = new MailMessage();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string company_logo_path;
        //Image branch_logo;
        double lsPRqty_poadjusted, lsSum_poAdjusted, lsPRqty_ordered, lsSum_Ordered, lsPOqty_ordered;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();
        private List<GetTaxSegmentList> allTaxSegmentsList;

        //public void DaGetPurchaseOrderAddselect(string branch_gid, MdlSO2PO values)
        //{
        //    try
        //    {



        //        msSQL = " select /*+ MAX_EXECUTION_TIME(300000) */  distinct a.purchaserequisition_gid,DATE_FORMAT(x.approved_date, '%d-%m-%Y') approved_date, a.created_by," +
        //                " DATE_FORMAT( a.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date, a.purchaserequisition_status,l.costcenter_name," +
        //                " CASE when a.purchaseorder_flag <> 'PO Pending' then a.purchaseorder_flag" +
        //                " when a.purchaserequisition_flag='PR Approved' then 'PR Approved'" +
        //                " else a.purchaserequisition_flag end as 'overall_status'," +
        //                " a.purchaserequisition_remarks, concat(c.user_firstname,' / ',e.department_name) as pr_raisedby," +
        //                " concat(mc.user_firstname,' / ',me.department_name) as mr_raisedby,a.purchaserequisition_referencenumber " +
        //                " from pmr_trn_tpurchaserequisition a " +
        //                " left join pmr_trn_tpurchaserequisitiondtl b on b.purchaserequisition_gid = a.purchaserequisition_gid " +
        //                " left join ims_trn_tmaterialrequisition z on z.materialrequisition_gid=a.materialrequisition_gid" +
        //                " left join pmr_mst_tproduct h on h.product_gid = b.product_gid" +
        //                " left join adm_mst_tuser c on c.user_gid = a.created_by" +
        //                " left join hrm_mst_temployee d on d.user_gid = c.user_gid " +
        //                " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
        //                " left join pmr_mst_tcostcenter l on a.costcenter_gid=l.costcenter_gid " +
        //                " left join pmr_trn_tapproval x on a.purchaserequisition_gid=x.pr_gid " +
        //                " left join adm_mst_tuser mc on mc.user_gid = z.user_gid " +
        //                " left join hrm_mst_temployee md on md.user_gid = mc.user_gid " +
        //                " left join hrm_mst_tdepartment me on me.department_gid = md.department_gid " +
        //                " where a.purchaserequisition_gid = b.purchaserequisition_gid and " +
        //                " a.branch_gid = '" + branch_gid + "' and " +
        //                " a.purchaserequisition_flag not in( 'PR Pending Approval','PR Rejected','Pending New Product','PI Pending Approval','PO Raised')  and " +
        //                " a.purchaseorder_flag not in ('PO Raised','PO Cancelled')" +
        //                " group by a.purchaserequisition_gid Order by date(a.purchaserequisition_date) desc, x.approved_date desc,a.purchaserequisition_date asc, a.purchaserequisition_gid desc ";


        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getModuleList = new List<GetPurchaseOrder_lists1>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new GetPurchaseOrder_lists1
        //                {

        //                    purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
        //                    approved_date = dt["approved_date"].ToString(),
        //                    created_by = dt["created_by"].ToString(),
        //                    purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
        //                    purchaserequisition_status = dt["purchaserequisition_status"].ToString(),
        //                    costcenter_name = dt["costcenter_name"].ToString(),
        //                    overall_status = dt["overall_status"].ToString(),
        //                    purchaserequisition_remarks = dt["purchaserequisition_remarks"].ToString(),
        //                    pr_raisedby = dt["pr_raisedby"].ToString(),
        //                    mr_raisedby = dt["mr_raisedby"].ToString(),
        //                    purchaserequisition_referencenumber = dt["purchaserequisition_referencenumber"].ToString(),


        //                });
        //                values.GetPurchaseOrder_lists1 = getModuleList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {

        //        values.message = "Exception occured while getting PO summary!";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
        //      $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
        //      ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
        //      msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
        //      DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
        //    }



        //}

        public void DaGetSO2POSummary (string salesorder_gid, MdlSO2PO values)
        {
            try
            {



                msSQL = " select a.salesorderdtl_gid,a.salesorder_gid,a.product_gid,b.product_name,b.cost_price," +
                        " b.product_desc,d.productuom_gid,d.productuom_name, a.productgroup_gid,c.productgroup_name," +
                        " CASE  WHEN b.customerproduct_code IS NULL OR b.customerproduct_code = ''  THEN b.product_code ELSE " +
                        " b.customerproduct_code  END AS product_code from smr_trn_tsalesorderdtl a left join " +
                        " pmr_mst_tproduct b ON a.product_gid = b.product_gid  LEFT JOIN pmr_mst_tproductgroup c " +
                        " ON c.productgroup_gid = a.productgroup_gid  LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid  " +
                        " where salesorder_gid = '" + salesorder_gid + "' group by salesorderdtl_gid ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPurchaseOrder1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPurchaseOrder1
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_price = dt["cost_price"].ToString(),
                            qty_ordered = "0",
                            //qty_requested = dt["qty_requested"].ToString(),
                            //product_total = dt["product_price"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["product_desc"].ToString(),
                            salesorderdtl_gid = dt["salesorderdtl_gid"].ToString(),


                        });
                        values.GetPurchaseOrder1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();

                msSQL = "select file_name,file_path from smr_trn_tsalesorder where salesorder_gid='" + salesorder_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    values.file_name = objMySqlDataReader["file_name"].ToString();
                    values.file_path = objMySqlDataReader["file_path"].ToString();

                }
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }


        public void DaPostPoaddSubmit(string user_gid, string purchaserequisition_gid, MdlSO2PO values)
        {
            try
            {
                msSQL = " delete from pmr_tmp_tpurchaseorder where " +
                        " created_by = '" + user_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msGetGID1 = objcmnfunctions.GetMasterGID("PPOT");


                msSQL = " select a.purchaserequisition_gid,a.created_by, " +
                        " date_format(a.purchaserequisition_date,'%d-%m-%Y')as purchaserequisition_date,a.purchaserequisition_referencenumber, " +
                        " b.purchaserequisitiondtl_gid,b.uom_gid,b.display_field, b.qty_requested, b.qty_received, " +
                        "(b.qty_requested - b.qty_ordered - qty_poadjusted) as qty_outstanding, b.product_remarks, " +
                        " b.product_gid, b.product_code, b.product_name, b.productgroup_name, b.productuom_name, " +
                        " g.department_name from pmr_trn_tpurchaserequisition a " +
                        " left join pmr_trn_tpurchaserequisitiondtl b on a.purchaserequisition_gid = b.purchaserequisition_gid " +
                        " left join pmr_mst_tproduct c on b.product_gid = c.product_gid " +
                        " left join pmr_mst_tproductgroup d on c.productgroup_gid = d.productgroup_gid " +
                        " left join pmr_mst_tproductuom e on c.productuom_gid = e.productuom_gid " +
                        " left join hrm_mst_temployee f on f.user_gid = a.created_by " +
                        " left join hrm_mst_tdepartment g on g.department_gid = f.department_gid " +
                        "where a.purchaserequisition_gid in ('" + purchaserequisition_gid + "') " +
                        " and  b.qty_requested<>b.qty_ordered order by a.purchaserequisition_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGID = objcmnfunctions.GetMasterGID("PPOT");

                        if (msGetGID == "E") // Assuming "E" is a string constant
                        {
                            values.status = true;
                            values.message = "Create Sequence Code PPOT for Purchase Order Details";
                        }

                        msSQL = " insert into pmr_tmp_tpurchaseorder (" +
                                " tmppurchaseorderdtl_gid, " +
                                " tmppurchaseorder_gid, " +
                                " product_gid, " +
                                " product_code, " +
                                " product_name, " +
                                " productuom_name, " +
                                " uom_gid, " +
                                " qty_ordered, " +
                                " reference_gid, " +
                                " created_by, " +
                                " display_field, " +
                                " purchaserequisitiondtl_gid)" +
                                " values ( " +
                                "'" + msGetGID + "'," +
                                "'" + msGetGID1 + "'," +
                                "'" + dt["product_gid"].ToString() + "'," +
                                "'" + dt["product_code"].ToString() + "'," +
                                "'" + dt["product_name"].ToString() + "'," +
                                "'" + dt["productuom_name"].ToString() + "'," +
                                "'" + dt["uom_gid"].ToString() + "'," +
                                "'" + dt["qty_outstanding"].ToString() + "', " +
                                "'" + dt["purchaserequisition_gid"].ToString() + "', " +
                                "'" + user_gid + "', " +
                                 "'" + dt["display_field"].ToString() + "', " +
                               "'" + dt["purchaserequisitiondtl_gid"].ToString() + "') ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }









                    if (mnResult == 1)
                    {
                        values.status = true;
                        //values.message = "GRN Raised Successfully";

                    }
                    else
                    {
                        values.status = false;
                        //values.message = "Error While Raising GRN";
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raising GRN!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                   ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                   msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                   DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetPurchaseOrderSelect(string user_gid, MdlSO2PO values)
        {
            try
            {



                msSQL = "  select concat_ws(' - ',a.user_firstname,c.department_name) as employee_name, " +
                        " b.employee_emailid, b.employee_phoneno, c.department_name,b.employee_mobileno from " +
                        " adm_mst_tuser a left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid where a.user_gid = '" + user_gid + "' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getemployeelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getemployeelist
                        {


                            employee_name = dt["employee_name"].ToString(),
                            employee_emailid = dt["employee_emailid"].ToString(),
                            employee_phoneno = dt["employee_phoneno"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),

                        });
                        values.Getemployeelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }

        public void DaGetRaisePOSummary(string purchaserequisition_gid, MdlSO2PO values)
        {
            try
            {

                msSQL = " select  c.user_gid,b.vendor_gid,b.email_id,a.purchaserequisition_gid,  a.branch_gid," +
                    " DATE_FORMAT( a.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date, a.purchaserequisition_remarks," +
                    " a.purchaserequisition_referencenumber, a.created_by, " +
                    " a.created_date,  a.purchaserequisition_flag,  " +
                    " a.purchaserequisition_status,  a.product_count,  " +
                    " a.enquiry_raised,   a.type,  a.purchaseorder_raised, " +
                    " a.priority,  a.costcenter_gid,f.branch_name" +
                    " from pmr_trn_tpurchaserequisition a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid" +
                    " left join adm_mst_taddress w on b.address_gid = w.address_gid " +
                    " left join adm_mst_tuser c on c.user_gid = a.created_by  " +
                    " left join hrm_mst_temployee d on d.user_gid = c.user_gid  " +
                    " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
                    " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid   " +
                    " left join adm_mst_tuser g on g.user_gid = a.approved_by  " +
                    " left join pmr_mst_tcostcenter h on h.costcenter_gid = a.costcenter_gid " +
                    " left join adm_mst_tuser m on m.user_gid = a.created_by " +
                    " where a.purchaserequisition_gid='" + purchaserequisition_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRaisePO>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRaisePO
                        {

                            branch_name = dt["branch_gid"].ToString(),
                            vendor_companyname = dt["vendor_gid"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            requested_by = dt["user_gid"].ToString(),
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString()
                        });

                        values.GetRaisePO = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }




        public void DaGetProductsearchSummary1(string producttype_gid, string product_gid, string vendor_gid, MdlSO2PO values)
        {
            try
            {


                string lsSQLTYPE = "vendor";

                msSQL = "call pmr_mst_spproductsearch('" + lsSQLTYPE + "','" + product_gid + "', '" + vendor_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductseg>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductseg
                        {



                            tstaxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tstaxsegment_name = dt["taxsegment_name"].ToString(),
                            tstax_name = dt["tax_name"].ToString(),
                            tstax_gid = dt["tax_gid"].ToString(),
                            tstax_percentage = dt["tax_percentage"].ToString(),
                            tstax_amount = dt["tax_amount"].ToString(),
                            tsmrp_price = dt["mrp_price"].ToString(),
                            tscost_price = dt["cost_price"].ToString(),
                            tsproduct_gid = dt["product_gid"].ToString(),
                            tsproduct_name = dt["product_name"].ToString(),
                            tax_prefix = dt["tax_prefix"].ToString(),

                        });
                        values.GetProductseg = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }

        public void DaPostOverallSubmit(string user_gid, postpo_list values)
        {
            try

            {
                if (string.IsNullOrEmpty(values.poref_no))
                {

                    msGetGID1 = objcmnfunctions.GetMasterGID("PPOP");

                    foreach (var data in values.Posummary_list)
                    {

                        //if (Request.QueryString["po_type"].ToString() == "Single")

                        msSQL = " Update pmr_tmp_tpurchaseorder " +
                                " Set qty_ordered = '" + data.qty_ordered + "'" +
                                " where tmppurchaseorderdtl_gid = '" + data.tmppurchaseorderdtl_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;


                        }
                        else
                        {
                            values.status = false;
                            values.message = "Some Error occurred while Updating Quantity";
                        }

                        msGetGID = objcmnfunctions.GetMasterGID("PODC");

                        if (msGetGID == "E") // Assuming "E" is a string constant
                        {
                            values.status = true;
                            values.message = "Create Sequence Code PODC for Purchase Order Details";
                        }

                        msSQL = " select product_name from pmr_mst_tproduct where product_gid ='" + data.product_gid + "' ";
                        string lsproductname = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select  product_code from pmr_mst_tproduct where product_gid ='" + data.product_gid + "' ";
                        string lsproduct_code = objdbconn.GetExecuteScalar(msSQL);

                        //msSQL = "select productuom_name from pmr_mst_tproductuom where productuom_gid='" + data.productuom_gid + "' ";
                        //string lsproductuom_name = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name + "' ";
                        string lstax_gid1 = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name2 + "' ";
                        string lstax_gid2 = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name3 + "' ";
                        string lstax_gid3 = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + values.tax_name4 + "' ";
                        string lstax_prefix4 = objdbconn.GetExecuteScalar(msSQL);

                        //msSQL = " SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                        //        " e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                        //        " THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                        //        " a.cost_price, a.product_gid, a.product_name FROM acp_mst_ttaxsegment2product d " +
                        //        " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                        //        " LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                        //        " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE f.vendor_gid ='" + vendor_gid + " ' and a.product_gid='" + product_gid + "' group by a.product_gid, d.tax_gid ";

                        //dt_datatable = objdbconn.GetDataTable(msSQL);
                        //if (dt_datatable.Rows.Count != 0)
                        //{
                        //    foreach (DataRow dt in dt_datatable.Rows)
                        //    {
                        //int productprice = Convert.ToInt32(data.product_price);
                        //int qty_ordered = Convert.ToInt32(data.qty_ordered);
                        //int discount_percentage = Convert.ToInt32(data.discount_percentage);
                        //int discountAmount = (productprice * qty_ordered);
                        //int discount = (discountAmount * discount_percentage)/100;
                        msSQL = " insert into pmr_trn_tpurchaseorderdtl (" +
                                " purchaseorderdtl_gid, " +
                                " purchaseorder_gid, " +
                                " product_gid, " +
                                " product_code, " +
                                " product_name, " +
                                " productuom_name, " +
                                " uom_gid, " +
                                " producttype_gid, " +
                                " product_price, " +
                                " discount_percentage, " +
                                " discount_amount, " +
                                " tax_name, " +
                                " tax_name2, " +
                                " tax_name3, " +
                                " tax1_gid, " +
                                " tax2_gid, " +
                                " tax3_gid, " +
                                " tax_percentage, " +
                                " tax_percentage2, " +
                                " tax_percentage3, " +
                                " tax_amount, " +
                                " tax_amount2, " +
                                " tax_amount3, " +
                                " qty_ordered, " +
                                " display_field_name," +
                                " display_field_name_old," +
                                " product_price_L," +
                                " discount_amount_L," +
                                " tax_amount1_L," +
                                " tax_amount2_L," +
                                " needby_date," +
                                " producttotal_amount," +
                                " tax_amount3_L" +
                                " )values ( " +
                                "'" + msGetGID + "', " +
                                "'" + msGetGID1 + "'," +
                                "'" + data.product_gid + "', " +
                                "'" + (String.IsNullOrEmpty(data.product_code) ? data.product_code : data.product_code.Replace("'", "\\\'")) + "'," +
                                "'" + (String.IsNullOrEmpty(data.product_name) ? data.product_name : data.product_name.Replace("'", "\\\'")) + "'," +
                                "'" + (String.IsNullOrEmpty(data.productuom_name) ? data.productuom_name : data.productuom_name.Replace("'", "\\\'")) + "'," +
                                "'" + data.productuom_gid + "', " +
                                "'" + data.producttype_gid + "', " +
                                "'" + data.product_price + "', ";
                        if (string.IsNullOrEmpty(data.discount_percentage))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.discount_percentage + "',";
                        }
                        msSQL += "'" + data.productdiscount_amountvalue + "',";
                        msSQL += "'" + data.tax_name + "', " +
                         "'" + data.tax_name2 + "', " +
                         "'" + data.tax_name3 + "', " +
                         "'" + lstax_gid1 + "', " +
                         "'" + lstax_gid2 + "', " +
                         "'" + lstax_gid3 + "', ";
                        if (string.IsNullOrEmpty(data.tax_percentage))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.tax_percentage + "',";
                        }
                        if (string.IsNullOrEmpty(data.tax_percentage2))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.tax_percentage2 + "',";
                        }
                        if (string.IsNullOrEmpty(data.tax_percentage3))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.tax_percentage3 + "',";
                        }

                        if (string.IsNullOrEmpty(data.taxamount1))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount1 + "',";
                        }
                        if (string.IsNullOrEmpty(data.taxamount2))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount2 + "',";
                        }
                        if (string.IsNullOrEmpty(data.taxamount3))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount3 + "',";
                        }
                        msSQL += "'" + data.qty_ordered + "', " +
                                "'" + (String.IsNullOrEmpty(data.display_field) ? data.display_field : data.display_field.Replace("'", "\\\'")) + "'," +
                                "'" + (String.IsNullOrEmpty(data.display_field_old) ? data.display_field_old : data.display_field_old.Replace("'", "\\\'")) + "'," +
                                "'" + data.product_price + "',";
                        if (string.IsNullOrEmpty(data.productdiscount_amountvalue))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.productdiscount_amountvalue + "',";
                        }
                        if (string.IsNullOrEmpty(data.taxamount1))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount1 + "',";
                        }
                        if (string.IsNullOrEmpty(data.taxamount2))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount2 + "',";
                        }

                        msSQL += "'" + data.needby_date + "'," +
                                 "'" + data.producttotal_amount + "',";
                        if (string.IsNullOrEmpty(data.taxamount3))
                        {
                            msSQL += "'0.00')";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount3 + "')";
                        }

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;


                        }
                        else
                        {
                            values.status = false;
                            values.message = "Some Error occurred while Adding into PO ";
                        }


                        msSQL = " select purchaserequisition_gid,purchaserequisitiondtl_gid," +
                                " (qty_requested - qty_ordered - qty_poadjusted) as qty_outstanding,qty_poadjusted, " +
                                " product_gid,uom_gid " +
                                " from pmr_trn_tpurchaserequisitiondtl " +
                                " where purchaserequisitiondtl_gid='" + data.purchaserequisitiondtl_gid + "'";

                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                mspGetGID = objcmnfunctions.GetMasterGID("PPPP");

                                if (mspGetGID == "E") // Assuming "E" is a string constant
                                {
                                    values.status = true;
                                    values.message = "Create Sequence Code PPOT for Purchase Order Details";
                                }
                                msSQL = " select costcenter_gid from pmr_trn_tpurchaserequisition a where a.purchaserequisition_gid ='" + dt["purchaserequisition_gid"].ToString() + "' ";
                                string lscostcenter_gid = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = " insert into pmr_trn_tpr2po (" +
                                        " pr2po_gid, " +
                                        " purchaseorder_gid, " +
                                        " purchaseorderdtl_gid, " +
                                        " purchaserequisition_gid, " +
                                        " purchaserequisitiondtl_gid, " +
                                        " product_gid, " +
                                        " costcenter_gid, " +
                                        " qty_ordered," +
                                        " qty_poadjusted," +
                                        " display_field," +
                                        " created_by," +
                                        " created_date )" +
                                        " values ( " +
                                        "'" + mspGetGID + "', " +
                                        "'" + msGetGID1 + "'," +
                                        "'" + msGetGID + "', " +
                                        "'" + dt["purchaserequisition_gid"].ToString() + "'," +
                                        "'" + dt["purchaserequisitiondtl_gid"].ToString() + "'," +
                                        "'" + dt["product_gid"].ToString() + "'," +
                                        "'" + lscostcenter_gid + "', " +
                                         "'" + dt["qty_outstanding"].ToString() + "'," +
                                        "'" + dt["qty_poadjusted"].ToString() + "'," +
                                        "'" + (String.IsNullOrEmpty(data.display_field) ? data.display_field : data.display_field.Replace("'", "\\\'")) + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                                if (mnResult == 1)
                                {
                                    values.status = true;


                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Some Error occurred while Adding into PO Details";
                                }


                                msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                        " Set po_status = 'Y'" +
                                       " where purchaserequisitiondtl_gid='" + data.purchaserequisitiondtl_gid + "'";


                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    values.status = true;


                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Some Error occurred while Updating into Flag";
                                }

                                lsPRqty_ordered = double.Parse(dt["qty_outstanding"].ToString());
                                lsSum_Ordered = lsPRqty_ordered;
                                lsSum_poAdjusted = lsPRqty_poadjusted;
                                lsPOqty_ordered = double.Parse(data.qty_ordered);

                                msSQL = " select qty_ordered, qty_poadjusted from pmr_trn_tpurchaserequisitiondtl where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    objOdbcDataReader.Read();
                                    lsSum_Ordered = lsPOqty_ordered + double.Parse(objOdbcDataReader["qty_ordered"].ToString());

                                    objOdbcDataReader.Close();
                                }

                                msSQL = "select qty_requested from pmr_trn_tpurchaserequisitiondtl where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    objOdbcDataReader.Read();
                                    lsreceived = objOdbcDataReader["qty_requested"].ToString();


                                }
                                else
                                {
                                    lsreceived = "0";
                                }
                                objOdbcDataReader.Close();

                                //if (Request.QueryString["po_type"].ToString() == "Multiple")


                                //{ }

                                msSQL = " select purchaserequisitiondtl_gid from pmr_tmp_tpurchaseorder " +
                                        " where created_by='" + user_gid + "' and product_gid='" + data.product_gid + "' and uom_gid='" + data.productuom_gid + "'" +
                                        " order by qty_ordered desc";


                                dt_datatable = objdbconn.GetDataTable(msSQL);
                                if (dt_datatable.Rows.Count != 0)
                                {
                                    foreach (DataRow dta in dt_datatable.Rows)
                                    {
                                        msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                                " Set qty_ordered = qty_requested" +
                                                " where purchaserequisitiondtl_gid =   '" + dta["purchaserequisitiondtl_gid"].ToString() + "'";


                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    }
                                }
                                else
                                {

                                    msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                            " Set qty_ordered = '" + lsSum_Ordered + "'," +
                                            " qty_poadjusted = '" + lsSum_poAdjusted + "'" +
                                            " where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";


                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        values.status = true;

                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Some Error occurred while Adding into PO Details";
                                    }
                                }
                                //else
                                //{
                                //    msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                //           " Set qty_ordered = '" + lsSum_Ordered + "'," +
                                //           " qty_poadjusted = '" + lsSum_poAdjusted + "'" +
                                //           " where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";


                                //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                //}

                                lsReference_gid = dt["purchaserequisition_gid"].ToString();

                                msSQL = "select qty_requested, qty_ordered from pmr_trn_tpurchaserequisitiondtl  where purchaserequisition_gid ='" + lsReference_gid + "' and (qty_ordered + qty_poadjusted) < qty_requested ";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    lspurchaserequisition_status = "PR Work In Progress";
                                    lstPR_PO_flag = "PO Raised Partial";
                                }
                                else
                                {
                                    lspurchaserequisition_status = "PR Completed";
                                    lstPR_PO_flag = "PO Raised";
                                }
                                objOdbcDataReader.Close();

                                msSQL = " Update pmr_trn_tpurchaserequisition " +
                                        " Set purchaserequisition_status = '" + lspurchaserequisition_status + "'," +
                                        " purchaseorder_flag = '" + lstPR_PO_flag + "'" +
                                        " where purchaserequisition_gid = '" + lsReference_gid + "'";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                msSQL = " select distinct a.reference_gid,b.costcenter_gid from pmr_tmp_tpurchaseorder a" +
                                        " left join pmr_trn_tpurchaserequisition b on b.purchaserequisition_gid=a.reference_gid where a.created_by = '" + user_gid + "' group by b.costcenter_gid";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    lsbsccallocation = "PO BSCC Allocation Pending";

                                }
                                else
                                {
                                    lsbsccallocation = "Not Applicable";
                                }
                                objOdbcDataReader.Close();
                            }

                        }


                    }
                    string uiDateStr = values.po_date;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string po_date = uiDate.ToString("yyyy-MM-dd");
                    msSQL = "select poapproval_flag from adm_mst_tcompany";
                    string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currency_code + "'";
                    string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);


                    string uiDateStr2 = values.expected_date;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string expected_date = uiDate2.ToString("yyyy-MM-dd");

                    msSQL = " insert into pmr_trn_tpurchaseorder (" +
                         " purchaseorder_gid, " +
                         " purchaserequisition_gid, " +
                         " purchaseorder_reference, " +
                         " purchaseorder_date," +
                         " expected_date," +
                         " branch_gid, " +
                         " created_by, " +
                         " created_date," +
                         " vendor_gid, " +
                         " vendor_address, " +
                         " shipping_address, " +
                         " requested_by, " +
                         " freight_terms, " +
                         " payment_terms, " +
                         " requested_details, " +
                         " mode_despatch, " +
                         " currency_code," +
                         " exchange_rate," +
                         " po_covernote," +
                         " purchaseorder_remarks," +
                         " total_amount, " +
                         " termsandconditions, " +
                         " purchaseorder_status, " +
                         " purchaseorder_flag, " +
                         " poref_no, " +
                         " netamount, " +
                         " addon_amount," +
                         " freightcharges," +
                         " discount_amount," +
                         " tax_gid," +
                         " tax_percentage," +
                         " tax_amount," +
                         " roundoff, " +
                         " taxsegment_gid " +
                         " ) values (" +
                          "'" + msGetGID1 + "'," +
                          "'" + lsReference_gid + "'," +
                         "'" + msGetGID1 + "'," +
                         "'" + po_date + "', " +
                         "'" + expected_date + "', " +
                         "'" + values.branch_name + "'," +
                         "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                         "'" + (String.IsNullOrEmpty(values.vendor_companyname) ? values.vendor_companyname : values.vendor_companyname.Replace("'", "\\\'")) + "',";
                    if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                    {
                        msSQL += "'" + values.address1.Replace("'", "") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.address1 + "', ";
                    }
                    if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                    {
                        msSQL += "'" + values.shipping_address.Replace("'", "") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.shipping_address + "', ";
                    }
                    msSQL += "'" + values.employee_name + "'," +
                             "'" + values.delivery_terms + "'," +
                             "'" + values.payment_terms + "'," +
                             "'" + values.Requestor_details + "'," +
                             "'" + values.despatch_mode + "'," +
                             "'" + lscurrency_code + "'," +
                             "'" + values.exchange_rate + "'," +
                             "'" + (String.IsNullOrEmpty(values.po_covernote) ? values.po_covernote : values.po_covernote.Replace("'", "\\\'")) + "'," +
                             "'" + (String.IsNullOrEmpty(values.po_covernote) ? values.po_covernote : values.po_covernote.Replace("'", "\\\'")) + "'," +
                             "'" + values.grandtotal + "',";
                    if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                    {
                        msSQL += "'" + values.template_content.Replace("'", "") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.template_content + "', ";
                    }
                    if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                    {
                        msSQL += "'PO Approved',";
                    }
                    else
                    {
                        msSQL += "'Operation Pending PO',";
                    }
                    msSQL += "'" + "PO Approved" + "'," +
                         "'" + values.poref_no + "'," +
                          "'" + values.totalamount + "'," +
                         "'" + values.addoncharge + "'," +
                         "'" + values.freightcharges + "',";
                    if (string.IsNullOrEmpty(values.additional_discount))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";
                    }
                    msSQL += "'" + values.tax_name4 + "',";
                    //"'" + lstaxpercentage + "',";
                    if (string.IsNullOrEmpty(lstaxpercentage))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + lstaxpercentage + "',";
                    }

                    if (string.IsNullOrEmpty(values.tax_amount4))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount4 + "',";
                    }
                    if (values.roundoff == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.roundoff + "',";
                    }
                    msSQL += "'" + values.taxsegment_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    //msSQL = " select * from pmr_trn_tpurchaseorderdtl where " +
                    //                    " purchaseorder_gid = '" + msGetGID1 + "'and" +
                    //                    " producttype_gid = 'PT00010001206'";

                    //objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    //if (objOdbcDataReader.HasRows)
                    //{
                    //    msSQL = " update pmr_trn_tpurchaseorder " +
                    //            " set asset_flag = 'Y'" +
                    //            " where purchaseorder_gid = '" + msGetGID1 + "'";

                    //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    //}

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Raised PO Added Successfully";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Some Error occured while Adding record into Purchase Order Table";
                    }
                }

                else
                {
                    msSQL = "select poref_no from pmr_trn_tpurchaseorder where poref_no ='" + values.poref_no + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        values.message = "PO Ref No Alredy Exist!";
                        return;
                    }
                    else
                    {
                        msGetGID1 = objcmnfunctions.GetMasterGID("PPOP");

                        foreach (var data in values.Posummary_list)
                        {

                            //if (Request.QueryString["po_type"].ToString() == "Single")

                            msSQL = " Update pmr_tmp_tpurchaseorder " +
                                    " Set qty_ordered = '" + data.qty_ordered + "'" +
                                    " where tmppurchaseorderdtl_gid = '" + data.tmppurchaseorderdtl_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;


                            }
                            else
                            {
                                values.status = false;
                                values.message = "Some Error occurred while Updating Quantity";
                            }

                            msGetGID = objcmnfunctions.GetMasterGID("PODC");

                            if (msGetGID == "E") // Assuming "E" is a string constant
                            {
                                values.status = true;
                                values.message = "Create Sequence Code PODC for Purchase Order Details";
                            }

                            msSQL = " select product_name from pmr_mst_tproduct where product_gid ='" + data.product_gid + "' ";
                            string lsproductname = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = " select  product_code from pmr_mst_tproduct where product_gid ='" + data.product_gid + "' ";
                            string lsproduct_code = objdbconn.GetExecuteScalar(msSQL);

                            //msSQL = "select productuom_name from pmr_mst_tproductuom where productuom_gid='" + data.productuom_gid + "' ";
                            //string lsproductuom_name = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name + "' ";
                            string lstax_gid1 = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name2 + "' ";
                            string lstax_gid2 = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name3 + "' ";
                            string lstax_gid3 = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + values.tax_name4 + "' ";
                            string lstax_prefix4 = objdbconn.GetExecuteScalar(msSQL);

                            //msSQL = " SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                            //        " e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                            //        " THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                            //        " a.cost_price, a.product_gid, a.product_name FROM acp_mst_ttaxsegment2product d " +
                            //        " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                            //        " LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                            //        " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE f.vendor_gid ='" + vendor_gid + " ' and a.product_gid='" + product_gid + "' group by a.product_gid, d.tax_gid ";

                            //dt_datatable = objdbconn.GetDataTable(msSQL);
                            //if (dt_datatable.Rows.Count != 0)
                            //{
                            //    foreach (DataRow dt in dt_datatable.Rows)
                            //    {
                            //int productprice = Convert.ToInt32(data.product_price);
                            //int qty_ordered = Convert.ToInt32(data.qty_ordered);
                            //int discount_percentage = Convert.ToInt32(data.discount_percentage);
                            //int discountAmount = (productprice * qty_ordered);
                            //int discount = (discountAmount * discount_percentage)/100;
                            msSQL = " insert into pmr_trn_tpurchaseorderdtl (" +
                                    " purchaseorderdtl_gid, " +
                                    " purchaseorder_gid, " +
                                    " product_gid, " +
                                    " product_code, " +
                                    " product_name, " +
                                    " productuom_name, " +
                                    " uom_gid, " +
                                    " producttype_gid, " +
                                    " product_price, " +
                                    " discount_percentage, " +
                                    " discount_amount, " +
                                    " tax_name, " +
                                    " tax_name2, " +
                                    " tax_name3, " +
                                    " tax1_gid, " +
                                    " tax2_gid, " +
                                    " tax3_gid, " +
                                    " tax_percentage, " +
                                    " tax_percentage2, " +
                                    " tax_percentage3, " +
                                    " tax_amount, " +
                                    " tax_amount2, " +
                                    " tax_amount3, " +
                                    " qty_ordered, " +
                                    " display_field_name," +
                                    " display_field_name_old," +
                                    " product_price_L," +
                                    " discount_amount_L," +
                                    " tax_amount1_L," +
                                    " tax_amount2_L," +
                                    " needby_date," +
                                    " producttotal_amount," +
                                    " tax_amount3_L" +
                                    " )values ( " +
                                    "'" + msGetGID + "', " +
                                    "'" + msGetGID1 + "'," +
                                    "'" + data.product_gid + "', " +
                                    "'" + (String.IsNullOrEmpty(data.product_code) ? data.product_code : data.product_code.Replace("'", "\\\'")) + "'," +
                                    "'" + (String.IsNullOrEmpty(data.product_name) ? data.product_name : data.product_name.Replace("'", "\\\'")) + "'," +
                                    "'" + (String.IsNullOrEmpty(data.productuom_name) ? data.productuom_name : data.productuom_name.Replace("'", "\\\'")) + "'," +
                                    "'" + data.productuom_gid + "', " +
                                    "'" + data.producttype_gid + "', " +
                                    "'" + data.product_price + "', ";
                            if (string.IsNullOrEmpty(data.discount_percentage))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.discount_percentage + "',";
                            }
                            msSQL += "'" + data.productdiscount_amountvalue + "',";
                            msSQL += "'" + data.tax_name + "', " +
                             "'" + data.tax_name2 + "', " +
                             "'" + data.tax_name3 + "', " +
                             "'" + lstax_gid1 + "', " +
                             "'" + lstax_gid2 + "', " +
                             "'" + lstax_gid3 + "', ";
                            if (string.IsNullOrEmpty(data.tax_percentage))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.tax_percentage + "',";
                            }
                            if (string.IsNullOrEmpty(data.tax_percentage2))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.tax_percentage2 + "',";
                            }
                            if (string.IsNullOrEmpty(data.tax_percentage3))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.tax_percentage3 + "',";
                            }

                            if (string.IsNullOrEmpty(data.taxamount1))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount1 + "',";
                            }
                            if (string.IsNullOrEmpty(data.taxamount2))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount2 + "',";
                            }
                            if (string.IsNullOrEmpty(data.taxamount3))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount3 + "',";
                            }
                            msSQL += "'" + data.qty_ordered + "', ";
                            //"'" + data.display_field + "'," +
                            if (!string.IsNullOrEmpty(data.display_field) && data.display_field.Contains("'"))
                            {
                                msSQL += "'" + data.display_field.Replace("'", "") + "',";
                            }
                            else
                            {
                                msSQL += "'" + data.display_field + "', ";
                            }
                            //"'" + data.display_field_old + "'," +
                            if (!string.IsNullOrEmpty(data.display_field_old) && data.display_field_old.Contains("'"))
                            {
                                msSQL += "'" + data.display_field_old.Replace("'", "") + "',";
                            }
                            else
                            {
                                msSQL += "'" + data.display_field_old + "', ";
                            }

                            msSQL += "'" + data.product_price + "',";
                            if (string.IsNullOrEmpty(data.productdiscount_amountvalue))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.productdiscount_amountvalue + "',";
                            }
                            if (string.IsNullOrEmpty(data.taxamount1))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount1 + "',";
                            }
                            if (string.IsNullOrEmpty(data.taxamount2))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount2 + "',";
                            }

                            msSQL += "'" + data.needby_date + "'," +
                                     "'" + data.producttotal_amount + "',";
                            if (string.IsNullOrEmpty(data.taxamount3))
                            {
                                msSQL += "'0.00')";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount3 + "')";
                            }

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;


                            }
                            else
                            {
                                values.status = false;
                                values.message = "Some Error occurred while Adding into PO ";
                            }


                            msSQL = " select purchaserequisition_gid,purchaserequisitiondtl_gid," +
                                    " (qty_requested - qty_ordered - qty_poadjusted) as qty_outstanding,qty_poadjusted, " +
                                    " product_gid,uom_gid " +
                                    " from pmr_trn_tpurchaserequisitiondtl " +
                                    " where purchaserequisitiondtl_gid='" + data.purchaserequisitiondtl_gid + "'";

                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    mspGetGID = objcmnfunctions.GetMasterGID("PPPP");

                                    if (mspGetGID == "E") // Assuming "E" is a string constant
                                    {
                                        values.status = true;
                                        values.message = "Create Sequence Code PPOT for Purchase Order Details";
                                    }
                                    msSQL = " select costcenter_gid from pmr_trn_tpurchaserequisition a where a.purchaserequisition_gid ='" + dt["purchaserequisition_gid"].ToString() + "' ";
                                    string lscostcenter_gid = objdbconn.GetExecuteScalar(msSQL);

                                    msSQL = " insert into pmr_trn_tpr2po (" +
                                            " pr2po_gid, " +
                                            " purchaseorder_gid, " +
                                            " purchaseorderdtl_gid, " +
                                            " purchaserequisition_gid, " +
                                            " purchaserequisitiondtl_gid, " +
                                            " product_gid, " +
                                            " costcenter_gid, " +
                                            " qty_ordered," +
                                            " qty_poadjusted," +
                                            " display_field," +
                                            " created_by," +
                                            " created_date )" +
                                            " values ( " +
                                            "'" + mspGetGID + "', " +
                                            "'" + msGetGID1 + "'," +
                                            "'" + msGetGID + "', " +
                                            "'" + dt["purchaserequisition_gid"].ToString() + "'," +
                                            "'" + dt["purchaserequisitiondtl_gid"].ToString() + "'," +
                                            "'" + dt["product_gid"].ToString() + "'," +
                                            "'" + lscostcenter_gid + "', " +
                                             "'" + dt["qty_outstanding"].ToString() + "'," +
                                            "'" + dt["qty_poadjusted"].ToString() + "',";

                                    if (!string.IsNullOrEmpty(data.display_field) && data.display_field.Contains("'"))
                                    {
                                        msSQL += "'" + data.display_field.Replace("'", "") + "',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + data.display_field + "', ";
                                    }
                                    msSQL += "'" + user_gid + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                                    if (mnResult == 1)
                                    {
                                        values.status = true;


                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Some Error occurred while Adding into PO Details";
                                    }


                                    msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                            " Set po_status = 'Y'" +
                                           " where purchaserequisitiondtl_gid='" + data.purchaserequisitiondtl_gid + "'";


                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult == 1)
                                    {
                                        values.status = true;


                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Some Error occurred while Updating into Flag";
                                    }

                                    lsPRqty_ordered = double.Parse(dt["qty_outstanding"].ToString());
                                    lsSum_Ordered = lsPRqty_ordered;
                                    lsSum_poAdjusted = lsPRqty_poadjusted;
                                    lsPOqty_ordered = double.Parse(data.qty_ordered);

                                    msSQL = " select qty_ordered, qty_poadjusted from pmr_trn_tpurchaserequisitiondtl where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        objOdbcDataReader.Read();
                                        lsSum_Ordered = lsPOqty_ordered + double.Parse(objOdbcDataReader["qty_ordered"].ToString());

                                        objOdbcDataReader.Close();
                                    }

                                    msSQL = "select qty_requested from pmr_trn_tpurchaserequisitiondtl where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        objOdbcDataReader.Read();
                                        lsreceived = objOdbcDataReader["qty_requested"].ToString();


                                    }
                                    else
                                    {
                                        lsreceived = "0";
                                    }
                                    objOdbcDataReader.Close();

                                    //if (Request.QueryString["po_type"].ToString() == "Multiple")


                                    //{ }

                                    msSQL = " select purchaserequisitiondtl_gid from pmr_tmp_tpurchaseorder " +
                                            " where created_by='" + user_gid + "' and product_gid='" + data.product_gid + "' and uom_gid='" + data.productuom_gid + "'" +
                                            " order by qty_ordered desc";


                                    dt_datatable = objdbconn.GetDataTable(msSQL);
                                    if (dt_datatable.Rows.Count != 0)
                                    {
                                        foreach (DataRow dta in dt_datatable.Rows)
                                        {
                                            msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                                    " Set qty_ordered = qty_requested" +
                                                    " where purchaserequisitiondtl_gid =   '" + dta["purchaserequisitiondtl_gid"].ToString() + "'";


                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        }
                                    }
                                    else
                                    {

                                        msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                                " Set qty_ordered = '" + lsSum_Ordered + "'," +
                                                " qty_poadjusted = '" + lsSum_poAdjusted + "'" +
                                                " where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";


                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            values.status = true;

                                        }
                                        else
                                        {
                                            values.status = false;
                                            values.message = "Some Error occurred while Adding into PO Details";
                                        }
                                    }
                                    //else
                                    //{
                                    //    msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                    //           " Set qty_ordered = '" + lsSum_Ordered + "'," +
                                    //           " qty_poadjusted = '" + lsSum_poAdjusted + "'" +
                                    //           " where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";


                                    //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    //}

                                    lsReference_gid = dt["purchaserequisition_gid"].ToString();

                                    msSQL = "select qty_requested, qty_ordered from pmr_trn_tpurchaserequisitiondtl  where purchaserequisition_gid ='" + lsReference_gid + "' and (qty_ordered + qty_poadjusted) < qty_requested ";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lspurchaserequisition_status = "PR Work In Progress";
                                        lstPR_PO_flag = "PO Raised Partial";
                                    }
                                    else
                                    {
                                        lspurchaserequisition_status = "PR Completed";
                                        lstPR_PO_flag = "PO Raised";
                                    }
                                    objOdbcDataReader.Close();

                                    msSQL = " Update pmr_trn_tpurchaserequisition " +
                                            " Set purchaserequisition_status = '" + lspurchaserequisition_status + "'," +
                                            " purchaseorder_flag = '" + lstPR_PO_flag + "'" +
                                            " where purchaserequisition_gid = '" + lsReference_gid + "'";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                    msSQL = " select distinct a.reference_gid,b.costcenter_gid from pmr_tmp_tpurchaseorder a" +
                                            " left join pmr_trn_tpurchaserequisition b on b.purchaserequisition_gid=a.reference_gid where a.created_by = '" + user_gid + "' group by b.costcenter_gid";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lsbsccallocation = "PO BSCC Allocation Pending";

                                    }
                                    else
                                    {
                                        lsbsccallocation = "Not Applicable";
                                    }
                                    objOdbcDataReader.Close();
                                }

                            }


                        }
                        string uiDateStr = values.po_date;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string po_date = uiDate.ToString("yyyy-MM-dd");
                        msSQL = "select poapproval_flag from adm_mst_tcompany";
                        string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currency_code + "'";
                        string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);


                        string uiDateStr2 = values.expected_date;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string expected_date = uiDate2.ToString("yyyy-MM-dd");

                        msSQL = " insert into pmr_trn_tpurchaseorder (" +
                             " purchaseorder_gid, " +
                             " purchaserequisition_gid, " +
                             " purchaseorder_reference, " +
                             " purchaseorder_date," +
                             " expected_date," +
                             " branch_gid, " +
                             " created_by, " +
                             " created_date," +
                             " vendor_gid, " +
                             " vendor_address, " +
                             " shipping_address, " +
                             " requested_by, " +
                             " freight_terms, " +
                             " payment_terms, " +
                             " requested_details, " +
                             " mode_despatch, " +
                             " currency_code," +
                             " exchange_rate," +
                             " po_covernote," +
                             " purchaseorder_remarks," +
                             " total_amount, " +
                             " termsandconditions, " +
                             " purchaseorder_status, " +
                             " purchaseorder_flag, " +
                             " poref_no, " +
                             " netamount, " +
                             " addon_amount," +
                             " freightcharges," +
                             " discount_amount," +
                             " tax_gid," +
                             " tax_percentage," +
                             " tax_amount," +
                             " roundoff, " +
                             " taxsegment_gid " +
                             " ) values (" +
                              "'" + msGetGID1 + "'," +
                              "'" + lsReference_gid + "'," +
                             "'" + msGetGID1 + "'," +
                             "'" + po_date + "', " +
                             "'" + expected_date + "', " +
                             "'" + values.branch_name + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + (String.IsNullOrEmpty(values.vendor_companyname) ? values.vendor_companyname : values.vendor_companyname.Replace("'", "\\\'")) + "',";
                        if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                        {
                            msSQL += "'" + values.address1.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.address1 + "', ";
                        }
                        if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                        {
                            msSQL += "'" + values.shipping_address.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.shipping_address + "', ";
                        }

                        msSQL += "'" + values.employee_name + "'," +
                             "'" + values.delivery_terms + "'," +
                             "'" + values.payment_terms + "'," +
                             "'" + values.Requestor_details + "'," +
                             "'" + values.despatch_mode + "'," +
                             "'" + lscurrency_code + "'," +
                             "'" + values.exchange_rate + "',";
                        //"'" + values.po_covernote + "'," +
                        if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                        {
                            msSQL += "'" + values.po_covernote.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.po_covernote + "', ";
                        }
                        //"'" + values.po_covernote + "'," +
                        if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                        {
                            msSQL += "'" + values.po_covernote.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.po_covernote + "', ";
                        }

                        msSQL += "'" + values.grandtotal + "',";

                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                        {
                            msSQL += "'" + values.template_content.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.template_content + "', ";
                        }

                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "'PO Approved',";
                        }
                        else
                        {
                            msSQL += "'Operation Pending PO',";
                        }
                        msSQL += "'" + "PO Approved" + "'," +
                             "'" + values.poref_no + "'," +
                              "'" + values.totalamount + "'," +
                             "'" + values.addoncharge + "'," +
                             "'" + values.freightcharges + "',";
                        if (string.IsNullOrEmpty(values.additional_discount))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.additional_discount + "',";
                        }
                        msSQL += "'" + values.tax_name4 + "',";
                        //"'" + lstaxpercentage + "',";
                        if (string.IsNullOrEmpty(lstaxpercentage))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + lstaxpercentage + "',";
                        }

                        if (string.IsNullOrEmpty(values.tax_amount4))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.tax_amount4 + "',";
                        }
                        if (values.roundoff == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.roundoff + "',";
                        }
                        msSQL += "'" + values.taxsegment_gid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL = " select * from pmr_trn_tpurchaseorderdtl where " +
                                            " purchaseorder_gid = '" + msGetGID1 + "'and" +
                                            " producttype_gid = 'PT00010001206'";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            msSQL = " update pmr_trn_tpurchaseorder " +
                                    " set asset_flag = 'Y'" +
                                    " where purchaseorder_gid = '" + msGetGID1 + "'";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Raised PO Added Successfully";

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Some Error occured while Adding record into Purchase Order Table";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raising Po!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                   ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                   msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                   DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        // submit with file
        public void DaPostPurchaseOrderfileupload(HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {
                var branchName = httpRequest.Form["branch_name"].ToString();
                var branchGid = httpRequest.Form["branch_gid"].ToString();
                var poNo = httpRequest.Form["poref_no"].ToString();
                var poDate = httpRequest.Form["po_date"].ToString();
                var expectedDate = httpRequest.Form["expected_date"].ToString();
                var vendorGid = httpRequest.Form["vendor_gid"].ToString();
                var vendorCompanyName = httpRequest.Form["vendor_companyname"].ToString();
                var vendorDetails = httpRequest.Form["contact_telephonenumber"].ToString();  // Assuming "vendor_details" should be "contact_telephonenumber"
                var address1 = httpRequest.Form["address1"].ToString().Replace("'", "\\\'");
                var employeeName = httpRequest.Form["employee_name"].ToString();
                var deliveryTerms = httpRequest.Form["delivery_terms"].ToString().Replace("'", "\\\'");
                var paymentTerms = httpRequest.Form["payment_terms"].ToString().Replace("'", "\\\'");
                var requestorDetails = httpRequest.Form["Requestor_details"].ToString().Replace("'", "\\\'");
                var dispatchMode = httpRequest.Form["despatch_mode"].ToString();  // Assuming "dispatch_mode" should be "despatch_mode"
                var currencyGid = httpRequest.Form["currency_gid"].ToString();
                var currencyCode = httpRequest.Form["currency_code"].ToString();
                var exchangeRate = httpRequest.Form["exchange_rate"].ToString();
                var poCoverNote = httpRequest.Form["po_covernote"].ToString().Replace("'", "\\\'");
                var templateName = httpRequest.Form["template_name"].ToString().Replace("'", "\\\'");
                var templateContent = httpRequest.Form["template_content"].ToString().Replace("'", "\\\'");
                var templateGid = httpRequest.Form["template_gid"].ToString();
                var totalAmount = httpRequest.Form["totalamount"].ToString();
                var addonCharge = httpRequest.Form["addoncharge"].ToString();
                var additionalDiscount = httpRequest.Form["additional_discount"].ToString();
                var freightCharges = httpRequest.Form["freightcharges"].ToString();
                var roundOff = httpRequest.Form["roundoff"].ToString();
                var grandTotal = httpRequest.Form["grandtotal"].ToString();
                var taxGid = httpRequest.Form["tax_gid"].ToString();
                var taxAmount4 = httpRequest.Form["tax_amount4"].ToString();
                var taxName4 = httpRequest.Form["tax_name4"].ToString();
                var taxSegmentGid = httpRequest.Form["taxsegment_gid"].ToString();
                var shippingAddress = httpRequest.Form["shipping_address"].ToString().Replace("'", "\\\'");
                var posummaryListJson = httpRequest.Form["Posummary_list"].ToString();

                // Deserialize the list
                var posummaryList = JsonConvert.DeserializeObject<List<Posummary_list>>(posummaryListJson);



                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();
                string document_gid = string.Empty;
                string lscompany_code = string.Empty;
                HttpPostedFile httpPostedFile;
                string lspath;
                string final_path = "";
                string vessel_name = "";



                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ms.Close();
                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        final_path = lspath + msdocument_gid + FileExtension;
                    }
                }

                if (string.IsNullOrEmpty(poNo))
                {

                    msGetGID1 = objcmnfunctions.GetMasterGID("PPOP");

                    foreach (var data in posummaryList)
                    {

                        //if (Request.QueryString["po_type"].ToString() == "Single")

                        msSQL = " Update pmr_tmp_tpurchaseorder " +
                                " Set qty_ordered = '" + data.qty_ordered + "'" +
                                " where tmppurchaseorderdtl_gid = '" + data.tmppurchaseorderdtl_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            objResult.status = true;


                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Some Error occurred while Updating Quantity";
                        }

                        msGetGID = objcmnfunctions.GetMasterGID("PODC");

                        if (msGetGID == "E") // Assuming "E" is a string constant
                        {
                            objResult.status = true;
                            objResult.message = "Create Sequence Code PODC for Purchase Order Details";
                        }

                        msSQL = " select product_name from pmr_mst_tproduct where product_gid ='" + data.product_gid + "' ";
                        string lsproductname = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select  product_code from pmr_mst_tproduct where product_gid ='" + data.product_gid + "' ";
                        string lsproduct_code = objdbconn.GetExecuteScalar(msSQL);

                        //msSQL = "select productuom_name from pmr_mst_tproductuom where productuom_gid='" + data.productuom_gid + "' ";
                        //string lsproductuom_name = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name + "' ";
                        string lstax_gid1 = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name2 + "' ";
                        string lstax_gid2 = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name3 + "' ";
                        string lstax_gid3 = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + taxName4 + "' ";
                        string lstax_prefix4 = objdbconn.GetExecuteScalar(msSQL);

                        //msSQL = " SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                        //        " e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                        //        " THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                        //        " a.cost_price, a.product_gid, a.product_name FROM acp_mst_ttaxsegment2product d " +
                        //        " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                        //        " LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                        //        " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE f.vendor_gid ='" + vendor_gid + " ' and a.product_gid='" + product_gid + "' group by a.product_gid, d.tax_gid ";

                        //dt_datatable = objdbconn.GetDataTable(msSQL);
                        //if (dt_datatable.Rows.Count != 0)
                        //{
                        //    foreach (DataRow dt in dt_datatable.Rows)
                        //    {
                        //int productprice = Convert.ToInt32(data.product_price);
                        //int qty_ordered = Convert.ToInt32(data.qty_ordered);
                        //int discount_percentage = Convert.ToInt32(data.discount_percentage);
                        //int discountAmount = (productprice * qty_ordered);
                        //int discount = (discountAmount * discount_percentage)/100;
                        msSQL = " insert into pmr_trn_tpurchaseorderdtl (" +
                                " purchaseorderdtl_gid, " +
                                " purchaseorder_gid, " +
                                " product_gid, " +
                                " product_code, " +
                                " product_name, " +
                                " productuom_name, " +
                                " uom_gid, " +
                                " producttype_gid, " +
                                " product_price, " +
                                " discount_percentage, " +
                                " discount_amount, " +
                                " tax_name, " +
                                " tax_name2, " +
                                " tax_name3, " +
                                " tax1_gid, " +
                                " tax2_gid, " +
                                " tax3_gid, " +
                                " tax_percentage, " +
                                " tax_percentage2, " +
                                " tax_percentage3, " +
                                " tax_amount, " +
                                " tax_amount2, " +
                                " tax_amount3, " +
                                " qty_ordered, " +
                                " display_field_name," +
                                " display_field_name_old," +
                                " product_price_L," +
                                " discount_amount_L," +
                                " tax_amount1_L," +
                                " tax_amount2_L," +
                                " needby_date," +
                                " producttotal_amount," +
                                " tax_amount3_L" +
                                " )values ( " +
                                "'" + msGetGID + "', " +
                                "'" + msGetGID1 + "'," +
                                "'" + data.product_gid + "', " +
                                "'" + (String.IsNullOrEmpty(data.product_code) ? data.product_code : data.product_code.Replace("'", "\\\'")) + "'," +
                                "'" + (String.IsNullOrEmpty(data.product_name) ? data.product_name : data.product_name.Replace("'", "\\\'")) + "'," +
                                "'" + (String.IsNullOrEmpty(data.productuom_name) ? data.productuom_name : data.productuom_name.Replace("'", "\\\'")) + "'," +
                                "'" + data.productuom_gid + "', " +
                                "'" + data.producttype_gid + "', " +
                                "'" + data.product_price + "', ";
                        if (string.IsNullOrEmpty(data.discount_percentage))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.discount_percentage + "',";
                        }
                        msSQL += "'" + data.productdiscount_amountvalue + "',";
                        msSQL += "'" + data.tax_name + "', " +
                         "'" + data.tax_name2 + "', " +
                         "'" + data.tax_name3 + "', " +
                         "'" + lstax_gid1 + "', " +
                         "'" + lstax_gid2 + "', " +
                         "'" + lstax_gid3 + "', ";
                        if (string.IsNullOrEmpty(data.tax_percentage))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.tax_percentage + "',";
                        }
                        if (string.IsNullOrEmpty(data.tax_percentage2))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.tax_percentage2 + "',";
                        }
                        if (string.IsNullOrEmpty(data.tax_percentage3))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.tax_percentage3 + "',";
                        }

                        if (string.IsNullOrEmpty(data.taxamount1))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount1 + "',";
                        }
                        if (string.IsNullOrEmpty(data.taxamount2))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount2 + "',";
                        }
                        if (string.IsNullOrEmpty(data.taxamount3))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount3 + "',";
                        }
                        msSQL += "'" + data.qty_ordered + "', " +
                                "'" + (String.IsNullOrEmpty(data.display_field) ? data.display_field : data.display_field.Replace("'", "\\\'")) + "'," +
                                "'" + (String.IsNullOrEmpty(data.display_field_old) ? data.display_field_old : data.display_field_old.Replace("'", "\\\'")) + "'," +
                                "'" + data.product_price + "',";
                        if (string.IsNullOrEmpty(data.productdiscount_amountvalue))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.productdiscount_amountvalue + "',";
                        }
                        if (string.IsNullOrEmpty(data.taxamount1))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount1 + "',";
                        }
                        if (string.IsNullOrEmpty(data.taxamount2))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount2 + "',";
                        }

                        msSQL += "'" + data.needby_date + "'," +
                                 "'" + data.producttotal_amount + "',";
                        if (string.IsNullOrEmpty(data.taxamount3))
                        {
                            msSQL += "'0.00')";
                        }
                        else
                        {
                            msSQL += "'" + data.taxamount3 + "')";
                        }

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            objResult.status = true;


                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Some Error occurred while Adding into PO ";
                        }


                        msSQL = " select purchaserequisition_gid,purchaserequisitiondtl_gid," +
                                " (qty_requested - qty_ordered - qty_poadjusted) as qty_outstanding,qty_poadjusted, " +
                                " product_gid,uom_gid " +
                                " from pmr_trn_tpurchaserequisitiondtl " +
                                " where purchaserequisitiondtl_gid='" + data.purchaserequisitiondtl_gid + "'";

                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                mspGetGID = objcmnfunctions.GetMasterGID("PPPP");

                                if (mspGetGID == "E") // Assuming "E" is a string constant
                                {
                                    objResult.status = true;
                                    objResult.message = "Create Sequence Code PPOT for Purchase Order Details";
                                }
                                msSQL = " select costcenter_gid from pmr_trn_tpurchaserequisition a where a.purchaserequisition_gid ='" + dt["purchaserequisition_gid"].ToString() + "' ";
                                string lscostcenter_gid = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = " insert into pmr_trn_tpr2po (" +
                                        " pr2po_gid, " +
                                        " purchaseorder_gid, " +
                                        " purchaseorderdtl_gid, " +
                                        " purchaserequisition_gid, " +
                                        " purchaserequisitiondtl_gid, " +
                                        " product_gid, " +
                                        " costcenter_gid, " +
                                        " qty_ordered," +
                                        " qty_poadjusted," +
                                        " display_field," +
                                        " created_by," +
                                        " created_date )" +
                                        " values ( " +
                                        "'" + mspGetGID + "', " +
                                        "'" + msGetGID1 + "'," +
                                        "'" + msGetGID + "', " +
                                        "'" + dt["purchaserequisition_gid"].ToString() + "'," +
                                        "'" + dt["purchaserequisitiondtl_gid"].ToString() + "'," +
                                        "'" + dt["product_gid"].ToString() + "'," +
                                        "'" + lscostcenter_gid + "', " +
                                         "'" + dt["qty_outstanding"].ToString() + "'," +
                                        "'" + dt["qty_poadjusted"].ToString() + "'," +
                                        "'" + (String.IsNullOrEmpty(data.display_field) ? data.display_field : data.display_field.Replace("'", "\\\'")) + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                                if (mnResult == 1)
                                {
                                    objResult.status = true;


                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = "Some Error occurred while Adding into PO Details";
                                }


                                msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                        " Set po_status = 'Y'" +
                                       " where purchaserequisitiondtl_gid='" + data.purchaserequisitiondtl_gid + "'";


                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    objResult.status = true;


                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = "Some Error occurred while Updating into Flag";
                                }

                                lsPRqty_ordered = double.Parse(dt["qty_outstanding"].ToString());
                                lsSum_Ordered = lsPRqty_ordered;
                                lsSum_poAdjusted = lsPRqty_poadjusted;
                                lsPOqty_ordered = double.Parse(data.qty_ordered);

                                msSQL = " select qty_ordered, qty_poadjusted from pmr_trn_tpurchaserequisitiondtl where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    objOdbcDataReader.Read();
                                    lsSum_Ordered = lsPOqty_ordered + double.Parse(objOdbcDataReader["qty_ordered"].ToString());

                                    objOdbcDataReader.Close();
                                }

                                msSQL = "select qty_requested from pmr_trn_tpurchaserequisitiondtl where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    objOdbcDataReader.Read();
                                    lsreceived = objOdbcDataReader["qty_requested"].ToString();


                                }
                                else
                                {
                                    lsreceived = "0";
                                }
                                objOdbcDataReader.Close();

                                //if (Request.QueryString["po_type"].ToString() == "Multiple")


                                //{ }

                                msSQL = " select purchaserequisitiondtl_gid from pmr_tmp_tpurchaseorder " +
                                        " where created_by='" + user_gid + "' and product_gid='" + data.product_gid + "' and uom_gid='" + data.productuom_gid + "'" +
                                        " order by qty_ordered desc";


                                dt_datatable = objdbconn.GetDataTable(msSQL);
                                if (dt_datatable.Rows.Count != 0)
                                {
                                    foreach (DataRow dta in dt_datatable.Rows)
                                    {
                                        msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                                " Set qty_ordered = qty_requested" +
                                                " where purchaserequisitiondtl_gid =   '" + dta["purchaserequisitiondtl_gid"].ToString() + "'";


                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    }
                                }
                                else
                                {

                                    msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                            " Set qty_ordered = '" + lsSum_Ordered + "'," +
                                            " qty_poadjusted = '" + lsSum_poAdjusted + "'" +
                                            " where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";


                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        objResult.status = true;

                                    }
                                    else
                                    {
                                        objResult.status = false;
                                        objResult.message = "Some Error occurred while Adding into PO Details";
                                    }
                                }
                                //else
                                //{
                                //    msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                //           " Set qty_ordered = '" + lsSum_Ordered + "'," +
                                //           " qty_poadjusted = '" + lsSum_poAdjusted + "'" +
                                //           " where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";


                                //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                //}

                                lsReference_gid = dt["purchaserequisition_gid"].ToString();

                                msSQL = "select qty_requested, qty_ordered from pmr_trn_tpurchaserequisitiondtl  where purchaserequisition_gid ='" + lsReference_gid + "' and (qty_ordered + qty_poadjusted) < qty_requested ";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    lspurchaserequisition_status = "PR Work In Progress";
                                    lstPR_PO_flag = "PO Raised Partial";
                                }
                                else
                                {
                                    lspurchaserequisition_status = "PR Completed";
                                    lstPR_PO_flag = "PO Raised";
                                }
                                objOdbcDataReader.Close();

                                msSQL = " Update pmr_trn_tpurchaserequisition " +
                                        " Set purchaserequisition_status = '" + lspurchaserequisition_status + "'," +
                                        " purchaseorder_flag = '" + lstPR_PO_flag + "'" +
                                        " where purchaserequisition_gid = '" + lsReference_gid + "'";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                msSQL = " select distinct a.reference_gid,b.costcenter_gid from pmr_tmp_tpurchaseorder a" +
                                        " left join pmr_trn_tpurchaserequisition b on b.purchaserequisition_gid=a.reference_gid where a.created_by = '" + user_gid + "' group by b.costcenter_gid";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    lsbsccallocation = "PO BSCC Allocation Pending";

                                }
                                else
                                {
                                    lsbsccallocation = "Not Applicable";
                                }
                                objOdbcDataReader.Close();
                            }

                        }


                    }
                    string uiDateStr = poDate;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string po_date = uiDate.ToString("yyyy-MM-dd");
                    msSQL = "select poapproval_flag from adm_mst_tcompany";
                    string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + currencyCode + "'";
                    string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);


                    string uiDateStr2 = expectedDate;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string expected_date = uiDate2.ToString("yyyy-MM-dd");

                    msSQL = " insert into pmr_trn_tpurchaseorder (" +
                         " purchaseorder_gid, " +
                         " purchaserequisition_gid, " +
                         " purchaseorder_reference, " +
                         " purchaseorder_date," +
                         " expected_date," +
                         " branch_gid, " +
                         " created_by, " +
                         " created_date," +
                         " vendor_gid, " +
                         " vendor_address, " +
                         " shipping_address, " +
                         " requested_by, " +
                         " freight_terms, " +
                         " payment_terms, " +
                         " requested_details, " +
                         " mode_despatch, " +
                         " currency_code," +
                         " exchange_rate," +
                         " po_covernote," +
                         " purchaseorder_remarks," +
                         " total_amount, " +
                         " termsandconditions, " +
                         " purchaseorder_status, " +
                         " purchaseorder_flag, " +
                         " poref_no, " +
                         " netamount, " +
                         " addon_amount," +
                         " freightcharges," +
                         " discount_amount," +
                         " tax_gid," +
                         " tax_percentage," +
                         " tax_amount," +
                         " roundoff, " +
                         " file_path, " +
                         " taxsegment_gid " +
                         " ) values (" +
                          "'" + msGetGID1 + "'," +
                          "'" + lsReference_gid + "'," +
                         "'" + msGetGID1 + "'," +
                         "'" + po_date + "', " +
                         "'" + expected_date + "', " +
                         "'" + branchName + "'," +
                         "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                         "'" + (String.IsNullOrEmpty(vendorCompanyName) ? vendorCompanyName : vendorCompanyName.Replace("'", "\\\'")) + "',";
                    if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))
                    {
                        msSQL += "'" + address1.Replace("'", "") + "',";
                    }
                    else
                    {
                        msSQL += "'" + address1 + "', ";
                    }
                    if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))
                    {
                        msSQL += "'" + shippingAddress.Replace("'", "") + "',";
                    }
                    else
                    {
                        msSQL += "'" + shippingAddress + "', ";
                    }
                    msSQL += "'" + employeeName + "'," +
                             "'" + deliveryTerms + "'," +
                             "'" + paymentTerms + "'," +
                             "'" + requestorDetails + "'," +
                             "'" + dispatchMode + "'," +
                             "'" + lscurrency_code + "'," +
                             "'" + exchangeRate + "'," +
                             "'" + (String.IsNullOrEmpty(poCoverNote) ? poCoverNote : poCoverNote.Replace("'", "\\\'")) + "'," +
                             "'" + (String.IsNullOrEmpty(poCoverNote) ? poCoverNote : poCoverNote.Replace("'", "\\\'")) + "'," +
                             "'" + grandTotal + "',";
                    if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))
                    {
                        msSQL += "'" + templateContent.Replace("'", "") + "',";
                    }
                    else
                    {
                        msSQL += "'" + templateContent + "', ";
                    }
                    if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                    {
                        msSQL += "'PO Approved',";
                    }
                    else
                    {
                        msSQL += "'Operation Pending PO',";
                    }
                    msSQL += "'" + "PO Approved" + "'," +
                         "'" + poNo + "'," +
                          "'" + totalAmount + "'," +
                         "'" + addonCharge + "'," +
                         "'" + freightCharges + "',";
                    if (string.IsNullOrEmpty(additionalDiscount))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + additionalDiscount + "',";
                    }
                    msSQL += "'" + taxName4 + "',";
                    //"'" + lstaxpercentage + "',";
                    if (string.IsNullOrEmpty(lstaxpercentage))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + lstaxpercentage + "',";
                    }

                    if (string.IsNullOrEmpty(taxAmount4))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + taxAmount4 + "',";
                    }
                    if (roundOff == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + roundOff + "',";
                    }
                    msSQL += "'" + final_path + "'," +
                     "'" + taxSegmentGid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    msSQL = " select * from pmr_trn_tpurchaseorderdtl where " +
                                        " purchaseorder_gid = '" + msGetGID1 + "'and" +
                                        " producttype_gid = 'PT00010001206'";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        msSQL = " update pmr_trn_tpurchaseorder " +
                                " set asset_flag = 'Y'" +
                                " where purchaseorder_gid = '" + msGetGID1 + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }

                    if (mnResult == 1)
                    {
                        objResult.status = true;
                        objResult.message = "Raised PO Added Successfully";

                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Some Error occured while Adding record into Purchase Order Table";
                    }
                }

                else
                {
                    msSQL = "select poref_no from pmr_trn_tpurchaseorder where poref_no ='" + poNo + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        objResult.message = "PO Ref No Alredy Exist!";
                        return;
                    }
                    else
                    {
                        msGetGID1 = objcmnfunctions.GetMasterGID("PPOP");

                        foreach (var data in posummaryList)
                        {

                            //if (Request.QueryString["po_type"].ToString() == "Single")

                            msSQL = " Update pmr_tmp_tpurchaseorder " +
                                    " Set qty_ordered = '" + data.qty_ordered + "'" +
                                    " where tmppurchaseorderdtl_gid = '" + data.tmppurchaseorderdtl_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                objResult.status = true;


                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Some Error occurred while Updating Quantity";
                            }

                            msGetGID = objcmnfunctions.GetMasterGID("PODC");

                            if (msGetGID == "E") // Assuming "E" is a string constant
                            {
                                objResult.status = true;
                                objResult.message = "Create Sequence Code PODC for Purchase Order Details";
                            }

                            msSQL = " select product_name from pmr_mst_tproduct where product_gid ='" + data.product_gid + "' ";
                            string lsproductname = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = " select  product_code from pmr_mst_tproduct where product_gid ='" + data.product_gid + "' ";
                            string lsproduct_code = objdbconn.GetExecuteScalar(msSQL);

                            //msSQL = "select productuom_name from pmr_mst_tproductuom where productuom_gid='" + data.productuom_gid + "' ";
                            //string lsproductuom_name = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name + "' ";
                            string lstax_gid1 = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name2 + "' ";
                            string lstax_gid2 = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select tax_gid from acp_mst_ttax where tax_prefix='" + data.tax_name3 + "' ";
                            string lstax_gid3 = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + taxName4 + "' ";
                            string lstax_prefix4 = objdbconn.GetExecuteScalar(msSQL);

                            //msSQL = " SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                            //        " e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                            //        " THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                            //        " a.cost_price, a.product_gid, a.product_name FROM acp_mst_ttaxsegment2product d " +
                            //        " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                            //        " LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                            //        " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE f.vendor_gid ='" + vendor_gid + " ' and a.product_gid='" + product_gid + "' group by a.product_gid, d.tax_gid ";

                            //dt_datatable = objdbconn.GetDataTable(msSQL);
                            //if (dt_datatable.Rows.Count != 0)
                            //{
                            //    foreach (DataRow dt in dt_datatable.Rows)
                            //    {
                            //int productprice = Convert.ToInt32(data.product_price);
                            //int qty_ordered = Convert.ToInt32(data.qty_ordered);
                            //int discount_percentage = Convert.ToInt32(data.discount_percentage);
                            //int discountAmount = (productprice * qty_ordered);
                            //int discount = (discountAmount * discount_percentage)/100;
                            msSQL = " insert into pmr_trn_tpurchaseorderdtl (" +
                                    " purchaseorderdtl_gid, " +
                                    " purchaseorder_gid, " +
                                    " product_gid, " +
                                    " product_code, " +
                                    " product_name, " +
                                    " productuom_name, " +
                                    " uom_gid, " +
                                    " producttype_gid, " +
                                    " product_price, " +
                                    " discount_percentage, " +
                                    " discount_amount, " +
                                    " tax_name, " +
                                    " tax_name2, " +
                                    " tax_name3, " +
                                    " tax1_gid, " +
                                    " tax2_gid, " +
                                    " tax3_gid, " +
                                    " tax_percentage, " +
                                    " tax_percentage2, " +
                                    " tax_percentage3, " +
                                    " tax_amount, " +
                                    " tax_amount2, " +
                                    " tax_amount3, " +
                                    " qty_ordered, " +
                                    " display_field_name," +
                                    " display_field_name_old," +
                                    " product_price_L," +
                                    " discount_amount_L," +
                                    " tax_amount1_L," +
                                    " tax_amount2_L," +
                                    " needby_date," +
                                    " producttotal_amount," +
                                    " tax_amount3_L" +
                                    " )values ( " +
                                    "'" + msGetGID + "', " +
                                    "'" + msGetGID1 + "'," +
                                    "'" + data.product_gid + "', " +
                                    "'" + (String.IsNullOrEmpty(data.product_code) ? data.product_code : data.product_code.Replace("'", "\\\'")) + "'," +
                                    "'" + (String.IsNullOrEmpty(data.product_name) ? data.product_name : data.product_name.Replace("'", "\\\'")) + "'," +
                                    "'" + (String.IsNullOrEmpty(data.productuom_name) ? data.productuom_name : data.productuom_name.Replace("'", "\\\'")) + "'," +
                                    "'" + data.productuom_gid + "', " +
                                    "'" + data.producttype_gid + "', " +
                                    "'" + data.product_price + "', ";
                            if (string.IsNullOrEmpty(data.discount_percentage))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.discount_percentage + "',";
                            }
                            msSQL += "'" + data.productdiscount_amountvalue + "',";
                            msSQL += "'" + data.tax_name + "', " +
                             "'" + data.tax_name2 + "', " +
                             "'" + data.tax_name3 + "', " +
                             "'" + lstax_gid1 + "', " +
                             "'" + lstax_gid2 + "', " +
                             "'" + lstax_gid3 + "', ";
                            if (string.IsNullOrEmpty(data.tax_percentage))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.tax_percentage + "',";
                            }
                            if (string.IsNullOrEmpty(data.tax_percentage2))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.tax_percentage2 + "',";
                            }
                            if (string.IsNullOrEmpty(data.tax_percentage3))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.tax_percentage3 + "',";
                            }

                            if (string.IsNullOrEmpty(data.taxamount1))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount1 + "',";
                            }
                            if (string.IsNullOrEmpty(data.taxamount2))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount2 + "',";
                            }
                            if (string.IsNullOrEmpty(data.taxamount3))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount3 + "',";
                            }
                            msSQL += "'" + data.qty_ordered + "', ";
                            //"'" + data.display_field + "'," +
                            if (!string.IsNullOrEmpty(data.display_field) && data.display_field.Contains("'"))
                            {
                                msSQL += "'" + data.display_field.Replace("'", "") + "',";
                            }
                            else
                            {
                                msSQL += "'" + data.display_field + "', ";
                            }
                            //"'" + data.display_field_old + "'," +
                            if (!string.IsNullOrEmpty(data.display_field_old) && data.display_field_old.Contains("'"))
                            {
                                msSQL += "'" + data.display_field_old.Replace("'", "") + "',";
                            }
                            else
                            {
                                msSQL += "'" + data.display_field_old + "', ";
                            }

                            msSQL += "'" + data.product_price + "',";
                            if (string.IsNullOrEmpty(data.productdiscount_amountvalue))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.productdiscount_amountvalue + "',";
                            }
                            if (string.IsNullOrEmpty(data.taxamount1))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount1 + "',";
                            }
                            if (string.IsNullOrEmpty(data.taxamount2))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount2 + "',";
                            }

                            msSQL += "'" + data.needby_date + "'," +
                                     "'" + data.producttotal_amount + "',";
                            if (string.IsNullOrEmpty(data.taxamount3))
                            {
                                msSQL += "'0.00')";
                            }
                            else
                            {
                                msSQL += "'" + data.taxamount3 + "')";
                            }

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                objResult.status = true;


                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Some Error occurred while Adding into PO ";
                            }


                            msSQL = " select purchaserequisition_gid,purchaserequisitiondtl_gid," +
                                    " (qty_requested - qty_ordered - qty_poadjusted) as qty_outstanding,qty_poadjusted, " +
                                    " product_gid,uom_gid " +
                                    " from pmr_trn_tpurchaserequisitiondtl " +
                                    " where purchaserequisitiondtl_gid='" + data.purchaserequisitiondtl_gid + "'";

                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    mspGetGID = objcmnfunctions.GetMasterGID("PPPP");

                                    if (mspGetGID == "E") // Assuming "E" is a string constant
                                    {
                                        objResult.status = true;
                                        objResult.message = "Create Sequence Code PPOT for Purchase Order Details";
                                    }
                                    msSQL = " select costcenter_gid from pmr_trn_tpurchaserequisition a where a.purchaserequisition_gid ='" + dt["purchaserequisition_gid"].ToString() + "' ";
                                    string lscostcenter_gid = objdbconn.GetExecuteScalar(msSQL);

                                    msSQL = " insert into pmr_trn_tpr2po (" +
                                            " pr2po_gid, " +
                                            " purchaseorder_gid, " +
                                            " purchaseorderdtl_gid, " +
                                            " purchaserequisition_gid, " +
                                            " purchaserequisitiondtl_gid, " +
                                            " product_gid, " +
                                            " costcenter_gid, " +
                                            " qty_ordered," +
                                            " qty_poadjusted," +
                                            " display_field," +
                                            " created_by," +
                                            " created_date )" +
                                            " values ( " +
                                            "'" + mspGetGID + "', " +
                                            "'" + msGetGID1 + "'," +
                                            "'" + msGetGID + "', " +
                                            "'" + dt["purchaserequisition_gid"].ToString() + "'," +
                                            "'" + dt["purchaserequisitiondtl_gid"].ToString() + "'," +
                                            "'" + dt["product_gid"].ToString() + "'," +
                                            "'" + lscostcenter_gid + "', " +
                                             "'" + dt["qty_outstanding"].ToString() + "'," +
                                            "'" + dt["qty_poadjusted"].ToString() + "',";

                                    if (!string.IsNullOrEmpty(data.display_field) && data.display_field.Contains("'"))
                                    {
                                        msSQL += "'" + data.display_field.Replace("'", "") + "',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + data.display_field + "', ";
                                    }
                                    msSQL += "'" + user_gid + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                                    if (mnResult == 1)
                                    {
                                        objResult.status = true;


                                    }
                                    else
                                    {
                                        objResult.status = false;
                                        objResult.message = "Some Error occurred while Adding into PO Details";
                                    }


                                    msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                            " Set po_status = 'Y'" +
                                           " where purchaserequisitiondtl_gid='" + data.purchaserequisitiondtl_gid + "'";


                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult == 1)
                                    {
                                        objResult.status = true;


                                    }
                                    else
                                    {
                                        objResult.status = false;
                                        objResult.message = "Some Error occurred while Updating into Flag";
                                    }

                                    lsPRqty_ordered = double.Parse(dt["qty_outstanding"].ToString());
                                    lsSum_Ordered = lsPRqty_ordered;
                                    lsSum_poAdjusted = lsPRqty_poadjusted;
                                    lsPOqty_ordered = double.Parse(data.qty_ordered);

                                    msSQL = " select qty_ordered, qty_poadjusted from pmr_trn_tpurchaserequisitiondtl where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        objOdbcDataReader.Read();
                                        lsSum_Ordered = lsPOqty_ordered + double.Parse(objOdbcDataReader["qty_ordered"].ToString());

                                        objOdbcDataReader.Close();
                                    }

                                    msSQL = "select qty_requested from pmr_trn_tpurchaserequisitiondtl where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        objOdbcDataReader.Read();
                                        lsreceived = objOdbcDataReader["qty_requested"].ToString();


                                    }
                                    else
                                    {
                                        lsreceived = "0";
                                    }
                                    objOdbcDataReader.Close();

                                    //if (Request.QueryString["po_type"].ToString() == "Multiple")


                                    //{ }

                                    msSQL = " select purchaserequisitiondtl_gid from pmr_tmp_tpurchaseorder " +
                                            " where created_by='" + user_gid + "' and product_gid='" + data.product_gid + "' and uom_gid='" + data.productuom_gid + "'" +
                                            " order by qty_ordered desc";


                                    dt_datatable = objdbconn.GetDataTable(msSQL);
                                    if (dt_datatable.Rows.Count != 0)
                                    {
                                        foreach (DataRow dta in dt_datatable.Rows)
                                        {
                                            msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                                    " Set qty_ordered = qty_requested" +
                                                    " where purchaserequisitiondtl_gid =   '" + dta["purchaserequisitiondtl_gid"].ToString() + "'";


                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        }
                                    }
                                    else
                                    {

                                        msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                                " Set qty_ordered = '" + lsSum_Ordered + "'," +
                                                " qty_poadjusted = '" + lsSum_poAdjusted + "'" +
                                                " where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";


                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            objResult.status = true;

                                        }
                                        else
                                        {
                                            objResult.status = false;
                                            objResult.message = "Some Error occurred while Adding into PO Details";
                                        }
                                    }
                                    //else
                                    //{
                                    //    msSQL = " Update pmr_trn_tpurchaserequisitiondtl " +
                                    //           " Set qty_ordered = '" + lsSum_Ordered + "'," +
                                    //           " qty_poadjusted = '" + lsSum_poAdjusted + "'" +
                                    //           " where purchaserequisitiondtl_gid = '" + data.purchaserequisitiondtl_gid + "'";


                                    //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    //}

                                    lsReference_gid = dt["purchaserequisition_gid"].ToString();

                                    msSQL = "select qty_requested, qty_ordered from pmr_trn_tpurchaserequisitiondtl  where purchaserequisition_gid ='" + lsReference_gid + "' and (qty_ordered + qty_poadjusted) < qty_requested ";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lspurchaserequisition_status = "PR Work In Progress";
                                        lstPR_PO_flag = "PO Raised Partial";
                                    }
                                    else
                                    {
                                        lspurchaserequisition_status = "PR Completed";
                                        lstPR_PO_flag = "PO Raised";
                                    }
                                    objOdbcDataReader.Close();

                                    msSQL = " Update pmr_trn_tpurchaserequisition " +
                                            " Set purchaserequisition_status = '" + lspurchaserequisition_status + "'," +
                                            " purchaseorder_flag = '" + lstPR_PO_flag + "'" +
                                            " where purchaserequisition_gid = '" + lsReference_gid + "'";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                    msSQL = " select distinct a.reference_gid,b.costcenter_gid from pmr_tmp_tpurchaseorder a" +
                                            " left join pmr_trn_tpurchaserequisition b on b.purchaserequisition_gid=a.reference_gid where a.created_by = '" + user_gid + "' group by b.costcenter_gid";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lsbsccallocation = "PO BSCC Allocation Pending";

                                    }
                                    else
                                    {
                                        lsbsccallocation = "Not Applicable";
                                    }
                                    objOdbcDataReader.Close();
                                }

                            }


                        }
                        string uiDateStr = poDate;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string po_date = uiDate.ToString("yyyy-MM-dd");
                        msSQL = "select poapproval_flag from adm_mst_tcompany";
                        string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + currencyCode + "'";
                        string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);


                        string uiDateStr2 = expectedDate;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string expected_date = uiDate2.ToString("yyyy-MM-dd");

                        msSQL = " insert into pmr_trn_tpurchaseorder (" +
                             " purchaseorder_gid, " +
                             " purchaserequisition_gid, " +
                             " purchaseorder_reference, " +
                             " purchaseorder_date," +
                             " expected_date," +
                             " branch_gid, " +
                             " created_by, " +
                             " created_date," +
                             " vendor_gid, " +
                             " vendor_address, " +
                             " shipping_address, " +
                             " requested_by, " +
                             " freight_terms, " +
                             " payment_terms, " +
                             " requested_details, " +
                             " mode_despatch, " +
                             " currency_code," +
                             " exchange_rate," +
                             " po_covernote," +
                             " purchaseorder_remarks," +
                             " total_amount, " +
                             " termsandconditions, " +
                             " purchaseorder_status, " +
                             " purchaseorder_flag, " +
                             " poref_no, " +
                             " netamount, " +
                             " addon_amount," +
                             " freightcharges," +
                             " discount_amount," +
                             " tax_gid," +
                             " tax_percentage," +
                             " tax_amount," +
                             " roundoff, " +
                             " file_path, " +
                             " taxsegment_gid " +
                             " ) values (" +
                              "'" + msGetGID1 + "'," +
                              "'" + lsReference_gid + "'," +
                             "'" + msGetGID1 + "'," +
                             "'" + po_date + "', " +
                             "'" + expected_date + "', " +
                             "'" + branchName + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + (String.IsNullOrEmpty(vendorCompanyName) ? vendorCompanyName : vendorCompanyName.Replace("'", "\\\'")) + "',";
                        if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))
                        {
                            msSQL += "'" + address1.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + address1 + "', ";
                        }
                        if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))
                        {
                            msSQL += "'" + shippingAddress.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + shippingAddress + "', ";
                        }

                        msSQL += "'" + employeeName + "'," +
                             "'" + deliveryTerms + "'," +
                             "'" + paymentTerms + "'," +
                             "'" + requestorDetails + "'," +
                             "'" + dispatchMode + "'," +
                             "'" + lscurrency_code + "'," +
                             "'" + exchangeRate + "',";
                        //"'" + values.po_covernote + "'," +
                        if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                        {
                            msSQL += "'" + poCoverNote.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + poCoverNote + "', ";
                        }
                        //"'" + values.po_covernote + "'," +
                        if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                        {
                            msSQL += "'" + poCoverNote.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + poCoverNote + "', ";
                        }

                        msSQL += "'" + grandTotal + "',";

                        if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))
                        {
                            msSQL += "'" + templateContent.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + templateContent + "', ";
                        }

                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "'PO Approved',";
                        }
                        else
                        {
                            msSQL += "'Operation Pending PO',";
                        }
                        msSQL += "'" + "PO Approved" + "'," +
                             "'" + poNo + "'," +
                              "'" + totalAmount + "'," +
                             "'" + addonCharge + "'," +
                             "'" + freightCharges + "',";
                        if (string.IsNullOrEmpty(additionalDiscount))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + additionalDiscount + "',";
                        }
                        msSQL += "'" + taxName4 + "',";
                        //"'" + lstaxpercentage + "',";
                        if (string.IsNullOrEmpty(lstaxpercentage))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + lstaxpercentage + "',";
                        }

                        if (string.IsNullOrEmpty(taxAmount4))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + taxAmount4 + "',";
                        }
                        if (roundOff == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + roundOff + "',";
                        }
                        msSQL += "'" + final_path + "'," +
                        "'" + taxSegmentGid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL = " select * from pmr_trn_tpurchaseorderdtl where " +
                                            " purchaseorder_gid = '" + msGetGID1 + "'and" +
                                            " producttype_gid = 'PT00010001206'";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            msSQL = " update pmr_trn_tpurchaseorder " +
                                    " set asset_flag = 'Y'" +
                                    " where purchaseorder_gid = '" + msGetGID1 + "'";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                        if (mnResult == 1)
                        {
                            objResult.status = true;
                            objResult.message = "Raised PO Added Successfully";

                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Some Error occured while Adding record into Purchase Order Table";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Submitting Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetSO2POproducttax(string user_gid, string salesorder_gid, string vendor_gid, MdlSO2PO values)
        {
            try
            {

                msSQL = "delete from pmr_tmp_tpurchaseorder where created_by ='" + user_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select a.salesorderdtl_gid,a.salesorder_gid,a.product_gid,b.product_name,b.cost_price," +
                        " b.product_desc,d.productuom_gid,d.productuom_name, a.productgroup_gid,a.productgroup_name," +
                        " CASE  WHEN b.customerproduct_code IS NULL OR b.customerproduct_code = ''  THEN b.product_code ELSE " +
                        " b.customerproduct_code  END AS product_code from smr_trn_tsalesorderdtl a left join " +
                        " pmr_mst_tproduct b ON a.product_gid = b.product_gid  LEFT JOIN pmr_mst_tproductgroup c " +
                        " ON c.productgroup_gid = a.productgroup_gid  LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid  " +
                        " where salesorder_gid = '" + salesorder_gid + "' group by salesorderdtl_gid ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList1 = new List<GetPurchaseOrder1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dta in dt_datatable.Rows)
                    {
                        var msSQL = "  SELECT f.taxsegment_gid, d.taxsegment_gid, a.product_gid, a.product_name, concat(b.tax_prefix) as tax_prefix," +
                                     "  e.taxsegment_name, d.tax_name, d.tax_gid, d.tax_percentage, d.tax_amount, a.mrp_price, a.cost_price FROM acp_mst_ttaxsegment2product d" +
                                     "  left join acp_mst_ttax b on b.tax_gid = d.tax_gid LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid" +
                                     "   LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid" +
                                     "  LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE a.product_gid = '" + dta["product_gid"].ToString() + "'" +
                                     "  and f.vendor_gid = '" + vendor_gid + "'";
                        var dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetProductseg>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            var msGetGID = objcmnfunctions.GetMasterGID("PPOT");

                            if (msGetGID == "E")
                            {
                                values.status = true;
                                values.message = "Create Sequence Code PPOT for Purchase Order Details";
                            }
                            for (int i = 0; i < dt_datatable.Rows.Count; i++)
                            {
                                DataRow dt = dt_datatable.Rows[i];



                                if (i == 0)
                                {
                                    msSQL = " insert into pmr_tmp_tpurchaseorder (" +
                                            " tmppurchaseorderdtl_gid, " +
                                            " tmppurchaseorder_gid, " +
                                            " product_gid, " +
                                            " product_code, " +
                                            " product_name, " +
                                            " productuom_name, " +
                                            " uom_gid, " +
                                            " qty_ordered, " +
                                            " product_price, " +
                                            " tax_name, " +
                                            " tax_percentage, " +
                                            " tax1_gid, " +
                                            " reference_gid, " +
                                            " productgroup_name, " +
                                            " created_by, " +
                                            " display_field, " +
                                            " purchaserequisitiondtl_gid)" +
                                            " values ( " +
                                            "'" + msGetGID + "'," +
                                            "'" + salesorder_gid + "'," +
                                            "'" + dta["product_gid"].ToString() + "'," +
                                            "'" + dta["product_code"].ToString() + "'," +
                                            "'" + dta["product_name"].ToString() + "'," +
                                            "'" + dta["productuom_name"].ToString() + "'," +
                                            "'" + dta["productuom_gid"].ToString() + "'," +
                                            "'0', " +
                                            "'" + dt["cost_price"].ToString() + "', " +
                                            "'" + dt["tax_prefix"].ToString() + "', " +
                                            "'" + dt["tax_percentage"].ToString() + "', " +
                                            "'" + dt["tax_gid"].ToString() + "', " +
                                            "'" + dta["salesorder_gid"].ToString() + "', " +
                                            "'" + dta["productgroup_name"].ToString() + "', " +
                                            "'" + user_gid + "', " +
                                            "'" + dta["product_desc"].ToString() + "', " +
                                            "'" + dta["salesorderdtl_gid"].ToString() + "') ";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                                else if (i == 1)
                                {
                                    msSQL = " Update pmr_tmp_tpurchaseorder " +
                                            " Set tax2_gid = '" + dt["tax_gid"].ToString() + "'," +
                                            " tax_percentage2 = '" + dt["tax_percentage"].ToString() + "'," +
                                            " tax_name2 = '" + dt["tax_prefix"].ToString() + "'" +
                                            " where tmppurchaseorderdtl_gid = '" + msGetGID + "'";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                    }


                }

                msSQL = " select tmppurchaseorderdtl_gid,tmppurchaseorder_gid,product_gid,qty_ordered,tax_percentage,tax_name,product_price," +
                        " tax1_gid,tax_name2,tax_percentage2,tax2_gid,uom_gid,display_field,product_code,product_name,productuom_name,productgroup_name,product_price,discount_percentage,purchaserequisitiondtl_gid " +
                        " from pmr_tmp_tpurchaseorder where created_by ='" + user_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleListi = new List<GetPurchaseOrder1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleListi.Add(new GetPurchaseOrder1
                        {
                            tmppurchaseorderdtl_gid = dt["tmppurchaseorderdtl_gid"].ToString(),
                            tmppurchaseorder_gid = dt["tmppurchaseorder_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            tax1_gid = dt["tax1_gid"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax2_gid = dt["tax2_gid"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            //purchaserequisitiondtl_gid = dt["salesorderdtl_gid"].ToString(),

                        });
                        values.GetPurchaseOrder1 = getModuleListi;
                    }
                }
                dt_datatable.Dispose();



            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }




    }


}
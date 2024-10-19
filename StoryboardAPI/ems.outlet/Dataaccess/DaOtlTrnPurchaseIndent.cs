
using ems.outlet.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Services.Description;


namespace ems.outlet.Dataaccess
{
    public class DaOtlTrnPurchaseIndent
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string hierarchy_flag;
        Image branch_logo;
        string company_logo_path, authorized_sign_path;
        Image company_logo;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, msGetGID, msGetGid, lsCode, msPOGetGID, msINGetGID, msGetGID1, msPO1GetGID;
        int mnResult, i;
        DataTable mail_datatable, dt_datatable;
        OdbcDataReader objOdbcDataReader;


        public void DaGetOtlTrnPurchaseIndent(string user_gid ,MdlOtlTrnPurchaseIndent values)
        {
            try
            {

                msSQL = " select distinct a.purchaserequisition_gid,DATE_FORMAT(a.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date, " +
                " a.materialrequisition_gid," +
                " a.purchaserequisition_flag,replace(a.purchaserequisition_remarks,'PI','PR') as purchaserequisition_remarks, " +
                " CASE when a.grn_flag <> 'GRN Pending' then a.grn_flag " +
                " when a.purchaseorder_flag <> 'PO Pending' then a.purchaseorder_flag " +
                " when a.purchaserequisition_flag='PR Pending Approval' then 'PR Pending Approval' " +
                " when a.purchaserequisition_flag='PR Approved' then 'PR Approved' " +
                " when a.purchaserequisition_flag='PR Rejected' then 'PR Rejected' " +
                " else a.purchaserequisition_flag end as 'overall_status', " +
                " b.user_firstname,d.department_name,CONCAT_WS('/', e.branch_name, d.department_name) AS branch_name,replace(a.purchaserequisition_referencenumber,'PI','PR') as purchaserequisition_referencenumber,g.costcenter_name " +
                " from pmr_trn_tpurchaserequisition a " +
                " left join adm_mst_tuser b on a.created_by = b.user_gid " +
                " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                " left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid " +
                " left join adm_mst_tmodule2employee  f on f.employee_gid = c.employee_gid " +
                " left join pmr_mst_tcostcenter g on a.costcenter_gid=g.costcenter_gid " +
                " where 0=0 and a.created_by = '" + user_gid +"'  Order by a.purchaserequisition_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Summary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Summary_list
                        {
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            purchaserequisition_referencenumber = dt["purchaserequisition_referencenumber"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            purchaserequisition_remarks = dt["purchaserequisition_remarks"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            branch_name = dt["branch_name"].ToString()



                        });
                        values.Summary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting in PR!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProductdetails(string purchaserequisition_gid, MdlOtlTrnPurchaseIndent values)
        {
            try
            {

                msSQL = "select product_code,product_name,qty_requested,pr_originalqty from pmr_trn_tpurchaserequisitiondtl where purchaserequisition_gid='" + purchaserequisition_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productlistdetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productlistdetail
                        {

                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            pr_originalqty = dt["pr_originalqty"].ToString(),
                        });
                        values.productlistdetail = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product details in PR!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostOnAdd(string user_gid, productlist values)
        {
            try
            {

                msGetGid = objcmnfunctions.GetMasterGID("PODC");
                msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                string lsproductName = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                {
                    msSQL = " insert into pmr_tmp_tpurchaserequisition ( " +
                            " product_gid," +
                            " product_code," +
                            " product_name," +
                            " productgroup_name," +
                            " productuom_name," +
                            " uom_gid," +
                            " created_by ," +
                            " display_field ," +
                            " qty_requested" +
                            " ) values ( " +
                            "'" + values.product_gid + "'," +
                            "'" + values.product_code + "'," +
                            "'" + lsproductName + "'," +
                            "'" + values.productgroup_name + "'," +
                            "'" + values.productuom_name + "'," +
                            "'" + lsproductuomgid + "'," +
                            "'" + user_gid + "'," +
                            "'" + values.display_field + "'," +
                            "'" + values.qty_requested + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaProductSummary(string user_gid, MdlOtlTrnPurchaseIndent values)
        {
            try
            {
                msSQL = " Select distinct a.tmppurchaserequisition_gid, c.productgroup_name,a.display_field," +
                        " b.product_code, b.product_gid, b.product_name, " +
                        " format(a.qty_requested,2) as qty_requested, d.productuom_name " +
                        " from pmr_tmp_tpurchaserequisition a " +
                        " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                        " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on d.productuom_gid = a.uom_gid" +
                        " where a.created_by='" + user_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new productsummary_list
                        {
                            tmppurchaserequisition_gid = dt["tmppurchaserequisition_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            display_field = dt["display_field"].ToString(),
                        });
                        values.productsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaPostPurchaseRequisition(string employee_gid, string user_gid, PostAllPI values)
        {
            try
            {

                string uiDateStr = values.purchaserequisition_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string purchaserequisition_date = uiDate.ToString("yyyy-MM-dd");

                msINGetGID = objcmnfunctions.GetMasterGID("PPRP");

                msSQL = "select count(*) as mnCtr from pmr_tmp_tpurchaserequisition where created_by = '" + user_gid + "'";
                string mnCtr = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " Insert into pmr_trn_tpurchaserequisition " +
                        " (purchaserequisition_gid, " +
                        " branch_gid, " +
                        " purchaserequisition_date, " +
                        " purchaserequisition_remarks, " +
                        " purchaserequisition_referencenumber, " +
                        " created_by, " +
                        " created_date, " +
                        " purchaserequisition_flag, " +
                        " purchaserequisition_status, " +
                        " product_count, " +
                        " enquiry_raised, " +
                        " type, " +
                        " purchaseorder_raised, " +
                        " priority, " +
                        " costcenter_gid)" +
                        " values (" +
                        "'" + msINGetGID + "', " +
                        "'" + values.branch_gid + "'," +
                        "'" + purchaserequisition_date + "', " +
                        "'" + values.purchaserequisition_remarks + "', " +
                        "'" + values.purchaserequisition_referencenumber + "', " +
                        "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'PR Approved', " +
                        "'PR Approved', " +
                        "'" + mnCtr + "', " +
                        "'N', " +
                        "'Opex', " +
                        "'N', " +
                        "'" + values.priority_remarks + "', " +
                        "'" + values.costcenter_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 1)
                {
                    msSQL = " Select distinct a.tmppurchaserequisition_gid, c.productgroup_name,c.productgroup_gid, " +
                            " b.product_code, b.product_gid, b.product_name,a.display_field,b.product_price, " +
                            " format(a.qty_requested,2) as qty_requested, d.productuom_name ,d.productuom_gid" +
                            " from pmr_tmp_tpurchaserequisition a " +
                            " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                            " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                            " left join pmr_mst_tproductuom d on d.productuom_gid = a.uom_gid" +
                            " where a.created_by='" + user_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PPDC");

                        msSQL = " Insert into pmr_trn_tpurchaserequisitiondtl " +
                           " (purchaserequisitiondtl_gid," +
                           " purchaserequisition_gid , " +
                           " product_gid," +
                           " product_code," +
                           " product_name," +
                           " uom_gid," +
                           " productuom_name," +
                           " productgroup_gid," +
                           " productgroup_name," +
                           " qty_requested, " +
                           " seq_no, " +
                           " user_gid, " +
                           " display_field, " +
                           " requested_by, " +
                           " pr_originalqty," +
                           " pr_newproductstatus," +
                           " product_price " +
                           " )values (" +
                           "'" + msGetGid + "'," +
                           "'" + msINGetGID + "'," +
                            "'" + dt["product_gid"].ToString() + "'," +
                            "'" + dt["product_code"].ToString() + "'," +
                            "'" + dt["product_name"].ToString() + "'," +
                            "'" + dt["productuom_gid"].ToString() + "'," +
                            "'" + dt["productuom_name"].ToString() + "'," +
                            "'" + dt["productgroup_gid"].ToString() + "'," +
                            "'" + dt["productgroup_name"].ToString() + "'," +
                            "'" + dt["qty_requested"].ToString() + "'," +
                            "'" + i + "'," +
                            "'" + user_gid + "'," +
                            "'" + dt["display_field"].ToString() + "'," +
                            "'" + values.employee_name + "'," +
                            "'" + dt["qty_requested"].ToString() + "'," +
                            "   'Y' ," +
                            "'" + dt["product_price"].ToString() + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Purchase Requisition Submited Successfully";
                        msSQL = " delete from pmr_tmp_tpurchaserequisition where created_by='" + user_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Purchase Requisition";
                    }

                    dt_datatable.Dispose();

                    msGetGID1 = objcmnfunctions.GetMasterGID("PODC");

                    msSQL = " insert into pmr_trn_tapproval ( " +
                        " approval_gid, " +
                        " approved_by, " +
                        " approved_date, " +
                        " submodule_gid, " +
                        " pr_gid " +
                        " ) values ( " +
                        "'" + msGetGID1 + "'," +
                        " '" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'PMRPROAPR'," +
                        "'" + msINGetGID + "') ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update pmr_trn_tapproval set " +
                       " approval_flag = 'Y', " +
                       " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                       " where approved_by = '" + user_gid + "'" +
                       " and pr_gid = '" + msINGetGID + "' and submodule_gid='PMRPROAPR'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding in purchase requisition!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetPurchaseRequisitionView(string purchaserequisition_gid, MdlOtlTrnPurchaseIndent values)
        {
            try
            {

                msSQL = " select a.purchaserequisition_gid, a.type,n.user_firstname as requested_by, " +
                        " date_format(a.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date, " +
                        " CASE when a.grn_flag <> 'GRN Pending' then a.grn_flag " +
                        " when a.purchaseorder_flag <> 'PO Pending' then a.purchaseorder_flag " +
                        " else a.purchaserequisition_flag end as 'overall_status', " +
                        " a.purchaserequisition_remarks,a.purchaserequisition_referencenumber,CASE WHEN a.priority = 'N' THEN 'Low' ELSE 'High' END AS priority, a.approver_remarks, a.purchaserequisition_flag, " +
                        " concat_ws(' - ',b.user_firstname,b.user_lastname) as user_firstname, e.user_firstname as approved_by, f.branch_name,d.department_name, " +
                        " a.costcenter_gid,format(g.budget_available, 2) as budget_available," +
                        " format((g.budget_allocated - g.amount_used - g.provisional_amount),2) as available_amount," +
                        " concat_ws('-',g.costcenter_code,g.costcenter_name) as costcenter_name,g.costcenter_gid, l.user_firstname as approvername,g.costcenter_name, " +
                        " date_format(a.approved_date,'%d-%m-%Y') as approved_date,format(a.provisional_amount,2)as provisional_amount  " +
                        " from pmr_trn_tpurchaserequisition a " +
                        " left join adm_mst_tuser b on a.created_by = b.user_gid " +
                        " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                        " left join adm_mst_tuser e on e.user_gid = a.approved_by " +
                        " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid " +
                        " left join pmr_mst_tcostcenter g on g.costcenter_gid = a.costcenter_gid " +
                        " left join adm_mst_tuser l on l.user_gid = a.approved_by " +
                        " left join pmr_trn_tpurchaserequisitiondtl m on m.purchaserequisition_gid = a.purchaserequisition_gid " +
                        " left join adm_mst_tuser n on m.requested_by = n.user_gid " +
                        " where a.purchaserequisition_gid = '" + purchaserequisition_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<purchaserequestitionview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new purchaserequestitionview
                        {

                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            purchaserequisition_remarks = dt["purchaserequisition_remarks"].ToString(),
                            purchaserequisition_referencenumber = dt["purchaserequisition_referencenumber"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            requested_by = dt["requested_by"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            priority_remarks = dt["priority"].ToString(),
                            //product_name = dt["product_name"].ToString(),
                            //product_code = dt["product_code"].ToString(),
                            //qty_requested = dt["qty_requested"].ToString(),
                            //productgroup_name = dt["productgroup_name"].ToString(),
                            //productuom_name = dt["productuom_name"].ToString(),
                            //display_field = dt["display_field"].ToString(),

                        });
                        values.purchaserequestitionview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PR view!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetPurchaseRequisitionproduct(string purchaserequisition_gid, MdlOtlTrnPurchaseIndent values)
        {
            try
            {

                msSQL = "select a.product_remarks, a. qty_requested ,  " +
                       " format(a.qty_ordered, 2) as qty_ordered, format(a.qty_received, 2) as qty_received," +
                       "a.product_name, a.product_code, a.productgroup_name, a.productuom_name,  " +
                       " date_format(e.purchaserequisition_date, '%d-%m-%Y') as purchaserequisition_date,a.display_field,a.pr_originalqty " +
                       "  from pmr_trn_tpurchaserequisitiondtl a " +
                      "   left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                     "    left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                      "   left join pmr_mst_tproductuom d on d.productuom_gid = a.uom_gid " +
                      "   left join pmr_trn_tpurchaserequisition e on e.purchaserequisition_gid = a.purchaserequisition_gid " +
                      "   where a.purchaserequisition_gid= '" + purchaserequisition_gid + "'and " +
                       "  qty_requested != 0.00 order by a.seq_no";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<purchaserequestitionview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new purchaserequestitionview
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),

                        });
                        values.purchaserequestitionview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PR view!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


    }
}
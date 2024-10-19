using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.outlet.Models;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace ems.outlet.Dataaccess
{
    public class DaOtlMstBranch
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid, lsproductname, lsproductgid, final_path;
        int mnResult;


        // Outlet Summary
        public void Daoutletsummary(MdlOtlMstBranch values)
        {
            try
            {

                msSQL = "SELECT a.branch_gid,a.branch_name,a.branch_gid,a.contact_number, " +
                    " COUNT(DISTINCT b.product_gid) AS assignedproduct, " +
                    " COUNT(DISTINCT d.branch2pincode_gid) AS assignedpincode " +
                    " FROM hrm_mst_tbranch a " +    
                    " LEFT JOIN otl_trn_branch2product b ON a.branch_gid = b.branch_gid " +
                    " LEFT JOIN otl_trn_branch2pincode d ON a.branch_gid = d.branch_gid " +
                    " LEFT JOIN pmr_mst_tproduct c ON c.product_gid = b.product_gid " +
                    " GROUP BY a.branch_gid,a.branch_name " +
                    "  ORDER BY a.branch_gid ASC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<outlet_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new outlet_list
                        {                            
                            campaign_title = dt["branch_name"].ToString(),                            
                            branch_gid = dt["branch_gid"].ToString(),                            
                            Assigned_product = dt["assignedproduct"].ToString(), 
                            Assigned_pincode = dt["assignedpincode"].ToString(),
                            mobile_number = dt["contact_number"].ToString(),
                        });
                        values.outlet_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        // Summary
        public void DaGetProductSummary(string branch_gid, MdlOtlMstBranch values)
        {
            try
            {

                msSQL = " SELECT a.product_image,a.product_gid,a.customerproduct_code,a.product_desc,a.product_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name," +
                        " CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date,  " +
                        " (case when a.stockable = 'Y' then 'Yes' else 'No ' end) as stockable,(case when a.status = '1' then 'Active' else 'Inactive' end) as Status, " +
                        " (case when a.serial_flag = 'Y' then 'Yes' else 'No' end)as serial_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time, '  ', 'days') end)as lead_time " +
                        " from pmr_mst_tproduct a" +
                        " left join adm_mst_tuser f on f.user_gid = a.created_by order by a.product_gid desc "                       ;

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Prod_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Prod_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),
                            Status = dt["Status"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            lead_time = dt["lead_time"].ToString(),


                        });
                        values.Prod_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // Assign Product Summary
        public void DaGetAssignProductSummary(string branch_gid, MdlOtlMstBranch values)
        {
            try
            {

                msSQL = " SELECT a.product_image,a.product_gid,a.customerproduct_code,a.product_desc," +
                    " a.product_price, a.cost_price, a.product_code, " +
                    " CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name" +
                    " from pmr_mst_tproduct a where a.product_gid not in " +
                    " (select product_gid from otl_trn_branch2product where branch_gid = '"+branch_gid+"')order by a.product_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Prod_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Prod_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),

                        });
                        values.Prod_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // Assign Product
        public void DaPostAssignedlist(string user_gid, assign_list values)
        {
            try
            {
               
                for (int i = 0; i < values.prodassign.ToArray().Length; i++)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BPRO"); 

                    msSQL = " insert into otl_trn_branch2product ( " +
                            " branch2product_gid, " +
                            " branch_gid, " +
                            " product_gid, " +
                            " cost_price, " +
                            " created_by," +
                            " created_date ) " +
                            " values (  " +
                            " '" + msGetGid + "', " +
                            " '" + values.campaign_gid + "', " +
                            " '" + values.prodassign[i].product_gid + "', " +
                            " '" + values.prodassign[i].cost_price + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product assigned successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Product Assign";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Assigning Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        // UnAssign Product Summary
        public void DaGetUnAssignProductSummary(string campaign_gid, MdlOtlMstBranch values)
        {
            try
            {

                msSQL = " SELECT a.product_image,a.product_name,a.product_desc,a.product_gid,a.customerproduct_code,a.product_desc,a.product_price," +
                        " a.cost_price, a.product_code " +
                        " from pmr_mst_tproduct a " +
                        " left join otl_trn_branch2product b on b.product_gid = a.product_gid " +
                        " left join adm_mst_tuser f on f.user_gid = a.created_by " +
                        " where b.branch_gid in   " +
                        " (select branch_gid from otl_trn_branch2product " +
                        " where branch_gid = '" + campaign_gid + "')  " +
                        " group by a.product_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Prod_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Prod_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            sku = dt["customerproduct_code"].ToString()
                        });
                        values.Prod_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // UnAssign Product
        public void DaPostUnAssignedlist(string user_gid, assign_list values)
        {
            try
            {

                for (int i = 0; i < values.produnassign.ToArray().Length; i++)

                {


                    msSQL = " delete from otl_trn_branch2product where branch_gid='" + values.campaign_gid + "' and product_gid='" + values.produnassign[i].product_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Product Unassign Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while occured Unassigning Product";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Assigning Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        // Change Product Price
        public void DaPostChangePrice(string user_gid, assign_list values)
        {
            try
            {

                for (int i = 0; i < values.prodassign.Length; i++)
                {
                    msSQL = " update otl_trn_branch2product set" +
                         " cost_price='" + values.prodassign[i].cost_price + "' "+
                        "where branch_gid='" + values.campaign_gid + "' and product_gid='" + values.prodassign[i].product_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Product Price Changed Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while occured Changing Product Price";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Changing Product Price!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        // Number Popup
        public void DaGetBranchDetails(string campaign_gid, MdlOtlMstBranch values)
        {
            try
            {

                msSQL = "SELECT a.campaign_gid,a.campaign_title,a.branch,b.branch_gid,a.campaign_description," +
                        "  CASE  WHEN a.delete_flag = 'N' THEN 'Active' ELSE 'InActive'  END as outlet_status," +
                        "   (SELECT DISTINCT COUNT(campaign2manager_gid) FROM otl_trn_tcampaign2manager k WHERE k.campaign_gid = a.campaign_gid) as managercount," +
                        "   (SELECT DISTINCT COUNT(campaign2employee_gid) FROM otl_trn_tcampaign2employee m WHERE m.campaign_gid = a.campaign_gid) AS employeecount " +
                        "FROM otl_trn_touletcampaign a " +
                        "LEFT JOIN hrm_mst_tbranch b ON a.branch_gid = b.branch_gid " +
                        " Where  a.campaign_gid = '" + campaign_gid+"'" +
                        "ORDER BY a.campaign_gid ASC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<outlet_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new outlet_list
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            branch = dt["branch"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            campaign_description = dt["campaign_description"].ToString(),
                            Assigned_product = dt["employeecount"].ToString(),
                            managercount = dt["managercount"].ToString(),
                            outlet_status = dt["outlet_status"].ToString(),


                        });
                        values.outlet_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // Number Update
        public void DaPostNumberUpdate(string user_gid, product_list values)
        {
            try
            {
                    msSQL = " update  hrm_mst_tbranch  set " +
                              " contact_number = '" + values.mobile + "'" +
                              " where branch_gid='" + values.campaign_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Number Updated Successfully";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Number";
                    }
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }

        public void DaPostPincodeassing(string user_gid, PostPincode_list values)
        {
            try
            {
                for (int i = 0; i < values.pincodeassing.ToArray().Length; i++)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BRPI");
                    msSQL = "insert into otl_trn_branch2pincode ( " +
                        " branch2pincode_gid," +
                        " branch_gid," +
                        " pincode_id," +
                        " created_by," +
                        " created_date" +
                        ")values( " +
                        "'" + msGetGid + "'," +
                        "'" + values.branch_gid + "'," +
                        "'" + values.pincodeassing[i].pincode_id + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Pincode assigned successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Pincode Assign";
                    }
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while Assigning Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetAssignedPincodeSummary(string branch_gid, MdlOtlMstBranch values)
        {
            msSQL = "select a.pincode_code, a.pincode_id, a.deliverycost from adm_mst_tpincode a " +
                " left join otl_trn_branch2pincode b on a.pincode_id=b.pincode_id" +
                " where b.branch_gid='"+ branch_gid +"'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var Getassignpincode = new List<Getassignpincode_code>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach(DataRow row in dt_datatable.Rows)
                {
                    Getassignpincode.Add(new Getassignpincode_code
                    {
                        pincode_code = row["pincode_code"].ToString(),
                        pincode_id = row["pincode_id"].ToString(),
                        deliverycost = row["deliverycost"].ToString(),
                    });
                    values.Getassignpincode_code = Getassignpincode;
                }
            }
        }
        public void DaGetPincodeSummaryAssign(string branch_gid, MdlOtlMstBranch values)
        {
            try
            {
                msSQL = "select a.pincode_code, a.pincode_id, a.created_date,a.branch_gid, a.created_by," +
                    " b.branch_name as branch_name , a.deliverycost" +
                  " from adm_mst_tpincode a left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid  " +
                  "where a.branch_gid='" + branch_gid + "' and pincode_id" +
                  " not in (select pincode_id from otl_trn_branch2pincode where branch_gid='" + branch_gid + "')";
                var GetPincodeSummaryAssign = new List<GetOtlPincodeSummaryAssign>();
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        GetPincodeSummaryAssign.Add(new Models.GetOtlPincodeSummaryAssign
                        {
                            created_by = row["created_by"].ToString(),
                            pincode_code = row["pincode_code"].ToString(),
                            pincode_id = row["pincode_id"].ToString(),
                            created_date = row["created_date"].ToString(),
                            branch_gid = row["branch_gid"].ToString(),
                            branch_name = row["branch_name"].ToString(),
                            deliverycost = row["deliverycost"].ToString(),
                        });
                        values.GetOtlPincodeSummaryAssign = GetPincodeSummaryAssign;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Insert DeliveryCost data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                "***********" + ex.Message.ToString() + "***********" + values.message.ToString() +
                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Pincode/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }       
        public void DaRemovePincode(string user_gid, PostRemovePincode_list values)
        {
            for (int i = 0; i < values.branchunassign.ToArray().Length; i++)
            {
                msSQL = "delete from otl_trn_branch2pincode where " +
                "branch_gid='" + values.branch_gid + "' and pincode_id='" + values.branchunassign[i].pincode_id + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Pincode Removed successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while Removing Pincode.";
                }
            }            
        }
        public void DaGetAmendProductSummary(string branch_gid, MdlOtlMstBranch values)
        {
            msSQL = " select a.branch_gid, a.product_gid, a.branch2product_gid, a.cost_price,b.product_image," +
                " b.customerproduct_code, b.product_code, b.product_name, b.product_desc from" +
                " otl_trn_branch2product a" +
                " left join pmr_mst_tproduct b on b.product_gid = a.product_gid" +
                " where a.branch_gid='" + branch_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var GetAmendProduct = new List<GetAmendProduct_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach(DataRow row in dt_datatable.Rows)
                {
                    GetAmendProduct.Add(new GetAmendProduct_list
                    {
                        branch_gid = row["branch_gid"].ToString(),
                        product_gid = row["product_gid"].ToString(),
                        branch2product_gid = row["branch2product_gid"].ToString(),
                        cost_price = row["cost_price"].ToString(),
                        product_image = row["product_image"].ToString(),
                        customerproduct_code = row["customerproduct_code"].ToString(),
                        product_name = row["product_name"].ToString(),
                        product_code = row["product_code"].ToString(),
                        product_desc = row["product_desc"].ToString(),
                    });
                    values.GetAmendProduct_list = GetAmendProduct;
                }
            }
        }
    }
}
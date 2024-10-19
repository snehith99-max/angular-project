using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaIndentPriceEstimation
    {
        string msSQL, lscostcenter_gid = string.Empty;
        double lsbudgetallocated, lsamtused, lsavailable, lsprovisional, costcenter_amount;
        dbconn objdbconn = new dbconn();
        DataTable dt_table = new DataTable();
        OdbcDataReader objOdbcDataReader;
        int mnResult;
        cmnfunctions objcmnfunctions = new cmnfunctions();
        public void DaGetIndentPriceSummary(MdlIndentPriceEstimation values)
        {
            try
            {
                msSQL = "select distinct a.materialrequisition_gid,a.materialrequisition_gid as material, a.materialrequisition_status,b.user_firstname,Format(a.provisional_amount,2) as provisional_amount, " +
                    " date_format(a.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date,date_format(a.created_date,'%d-%m-%Y') as created_date,a.materialrequisition_reference, b.user_firstname,d.department_name,f.branch_name,f.branch_prefix" +
                    " from ims_trn_tmaterialrequisition a " +
                    " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                    " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                    " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                    " left join pmr_mst_tproducttype e on a.materialrequisition_type = e.producttype_gid" +
                    " left join hrm_mst_tbranch f on f.branch_gid = a.branch_gid " +
                    "where 0=0 Order by date(a.materialrequisition_date)desc, a.materialrequisition_date asc, a.materialrequisition_gid desc ";
                dt_table = objdbconn.GetDataTable(msSQL);
                var GetIndentPriceEstimate = new List<GetIndentPriceEstimate_list>();
                if (dt_table.Rows.Count > 0)
                {
                    foreach(DataRow row in dt_table.Rows)
                    {
                        GetIndentPriceEstimate.Add(new GetIndentPriceEstimate_list
                        {
                            materialrequisition_date = row["materialrequisition_date"].ToString(),
                            materialrequisition_gid = row["materialrequisition_gid"].ToString(),
                            material = row["material"].ToString(),
                            branch_name = row["branch_name"].ToString(),
                            department_name = row["department_name"].ToString(),
                            user_firstname = row["user_firstname"].ToString(),
                            materialrequisition_reference = row["materialrequisition_reference"].ToString(),
                            materialrequisition_status = row["materialrequisition_status"].ToString(),
                            provisional_amount = row["provisional_amount"].ToString(),
                            branch_prefix = row["branch_prefix"].ToString(),
                            created_date = row["created_date"].ToString(),
                        });
                    }
                    values.GetIndentPriceEstimate_list = GetIndentPriceEstimate;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Indent price Estimation Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetIDTPREstimateProductDetails(string materialrequisition_gid, MdlIndentPriceEstimation values)
        {
            try
            {
                msSQL = "select a.product_remarks, a.qty_requested, a.qty_issued,a.unit_price,  b.product_name, b.product_code, " +
                    " c.productgroup_name, f.productuom_name,a.product_gid,date_format(e.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date," +
                    " a.display_field  from ims_trn_tmaterialrequisitiondtl a" +
                    " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                    " left join pmr_mst_tproductuomclass d on d.productuomclass_gid = b.productuomclass_gid" +
                    " left join pmr_mst_tproductuom f on f.productuom_gid = a.productuom_gid " +
                    " left join ims_trn_tmaterialrequisition e on e.materialrequisition_gid = a.materialrequisition_gid " +
                    " where a.materialrequisition_gid = '" + materialrequisition_gid + "'";
                dt_table = objdbconn.GetDataTable(msSQL);
                var GetIDPREstimationProductDetails = new List<GetIDPREstimationProductDetails_list>();
                if (dt_table.Rows.Count > 0) 
                {
                    foreach(DataRow row in dt_table.Rows)
                    {
                        GetIDPREstimationProductDetails.Add(new GetIDPREstimationProductDetails_list
                        {
                            product_gid = row["product_gid"].ToString(),
                            product_code = row["product_code"].ToString(),
                            product_name = row["product_name"].ToString(),
                            display_field = row["display_field"].ToString(),
                            productgroup_name = row["productgroup_name"].ToString(),
                            productuom_name = row["productuom_name"].ToString(),
                            qty_requested = int.Parse(row["qty_requested"].ToString()),
                            unit_price = 0.00,
                        });
                    }
                    values.GetIDPREstimationProductDetails_list = GetIDPREstimationProductDetails;
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while Getting Indent price Estimation Details Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetIDTPREstimateDetails(string materialrequisition_gid, MdlIndentPriceEstimation values)
        {
            try
            {
                msSQL = "select  a.materialrequisition_gid, " +
                    " date_format(a.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date, a.approver_remarks," +
                    " a.materialrequisition_type,a.materialrequisition_remarks,a.materialrequisition_reference,e.branch_name,a.branch_gid, " +
                    " concat(b.user_firstname,' - ',b.user_lastname) as user_firstname,d.department_name, " +
                    " a.costcenter_gid,format(f.budget_available, 2) as budget_available,concat_ws('-',f.costcenter_name,f.costcenter_code) as costcenter_name, " +
                    " g.user_firstname as approvername,date_format(a.approved_date,'%d-%m-%Y' ) as approved_date,a.materialrequisition_status " +
                    " from ims_trn_tmaterialrequisition a " +
                    " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                    " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                    " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                    " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid " +
                    " left join pmr_mst_tcostcenter f on f.costcenter_gid = a.costcenter_gid " +
                    " left join adm_mst_tuser g on g.user_gid = a.approved_by " +
                    " where a.materialrequisition_gid = '" + materialrequisition_gid + "'";
                dt_table = objdbconn.GetDataTable(msSQL);
                var GetIDPREstimationDetails = new List<GetIDPREstimationDetails_list>();

                msSQL = " select costcenter_gid from ims_trn_tmaterialrequisition where materialrequisition_gid = '" + materialrequisition_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lscostcenter_gid = objOdbcDataReader["costcenter_gid"].ToString();

                    msSQL = " SELECT a.costcenter_gid, a.costcenter_code, a.costcenter_name," +
                    " a.budget_allocated as budget_allocated " +
                    " FROM pmr_mst_tcostcenter a  " +
                    " where costcenter_gid='" + lscostcenter_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        objOdbcDataReader.Read();
                        lsbudgetallocated = int.Parse(objOdbcDataReader["budget_allocated"].ToString());
                    }
                    msSQL = " SELECT a.costcenter_gid,sum(case when b.provisional_amount is null then 0.00 " +
                    " when b.provisional_amount is not null then " +
                    " b.provisional_amount end) as provisional_amount " +
                    " FROM pmr_mst_tcostcenter a" +
                    " left join pmr_trn_tpurchaserequisition b on b.costcenter_gid=a.costcenter_gid" +
                    " where a.costcenter_gid='" + lscostcenter_gid + "' and b.purchaseorder_flag <>'PO raised' and b.purchaserequisition_flag <> 'PR Pending Approval'  group by b.costcenter_gid";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        objOdbcDataReader.Read();
                        lsprovisional = int.Parse(objOdbcDataReader["provisional_amount"].ToString());
                    }
                    msSQL = " SELECT a.costcenter_gid,sum(case when b.total_amount is null then 0.00 " +
                    " when b.total_amount is not null then " +
                    " b.total_amount end) as total_amount " +
                    " FROM pmr_mst_tcostcenter a" +
                    " left join pmr_trn_tpurchaseorder b on b.costcenter_gid=a.costcenter_gid" +
                    " where a.costcenter_gid='" + lscostcenter_gid + "' and b.purchaseorder_flag = 'PO Approved' group by b.costcenter_gid";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true) 
                    {
                        lsamtused = Convert.ToDouble(objOdbcDataReader["total_amount"].ToString());
                    }
                    lsavailable = lsbudgetallocated - lsamtused - lsprovisional;
                    costcenter_amount = (lsavailable);

                    objOdbcDataReader.Close();
                }

                if (dt_table.Rows.Count > 0)
                {
                    foreach(DataRow row in dt_table.Rows)
                    {
                        GetIDPREstimationDetails.Add(new GetIDPREstimationDetails_list
                        {
                            user_firstname = row["user_firstname"].ToString(),
                            materialrequisition_remarks = row["materialrequisition_remarks"].ToString(),
                            materialrequisition_reference = row["materialrequisition_reference"].ToString(),
                            materialrequisition_date = row["materialrequisition_date"].ToString(),
                            materialrequisition_gid = row["materialrequisition_gid"].ToString(),
                            branch_name = row["branch_name"].ToString(),
                            department_name = row["department_name"].ToString(),
                            costcenter_name = row["costcenter_name"].ToString(),
                            materialrequisition_status = row["materialrequisition_status"].ToString(),
                            approver_remarks = row["approver_remarks"].ToString(),
                            costcenter_amount = costcenter_amount,
                        });
                    }
                    values.GetIDPREstimationDetails_list = GetIDPREstimationDetails;
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Getting Indent price Estimation Product Details Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetIDPRProductDetailsCheckPrice(string product_gid, MdlIndentPriceEstimation values)
        {
            try
            {
                msSQL = " select a.purchaseorderdtl_gid,e.productuom_name,date_format(b.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date,a.purchaseorder_gid," +
                    " a.product_gid,a.product_price,format(a.product_price,2) as price,c.vendor_gid,c.vendor_companyname,c.contactperson_name," +
                    " c.contact_telephonenumber,d.product_name  from pmr_trn_tpurchaseorderdtl a" +
                    " left join pmr_trn_tpurchaseorder b on a.purchaseorder_gid=b.purchaseorder_gid" +
                    " left join acp_mst_tvendor c on b.vendor_gid=c.vendor_gid" +
                    " left join pmr_mst_tproduct d on d.product_gid=a.product_gid" +
                    " left join pmr_mst_tproductuom e on e.productuom_gid=d.productuom_gid" +
                    " where a.product_gid='" + product_gid + "' order by (purchaseorder_date)desc,purchaseorder_date asc,purchaseorder_gid desc";
                dt_table = objdbconn.GetDataTable(msSQL);
                var GetIDPRProductDetailsCheckPrice = new List<GetIDPRProductDetailsCheckPrice_list>();
                if (dt_table.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_table.Rows)
                    {
                        GetIDPRProductDetailsCheckPrice.Add(new GetIDPRProductDetailsCheckPrice_list
                        {
                            purchaseorder_date = row["purchaseorder_date"].ToString(),
                            vendor_companyname = row["vendor_companyname"].ToString(),
                            product_name = row["product_name"].ToString(),
                            productuom_name = row["productuom_name"].ToString(),
                            price = row["price"].ToString(),
                            product_gid = row["product_gid"].ToString(),
                        });
                    }
                    values.GetIDPRProductDetailsCheckPrice_list = GetIDPRProductDetailsCheckPrice;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Indent price Estimation Check price !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaUpdateProductPriceGenerate(generateprice_list values)
        {
            try
            {
               
                for (int i=0; i< values.GetIDPREstimationProductDetails_list.ToArray().Length; i++)
                {
                    if (values.GetIDPREstimationProductDetails_list[i].unit_price != 0.00)
                    {
                        msSQL = "update ims_trn_tmaterialrequisitiondtl set" +
                        " unit_price='" + values.GetIDPREstimationProductDetails_list[i].unit_price + "'" +
                        " where materialrequisition_gid='" + values.materialrequisition_gid + "'" +
                        " and product_gid='" + values.GetIDPREstimationProductDetails_list[i].product_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }                   
                }
                for (int i = 0; i < values.GetIDPREstimationProductDetails_list.ToArray().Length; i++)
                {
                    if (values.GetIDPREstimationProductDetails_list[i].unit_price != 0.00)
                    {
                        lsprovisional = values.provisional + (values.GetIDPREstimationProductDetails_list[i].qty_requested * values.GetIDPREstimationProductDetails_list[i].unit_price);
                    }
                }
                lsprovisional = Math.Round(lsprovisional, 2);

                if (mnResult == 1)
                {                    
                    values.provisional = lsprovisional;
                    values.status = true;
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Getting Indent price Estimation Update provisional !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaUpdateProvisionalAmount(string materialrequisition_gid, string provisional_amount, MdlIndentPriceEstimation values)
        {
            try
            {
                msSQL = " update ims_trn_tmaterialrequisition set" +
                " provisional_amount='" + provisional_amount + "'" +
                " where materialrequisition_gid='" + materialrequisition_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Successfully updated the provisional amount.";
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while Getting Indent price Estimation Update provisional amount !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
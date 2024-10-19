using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ems.sales.DataAccess
{
    public class DaSmrProductHsnCode
    {
        string msSQL = string.Empty;
        cmnfunctions objcmnfunctions = new cmnfunctions();
        DataTable dt_datatable;
        dbconn objdbconn = new dbconn();
        private OdbcDataReader objOdbcDataReader;
        int mnResult;
        public void DaProductSummary(MdlSmrProductHsnCode values)
        {
            try
            {

                msSQL = " SELECT a.hsn_number,a.hsn_desc,a.gstproducttax_percentage,d.producttype_name,b.productgroup_name, a.product_desc,b.productgroup_code,a.product_gid, a.product_price, a.cost_price, a.mrp_price , a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
                    " c.productuomclass_code,e.productuom_code,c.productuomclass_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type,a.product_image, " +
                    " CASE  WHEN a.status = '1' THEN 'Active' WHEN a.status IS NULL OR a.status = '' OR a.status = '2' THEN 'InActive' END AS status," +
                    " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time, a.customerproduct_code  from pmr_mst_tproduct a " +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                    " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                    " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                    " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                    " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.product_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProductHsnCode_summary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new ProductHsnCode_summary
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            Status = dt["status"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),
                            gstproducttax_percentage = dt["gstproducttax_percentage"].ToString(),
                            hsn_desc = dt["hsn_desc"].ToString(),
                            hsn_number = dt["hsn_number"].ToString()


                        });
                        values.ProductHsnCode_summary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaUpdateProductHSNCode(string employee_gid, string product_gid, string product_hsncode, string product_hsncode_desc, string product_hsngst, MdlSmrProductHsnCode values)
        {
            try
            {
               

                    string lsHsnCode = product_hsncode;
                    string lsHsnDesc = product_hsncode_desc;
                    string lsHsnGst = product_hsngst;
                  
                    if (lsHsnCode != "" || lsHsnCode != null)
                    {
                        msSQL = " update pmr_mst_tproduct set hsn_number='" + lsHsnCode + "', " +
                               " updated_by='" + employee_gid + "',updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where product_gid='" + product_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "HSN Code Updated Successfully";
                        }
                    }
                    if (lsHsnDesc != "" || lsHsnDesc != null)
                    {
                        msSQL = " update pmr_mst_tproduct set hsn_desc='" + lsHsnDesc.Replace("'","") + "', " +
                                " updated_by='" + employee_gid + "',updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where product_gid='" + product_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "HSN Code Updated Successfully";
                        }
                    }
                    if (lsHsnGst != "" || lsHsnGst != null)
                    {
                        msSQL = " update pmr_mst_tproduct set gstproducttax_percentage='" + lsHsnGst + "', " +
                               " updated_by='" + employee_gid + "',updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where product_gid='" + product_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "HSN Code Updated Successfully";
                    }
                
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Updating Hsn Code !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
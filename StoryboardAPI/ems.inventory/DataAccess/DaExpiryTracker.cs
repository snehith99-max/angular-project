using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaExpiryTracker
    {
        string msSQL = string.Empty;
        int mnResult;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        DataTable dt_datatable;
        public void DaPostSetProductdays(PostExpiryDays_list values)
        {
            msSQL = " update pmr_mst_tproduct set expiry_days='" + values.expirydate + "' " +
                " where product_gid='" + values.product_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            { values.status = true; values.message = "Set expiry days successfully."; }
            else
            { values.status = false; values.message = "Error while Set expiry days."; }
        }
        public void DaGetUpcomingdays(MdlExpiryTracker values)
        {
            msSQL = " select a.product_name, a.product_code, a.customerproduct_code, a.cost_price, a.product_image," +
                " a.product_desc, a.product_gid, a.productuom_gid, a.productgroup_gid," +
                " b.productgroup_name, c.productuomclass_name, e.productuom_name," +
                " d.producttype_name" +
                " from pmr_mst_tproduct a" +
                " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                " where  date_add(current_date, interval a.expiry_days day) > current_date";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getupdaysproduct_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getupdaysproduct_list
                    {
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_desc = dt["product_desc"].ToString(),
                        producttype_name = dt["producttype_name"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        cost_price = dt["cost_price"].ToString(),
                        productuomclass_name = dt["productuomclass_name"].ToString(),
                        sku = dt["customerproduct_code"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                    });
                    values.Getupdaysproduct_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetExpirySummary(MdlExpiryTracker values)
        {
            msSQL = " select a.product_name, a.product_code, a.customerproduct_code, a.cost_price, a.product_image," +
                " a.product_desc, a.product_gid, a.productuom_gid, a.productgroup_gid," +
                " b.productgroup_name, c.productuomclass_name, e.productuom_name," +
                " d.producttype_name" +
                " from pmr_mst_tproduct a" +
                " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                " where date_add(current_date, interval a.expiry_days day) <= current_date";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getexpiryproduct_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getexpiryproduct_list
                    {
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_desc = dt["product_desc"].ToString(),
                        producttype_name = dt["producttype_name"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        cost_price = dt["cost_price"].ToString(),
                        productuomclass_name = dt["productuomclass_name"].ToString(),
                        sku = dt["customerproduct_code"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                    });
                    values.Getexpiryproduct_list = getModuleList;
                }
            }
            dt_datatable.Dispose();

        }
    }
}

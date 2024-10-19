using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaImsMstReorderlevel
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        int productadd, productuomclassadd, mnResult;
        string msGetGID, lsproductgroup_gid;

        public void DaGetImsMstReorderlevelSummary(MdlImsMstReorderlevel values)
        {
            try
            {

                msSQL = " SELECT a.rol_gid,a.product_gid,a.display_field, b.product_code, b.product_name,c.productgroup_name,e.branch_name,e.branch_prefix, " +
                " d.productuom_name,a.reorder_level " +
                " FROM ims_mst_trol a " +
                " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                " left join pmr_mst_tproductuom d on a.productuom_gid = d.productuom_gid " +
                " left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid " +
                " where 0 = 0  Order by a.rol_gid desc, date(a.created_date) desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<rol_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new rol_list
                        {

                            rol_gid = dt["rol_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            reorder_level = dt["reorder_level"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),

                        });
                        values.rol_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Reorder Level Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProductGroup(MdlImsMstReorderlevel values)
        {
            try
            {

                msSQL = " Select productgroup_gid, productgroup_name from pmr_mst_tproductgroup  " +
                    " order by productgroup_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProductGroup>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ProductGroup
                        {
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.ProductGroup = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Reorder Level Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProducttype(MdlImsMstReorderlevel values)
        {
            try
            {

                msSQL = " Select producttype_name,producttype_gid  " +
                    " from pmr_mst_tproducttype";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Producttype>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Producttype
                        {
                            producttype_name = dt["producttype_name"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                        });
                        values.Producttype = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Reorder Level Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProductUnitclass(MdlImsMstReorderlevel values)
        {
            try
            {

                msSQL = " Select productuomclass_gid, productuomclass_code, productuomclass_name  " +
                " from pmr_mst_tproductuomclass order by productuomclass_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProductUnitclass>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ProductUnitclass
                        {
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                        });
                        values.ProductUnitclass = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Reorder Level Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProductNamDtl(MdlImsMstReorderlevel values)
        {
            try
            {

                msSQL = "Select product_gid, product_name from pmr_mst_tproduct where status='1'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProductNamDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ProductNamDropdown

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.ProductNamDropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Reorder Level Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetBranchDtl(MdlImsMstReorderlevel values)
        {
            try
            {
                msSQL = "select branch_gid,branch_name, address1 from hrm_mst_tbranch";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<BranchDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new BranchDropdown

                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            address1 = dt["address1"].ToString(),

                        });
                        values.BranchDropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Reorder Level Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeproductName(string product_gid, MdlImsMstReorderlevel values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select a.product_name, a.product_code,a.mrp_price as cost_price,a.product_desc, " +
                        " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                    " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<productsCode>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productsCode
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),

                        });
                        values.productsCode = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }

        }

        public void DaGetonchangeProductNamDtl(string productgroup_gid, MdlImsMstReorderlevel values)
        {
            try
            {
                msSQL = "Select product_gid, product_name from pmr_mst_tproduct where productgroup_gid='" + productgroup_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Product_names>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Product_names

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.Product_names = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }

        }

        public void DaPostROL(string user_gid, postrollist values)
        {
            try
            {
                msSQL = " Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name = '" + values.productgroup_name + "' ";
                lsproductgroup_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select * from ims_mst_trol where productgroup_gid='" + lsproductgroup_gid + "' " +
                        " and product_gid='" + values.product_gid + "' and productuom_gid='" + values.uom_gid + "' " +
                        " and branch_gid= '" + values.branch_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Reorder level already set for the product!";
                    objOdbcDataReader.Close();
                }
                else { 
                objOdbcDataReader.Close();
                        msGetGID = objcmnfunctions.GetMasterGID("IRLM");

                        msSQL = " insert into ims_mst_trol ( " +
                                " rol_gid, " +
                                " branch_gid, " +
                                " productgroup_gid," +
                                " product_gid, " +
                                " productuom_gid, " +
                                " reorder_level," +
                                " display_field," +
                                " created_by, " +
                                " created_date )" +
                                " values ( " +
                                "'" + msGetGID + "', " +
                                "'" + values.branch_gid + "'," +
                                "'" + lsproductgroup_gid + "'," +
                                "'" + values.product_gid + "'," +
                                "'" + values.uom_gid + "', " +
                                "'" + values.reorder_level + "', " +
                                "'" + values.display_field.Replace("'", "\\\'") + "', " +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "ROL Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "ROL Inserted Failed";
                    }
                }
                
                productadd = 0;
                productuomclassadd = 0;



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while ROL!";
            }
        }

        public void DaGetEditROLSummary(string rol_gid, MdlImsMstReorderlevel values)
        {
            try
            {
                msSQL = " select a.rol_gid,a.branch_gid, a.productgroup_gid, a.product_gid, b.product_code, a.productuom_gid, a.reorder_level, " +
                        " b.product_name,c.productgroup_name,d.productuom_name,a.display_field,branch_name " +
                        " from ims_mst_trol a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on a.productuom_gid = d.productuom_gid " +
                        " left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid " +
                        " where rol_gid = '" + rol_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<EditRol_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new EditRol_list
                        {

                            rol_gid = dt["rol_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            product_desc = dt["display_field"].ToString(),
                            reorder_level = dt["reorder_level"].ToString(),



                        });
                        values.EditRol_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostROLUpdate(roledit_list values)
        {
            try
            {
                msSQL = " Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name = '" + values.productgroup_name + "'";
                string lsproductgroupgid=objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select product_gid from pmr_mst_tproduct where product_name = '" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update ims_mst_trol set " +
                        " branch_gid='" + values.branch_gid + "', " +
                        " product_gid ='" + lsproductgid + "'," +
                        " productuom_gid ='" + values.productuom_gid + "'," +
                        " productgroup_gid ='" + lsproductgroupgid + "'," +
                        " display_field ='" + values.product_desc.Replace("'","\\\'") + "'," +
                        " reorder_level='" + values.reorder_level + "'" +
                        " where rol_gid='" + values.rol_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Reorder level updated successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occurred while updating the rol";
                }
            }
            catch (Exception ex)
            {
                values.message = "Error occurred while updating the rol!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
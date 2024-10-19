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

namespace ems.outlet.Dataaccess
{
    public class DaOtlMstProduct
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL, symbol = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid, lsproductname, lsproductgid, final_path;
        int mnResult;

        // Summary
        public void DaGetProductSummary(string branch_gid, MdlOtlMstProduct values)
        {
            try
            {
                msSQL = " select symbol from crm_trn_tcurrencyexchange where default_currency='Y'";
                symbol = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT a.product_image,a.product_gid,a.customerproduct_code,a.product_desc,a.product_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name," +
                        " CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date,  "+
                        " (case when a.stockable = 'Y' then 'Yes' else 'No ' end) as stockable,(case when a.status = '1' then 'Active' else 'Inactive' end) as Status, " +
                        " (case when a.serial_flag = 'Y' then 'Yes' else 'No' end)as serial_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time, '  ', 'days') end)as lead_time " +
                        " from pmr_mst_tproduct a" +
                        " left join adm_mst_tuser f on f.user_gid = a.created_by order by a.product_gid asc";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Products_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Products_list
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
                            symbol = symbol,


                        });
                        values.Products_list = getModuleList;
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

        // Product Image
        public void DaGetProductImage(string user_gid,HttpRequest httpRequest, Products_list objResult )
        {

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            string product_name = httpRequest.Form[0];
            string file = httpRequest.Form[0];

            MemoryStream ms = new MemoryStream();

            try
            {
                msSQL = "select product_gid from pmr_mst_tproduct where product_name = '"+ product_name + "'";
                string lsProductgid = objdbconn.GetExecuteScalar(msSQL); 

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        string FileName = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        bool status1;


                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        ms.Close();

                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                         msSQL = "UPDATE pmr_mst_tproduct SET " +
                              "name = '" + FileName + "',"+
                                    "product_image = '" +
                                    ConfigurationManager.AppSettings["blob_imagepath1"] +
                                    final_path +
                                    msdocument_gid +
                                    FileExtension +
                                    ConfigurationManager.AppSettings["blob_imagepath2"] +
                                    "&" + ConfigurationManager.AppSettings["blob_imagepath3"] +
                                    "&" + ConfigurationManager.AppSettings["blob_imagepath4"] +
                                    "&" + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                    "&" + ConfigurationManager.AppSettings["blob_imagepath6"] +
                                    "&" + ConfigurationManager.AppSettings["blob_imagepath7"] +
                                    "&" + ConfigurationManager.AppSettings["blob_imagepath8"] + "' " +
                                    "WHERE product_gid = '" + lsProductgid + "'";



                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }


                if (mnResult != 0)
                {
                    objResult.status = true;
                    objResult.message = "Product Image Added Successfully !!";
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Adding Product Image !!";
                }

            }

            catch (Exception ex)
            {
                objResult.message = ex.ToString();
            }
            //return true;

        }

        // Post Product
        public void DaPostProduct(string user_gid, HttpRequest httpRequest, Products_list values)
        {

            try
            {
                msSQL = " select * from pmr_mst_tproduct where product_name= '" + values.product_name + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    //values.status = true;
                    values.message = "Product Name Already Exist";
                }
                else
                {

                    msGetGid = objcmnfunctions.GetMasterGID("PPTM");

                    

                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lsproduct_code = "PC" + "00" + lsCode;

                    msSQL = " insert into pmr_mst_tproduct (" +
                            " product_gid," +
                            " product_code," +
                            " product_name," +
                            " product_desc, " +
                           
                            " mrp_price, " +
                            " cost_price, " +
                            " avg_lead_time, " +
                            " stockable, " +
                            " status, " +
                            " purchasewarrenty_flag, " +
                            " expirytracking_flag, " +
                            " batch_flag," +
                            " serial_flag," +
                            " customerproduct_code, " +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid + "'," +
                            "'" + lsproduct_code + "',";
                    if (values.product_name == null || values.product_name == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.product_name.Replace("'", "\\'") + "',";
                    }
                    msSQL += "'" + values.product_desc.Replace("'", "\\'") + "',";
                            
                            
                    if (string.IsNullOrEmpty(values.mrp_price))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.mrp_price + "',";
                    }
                    msSQL += "'" + values.cost_price + "'," +
                             "'" + values.avg_lead_time + "'," +
                             "'" + "Y" + "'," +
                             "'" + "1" + "'," +
                             "'" + values.purchasewarrenty_flag + "'," +
                             "'" + values.expirytracking_flag + "'," +
                             "'" + values.batch_flag + "'," +
                             "'" + values.serial_flag + "'," +
                             "'" + values.sku + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product ";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }


        // Edit
        public void DaGetEditProductSummary(string product_gid, MdlOtlMstProduct values)
        {
            try
            {

                msSQL = "  select  a.product_image,a.name,a.batch_flag,a.serial_flag, a.purchasewarrenty_flag,a.expirytracking_flag,a.product_desc,a.avg_lead_time," +
                      "  a.mrp_price,a.cost_price,a.product_gid,a.product_name,a.product_code,b.productgroup_gid,b.productgroup_name,c.productuomclass_gid,c.productuomclass_name," +
                      "  d.producttype_gid,d.producttype_name,e.productuom_gid,e.productuom_name from pmr_mst_tproduct a " +
                      " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
                      " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid" +
                      " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                      " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid" +
                      " where a.product_gid='" + product_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Product_listedit>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Product_listedit
                        {

                            product_image = dt["name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            batch_flag = dt["batch_flag"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            expirytracking_flag = dt["expirytracking_flag"].ToString(),
                            purchasewarrenty_flag = dt["purchasewarrenty_flag"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            avg_lead_time = dt["avg_lead_time"].ToString(),



                        });
                        values.Product_listedit = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting edit product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }


        }


        // Update
        public void DaProductUpdate(string user_gid, Products_list values)
        {
            try
            {
                msSQL = " select product_gid,product_name from pmr_mst_tproduct where product_name='" + values.product_name.Trim() + "' ; ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)

                {
                    lsproductgid = objOdbcDataReader["product_gid"].ToString();
                    lsproductname = objOdbcDataReader["product_name"].ToString();
                }

                if (lsproductgid == values.product_gid)

                {
                
                    msSQL = " update  pmr_mst_tproduct  set " +
                    " product_name = '" + values.product_name.Trim() + "'," +
                    " product_code = '" + values.product_code + "'," +
                    " product_desc = '" + values.product_desc.Replace("'", "\\\'") + "'," +
                    " currency_code = '" + values.currency_code + "'," +
                    " mrp_price = '" + values.mrp_price + "'," +
                    " cost_price = '" + values.cost_price + "'," +
                    " avg_lead_time = '" + values.avg_lead_time + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where product_gid='" + values.product_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Product Updated Successfully";
                    }

                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product";
                    }
                }
                else
                {
                    //if (string.Equals(lsproductname, values.product_name, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    values.status = false;
                    //    values.message = "product Name Already Exist !!";
                    //}
                    
                    
                        msSQL = " update  pmr_mst_tproduct  set " +
                        " product_name = '" + values.product_name.Trim() + "'," +
                        " product_code = '" + values.product_code + "'," +
                        " product_desc = '" + values.product_desc + "'," +
                        " currency_code = '" + values.currency_code + "'," +
                        " mrp_price = '" + values.mrp_price + "'," +
                        " cost_price = '" + values.cost_price + "'," +
                        " avg_lead_time = '" + values.avg_lead_time + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where product_gid='" + values.product_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Product Updated Successfully";

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Product";
                        }
                    }

                
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }



        }


        // View
        public void DaGetViewProductSummary(string product_gid, MdlOtlMstProduct values)
        {
            try
            {

                msSQL = "  select CASE WHEN a.batch_flag = 'N' THEN 'NO' ELSE 'YES' END AS batch_flag,CASE WHEN a.serial_flag = 'N' THEN 'NO' ELSE 'YES' END AS serial_flag," +
                        " CASE WHEN a.purchasewarrenty_flag = 'N' THEN 'NO' ELSE 'YES' END AS purchasewarrenty_flag,CASE WHEN a.expirytracking_flag = 'N' THEN 'NO' ELSE 'YES' END AS expirytracking_flag," +
                        "  a.product_desc,a.avg_lead_time,a.mrp_price,e.producttype_name,a.cost_price,b.currency_code,a.product_gid,c.productgroup_name,a.product_name,f.productuomclass_name,a.product_code,d.productuom_name " +
                        "  from pmr_mst_tproduct a " +
                        "  left join crm_trn_tcurrencyexchange b on b.currency_code=a.currency_code" +
                        "  left  join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid" +
                        "  left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid" +
                        "  left  join pmr_mst_tproducttype e on a.producttype_gid=e.producttype_gid" +
                        "  left  join pmr_mst_tproductuomclass f on a.productuomclass_gid=f.productuomclass_gid" +
                        "  where a.product_gid='" + product_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Product_listview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Product_listview
                        {


                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            batch_flag = dt["batch_flag"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            expirytracking_flag = dt["expirytracking_flag"].ToString(),
                            purchasewarrenty_flag = dt["purchasewarrenty_flag"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            avg_lead_time = dt["avg_lead_time"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),

                        });
                        values.Product_listview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product view!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }


        // Drop-Downs

        public void DaGetProductGroup(MdlOtlMstProduct values)
        {
            try
            {

                msSQL = " Select productgroup_gid, productgroup_name from pmr_mst_tproductgroup  " +
                    " order by productgroup_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductGroup_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductGroup_list
                        {
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.GetProductGroup_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProducttype(MdlOtlMstProduct values)
        {
            try
            {

                msSQL = " Select producttype_name,producttype_gid  " +
                    " from pmr_mst_tproducttype";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProducttype_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProducttype_list
                        {
                            producttype_name = dt["producttype_name"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                        });
                        values.GetProducttype_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Product type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProductUnitclass(MdlOtlMstProduct values)
        {
            try
            {

                msSQL = " Select productuomclass_gid, productuomclass_code, productuomclass_name  " +
                " from pmr_mst_tproductuomclass order by productuomclass_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductUnitclass_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductUnitclass_list
                        {
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                        });
                        values.GetProductUnitclass_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product unit class!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void Dagettaxdropdown(MdlOtlMstProduct values)
        {
            try
            {

                msSQL = " Select tax_prefix,tax_gid  " +
                    " from acp_mst_ttax where active_flag='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tax_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new tax_list
                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_prefix"].ToString(),
                        });
                        values.tax_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProductUnit(MdlOtlMstProduct values)
        {
            try
            {

                msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
                        " order by a.sequence_level ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductUnit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductUnit_list
                        {
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),

                        });
                        values.GetProductUnit_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product UOM Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeProductUnitClass(string productuomclass_gid, MdlOtlMstProduct values)
        {
            try
            {
                msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
         " where b.productuomclass_gid ='" + productuomclass_gid + "' order by a.sequence_level  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductUnit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductUnit_list
                        {
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),

                        });
                        values.GetProductUnit_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while changing Product unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}
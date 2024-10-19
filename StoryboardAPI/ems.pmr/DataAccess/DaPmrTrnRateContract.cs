using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.pmr.Models;
using System.Globalization;

namespace ems.pmr.DataAccess
{
    public class DaPmrTrnRateContract
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable, dt_datatable1;
        string company_logo_path, authorized_sign_path;
        OdbcDataReader objOdbcDataReader, objODBCDataReader;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        int mnResult, mnResult2, mnResult1, mnResult3;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();
        string lsissuenow_qty, lsqty_issued, lsqty_requested, msGetGid, lsvendor;

        public void DaRateContractsummary(MdlPmrTrnRateContract values)
        {
            try
            {
                msSQL = " select a.ratecontract_gid,a.vendor_gid,DATE_FORMAT(a.agreement_date, '%d-%m-%Y') AS agreement_date,a.vendor_companyname, " +
                        " DATE_FORMAT(a.expairy_date, '%d-%m-%Y') AS expairy_date ,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date from pmr_trn_tratecontract a ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<contract_summarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = "select count(ratecontractdtl_gid) As assigned_product from pmr_trn_tratecontractdtl where ratecontract_gid='" + dt["ratecontract_gid"].ToString() + "'";
                        string lsassigned_product =objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new contract_summarylist
                        {
                            ratecontract_gid = dt["ratecontract_gid"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            agreement_date = dt["agreement_date"].ToString(),
                            expairy_date = dt["expairy_date"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            assigned_product = lsassigned_product,
                        });;
                        values.contract_summarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Rate Contract summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        // Inside your method or class
        public void DaImsvendorcontract(MdlPmrTrnRateContract values)
        {
                try
                {
                    // Fetch vendor details
                    msSQL = "SELECT a.vendor_code, a.vendor_gid, a.vendor_companyname FROM acp_mst_tvendor a WHERE vendor_gid NOT IN (SELECT vendor_gid FROM pmr_trn_tratecontract)";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<Imsvendorrate_list>();

                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new Imsvendorrate_list
                            {
                                vendor_gid = dt["vendor_gid"].ToString(),
                                vendor_companyname = dt["vendor_companyname"].ToString(),
                                vendor_code = dt["vendor_code"].ToString(),
                            });
                        values.Imsvendorrate_list = getModuleList;
                        }
                    }

                    // Fetch product type details
                    msSQL = "SELECT producttype_name, producttype_gid FROM pmr_mst_tproducttype";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var productTypeList = new List<producttype>();

                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            productTypeList.Add(new producttype
                            {
                                producttype_name = dt["producttype_name"].ToString(),
                                producttype_gid = dt["producttype_gid"].ToString(),
                            });
                        values.Imsvendorrate_list[0].producttype = productTypeList;
                    }
                    }

            

                    dt_datatable.Dispose();
                }
                catch (Exception ex)
                {
                    values.message = "Exception occurred while loading Vendor Details!";
                    objcmnfunctions.LogForAudit($"*******Date***** {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} *********** DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name} *********** {ex.Message} *********** {values.message} *****Query**** {msSQL} *******Apiref********", $"ErrorLog/Inventory/Log{DateTime.Now.ToString("yyyy-MM-dd HH")}.txt");
                }
        }


        public void DaPostratecontract(string user_gid, contract_summarylist values)
        {
            try
            {
                for (int i = 0; i < values.vendorgid.Count; i++)
                {
                    DateTime uiDate = DateTime.ParseExact(values.vendorgid[i].agreement_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string agreement_date = uiDate.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime uiDate1 = DateTime.ParseExact(values.vendorgid[i].expairy_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string expairy_date = uiDate1.ToString("yyyy-MM-dd HH:mm:ss");
                    msGetGid = objcmnfunctions.GetMasterGID("RCPM");
                    msSQL = " INSERT INTO pmr_trn_tratecontract (" +
                            " ratecontract_gid, " +
                            " vendor_gid, " +
                            " vendor_companyname, " +
                            " agreement_date, " +
                            " expairy_date, " +
                            " created_date," +
                            " created_by)" +
                            " VALUES (" +
                            " '" + msGetGid + "'," +
                            " '" + values.vendorgid[i].vendor_gid + "'," +
                            " '" + values.vendorgid[i].vendor_companyname + "', " +
                            " '" + agreement_date + "', " +
                            " '" + expairy_date + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                            " '" + user_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        for (int j = 0; j < values.vendorgid[i].producttype.Count; j++)
                        {
                            msSQL = "SELECT  producttype_code FROM pmr_mst_tproducttype where producttype_gid= '" + values.vendorgid[i].producttype[j].producttype_gid +"'";
                           string lsproductcode = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "SELECT producttype_name FROM pmr_mst_tproducttype where producttype_gid= '" + values.vendorgid[i].producttype[j].producttype_gid + "'";
                            string lsproductName = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = " insert into pmr_trn_trateproducttype (" +
                                    " ratecontract_gid, " +
                                    " vendor_gid, " +
                                    " producttype_gid, " +
                                    " producttype_code, " +
                                    " producttype_name, " +
                                    " created_date, " +
                                    " created_by)" +
                                    " values (" +
                                    " '" + msGetGid + "'," +
                                    " '" + values.vendorgid[i].vendor_gid + "', " +
                                    " '" + values.vendorgid[i].producttype[j].producttype_gid + "', " +
                                    " '" + lsproductcode + "', " +
                                    " '" + lsproductName + "', " +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                    " '" + user_gid + "')";
                            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }   
                    }
                } 

                if (mnResult1 != 0)
                {
                        values.status = true;
                        values.message = "Contract Added Successfully";
                }
                else
                {
                        values.status = false;
                        values.message = "Error While Adding Contract";
                } 
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Contract!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetcontractvendor(string ratecontract_gid, MdlPmrTrnRateContract values)
        {
            try
            {
                msSQL = " select a.vendor_gid,a.agreement_date,a.expairy_date,b.vendor_companyname,b.contactperson_name,b.contact_telephonenumber,b.email_id,b.gst_no, " +
                        " concat(c.address1,',',city,',',c.state,',',c.postal_code,',',d.country_name) as address from pmr_trn_tratecontract a " +
                        " left join acp_mst_tvendor b on b.vendor_gid = a.vendor_gid " +
                        " left join adm_mst_taddress c on c.address_gid = b.address_gid " +
                        " left join adm_mst_tcountry d on d.country_gid = c.country_gid " +
                        " where a.ratecontract_gid ='" + ratecontract_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<contractvendor_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        DateTime agreementDate;
                        DateTime expiryDate;
                        bool isAgreementDateParsed = DateTime.TryParse(dt["agreement_date"].ToString(), out agreementDate);
                        bool isExpiryDateParsed = DateTime.TryParse(dt["expairy_date"].ToString(), out expiryDate);
                        getModuleList.Add(new contractvendor_list
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            address = dt["address"].ToString(),
                            gst_no = dt["gst_no"].ToString(),
                            agreement_date = isAgreementDateParsed ? agreementDate.ToString("dd-MM-yyyy") : string.Empty,
                            expairy_date = isExpiryDateParsed ? expiryDate.ToString("dd-MM-yyyy") : string.Empty,
                        });
                    }
                    values.contractvendor_list = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetcontractProductSummary(string ratecontract_gid, MdlPmrTrnRateContract values)
        {
            try
            {
                var getModuleList = new List<contractproduct_list>();
                msSQL = " select c.producttype_name from pmr_trn_trateproducttype c  " +
                    " left join pmr_mst_tproduct a on c.producttype_gid = a.producttype_gid " +
                    " where c.ratecontract_gid = '" + ratecontract_gid + "'  group by c.producttype_gid order by c.ratecontract_gid";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count != 0)
                {
                    foreach (DataRow dt1 in dt_datatable1.Rows)
                    {
                        string producttype_name = dt1["producttype_name"].ToString();

                        msSQL = " SELECT a.product_name, a.customerproduct_code, a.product_code, a.cost_price, a.mrp_price,a.product_gid," +
                        " c.productgroup_gid,c.productgroup_name,c.productgroup_code,a.product_price,d.productuom_name," +
                        " g.productuomclass_name FROM pmr_mst_tproduct a " +
                        " LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid " +
                        " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.productuom_gid " +
                        " LEFT JOIN pmr_mst_tproductuomclass g ON g.productuomclass_gid = a.productuomclass_gid  " +
                        " where a.product_gid not in ( select product_gid from pmr_trn_tratecontractdtl " +
                        " where ratecontract_gid='" + ratecontract_gid + "') " +
                        " and a.producttype_gid in (select producttype_gid from  pmr_trn_trateproducttype where producttype_name='" + producttype_name + "' ) " +
                        " order by product_gid desc";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new contractproduct_list
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    cost_price = dt["cost_price"].ToString(),
                                    mrp_price = dt["mrp_price"].ToString(),
                                    product_gid = dt["product_gid"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    productgroup_code = dt["productgroup_code"].ToString(),
                                    product_price = dt["product_price"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    sku = dt["customerproduct_code"].ToString(),
                                });
                            }
                        }
                        dt_datatable.Dispose();
                    }

                    values.contractproduct_list = getModuleList;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
            //    public void DaGetcontractProductSummary(string ratecontract_gid,MdlPmrTrnRateContract values)
            //    {
            //    try
            //    {
            //        msSQL = " SELECT a.product_name, a.customerproduct_code, a.product_code, a.cost_price, a.mrp_price,a.product_gid," +
            //                " c.productgroup_gid,c.productgroup_name,c.productgroup_code,a.product_price,d.productuom_name," +
            //                " g.productuomclass_name FROM pmr_mst_tproduct a " +
            //                " LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid " +
            //                " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.productuom_gid " +
            //                " LEFT JOIN pmr_mst_tproductuomclass g ON g.productuomclass_gid = a.productuomclass_gid  " +
            //                " where a.product_gid not in ( select product_gid from pmr_trn_tratecontractdtl " +
            //                " where ratecontract_gid='"+ ratecontract_gid + "') order by product_gid desc";
            //        dt_datatable = objdbconn.GetDataTable(msSQL);
            //        var getModuleList = new List<contractproduct_list>();
            //        if (dt_datatable.Rows.Count != 0)
            //        {
            //            foreach (DataRow dt in dt_datatable.Rows)
            //            {
            //                getModuleList.Add(new contractproduct_list
            //                {
            //                    product_name = dt["product_name"].ToString(),
            //                    product_code = dt["product_code"].ToString(),
            //                    cost_price = dt["cost_price"].ToString(),
            //                    mrp_price = dt["mrp_price"].ToString(),
            //                    product_gid = dt["product_gid"].ToString(),
            //                    productgroup_name = dt["productgroup_name"].ToString(),
            //                    productgroup_gid = dt["productgroup_gid"].ToString(),
            //                    productgroup_code = dt["productgroup_code"].ToString(),
            //                    product_price = dt["product_price"].ToString(),
            //                    productuom_name = dt["productuom_name"].ToString(),
            //                    sku = dt["customerproduct_code"].ToString(),
            //                });
            //                values.contractproduct_list = getModuleList;
            //            }
            //        }
            //        dt_datatable.Dispose();
            //    }
            //    catch (Exception ex)
            //    {
            //        values.message = "Exception occured while loading Product Details!";
            //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            //    }

            //}

            public void DaPostMapProduct(string user_gid, assignproduct_list values)
        {
            try
            {
                msSQL = " select vendor_gid from pmr_trn_tratecontract where ratecontract_gid='" + values.ratecontract_gid + "'";
                lsvendor=objdbconn.GetExecuteScalar(msSQL);
                for (int i = 0; i < values.contractassignlist.Count; i++)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("RCDP");
                    msSQL = " INSERT INTO pmr_trn_tratecontractdtl (" +
                            " ratecontractdtl_gid, " +
                            " ratecontract_gid, " +
                            " vendor_gid, " +
                            " productgroup_gid, " +
                            " product_gid, " +
                            " product_name, " +
                            " product_price, " +
                            " created_date," +
                            " created_by)" +
                            " VALUES (" +
                            " '" + msGetGid + "'," +
                            " '" + values.ratecontract_gid + "'," +
                            " '" + lsvendor + "'," +
                            " '" + values.contractassignlist[i].productgroup_gid + "', " +
                            " '" + values.contractassignlist[i].product_gid + "', " +
                            " '" + values.contractassignlist[i].product_name + "', " +
                            " '" + values.contractassignlist[i].product_price + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                            " '" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Assigned Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Assign Product!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProductunAssignSummary(string ratecontract_gid, MdlPmrTrnRateContract values)
        {
            try
            {
                msSQL = " SELECT a.product_name, a.customerproduct_code, a.product_code, a.cost_price, a.mrp_price,a.product_gid, c.productgroup_gid," +
                        " c.productgroup_name,c.productgroup_code,e.product_price,d.productuom_name, g.productuomclass_name FROM pmr_mst_tproduct a " +
                        " LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid " +
                        " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.productuom_gid " +
                        " LEFT JOIN pmr_mst_tproductuomclass g ON g.productuomclass_gid = a.productuomclass_gid " +
                        " left join  pmr_trn_tratecontractdtl e on e.product_gid=a.product_gid " +
                        " where a.product_gid in ( select product_gid from pmr_trn_tratecontractdtl " +
                        " where ratecontract_gid='"+ ratecontract_gid + "') order by product_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<unassignproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new unassignproduct_list
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),
                        });
                        values.unassignproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Assigned Product list Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaUnAssignProduct(string user_gid, assignproduct_list values)
        {
            try
            {
                for (int i = 0; i < values.contractassignlist.Count; i++)
                {
                    msSQL = "delete from pmr_trn_tratecontractdtl where ratecontract_gid='"+values.ratecontract_gid+"' and product_gid='"+ values.contractassignlist[i].product_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Product Unassigned successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while unassigning product";
                    }
                }
            }   
            catch (Exception ex)
            {
                values.message = "Exception occured while unassign data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostAmendProduct(string user_gid, assignproduct_list values)
        {
            try
            {
                for (int i = 0; i < values.contractassignlist.Count; i++)
                {
                    msSQL = " select * from pmr_trn_tratecontractdtl where ratecontract_gid= '" + values.ratecontract_gid + "' and product_gid= '" + values.contractassignlist[i].product_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("RCLM");
                            msSQL = " INSERT INTO pmr_trn_tratecontractlog (" +
                                    " ratecontractlog_gid, " +
                                    " ratecontractdtl_gid, " +
                                    " ratecontract_gid, " +
                                    " vendor_gid, " +
                                    " productgroup_gid, " +
                                    " product_gid, " +
                                    " product_name, " +
                                    " product_price, " +
                                    " created_date," +
                                    " created_by)" +
                                    " VALUES (" +
                                    " '" + msGetGid + "'," +
                                    " '" + dt["ratecontractdtl_gid"].ToString() + "'," +
                                    " '" + dt["ratecontract_gid"].ToString() + "'," +
                                    " '" + dt["vendor_gid"].ToString() + "', " +
                                    " '" + dt["productgroup_gid"].ToString() + "', " +
                                    " '" + dt["product_gid"].ToString() + "', " +
                                    " '" + dt["product_name"].ToString() + "', " +
                                    " '" + dt["product_price"].ToString() + "', " +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                    " '" + user_gid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }

                if (mnResult != 0)
                {
                    for (int i = 0; i < values.contractassignlist.Count; i++)
                    {
                        msSQL = " update pmr_trn_tratecontractdtl " +
                                  " set product_price ='" + values.contractassignlist[i].product_price + "', " +
                                  " updated_by ='" + user_gid + "', " +
                                  " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                  " where product_gid = '" + values.contractassignlist[i].product_gid + "' and " +
                                  " ratecontract_gid = '" + values.ratecontract_gid + "'";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult1 != 0)
                    {
                        values.status = true;
                        values.message = "Product Amend Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Amend Product!!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Amend Product!!";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetcontractamend(string ratecontract_gid,string product_gid, MdlPmrTrnRateContract values)
        {
            try
            {
                msSQL = " select a.product_price,Concat(b.user_firstname,' ',b.user_lastname) as created_by, " +
                        " DATE_FORMAT(a.created_date, '%d-%m-%Y') AS  created_date from pmr_trn_tratecontractlog a "+
                        " left join adm_mst_tuser b on b.user_gid = a.created_by " +
                        " where ratecontract_gid = '"+ ratecontract_gid  +"' and product_gid = '"+ product_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<contractamend_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new contractamend_list
                        {
                            product_price = dt["product_price"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        
                        });
                        values.contractamend_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void Davendorposummary(MdlPmrTrnRateContract values)
        {
            try
            {
                msSQL = " select a.ratecontract_gid,a.vendor_gid,a.vendor_companyname,b.vendor_code, "+
                        " concat(b.contactperson_name,'/',b.contact_telephonenumber,'/',b.email_id) as vendor_details from pmr_trn_tratecontract a "+
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<contract_summarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = "select count(ratecontractdtl_gid) As assigned_product from pmr_trn_tratecontractdtl where ratecontract_gid='" + dt["ratecontract_gid"].ToString() + "'";
                        string lsassigned_product = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new contract_summarylist
                        {
                            ratecontract_gid = dt["ratecontract_gid"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            vendor_details = dt["vendor_details"].ToString(),
                            assigned_product = lsassigned_product,
                        }); ;
                        values.contract_summarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Rate Contract summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}
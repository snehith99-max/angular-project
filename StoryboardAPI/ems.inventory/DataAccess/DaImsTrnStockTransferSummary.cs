using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Runtime.Remoting;


namespace ems.inventory.DataAccess
{
    public class DaImsTrnStockTransferSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string msGetGID, msGetGID1, msstocktransferDTL, msstockdtlGid, mssalesorderGID, msGetStockGID, lsstock_quantity, lsuom_gid, lslocationto_gid;
        int mnResult, mnResult1, mnResult3;
        string lsstart_date = "", lsend_date = "";
        double lsbudgetallocated, lsprovisional, lsamtused, lsavailable, lsreq, lstolrequest, lsreq1, lsrequested;

        public void DaGetLocation(string branch_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select location_gid,location_name,location_code from  ims_mst_tlocation " +
                        " where branch_gid='" + branch_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Location>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Location
                        {
                            location_name = dt["location_name"].ToString(),
                            location_gid = dt["location_gid"].ToString(),

                        });
                        values.Location = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Location Dropdown !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetLocationTo(string branch_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select location_gid as locationto_gid,location_name as locationto_name,location_code as locationto_code from  ims_mst_tlocation " +
                    " where branch_gid='" + branch_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<LocationTo>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new LocationTo
                        {
                            locationto_name = dt["locationto_name"].ToString(),
                            locationto_gid = dt["locationto_gid"].ToString(),

                        });
                        values.LocationTo = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Location Dropdown !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetBranchWiseSummary(MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.stocktransfer_gid, b.branch_name as branch_from,c.branch_name as branch_to, a.transfered_by, a.status, " +
                        " date_format(a.transfered_date, '%d-%m-%Y') as transfer_date, a.product_gid, a.remarks, a.si_no, a.stock_gid, f.user_firstname " +
                        " from ims_trn_tstocktransfer a " +
                        " left join hrm_mst_tbranch b on a.branchfrom_gid = b.branch_gid " +
                        " left join hrm_mst_tbranch c on a.branchto_gid = c.branch_gid " +
                        " left join adm_mst_tuser f on f.user_gid = a.transfered_by " +
                        " where a.branchto_gid is not null " +
                        " order by date(a.transfered_date) desc,a.transfered_date asc, a.stocktransfer_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferbranchsummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferbranchsummary
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            branch_from = dt["branch_from"].ToString(),
                            branch_to = dt["branch_to"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            status = dt["status"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),

                        });
                        values.stocktransferbranchsummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetBranchWiseView(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.stocktransfer_gid, b.branch_name as branch_from,c.branch_name as branch_to, f.user_firstname as transfered_by, a.status, " +
                          " date_format(a.transfered_date, '%d-%m-%Y') as transfer_date, a.product_gid, a.remarks, a.si_no, a.stock_gid, f.user_firstname,  " +
                          " g.stock_qty, d.productuom_name,e.product_code,e.product_desc, e.product_name,h.productgroup_name   " +
                          " from ims_trn_tstocktransfer a " +
                          " left join ims_trn_tstocktransferdtl g on a.stocktransfer_gid = g.stocktransfer_gid " +
                          " left join hrm_mst_tbranch b on a.branchfrom_gid = b.branch_gid " +
                          " left join hrm_mst_tbranch c on a.branchto_gid = c.branch_gid " +
                          " left join adm_mst_tuser f on f.user_gid = a.transfered_by " +
                          " left join pmr_mst_tproduct e on e.product_gid = a.product_gid " +
                          " left join pmr_mst_tproductuom d on d.productuom_gid = g.productuom_gid " +
                          " left join pmr_mst_tproductgroup h on h.productgroup_gid=e.productgroup_gid " +
                          " where a.stocktransfer_gid = '" + stocktransfer_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferbranchview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferbranchview
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            branch_from = dt["branch_from"].ToString(),
                            branch_to = dt["branch_to"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            status = dt["status"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_desc = dt["product_desc"].ToString(),

                        });
                        values.stocktransferbranchview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetLocationView(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = "select stocktransfer_gid, b.location_name as branch_from,c.location_name as branch_to, transfered_by, date_format(transfered_date,'%d-%m-%Y') as transfer_date, product_gid, remarks, si_no, stock_gid " +
                " from ims_trn_tstocktransfer a " +
                " left join ims_mst_tlocation b on a.locationfrom_gid=b.location_gid " +
                " left join ims_mst_tlocation c on a.locationto_gid=c.location_gid " +
                " where stocktransfer_gid='" + stocktransfer_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferlocationview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferlocationview
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            branch_from = dt["branch_from"].ToString(),
                            branch_to = dt["branch_to"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                        });
                        values.stocktransferlocationview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetLocationProductView(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.stocktransferdtl_gid, a.stock_qty,c.productgroup_name,a.product_gid,a.display_field, " +
             " b.product_code,b.product_name,d.productuom_name from ims_trn_tstocktransferdtl a " +
             " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
             " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
             " left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid " +
              " where stocktransfer_gid='" + stocktransfer_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferlocationproductview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferlocationproductview
                        {
                            stocktransferdtl_gid = dt["stocktransferdtl_gid"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                        });
                        values.stocktransferlocationproductview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetLocationWiseSummary(MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.stocktransfer_gid, b.location_name as branch_from,c.location_name as branch_to, a.transfered_by, a.status, " +
                        " date_format(a.transfered_date, '%d-%m-%Y') as transfer_date, a.product_gid, a.remarks, a.si_no, a.stock_gid, f.user_firstname " +
                        " from ims_trn_tstocktransfer a " +
                        " left join ims_mst_tlocation b on a.locationfrom_gid = b.location_gid " +
                        " left join ims_mst_tlocation c on a.locationto_gid = c.location_gid " +
                        " left join adm_mst_tuser f on f.user_gid = a.transfered_by " +
                        " where a.locationto_gid is not null " +
                        " order by date(a.transfered_date) desc,a.transfered_date asc, a.stocktransfer_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransfersummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransfersummary
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            branch_from = dt["branch_from"].ToString(),
                            branch_to = dt["branch_to"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            status = dt["status"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),

                        });
                        values.stocktransfersummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetBranchWiseaddSummary(string branch_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.product_gid,a.uom_gid,a.stock_gid,a.created_date,a.reference_gid, " +
                            " (sum(a.stock_qty + a.amend_qty - a.issued_qty - a.damaged_qty - a.transfer_qty)) as stock_balance, " +
                            " a.branch_gid,b.product_code,b.product_name,a.display_field,c.productgroup_name,d.productuom_name,e.branch_name,f.producttype_name,b.serial_flag,  " +
                            " sum(a.transfer_qty) as transfer_qty,g.location_name,h.bin_number from ims_trn_tstock a " +
                            " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                            " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                            " left join pmr_mst_tproductuom d on a.uom_gid = d.productuom_gid " +
                            " left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid " +
                            " left join pmr_mst_tproducttype f on b.producttype_gid = f.producttype_code " +
                            " left join ims_mst_tlocation g on a.location_gid = g.location_gid " +
                            " left join ims_mst_tbin h on a.bin_gid = h.bin_gid " +
                            " where a.stock_flag = 'Y' and a.branch_gid = '" + branch_gid + "'  and a.stock_qty + a.amend_qty - a.issued_qty - a.damaged_qty - transfer_qty > 0 " +
                            " group by a.stock_gid Order by date(a.created_date) desc,a.created_date asc, a.stock_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branchaddsummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branchaddsummary
                        {
                            product_gid = dt["product_gid"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            reference_gid = dt["reference_gid"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            transfer_qty = dt["transfer_qty"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            bin_number = dt["bin_number"].ToString(),

                        });
                        values.branchaddsummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetBranchWiseTransfer(string stock_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = "  select  b.branch_name,a.unit_price,c.product_code,d.productuom_name,c.product_desc,e.productgroup_name,a.stock_gid, " +
                        "  c.product_name,a.product_gid,sum(a.stock_qty + a.amend_qty - a.issued_qty - a.damaged_qty - a.transfer_qty) as stock_qty,a.uom_gid,  " +
                        " cast(concat(sum(a.stock_qty + a.amend_qty - a.issued_qty - a.damaged_qty - a.transfer_qty), ' ', d.productuom_name) as char) as stock " +
                        " from ims_trn_tstock a " +
                        " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
                        " left join pmr_mst_tproduct c on a.product_gid = c.product_gid " +
                        " left join pmr_mst_tproductuom d on d.productuom_gid = c.productuom_gid " +
                        " left join pmr_mst_tproductgroup e on e.productgroup_gid = c.productgroup_gid " +
                        " where a.stock_gid = '" + stock_gid + "' group by a.stock_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branchtransfer>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branchtransfer
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            unit_price = dt["unit_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),

                        });
                        values.branchtransfer = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        public void DaGetProductCode1(string branch_gid, string location_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {


                msSQL = " select a.product_gid,b.product_name,b.product_code,c.productgroup_name,a.stock_gid from ims_trn_tstock a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                        " where a.branch_gid = '" + branch_gid + "' and a.location_gid = '" + location_gid + "' " +
                        " group by a.product_gid ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductCode1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductCode1
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                        });
                        values.GetProductCode1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while gettting product code! !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetProductgroup(string branch_gid, string location_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {

                msSQL = " select a.product_gid,b.product_name,b.product_code,c.productgroup_name,a.stock_gid from ims_trn_tstock a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                        " where a.branch_gid = '" + branch_gid + "' and a.location_gid = '" + location_gid + "' " +
                        " group by a.product_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductgroup>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductgroup
                        {
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.GetProductgroup = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProduct1(string branch_gid, string location_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {

                msSQL = " select a.product_gid,b.product_name,b.product_code,c.productgroup_name,a.stock_gid from ims_trn_tstock a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                        " where a.branch_gid = '" + branch_gid + "' and a.location_gid = '" + location_gid + "' " +
                        " group by a.product_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProduct1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProduct1
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.GetProduct1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetPopSummary(string branch_gid, string location_gid, string productuom_gid, string product_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {

                msSQL = " select a.created_date,a.stock_gid,a.product_gid,a.display_field,a.uom_gid,a.reference_gid, " +
                       " sum(a.stock_qty + amend_qty - damaged_qty - a.issued_qty - transfer_qty) as stock_qty,b.product_name,c.productuom_name,b.product_code,d.productgroup_name " +
                       " from ims_trn_tstock a " +
                       " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                       " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid " +
                       " left join pmr_mst_tproductgroup d on d.productgroup_gid = b.productgroup_gid " +
                       " where a.product_gid = '" + product_gid + "' " +
                       " and a.uom_gid = '" + productuom_gid + "' and a.stock_flag = 'Y' " +
                       " and a.branch_gid = '" + branch_gid + "' and a.location_gid = '" + location_gid + "' " +
                       " group by stock_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPopsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPopsummary_list
                        {
                            created_date = dt["created_date"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            reference_gid = dt["reference_gid"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.GetPopsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPopSubmit(string employee_gid, string branch_gid, GetPopsummary_list values)
        {
            try
            {
                msSQL = " select a.created_date,a.stock_gid,a.product_gid,a.display_field,a.uom_gid,a.reference_gid, " +
                       " sum(a.stock_qty + amend_qty - damaged_qty - a.issued_qty - transfer_qty) as stock_qty,b.product_name,c.productuom_name,b.product_code,d.productgroup_name " +
                       " from ims_trn_tstock a " +
                       " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                       " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid " +
                       " left join pmr_mst_tproductgroup d on d.productgroup_gid = b.productgroup_gid " +
                       " where a.product_gid = '" + values.product_gid + "' " +
                       " and a.uom_gid = '" + values.productuom_gid + "' and a.stock_flag = 'Y' " +
                       " and a.branch_gid = '" + branch_gid + "' and a.location_gid = '" + values.location_gid + "' " +
                       " group by stock_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPopsummary_list>();
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    double issuedQty = Convert.ToDouble(values.issued_qty);
                    double stockQty = Convert.ToDouble(values.stock_qty);

                    if (issuedQty > stockQty)
                    {
                        values.message = "Quantity Transfer cannot be greater than Stock in hand ";
                        return;
                    }
                }
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    double issueQTY = Convert.ToDouble(values.issued_qty);
                    double stockQty = Convert.ToDouble(values.stock_qty);

                    if (issueQTY <= stockQty)
                    {

                        if (issueQTY == 0.00 || issueQTY == 0)
                        {
                        }
                        else
                        {
                            msSQL = " select stock_quantity from ims_tmp_tstock where created_by='" + employee_gid + "' and product_gid='" + values.product_gid + "' " +
                           " and stock_gid='" + values.stock_gid + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows == true)
                            {
                                msSQL = " UPDATE ims_trn_tstock SET " +
                                        " transfer_qty = (transfer_qty - " + objOdbcDataReader["stock_quantity"].ToString().Replace("-", "") + ") " +
                                        " WHERE stock_gid = '" + values.stock_gid + "'";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = " update ims_tmp_tstock set " +
                                " stock_quantity= stock_quantity + '" + issueQTY + " '" +
                                " where stock_gid='" + values.stock_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                objOdbcDataReader.Close();
                            }
                            else
                            {

                                msSQL = " insert into ims_tmp_tstock(" +
                                 " stock_gid," +
                                 " product_gid," +
                                 " stock_quantity," +
                                 " created_by," +
                                 " created_date," +
                                 " branch_gid," +
                                 " location_gid," +
                                 " productuom_gid," +
                                 " display_field" +
                                 ") values (" +
                                "'" + values.stock_gid + "'," +
                                "'" + values.product_gid + "'," +
                                "'" + issueQTY + "'," +
                                "'" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'" + branch_gid + "'," +
                                "'" + values.location_gid + "'," +
                                "'" + values.productuom_gid + "'," +
                                "'" + values.display_field + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }
                    else
                    {
                        values.message = "Issue Quantity must be Less than or equal to Actual  Quantity";
                        return;
                    }
                }
                values.stock_total = "0.00";
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Quantity";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetStockQuantity(string stock_gid, string product_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {


                msSQL = "select stock_quantity from ims_tmp_tstock where stock_gid ='" + stock_gid + "' and product_gid ='" + product_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPop_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPop_list
                        {
                            stock_quantity = dt["stock_quantity"].ToString()
                        });
                        values.GetPop_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while gettting product code! !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostOnAddpr(string user_gid, productlist1 values)
        {
            try
            {

                mssalesorderGID = objcmnfunctions.GetMasterGID("TMST");
                msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                string lsproductName = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL= " select stock_qty,stock_quantity from ims_tmp_tstocktransferdtl where created_by='" + user_gid + "' and " +
                       " product_gid ='"+ values.product_gid + "' and " +
                       " productuom_gid='"+ lsproductuomgid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lsrequested = (double)dt["stock_qty"];
                        lsreq1 = values.qty_requested;
                        lstolrequest = lsreq1 + lsrequested;
                        lsreq = values.stock_quantity;
                        if (lsreq < lstolrequest)
                        {
                            values.status = false;
                            values.message = "Requested Quantity should not be higher than available stock!";
                        }
                        else
                        {
                            msSQL= " update ims_tmp_tstocktransferdtl" +
                                   " set stock_qty ='" + lstolrequest + "' " +
                                   " where  " +
                                   " product_gid = '" + values.product_gid + "' and " +
                                   " productuom_gid = '" + lsproductuomgid + "' and" +
                                   " created_by = '" + user_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Stock Qty Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Stock Qty!!";
                            }
                        }
                    }
                }
                else
                {
                    msSQL = " insert into ims_tmp_tstocktransferdtl (" +
                            " tmpstocktransfer_gid," +
                            " product_gid," +
                            " productuom_gid," +
                            " stock_qty," +
                            " display_field," +
                            " stock_quantity," +
                            " created_date," +
                            " created_by)" +
                            " values(" +
                            "'" + mssalesorderGID + "'," +
                            "'" + values.product_gid + "'," +
                            "'" + lsproductuomgid + "'," +
                            "'" + values.qty_requested + "'," +
                            "'" + values.display_field + "'," +
                            "'" + values.stock_quantity + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'" + user_gid + "')";


                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
        public void DaProductSummary(string user_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.tmpstocktransfer_gid,a.product_gid,a.stock_qty,a.stock_quantity,c.productgroup_name,a.display_field, " +
                " b.product_code,b.product_name,d.productuom_name from ims_tmp_tstocktransferdtl a " +
                " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                " left join pmr_mst_tproductuom d on b.productuom_gid=d.productuom_gid " +
                " where a.created_by='" + user_gid + "'";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productsummary_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new productsummary_list1
                        {

                            tmpstocktransfer_gid = dt["tmpstocktransfer_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["stock_qty"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),




                        });
                        values.productsummary_list1 = getModuleList;
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
        public void DaGetDeletePrProductSummary(string user_gid, string tmpstocktransfer_gid, productsummary_list1 values)
        {
            try
            {
                msSQL = " delete from ims_tmp_tstocktransferdtl " +
                        " where created_by='" + user_gid + "' and tmpstocktransfer_gid='" + tmpstocktransfer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product  Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while delete product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPoststocktransfer(string branch_gid, string user_gid, Poststocktransfer values)
        {
            try
            {

                msGetGID = objcmnfunctions.GetMasterGID("ISRP");
                if (msGetGID == "E")
                {
                    values.status = true;
                    values.message = "Create sequence code ISRP for temp sales enquiry";
                }
                msSQL = " insert into ims_trn_tstocktransfer (" +
                    " stocktransfer_gid," +
                    " product_gid," +
                    " branchfrom_gid, " +
                    " branchto_gid, " +
                    " transfered_by," +
                    " transfered_date," +
                    " created_by," +
                    " created_date," +
                    " stock_gid, " +
                    " remarks ) " +
                    " values ( " +
                    " '" + msGetGID + "', " +
                    " '" + values.product_gid + "'," +
                    " '" + branch_gid + "'," +
                    " '" + values.branch_gid + "'," +
                    " '" + user_gid + "'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " '" + user_gid + "'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " '" + values.stock_gid + "'," +
                    " '" + values.remarks.Replace("'", "\\\'") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msstocktransferDTL = objcmnfunctions.GetMasterGID("STDT");
                if (msstocktransferDTL == "E")
                {
                    values.status = true;
                    values.message = "Create sequence code STDT for temp sales enquiry";
                }
                msSQL = "select uom_gid from ims_trn_tstock where stock_gid='" + values.stock_gid + "' and branch_gid='" + branch_gid + "'";
                lsuom_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " insert into ims_trn_tstocktransferdtl ( " +
                       " stocktransferdtl_gid," +
                       " stocktransfer_gid," +
                       " stock_qty," +
                       " product_gid," +
                       " productuom_gid," +
                       " stock_gid, " +
                       " created_date, " +
                       " created_by)" +
                       " values(" +
                       "'" + msstocktransferDTL + "'," +
                       "'" + msGetGID + "'," +
                       "'" + values.transfer_stock + "'," +
                       "'" + values.product_gid + "'," +
                       "'" + lsuom_gid + "'," +
                       "'" + values.stock_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       "'" + user_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msstockdtlGid = objcmnfunctions.GetMasterGID("ISTP");
                if (msstockdtlGid == "E")
                {
                    values.status = true;
                    values.message = "Create sequence code ISTP for temp sales enquiry";
                }
                msSQL = " insert into ims_trn_tstockdtl(" +
                  " stockdtl_gid," +
                  " stock_gid," +
                  " branch_gid," +
                  " product_gid," +
                  " uom_gid," +
                  " issued_qty," +
                  " amend_qty," +
                  " damaged_qty," +
                  " adjusted_qty," +
                  " transfer_qty," +
                  " return_qty," +
                  " reference_gid," +
                  " stock_type," +
                  " remarks," +
                  " created_by," +
                  " created_date," +
                  " display_field" +
                  " ) values ( " +
                  "'" + msstockdtlGid + "'," +
                  "'" + values.stock_gid + "'," +
                  "'" + values.branch_gid + "'," +
                  "'" + values.product_gid + "'," +
                  "'" + lsuom_gid + "'," +
                  "'0.00'," +
                  "'0.00'," +
                  "'0.00'," +
                  "'0.00'," +
                  "'" + values.transfer_stock + "'," +
                  "'0.00'," +
                  "'" + msstocktransferDTL + "'," +
                  "'Transfer'," +
                  "'" + values.remarks.Replace("'", "\\\'") + "'," +
                  "'" + values.employee_gid + "'," +
                  "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                  "'')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                if (msGetStockGID == "E")
                {
                    values.status = true;
                    values.message = "Create sequence code ISKP for temp sales enquiry";
                }
                msSQL = " insert into ims_trn_tstock (" +
                      " stock_gid," +
                      " branch_gid," +
                      " product_gid," +
                      " uom_gid," +
                      " stock_qty," +
                      " grn_qty," +
                      " rejected_qty," +
                      " unit_price," +
                      " location_gid," +
                      " stocktype_gid," +
                      " reference_gid," +
                      " stock_flag," +
                      " remarks," +
                      " created_by," +
                      " created_date," +
                      " issued_qty," +
                      " amend_qty," +
                      " damaged_qty," +
                      " adjusted_qty," +
                      " display_field" +
                      " )values(" +
                      " '" + msGetStockGID + "'," +
                      " '" + values.branch_gid + "'," +
                      " '" + values.product_gid + "'," +
                      " '" + lsuom_gid + "'," +
                      " '" + values.transfer_stock + "'," +
                      " '0'," +
                      " '0'," +
                      "'" + values.unit_price + "'," +
                     " '" + values.location_gid + "'," +
                      " 'SY0905270012'," +
                      " '" + msGetStockGID + "'," +
                      " 'Y'," +
                      " 'From Transfer'," +
                      " '" + user_gid + "'," +
                      " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                      " '0'," +
                      " '0'," +
                      " '0'," +
                      " '0'," +
                      " 'From Transfer')";

                mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (lsuom_gid == "PPMM1502050002")
                {
                    msSQL = "UPDATE ims_trn_tstock SET " +
                            "transfer_qty = transfer_qty + " + values.transfer_stock + "," +
                            "remarks = 'On Transfer' " +
                            "WHERE stock_gid = '" + values.stock_gid + "' AND branch_gid = '" + branch_gid + "'";
                }
                else
                {
                    msSQL = "UPDATE ims_trn_tstock SET " +
                            "stock_qty = stock_qty - " + values.transfer_stock + "," +
                            "remarks = 'On Transfer' " +
                            "WHERE stock_gid = '" + values.stock_gid + "' AND branch_gid = '" + branch_gid + "'";
                }

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select stock_qty from ims_trn_tstock Where stock_gid = '" + values.stock_gid + "' and branch_gid='" + branch_gid + "'";
                string lsstock_qty = objdbconn.GetExecuteScalar(msSQL);
                if (lsstock_qty == "0.00" || lsstock_qty == "0")
                {
                    msSQL = "DELETE FROM ims_trn_tstock " +
                            "WHERE stock_gid = '" + values.stock_gid + "' AND branch_gid = '" + branch_gid + "'";
                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Stock Transferred Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Stock Transferring";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //Location Stock Transfer Submit


        public void DaPostLocationstocktransfer(string branch_gid, string user_gid, string employee_gid, Postlocationstocktransfer values)
        {
            try
            {
                msSQL = " select a.tmpstocktransfer_gid,a.product_gid,a.productuom_gid,a.stock_qty,c.productgroup_name,a.display_field, " +
                        " b.product_code,b.product_name from ims_tmp_tstocktransferdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on b.productuom_gid=d.productuom_gid " +
                        " where a.created_by='" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt1 in dt_datatable.Rows)
                {

                    msSQL = " select a.created_date,a.stock_gid,a.product_gid,a.display_field,a.uom_gid,a.reference_gid, " +
                       " sum(a.stock_qty + amend_qty - damaged_qty - a.issued_qty - transfer_qty) as stock_qty,b.product_name,c.productuom_name,b.product_code,d.productgroup_name " +
                       " from ims_trn_tstock a " +
                       " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                       " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid " +
                       " left join pmr_mst_tproductgroup d on d.productgroup_gid = b.productgroup_gid " +
                       " where a.product_gid = '" + dt1["product_gid"].ToString() + "' " +
                       " and a.uom_gid = '" + dt1["productuom_gid"].ToString() + "' and a.stock_flag = 'Y' " +
                       " and a.branch_gid = '" + branch_gid + "' and a.location_gid = '" + values.location_gid + "' " +
                       " group by stock_gid ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetPopsummary_list>();
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        double issuedQty = Convert.ToDouble(dt1["stock_qty"].ToString());
                        double stockQty = Convert.ToDouble(dt["stock_qty"].ToString());

                        if (issuedQty > stockQty)
                        {
                            values.message = "Quantity Transfer cannot be greater than Stock in hand ";
                            return;
                        }
                    }
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        double issueQTY = Convert.ToDouble(dt1["stock_qty"].ToString());
                        double stockQty = Convert.ToDouble(dt["stock_qty"].ToString());

                        if (issueQTY <= stockQty)
                        {

                            if (issueQTY == 0.00 || issueQTY == 0)
                            {
                            }
                            else
                            {
                                msSQL = " select stock_quantity from ims_tmp_tstock where created_by='" + employee_gid + "' and product_gid='" + dt["product_gid"].ToString() + "' " +
                               " and stock_gid='" + dt["stock_gid"].ToString() + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    msSQL = " UPDATE ims_trn_tstock SET " +
                                            " transfer_qty = (transfer_qty - " + objOdbcDataReader["stock_quantity"].ToString().Replace("-", "") + ") " +
                                            " WHERE stock_gid = '" + dt["stock_gid"].ToString() + "'";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    msSQL = " update ims_tmp_tstock set " +
                                    " stock_quantity= stock_quantity + '" + issueQTY + " '" +
                                    " where stock_gid='" + dt["stock_gid"].ToString() + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    objOdbcDataReader.Close();
                                }
                                else
                                {

                                    msSQL = " insert into ims_tmp_tstock(" +
                                     " stock_gid," +
                                     " product_gid," +
                                     " stock_quantity," +
                                     " created_by," +
                                     " created_date," +
                                     " branch_gid," +
                                     " location_gid," +
                                     " productuom_gid," +
                                     " display_field" +
                                     ") values (" +
                                    "'" + dt["stock_gid"].ToString() + "'," +
                                    "'" + dt["product_gid"].ToString() + "'," +
                                    "'" + issueQTY + "'," +
                                    "'" + employee_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    "'" + branch_gid + "'," +
                                    "'" + values.location_gid + "'," +
                                    "'" + dt1["productuom_gid"].ToString() + "'," +
                                    "'" + dt1["display_field"].ToString() + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                    }
                }
                msGetGID = objcmnfunctions.GetMasterGID("ISRP");
                if (msGetGID == "E")
                {
                    values.status = true;
                    values.message = "Create sequence code ISRP for temp sales enquiry";
                }

                msGetGID1 = objcmnfunctions.GetMasterGID("SOTR");
                msSQL = " insert into ims_trn_tstocktransfer (" +
                " stocktransfer_gid," +
                " product_gid," +
                " branchfrom_gid," +
                " locationfrom_gid, " +
                " locationto_gid, " +
                " si_no," +
                " transfered_by," +
                " transfered_date," +
                " created_by," +
                " created_date," +
                " stock_gid, " +
                " remarks ) " +
                " values ( " +
                "'" + msGetGID + "', " +
                "'" + values.product_gid + "'," +
                "'" + branch_gid + "'," +
                "'" + values.location_gid + "'," +
                "'" + values.locationto_gid + "'," +
                "'" + msGetGID1 + "'," +
                "'" + user_gid + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                "'" + user_gid + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                "'" + values.stock_gid + "'," +
                "'" + values.remarks + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL = " select * from ims_tmp_tstock where created_by='" + employee_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select * from ims_trn_tstock  " +
                           " where branch_gid='" + branch_gid + "'" +
                            " and location_gid='" + values.locationto_gid + "' and product_gid='" + dt["product_gid"].ToString() + "'" +
                            " and uom_gid='" + dt["productuom_gid"].ToString() + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        if (objOdbcDataReader.RecordsAffected > 1)
                        {
                            msSQL = " select * from ims_trn_tstock  " +
                            " where branch_gid='" + branch_gid + "'" +
                            " and location_gid='" + values.locationto_gid + "' and product_gid='" + dt["product_gid"].ToString() + "'" +
                            " and uom_gid='" + dt["productuom_gid"].ToString() + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            foreach (DataRow dt1 in dt_datatable.Rows)
                            {
                                msSQL = " update ims_trn_tstock set " +
                                " stock_qty= stock_qty + '" + dt["stock_quantity"].ToString() + " '" +
                                " where branch_gid='" + branch_gid + "'" +
                                " and location_gid='" + values.locationto_gid + "' and product_gid='" + dt["product_gid"].ToString() + "' and stock_gid='" + dt["stock_gid"].ToString() + "'" +
                                " and uom_gid='" + dt["productuom_gid"].ToString() + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    msSQL = " select transfer_qty from ims_trn_tstock " +
                                    " WHERE branch_gid = '" + branch_gid + "'" +
                                    " AND location_gid = '" + values.location_gid + "' " +
                                    " AND product_gid = '" + dt["product_gid"].ToString() + "' " +
                                    " AND uom_gid = '" + dt["productuom_gid"].ToString() + "'";
                                    string lstransfer_qty = objdbconn.GetExecuteScalar(msSQL);

                                    double lstransfer = double.Parse(lstransfer_qty) - double.Parse(dt["stock_quantity"].ToString());



                                    lsstock_quantity = Math.Abs(lstransfer).ToString();

                                    msSQL = " UPDATE ims_trn_tstock SET " +
                                            " transfer_qty = '" + lsstock_quantity + "'" +
                                            " WHERE branch_gid = '" + branch_gid + "'" +
                                            " AND location_gid = '" + values.location_gid + "' " +
                                            " AND product_gid = '" + dt["product_gid"].ToString() + "' " +
                                            " AND stock_gid = '" + dt["stock_gid"].ToString() + "' " +
                                            " AND uom_gid = '" + dt["productuom_gid"].ToString() + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                        else
                        {
                            msSQL = " update ims_trn_tstock set " +
                            " stock_qty= stock_qty + '" + dt["stock_quantity"].ToString() + " '" +
                            " where branch_gid='" + branch_gid + "'" +
                            " and location_gid='" + values.locationto_gid + "' and product_gid='" + dt["product_gid"].ToString() + "'" +
                            " and uom_gid='" + dt["productuom_gid"].ToString() + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = " select transfer_qty from ims_trn_tstock " +
                                    " WHERE branch_gid = '" + branch_gid + "'" +
                                    " AND location_gid = '" + values.location_gid + "' " +
                                    " AND product_gid = '" + dt["product_gid"].ToString() + "' " +
                                    " AND uom_gid = '" + dt["productuom_gid"].ToString() + "'";
                                string lstransfer_qty = objdbconn.GetExecuteScalar(msSQL);

                                double lstransfer = double.Parse(lstransfer_qty) - double.Parse(dt["stock_quantity"].ToString());



                                lsstock_quantity = Math.Abs(lstransfer).ToString();

                                msSQL = " UPDATE ims_trn_tstock SET " +
                                        " transfer_qty = '" + lsstock_quantity + "'" +
                                        " WHERE branch_gid = '" + branch_gid + "'" +
                                        " AND location_gid = '" + values.location_gid + "' " +
                                        " AND product_gid = '" + dt["product_gid"].ToString() + "' " +
                                        " AND stock_gid = '" + dt["stock_gid"].ToString() + "' " +
                                        " AND uom_gid = '" + dt["productuom_gid"].ToString() + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                        objOdbcDataReader.Close();
                    }
                    else
                    {

                        msGetStockGID = objcmnfunctions.GetMasterGID("ISTP");
                        msSQL = " insert into ims_trn_tstock (" +
                                  " stock_gid," +
                                  " branch_gid," +
                                  " location_gid," +
                                  " product_gid," +
                                  " uom_gid," +
                                  " stock_qty," +
                                  " grn_qty," +
                                  " rejected_qty," +
                                  " unit_price," +
                                  " stocktype_gid," +
                                  " reference_gid," +
                                  " stock_flag," +
                                  " remarks," +
                                  " created_by," +
                                  " created_date," +
                                  " issued_qty," +
                                  " amend_qty," +
                                  " damaged_qty," +
                                  " adjusted_qty," +
                                  " display_field" +
                                  " )values(" +
                                  " '" + msGetStockGID + "'," +
                                  " '" + branch_gid + "'," +
                                  " '" + values.locationto_gid + "'," +
                                  " '" + dt["product_gid"].ToString() + "'," +
                                  " '" + dt["productuom_gid"].ToString() + "'," +
                                  " '" + dt["stock_quantity"].ToString() + "'," +
                                  " '0'," +
                                  " '0'," +
                                  " '0.00'," +
                                  " 'SY0905270012'," +
                                  " '" + msGetStockGID + "'," +
                                  " 'Y'," +
                                  " 'From Transfer'," +
                                  " '" + user_gid + "'," +
                                  " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                  " '0'," +
                                  " '0'," +
                                  " '0'," +
                                  " '0'," +
                                  " '" + dt["display_field"].ToString() + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {


                            msSQL = " select transfer_qty from ims_trn_tstock " +
                                    " WHERE branch_gid = '" + branch_gid + "'" +
                                    " AND location_gid = '" + values.location_gid + "' " +
                                    " AND product_gid = '" + dt["product_gid"].ToString() + "' " +
                                    " AND uom_gid = '" + dt["productuom_gid"].ToString() + "'";
                            string lstransfer_qty = objdbconn.GetExecuteScalar(msSQL);

                            double lstransfer = double.Parse(lstransfer_qty) - double.Parse(dt["stock_quantity"].ToString());



                            lsstock_quantity = Math.Abs(lstransfer).ToString();

                            msSQL = " UPDATE ims_trn_tstock SET " +
                                    " transfer_qty = '" + lsstock_quantity + "'" +
                                    " WHERE branch_gid = '" + branch_gid + "'" +
                                    " AND location_gid = '" + values.location_gid + "' " +
                                    " AND product_gid = '" + dt["product_gid"].ToString() + "' " +
                                    " AND stock_gid = '" + dt["stock_gid"].ToString() + "' " +
                                    " AND uom_gid = '" + dt["productuom_gid"].ToString() + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                    }
                }

                msSQL = " select a.tmpstocktransfer_gid,a.product_gid,a.productuom_gid,a.stock_qty,c.productgroup_name,a.display_field, " +
                        " b.product_code,b.product_name from ims_tmp_tstocktransferdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on b.productuom_gid=d.productuom_gid " +
                        " where a.created_by='" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt1 in dt_datatable.Rows)
                {

                    msstocktransferDTL = objcmnfunctions.GetMasterGID("STDT");
                    if (msstocktransferDTL == "E")
                    {
                        values.status = true;
                        values.message = "Create Sequence Code ISRP for Stock Transfer Table";
                    }
                    msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                    if (msGetStockGID == "E")
                    {
                        values.status = true;
                        values.message = "Create a sequence Code ISKP for Stock Tabl";
                    }
                    msSQL = " insert into ims_trn_tstocktransferdtl ( " +
                            " stocktransferdtl_gid," +
                            " stocktransfer_gid," +
                            " stock_qty," +
                            " product_gid," +
                            " productuom_gid," +
                            " stock_gid, " +
                            " display_field, " +
                            " created_date, " +
                            " created_by)" +
                            " values(" +
                            "'" + msstocktransferDTL + "'," +
                            "'" + msGetGID + "'," +
                            "'" + dt1["stock_qty"].ToString() + "'," +
                            "'" + dt1["product_gid"].ToString() + "'," +
                            "'" + dt1["productuom_gid"].ToString() + "'," +
                            "'" + msGetStockGID + "'," +
                            "'" + dt1["display_field"].ToString() + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = " select max(unit_price) as unit_price from ims_trn_tstock " +
                        " where product_gid='" + dt1["product_gid"].ToString() + "' and uom_gid='" + dt1["productuom_gid"].ToString() + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            string lsunitprice = objOdbcDataReader["unit_price"].ToString();
                            objOdbcDataReader.Close();
                        }
                        if (mnResult == 1)
                        {
                            msSQL = " SELECT serial_gid FROM ims_tmp_tstocktrnasferserials WHERE " +
                                    " product_gid = '" + dt1["product_gid"].ToString() + "' AND unit_gid = '" + dt1["productuom_gid"].ToString() + "' " +
                                    " AND created_by = '" + employee_gid + "'";

                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows)
                            {
                                while (objOdbcDataReader.Read())
                                {
                                    msSQL = "UPDATE pmr_trn_tserials SET issued_flag = 'Y', issued_date = '" + objOdbcDataReader["SelectedDate"].ToString() + "'," +
                                            "reference_gid = '" + msGetGID + "', referencedtl_gid = '" + msstocktransferDTL + "', issued_from = 'Stock Transfer' " +
                                            "WHERE serial_gid = '" + objOdbcDataReader["serial_gid"].ToString() + "'";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    objOdbcDataReader.Close();
                                }
                                objOdbcDataReader.Close();
                            }
                        }

                    }


                }
                msSQL = " select * from ims_tmp_tstock where created_by='" + employee_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msstockdtlGid = objcmnfunctions.GetMasterGID("ISTP");
                    msSQL = " insert into ims_trn_tstockdtl(" +
                          " stockdtl_gid," +
                          " stock_gid," +
                          " branch_gid," +
                          " product_gid," +
                          " uom_gid," +
                          " issued_qty," +
                          " amend_qty," +
                          " damaged_qty," +
                          " adjusted_qty," +
                          " transfer_qty," +
                          " return_qty," +
                          " reference_gid," +
                          " stock_type," +
                          " remarks," +
                          " created_by," +
                          " created_date," +
                          " display_field" +
                          " ) values ( " +
                          "'" + msstockdtlGid + "'," +
                          "'" + dt["stock_gid"].ToString() + "'," +
                          "'" + branch_gid + "'," +
                          "'" + dt["product_gid"].ToString() + "'," +
                          "'" + dt["productuom_gid"].ToString() + "'," +
                          "'0.00'," +
                          "'0.00'," +
                          "'0.00'," +
                          "'0.00'," +
                          "'" + dt["stock_quantity"].ToString() + "'," +
                          "'0.00'," +
                          "'" + msstocktransferDTL + "'," +
                          "'Transfer'," +
                          "'" + values.req_remarks + "'," +
                          "'" + employee_gid + "'," +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                          "'')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occured While Inserting Records";
                    }
                }


                msSQL = " select count(*) as serial_count,stock_gid, serial_no, batch_no, remarks, product_gid, " +
                        " unit_gid, display_field from ims_tmp_tstocktrnasferserials " +
                        " where created_by='" + employee_gid + "' group by stock_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt in dt_datatable.Rows)
                {

                    msstockdtlGid = objcmnfunctions.GetMasterGID("ISTP");
                    msSQL = " insert into ims_trn_tstockdtl(" +
                  " stockdtl_gid," +
                  " stock_gid," +
                  " branch_gid," +
                  " product_gid," +
                  " uom_gid," +
                  " issued_qty," +
                  " amend_qty," +
                  " damaged_qty," +
                  " adjusted_qty," +
                  " transfer_qty," +
                  " return_qty," +
                  " reference_gid," +
                  " stock_type," +
                  " remarks," +
                  " created_by," +
                  " created_date," +
                  " display_field" +
                  " ) values ( " +
                  "'" + msstockdtlGid + "'," +
                  "'" + values.stock_gid + "'," +
                  "'" + branch_gid + "'," +
                  "'" + dt["product_gid"].ToString() + "'," +
                  "'" + dt["productuom_gid"].ToString() + "'," +
                  "'0.00'," +
                  "'0.00'," +
                  "'0.00'," +
                  "'0.00'," +
                  "'" + dt["serial_count"] + "'," +
                  "'0.00'," +
                  "'" + msstocktransferDTL + "'," +
                  "'Transfer'," +
                  "'" + values.req_remarks + "'," +
                  "'" + employee_gid + "'," +
                  "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                  "'')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occured While Inserting Records";
                    }
                }
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " delete from ims_tmp_tstocktrnasferserials " +
                        " where  created_by = '" + employee_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error Occured While Deleting Records";
                }
                msSQL = " delete from ims_tmp_tstock " +
                        " where  created_by = '" + employee_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error Occured While Deleting Records";
                }
                msSQL = " delete from ims_tmp_tstocktransferdtl " +
                " where created_by='" + user_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error Occured While Product Deleting Records";
                }
                else
                {
                    values.status = true;
                    values.message = "Stock Transfered successfully";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetImsRptStocktransferapproval(MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.stocktransfer_gid,  a.transfered_by, a.status, " +
                        " CASE WHEN b.location_name IS NOT NULL THEN b.location_name ELSE d.branch_name END AS branchfrom_name, " +
                        " CASE WHEN c.location_name IS NOT NULL THEN c.location_name ELSE e.branch_name END AS branchto_name, " +
                        " date_format(a.transfered_date, '%d-%m-%Y') as transfer_date, a.product_gid, a.remarks, a.si_no, a.stock_gid, f.user_firstname " +
                        " from ims_trn_tstocktransfer a " +
                        " left join ims_mst_tlocation b on a.locationfrom_gid = b.location_gid " +
                        " left join ims_mst_tlocation c on a.locationto_gid = c.location_gid " +
                        " left join hrm_mst_tbranch d on a.branchfrom_gid = d.branch_gid " +
                        " left join hrm_mst_tbranch e on a.branchto_gid = e.branch_gid " +
                        " left join adm_mst_tuser f on f.user_gid = a.transfered_by " +
                        " order by date(a.transfered_date) desc,a.transfered_date asc, a.stocktransfer_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferapproval_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferapproval_list
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            status = dt["status"].ToString(),
                            branchfrom_name = dt["branchfrom_name"].ToString(),
                            branchto_name = dt["branchto_name"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                        });
                        values.stocktransferapproval_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetImsRptStocktransferapprovalview(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select stocktransfer_gid, transfered_by, date_format(transfered_date, '%d-%m-%Y') as transfer_date,a.status," +
                        " CASE WHEN b.location_name IS NOT NULL THEN b.location_name ELSE d.branch_name END AS branchfrom_name," +
                        " CASE WHEN c.location_name IS NOT NULL THEN c.location_name ELSE e.branch_name END AS branchto_name," +
                        " product_gid, remarks, si_no, stock_gid " +
                        " from ims_trn_tstocktransfer a " +
                        " left join ims_mst_tlocation b on a.locationfrom_gid = b.location_gid " +
                        " left join ims_mst_tlocation c on a.locationto_gid = c.location_gid " +
                        " left join hrm_mst_tbranch d on a.branchfrom_gid = d.branch_gid " +
                        " left join hrm_mst_tbranch e on a.branchto_gid = e.branch_gid " +
                        " where stocktransfer_gid = '" + stocktransfer_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferapproval_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferapproval_list
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            branchfrom_name = dt["branchfrom_name"].ToString(),
                            branchto_name = dt["branchto_name"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            status = dt["status"].ToString(),
                        });
                        values.stocktransferapproval_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetStocktransferProductView(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.stocktransferdtl_gid, a.stock_qty,c.productgroup_name,a.product_gid,a.display_field, " +
                        " b.product_code,b.product_name,d.productuom_name from ims_trn_tstocktransferdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid " +
                        " where stocktransfer_gid='" + stocktransfer_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferlocationproductview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferlocationproductview
                        {
                            stocktransferdtl_gid = dt["stocktransferdtl_gid"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                        });
                        values.stocktransferlocationproductview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaStockTransferApproval(string stocktransfer_gid, string user_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " Update ims_trn_tstocktransfer set " +
                        " status='Stock Transfer Approved'," +
                        " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " approved_by = '" + user_gid + "' " +
                        " where stocktransfer_gid='" + stocktransfer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " Stock transfer Approved successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while approve stock transfer.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Approval !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaStockTransferReject(string stocktransfer_gid, string user_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " Update ims_trn_tstocktransfer set " +
                        " status='Stock Transfer Rejected'," +
                        " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " approved_by = '" + user_gid + "' " +
                        " where stocktransfer_gid='" + stocktransfer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Stock Transfer Rejected.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while Reject Stock Transfer.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Reject !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetImsRptStocktransferacknowlege(string branch_gid,MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.stocktransfer_gid,  a.transfered_by, a.status, " +
                        " CASE WHEN b.location_name IS NOT NULL THEN b.location_name ELSE d.branch_name END AS branchfrom_name, " +
                        " CASE WHEN c.location_name IS NOT NULL THEN c.location_name ELSE e.branch_name END AS branchto_name, " +
                        " date_format(a.transfered_date, '%d-%m-%Y') as transfer_date, a.product_gid, a.remarks, a.si_no, a.stock_gid, f.user_firstname " +
                        " from ims_trn_tstocktransfer a " +
                        " left join ims_mst_tlocation b on a.locationfrom_gid = b.location_gid " +
                        " left join ims_mst_tlocation c on a.locationto_gid = c.location_gid " +
                        " left join hrm_mst_tbranch d on a.branchfrom_gid = d.branch_gid " +
                        " left join hrm_mst_tbranch e on a.branchto_gid = e.branch_gid " +
                        " left join adm_mst_tuser f on f.user_gid = a.transfered_by " +
                        " where a.branchto_gid is not null and a.branchto_gid='" + branch_gid + "' " +
                        " order by date(a.transfered_date) desc,a.transfered_date asc, a.stocktransfer_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferapproval_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferapproval_list
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            status = dt["status"].ToString(),
                            branchfrom_name = dt["branchfrom_name"].ToString(),
                            branchto_name = dt["branchto_name"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                        });
                        values.stocktransferapproval_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Acknowlegement !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetImsRptStocktransferacknowlegelocation(string branch_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
               msSQL = " select locationto_gid from  ims_trn_tstocktransfer where branchfrom_gid='" + branch_gid + "' AND locationto_gid IS NOT NULL ";
               lslocationto_gid=objdbconn.GetExecuteScalar(msSQL);
                
                msSQL = " select a.stocktransfer_gid,  a.transfered_by, a.status, " +
                        " CASE WHEN b.location_name IS NOT NULL THEN b.location_name ELSE d.branch_name END AS branchfrom_name, " +
                        " CASE WHEN c.location_name IS NOT NULL THEN c.location_name ELSE e.branch_name END AS branchto_name, " +
                        " date_format(a.transfered_date, '%d-%m-%Y') as transfer_date, a.product_gid, a.remarks, a.si_no, a.stock_gid, f.user_firstname " +
                        " from ims_trn_tstocktransfer a " +
                        " left join ims_mst_tlocation b on a.locationfrom_gid = b.location_gid " +
                        " left join ims_mst_tlocation c on a.locationto_gid = c.location_gid " +
                        " left join hrm_mst_tbranch d on a.branchfrom_gid = d.branch_gid " +
                        " left join hrm_mst_tbranch e on a.branchto_gid = e.branch_gid " +
                        " left join adm_mst_tuser f on f.user_gid = a.transfered_by " +
                        " where a.locationto_gid is not null and a.locationto_gid='" + lslocationto_gid + "' " +
                        " order by date(a.transfered_date) desc,a.transfered_date asc, a.stocktransfer_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferapproval_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferapproval_list
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            status = dt["status"].ToString(),
                            branchfrom_name = dt["branchfrom_name"].ToString(),
                            branchto_name = dt["branchto_name"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                        });
                        values.stocktransferapproval_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Acknowlegement !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // from date



        public void DaGetStocktransferAckdate(string ref_no,string from_date, string to_date, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                if ((from_date == null) || (to_date == null))
                {
                    lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                    lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    //-- from date
                    DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");

                    //-- to date
                    DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");
                }


                msSQL = " select a.stocktransfer_gid,  a.transfered_by, a.status, " +
                        " CASE WHEN b.location_name IS NOT NULL THEN b.location_name ELSE d.branch_name END AS branchfrom_name, " +
                        " CASE WHEN c.location_name IS NOT NULL THEN c.location_name ELSE e.branch_name END AS branchto_name, " +
                        " date_format(a.transfered_date, '%d-%m-%Y') as transfer_date, a.product_gid, a.remarks, a.si_no, a.stock_gid, f.user_firstname " +
                        " from ims_trn_tstocktransfer a " +
                        " left join ims_mst_tlocation b on a.locationfrom_gid = b.location_gid " +
                        " left join ims_mst_tlocation c on a.locationto_gid = c.location_gid " +
                        " left join hrm_mst_tbranch d on a.branchfrom_gid = d.branch_gid " +
                        " left join hrm_mst_tbranch e on a.branchto_gid = e.branch_gid " +
                        " left join adm_mst_tuser f on f.user_gid = a.transfered_by " +
                        " where  a.transfered_date >= '" + lsstart_date + "' and a.transfered_date <= '" + lsend_date + "' and a.stocktransfer_gid = '" + ref_no + "' " +
                        " order by date(a.transfered_date) desc,a.transfered_date asc, a.stocktransfer_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferapproval_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferapproval_list
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            status = dt["status"].ToString(),
                            branchfrom_name = dt["branchfrom_name"].ToString(),
                            branchto_name = dt["branchto_name"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                        });
                        values.stocktransferapproval_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //Acknowledgemet

        public void DaGetImsRptStocktransferackview(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select stocktransfer_gid, transfered_by, date_format(transfered_date, '%d-%m-%Y') as transfer_date,a.status," +
                        " CASE WHEN b.location_name IS NOT NULL THEN b.location_name ELSE d.branch_name END AS branchfrom_name," +
                        " CASE WHEN c.location_name IS NOT NULL THEN c.location_name ELSE e.branch_name END AS branchto_name," +
                        " product_gid, remarks, si_no, stock_gid " +
                        " from ims_trn_tstocktransfer a " +
                        " left join ims_mst_tlocation b on a.locationfrom_gid = b.location_gid " +
                        " left join ims_mst_tlocation c on a.locationto_gid = c.location_gid " +
                        " left join hrm_mst_tbranch d on a.branchfrom_gid = d.branch_gid " +
                        " left join hrm_mst_tbranch e on a.branchto_gid = e.branch_gid " +
                        " where stocktransfer_gid = '" + stocktransfer_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferapproval_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferapproval_list
                        {
                            stocktransfer_gid = dt["stocktransfer_gid"].ToString(),
                            branchfrom_name = dt["branchfrom_name"].ToString(),
                            branchto_name = dt["branchto_name"].ToString(),
                            transfer_date = dt["transfer_date"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            si_no = dt["si_no"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            status = dt["status"].ToString(),
                        });
                        values.stocktransferapproval_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetStocktransferackProductView(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select a.stocktransferdtl_gid, a.stock_qty,c.productgroup_name,a.product_gid,a.display_field, " +
                        " b.product_code,b.product_name,d.productuom_name from ims_trn_tstocktransferdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid " +
                        " where stocktransfer_gid='" + stocktransfer_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferlocationproductview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferlocationproductview
                        {
                            stocktransferdtl_gid = dt["stocktransferdtl_gid"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                        });
                        values.stocktransferlocationproductview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock transfer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaStockTransferAckApproval(string stocktransfer_gid, string user_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " Update ims_trn_tstocktransfer set " +
                        " status='Stock Acknowledged'," +
                        " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " approved_by = '" + user_gid + "' " +
                        " where stocktransfer_gid='" + stocktransfer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " Stock transfer Acknowledgement successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while approve stock transfer.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Approval !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaStockTransferAckReject(string stocktransfer_gid, string user_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " Update ims_trn_tstocktransfer set " +
                        " status='Stock Transfer Rejected'," +
                        " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " approved_by = '" + user_gid + "' " +
                        " where stocktransfer_gid='" + stocktransfer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Stock Transfer Acknowledgement Rejected.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while Reject Stock Transfer.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Reject !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetImsRptStocktransferreport( MdlImsTrnStockTransferSummary values)
        {
            try
            {
                msSQL = " select d.product_gid,x.stock_qty as Transfer_qty, d.product_code,d.product_name,  b.productgroup_name, c.productuom_name, " +
                " date_format(a.transfered_date, '%d-%m-%Y') as transfered_date,a.branchto_gid,a.stock_quantity,concat(i.user_firstname,' ',i.user_lastname) as transfered_by, " +
                " CASE WHEN j.branch_name IS NOT NULL THEN j.branch_name ELSE y.location_name END AS branchfrom_name, " +
                " CASE WHEN h.branch_name IS NOT NULL THEN h.branch_name ELSE z.location_name END AS branchto_name, " +
                " j.branch_name from ims_trn_tstocktransfer a " +
                " left join hrm_mst_tbranch h on a.branchto_gid= h.branch_gid " +
                " left join hrm_mst_tbranch j on a.branchfrom_gid= j.branch_gid " +
                " left join ims_trn_tstock m on m.reference_gid= a.stocktransfer_gid " +
                " left join ims_trn_tstockdtl n on n.stock_gid= m.stock_gid " +
                " left join pmr_mst_tproduct d on d.product_gid = a.product_gid " +
                " left join  ims_trn_tstocktransferdtl x on x.stocktransfer_gid=a.stocktransfer_gid " +
                " left join pmr_mst_tproductgroup b on d.productgroup_gid = b.productgroup_gid " +
                " left join pmr_mst_tproductuom c on d.productuom_gid = c.productuom_gid " +
                " left join adm_mst_tuser i on a.transfered_by = i.user_gid " +
                " left join ims_mst_tlocation y on y.location_gid = a.locationfrom_gid " +
                " left join ims_mst_tlocation z on z.location_gid = a.locationto_gid " +
                " where a.status='Stock Transfer Approved' group by a.stocktransfer_gid order by date(a.transfered_date) desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocktransferreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocktransferreport_list
                        {
                            Transfer_qty = dt["Transfer_qty"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            transfered_date = dt["transfered_date"].ToString(),
                            branchto_gid = dt["branchto_gid"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            transfered_by = dt["transfered_by"].ToString(),
                            branchfrom_name = dt["branchfrom_name"].ToString(),
                            branchto_name = dt["branchto_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.stocktransferreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Transfer Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetDetialViewProduct(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {

                msSQL = " select a.stocktransferdtl_gid, a.stock_qty,c.productgroup_name,b.product_code,a.product_gid, " +
                         " b.product_code,b.product_name,d.productuom_name from ims_trn_tstocktransferdtl a " +
                         " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                         " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                         " left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid " +
                          " where stocktransfer_gid='" + stocktransfer_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Stockproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Stockproduct_list
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),

                        });
                        values.Stockproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue Request Data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetDetialProduct(string stocktransfer_gid, MdlImsTrnStockTransferSummary values)
        {
            try
            {

                msSQL = " select a.stocktransferdtl_gid, a.stock_qty,c.productgroup_name,b.product_code,a.product_gid, " +
                         " b.product_code,b.product_name,d.productuom_name from ims_trn_tstocktransferdtl a " +
                         " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                         " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                         " left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid " +
                          " where stocktransfer_gid='" + stocktransfer_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Stockdeatails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Stockdeatails_list
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),

                        });
                        values.Stockdeatails_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue Request Data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}
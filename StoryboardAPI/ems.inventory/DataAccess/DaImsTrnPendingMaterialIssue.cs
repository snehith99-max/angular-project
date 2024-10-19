using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using System.Windows.Media.Media3D;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using CrystalDecisions.Shared.Json;
using System.Web.Services.Description;
using System.Globalization;

namespace ems.inventory.DataAccess
{
    public class DaImsTrnPendingMaterialIssue
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsstart_date, lsend_date, lsbranch_gid;
        string msGetGID,msRQGID, lsprstatus;
        int i,mnResult, mnCtr;
        string lsproductname, lsproductcode, lsproductuomname, lsproductgroupname, lsproductgroupgid, lsproductPrice, lspr_newproductstatus, employee;
        string lsqty_requested;
        public void DaGetPendingMaterialIssueSummary(string user_gid, MdlImsTrnPendingMaterialIssue values)
        {
            try
            {
                msSQL = " SELECT branch_gid FROM hrm_mst_temployee where user_gid = '" + user_gid + "'";
                lsbranch_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "  select /*+ MAX_EXECUTION_TIME(300000) */ distinct h.materialrequisition_gid,h.materialrequisition_reference, " +
                        "  (case when h.materialrequisition_status='ApproveToIssue' then 'Approved To Issue' else h.materialrequisition_status end) as materialrequisition_status,  " +
                        "  case when h.material_status='PR Not Raised' then 'PI Not Raised' when h.material_status='PR Raised' then 'PI Raised'  else h.material_status end as material_status," +
                        "  date_format(h.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date, b.user_firstname,g.costcenter_name," +
                        "  d.department_name, date_format(h.created_date,'%d-%m-%Y') as created_date,   date_format(h.expected_date,'%d-%m-%Y') as  expected_date,e.branch_prefix" +
                        "  from ims_trn_tmaterialrequisition h   " +
                        "  left join pmr_mst_tcostcenter g on h.costcenter_gid=g.costcenter_gid   " +
                        "  left join adm_mst_tuser b on h.user_gid = b.user_gid   " +
                        "  left join hrm_mst_temployee c on c.user_gid = b.user_gid   " +
                        "  left join hrm_mst_tdepartment d on c.department_gid = d.department_gid  " +
                        "  left join hrm_mst_tbranch e on e.branch_gid=h.branch_gid where 1=1 and h.branch_gid = '"+ lsbranch_gid + "' " +
                        "  and h.materialrequisition_status ='Approved' Order by date(h.materialrequisition_date)desc,(h.materialrequisition_date)asc,h.materialrequisition_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<pendingmaterialissue_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new pendingmaterialissue_list
                        {
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                            materialrequisition_status = dt["materialrequisition_status"].ToString(),
                            material_status = dt["material_status"].ToString(),
                            materialrequisition_date = dt["materialrequisition_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.pendingmaterialissue_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while error!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetRaiseMaterialIndent(string materialrequisition_gid, MdlImsTrnPendingMaterialIssue values)
        {
            try
            { 
  
                    msSQL = " select  a.materialrequisition_gid, " +
                            "  date_format(a.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date,format(a.provisional_amount, 2) as provisional_amount, " +
                            " a.materialrequisition_type,replace(a.materialrequisition_remarks,'<br/>','')as materialrequisition_remarks,a.materialrequisition_reference,e.branch_name,a.branch_gid, " +
                            " concat(b.user_firstname,' - ',b.user_lastname) as user_firstname,d.department_name,date_format(g.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date,g.priority_remarks,g.priority,  " +
                            " a.costcenter_gid,format(f.budget_available, 2) as budget_available,concat_ws('-',f.costcenter_name,f.costcenter_gid) as costcenter_name " +
                            " from ims_trn_tmaterialrequisition a " +
                            " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                            " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                            " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                            " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid " +
                            " left join pmr_mst_tcostcenter f on f.costcenter_gid = a.costcenter_gid " +
                            " left join pmr_trn_tpurchaserequisition g on g.materialrequisition_gid = a.materialrequisition_gid   " +
                            " where a.materialrequisition_gid = '" + materialrequisition_gid + "' group by materialrequisition_gid";


                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<raisematerialindent_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new raisematerialindent_list
                            {
                                materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                                materialrequisition_date = dt["materialrequisition_date"].ToString(),
                                provisional_amount = dt["provisional_amount"].ToString(),
                                materialrequisition_type = dt["materialrequisition_type"].ToString(),
                                materialrequisition_remarks = dt["materialrequisition_remarks"].ToString(),
                                branch_name = dt["branch_name"].ToString(),
                                branch_gid = dt["branch_gid"].ToString(),
                                materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                                user_firstname = dt["user_firstname"].ToString(),
                                costcenter_name = dt["costcenter_name"].ToString(),
                                department_name = dt["department_name"].ToString(),
                                expected_date = dt["purchaserequisition_date"].ToString(),
                                priority_remarks = dt["priority_remarks"].ToString(),
                                priority = dt["priority"].ToString(),
                                
                            });
                            values.raisematerialindent_list = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                    values.message = "Exception occured while error!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaGetRaiseMaterialIndentProduct(string branch_gid, string materialrequisition_gid, MdlImsTrnPendingMaterialIssue values)
        {
            try
            {
                
                msSQL = "SELECT P.product_gid, P.product_code, P.product_name, MR_DTL.qty_requested AS req_qty, " +
                        "MR.branch_gid, MR_DTL.materialrequisition_gid, MR_DTL.productuom_gid, c.productuom_name, " +
                        "MR_DTL.display_field, MR_DTL.materialrequisitiondtl_gid, e.productgroup_name " +
                        "FROM ims_trn_tmaterialrequisitiondtl MR_DTL " +
                        "LEFT JOIN ims_trn_tmaterialrequisition MR ON MR.materialrequisition_gid = MR_DTL.materialrequisition_gid " +
                        "LEFT JOIN pmr_mst_tproduct P ON P.product_gid = MR_DTL.product_gid " +
                        "LEFT JOIN pmr_mst_tproductuom c ON c.productuom_gid = MR_DTL.productuom_gid " +
                        "LEFT JOIN pmr_mst_tproductgroup e ON e.productgroup_gid = P.productgroup_gid " +
                        "WHERE MR.branch_gid = '" + branch_gid + "' AND MR.materialrequisition_gid = '" + materialrequisition_gid + "' " +
                        "AND (MR_DTL.qty_requested - MR_DTL.qty_issued) > 0 " +
                        "GROUP BY MR_DTL.product_gid, MR_DTL.productuom_gid, MR_DTL.display_field";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialindentproduct_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string product_gid = dt["product_gid"].ToString();
                        string productuom_gid = dt["productuom_gid"].ToString();

                       
                        msSQL = "SELECT SUM(qty_issued) AS qty_issued FROM ims_trn_tmaterialrequisitiondtl a " +
                                "LEFT JOIN ims_trn_tmaterialrequisition b ON a.materialrequisition_gid = b.materialrequisition_gid " +
                                "WHERE a.product_gid = '" + product_gid + "' AND a.productuom_gid = '" + productuom_gid + "' " +
                                "AND a.materialrequisition_gid = '" + materialrequisition_gid + "' " +
                                "AND b.branch_gid = '" + branch_gid + "' GROUP BY a.product_gid, a.productuom_gid";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        string qtyIssued = "0";
                        if (objOdbcDataReader.HasRows)
                        {
                            objOdbcDataReader.Read();
                            qtyIssued = objOdbcDataReader["qty_issued"].ToString();
                        }
                        objOdbcDataReader.Close();

                        
                        msSQL = "SELECT SUM((a.qty_requested - a.qty_issued)) AS pending_mr FROM ims_trn_tmaterialrequisitiondtl a " +
                                "LEFT JOIN ims_trn_tmaterialrequisition b ON a.materialrequisition_gid = b.materialrequisition_gid " +
                                "WHERE a.product_gid = '" + product_gid + "' AND a.productuom_gid = '" + productuom_gid + "' " +
                                "AND b.branch_gid = '" + branch_gid + "' GROUP BY a.product_gid, a.productuom_gid";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        string pendingMr = "0";
                        if (objOdbcDataReader.HasRows)
                        {
                            objOdbcDataReader.Read();
                            pendingMr = objOdbcDataReader["pending_mr"].ToString();
                        }
                        objOdbcDataReader.Close();

                       
                        msSQL = "SELECT SUM((qty_requested - qty_received)) AS pending_pr FROM pmr_trn_tpurchaserequisitiondtl a " +
                                "LEFT JOIN pmr_trn_tpurchaserequisition b ON a.purchaserequisition_gid = b.purchaserequisition_gid " +
                                "WHERE a.product_gid = '" + product_gid + "' AND a.uom_gid = '" + productuom_gid + "' " +
                                "AND b.branch_gid = '" + branch_gid + "' AND b.purchaserequisition_flag <> 'PR Rejected' " +
                                "AND b.purchaseorder_flag <> 'PO Raised' GROUP BY a.product_gid, a.uom_gid";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        string pendingPr = "0";
                        if (objOdbcDataReader.HasRows)
                        {
                            objOdbcDataReader.Read();
                            pendingPr = objOdbcDataReader["pending_pr"].ToString();
                        }
                        objOdbcDataReader.Close();

                        
                        msSQL = "SELECT SUM(a.stock_qty + amend_qty - a.damaged_qty - a.issued_qty - transfer_qty) AS stock_quantity FROM ims_trn_tstock a " +
                                "WHERE a.product_gid = '" + product_gid + "' AND a.uom_gid = '" + productuom_gid + "' " +
                                "AND a.branch_gid = '" + branch_gid + "' AND a.stock_flag = 'Y' GROUP BY a.product_gid, a.uom_gid";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        string stockQuantity = "0";
                        if (objOdbcDataReader.HasRows)
                        {
                            objOdbcDataReader.Read();
                            stockQuantity = objOdbcDataReader["stock_quantity"].ToString();
                        }
                        objOdbcDataReader.Close();



                        
                        decimal stockQty = Convert.ToDecimal(stockQuantity);
                        decimal pendingPrVal = Convert.ToDecimal(pendingPr);
                        decimal pendingMrVal = Convert.ToDecimal(pendingMr);

                        decimal netAvailable = stockQty + pendingPrVal - pendingMrVal;

                        
                        string lsnet_available = netAvailable.ToString();


                        
                        getModuleList.Add(new materialindentproduct_list
                        {
                            product_gid = product_gid,
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            req_qty = dt["req_qty"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            productuom_gid = productuom_gid,
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            materialrequisitiondtl_gid = dt["materialrequisitiondtl_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            qty_issued = qtyIssued,
                            pending_mr = pendingMr,
                            pending_pr = pendingPr,
                            stock_quantity = stockQuantity,
                            net_avalible= lsnet_available
                        });

                        values.materialindentproduct_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred!";
                objcmnfunctions.LogForAudit($"Error: {ex.Message}");
            }
        }

        public void DaGetDetailPopup(string materialrequisition_gid, MdlImsTrnPendingMaterialIssue values)
        {
            try
            {
                msSQL = " select /*+ MAX_EXECUTION_TIME(300000) */ f.productgroup_name,c.product_code,c.product_name,a.qty_issued,e.branch_gid,d.productuom_name,a.qty_requested, " +
                    " (select ifnull(sum(b.stock_qty+amend_qty-b.damaged_qty-b.issued_qty-transfer_qty),0) as stockqty from ims_trn_tstock b where b.product_gid=a.product_gid" +
                    " and b.uom_gid=a.productuom_gid and b.stock_flag='Y' and b.branch_gid=e.branch_gid) as stock," +
                    " case when a.issuerequestqty<>'0.00'  then a.issuerequestqty else a.qty_issued end as issuerequestqty" +
                    " from ims_trn_tmaterialrequisitiondtl a" +
                    " left join ims_trn_tmaterialrequisition e on e.materialrequisition_gid=a.materialrequisition_gid" +
                    " left join pmr_mst_tproduct c on c.product_gid=a.product_gid" +
                    " LEFT JOIN pmr_mst_tproductuom d on d.productuom_gid = c.productuom_gid" +
                    " LEFT JOIN pmr_mst_tproductgroup f on c.productgroup_gid = f.productgroup_gid " +
                    " where  a.materialrequisition_gid='" + materialrequisition_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<detialspop_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new detialspop_list
                        {
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            qty_issued = dt["qty_issued"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            stockqty = dt["stock"].ToString(),


                        });
                        values.detialspop_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while error!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostRaisePRSubmit(string user_gid,  raisepr_list values)
        {
            msRQGID = objcmnfunctions.GetMasterGID("PPRP");
            if (msRQGID == "E")
            {
                values.status = true;
                values.message = "Create sequence code PPRP";
            }
            msSQL = " select  a.materialrequisition_gid, " +
                            "  date_format(a.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date,format(a.provisional_amount, 2) as provisional_amount, " +
                            " a.materialrequisition_type,replace(a.materialrequisition_remarks,'<br/>','')as materialrequisition_remarks,a.materialrequisition_reference,e.branch_name,a.branch_gid, " +
                            " concat(b.user_firstname,' - ',b.user_lastname) as user_firstname,d.department_name,  " +
                            " a.costcenter_gid,format(f.budget_available, 2) as budget_available,concat_ws('-',f.costcenter_name,f.costcenter_gid) as costcenter_name " +
                            " from ims_trn_tmaterialrequisition a " +
                            " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                            " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                            " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                            " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid " +
                            " left join pmr_mst_tcostcenter f on f.costcenter_gid = a.costcenter_gid " +
                            " where a.materialrequisition_gid = '" + values.materialrequisition_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    string uiDateStr1 = values.expected_date;
                    DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string expected_date = uiDate1.ToString("yyyy-MM-dd");

                    msSQL = " Insert into pmr_trn_tpurchaserequisition " +
                            " (purchaserequisition_gid, " +
                            " branch_gid, " +
                            " materialrequisition_gid, " +
                            " purchaserequisition_date, " +
                            " purchaserequisition_remarks, " +
                            " purchaserequisition_referencenumber, " +
                            " created_by, " +
                            " created_date, " +
                            " purchaserequisition_flag, " +
                            " product_count, " +
                            " enquiry_raised, " +
                            " purchasereq_type, " +
                            " type, " +
                            " purchaseorder_raised, " +
                            " provisional_amount, " +
                            " priority, " +
                            " priority_remarks," +
                            " costcenter_gid) " +
                            " values (" +
                            "'" + msRQGID + "', " +
                            "'" + dt["branch_gid"] + "'," +
                            "'" + dt["materialrequisition_gid"] + "'," +
                            "'" + expected_date + "', " +
                            "'" + dt["materialrequisition_remarks"].ToString().Replace("'", "\\\'") + "', " +
                            " '" + dt["materialrequisition_reference"].ToString().Replace("'", "\\\'") + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                            " 'PR Approved', " +
                            " '" + mnCtr + "', " +
                            " 'N', " +
                            " 'Opex', " +
                            " '', " +
                            "'N', " +
                            "'" + dt["provisional_amount"] + "'," +
                            "'" + (values.priority == "N" ? "low" : "Y") + "', " +
                            "'" + values.priority_remarks.Replace("'", "\\\'") + "', " +
                            "'" + dt["costcenter_gid"] + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
            }

            foreach (var data2 in values.product_requestlist)
            {
                msGetGID = objcmnfunctions.GetMasterGID("PPDC");
                if (msGetGID == "E")
                {
                    values.status = true;
                    values.message = "Create sequence code PPDC";
                }
                msSQL = " select product_code, product_name, b.productuom_name, a.productgroup_gid, c.productgroup_name " +
                        " from pmr_mst_tproduct a " +
                        " left join pmr_mst_tproductuom b on b.productuom_gid = a.productuom_gid " +
                        " left join pmr_mst_tproductgroup c on c.productgroup_gid = a.productgroup_gid " +
                        " where product_gid='" + data2.product_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    lsproductname = objOdbcDataReader["product_name"].ToString();
                    lsproductcode = objOdbcDataReader["product_code"].ToString();
                    lsproductuomname = objOdbcDataReader["productuom_name"].ToString();
                    lsproductgroupname = objOdbcDataReader["productgroup_name"].ToString();
                    lsproductgroupgid = objOdbcDataReader["productgroup_gid"].ToString();
                }
                objOdbcDataReader.Close();

                // Fetching product price
                msSQL = " select product_price from pmr_trn_tpurchaseorderdtl " +
                        " where product_gid = '" + data2.product_gid + "' and uom_gid='" + data2.productuom_gid + "'" +
                        " order by purchaseorderdtl_gid desc limit 0,1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                lsproductPrice = "0";
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    lsproductPrice = objOdbcDataReader["product_price"].ToString();
                    objOdbcDataReader.Close();
                }
                
                else
                {
                    msSQL = " select price from pmr_trn_tquotationdtl where product_gid ='" + data2.product_gid + "' and uom_gid='" + data2.productuom_gid + "'" +
                            " order by quotationdtl_gid desc limit 0,1";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    lsproductPrice = "0";
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lsproductPrice = objOdbcDataReader["product_price"].ToString();
                        objOdbcDataReader.Close();
                    }
                    else
                    {
                        msSQL = " select cost_price from pmr_mst_tproduct " +
                                " where product_gid ='" + data2.product_gid + "' and productuom_gid='" + data2.productuom_gid + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        lsproductPrice = "0";
                        if (objOdbcDataReader.HasRows)
                        {
                            objOdbcDataReader.Read();
                            lsproductPrice = objOdbcDataReader["cost_price"].ToString();
                            objOdbcDataReader.Close();
                        }
                    }

                }
                if (data2.product_gid == ConfigurationManager.AppSettings["newproduct"])
                {
                    lspr_newproductstatus = "Y";
                    msSQL = " Insert into pmr_trn_tpurchaserequisitiondtl " +
                      " (purchaserequisitiondtl_gid," +
                      " purchaserequisition_gid , " +
                      " product_gid," +
                      " uom_gid," +
                      " qty_requested, " +
                      " seq_no, " +
                      " user_gid, " +
                      " requested_by, " +
                      " display_field , " +
                      " pr_originalqty," +
                      " materialrequisition_gid, " +
                      " productgroup_gid,  " +
                      " product_code,  " +
                      " product_name,  " +
                      " productuom_name,  " +
                      " productgroup_name,  " +
                      " pr_newproductstatus," +
                      " product_price" +
                      " )values (" +
                      "'" + msGetGID + "'," +
                      "'" + msRQGID + "'," +
                      "'" + data2.product_gid + "'," +
                      "'" + data2.productuom_gid + "'," +
                      "'" + data2.req_qty + "', " +
                      "'" + i + "', " +
                      "'" + user_gid + "', " +
                      "'" + user_gid + "', " +
                      "'" + "Null"+ "'," +
                      "'" + data2.req_qty + "', " +
                      "'" + values.materialrequisition_gid + "', " +
                      "'" + lsproductgroupgid + "'," +
                      "'" + lsproductcode + "'," +
                      "'" + lsproductname + "'," +
                      "'" + lsproductuomname + "'," +
                      "'" + lsproductgroupname + "'," +
                      "'Y'," +
                      "'" + lsproductPrice + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    lspr_newproductstatus = "N";
                    msSQL = " Insert into pmr_trn_tpurchaserequisitiondtl " +
                      " (purchaserequisitiondtl_gid," +
                      " purchaserequisition_gid , " +
                      " product_gid," +
                      " uom_gid," +
                      " qty_requested, " +
                      " seq_no, " +
                      " user_gid, " +
                      " requested_by, " +
                      " pr_originalqty," +
                      " materialrequisition_gid, " +
                      " productgroup_gid,  " +
                      " product_code,  " +
                      " product_name,  " +
                      " productuom_name,  " +
                      " productgroup_name,  " +
                      " display_field," +
                      " product_price) " +
                      " values (" +
                      "'" + msGetGID + "'," +
                      "'" + msRQGID + "'," +
                      "'" + data2.product_gid + "'," +
                      "'" + data2.productuom_gid + "'," +
                      "'" + data2.req_qty + "', " +
                      "'" + i + "', " +
                      "'" + user_gid + "', " +
                      "'" + user_gid + "', " +
                      "'" + data2.req_qty + "', " +
                      "'" + values.materialrequisition_gid + "', " +
                      "'" + lsproductgroupgid + "'," +
                      "'" + lsproductcode + "'," +
                      "'" + lsproductname + "'," +
                      "'" + lsproductuomname + "'," +
                      "'" + lsproductgroupname + "'," +
                      "'" + "Null" + "'," +
                      "'" + lsproductPrice + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult == 0)
                {
                    values.status = false;
                }
                i++;
            }

            // Commented approval-related code
            /*
            msSQL = " select a.employee_gid, a.hierarchy_level, concat(b.user_firstname, '-', b.user_code) as user from adm_mst_tsubmodule a " +
                    " inner join hrm_mst_temployee c on a.employee_gid = c.employee_gid " +
                    " inner join adm_mst_tuser b on c.user_gid = b.user_gid " +
                    " where a.module_gid = 'PMR' and a.submodule_id='PMRPROAPR' and a.employee_gid = '" + employee_gid + "' ";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    employee = dt["employee_gid"].ToString();
                    msGetGID = objcmnfunctions.GetMasterGID("PODC");
                    msSQL = "insert into pmr_trn_tapproval (approval_gid, approved_by, approved_date, submodule_gid, pr_gid) " +
                            " values ('" + msGetGID + "', '" + employee + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'PMRPROAPR', '" + msRQGID + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                    }
                }
            }
            */

            if (mnResult == 1)
            {
                msSQL = " Update ims_trn_tmaterialrequisition Set material_status = 'PR Raised' where materialrequisition_gid = '" + values.materialrequisition_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Raise Purchase Requisition Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Some error occurred while raising Purchase Requisition";
                }
            }
            else
            {
                values.status = false;
                values.message = "Some error occurred while raising Purchase Requisition";
            }
        }
    }
}
using ems.pmr.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Http.Results;
using static ems.pmr.Models.addgrn_lists;
using System.Web.UI.WebControls;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;
using System.Web.UI;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Configuration;
using CrystalDecisions.Shared;
using System.Management.Instrumentation;
using System.Web.WebSockets;
using Image = System.Drawing.Image;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using ParameterType = RestSharp.ParameterType;
using System.Security.Cryptography;


namespace ems.pmr.DataAccess
{
    public class DaPmrTrnGrn
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, txtGRNRefNo, lsapproval, lsgrn_flage, lblpurchasebranch_gid, msGetStockGID,
            lsgrn_status, lspurchaserequisition_gid, grn_date, expected_date, lsqty_billed, lsgrn_gid, lblVendor_gid, msGetGID,
            lstPR_GRN_flag, lsproductname, lsproductcode, lsproductuomname, lstPO_GRN_flag, lspurchaseorder_status,
            lsemployee_gid, lblBranch_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msStockGid,
            msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid, lsdisplay, lsunit_price;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lsQty_ReceivedAS, lsQty_Ordered, lsQty_Delivered, lsQty_Adjustable;
        int lsPR_Rec, lsPRt_GRNAdj;
        DataSet ds_dataset;
        private string grn_gid;
        double qtyrequested, finalQty;
        string base_url, api_key, client_id = string.Empty, mintsoft_flag,lsfinyear;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        public void DaGrninwardSummary(MdlPmrTrnGrn values)
        {
            try
            {


                //msSQL = " SELECT a.purchaseorder_gid, a.vendor_gid, a.created_by, a.purchaseorder_status, a.created_by,concat(e.contactperson_name,'/',e.email_id,'/',e.contact_telephonenumber) as contact_info,e.vendor_companyname,j.costcenter_name, " +
                //    " date_format(a.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date, y.branch_name, c.department_name, concat(d.user_firstname,' ',d.user_lastname) as user_firstname,case when group_concat(distinct f.purchaserequisition_referencenumber)=',' then '' " +
                //    " when group_concat(distinct f.purchaserequisition_referencenumber) <> ',' then  group_concat(distinct f.purchaserequisition_referencenumber) end  as refrence_no " +
                //    " FROM pmr_trn_tpurchaseorder a  left join hrm_mst_temployee b on a.created_by = b.user_gid " +
                //    " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid " +
                //    " left join adm_mst_tuser d on d.user_gid = a.created_by  " +
                //    " left join acp_mst_tvendor e on e.vendor_gid=a.vendor_gid " +
                //    " left join pmr_mst_tcostcenter j on j.costcenter_gid=a.costcenter_gid " +
                //    " left join pmr_Trn_tpurchaserequisition f on a.purchaserequisition_gid=f.purchaserequisition_gid " +
                //    " left join hrm_mst_tbranch y on a.branch_gid=y.branch_gid " +
                //    " where 0=0 and  ((a.purchaseorder_flag = 'PO Approved' and a.grn_flag = 'GRN Pending')  or" +
                //    " (a.grn_flag = 'Goods Received Partial' and (a.invoice_flag = 'IV Pending' or a.invoice_flag = 'Invoice Raised Partial'))) " +
                //    " group by a.purchaseorder_gid  order by a.created_date desc ";
                msSQL = "select poapproval_flag from adm_mst_tcompany ";
                string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.purchaseorder_gid," +
                        " CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE a.poref_no END AS porefno,y.branch_prefix, " +
                        " a.vendor_gid, a.created_by, a.purchaseorder_status, " +
                        " concat(e.contactperson_name, '/', e.email_id, '/', e.contact_telephonenumber) as contact_info, " +
                        " e.vendor_companyname,j.costcenter_name, date_format(a.purchaseorder_date, '%d-%m-%Y') as purchaseorder_date, " +
                        " y.branch_name,c.department_name, concat(d.user_firstname, ' ', d.user_lastname) as user_firstname, " +
                        " case when group_concat(distinct f.purchaserequisition_referencenumber) = ',' then '' " +
                        " when group_concat(distinct f.purchaserequisition_referencenumber) <> ',' then group_concat(distinct f.purchaserequisition_referencenumber) end as reference_no, " +
                        " SUM(CASE WHEN a.purchaseorder_status = 'Operational Approved PO, Finance Approved PO' THEN 1 ELSE 0 END) as operational_finance_approved_count, " +
                        " SUM(CASE WHEN a.purchaseorder_flag = 'PO Approved' THEN 1 ELSE 0 END) as po_approved_count " +
                        " FROM pmr_trn_tpurchaseorder a " +
                        " LEFT JOIN hrm_mst_temployee b ON a.created_by = b.user_gid " +
                        " LEFT JOIN hrm_mst_tdepartment c ON b.department_gid = c.department_gid " +
                        " LEFT JOIN adm_mst_tuser d ON d.user_gid = a.created_by " +
                        " LEFT JOIN acp_mst_tvendor e ON e.vendor_gid = a.vendor_gid " +
                        " LEFT JOIN pmr_mst_tcostcenter j ON j.costcenter_gid = a.costcenter_gid " +
                        " LEFT JOIN pmr_Trn_tpurchaserequisition f ON a.purchaserequisition_gid = f.purchaserequisition_gid " +
                        " LEFT JOIN hrm_mst_tbranch y ON a.branch_gid = y.branch_gid LEFT JOIN pmr_trn_tpurchaseorderdtl s ON a.purchaseorder_gid = s.purchaseorder_gid " +
                        " LEFT JOIN pmr_mst_tproduct g ON s.product_gid = g.product_gid LEFT JOIN pmr_mst_tproducttype i ON i.producttype_gid = g.producttype_gid" +
                        " LEFT JOIN pmr_mst_tproductgroup l ON g.productgroup_gid = l.productgroup_gid " +
                        " WHERE 0=0 ";
                if (lsapproval_flage == "Y")
                {
                    msSQL += " And ((a.purchaseorder_status = 'Operational Approved PO, Finance Approved PO' OR " +
                            "a.purchaseorder_status = 'Approved' or a.purchaseorder_status = 'PO Approved' ) AND a.grn_flag = 'GRN Pending'";
                }
                else
                {
                    msSQL += " And ((a.purchaseorder_flag = 'PO Approved' and a.grn_flag = 'GRN Pending') ";
                }
                msSQL += " OR a.grn_flag = 'Goods Received Partial' AND(a.invoice_flag = 'IV Pending' OR a.invoice_flag = 'Invoice Raised Partial')) " +
                        "AND ((i.producttype_name IS NULL OR i.producttype_name = '') OR (i.producttype_name != 'Services')    " +
                        " OR (i.producttype_name = 'Services' AND l.productgroup_name = 'General')   ) " +
                          "GROUP BY a.purchaseorder_gid " +
                        " ORDER BY a.created_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getgrn_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getgrn_lists
                        {

                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            porefno = dt["porefno"].ToString(),
                            purchaseorder_status = dt["purchaseorder_status"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["user_firstname"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            contact_info = dt["contact_info"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.Getgrn_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting GRN Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                   ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                   msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                   DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetaddgrnsummary(string user_gid, string purchaseorder_gid, MdlPmrTrnGrn values)
        {
            try
            {

                grn_gid = objcmnfunctions.GetMasterGID("PGNP", "", user_gid);

                if (grn_gid == "E")
                {
                    values.message = "Create Sequence Code PGNP for GRN Table";
                }

                //Raised By Binding Event
                string userFirstNameSQL = " select a.user_gid, concat(a.user_firstname,' - ',c.department_name) as user_firstname " +
                                          " from adm_mst_tuser a " +
                                          " left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                                          " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid " +
                                          " where a.user_gid = '" + user_gid + "' ";
                DataTable userFirstNameDataTable = objdbconn.GetDataTable(userFirstNameSQL);

                string userFirstName = string.Empty;

                if (userFirstNameDataTable.Rows.Count > 0)
                {
                    userFirstName = userFirstNameDataTable.Rows[0]["user_firstname"].ToString();
                }
                //--END--//


                //Check by user drop down event
                msSQL = " select branch_gid from pmr_trn_tpurchaseorder where purchaseorder_gid = '" + purchaseorder_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    lblpurchasebranch_gid = objOdbcDataReader["branch_gid"].ToString();
                    objOdbcDataReader.Close();
                }
                msSQL = " select b.branch_gid, d.mainbranch_flag " +
                     " from adm_mst_tuser a " +
                     " left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                     " left join hrm_mst_tbranch d on b.branch_gid = d.branch_gid " +
                     " where a.user_gid = '" + user_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    lblBranch_gid = objOdbcDataReader["branch_gid"].ToString();
                    objOdbcDataReader.Close();
                }

                string userFirstName1SQL = "SELECT CONCAT(a.user_firstname, ' ', a.user_lastname) AS user_firstname1, a.user_gid, a.user_code " +
                        "FROM adm_mst_tuser a " +
                        "LEFT JOIN hrm_mst_temployee b ON a.user_gid = b.user_gid " +
                        "WHERE b.branch_gid = '" + lblpurchasebranch_gid + "' OR b.branch_gid = '" + lblBranch_gid + "'";

                DataTable userFirstName1DataTable = objdbconn.GetDataTable(userFirstName1SQL);

                List<string> user_firstname1List = new List<string>();

                foreach (DataRow row in userFirstName1DataTable.Rows)
                {
                    string user_firstname1 = row["user_firstname1"].ToString();
                    user_firstname1List.Add(user_firstname1);
                }

                //--END--//



                msSQL = "select mintsoft_flag from adm_mst_tcompany";
                mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);

                //Other fieds biding event query
                msSQL = "SELECT a.purchaseorder_gid,date_format(a.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date ,c.tax_number, a.branch_gid, b.branch_name, c.vendor_companyname, c.contactperson_name, c.contact_telephonenumber,date_format(a.expected_date,'%d-%m-%Y') as expected_date, c.email_id, a.shipping_address as address " +
                        "FROM pmr_trn_tpurchaseorder a " +
                        "LEFT JOIN hrm_mst_tbranch b ON a.branch_gid = b.branch_gid " +
                        "LEFT JOIN acp_mst_tvendor c ON a.vendor_gid = c.vendor_gid " +
                        "LEFT JOIN adm_mst_taddress d ON c.address_gid = d.address_gid " +
                        "WHERE a.purchaseorder_gid = '" + purchaseorder_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<grn_lists>();

                if (dt_datatable.Rows.Count != 0)
                {

                    int user_firstname1Index = 0;

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        if (user_firstname1Index < user_firstname1List.Count)
                        {
                            // Get the user_firstname1 value from the list using the counter
                            string user_firstname1 = user_firstname1List[user_firstname1Index];

                            getModuleList.Add(new grn_lists
                            {
                                branch_name = dt["branch_name"].ToString(),
                                vendor_companyname = dt["vendor_companyname"].ToString(),
                                contactperson_name = dt["contactperson_name"].ToString(),
                                contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                                email_id = dt["email_id"].ToString(),
                                address = dt["address"].ToString(),
                                purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                                tax_number = dt["tax_number"].ToString(),
                                purchaseorder_date = dt["purchaseorder_date"].ToString(),
                                expected_date = dt["expected_date"].ToString(),
                                grn_gid = grn_gid,
                                user_firstname = userFirstName,
                                user_firstname1 = user_firstname1,
                                mintsoft_flag = mintsoft_flag,
                            });

                            // Increment the counter for the next iteration
                            user_firstname1Index++;
                        }
                    }

                    values.grn_lists = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  add GRN Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                   ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                   msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                   DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetsummaryaddgrnsummary(string user_gid, string purchaseorder_gid, MdlPmrTrnGrn values)
        {
            try
            {

                msSQL = " delete from pmr_tmp_tgrn where user_gid = '" + user_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL = " select a.product_gid,e.producttype_gid,e.producttype_name, a.uom_gid, a.purchaseorderdtl_gid, a.qty_ordered, a.qty_received, a.qty_grnadjusted, " +
                        " a.qty_received  as qty_delivered, " +
                        " a.purchaseorder_gid, a.product_price, a.display_field_name, a.product_name, a.product_code, a.productuom_name,i.productgroup_name, " +
                        " d.purchaseorder_status " +
                        " from pmr_trn_tpurchaseorderdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " left join pmr_mst_tproductuom c on c.productuom_gid = b.productuom_gid " +
                        " left join pmr_mst_tproductgroup i on i.productgroup_gid = b.productgroup_gid " +
                        " left join pmr_trn_tpurchaseorder d on d.purchaseorder_gid = a.purchaseorder_gid " +
                        " left join pmr_mst_tproducttype e on e.producttype_gid = b.producttype_gid " +
                        " WHERE a.purchaseorder_gid = '" + purchaseorder_gid + "'and a.qty_ordered <> a.qty_received order by a.purchaseorder_gid desc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addgrn_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addgrn_list
                        {

                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            display_field_name = dt["display_field_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            qty_received = dt["qty_received"].ToString(),
                            //qty_free = dt["qty_free"].ToString(),
                            qty_grnadjusted = dt["qty_grnadjusted"].ToString(),
                            qty_delivered = dt["qty_delivered"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                        });
                        values.addgrn_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting add GRN Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                   ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                   msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                   DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostGrnSubmit(string user_gid, addgrn_lists values)
        {
            try
            {

                foreach (var data in values.summary_list)
                {
                    lsQty_Delivered = (int)Convert.ToDouble(data.qty_delivered);
                    lsQty_ReceivedAS = (int)Convert.ToDouble(data.qty_received);
                    lsQty_Adjustable = (int)Convert.ToDouble(data.qty_grnadjusted);
                    lsQty_Ordered = (int)Convert.ToDouble(data.qty_ordered);

                    if (lsQty_Ordered < lsQty_ReceivedAS)
                    {
                        values.message = "Sum of Qty Received and Qty Received as per Invoice should not be greater than Qty Ordered";
                        values.status = false;
                        return;
                    }

                    //msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + data.product_name + "' ";
                    //string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select purchaseorderdtl_gid from pmr_trn_tpurchaseorderdtl where product_gid='" + data.product_gid + "' AND purchaseorder_gid='" + values.purchaseorder_gid + "' ";
                    string lspurchaseorderdtlgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select producttype_gid from pmr_mst_tproduct where  product_gid='" + data.product_gid + "' ";
                    string lsproducttypegid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select a.product_name, a.product_code, a.productuom_name from pmr_trn_tpurchaseorderdtl a  " +
                            " where a.purchaseorderdtl_gid = '" + lspurchaseorderdtlgid + "' and product_gid = '" + data.product_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lsproductname = objOdbcDataReader["product_name"].ToString();
                        lsproductcode = objOdbcDataReader["product_code"].ToString();
                        lsproductuomname = objOdbcDataReader["productuom_name"].ToString();
                        objOdbcDataReader.Close();
                    }
                    msGetGID = objcmnfunctions.GetMasterGID("PGDC");
                    msSQL = " insert into pmr_trn_tgrndtl (" +
                             " grndtl_gid, " +
                             " grn_gid, " +
                             " purchaseorderdtl_gid, " +
                             " product_gid," +
                             " product_code," +
                             " product_name," +
                             " productuom_name," +
                             " display_field," +
                             " qty_delivered," +
                             " qtyreceivedas," +
                             " producttype_gid, " +
                             " qty_grnadjusted) " +
                             " values (" +
                             "'" + msGetGID + "', " +
                             "'" + values.grn_gid + "', " +
                             "'" + lspurchaseorderdtlgid + "', " +
                             "'" + data.product_gid + "'," +
                             "'" + lsproductcode + "'," +
                             "'" + data.product_name.Replace("'", "\\\'") + "', " +
                             "'" + data.productuom_name.Replace("'", "\\\'") + "', " +
                             "'" + data.display_field_name.Replace("'", "\\\'") + "', " +
                             "'" + data.qty_received + "'," +
                             "'" + data.qty_received + "'," +
                             "'" + lsproducttypegid + "'," +
                             "'0')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    double lsSum_Rec = data.qty_received;
                    double lsSumPOt_GRNAdj = data.qty_grnadjusted;
                    msSQL = " select qty_received, qty_grnadjusted " +
                            " from pmr_trn_tpurchaseorderdtl  where " +
                            " purchaseorderdtl_gid = '" + lspurchaseorderdtlgid + "' and " +
                            " product_gid = '" + data.product_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsSum_Rec = lsSum_Rec + double.Parse(objOdbcDataReader["qty_received"].ToString());
                        lsSumPOt_GRNAdj = lsSumPOt_GRNAdj + double.Parse(objOdbcDataReader["qty_grnadjusted"].ToString());
                    }
                    msSQL = "UPDATE pmr_trn_tpurchaseorderdtl " +
                            "SET qty_received = '" + lsSum_Rec + "', " +
                            "qty_grnadjusted = '" + lsSumPOt_GRNAdj + "'" +
                            "WHERE purchaseorderdtl_gid = '" + lspurchaseorderdtlgid + "' AND " +
                            "product_gid = '" + data.product_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error occured while inserting into Purchaseorder table";
                    }
                    else
                    {
                        lspurchaseorder_status = "PO Completed";
                        lstPO_GRN_flag = "GRN Pending";
                    }

                    msSQL = " SELECT qty_received, qty_grnadjusted, qty_ordered " +
                            " FROM pmr_trn_tpurchaseorderdtl WHERE " +
                            " purchaseorder_gid = '" + values.purchaseorder_gid + "' AND " +
                            " (qty_received + qty_grnadjusted) < qty_ordered";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        lspurchaseorder_status = "PO Work In Progress";
                        lstPO_GRN_flag = "Goods Received Partial";

                    }
                    else
                    {
                        lspurchaseorder_status = "PO Completed";
                        lstPO_GRN_flag = "Goods Received";
                    }
                    msSQL = " Update pmr_trn_tpurchaseorder " +
                                           " Set purchaseorder_status = '" + lspurchaseorder_status + "'," +
                                           " grn_flag = '" + lstPO_GRN_flag + "'" +
                                           " where purchaseorder_gid = '" + values.purchaseorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        lsgrn_status = "GRN Pending";
                        values.message = "Error occured while inserting into Purchaseorder table";
                    }
                    msSQL = "select purchaserequisition_gid from pmr_trn_tpurchaseorder where purchaseorder_gid='" + values.purchaseorder_gid + "' ";
                    string lspurchaserequisition_gid = objdbconn.GetExecuteScalar(msSQL);
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    {
                        objOdbcDataReader.Read();
                        lspurchaserequisition_gid = objOdbcDataReader["purchaserequisition_gid"].ToString();
                        objOdbcDataReader.Close();
                    }

                    lsPR_Rec = lsQty_ReceivedAS;
                    lsPRt_GRNAdj = lsQty_Adjustable;
                    msSQL = " select qty_received, qty_grnadjusted " +
                                    " from pmr_trn_tpurchaserequisitiondtl where " +
                                    " purchaserequisition_gid = '" + lspurchaserequisition_gid + "' and " +
                                    " product_gid = '" + data.product_gid + "'";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lsPR_Rec = lsPR_Rec + int.Parse(objOdbcDataReader["qty_received"].ToString());
                        lsPRt_GRNAdj = lsPRt_GRNAdj + int.Parse(objOdbcDataReader["qty_grnadjusted"].ToString());
                        objOdbcDataReader.Close();
                    }
                    msSQL = " update pmr_trn_tpurchaserequisitiondtl set " +
                                    " qty_received = '" + lsPR_Rec + "'," +
                                    " qty_grnadjusted = '" + lsPRt_GRNAdj + "'" +
                                    " where purchaserequisition_gid = '" + lspurchaserequisition_gid + "' and " +
                                    " product_gid = '" + data.product_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error occured while inserting into Purchase Requisition Detail table";
                    }
                    msSQL = " select qty_received, qty_grnadjusted, qty_requested " +
                                    " from pmr_trn_tpurchaserequisitiondtl  where " +
                                    " purchaserequisition_gid = '" + lspurchaserequisition_gid + "'and" +
                                    " (qty_received + qty_grnadjusted) < qty_requested ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        lstPR_GRN_flag = "Goods Received Partial";
                    }
                    else
                    {
                        lstPR_GRN_flag = "Goods Received";
                    }
                    msSQL = " Update pmr_trn_tpurchaserequisition " +
                                    " Set grn_flag = '" + lstPR_GRN_flag + "'" +
                                    " where purchaserequisition_gid = '" + lspurchaserequisition_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error occured while inserting into Purchase Requisition Table";
                    }
                    msSQL = " select branch_gid,vendor_gid from pmr_trn_tpurchaseorder where purchaseorder_gid = '" + values.purchaseorder_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lblpurchasebranch_gid = objOdbcDataReader["branch_gid"].ToString();
                        lblVendor_gid = objOdbcDataReader["vendor_gid"].ToString();
                        objOdbcDataReader.Close();
                    }
                    msSQL = "select branch_gid from hrm_mst_tbranch where branch_name='" + values.branch_name + "' ";
                    string lsbranchgid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + data.productuom_name + "' ";
                    string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select product_price,display_field_name from pmr_trn_tpurchaseorderdtl where product_gid='" + data.product_gid + "' and purchaseorder_gid='" + values.purchaseorder_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lsunit_price = objOdbcDataReader["product_price"].ToString();
                        lsdisplay = objOdbcDataReader["display_field_name"].ToString();
                        objOdbcDataReader.Close();
                    }

                    msSQL = " SELECT product_gid,stock_qty FROM ims_trn_tstock " +
                          " where product_gid = '" + data.product_gid + "' and" +
                          " stocktype_gid = 'SY0905270001' and" +
                          " branch_gid = '" + lsbranchgid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (objOdbcDataReader.HasRows == false)
                    {
                        objOdbcDataReader.Close();
                    }
                    if (dt_datatable.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            string lsstockQty = dt["stock_qty"].ToString();
                            int stockQty = int.Parse(lsstockQty);
                            double OpeningstockQty = data.qty_received;
                            qtyrequested = stockQty + OpeningstockQty;
                        }

                        if (qtyrequested != 0)
                        {
                            finalQty = qtyrequested;
                        }
                        else
                        {
                            finalQty = data.qty_received;
                        }

                        msSQL = " UPDATE ims_trn_tstock " +
                                " SET stock_qty = '" + finalQty + "'," +
                                " display_field='" + lsdisplay.Replace("'", "\\\'") + "', " +
                                " created_by='" + user_gid + "'" +
                                " WHERE product_gid = '" + data.product_gid + "' and " +
                                " branch_gid = '" + lsbranchgid + "' and " +
                                " stocktype_gid = 'SY0905270001'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {

                            values.status = false;
                            values.message = "Error Occured while adding Stock";
                        }
                        else
                        {
                            msSQL = "select mintsoft_flag from adm_mst_tcompany";
                            mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (mintsoft_flag == "Y")
                            {
                                StockToMintSoft OBJMintsoftStock = new StockToMintSoft();

                                msSQL = " select * from smr_trn_tminsoftconfig;";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    objOdbcDataReader.Read();
                                    base_url = objOdbcDataReader["base_url"].ToString();
                                    api_key = objOdbcDataReader["api_key"].ToString();
                                    client_id = objOdbcDataReader["client_id"].ToString();
                                }
                                objOdbcDataReader.Close();
                                msSQL = " select customerproduct_code, mintsoftproduct_id from pmr_mst_tproduct where product_gid='" + data.product_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    //try
                                    //{
                                    //    if (objOdbcDataReader["customerproduct_code"] != "Null" || objOdbcDataReader["customerproduct_code"] != "")
                                    //    {
                                    //        OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                                    //    }
                                    //    else
                                    //    {
                                    //        return;
                                    //    }
                                    //    if (objOdbcDataReader["mintsoftproduct_id"] != "Null" || objOdbcDataReader["mintsoftproduct_id"] != "")
                                    //    {
                                    //        OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                                    //    }
                                    //    else
                                    //    {
                                    //        return;
                                    //    }
                                    //} 
                                    //catch 
                                    //{
                                    //    values.message = "Mintsoftproduct id is null!...";
                                    //}

                                    OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                                    OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                                }

                                OBJMintsoftStock.Quantity = Convert.ToInt32(finalQty);
                                OBJMintsoftStock.WarehouseId = 3;
                                //string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                // Parse the JSON object
                                JObject jsonObject = JObject.Parse(json);

                                // Create a JArray and add the JObject to it
                                JArray jsonArray = new JArray();
                                jsonArray.Add(jsonObject);

                                // Convert the JArray back to JSON string
                                string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(base_url);
                                var request = new RestRequest("/api/Product/BulkOnHandStockUpdate?ClientId="+ client_id + "", Method.POST);
                                request.AddHeader("ms-apikey", api_key);
                                request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    List<Class1> objResult = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
                                    if (objResult[0].Success)
                                    {
                                        var result = objResult[0].ID;
                                    }
                                }
                            }
                            values.status = true;
                            values.message = "Opening Stock Updated Successfully";
                        }

                    }
                    else
                    {
                        msSQL = " select cast(concat(year(fyear_start), '-',  case when fyear_end is not null then year(fyear_end)  when month(curdate())= '1' then year(curdate()) " + 
                                " when month(curdate())= '2' then year(curdate())  when month(curdate())= '3' then year(curdate())  else year(date_Add(curdate(), interval 1 year)) end )as char) as finyear " +
                                " from adm_mst_tyearendactivities order by finyear_gid desc ";
                        lsfinyear = objdbconn.GetExecuteScalar(msSQL);


                        msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                        msSQL = "INSERT INTO ims_trn_tstock (" +
                                "stock_gid, " +
                                "branch_gid, " +
                                "product_gid, " +
                                "unit_price, " +
                                "display_field, " +
                                "uom_gid, " +
                                "stock_qty, " +
                                "financial_year, " +
                                "grn_qty, " +
                                "rejected_qty, " +
                                "stocktype_gid, " +
                                "reference_gid, " +
                                "stock_flag, " +
                                "created_date, " +
                                "adjusted_qty) " +
                                "VALUES (" +
                                "'" + msGetStockGID + "', " +
                                "'" + lsbranchgid + "', " +
                                "'" + data.product_gid + "', " +
                                "'" + lsunit_price + "', " +
                                "'" + lsdisplay.Replace("'", "\\\'") + "', " +
                                "'" + lsproductuomgid + "', " +
                                "'" + data.qty_received + "', " +
                                "'" + lsfinyear + "', " +
                                "'" + data.qty_received + "', " +
                                "'0', " +
                                "'SY0905270002', " +
                                "'" + values.grn_gid + "', " +
                                "'Y', " +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                "'0')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            values.status = false;
                        }
                        else
                        {
                            msSQL = "select mintsoft_flag from adm_mst_tcompany";
                            mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (mintsoft_flag == "Y")
                            {
                                StockToMintSoft OBJMintsoftStock = new StockToMintSoft();

                                msSQL = " select * from smr_trn_tminsoftconfig;";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    objOdbcDataReader.Read();
                                    base_url = objOdbcDataReader["base_url"].ToString();
                                    api_key = objOdbcDataReader["api_key"].ToString();
                                    client_id = objOdbcDataReader["client_id"].ToString();
                                }
                                objOdbcDataReader.Close();
                                msSQL = " select customerproduct_code, mintsoftproduct_id from pmr_mst_tproduct where product_gid='" + data.product_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                                    OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                                }
                                OBJMintsoftStock.Quantity = Convert.ToInt32(data.qty_received); ;
                                OBJMintsoftStock.WarehouseId = 3;
                                //string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                // Parse the JSON object
                                JObject jsonObject = JObject.Parse(json);

                                // Create a JArray and add the JObject to it
                                JArray jsonArray = new JArray();
                                jsonArray.Add(jsonObject);

                                // Convert the JArray back to JSON string
                                string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(base_url);
                                var request = new RestRequest("/api/Product/BulkOnHandStockUpdate?ClientId=" + client_id + "", Method.POST);
                                request.AddHeader("ms-apikey", api_key);
                                request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    List<Class1> objResult = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
                                    if (objResult[0].Success)
                                    {
                                        var result = objResult[0].ID;
                                    }
                                }
                            }
                            values.status = true;
                            values.message = "Opening Stock Added Successfully";
                        }

                    }
                }
                msSQL = "select poapproval_flag from adm_mst_tcompany";
                lsapproval = objdbconn.GetExecuteScalar(msSQL);
                if (lsapproval == "Y")
                {
                    lsgrn_flage = "GRN Pending QC";
                }
                else
                {
                    lsgrn_flage = "GRN Approved";
                }
                string uiDateStr = values.grn_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                grn_date = uiDate.ToString("yyyy-MM-dd");
                string uiDateStr1 = values.expected_date;
                DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                expected_date = uiDate1.ToString("yyyy-MM-dd");
                msSQL = "select name from crm_smm_tmintsoftcourierservice where id='" + values.modeof_dispatch + "'";
                string mode = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "INSERT INTO pmr_trn_tgrn (" +
                            "grn_gid, " +
                            "branch_gid, " +
                            "purchaseorder_gid, " +
                            "grn_date, " +
                            "expected_date, " +
                            "received_note, " +
                            "no_of_boxs, " +
                            "dispatch_mode, " +
                            "deliverytracking_number, " +
                            "vendor_gid, " +
                            "vendor_contact_person, " +
                            "dc_no, " +
                            "invoice_refno, " +
                            "grn_status, " +
                            "grn_flag, " +
                            "grn_remarks, " +
                            "priority, " +
                            "checkeruser_gid, " +
                            "user_gid, " +
                            "created_date, " +
                            "vendor_address, " +
                            "currency_code, " +
                            "invoice_date, " +
                            "dc_date) " +
                            "VALUES (" +
                            "'" + values.grn_gid + "', " +
                            "'" + lblpurchasebranch_gid + "', " +
                            "'" + values.purchaseorder_gid + "', " +
                            "'" + grn_date + "', " +
                            "'" + expected_date + "', " +
                            "'" + values.received_note.Replace("'", "\\\'") + "', " +
                            "'" + values.no_box + "', " +
                            "'" + mode + "', " +
                            "'" + values.deliverytracking + "', " +
                            "'" + lblVendor_gid + "', " +
                            "'" + values.contactperson_name + "', " +
                            "'" + values.dc_no + "', " +
                            "'" + values.invoiceref_no + "', " +
                            "'Invoice Pending', " +
                            "'" + lsgrn_flage + "', " +
                            "'" + values.grn_remarks + "', " +
                            "'" + values.priority_flag + "', " +
                            "'" + user_gid + "', " +
                            "'" + user_gid + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                            "'" + values.address + "', " +
                            "'INR', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                }
                msSQL = "select mintsoft_flag from adm_mst_tcompany";
                mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                if (mintsoft_flag == "Y")
                {
                    result objresult = new result();
                    ASNList objMdlMintsoftJSON = new ASNList();
                    PMRASNSTOCK_list OBJMintsoftStock = new PMRASNSTOCK_list();
                    msSQL = " select * from smr_trn_tminsoftconfig;";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        objOdbcDataReader.Read();
                        base_url = objOdbcDataReader["base_url"].ToString();
                        api_key = objOdbcDataReader["api_key"].ToString();
                        client_id = objOdbcDataReader["client_id"].ToString();
                    }
                    objOdbcDataReader.Close();

                    msSQL = " select goodsintypes_name from crm_smm_tmintsoftasngoodsintypes where goodsintypes_id='" + values.goodsintypes_id + "'";
                    string goodsintypes_name = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select supplier_id from acP_mst_tvendor where  vendor_companyname='" + values.vendor_companyname + "'";
                    string supplier_id = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select a.expected_date, b.poref_no from pmr_trn_tgrn a  " +
                        " left join pmr_trn_tpurchaseorder b on b.purchaseorder_gid = a.purchaseorder_gid " +
                        " where grn_gid ='" + values.grn_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        string expecteddate = objOdbcDataReader["expected_date"].ToString() + DateTimeOffset.Now.ToString("THH:mm:ss.fffffffK");
                        objMdlMintsoftJSON.WarehouseId = 3;
                        objMdlMintsoftJSON.Supplier = values.vendor_companyname;
                        objMdlMintsoftJSON.POReference = objOdbcDataReader["poref_no"].ToString();
                        objMdlMintsoftJSON.EstimatedDelivery = expecteddate;
                        objMdlMintsoftJSON.GoodsInType = goodsintypes_name;
                        objMdlMintsoftJSON.ProductSupplierId = int.Parse(supplier_id);
                        objMdlMintsoftJSON.ClientId = client_id;
                    }

                    msSQL = "select b.customerproduct_code,a.qtyreceivedas,b.mintsoftproduct_id, d.expected_date" +
                            " from pmr_trn_tgrndtl a " +
                            " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                            " left join pmr_trn_tgrn d on a.grn_gid=d.grn_gid" +
                            " where a.grn_gid = '" + values.grn_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count > 0)
                    {
                        int i = 0;
                        objMdlMintsoftJSON.Items = new AsnItem[dt_datatable.Rows.Count];

                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            objMdlMintsoftJSON.Items[i] = new AsnItem();
                            objMdlMintsoftJSON.Items[i].ProductId = dt["mintsoftproduct_id"].ToString();
                            objMdlMintsoftJSON.Items[i].SKU = dt["customerproduct_code"].ToString();
                            objMdlMintsoftJSON.Items[i].Quantity = int.Parse(dt["qtyreceivedas"].ToString());
                            objMdlMintsoftJSON.Items[i].ExpiryDate = dt["expected_date"].ToString() + DateTimeOffset.Now.ToString("THH:mm:ss.fffffffK");
                            i++;
                        }
                        dt_datatable.Dispose();
                    }
                    string json = JsonConvert.SerializeObject(objMdlMintsoftJSON);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(base_url);
                    var request = new RestRequest("/api/ASN", Method.PUT);
                    request.AddHeader("ms-apikey", api_key);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", json, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseData = JsonConvert.DeserializeObject<MintsoftASNResponse>(response.Content);
                        msSQL = "update pmr_trn_tgrn set mintsoftasn_id = '" + responseData.ID + "' where grn_gid ='" + values.grn_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            objresult.status = true;
                             objresult.message = "ASN Added Successfully, ASN ID:" + responseData.ID + "";
                            //string url = "/api/ASN/" + responseData.ID + "/ Confirm";
                            //var client1 = new RestClient(base_url);
                            //var request1 = new RestRequest(url, Method.GET);
                            //request.AddHeader("Accept", "application/json");
                            //request.AddHeader("ms-apikey", api_key);
                            //IRestResponse response1 = client.Execute(request1);
                            //if (response1.StatusCode == HttpStatusCode.OK)
                            //{
                            //    objresult.status = true;
                            //    objresult.message = "ASN Added Successfully, ASN ID:" + responseData.ID + "";
                            //}
                        }
                    }
                }
                msSQL = "select employee_gid from hrm_mst_temployee where user_gid =  '" + user_gid + "'";
                string employee_gid = objdbconn.GetExecuteScalar(msSQL);

                msGetGID = objcmnfunctions.GetMasterGID("PODC");

                msSQL = "insert into pmr_trn_tapproval ( " +
                            " approval_gid, " +
                            " approved_by, " +
                            " approved_date, " +
                            " submodule_gid, " +
                            " grnapproval_gid " +
                            " ) values ( " +
                            "'" + msGetGID + "'," +
                            " '" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'PMRSTKGRA'," +
                            "'" + values.grn_gid + "') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                }
                msSQL = " Delete from pmr_tmp_tgrn where user_gid = '" + user_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "GRN Raised Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Raising GRN";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raising GRN!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                   ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                   msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                   DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public class MintsoftASNResponse
        {
            public string ID { get; set; }
            public string Success { get; set; }
            public string Message { get; set; }
            public string WarningMessage { get; set; }
            public string AllocatedFromReplen { get; set; }
        }
        public Dictionary<string, object> DaGetGRNPDF(string grn_gid, MdlPmrTrnGrn values)
        {
            var response = new Dictionary<string, object>();
            string full_path = null;
            try
            {
                OdbcConnection myConnection = new OdbcConnection();
                myConnection.ConnectionString = objdbconn.GetConnectionString();
                OdbcCommand MyCommand = new OdbcCommand();
                MyCommand.Connection = myConnection;
                DataSet myDS = new DataSet();
                OdbcDataAdapter MyDA = new OdbcDataAdapter();

                msSQL = " select a.grn_gid, date_format(a.grn_date,'%d-%m-%Y ') as grn_date_new,date_format(a.expected_date,'%d-%m-%Y ') as grn_date, a.vendor_contact_person, " +
                                        " a.checkeruser_gid, CASE WHEN j.poref_no IS NOT NULL AND j.poref_no != '' THEN j.poref_no ELSE j.purchaseorder_gid END AS purchaseorder_list , a.grn_remarks, a.grn_reference, a.grn_receipt, " +
                                        " concat(b.user_firstname,' - ',d.department_name) as user_firstname, a.invoice_refno as invoice_ref, " +
                                        " if(date_format(a.dc_date,'%d-%m-%Y')='00-00-0000','',date_format(a.dc_date,'%d-%m-%Y ')) as dc_date," +
                                        " if(date_format(a.invoice_date,'%d-%m-%Y')='00-00-0000','',date_format(a.invoice_date,'%d-%m-%Y ')) as invoice_date, " +
                                        " e.email_id as user_email, c.employee_phoneno as user_phone,e.contact_telephonenumber,e.contactperson_name, " +
                                        " e.vendor_gid, e.vendor_companyname,a.dc_no,e.tin_number,e.cst_number, " +
                                        " concat(f.user_firstname,'  ',f.user_lastname) as user_checkername, " +
                                        " concat(g.user_firstname,'  ',g.user_lastname) as user_approvedby, " +
                                        " h.country_gid, Concat(h.address1,',',h.address2) as address2, h.city, h.state, h.postal_code, h.parent_gid, h.fax,dc_date, invoice_refno, invoice_date, " +
                                        " i.branch_logo_path , (i.branch_location) as branch_footer ," +
                                        " i.authorized_sign_path" +
                                        " from pmr_trn_tgrn a " +
                                        " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                                        " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                                        " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                                        " left join acp_mst_tvendor e on e.vendor_gid = a.vendor_gid " +
                                        " left join adm_mst_tuser g on g.user_gid = a.approved_by " +
                                        " left join pmr_trn_tpurchaseorder j on a.purchaseorder_gid = j.purchaseorder_gid " +
                                         " left join adm_mst_tuser f on f.user_gid = a.checkeruser_gid " +
                                        " left join adm_mst_taddress h on h.address_gid = e.address_gid " +
                                        " left join hrm_mst_tbranch i on a.branch_gid = i.branch_gid " +
                                        " where a.grn_gid = '" + grn_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                dt_datatable.Columns.Add("branch_logo", typeof(byte[]));
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string branch_logo_path = HttpContext.Current.Server.MapPath("../../../" + dt["branch_logo_path"].ToString().Replace("../../", ""));
                        if (File.Exists(branch_logo_path))
                        {
                            Image branch_logo = Image.FromFile(branch_logo_path);
                            dt["branch_logo"] = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                        }
                        else
                        {
                            dt["branch_logo"] = DBNull.Value;
                        }
                    }
                }
                DataTable DataTable1 = dt_datatable;
                DataTable1.TableName = "DataTable1";
                myDS.Tables.Add(DataTable1);

                msSQL = "select a.grn_gid,a.product_remarks,replace(format(a.qty_delivered,2),',','') as qty_delivered,a.qty_billed," +
                                        " a.qty_rejected,replace(format(b.qty_ordered,2),',','') as qty_ordered,a.qty_invoice," +
                                        " replace(format(a.qtyreceivedas,2),',','') as qtyreceivedas,a.display_field," +
                                        " a.product_name,a.product_code, d.productgroup_name, a.productuom_name," +
                                        " replace(format(b.qty_ordered-sum(b.qty_received),2),',','') as qty_balance from pmr_trn_tgrndtl a" +
                                        " left join pmr_trn_tpurchaseorderdtl b on a.purchaseorderdtl_gid=b.purchaseorderdtl_gid" +
                                        " left join pmr_mst_tproduct c on a.product_gid = c.product_gid" +
                                        " left join pmr_mst_tproductgroup d on c.productgroup_gid = d.productgroup_gid" +
                                        " left join pmr_mst_tproductuom e on e.productuom_gid = a.uom_gid" +
                                        " where a.grn_gid='" + grn_gid + "' group by a.grndtl_gid";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable2");

                msSQL = "select a.branch_name,a.address1,a.city,a.state,a.postal_code,a.contact_number,a.email,a.branch_gid,a.branch_logo,a.tin_number,a.cst_number from hrm_mst_tbranch a " +
                                        "left join pmr_trn_tgrn b on a.branch_gid=b.branch_gid " +
                                        "where b.grn_gid='" + grn_gid + "'";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

                msSQL = " select concat(d.user_firstname, ' ',d.user_lastname,' ','/',' ',a.approval_remarks) as approval_remarks  " +
                                        " from pmr_trn_tapproval a left join pmr_trn_tgrn b on a.grnapproval_gid=b.grn_gid " +
                                        " left join hrm_mst_temployee c on a.approved_by=c.employee_gid " +
                                        " left join adm_mst_tuser d on c.user_gid=d.user_gid " +
                                        " where b.grn_gid='" + grn_gid + "' and a.approval_flag='Y' ";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable4");

                msSQL = "select currency_code from adm_mst_tcompany";
                string currency_code = objdbconn.GetExecuteScalar(msSQL);

                try
                {
                    ReportDocument oRpt = new ReportDocument();
                    string base_pathOF_currentFILE = AppDomain.CurrentDomain.BaseDirectory;
                    string report_path = Path.Combine(base_pathOF_currentFILE, "ems.pmr", "Reports", "Pmr_crp_grninward.rpt");

                    if (!File.Exists(report_path))
                    {
                        values.status = false;
                        values.message = "Your Rpt path not found !!";
                        response = new Dictionary<string, object>{
                        {"status",false },
                        {"message",values.message}
                        };
                    }
                    oRpt.Load(report_path);
                    oRpt.SetDataSource(myDS);
                    string path = Path.Combine(ConfigurationManager.AppSettings["report_path"]?.ToString());
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string PDFfile_name = "GRNInward.pdf";
                    full_path = Path.Combine(path, PDFfile_name);

                    oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, full_path);
                    myConnection.Close();
                    response = objFnazurestorage.reportStreamDownload(full_path);
                    values.status = true;
                }
                catch (Exception ex)
                {
                    values.status = false;
                    values.message = ex.Message;
                    response = new Dictionary<string, object>
                {
                     { "status", false },
                     { "message", ex.Message }
                };
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }



        // Upload Image


        public void DaPostGrnSubmitUpload(HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {

                var branchName = httpRequest.Form["branch_name"].ToString();
                var vendor_companyname = httpRequest.Form["vendor_companyname"].ToString();
                var purchaseorder_gid = httpRequest.Form["purchaseorder_gid"].ToString();
                var contactperson_name = httpRequest.Form["contactperson_name"].ToString();
                var dc_no = httpRequest.Form["dc_no"].ToString();
                var grn_date = httpRequest.Form["grn_date"].ToString();
                var expected_date = httpRequest.Form["expected_date"].ToString();
                var contact_telephonenumber = httpRequest.Form["contact_telephonenumber"].ToString();
                var email_id = httpRequest.Form["email_id"].ToString();
                var modeof_dispatch = httpRequest.Form["modeof_dispatch"].ToString();
                var deliverytracking = httpRequest.Form["deliverytracking"].ToString();
                var address = httpRequest.Form["address"].ToString();
                var grn_gid = httpRequest.Form["grn_gid"].ToString();
                var user_firstname = httpRequest.Form["user_firstname"].ToString();
                var user_firstname1 = httpRequest.Form["user_firstname1"].ToString();
                var no_box = httpRequest.Form["no_box"].ToString();
                var received_note = httpRequest.Form["received_note"].ToString();
                var goodsintypes_id = httpRequest.Form["goodsintypes_id"];
                var summary_listJson = httpRequest.Form["summary_list"];

                // Deserialize JSON
                var summary_list = JsonConvert.DeserializeObject<List<SummaryItem>>(summary_listJson);


                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();
                string document_gid = string.Empty;
                string lscompany_code = string.Empty;
                string FileExtensionname = string.Empty;
                HttpPostedFile httpPostedFile;
                string lspath;
                string final_path = "";
                string vessel_name = "";



                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Inventory/AddGrnfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        FileExtensionname = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        lspath = ConfigurationManager.AppSettings["upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Inventory/AddGrnfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ms.Close();
                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "Inventory/AddGrnfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        final_path = lspath + msdocument_gid + FileExtension;
                    }
                }


                foreach (var data in summary_list)
                {
                    lsQty_Delivered = (int)Convert.ToDouble(data.qty_delivered);
                    lsQty_ReceivedAS = (int)Convert.ToDouble(data.qty_received);
                    lsQty_Adjustable = (int)Convert.ToDouble(data.qty_grnadjusted);
                    lsQty_Ordered = (int)Convert.ToDouble(data.qty_ordered);

                    if (lsQty_Ordered < lsQty_ReceivedAS)
                    {
                        objResult.message = "Sum of Qty Received and Qty Received as per Invoice should not be greater than Qty Ordered";
                        objResult.status = false;
                        return;
                    }

                    //msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + data.product_name + "' ";
                    //string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select purchaseorderdtl_gid from pmr_trn_tpurchaseorderdtl where product_gid='" + data.product_gid + "' AND purchaseorder_gid='" + purchaseorder_gid + "' ";
                    string lspurchaseorderdtlgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select producttype_gid from pmr_mst_tproduct where  product_gid='" + data.product_gid + "' ";
                    string lsproducttypegid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select a.product_name, a.product_code, a.productuom_name from pmr_trn_tpurchaseorderdtl a  " +
                            " where a.purchaseorderdtl_gid = '" + lspurchaseorderdtlgid + "' and product_gid = '" + data.product_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lsproductname = objOdbcDataReader["product_name"].ToString();
                        lsproductcode = objOdbcDataReader["product_code"].ToString();
                        lsproductuomname = objOdbcDataReader["productuom_name"].ToString();
                        objOdbcDataReader.Close();
                    }
                    msGetGID = objcmnfunctions.GetMasterGID("PGDC");
                    msSQL = " insert into pmr_trn_tgrndtl (" +
                             " grndtl_gid, " +
                             " grn_gid, " +
                             " purchaseorderdtl_gid, " +
                             " product_gid," +
                             " product_code," +
                             " product_name," +
                             " productuom_name," +
                             " display_field," +
                             " qty_delivered," +
                             " qtyreceivedas," +
                             " producttype_gid, " +
                             " qty_grnadjusted) " +
                             " values (" +
                             "'" + msGetGID + "', " +
                             "'" + grn_gid + "', " +
                             "'" + lspurchaseorderdtlgid + "', " +
                             "'" + data.product_gid + "'," +
                             "'" + lsproductcode + "'," +
                             "'" + data.product_name.Replace("'", "\\\'") + "', " +
                             "'" + data.productuom_name.Replace("'", "\\\'") + "', " +
                             "'" + data.display_field_name.Replace("'", "\\\'") + "', " +
                             "'" + data.qty_received + "'," +
                             "'" + data.qty_received + "'," +
                             "'" + lsproducttypegid + "'," +
                             "'0')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    double lsSum_Rec = data.qty_received;
                    double lsSumPOt_GRNAdj = data.qty_grnadjusted;
                    msSQL = " select qty_received, qty_grnadjusted " +
                            " from pmr_trn_tpurchaseorderdtl  where " +
                            " purchaseorderdtl_gid = '" + lspurchaseorderdtlgid + "' and " +
                            " product_gid = '" + data.product_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsSum_Rec = lsSum_Rec + double.Parse(objOdbcDataReader["qty_received"].ToString());
                        lsSumPOt_GRNAdj = lsSumPOt_GRNAdj + double.Parse(objOdbcDataReader["qty_grnadjusted"].ToString());
                    }
                    msSQL = "UPDATE pmr_trn_tpurchaseorderdtl " +
                            "SET qty_received = '" + lsSum_Rec + "', " +
                            "qty_grnadjusted = '" + lsSumPOt_GRNAdj + "'" +
                            "WHERE purchaseorderdtl_gid = '" + lspurchaseorderdtlgid + "' AND " +
                            "product_gid = '" + data.product_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        objResult.status = false;
                        objResult.message = "Error occured while inserting into Purchaseorder table";
                    }
                    else
                    {
                        lspurchaseorder_status = "PO Completed";
                        lstPO_GRN_flag = "GRN Pending";
                    }

                    msSQL = " SELECT qty_received, qty_grnadjusted, qty_ordered " +
                            " FROM pmr_trn_tpurchaseorderdtl WHERE " +
                            " purchaseorder_gid = '" + purchaseorder_gid + "' AND " +
                            " (qty_received + qty_grnadjusted) < qty_ordered";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        lspurchaseorder_status = "PO Work In Progress";
                        lstPO_GRN_flag = "Goods Received Partial";

                    }
                    else
                    {
                        lspurchaseorder_status = "PO Completed";
                        lstPO_GRN_flag = "Goods Received";
                    }
                    msSQL = " Update pmr_trn_tpurchaseorder " +
                                           " Set purchaseorder_status = '" + lspurchaseorder_status + "'," +
                                           " grn_flag = '" + lstPO_GRN_flag + "'" +
                                           " where purchaseorder_gid = '" + purchaseorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        objResult.status = false;
                        lsgrn_status = "GRN Pending";
                        objResult.message = "Error occured while inserting into Purchaseorder table";
                    }
                    msSQL = "select purchaserequisition_gid from pmr_trn_tpurchaseorder where purchaseorder_gid='" + purchaseorder_gid + "' ";
                    string lspurchaserequisition_gid = objdbconn.GetExecuteScalar(msSQL);
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    {
                        objOdbcDataReader.Read();
                        lspurchaserequisition_gid = objOdbcDataReader["purchaserequisition_gid"].ToString();
                        objOdbcDataReader.Close();
                    }

                    lsPR_Rec = lsQty_ReceivedAS;
                    lsPRt_GRNAdj = lsQty_Adjustable;
                    msSQL = " select qty_received, qty_grnadjusted " +
                                    " from pmr_trn_tpurchaserequisitiondtl where " +
                                    " purchaserequisition_gid = '" + lspurchaserequisition_gid + "' and " +
                                    " product_gid = '" + data.product_gid + "'";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lsPR_Rec = lsPR_Rec + int.Parse(objOdbcDataReader["qty_received"].ToString());
                        lsPRt_GRNAdj = lsPRt_GRNAdj + int.Parse(objOdbcDataReader["qty_grnadjusted"].ToString());
                        objOdbcDataReader.Close();
                    }
                    msSQL = " update pmr_trn_tpurchaserequisitiondtl set " +
                                    " qty_received = '" + lsPR_Rec + "'," +
                                    " qty_grnadjusted = '" + lsPRt_GRNAdj + "'" +
                                    " where purchaserequisition_gid = '" + lspurchaserequisition_gid + "' and " +
                                    " product_gid = '" + data.product_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        objResult.status = false;
                        objResult.message = "Error occured while inserting into Purchase Requisition Detail table";
                    }
                    msSQL = " select qty_received, qty_grnadjusted, qty_requested " +
                                    " from pmr_trn_tpurchaserequisitiondtl  where " +
                                    " purchaserequisition_gid = '" + lspurchaserequisition_gid + "'and" +
                                    " (qty_received + qty_grnadjusted) < qty_requested ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        lstPR_GRN_flag = "Goods Received Partial";
                    }
                    else
                    {
                        lstPR_GRN_flag = "Goods Received";
                    }
                    msSQL = " Update pmr_trn_tpurchaserequisition " +
                                    " Set grn_flag = '" + lstPR_GRN_flag + "'" +
                                    " where purchaserequisition_gid = '" + lspurchaserequisition_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        objResult.status = false;
                        objResult.message = "Error occured while inserting into Purchase Requisition Table";
                    }
                    msSQL = " select branch_gid,vendor_gid from pmr_trn_tpurchaseorder where purchaseorder_gid = '" + purchaseorder_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lblpurchasebranch_gid = objOdbcDataReader["branch_gid"].ToString();
                        lblVendor_gid = objOdbcDataReader["vendor_gid"].ToString();
                        objOdbcDataReader.Close();
                    }
                    msSQL = "select branch_gid from hrm_mst_tbranch where branch_name='" + branchName + "' ";
                    string lsbranchgid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + data.productuom_name + "' ";
                    string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select product_price,display_field_name from pmr_trn_tpurchaseorderdtl where product_gid='" + data.product_gid + "' and purchaseorder_gid='" + purchaseorder_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        lsunit_price = objOdbcDataReader["product_price"].ToString();
                        lsdisplay = objOdbcDataReader["display_field_name"].ToString();
                        objOdbcDataReader.Close();
                    }

                    msSQL = " SELECT product_gid,stock_qty FROM ims_trn_tstock " +
                          " where product_gid = '" + data.product_gid + "' and" +
                          " stocktype_gid = 'SY0905270001' and" +
                          " branch_gid = '" + lsbranchgid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (objOdbcDataReader.HasRows == false)
                    {
                        objOdbcDataReader.Close();
                    }
                    if (dt_datatable.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            string lsstockQty = dt["stock_qty"].ToString();
                            int stockQty = int.Parse(lsstockQty);
                            double OpeningstockQty = data.qty_received;
                            qtyrequested = stockQty + OpeningstockQty;
                        }

                        if (qtyrequested != 0)
                        {
                            finalQty = qtyrequested;
                        }
                        else
                        {
                            finalQty = data.qty_received;
                        }

                        msSQL = " UPDATE ims_trn_tstock " +
                                " SET stock_qty = '" + finalQty + "'," +
                                " display_field='" + lsdisplay + "', " +
                                " created_by='" + user_gid + "'" +
                                " WHERE product_gid = '" + data.product_gid + "' and " +
                                " branch_gid = '" + lsbranchgid + "' and " +
                                " stocktype_gid = 'SY0905270001'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {

                            objResult.status = false;
                            objResult.message = "Error Occured while adding Stock";
                        }
                        else
                        {
                            msSQL = "select mintsoft_flag from adm_mst_tcompany";
                            mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (mintsoft_flag == "Y")
                            {
                                StockToMintSoft OBJMintsoftStock = new StockToMintSoft();

                                msSQL = " select * from smr_trn_tminsoftconfig;";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    objOdbcDataReader.Read();
                                    base_url = objOdbcDataReader["base_url"].ToString();
                                    api_key = objOdbcDataReader["api_key"].ToString();
                                    client_id = objOdbcDataReader["client_id"].ToString();
                                }
                                objOdbcDataReader.Close();
                                msSQL = " select customerproduct_code, mintsoftproduct_id from pmr_mst_tproduct where product_gid='" + data.product_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                                    OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                                }

                                OBJMintsoftStock.Quantity = Convert.ToInt32(finalQty);
                                OBJMintsoftStock.WarehouseId = 3;
                                //string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                // Parse the JSON object
                                JObject jsonObject = JObject.Parse(json);

                                // Create a JArray and add the JObject to it
                                JArray jsonArray = new JArray();
                                jsonArray.Add(jsonObject);

                                // Convert the JArray back to JSON string
                                string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(base_url);
                                var request = new RestRequest("/api/Product/BulkOnHandStockUpdate?ClientId=" + client_id + "", Method.POST);
                                request.AddHeader("ms-apikey", api_key);
                                request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    List<Class1> objResult1 = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
                                    if (objResult1[0].Success)
                                    {
                                        var result = objResult1[0].ID;
                                    }
                                }
                            }
                            objResult.status = true;
                            objResult.message = "Opening Stock Updated Successfully";
                        }

                    }
                    else
                    {
                        msSQL = " select cast(concat(year(fyear_start), '-',  case when fyear_end is not null then year(fyear_end)  when month(curdate())= '1' then year(curdate()) " +
                                " when month(curdate())= '2' then year(curdate())  when month(curdate())= '3' then year(curdate())  else year(date_Add(curdate(), interval 1 year)) end )as char) as finyear " +
                                " from adm_mst_tyearendactivities order by finyear_gid desc ";
                        lsfinyear = objdbconn.GetExecuteScalar(msSQL);

                        msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                        msSQL = "INSERT INTO ims_trn_tstock (" +
                                "stock_gid, " +
                                "branch_gid, " +
                                "product_gid, " +
                                "unit_price, " +
                                "display_field, " +
                                "uom_gid, " +
                                "stock_qty, " +
                                "financial_year, " +
                                "grn_qty, " +
                                "rejected_qty, " +
                                "stocktype_gid, " +
                                "reference_gid, " +
                                "stock_flag, " +
                                "created_date, " +
                                "adjusted_qty) " +
                                "VALUES (" +
                                "'" + msGetStockGID + "', " +
                                "'" + lsbranchgid + "', " +
                                "'" + data.product_gid + "', " +
                                "'" + lsunit_price + "', " +
                                "'" + lsdisplay + "', " +
                                "'" + lsproductuomgid + "', " +
                                "'" + data.qty_received + "', " +
                                "'" + lsfinyear + "', " +
                                "'" + data.qty_received + "', " +
                                "'0', " +
                                "'SY0905270002', " +
                                "'" + grn_gid + "', " +
                                "'Y', " +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                "'0')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            objResult.status = false;
                        }
                        else
                        {
                            msSQL = "select mintsoft_flag from adm_mst_tcompany";
                            mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (mintsoft_flag == "Y")
                            {
                                StockToMintSoft OBJMintsoftStock = new StockToMintSoft();

                                msSQL = " select * from smr_trn_tminsoftconfig;";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    objOdbcDataReader.Read();
                                    base_url = objOdbcDataReader["base_url"].ToString();
                                    api_key = objOdbcDataReader["api_key"].ToString();
                                    client_id = objOdbcDataReader["client_id"].ToString();
                                }
                                objOdbcDataReader.Close();
                                msSQL = " select customerproduct_code, mintsoftproduct_id from pmr_mst_tproduct where product_gid='" + data.product_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                                    OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                                }
                                OBJMintsoftStock.Quantity = Convert.ToInt32(data.qty_received); ;
                                OBJMintsoftStock.WarehouseId = 3;
                                //string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                // Parse the JSON object
                                JObject jsonObject = JObject.Parse(json);

                                // Create a JArray and add the JObject to it
                                JArray jsonArray = new JArray();
                                jsonArray.Add(jsonObject);

                                // Convert the JArray back to JSON string
                                string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(base_url);
                                var request = new RestRequest("/api/Product/BulkOnHandStockUpdate?ClientId=" + client_id + "", Method.POST);
                                request.AddHeader("ms-apikey", api_key);
                                request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    List<Class1> objResult2 = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
                                    if (objResult2[0].Success)
                                    {
                                        var result = objResult2[0].ID;
                                    }
                                }
                            }
                            objResult.status = true;
                            objResult.message = "Opening Stock Added Successfully";
                        }

                    }
                }
                msSQL = "select poapproval_flag from adm_mst_tcompany";
                lsapproval = objdbconn.GetExecuteScalar(msSQL);
                if (lsapproval == "Y")
                {
                    lsgrn_flage = "GRN Pending QC";
                }
                else
                {
                    lsgrn_flage = "GRN Approved";
                }
                string uiDateStr = grn_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                grn_date = uiDate.ToString("yyyy-MM-dd");
                string uiDateStr1 = expected_date;
                DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                expected_date = uiDate1.ToString("yyyy-MM-dd");
                msSQL = "select name from crm_smm_tmintsoftcourierservice where id='" + modeof_dispatch + "'";
                string mode = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "INSERT INTO pmr_trn_tgrn (" +
                            "grn_gid, " +
                            "branch_gid, " +
                            "purchaseorder_gid, " +
                            "grn_date, " +
                            "expected_date, " +
                            "received_note, " +
                            "no_of_boxs, " +
                            "dispatch_mode, " +
                            "deliverytracking_number, " +
                            "vendor_gid, " +
                            "vendor_contact_person, " +
                            "dc_no, " +
                            "invoice_refno, " +
                            "grn_status, " +
                            "grn_flag, " +
                            "grn_remarks, " +
                            "priority, " +
                            "checkeruser_gid, " +
                            "user_gid, " +
                            "created_date, " +
                            "vendor_address, " +
                            "currency_code, " +
                            "invoice_date, " +
                            "file_path, " +
                            "file_name," +
                            "dc_date) " +
                            "VALUES (" +
                            "'" + grn_gid + "', " +
                            "'" + lblpurchasebranch_gid + "', " +
                            "'" + purchaseorder_gid + "', " +
                            "'" + grn_date + "', " +
                            "'" + expected_date + "', " +
                            "'" + received_note.Replace("'", "\\\'") + "', " +
                            "'" + no_box + "', " +
                            "'" + mode + "', " +
                            "'" + deliverytracking + "', " +
                            "'" + lblVendor_gid + "', " +
                            "'" + contactperson_name + "', " +
                            "'" + dc_no + "', " +
                            "'Null', " +
                            "'Invoice Pending', " +
                            "'" + lsgrn_flage + "', " +
                            "'Null', " +
                            "'N', " +
                            "'" + user_gid + "', " +
                            "'" + user_gid + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                            "'" + address + "', " +
                            "'INR', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                            "'" + final_path + "'," +
                            "'" + FileExtensionname + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    objResult.status = false;
                }
                msSQL = "select mintsoft_flag from adm_mst_tcompany";
                mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                if (mintsoft_flag == "Y")
                {
                    result objresult = new result();
                    ASNList objMdlMintsoftJSON = new ASNList();
                    PMRASNSTOCK_list OBJMintsoftStock = new PMRASNSTOCK_list();
                    msSQL = " select * from smr_trn_tminsoftconfig;";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        objOdbcDataReader.Read();
                        base_url = objOdbcDataReader["base_url"].ToString();
                        api_key = objOdbcDataReader["api_key"].ToString();
                        client_id = objOdbcDataReader["client_id"].ToString();
                    }
                    objOdbcDataReader.Close();

                    msSQL = " select goodsintypes_name from crm_smm_tmintsoftasngoodsintypes where goodsintypes_id='" + goodsintypes_id + "'";
                    string goodsintypes_name = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select supplier_id from acP_mst_tvendor where  vendor_companyname='" + vendor_companyname + "'";
                    string supplier_id = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select a.expected_date, b.poref_no from pmr_trn_tgrn a  " +
                        " left join pmr_trn_tpurchaseorder b on b.purchaseorder_gid = a.purchaseorder_gid " +
                        " where grn_gid ='" + grn_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        string expecteddate = objOdbcDataReader["expected_date"].ToString() + DateTimeOffset.Now.ToString("THH:mm:ss.fffffffK");
                        objMdlMintsoftJSON.WarehouseId = 3;
                        objMdlMintsoftJSON.Supplier = vendor_companyname;
                        objMdlMintsoftJSON.POReference = objOdbcDataReader["poref_no"].ToString();
                        objMdlMintsoftJSON.EstimatedDelivery = expecteddate;
                        objMdlMintsoftJSON.GoodsInType = goodsintypes_name;
                        objMdlMintsoftJSON.ProductSupplierId = int.Parse(supplier_id);
                    }

                    msSQL = "select b.customerproduct_code,a.qtyreceivedas,b.mintsoftproduct_id, d.expected_date" +
                            " from pmr_trn_tgrndtl a " +
                            " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                            " left join pmr_trn_tgrn d on a.grn_gid=d.grn_gid" +
                            " where a.grn_gid = '" + grn_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count > 0)
                    {
                        int i = 0;
                        objMdlMintsoftJSON.Items = new AsnItem[dt_datatable.Rows.Count];

                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            objMdlMintsoftJSON.Items[i] = new AsnItem();
                            objMdlMintsoftJSON.Items[i].ProductId = dt["mintsoftproduct_id"].ToString();
                            objMdlMintsoftJSON.Items[i].SKU = dt["customerproduct_code"].ToString();
                            objMdlMintsoftJSON.Items[i].Quantity = int.Parse(dt["qtyreceivedas"].ToString());
                            objMdlMintsoftJSON.Items[i].ExpiryDate = dt["expected_date"].ToString() + DateTimeOffset.Now.ToString("THH:mm:ss.fffffffK");
                            i++;
                        }
                        dt_datatable.Dispose();
                    }
                    string json = JsonConvert.SerializeObject(objMdlMintsoftJSON);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(base_url);
                    var request = new RestRequest("/api/ASN", Method.PUT);
                    request.AddHeader("ms-apikey", api_key);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", json, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseData = JsonConvert.DeserializeObject<MintsoftASNResponse>(response.Content);
                        msSQL = "update pmr_trn_tgrn set mintsoftasn_id = '" + responseData.ID + "' where grn_gid ='" + grn_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            objresult.status = true;
                            objresult.message = "ASN Added Successfully, ASN ID:" + responseData.ID + "";
                            //    var client1 = new RestClient(base_url);
                            //    var request1 = new RestRequest("/api/ASN/" + responseData.ID + "/Confirm", Method.GET);
                            //    request.AddHeader("Accept", "application/json");
                            //    request.AddHeader("ms-apikey", api_key);
                            //    IRestResponse response1 = client.Execute(request1);
                            //    if (response1.StatusCode == HttpStatusCode.OK)
                            //    {
                            //        objresult.status = true;
                            //        objresult.message = "ASN Added Successfully, ASN ID:" + responseData.ID + "";
                            //    }
                            }
                        }
                    }
                msSQL = "select employee_gid from hrm_mst_temployee where user_gid =  '" + user_gid + "'";
                string employee_gid = objdbconn.GetExecuteScalar(msSQL);

                msGetGID = objcmnfunctions.GetMasterGID("PODC");

                msSQL = "insert into pmr_trn_tapproval ( " +
                            " approval_gid, " +
                            " approved_by, " +
                            " approved_date, " +
                            " submodule_gid, " +
                            " grnapproval_gid " +
                            " ) values ( " +
                            "'" + msGetGID + "'," +
                            " '" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'PMRSTKGRA'," +
                            "'" + grn_gid + "') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    objResult.status = false;
                }
                msSQL = " Delete from pmr_tmp_tgrn where user_gid = '" + user_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    objResult.status = true;
                    objResult.message = "GRN Raised Successfully";

                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Raising GRN";
                }


            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Raising GRN!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                   ex.Message.ToString() + "***********" + objResult.message.ToString() + "*****Query****" +
                   msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                   DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public class MintsoftASNResponse1
        {
            public string ID { get; set; }
            public string Success { get; set; }
            public string Message { get; set; }
            public string WarningMessage { get; set; }
            public string AllocatedFromReplen { get; set; }
        }

        public void DaGetGRNViewProduct(string grn_gid, MdlPmrTrnGrn values)
        {
            try
            {

                msSQL = " SELECT a.grn_gid,a.grndtl_gid, a.product_gid,a.qtyreceivedas,e.qty_ordered,d.productuom_name,  " +
                        " a.qty_delivered, a.product_name,a.product_code, c.productgroup_name " +
                        " FROM pmr_trn_tgrndtl a " +
                        " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                        " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on d.productuom_gid = b.productuom_gid " +
                        " left join pmr_trn_tpurchaseorderdtl e on e.purchaseorderdtl_gid = a.purchaseorderdtl_gid " +
                        " where a.grn_gid = '"+ grn_gid + "'" +
                        " order by a.purchaseorderdtl_gid asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<grnproduct_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new grnproduct_lists
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            grndtl_gid = dt["grndtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            qty_delivered = dt["qtyreceivedas"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                        });
                        values.grnproduct_lists = getModuleList;
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
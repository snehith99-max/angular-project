using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Shared.Json;
using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaPurchaseReturn
    {
        string msSQL, lsbranch_gid, lsbranch_name,
        lsdepartment_gid, lsmainbranch, lsvendor_companyname, msGetDtlGID, msGetGID1
        = string.Empty;
        string lstPO_GRN_flag, lspurchaseorder_status, lspurchaserequisitiondtl_gid, lstPR_GRN_flag
            , ls_referenceno = string.Empty;
        string qty_returned, product_gid, grndtl_gid, purchaserequisition_gid, purchaseorder_gid = string.Empty;
        int mnResult;
        double lsGRN_Sum_Returned, lsPO_Sum_Returned, lsPO_Sum_Received, lsPR_Received;
        DataSet dt_set;
        DataTable dt_table;
        OdbcDataReader objodbcdatareader;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();
        Image branch_logo;
        string company_logo_path, authorized_sign_path;
        Image company_logo;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataTable DataTable4 = new DataTable();



        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        public void DaGetPurchaseReturnSummary(MdlPurchaseReturn values)
        {
            msSQL = " select distinct a.purchasereturn_gid, c.branch_name,c.branch_prefix, date_format(a.purchasereturn_date,'%d-%m-%Y') as purchasereturn_date, a.purchasereturn_reference," +
                    " concat(b.contactperson_name,' / ',b.contact_telephonenumber,' / ',b.email_id)  as contact," +
                    " a.grn_gid, b.vendor_companyname from pmr_trn_tpurchasereturn a " +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    " left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid where 1=1 " +
                    " order by date(a.purchasereturn_date) desc,a.purchasereturn_date asc, a.purchasereturn_gid desc ";
            dt_set = objdbconn.GetDataSet(msSQL, "pmr_trn_tpurchasereturn");
            var GetPurchaseReturn = new List<GetPurchaseReturn_list>();
            if (dt_set.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow ds in dt_set.Tables[0].Rows)
                {
                    GetPurchaseReturn.Add(new GetPurchaseReturn_list
                    {
                        purchasereturn_gid = ds["purchasereturn_gid"].ToString(),
                        branch_name = ds["branch_name"].ToString(),
                        branch_prefix = ds["branch_prefix"].ToString(),
                        purchasereturn_date = ds["purchasereturn_date"].ToString(),
                        purchasereturn_reference = ds["purchasereturn_reference"].ToString(),
                        grn_gid = ds["grn_gid"].ToString(),
                        vendor_companyname = ds["vendor_companyname"].ToString(),
                        contact = ds["contact"].ToString(),
                    });
                    values.GetPurchaseReturn_list = GetPurchaseReturn;
                }
            }
        }
        public void DaGetBranchPurchaseReturn(string user_gid, MdlPurchaseReturn values)
        {
            try
            {
                msSQL = " call hrm_mst_spemployee('query2', '" + user_gid + "','','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lsbranch_gid = objodbcdatareader["branch_gid"].ToString();
                    lsdepartment_gid = objodbcdatareader["department_gid"].ToString();
                    objodbcdatareader.Close();
                }
                msSQL = " call hrm_mst_spbranch('query1','" + lsbranch_gid + "','','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lsmainbranch = objodbcdatareader["mainbranch_flag"].ToString();
                    objodbcdatareader.Close();
                }
                msSQL = "SELECT branch_gid, branch_name FROM hrm_mst_tbranch";
                if (lsmainbranch == "Y")
                {
                    msSQL += " where branch_gid='" + lsbranch_gid + "'";
                }
                dt_table = objdbconn.GetDataTable(msSQL);
                var Getbranch = new List<GetPurchaseReturnbranch_list>();
                if (dt_table.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_table.Rows)
                    {
                        Getbranch.Add(new GetPurchaseReturnbranch_list
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.GetPurchaseReturnbranch_list = Getbranch;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetPurchaseReturnGRN(string branch_gid, MdlPurchaseReturn values)
        {
            try
            {
                msSQL = " call pmr_trn_spgrn('query13','" + branch_gid + "','','')";
                dt_table = objdbconn.GetDataTable(msSQL);
                var GetGRNPurchaseReturn = new List<GetGRNPurchaseReturn_list>();
                if (dt_table.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_table.Rows)
                    {
                        GetGRNPurchaseReturn.Add(new GetGRNPurchaseReturn_list
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            grn_status = dt["grn_status"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            grn_date = dt["grn_date"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                        });
                        values.GetGRNPurchaseReturn_list = GetGRNPurchaseReturn;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetPurchaseReturnDetailsSummary(string vendor_gid, string grn_gid, string user_gid, MdlPurchaseReturn values)
        {
            try
            {
                msSQL = " call adm_mst_spuser('query9','" + user_gid + "','','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lsbranch_gid = objodbcdatareader["branch_gid"].ToString();
                    lsbranch_name = objodbcdatareader["branch_name"].ToString();
                    objodbcdatareader.Close();
                }
                msSQL = " call acp_mst_spvendor('query13','" + vendor_gid + "','','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lsvendor_companyname = objodbcdatareader["vendor_companyname"].ToString();
                    objodbcdatareader.Close();

                }
                msSQL = "select qty_rejected from pmr_trn_tgrndtl where grn_gid = '" + grn_gid + "'";
                string lsqty_rejected=objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select distinct a.grn_gid, a.purchaseorder_gid, b.product_gid, c.product_name," +
                    "c.product_code, d.purchaserequisition_gid,b.purchaseorderdtl_gid, " +
                " b.qty_delivered, b.qty_rejected, b.qty_returned, b.grndtl_gid " +
                " from pmr_trn_tgrn a " +
                " left join pmr_trn_tgrndtl b on b.grn_gid = a.grn_gid " +
                " left join pmr_mst_tproduct c on c.product_gid = b.product_gid " +
                " left join pmr_trn_tpurchaseorder d on a.purchaseorder_gid = d.purchaseorder_gid " +
                " where a.grn_gid = '" + grn_gid + "'";
                if (lsqty_rejected != "0")
                {
                    msSQL += "and b.qty_rejected <> b.qty_returned ";
                }
                msSQL+=" order by a.grn_gid desc ";
                dt_set = objdbconn.GetDataSet(msSQL, "pmr_trn_tgrn");
                var GetGRNDetailsSummary = new List<GetGRNDetailsSummary_list>();
                if (dt_set.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in dt_set.Tables[0].Rows)
                    {
                        GetGRNDetailsSummary.Add(new GetGRNDetailsSummary_list
                        {
                            grn_gid = ds["grn_gid"].ToString(),
                            purchaseorder_gid = ds["purchaseorder_gid"].ToString(),
                            product_gid = ds["product_gid"].ToString(),
                            product_name = ds["product_name"].ToString(),
                            product_code = ds["product_code"].ToString(),
                            purchaserequisition_gid = ds["purchaserequisition_gid"].ToString(),
                            purchaseorderdtl_gid = ds["purchaseorderdtl_gid"].ToString(),
                            qty_delivered = ds["qty_delivered"].ToString(),
                            qty_rejected = ds["qty_rejected"].ToString(),
                            qty_returned = ds["qty_returned"].ToString(),
                            grndtl_gid = ds["grndtl_gid"].ToString(),
                            qty_purchasereturn = 0,
                            product_remarks = "",
                        });
                        values.GetGRNDetailsSummary_list = GetGRNDetailsSummary;
                    }
                }
                values.branch_name = lsbranch_name;
                values.branch_gid = lsbranch_gid;
                values.vendor_companyname = lsvendor_companyname;
            }
            catch (Exception ex)
            {

            }
        }
        public void DaPostPurchaseReturn(string user_gid, PostPurchaseReturn_list values)
        {
            try
            {
                for (int i = 0; i < values.purchaseQTY.ToArray().Length; i++)
                {
                    msGetDtlGID = objcmnfunctions.GetMasterGID("PRDC");
                    msGetGID1 = objcmnfunctions.GetMasterGID("PRMDC");

                    msSQL = " insert into pmr_trn_tpurchasereturndtl (" +
                        " purchasereturndtl_gid, " +
                        " purchasereturn_gid, " +
                        " grndtl_gid, " +
                        " product_gid, " +
                        " qty_returned, " +
                        " product_remarks)" +
                        " values ( " +
                        "'" + msGetDtlGID + "', " +
                        "'" + msGetGID1 + "', " +
                        "'" + values.purchaseQTY[i].grndtl_gid + "'," +
                        "'" + values.purchaseQTY[i].product_gid + "'," +
                        "'" + values.purchaseQTY[i].qty_purchasereturn + "'," +
                        "'" + values.purchaseQTY[i].product_remarks.Replace("'", "\\\'") + "'" +
                        ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " call pmr_trn_spgrndtl('query12','" + values.purchaseQTY[i].grndtl_gid + "','','')";
                    objodbcdatareader = objdbconn.GetDataReader(msSQL);
                    if (objodbcdatareader.HasRows == true)
                    {
                        objodbcdatareader.Read();
                        lsGRN_Sum_Returned = Convert.ToInt32(values.purchaseQTY[i].qty_purchasereturn) + Convert.ToInt32(objodbcdatareader["qty_returned"].ToString().Replace(".00",""));
                    }
                    msSQL = " Update pmr_trn_tgrndtl set " +
                        " qty_returned = '" + lsGRN_Sum_Returned + "'" +
                        " where grndtl_gid = '" + values.purchaseQTY[i].grndtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " select qty_received " +
                        " from pmr_trn_tpurchaseorderdtl  where " +
                        " purchaseorderdtl_gid = '" + values.purchaseQTY[i].purchaseorderdtl_gid + "' and " +
                        " product_gid = '" + values.purchaseQTY[i].product_gid + "'";
                    objodbcdatareader = objdbconn.GetDataReader(msSQL);
                    if (objodbcdatareader.HasRows == true)
                    {
                        objodbcdatareader.Read();
                        string lsqty_received = objodbcdatareader["qty_received"].ToString().Replace(".00", "");
                lsPO_Sum_Received = Convert.ToInt32(lsqty_received) - Convert.ToInt32(values.purchaseQTY[i].qty_purchasereturn);
                    }
                    msSQL = " update pmr_trn_tpurchaseorderdtl " +
                        " Set qty_received = '" + lsPO_Sum_Received + "'" +
                        " where purchaseorderdtl_gid = '" + values.purchaseQTY[i].purchaseorderdtl_gid + "' and " +
                        " product_gid = '" + values.purchaseQTY[i].product_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " call pmr_trn_sppurchaseorderdtl('query5','" + values.purchaseQTY[i].purchaseorder_gid + "','','')";
                    objodbcdatareader = objdbconn.GetDataReader(msSQL);
                    if (objodbcdatareader.HasRows == true)
                    {
                        objodbcdatareader.Read();
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
                        " where purchaseorder_gid = '" + values.purchaseQTY[i].purchaseorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "select purchaserequisitiondtl_gid from pmr_trn_tpurchaserequisitiondtl a" +
                        " left join pmr_trn_tpurchaseorderdtl b on a.product_gid=b.product_gid " +
                        "where a.purchaserequisition_gid = '" + values.purchaseQTY[i].purchaserequisition_gid + "' and" +
                        " b.purchaseorder_gid = '" + values.purchaseQTY[i].purchaseorder_gid + "' " +
                        "and b.product_gid='" + values.purchaseQTY[i].product_gid + "' and b.qty_ordered = a.qty_requested";
                    objodbcdatareader = objdbconn.GetDataReader(msSQL);
                    if (objodbcdatareader.HasRows == true)
                    {
                        objodbcdatareader.Read();
                        lspurchaserequisitiondtl_gid = objodbcdatareader["purchaserequisitiondtl_gid"].ToString();
                    }
                    msSQL = " select qty_received " +
                       " from pmr_trn_tpurchaserequisitiondtl where " +
                       " purchaserequisitiondtl_gid = '" + lspurchaserequisitiondtl_gid + "' and " +
                       " product_gid = '" + values.purchaseQTY[i].product_gid + "'";
                    objodbcdatareader = objdbconn.GetDataReader(msSQL);
                    if (objodbcdatareader.HasRows == true)
                    {
                        objodbcdatareader.Read();
                        string lsqty_received = objodbcdatareader["qty_received"].ToString().Replace(".00", "");
                lsPR_Received = Convert.ToInt32(lsqty_received) - Convert.ToInt32(values.purchaseQTY[i].qty_purchasereturn);
                    }
                    msSQL = " update pmr_trn_tpurchaserequisitiondtl set " +
                        " qty_received = '" + lsPO_Sum_Received + "'" +
                        " where purchaserequisitiondtl_gid = '" + lspurchaserequisitiondtl_gid + "' and " +
                        " product_gid = '" + values.purchaseQTY[i].product_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " select qty_received, qty_grnadjusted, qty_requested " +
                      " from pmr_trn_tpurchaserequisitiondtl  where " +
                      " purchaserequisition_gid = '" + values.purchaseQTY[i].purchaserequisition_gid + "'and" +
                      " (qty_received + qty_grnadjusted) < qty_requested ";
                    objodbcdatareader = objdbconn.GetDataReader(msSQL);
                    if (objodbcdatareader.HasRows == true)
                    {
                        objodbcdatareader.Read();
                        lstPR_GRN_flag = "Goods Received Partial";
                    }
                    else
                    {
                        lstPR_GRN_flag = "Goods Received";
                    }
                    msSQL = " Update pmr_trn_tpurchaserequisition " +
                        " Set grn_flag = '" + lstPR_GRN_flag + "'" +
                        " where purchaserequisition_gid = '" + values.purchaseQTY[i].purchaserequisition_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);                    
                }
                ls_referenceno = objcmnfunctions.GetMasterGID("SOTR");

                DateTime lspurchasereturn_date = DateTime.ParseExact(values.purchasereturn_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string purchasereturn_date = lspurchasereturn_date.ToString("yyyy-MM-dd");

                msSQL = " insert into pmr_trn_tpurchasereturn (" +
                        " purchasereturn_gid, " +
                        " branch_gid, " +
                        " vendor_gid, " +
                        " grn_gid, " +
                        " purchasereturn_date, " +
                        " purchasereturn_reference, " +
                        " purchasereturn_remarks, " +
                        " user_gid, " +
                        " created_date)" +
                        " values ( " +
                        "'" + msGetGID1 + "', " +
                        "'" + values.branch_gid + "', " +
                        "'" + values.vendor_gid + "', " +
                        "'" + values.grn_gid + "', " +
                        "'" + purchasereturn_date + "', " +
                        "'" + ls_referenceno + "', " +
                        "'" + values.remarks.Replace("'", "\\\'") + "'," +
                        "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                        ")";
                 mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " Purchase Return added successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while add purchase return.";
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetPurchaseReturnView(string purchasereturn_gid, MdlPurchaseReturn values)
        {
            try
            {
                msSQL = " call pmr_trn_sppurchasereturn('query2','" + purchasereturn_gid + "','','')";
                dt_set = objdbconn.GetDataSet(msSQL, "pmr_trn_tpurchasereturn");
                var GetPurchaseReturnView = new List<GetPurchaseReturnView_list>();
                if (dt_set.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_set.Tables[0].Rows)
                    {
                        GetPurchaseReturnView.Add(new GetPurchaseReturnView_list
                        {
                            purchasereturn_remarks= dt["purchasereturn_remarks"].ToString(),
                            purchasereturn_reference = dt["purchasereturn_reference"].ToString(),
                            purchasereturn_date = dt["purchasereturn_date"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.GetPurchaseReturnView_list = GetPurchaseReturnView;
                    }
                }
            }
            catch (Exception ex) 
            { 

            }
        }
        public void DaGetPurchaseReturnViewDetails(string purchasereturn_gid, MdlPurchaseReturn values)
        {
            try
            {
                msSQL = " call pmr_trn_sppurchasereturn('query3','" + purchasereturn_gid + "','','')";
                dt_set = objdbconn.GetDataSet(msSQL, "pmr_trn_tpurchasereturn");
                var GetPurchaseReturnViewDetails = new List<GetPurchaseReturnViewDetails_list>();
                if (dt_set.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_set.Tables[0].Rows)
                    {
                        GetPurchaseReturnViewDetails.Add(new GetPurchaseReturnViewDetails_list
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            qty_delivered = dt["qty_delivered"].ToString(),
                            qty_rejected = dt["qty_rejected"].ToString(),
                            qty_returned = dt["qty_returned"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                        });
                        values.GetPurchaseReturnViewDetails_list = GetPurchaseReturnViewDetails;
                    }
                }
            }
            catch (Exception ex) 
            { 

            }
        }
        public void DaPurchaseReturnCancel(string purchasereturn_gid, MdlPurchaseReturn values)
        {
            try
            {
                msSQL = " select c.grn_gid, f.purchaseorder_gid, a.product_gid, e.product_name, f.purchaserequisition_gid, a.product_remarks, " +
                        " c.qty_delivered, c.qty_rejected, c.grndtl_gid, a.qty_returned " +
                        " from pmr_trn_tpurchasereturndtl a " +
                        " INNER join pmr_trn_tpurchasereturn b on a.purchasereturn_gid = b.purchasereturn_gid " +
                        " INNER join pmr_trn_tgrndtl c on a.grndtl_gid = c.grndtl_gid " +
                        " INNER join pmr_trn_tgrn d on d.grn_gid = c.grn_gid " +
                        " INNER join pmr_mst_tproduct e on e.product_gid = a.product_gid " +
                        " INNER join pmr_trn_tpurchaseorder f on d.purchaseorder_gid = f.purchaseorder_gid " +
                        " where a.purchasereturn_gid =  '" + purchasereturn_gid + "'";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    qty_returned = objodbcdatareader["qty_returned"].ToString();
                    grndtl_gid = objodbcdatareader["grndtl_gid"].ToString();
                    product_gid = objodbcdatareader["product_gid"].ToString();
                    purchaseorder_gid = objodbcdatareader["purchaseorder_gid"].ToString();
                    purchaserequisition_gid = objodbcdatareader["purchaserequisition_gid"].ToString();
                    objodbcdatareader.Close();
                }
                msSQL = " call pmr_trn_spgrndtl('query12','" + grndtl_gid + "','','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lsGRN_Sum_Returned = Convert.ToInt32(objodbcdatareader["qty_returned"].ToString()) - Convert.ToInt32(qty_returned);
                    objodbcdatareader.Close();
                }
                msSQL = " Update pmr_trn_tgrndtl set " +
                            " qty_returned = '" + lsGRN_Sum_Returned + "'" +
                            " where grndtl_gid = '" + grndtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " call pmr_trn_sppurchaseorderdtl('query18','" + purchaseorder_gid + "','" + product_gid + "','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lsPO_Sum_Returned = Convert.ToInt32(objodbcdatareader["qty_received"].ToString()) - Convert.ToInt32(qty_returned);
                    objodbcdatareader.Close();
                }
                msSQL = " update pmr_trn_tpurchaseorderdtl " +
                            " Set qty_received = '" + lsPO_Sum_Returned + "'" +
                            " where purchaseorder_gid = '" + purchaseorder_gid + "' and " +
                            " product_gid = '" + product_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " call pmr_trn_sppurchaseorderdtl('query5','" + product_gid + "','','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lspurchaseorder_status = "PO Work In Progress";
                    lstPO_GRN_flag = "Goods Received Partial";
                    objodbcdatareader.Close();
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

                msSQL = " call pmr_trn_sppurchaserequisitiondtl('query14','" + purchaseorder_gid + "','" + product_gid + " ','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lsPR_Received = Convert.ToInt32(objodbcdatareader["qty_received"].ToString()) - Convert.ToInt32(qty_returned);
                    objodbcdatareader.Close();
                }

                msSQL = " update pmr_trn_tpurchaserequisitiondtl set " +
                            " qty_received = '" + lsPR_Received + "'" +
                            " where purchaserequisition_gid = '" + purchaserequisition_gid + "' and " +
                            " product_gid = '" + product_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " call pmr_trn_sppurchaserequisitiondtl('query8','" + purchaserequisition_gid + "','','')";
                objodbcdatareader = objdbconn.GetDataReader(msSQL);
                if (objodbcdatareader.HasRows == true)
                {
                    objodbcdatareader.Read();
                    lstPR_GRN_flag = "Goods Received Partial";
                    objodbcdatareader.Close();
                }
                else
                {
                    lstPR_GRN_flag = "Goods Received";
                }
                msSQL = " Update pmr_trn_tpurchaserequisition " +
                            " Set grn_flag = '" + lstPR_GRN_flag + "'" +
                            " where purchaserequisition_gid = '" + purchaserequisition_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pmr_trn_tpurchasereturndtl where " +
                        " purchasereturn_gid = '" + purchasereturn_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult==1)
                {
                    msSQL = " delete from pmr_trn_tpurchasereturn where " +
                             " purchasereturn_gid = '" + purchasereturn_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Purchase Return Cancelled Successfully.";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while cancelling the purchase return.";
                    }
                }                
            }
            catch (Exception ex)
            {

            }
        }
        public Dictionary<string, object> DaGetPurchaseReturnRpt(string purchasereturn_gid, string branch_gid, MdlPurchaseReturn values)

        {
            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = " select vendor_companyname,contactperson_name, contact_telephonenumber,address1, address2, city, state, postal_code,country_name,a.created_date, e.grn_gid, " +
                    " e.purchaseorder_gid, concat(f.user_firstname,' - ',f.user_lastname) as user_firstname from pmr_Trn_tpurchasereturn a" +
                    " left join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid " +
                    " left join adm_mst_taddress c on c.address_gid=b.address_gid " +
                    " left join adm_mst_tcountry d on d.country_gid=c.country_gid " +
                    " left join pmr_trn_tgrn e on e.grn_gid=a.grn_gid " +
                    " left join adm_mst_tuser f on f.user_gid=a.user_gid " +
                    " WHERE a.purchasereturn_gid='" + purchasereturn_gid + "' " +
                    " group by a.purchasereturn_gid ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = " select company_name, company_address, company_phone, company_mail,contact_person from adm_mst_tcompany ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");


            msSQL = " select  d.product_name, c.qty_delivered, c.qty_rejected, b.qty_returned, d.Product_code, e.display_field_name" +
                    " from pmr_trn_tpurchasereturn a " +
                    " inner join pmr_trn_tpurchasereturndtl b on a.purchasereturn_gid = b.purchasereturn_gid " +
                    " left join pmr_trn_tgrndtl c on b.grndtl_gid = c.grndtl_gid " +
                    " inner join pmr_mst_tproduct d on d.product_gid = b.product_gid " +
                    " inner join pmr_trn_tpurchaseorderdtl e on c.purchaseorderdtl_gid=e.purchaseorderdtl_gid " +
                    " WHERE a.purchasereturn_gid='" + purchasereturn_gid + "'";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3");

            msSQL = "select  (branch_logo_path) as company_logo  from hrm_mst_tbranch where branch_gid = '" + branch_gid + "' and branch_logo_path is not null";
            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable4.Columns.Add("company_logo", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo"].ToString().Replace("../../", ""));

                    if (System.IO.File.Exists(company_logo_path))
                    {
                        //Convert  Image Path to Byte
                        company_logo = System.Drawing.Image.FromFile(company_logo_path);
                        byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));
                        DataRow newRow = DataTable4.NewRow();
                        newRow["company_logo"] = branch_logo_bytes;

                        DataTable4.Rows.Add(newRow);
                    }
                }
            }
            dt1.Dispose();
            DataTable4.TableName = "DataTable4";
            myDS.Tables.Add(DataTable4);

            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_purchasereturn"].ToString(), "ims_purchase_return.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Purchase Return_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();
            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;

        }

        public void DaGetViewSRProduct(string purchasereturn_gid, MdlPurchaseReturn values)
        {
            try
            {

                msSQL = " select f.productgroup_name, d.product_name, d.product_code,e.productuom_name,c.display_field, " +
                        " c.qty_delivered, c.qty_rejected, b.qty_returned " +
                        " from pmr_trn_tpurchasereturn a " +
                        " inner join pmr_trn_tpurchasereturndtl b on a.purchasereturn_gid = b.purchasereturn_gid " +
                        " left join pmr_trn_tgrndtl c on b.grndtl_gid = c.grndtl_gid "  +
                        " inner join pmr_mst_tproduct d on d.product_gid = b.product_gid " +
                        " left join pmr_mst_tproductuom e on d.productuom_gid = e.productuom_gid " +
                        " left join pmr_mst_tproductgroup f on f.productgroup_gid = d.productgroup_gid" +
                        " where a.purchasereturn_gid = '" + purchasereturn_gid + "'";
                dt_table = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getProduct_list>();
                if (dt_table.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_table.Rows)
                    {
                        getModuleList.Add(new getProduct_list
                        {
                            qty_returned = dt["qty_returned"].ToString(),
                            qty_rejected = dt["qty_rejected"].ToString(),
                            qty_delivered = dt["qty_delivered"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_remarks = dt["display_field"].ToString(),
                        });
                        values.getProduct_list = getModuleList;
                    }
                }
                dt_table.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue Request Data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


    }
}
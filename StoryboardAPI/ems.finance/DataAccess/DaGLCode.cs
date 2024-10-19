using ems.finance.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Web;
using System.Linq;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using OfficeOpenXml.Style;
using System.Net.Mail;
using System.Web.DynamicData;

namespace ems.finance.DataAccess
{
    public class DaGLCode
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader, objODBCDatareader1;
        DataTable dt_datatable, dt_datatable1;
        string msGetGid, msGetGid1, msGetGid2, mscusconGetGID, msleadbankGetGID, msleadbankconGetGID,msGetGidfil, lsmodule_name, lssalestype_gid, lssalestype_name, lsasset_status, lsproduct_name, lsassetdtl_gid, lssasset_status;
        int mnResult;

        public void DaGetDebtorSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " Select distinct a.customer_gid,a.customer_id as customerref_no,a.customer_code,a.customer_name, " +
                    " a.company_website as website,a.external_gl_code," +
                    " concat(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) as contact_details, " +
                    " concat(d.region_name, ' / ', a.customer_city, ' / ', a.customer_state) as region_name," +
                    " a.gl_code,a.gl_code_flag " +
                    " from crm_mst_tcustomer a " +
                    " left join crm_mst_tregion d on a.customer_region = d.region_gid " +
                    " left join crm_mst_tcustomercontact c on a.customer_gid = c.customer_gid " +
                    " where a.account_gid in " +
                    " (select b.account_gid from acc_trn_journalentrydtl b where b.account_gid is not null) "+
                    " group by a.customer_gid Order by a.customer_name asc";           
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var DebtorSummary_List = new List<GetDebtorSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    DebtorSummary_List.Add(new GetDebtorSummary_List
                    {
                        customerref_no = dt["customerref_no"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        customer_code = dt["customer_code"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        website = dt["website"].ToString(),
                        external_gl_code = dt["external_gl_code"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        gl_code = dt["gl_code"].ToString(),
                        gl_code_flag = dt["gl_code_flag"].ToString()
                    });
                    values.GetDebtorSummary_List = DebtorSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetTaxSegmentMapping(MdlGLCode values)
        {
            try
            {

                msSQL = " select a.taxsegment_gid,a.taxsegment_name,a.taxsegment_code,a.reference_type,a.account_name, a.active_flag,a.created_by,a.created_date from acp_mst_ttaxsegment a ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<taxsegmentmapping_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new taxsegmentmapping_list
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_code = dt["taxsegment_code"].ToString(),
                            reference_type = dt["reference_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),

                        });
                        values.taxsegmentmapping_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetCreditorSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " Select vendor_gid, vendor_code, " + 
                    " vendor_companyname, contactperson_name, " +
                    " contact_telephonenumber,gl_code,external_gl_code,gl_code_flag " +
                    " from acp_mst_tvendor where account_gid is not null " +
                    " Order by vendorregister_gid desc";           
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var CreditorSummary_List = new List<GetCreditorSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CreditorSummary_List.Add(new GetCreditorSummary_List
                    {
                        vendor_gid = dt["vendor_gid"].ToString(),
                        vendor_code = dt["vendor_code"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        contactperson_name = dt["contactperson_name"].ToString(),
                        contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                        gl_code = dt["gl_code"].ToString(),
                        external_gl_code = dt["external_gl_code"].ToString(),
                        gl_code_flag = dt["gl_code_flag"].ToString()
                    });
                    values.GetCreditorSummary_List = CreditorSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetTaxPayableSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " Select tax_gid,tax_name,external_gl_code,percentage,reference_type,gl_code,gl_code_flag" +
                    " from acp_mst_ttax" +
                    " where 1 = 1 order by tax_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var TaxPayableSummary_List = new List<GetTaxPayableSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    TaxPayableSummary_List.Add(new GetTaxPayableSummary_List
                    {
                        tax_gid = dt["tax_gid"].ToString(),
                        tax_name = dt["tax_name"].ToString(),
                        external_gl_code = dt["external_gl_code"].ToString(),
                        percentage = dt["percentage"].ToString(),
                        gl_code = dt["gl_code"].ToString(),
                        reference_type = dt["reference_type"].ToString(),
                        gl_code_flag = dt["gl_code_flag"].ToString()
                    });
                    values.GetTaxPayableSummary_List = TaxPayableSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAssetDtlsSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " select assetgl_code from ams_trn_tassetdtl where assetgl_code is not null ";
            objODBCDatareader = objdbconn.GetDataReader(msSQL);

            if (objODBCDatareader.HasRows == false)
            {
                msSQL = " select a.asset_status,b.product_name,a.assetdtl_gid " +
                        " from ams_trn_tassetdtl a" +
                        " left join pmr_mst_tproduct b on b.product_gid=a.product_gid ";
                objODBCDatareader1 = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader1.HasRows == true)
                {
                    lsasset_status = objODBCDatareader1["asset_status"].ToString();
                    lsproduct_name = objODBCDatareader1["product_name"].ToString();
                    lsassetdtl_gid = objODBCDatareader1["assetdtl_gid"].ToString();
                }
                if(lsasset_status == null)
                {
                    lssasset_status = "";
                }
                else
                {
                    lssasset_status = lsasset_status;
                }
                objODBCDatareader1.Close();

                msSQL = " update ams_trn_tassetdtl set " +
                        " assetgl_code  = '" + lsproduct_name + "'" +
                        " where assetdtl_gid='" + lsassetdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            objODBCDatareader.Close();

            msSQL = " Select a.assetdtl_gid,a.assetgl_code,b.product_name,a.asset_status, " +
                    " a.external_gl_code,a.gl_code_flag " +
                    " from ams_trn_tassetdtl a " +
                    " left join pmr_mst_tproduct b on b.product_gid = a.product_gid where 1 = 1 " +
                    " order by a.assetgl_code ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var AssetDtlsSummary_List = new List<GetAssetDtlsSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    AssetDtlsSummary_List.Add(new GetAssetDtlsSummary_List
                    {
                        assetdtl_gid = dt["assetdtl_gid"].ToString(),
                        assetgl_code = dt["assetgl_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        asset_status = dt["asset_status"].ToString(),
                        external_gl_code = dt["external_gl_code"].ToString(),
                        gl_code_flag = dt["gl_code_flag"].ToString()
                    });
                    values.GetAssetDtlsSummary_List = AssetDtlsSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetSalesTypeSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " Select a.salestype_gid,a.salestype_code,a.salestype_name,b.account_name,b.account_gid " +
                    " from smr_trn_tsalestype a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " order by salestype_name asc";            
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var SalesTypeSummary_List = new List<GetSalesTypeSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    SalesTypeSummary_List.Add(new GetSalesTypeSummary_List
                    {
                        salestype_gid = dt["salestype_gid"].ToString(),
                        salestype_code = dt["salestype_code"].ToString(),
                        salestype_name = dt["salestype_name"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.GetSalesTypeSummary_List = SalesTypeSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetPurchaseTypeSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " Select a.purchasetype_gid,a.purchasetype_code,a.purchasetype_name,b.account_name,b.account_gid " +
                    " from pmr_trn_tpurchasetype a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " order by purchasetype_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var PurchaseTypeSummary_List = new List<GetPurchaseTypeSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    PurchaseTypeSummary_List.Add(new GetPurchaseTypeSummary_List
                    {
                        purchasetype_gid = dt["purchasetype_gid"].ToString(),
                        purchasetype_code = dt["purchasetype_code"].ToString(),
                        purchasetype_name = dt["purchasetype_name"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.GetPurchaseTypeSummary_List = PurchaseTypeSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetDepartmentSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " select  a.department_gid,a.department_code,a.account_gid,a.account_name, a.department_prefix, a.department_name,  " +
                    " concat(c.user_firstname,' ',c.user_lastname) as created_by  ," +
                    " date_format(a.created_date,'%d-%b-%Y') as created_date " +
                    " from hrm_mst_tdepartment a  " +
                    " left join adm_mst_tuser c on a.created_by = c.user_gid " +
                    " order by a.department_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var GetDepartmentSummary_List = new List<GetDepartmentSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetDepartmentSummary_List.Add(new GetDepartmentSummary_List
                    {
                        department_gid = dt["department_gid"].ToString(),
                        department_code = dt["department_code"].ToString(),
                        department_prefix = dt["department_prefix"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                    });
                    values.GetDepartmentSummary_List = GetDepartmentSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAcctMappingSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " Select a.accountmapping_gid,a.account_gid,b.account_name, a.screen_name as screen," +
                    " case when a.module_name = 'PMR' then 'Purchase'" +
                    " when a.module_name = 'RBL' then 'Receivables' when a.module_name = 'TAE' " +
                    " then 'Travel and Expense' end as module,a.field_name as field" +
                    " from acc_mst_accountmapping a" +
                    " left join acc_mst_tchartofaccount b on b.account_gid = a.account_gid" +
                    " order by accountmapping_gid asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var AcctMappingSummary_List = new List<GetAcctMappingSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    AcctMappingSummary_List.Add(new GetAcctMappingSummary_List
                    {
                        accountmapping_gid = dt["accountmapping_gid"].ToString(),
                        module = dt["module"].ToString(),
                        screen = dt["screen"].ToString(),
                        field = dt["field"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        account_gid = dt["account_gid"].ToString()
                    });
                    values.GetAcctMappingSummary_List = AcctMappingSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetExpenseGroupSummary(string user_gid, MdlGLCode values)
        {
            msSQL = " select a.producttype_gid, a.producttype_code,b.account_name,a.producttype_name,a.account_gid" +
                    " from pmr_mst_tproducttype a" +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid" +
                    " where 0 = 0" +
                    " Order by  producttype_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var ExpenseGroupSummary_List = new List<GetExpenseGroupSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    ExpenseGroupSummary_List.Add(new GetExpenseGroupSummary_List
                    {
                        producttype_gid = dt["producttype_gid"].ToString(),
                        producttype_code = dt["producttype_code"].ToString(),
                        producttype_name = dt["producttype_name"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.GetExpenseGroupSummary_List = ExpenseGroupSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostSalesType(string user_gid, salestype_list values)
        {
            try
            {
                msSQL = " select salestype_name from smr_trn_tsalestype " +
                        " where salestype_name  = '" + values.salestype_name + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Sales Type Name Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("STPM");

                    msSQL = " insert into smr_trn_tsalestype  (" +
                            " salestype_gid," +
                            " salestype_name," +
                            " salestype_code," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + values.salestype_name + "'," +
                            "'" + values.salestype_code + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Sales Type Details Inserted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Inserting Sales Type Details !!";
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Inserting Sales Type Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetupdateSalesTypeDtls(string user_gid, salestype_list values)
        {
            try
            {

                msSQL = " SELECT salestype_name FROM smr_trn_tsalestype WHERE salestype_name='" + values.editsalestype_name + "' ";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Sales type name already exist";
                    return;
                }

                msSQL = " update smr_trn_tsalestype  set " +
                       " salestype_name = '" + values.editsalestype_name + "'," +
                       " upadated_by = '" + user_gid + "'," +
                       " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                       " where salestype_gid='" + values.salestype_gid + "'";
               mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
               if (mnResult == 1)
               {
                   values.status = true;
                   values.message = "Sales Type Updated Successfully !!";
               }
               else
               {
                   values.status = false;
                   values.message = "Error While Updating Sales Type !!";
                } 
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Sales Type";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetMappingAccountTo(string user_gid, MdlGLCode values)
        {
            msSQL = " Select account_gid, account_name" +
                    " from acc_mst_tchartofaccount where ledger_type='Y' and display_type='Y' and has_child='N'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var MappingAcctTo_List = new List<GetMappingAcctTo_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    MappingAcctTo_List.Add(new GetMappingAcctTo_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.GetMappingAcctTo_List = MappingAcctTo_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateSalesTypeMapping(string user_gid, salestype_list values)
        {
            try
            {
                msSQL = " update smr_trn_tsalestype  set " +
                        " account_gid = '" + values.account_name + "'" +
                        " where salestype_gid='" + values.salestype_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Account Mapped Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Data !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostPurchaseType(string user_gid, purchasetype_list values)
        {
            try
            {
                msSQL = " select purchasetype_name from pmr_trn_tpurchasetype " +
                        " where purchasetype_name = '" + values.purchasetype_name + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Purchase Type Name Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("PTPM");

                    msSQL = " insert into pmr_trn_tpurchasetype (" +
                            " purchasetype_gid," +
                            " purchasetype_name," +
                            " purchasetype_code," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + values.purchasetype_name + "'," +
                            "'" + values.purchasetype_code + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Purchase Type Details Inserted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Inserting Purchase Type Details !!";
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Inserting Purchase Type Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetupdatePurchaseTypeDtls(string user_gid, purchasetype_list values)
        {
            try
            {
                msSQL = " update pmr_trn_tpurchasetype set " +
                        " purchasetype_name = '" + values.editpurchasetype_name + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where purchasetype_gid='" + values.purchasetype_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " update acc_mst_tchartofaccount set" +
                            " account_name='" + values.editpurchasetype_name + "'" +
                            " where reference_gid='" + values.purchasetype_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Purchase Type Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Purchase Type !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Sales Type !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Sales Type";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetPurchaseMappingAccountTo(string user_gid, MdlGLCode values)
        {
            msSQL = " Select account_gid, account_name" +
                    " from acc_mst_tchartofaccount" +
                    " where ledger_type='Y' and display_type='N'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var PurchaseMappingAcctTo_List = new List<GetPurchaseMappingAcctTo_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    PurchaseMappingAcctTo_List.Add(new GetPurchaseMappingAcctTo_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.GetPurchaseMappingAcctTo_List = PurchaseMappingAcctTo_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdatePurchaseTypeMapping(string user_gid, purchasetype_list values)
        {
            try
            {
                msSQL = " update pmr_trn_tpurchasetype set " +
                        " account_gid = '" + values.account_name + "'" +
                        " where purchasetype_gid='" + values.purchasetype_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Account Mapped Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Data !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostTaxPayables(string user_gid, taxpayable_list values)
        {
            try
            {
                msSQL = " select tax_name from acp_mst_ttax " +
                        " where tax_name  = '" + values.tax_name + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Tax Name Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("STXM");

                    msGetGid1 = objcmnfunctions.GetMasterGID("FCOA");

                    msSQL = " insert into acp_mst_ttax (" +
                            " tax_gid," +
                            " tax_name," +
                            " tax_type," +
                            " gl_code," +
                            " account_gid," +
                            " percentage," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + values.tax_name + "'," +
                            "'" + values.tax_name + "'," +
                            "'" + values.tax_name + "'," +
                            "'" + msGetGid1 + "'," +
                            "'" + values.tax_percentage + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Tax Details Inserted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Inserting Tax Details !!";
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Inserting Tax Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetupdateTaxDtls(string user_gid, taxpayable_list values)
        {
            try
            {
                msSQL = " SELECT tax_name FROM acp_mst_ttax WHERE tax_name='" + values.edittax_name + "' ";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Tax name already exist";
                    return;
                }

                msSQL = " update acp_mst_ttax set " +
                        " tax_name = '" + values.edittax_name + "'," +
                        " tax_type = '" + values.edittax_name + "'," +
                        " percentage = '" + values.edittax_percentage + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where tax_gid='" + values.tax_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " update acc_mst_tchartofaccount  set" +
                            " account_name='" + values.edittax_name + "'" +
                            " where reference_gid='" + values.tax_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Tax Details Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Tax Details !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Tax Details !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetupdateTaxGLCode(string user_gid, taxpayable_list values)
        {
            try
            {
                msSQL = " select gl_code from acc_mst_tchartofaccount " +
                        " where gl_code = '" + values.externalgl_code + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "GL Code Already Exist !!";
                }
                else
                {
                    msSQL = " update acp_mst_ttax  set " +
                            //" tax_name  = '" + values.externalacct_name + "'," +
                            " external_gl_code  = '" + values.externalgl_code + "'," +
                            " gl_code_flag  = 'Assigned'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where tax_gid='" + values.tax_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "GL Code Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating GL Code !!";
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetupdateAssetGLCode(string user_gid, assetgl_list values)
        {
            try
            {
                msSQL = " select gl_code from acc_mst_tchartofaccount " +
                        " where gl_code = '" + values.assetexternalgl_code + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "GL Code Already Exist !!";
                }
                else
                {
                    msSQL = " update ams_trn_tassetdtl set " +
                            " external_gl_code  = '" + values.assetexternalgl_code + "'," +
                            " gl_code_flag  = 'Assigned'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where assetdtl_gid='" + values.assetdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "GL Code Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating GL Code !!";
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetupdateEmployeeGLCode(string user_gid, empglcode_list values)
        {
            try
            {
                msSQL = " select gl_code from acc_mst_taccounts " +
                        " where gl_code = '" + values.empexternalgl_code + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "GL Code Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("FCOA");

                    msSQL = " update hrm_mst_temployee set " +
                            " external_gl_code = '" + values.empexternalgl_code + "'," +
                            " gl_code_flag  = 'Assigned'," +
                            " account_gid  = '" + msGetGid + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where employee_gid='" + values.employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "External GL code Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating External GL Code !!";
                    }
                }
                objODBCDatareader.Close();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetExpenseMappingAcct(string user_gid, MdlGLCode values)
        {
            msSQL = " select account_gid, account_code, CONCAT(UCASE(substring(account_name,1,1)),LCASE(SUBSTRING(account_name,2))) as account_name from acc_mst_tchartofaccount where 0=0 ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var ExpenseMappingAcct_List = new List<GetExpenseMappingAcct_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    ExpenseMappingAcct_List.Add(new GetExpenseMappingAcct_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.GetExpenseMappingAcct_List = ExpenseMappingAcct_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateExpenseGroupMapping(string user_gid, expensegroup_list values)
        {
            try
            {
                    msSQL = " update pmr_mst_tproducttype set " +
                            " account_gid = '" + values.exaccount_name + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where producttype_gid='" + values.producttype_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Expense Group Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Expense Group !!";
                    }
                

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetAccountMappingDtl(string user_gid, MdlGLCode values)
        {
            msSQL = " select account_gid,account_name,accountgroup_name " +
                    " from acc_mst_tchartofaccount " +
                    " where accountgroup_gid in (select account_gid " +
                    " from acc_mst_tchartofaccount " +
                    " where (ledger_type='Y' and display_type='N' and accountgroup_name='$') or " +
                    " (ledger_type='Y' and display_type='Y' and accountgroup_name='$') or " +
                    " (ledger_type='N' and display_type='N' and accountgroup_name='$') or " +
                    " (ledger_type='N' and display_type='Y' and accountgroup_name='$')) ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var AccountMapping_List = new List<GetAccountMapping_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = "select account_gid,concat(accountgroup_name,' | ',account_name) as account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"].ToString() + "'";
                    dt_datatable1 = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable1.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable1.Rows)
                        {
                            AccountMapping_List.Add(new GetAccountMapping_List
                            {
                                account_gid = dt1["account_gid"].ToString(),
                                account_name = dt1["account_name"].ToString()
                            });
                        }
                    }
                }
                values.GetAccountMapping_List = AccountMapping_List;
            }
            dt_datatable.Dispose();
        }
        public void DaPostAccountMapping(string user_gid, accountmapping_list values)
        {
            try
            {
                if (values.module_name == "Purchase")
                {
                    lsmodule_name = "PMR";
                }
                else if (values.module_name == "Receivables")
                {
                    lsmodule_name = "RBL";
                }
                else if (values.module_name == "Travel and Expense")
                {
                    lsmodule_name = "TAE";
                }

                msSQL = " select screen_name,module_name,field_name " +
                        " from acc_mst_accountmapping   " +
                        " where account_gid = '" + values.mapping_account + "'" +
                        " and screen_name = '" + values.screen_name + "'" +
                        " and module_name = '" + lsmodule_name + "'" +
                        " and field_name = '" + values.field_name + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Mapping Details Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("FACM");

                    msSQL = " insert into acc_mst_accountmapping (" +
                            " accountmapping_gid," +
                            " account_gid," +
                            " screen_name," +
                            " module_name," +
                            " field_name," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + values.mapping_account + "'," +
                            "'" + values.screen_name + "'," +
                            "'" + lsmodule_name + "'," +
                            "'" + values.field_name + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Account Mapping Details Inserted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Inserting Account Mapping Details !!";
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Inserting Sales Type Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateMapAccount(string user_gid, mapaccount_list values)
        {
            try
            {
                msSQL = " update acc_mst_accountmapping set " +
                        " account_gid = '" + values.editmapping_account + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where accountmapping_gid='" + values.accountmapping_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Mapping Details Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Mapping Details !!";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaUpdateDebtorExternalCode(string user_gid, debtor_list values)
        {
            try
            {
                msSQL = " select gl_code from acc_mst_tchartofaccount " +
                        " where gl_code = '" + values.debtorexternalgl_code + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "GL Code Already Exist !!";
                }
                else
                {               
                msSQL = " update crm_mst_tcustomer set " +
                        " external_gl_code = '" + values.debtorexternalgl_code + "'," +
                        " gl_code_flag  = 'Assigned'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where customer_gid='" + values.customer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "GL Code Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating GL Code !!";
                }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetRegionDtls(string user_gid, MdlGLCode values)
        {
            msSQL = " Select region_gid, region_code, region_name " +
                    " from crm_mst_tregion Order by region_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var RegionName_List = new List<GetRegionName_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    RegionName_List.Add(new GetRegionName_List
                    {
                        region_gid = dt["region_gid"].ToString(),
                        region_name = dt["region_name"].ToString()
                    });
                    values.GetRegionName_List = RegionName_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCountryDtls(string user_gid, MdlGLCode values)
        {
            msSQL = " Select country_gid,country_name from adm_mst_tcountry ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var CountryName_List = new List<GetCountryName_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CountryName_List.Add(new GetCountryName_List
                    {
                        country_gid = dt["country_gid"].ToString(),
                        country_name = dt["country_name"].ToString()
                    });
                    values.GetCountryName_List = CountryName_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostDebitorGLCode(string user_gid, debtorglcode_list values)
        {
            try
            {
                msSQL = " select customer_id " +
                        " from crm_mst_tcustomer " +
                        " where customer_id  = '" + values.customer_code + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                msGetGid = objcmnfunctions.GetMasterGID("CC");
                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC'";
                string lsCode = objdbconn.GetExecuteScalar(msSQL);

                string lscustomer_code = "CC-" + "00" + lsCode;

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Customer Code Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BCRM");

                    msSQL = " insert into crm_mst_tcustomer " +
                            " (customer_gid," +
                             " customer_code, " +
                            " customer_id," +
                            " customer_name," +
                            " company_website," +
                            " customer_code," +
                            " customer_address," +
                            " customer_address2," +
                            " customer_state," +
                            " customer_country," +
                            " customer_city," +
                            " customer_pin," +
                            " customer_region," +
                            " main_branch," +
                            " created_by," +
                            " created_date)" +
                            " values( " +
                            "'" + msGetGid + "'," +
                             "'" + lscustomer_code + "'," +
                            "'" + values.customer_code + "'," +
                            "'" + values.customer_name + "'," +
                            "'" + values.company_website + "'," +
                            "'H.Q'," +
                            "'" + values.address_line1 + "'," +
                            "'" + values.address_line2 + "'," +
                            "'" + values.state_name + "'," +
                            "'" + values.country_name + "'," +
                            "'" + values.city_name + "'," +
                            "'" + values.pincode + "'," +
                            "'" + values.region_name + "'," +
                            "'Y'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msGetGid1 = objcmnfunctions.GetMasterGID("FCOA");

                         msSQL =  " INSERT INTO acc_mst_tchartofaccount (" +
                                  " account_gid, " +
                                  " accountgroup_gid, " +
                                  " gl_code, " +
                                  " account_name, " +
                                  " accountgroup_name, " +
                                  " display_type, " +
                                  " ledger_type, " +
                                  " Created_By, " +
                                  " Created_Date, " +
                                  " has_child) " +
                                  " VALUES (" +
                                  "'" + msGetGid1 + "', " +
                                  "'FCOA000022', " +
                                  "'" + values.customer_name + "'," +
                                  "'" + values.customer_name + "'," +
                                  "'Sales', " +
                                  "'N', " +
                                  "'N', " +
                                  "'" + user_gid + "', " +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                  "'N')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            objfinance.finance_vendor_debitor("Sales", lscustomer_code, values.customer_name, msGetGid, user_gid);
                            string trace_comment = " Added a customer on " + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_customer");
                            mscusconGetGID = objcmnfunctions.GetMasterGID("BCCM");

                            msSQL = " INSERT INTO crm_mst_tcustomercontact" +
                                    " (customercontact_gid," +
                                    " customer_gid," +
                                    " customercontact_name," +
                                    " email," +
                                    " mobile," +
                                    " designation," +
                                    " created_date," +
                                    " created_by," +
                                    " main_contact," +
                                    " phone," +
                                    " area_code," +
                                    " country_code," +
                                    " fax," +
                                    " fax_area_code," +
                                    " fax_country_code)" +
                                    " values( " +
                                    "'" + mscusconGetGID + "'," +
                                    "'" + msGetGid + "'," +
                                    "'" + values.contactperson_name + "'," +
                                    "'" + values.email_address + "'," +
                                    "'" + values.mobile_number + "'," +
                                    "'" + values.designation + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    "'" + user_gid + "'," +
                                    "'Y'," +
                                    "'" + values.countrycontact_number + "'," +
                                    "'" + values.countryarea_code + "'," +
                                    "'" + values.contactcountry_code + "'," +
                                    "'" + values.fax_number + "'," +
                                    "'" + values.faxarea_code + "'," +
                                    "'" + values.faxcountry_code + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                msleadbankGetGID = objcmnfunctions.GetMasterGID("BLBP");

                                msSQL = " INSERT INTO crm_trn_tleadbank " +
                                        " (leadbank_gid, " +
                                        " customer_gid, " +
                                        " leadbank_name," +
                                        " leadbank_address1, " +
                                        " leadbank_address2, " +
                                        " leadbank_city, " +
                                        " leadbank_state, " +
                                        " leadbank_pin, " +
                                        " leadbank_country, " +
                                        " leadbank_region, " +
                                        " approval_flag, " +
                                        " leadbank_id, " +
                                        " status, " +
                                        " main_branch," +
                                        " created_by, " +
                                        " created_date)" +
                                        " values ( " +
                                        "'" + msleadbankGetGID + "'," +
                                        "'" + msGetGid + "'," +
                                        "'" + values.customer_name + "'," +
                                        "'" + values.address_line1 + "'," +
                                        "'" + values.address_line2 + "'," +
                                        "'" + values.city_name + "'," +
                                        "'" + values.state_name + "'," +
                                        "'" + values.pincode + "'," +
                                        "'" + values.country_name + "'," +
                                        "'" + values.region_name + "'," +
                                        "'Approved'," +
                                        "'" + values.customer_code + "'," +
                                        "'Y'," +
                                        "'Y'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult != 0)
                                {
                                    msleadbankconGetGID = objcmnfunctions.GetMasterGID("BLCC");

                                    msSQL = " INSERT into crm_trn_tleadbankcontact (" +
                                            " leadbankcontact_gid, " +
                                            " leadbank_gid," +
                                            " leadbankcontact_name," +
                                            " email," +
                                            " mobile," +
                                            " designation," +
                                            " did_number," +
                                            " created_date," +
                                            " created_by," +
                                            " leadbankbranch_name, " +
                                            " address1," +
                                            " address2, " +
                                            " state, " +
                                            " country_gid, " +
                                            " city, " +
                                            " pincode, " +
                                            " region_name, " +
                                            " main_contact," +
                                            " phone1," +
                                            " area_code1," +
                                            " country_code1," +
                                            " fax," +
                                            " fax_area_code," +
                                            " fax_country_code)" +
                                            " values (" +
                                            "'" + msleadbankconGetGID + "'," +
                                            "'" + msleadbankGetGID + "'," +
                                            "'" + values.contactperson_name + "'," +
                                            "'" + values.email_address + "'," +
                                            "'" + values.mobile_number + "'," +
                                            "'" + values.designation + "'," +
                                            "'0'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "'" + user_gid + "'," +
                                            "'H.Q'," +
                                            "'" + values.address_line1 + "'," +
                                            "'" + values.address_line2 + "'," +
                                            "'" + values.state_name + "'," +
                                            "'" + values.country_name + "'," +
                                            "'" + values.city_name + "'," +
                                            "'" + values.pincode + "'," +
                                            "'" + values.region_name + "'," +
                                            "'Y'," +
                                            "'" + values.countrycontact_number + "'," +
                                            "'" + values.countryarea_code + "'," +
                                            "'" + values.contactcountry_code + "'," +
                                            "'" + values.fax_number + "'," +
                                            "'" + values.faxarea_code + "'," +
                                            "'" + values.faxcountry_code + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = " Debtor Details Inserted Successfully !!";
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error Occurred While Inserting Debtor Details !!";
                                    }
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error Occurred While Inserting Debtor Details !!";
                                }
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Inserting Debtor Details !!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Inserting Debtor Details !!";
                    }



                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Inserting Debtor Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateCreditorExternalCode(string user_gid, creditor_list values)
        {
            try
            {
                msSQL = " select gl_code from acc_mst_tchartofaccount " +
                        " where gl_code = '" + values.creditorexternalgl_code + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "GL Code Already Exist !!";
                }
                else
                {
                    msSQL = " update acp_mst_tvendor set " +
                            " external_gl_code = '" + values.creditorexternalgl_code + "'," +
                            " gl_code_flag  = 'Assigned'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where vendor_gid='" + values.vendor_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "GL Code Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating GL Code !!";
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostCreditorGLCode(string user_gid, creditor_list values)
        {
            try
            {
                msSQL = " select vendor_code " +
                        " from acp_mst_tvendor " +
                        " where vendor_code  = '" + values.vendor_code + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "vendor Code Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("PVRM");

                    msGetGid1 = objcmnfunctions.GetMasterGID("SADM");

                    msSQL = " insert into acp_mst_tvendor " +
                            " (vendor_gid," +
                            " vendor_code," +
                            " vendor_companyname," +
                            " contactperson_name," +
                            " contact_telephonenumber," +
                            " email_id," +
                            " address_gid ," +
                            " tin_number ," +
                            " created_by," +
                            " created_date)" +
                            " values( " +
                            "'" + msGetGid + "'," +
                            "'" + values.vendor_code + "'," +
                            "'" + values.vendorcompany_name + "'," +
                            "'" + values.contactperson_name + "'," +
                            "'" + values.contact_number + "'," +
                            "'" + values.email_address + "'," +
                            "'" + msGetGid1 + "'," +
                            "''," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msSQL =  " insert into adm_mst_taddress (" +
                                 " address_gid, " +
                                 " address1, " +
                                 " address2, " +
                                 " city, " +
                                 " state, " +
                                 " postal_code, " +
                                 " country_gid, " +
                                 " fax ) " +
                                 " values (" +
                                 "'" + msGetGid1 + "', " +
                                 "'" + values.address_line1 + "', " +
                                 "'" + values.address_line2 + "'," +
                                 "'" + values.city_name + "'," +
                                 "'" + values.state_name + "', " +
                                 "'" + values.pincode + "'," +
                                 "'" + values.country_name + "'," +
                                 "'" + values.fax_number + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msGetGid2 = objcmnfunctions.GetMasterGID("FCOA");

                            msSQL = " insert into acc_mst_tchartofaccount (" +
                                     " account_gid, " +
                                     " accountgroup_gid, " +
                                     " gl_code, " +
                                     " account_name, " +
                                     " accountgroup_name, " +
                                     " display_type, " +
                                     " ledger_type, " +
                                     " has_child) " +
                                     " values (" +
                                     "'" + msGetGid2 + "', " +
                                     "'FCOA000021', " +
                                     "'" + values.vendorcompany_name + "'," +
                                     "'" + values.vendorcompany_name + "'," +
                                     "'Purchase', " +
                                     "'N', " +
                                     "'N', " +
                                     "'N')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Creditor Details Inserted Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occurred While Inserting Creditor Details !!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Inserting Creditor Details !!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Inserting Creditor Details !!";
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Inserting Creditor Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTaxChartDetails(MdlGLCode values)
        {
            try
            {
                msSQL = @"SELECT SUM(overalltax_amount) AS overalltax_amount,
                        MONTHNAME(created_date) AS month_name FROM acc_trn_ttaxfiling a
                        WHERE a.created_date >= DATE_SUB(CURRENT_DATE(), INTERVAL 6 MONTH)
                                GROUP BY
                                  MONTHNAME(created_date)
                                ORDER BY
                                  month_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<taxchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new taxchart_list
                        {
                            tax_amount = dt["overalltax_amount"].ToString(),
                            month_name = dt["month_name"].ToString(),

                        });
                        values.taxchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax Details";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaTaxSegmentAccountMappingTo(string user_gid, MdlGLCode values)
        {
            msSQL = " select account_gid, account_name, accountgroup_gid, accountgroup_name, ledger_type, display_type, has_child " +
                    " from acc_mst_tchartofaccount " +
                    " where account_name like '%Current Asset%' and accountgroup_gid = '$' and ledger_type='N' and display_type='Y' " +
                    " union " +
                    " Select account_gid, account_name, accountgroup_gid, accountgroup_name, ledger_type, display_type, has_child " +
                    " from acc_mst_tchartofaccount " +
                    " where account_name like '%Current Liabilities%' and accountgroup_gid = '$' and ledger_type='N' and display_type='N' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var TaxSegmentAccountMappingTo_List = new List<GetTaxSegmentAccountMappingTo_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select account_gid,concat(accountgroup_name,' || ',account_name) as account_name from acc_mst_tchartofaccount " +
                            " where accountgroup_gid= '" + dt["account_gid"].ToString() + "'";
                    DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable1.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable1.Rows)
                        {
                            TaxSegmentAccountMappingTo_List.Add(new GetTaxSegmentAccountMappingTo_List
                            {
                                account_gid = dt1["account_gid"].ToString(),
                                account_name = dt1["account_name"].ToString(),
                            });
                        }
                    }
                }
                values.GetTaxSegmentAccountMappingTo_List = TaxSegmentAccountMappingTo_List;
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateTaxSegmentMapping(string user_gid, TaxSegmentAccountMapping_list values)
        {
            try
            {
                msSQL = "select account_name from acc_mst_tchartofaccount where account_gid = '" + values.taxaccount_name + "'";
                string lsaccountname=objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select accountgroup_gid from acc_mst_tchartofaccount where account_gid = '" + values.taxaccount_name + "'";
                string lsaccountgroupgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " update acp_mst_ttaxsegment set " +
                        " account_gid = '" + values.taxaccount_name + "'," +
                        " account_name = '" + lsaccountname + "'," +
                        " accountgroup_gid = '" + lsaccountgroupgid + "'," +
                        " updated_by = '" + user_gid + "', " +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where taxsegment_gid='" + values.taxsegment_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Tax Segment Account Mapped Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Tax Segment Account !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetTaxAccountMappingTo (MdlGLCode values)
        {
            msSQL = "Select account_gid from acc_mst_tchartofaccount where accountgroup_name like '%Duties And Taxes%'";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var GetTaxAccountMappingTo_List = new List<GetTaxAccountMappingTo_List>();

            if(dt_datatable.Rows.Count != 0)
            {
                foreach(DataRow dt in dt_datatable.Rows)
                {
                    msSQL = "select account_gid,account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"].ToString() + "'";
                    dt_datatable1 = objdbconn.GetDataTable(msSQL);
                    if(dt_datatable1.Rows.Count != 0)
                    {
                        foreach(DataRow dt1 in dt_datatable1.Rows)
                        {
                            GetTaxAccountMappingTo_List.Add(new GetTaxAccountMappingTo_List
                            {
                                account_gid = dt1["account_gid"].ToString(),
                                account_name = dt1["account_name"].ToString(),
                            });
                        }
                    }
                }
                values.GetTaxAccountMappingTo_List = GetTaxAccountMappingTo_List;
            }
        }
        public void DaUpdateTaxAccountMapping(string user_gid, GetTaxAccountMappingTo_List values)
        {
            try
            {
                msSQL = " update acp_mst_ttax set account_gid = '" + values.account_gid + "'," +
                        " updated_by = '" + user_gid + "', " +
                        " gl_code = (select account_name from acc_mst_tchartofaccount where account_gid = '" + values.account_gid + "'), " +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where tax_gid = '" + values.tax_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Tax Account Mapped Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Tax Account !!";
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Updating Tax Account Data";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetDeptAcctMappingDropdown(MdlGLCode values)
        {
            try
            {
                msSQL = "select account_gid from acc_mst_tchartofaccount where account_name like 'Indirect Expense%'";
                string account_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select account_gid from acc_mst_tchartofaccount where accountgroup_gid = '" + account_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetDepartmentAccountDropDown_list = new List<GetDepartmentAccountDropDown_list>();

                if(dt_datatable.Rows.Count != 0 )
                {
                    foreach(DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = "select account_gid,concat(accountgroup_name,' | ',account_name) as account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"].ToString() + "'";
                        dt_datatable1 = objdbconn.GetDataTable(msSQL);
                        if(dt_datatable1.Rows.Count != 0 )
                        {
                            foreach(DataRow dt1  in dt_datatable1.Rows)
                            {
                                GetDepartmentAccountDropDown_list.Add(new GetDepartmentAccountDropDown_list
                                {
                                    account_gid = dt1["account_gid"].ToString(),
                                    account_name = dt1["account_name"].ToString(),
                                });
                            }
                        }
                    }
                    values.GetDepartmentAccountDropDown_list = GetDepartmentAccountDropDown_list;
                }

            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Getting Department Account Ledgers Dropdown";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateDepartmentAccountMapping(string user_gid, GetDepartmentAccountDropDown_list values)
        {
            try
            {
                msSQL = "update hrm_mst_tdepartment set " +
                        "account_gid = '" + values.Dept_mapping_account + "', " +
                        "account_name = (select account_name from acc_mst_tchartofaccount where account_gid = '" + values.Dept_mapping_account + "'), " +
                        "updated_by = '" + user_gid + "', " +
                        "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        "where department_gid = '" + values.department_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Account Successfully Mapped Into Department!!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured While Mapping Account Into Department!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Departmaent Account Mapping";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetSalaryCompSummary(MdlGLCode values)
        {
            try
            {
                msSQL = "select salarycomponent_gid,componentgroup_gid,component_code,component_name,componentgroup_name,account_gid,account_name " +
                        " from pay_mst_tsalarycomponent";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSalaryCompSummary_list = new List<GetSalaryCompSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSalaryCompSummary_list.Add(new GetSalaryCompSummary_list
                        {
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            component_code = dt["component_code"].ToString(),
                            component_name = dt["component_name"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),

                        });
                        values.GetSalaryCompSummary_list = GetSalaryCompSummary_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch(Exception ex) 
            {
                values.message = "Exception occured while Getting Salary Component Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetSalaryCompMappingDropdown (MdlGLCode values)
        {
            try
            {
                msSQL = "select account_gid,account_name,accountgroup_name " +
                        " from acc_mst_tchartofaccount " +
                        " where accountgroup_gid in (select account_gid from acc_mst_tchartofaccount where ledger_type='Y' and display_type='N' and accountgroup_name='$')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSalaryCompDropdown_list = new List<GetSalaryCompDropdown_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = "select account_gid,concat(accountgroup_name,' | ',account_name) as account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"].ToString() + "'";
                        dt_datatable1 = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable1.Rows.Count != 0)
                        {
                            foreach (DataRow dt1 in dt_datatable1.Rows)
                            {
                                GetSalaryCompDropdown_list.Add(new GetSalaryCompDropdown_list
                                {
                                    account_gid = dt1["account_gid"].ToString(),
                                    account_name = dt1["account_name"].ToString(),
                                });
                            }
                        }
                    }
                    values.GetSalaryCompDropdown_list = GetSalaryCompDropdown_list;
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Getting Salary Component Mapping DropDown";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateSalaryComponentMapping(string user_gid, GetSalaryCompSummary_list values)
        {
            try
            {
                msSQL = " update pay_mst_tsalarycomponent set " +
                        " account_gid = '" + values.salarycomponent_ledger_gid + "' , " +
                        " account_name = (select account_name from acc_mst_tchartofaccount where account_gid = '" + values.salarycomponent_ledger_gid + "') , " +
                        " updated_by = '" + user_gid + "' , " +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where salarycomponent_gid = '" + values.salarycomponent_gid1 + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Salary Component Mapped Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Mapping Salary Component !!";
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Updating Salary Conponent Account Mapping";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}
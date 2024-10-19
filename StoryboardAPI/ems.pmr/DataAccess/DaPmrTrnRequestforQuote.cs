﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.pmr.Models;
using System.Globalization;
using System.Web.Services.Description;


namespace ems.pmr.DataAccess
{
    public class DaPmrTrnRequestforQuote
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGID5, msPOGetGID, msPO1GetGID, msGetGID, msGetGid, msGetGID4, msGetGID3, msGetGID1, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;


        public void DaGetRequestforQuoteSummary(MdlPmrTrnRequestforQuote values)
        {
            try
            {
                

                msSQL = " select date_format(a.enquiry_date,'%d-%b-%Y')as enquiry_date,a.enquiry_remarks,a.salesenquiry_gid,a.enquiry_gid,a.enquiry_status,c.purchaserequisition_gid,b.quotation_gid " +
                    " from pmr_trn_tenquiry a" +
                    " left join pmr_trn_tquotation b on b.enquiry_gid=b.enquiry_gid " +
                    " left join pmr_trn_tpr2enquiry c on c.enquiry_gid=a.enquiry_gid " +
                    " order by a.enquiry_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getrequestforquote_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getrequestforquote_lists
                        {

                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString(),
                            enquiry_status = dt["enquiry_status"].ToString(),
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            quotation_gid = dt["quotation_gid"].ToString(),
                            salesenquiry_gid = dt["salesenquiry_gid"].ToString(),
                            enquiry_remarks = dt["enquiry_remarks"].ToString(),



                        });
                        values.Getrequestforquote_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting RFQ summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }



        public void DaGetEnquirySelect(MdlPmrTrnRequestforQuote values)
        {
            try
            { 


                msSQL = " Select /*+ MAX_EXECUTION_TIME(300000) */ distinct a.purchaserequisition_gid ,date_format(a.purchaserequisition_date,'%d-%b-%Y')as purchaserequisition_date, " +
                " concat(b.user_firstname,'',b.user_lastname) as user_firstname,d.department_name,a.purchaserequisition_referencenumber," +
                " CASE when a.grn_flag <> 'GRN Pending' then a.grn_flag  when a.purchaseorder_flag <> 'PO Pending' then a.purchaseorder_flag " +
                " when a.enquiry_flag <> 'EQ Pending' then a.enquiry_flag " +
                " when a.purchaserequisition_flag='PR Approved' then 'PR Approved' else a.purchaserequisition_flag end as overall_status" +
                " from pmr_trn_tpurchaserequisition a " +
                " left join adm_mst_tuser b on b.user_gid=a.created_by " +
                " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                " left join hrm_mst_tdepartment d on d.department_gid = c.department_gid" +
                " left join pmr_trn_tpurchaserequisitiondtl e on e.purchaserequisition_gid= a.purchaserequisition_gid" +
                " where a.purchaserequisition_gid = e.purchaserequisition_gid and " +
                " a.purchaserequisition_flag not in('PR Pending Approval','PR Rejected','Pending New Product','PI Pending Approval') and a.purchaseorder_flag <> 'PO Raised'" +
                "  order by date(a.purchaserequisition_date) desc,a.purchaserequisition_date asc, a.purchaserequisition_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEnquirySelect>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEnquirySelect
                        {
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            purchaserequisition_referencenumber = dt["purchaserequisition_referencenumber"].ToString(),
                            overall_status = dt["overall_status"].ToString(),

                        });
                        values.GetEnquirySelect = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting enqiry select summary in RFQ!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetEnquiryproceed(string user_gid, GetEnquiryproceed values)
        {
            try
            {
                 
                msSQL = "  delete from pmr_tmp_tenquiry where user_gid='" + user_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                foreach (var data in values.enquiryaddprodeed_list1)
                {
                    msGetGID = objcmnfunctions.GetMasterGID("PEYP");
                    msGetGID1 = objcmnfunctions.GetMasterGID("PEYP");

                    msSQL = "  select product_gid from pmr_trn_tpurchaserequisitiondtl where product_name='" + data.product_name + "'  ";
                    string lsproduct_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "  select purchaserequisitiondtl_gid from pmr_trn_tpurchaserequisitiondtl where product_name='" + data.product_name + "'  ";
                    string lspurchaserequisitiondtl_gid = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " insert into pmr_tmp_tenquiry (" +
                            " tmpenquirydtl_gid, " +
                            " tmpenquiry_gid, " +
                            " purchaserequisition_gid, " +
                            " purchaserequisitiondtl_gid, " +
                            " uom_gid, " +
                            " display_field, " +
                            " product_gid ," +
                            " enquiryprocess_status, " +
                            " qty_enquired," +
                            " user_gid " +
                            " )values ( " +
                            "'" + msGetGID + "'," +
                            "'" + msGetGID1 + "'," +
                            "'" + data.purchaserequisition_gid + "'," +
                            "'" + lspurchaserequisitiondtl_gid + "', " +
                            "'" + data.productuom_name + "'," +
                            "'" + data.product_name + "'," +
                            "'" + lsproduct_gid + "', " +
                            "'" + "EQ Pending" + "'," +
                            "'" + data.qty_requested + "'," +
                            " '" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)

                    {
                        values.status = true;
                        values.message = "Enquiry Proceed Successfully!";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Enquiry Proceed!";

                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding enquiry proceed!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }


        public void DaGetEnquiryproceedConfirm(string user_gid, GetEnquiryproceedConfirm values)
        {
            try
            {
                 
                foreach (var data in values.enquiryaddconfirm_list)

                {
                    msGetGID4 = objcmnfunctions.GetMasterGID("PEDC");

                    msSQL = "  select product_gid from pmr_trn_tpurchaserequisitiondtl where product_code='" + data.product_code + "'  ";
                    string lsproduct_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "  select purchaserequisition_gid from pmr_trn_tpurchaserequisitiondtl where product_name='" + data.display_field + "'  ";
                    string lspurchaserequisition_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " insert into pmr_trn_tenquirydtl (" +
                            " enquirydtl_gid, " +
                            " enquiry_gid, " +
                            " product_gid, " +
                            " uom_gid, " +
                            " qty_enquired " +
                            " )values ( " +
                            "'" + msGetGID4 + "'," +
                            "'" + values.msGetGID3 + "'," +
                            "'" + lsproduct_gid + "', " +
                            "'" + data.productuom_name + "'," +
                            "'" + data.Enquiry_quantity + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msGetGID4 = objcmnfunctions.GetMasterGID("PRTE");

                        msSQL = " insert into pmr_trn_tpr2enquiry (" +
                            " pr2enquiry_gid, " +
                            " purchaserequisition_gid, " +
                            " enquiry_gid, " +
                            " created_by, " +
                            " created_date " +
                            " )values ( " +
                            "'" + msGetGID4 + "'," +
                            "'" + lspurchaserequisition_gid + "', " +
                            "'" + values.msGetGID3 + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            msSQL = " update pmr_trn_tpurchaserequisition set " +
                                    " enquiry_flag='Enquiry Sent'" +
                                    " where purchaserequisition_gid = '" + lspurchaserequisition_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        if (mnResult != 0)

                        {
                            values.status = true;

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Some Error occured while Adding Enquiry in EnquiryDtl Table!";

                        }


                    }
                }

                if (mnResult != 0)
                {
                    //msSQL = "  select product_gid from hrm_mst_temployee where user_gid='" + values.employee_name + "'  ";
                    //string lsproduct_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select d.branch_gid ," +
                        " b.employee_emailid, b.employee_mobileno, c.department_name  from  adm_mst_tuser a " +
                        " left join hrm_mst_temployee b on a.user_gid = b.user_gid  " +
                        " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid  " +
                        "left join hrm_mst_tbranch d on d.branch_gid = b.branch_gid "+
                        " where  a.user_gid ='" + user_gid + "'";
                    string branch_gid = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " insert into pmr_trn_tenquiry (" +
                            " enquiry_gid, " +
                            " branch_gid, " +
                            " enquiry_date, " +
                            " enquiry_remarks, " +
                            " enquiry_status, " +
                            " terms_conditions, " +
                            " created_by, " +
                            " created_date " +
                            " )values ( " +
                            "'" + values.msGetGID3 + "'," +
                            "'" + branch_gid + "', " +
                            "'" + values.Enquiry_Date + "'," +
                            "'" + values.remarks + "'," +
                            "'" + "EQ Pending" + "'," +
                            "'" + values.template_content + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                   

                    if (mnResult != 0)

                    {
                        values.status = true;
                        values.message = "Enquiry added successfully!";

                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Enquiry!";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Enquiry in RFQ!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            



        }

        public void DaGetEnquiryaddProceed(string purchaserequisition_gid, MdlPmrTrnRequestforQuote values)
        {
            try
            {
              

                msSQL = " Select /*+ MAX_EXECUTION_TIME(300000) */ distinct a.purchaserequisition_gid, a.created_by, b.uom_gid,date_format(a.purchaserequisition_date,'%d-%b-%Y')as purchaserequisition_date, " +
    "  g.department_name,  b.purchaserequisitiondtl_gid, sum(b.qty_requested) as qty_requested,format((sum(b.qty_requested )- sum(b.qty_received)),2) as qty_outstanding," +
    " b.display_field,b.product_code,b.productgroup_name,e.productuom_name,b.product_name" +
    " from pmr_trn_tpurchaserequisition a " +
    " left join pmr_trn_tpurchaserequisitiondtl b on a.purchaserequisition_gid = b.purchaserequisition_gid  " +
    " left join pmr_mst_tproduct c on b.product_gid = c.product_gid  " +
    "left join pmr_mst_tproductgroup d on c.productgroup_gid = d.productgroup_gid " +
    " left join pmr_mst_tproductuom e on b.uom_gid = e.productuom_gid " +
    " left join hrm_mst_temployee f on f.user_gid = a.created_by " +
    " left join hrm_mst_tdepartment g on g.department_gid = f.department_gid " +
    " where a.purchaserequisition_gid ='" + purchaserequisition_gid + "' group by c.product_gid,b.uom_gid,b.display_field  order by a.Purchaserequisition_date asc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEnquiryaddProceed>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEnquiryaddProceed
                        {
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_outstanding = dt["qty_outstanding"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            product_name = dt["product_name"].ToString(),




                        });
                        values.GetEnquiryaddProceed = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting enquiry add proceed in RFQ!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             

        }
        public void DaGetEnquiryaddConfirm(string purchaserequisition_gid, string user_gid, MdlPmrTrnRequestforQuote values)
        {
            try
            {
               

                msSQL = " select a.tmpenquirydtl_gid,a.tmpenquiry_gid,a.product_gid,a.display_field,a.uom_gid,b.product_code,b.productgroup_name," +
" a.qty_enquired,a.qty_enquired as Enquiry_quantity ,b.purchaserequisitiondtl_gid from pmr_tmp_tenquiry a" +
"  left join pmr_trn_tpurchaserequisitiondtl b on b.purchaserequisition_gid = a.purchaserequisition_gid " +
" where  a.user_gid ='" + user_gid + "' group by  a.product_gid, a.uom_gid,a.display_field order by b.purchaserequisitiondtl_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEnquiryaddConfirm>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEnquiryaddConfirm
                        {
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productuom_name = dt["uom_gid"].ToString(),
                            qty_enquired = dt["qty_enquired"].ToString(),
                            Enquiry_quantity = dt["Enquiry_quantity"].ToString(),


                        });
                        values.GetEnquiryaddConfirm = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting enquiry add confirm in RFQ !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }


        public void DaGetEnquiryaddConfirm1(string purchaserequisition_gid, string user_gid, MdlPmrTrnRequestforQuote values)
        {
            try
            {
                
                msGetGID3 = objcmnfunctions.GetMasterGID("PEYP");
                msSQL = " select concat( a.user_firstname,' - ',c.department_name) as employee_name," +
                        " b.employee_emailid, b.employee_mobileno, c.department_name from  adm_mst_tuser a " +
                        " left join hrm_mst_temployee b on a.user_gid = b.user_gid  " +
                        " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid  " +
                        " where  a.user_gid ='" + user_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEnquiryaddConfirm1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEnquiryaddConfirm1
                        {
                            msGetGID3 = msGetGID3,
                            employee_name = dt["employee_name"].ToString(),
                            employee_emailid = dt["employee_emailid"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            Enquiry_Date = DateTime.Now.ToString("yyyy-MM-dd")


                        });
                        values.GetEnquiryaddConfirm1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting enquiry add confirm in RFQ!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }



        public void DaGetRequestforQuoteSummarygrid(string enquiry_gid, MdlPmrTrnRequestforQuote values)
        {
            try
            {
                
                msSQL = " SELECT a.enquiry_gid,a.enquirydtl_gid, a.product_gid,a.display_field,b.product_code, " +
" a.qty_enquired, b.product_name, c.productgroup_name " +
" FROM pmr_trn_tenquirydtl a " +
" left join pmr_trn_tpurchaserequisitiondtl b on b.product_gid = a.product_gid " +
" left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
" where a.enquiry_gid ='" + enquiry_gid + "'" +
" order by a.enquirydtl_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getrequestforquotegrid_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getrequestforquotegrid_lists
                        {
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            qty_enquired = dt["qty_enquired"].ToString(),
                        });
                        values.Getrequestforquotegrid_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting RFQ summary grid!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetEnquirySelectgrid(string purchaserequisition_gid, MdlPmrTrnRequestforQuote values)
        {
            try
            {
                 
                msSQL = " SELECT a.purchaserequisition_gid,a.purchaserequisitiondtl_gid, a.product_gid,product_code, " +
" a.qty_requested, product_name, productgroup_name " +
" FROM pmr_trn_tpurchaserequisitiondtl a " +
" where a.purchaserequisition_gid = '" + purchaserequisition_gid + "'" +
" order by a.purchaserequisitiondtl_gid asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEnquirySelectgrid_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEnquirySelectgrid_lists
                        {
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                        });
                        values.GetEnquirySelectgrid_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while selecting enquiry summary details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetRequestForQuotationView(string enquiry_gid, MdlPmrTrnRequestforQuote values)
        {
            try
            {

                msSQL = " select a.enquiry_gid, date_format(a.enquiry_date,'%d-%m-%Y') as enquiry_date, " +
                    " a.created_by, a.vendor_gid, a.vendor_address, a.enquiry_referencenumber, a.enquiry_remarks, " +
                    " a.terms_conditions,  " +
                    " concat_ws(' - ',c.user_firstname,e.department_name) as user_firstname , " +
                    " d.employee_emailid, d.employee_mobileno " +
                    " from pmr_trn_tenquiry a " +
                    " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                    " left join hrm_mst_temployee d on d.user_gid = c.user_gid " +
                    " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
                    " where a.enquiry_gid = '" + enquiry_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<RFQview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new RFQview_list
                        {

                            enquiry_date = dt["enquiry_date"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            employee_emailid = dt["employee_emailid"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            terms_conditions = dt["terms_conditions"].ToString(),
                            enquiry_remarks = dt["enquiry_remarks"].ToString(),
                           
                        });
                        values.RFQview_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PR view!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }




    }

}



using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using static System.Drawing.ImageConverter;
using System.Web;
using ems.inventory.Models;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using CrystalDecisions.Shared.Json;
using System.Threading.Tasks;
using System.Runtime.Remoting;



namespace ems.inventory.DataAccess
{
    public class DaImsTrnIssueMaterial
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objODBCDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsbranch_gid, msIssueGID, msGetStockGID, msGetISTP, msGetStockTrackerGID, lsstock_gid, mcGetGID;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        string company_logo_path, authorized_sign_path;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        Image branch_logo;
        Image company_logo;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();

        public void DaGetImsTrnIssueMaterial(MdlImsTrnIssueMaterial values)
        {
            try
            {
                
                msSQL = " select distinct a.materialissued_gid, a.materialrequisition_gid, a.materialissued_status, " +
                        " date_format(a.materialissued_date,'%d-%m-%Y') as materialissued_date,concat(b.user_firstname,'/',d.department_name) as issued_to,m.costcenter_name," +
                        " b.user_firstname,d.department_name,g.branch_prefix,a.branch_gid,f.materialrequisition_reference" +
                        " from ims_trn_tmaterialissued a " +
                        " Left join ims_trn_tmaterialrequisition f on f.materialrequisition_gid = a.materialrequisition_gid " +
                        " left join ims_trn_tmaterialrequisitiondtl  z on z.materialrequisition_gid = f.materialrequisition_gid " +
                        " left join pmr_mst_tcostcenter m on f.costcenter_gid=m.costcenter_gid " +
                        " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " left join adm_mst_tuser e on e.user_gid = z.requested_by " +
                        " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                        " left join hrm_mst_tbranch g on a.branch_gid = g.branch_gid" +
                        " where 1=1 order by  date(a.materialissued_date) desc,a.materialissued_date asc, a.materialissued_gid desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getissuematerial_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getissuematerial_list
                    {

                        productuom_gid = dt["materialissued_gid"].ToString(),
                        materialissued_date = dt["materialissued_date"].ToString(),
                        materialissued_gid = dt["materialissued_gid"].ToString(),
                        costcenter_name = dt["costcenter_name"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                        materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                        user_firstname = dt["user_firstname"].ToString(),
                        issued_to = dt["issued_to"].ToString(),
                        branch_prefix = dt["branch_prefix"].ToString(),
                    });
                    values.Getissuematerial_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue Material !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public Dictionary<string, object> DaGetmaterialissueRpt(string branch_gid, string materialissued_gid, MdlImsTrnIssueMaterial values)
        {
            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();
            msSQL = " SELECT date_format(a.materialissued_date,'%d-%m-%Y') as materialissued_date,a.materialissued_gid,c.materialrequisition_gid," +
                    " d.product_name,d.product_code,g.productuom_name,i.display_field,c.qty_requested,b.qty_issued,a.materialissued_remarks FROM ims_trn_tmaterialissued a" +
                    " LEFT JOIN ims_trn_tstocktracker b on a.materialissued_gid = b.reference_gid" +
                    " LEFT JOIN ims_trn_tmaterialrequisitiondtl c on c.materialrequisitiondtl_gid = b.mrdtl_gid" +
                    " LEFT JOIN pmr_mst_tproduct d on d.product_gid = c.product_gid" +
                    " LEFT JOIN pmr_mst_tproductuom g on c.productuom_gid = g.productuom_gid" +
                    " left join ims_trn_tstock i on i.stock_gid=b.stock_gid" +
                    " where materialissued_gid = '" + materialissued_gid + "' and c.qty_issued <> '0.00'" +
                    " group by b.stocktracker_gid";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");
            msSQL = " select(a.email) branch_gid,a.branch_logo,a.branch_name,a.address1,a.city,a.state,a.postal_code,a.contact_number from hrm_mst_tbranch a" +
                    " left join ims_trn_tmaterialissued c on c.branch_gid=a.branch_gid" +
                    " where c.materialissued_gid='" + materialissued_gid + "'";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");
            msSQL = " Select p.department_name,b.materialrequisition_reference,concat(a.user_firstname,'-',a.user_lastname) as raisedby from adm_mst_tuser a" +
                    " left join ims_trn_tmaterialrequisition b on b.user_gid=a.user_gid" +
                    " LEFT JOIN hrm_mst_temployee o on o.user_gid = a.user_gid" +
                    " LEFT JOIN hrm_mst_tdepartment p on p.department_gid = o.department_gid" +
                    " left join ims_trn_tmaterialissued c on c.materialrequisition_gid=b.materialrequisition_gid where c.materialissued_gid='" + materialissued_gid + "'";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3");
            msSQL = "select  (branch_logo_path) as company_logo  from hrm_mst_tbranch where branch_gid = '" + branch_gid + "' and branch_logo_path is not null";
            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable4.Columns.Add("company_logo", typeof(byte[]));
            //DataTable4.Columns.Add("DataColumn14", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo"].ToString().Replace("../../", ""));

                    if (System.IO.File.Exists(company_logo_path) == true)
                    {
                        //Convert  Image Path to Byte
                        company_logo = System.Drawing.Image.FromFile(company_logo_path);
                        byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));

                        DataTable4.Rows.Add(bytes);
                    }
                }
            }

            dt1.Dispose();
            DataTable4.TableName = "DataTable4";
            myDS.Tables.Add(DataTable4);
            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_inventory"].ToString(), "imsTrnissuematerial.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Material Issue_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();
            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;

        }
        public void DaGetViewIssueMaterial(string reference_gid, MdlImsTrnIssueMaterial values)
        {
            try
            {

                msSQL = " select a.reference_gid as materialissued_gid,z.user_firstname as requested_by, a.stock_gid, a.product_gid,concat(i.user_firstname,' ',i.user_lastname) as raised_by,m.qty_requested, " +
                        " a.qty_issued as issued_quantity, b.product_name,m.display_field,b.product_code,k.department_name,e.materialissued_remarks,e.materialissued_reference, " +
                        " FORMAT((g.stock_qty + g.amend_qty - g.issued_qty - g.damaged_qty - g.transfer_qty), 2) AS stock_quantity,n.location_name, " +
                        " c.productgroup_name, f.productuom_name,h.branch_name,date_format(e.materialissued_date,'%d-%m-%Y') as materialissued_date " +
                        " from ims_trn_tstocktracker a  left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                        " left join pmr_mst_tproductuomclass d on d.productuomclass_gid = b.productuomclass_gid " +
                        " left join pmr_mst_tproductuom f on f.productuom_gid = a.productuom_gid " +
                        " left join ims_trn_tmaterialissued e on e.materialissued_gid = a.reference_gid " +
                        " left join ims_trn_tstock g on g.stock_gid=a.stock_gid" +
                        " left join hrm_mst_tbranch h on e.branch_gid = h.branch_gid" +
                        " left join adm_mst_tuser i on e.user_gid = i.user_gid " +
                        " left join ims_trn_tmaterialrequisitiondtl m on e.user_gid = m.user_gid " +
                        " left join hrm_mst_temployee j on i.user_gid = j.user_gid " +
                        " left join hrm_mst_tdepartment k on j.department_gid = k.department_gid " +
                        " left join ims_mst_tlocation n on e.location_gid = n.location_gid" +
                        " left join ims_trn_tmaterialrequisition y on e.materialrequisition_gid = y.materialrequisition_gid  " +
                        " left join adm_mst_tuser z on z.user_gid = y.user_gid " +
                        " where a.reference_gid = '" + reference_gid + "' group by e.materialissued_gid='" + reference_gid + "',a.stocktracker_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Viewissuematerial_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Viewissuematerial_list
                        {
                            materialissued_gid = dt["materialissued_gid"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            issued_quantity = dt["issued_quantity"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            materialissued_date = dt["materialissued_date"].ToString(),
                            raised_by = dt["raised_by"].ToString(),
                            requested_by = dt["requested_by"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            materialissued_reference = dt["materialissued_reference"].ToString(),
                            materialissued_remarks = dt["materialissued_remarks"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            

                        });

                        values.Viewissuematerial_list = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting IssueMaterial Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss")
                + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() 
                + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********",
                "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        //view summary
        public void DaGetViewIssueMaterialSummary(string reference_gid, MdlImsTrnIssueMaterial values)
        {
            try
            {

                msSQL = " select a.reference_gid as materialissued_gid,z.user_firstname as requested_by,concat(i.user_firstname,' ',i.user_lastname) as raised_by, " +
                        " k.department_name,e.materialissued_remarks,e.materialissued_reference, " +
                        " n.location_name, " +
                        " h.branch_name,date_format(e.materialissued_date,'%d-%m-%Y') as materialissued_date " +
                        " from ims_trn_tstocktracker a " +
                        " left join ims_trn_tmaterialissued e on e.materialissued_gid = a.reference_gid " +
                        " left join hrm_mst_tbranch h on e.branch_gid = h.branch_gid" +
                        " left join adm_mst_tuser i on e.user_gid = i.user_gid " +
                        " left join ims_trn_tmaterialrequisitiondtl m on e.user_gid = m.user_gid " +
                        " left join hrm_mst_temployee j on i.user_gid = j.user_gid " +
                        " left join hrm_mst_tdepartment k on j.department_gid = k.department_gid " +
                        " left join ims_mst_tlocation n on e.location_gid = n.location_gid" +
                        " left join ims_trn_tmaterialrequisition y on e.materialrequisition_gid = y.materialrequisition_gid  " +
                        " left join adm_mst_tuser z on z.user_gid = y.user_gid " +
                        " where a.reference_gid = '" + reference_gid + "' group by e.materialissued_gid='" + reference_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Viewissuematerialsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Viewissuematerialsummary_list
                        {
                            materialissued_gid = dt["materialissued_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            materialissued_date = dt["materialissued_date"].ToString(),
                            raised_by = dt["raised_by"].ToString(),
                            requested_by = dt["requested_by"].ToString(),
                            materialissued_reference = dt["materialissued_reference"].ToString(),
                            materialissued_remarks = dt["materialissued_remarks"].ToString(),
                            location_name = dt["location_name"].ToString(),


                        });

                        values.Viewissuematerialsummary_list = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting IssueMaterial Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss")
                + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString()
                + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********",
                "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetIssueMaterialselect(string user_gid, MdlImsTrnIssueMaterial values)
        {
            try
            {
                msSQL = "select b.branch_gid from hrm_mst_temployee a " +
                        "left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                        "where a.user_gid='"+ user_gid + "'";
                lsbranch_gid=objdbconn.GetExecuteScalar(msSQL);

                msSQL = "  select distinct a.materialrequisition_gid,a.materialrequisition_reference,date_format(a.materialrequisition_date, '%d-%m-%Y') as materialrequisition_date," +
                        " (case when a.materialrequisition_status='ApproveToIssue' then  'Approved To Issue' else a.materialrequisition_status end) as materialrequisition_status," +
                        "  case when a.material_status='PR Not Raised' then 'PI Not Raised'   when a.material_status='PR Raised' then 'PI Raised' " +
                        "  else a.material_status end as material_status,  b.user_firstname,d.department_name,a.created_date," +
                        "  a.materialrequisition_remarks,date_format(a.expected_date, '%d-%m-%Y') as expected_date,g.costcenter_name " +
                        "  from ims_trn_tmaterialrequisition a   left join adm_mst_tuser b on a.user_gid = b.user_gid   left join hrm_mst_temployee c on c.user_gid = b.user_gid" +
                        "  left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                        "  left join ims_trn_tmaterialrequisitiondtl f on f.materialrequisition_gid = a.materialrequisition_gid " +
                        "  left join pmr_mst_tcostcenter g on g.costcenter_gid = a.costcenter_gid " +
                        "  where 1=1 and a.branch_gid = '"+ lsbranch_gid + "' and a.materialrequisition_status  " +
                        "  in ('Approved' ,'Work In Progress')  " +
                        "  Order by date(a.materialrequisition_date)desc,(a.materialrequisition_date)asc,a.materialrequisition_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<issuematerialselect_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new issuematerialselect_list
                        {
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                            materialrequisition_status = dt["materialrequisition_status"].ToString(),
                            material_status = dt["material_status"].ToString(),
                            materialrequisition_date = dt["materialrequisition_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            materialrequisition_remarks = dt["materialrequisition_remarks"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString()
                        });

                        values.issuematerialselect_list = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting IssueMaterial Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss")
                + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString()
                + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********",
                "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetIssueViewProduct(string materialissued_gid, MdlImsTrnIssueMaterial values)
        {
            try
            {

                msSQL = "  SELECT d.product_name,i.display_field,b.qty_issued,d.product_code,g.productuom_name,c.qty_requested, "+
                        "  case when c.issuerequestqty<>'0.00'  then c.issuerequestqty else b.qty_issued end as issuerequestqty  " +
                        "  FROM ims_trn_tmaterialissued a  " +
                        "  LEFT JOIN ims_trn_tstocktracker b on a.materialissued_gid = b.reference_gid  " +
                        "  LEFT JOIN ims_trn_tmaterialrequisitiondtl c on c.materialrequisitiondtl_gid = b.mrdtl_gid  " +
                        "  LEFT JOIN pmr_mst_tproduct d on d.product_gid = c.product_gid  " +
                        "  LEFT JOIN pmr_mst_tproductuom g on c.productuom_gid = g.productuom_gid  " +
                        "  left join ims_trn_tstock i on i.stock_gid = b.stock_gid  " +
                        "  where materialissued_gid = '"+ materialissued_gid + "' and c.qty_issued<> '0.00'  " +
                        "  group by b.stocktracker_gid ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<issuematerialproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new issuematerialproduct_list
                        {
                            product_name = dt["product_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            qty_issued = dt["qty_issued"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            issuerequestqty = dt["issuerequestqty"].ToString(),
                        });
                        values.issuematerialproduct_list = getModuleList;
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


        public void DaGetDetialViewProduct(string materialrequisition_gid, MdlImsTrnIssueMaterial values)
        {
            try
            {

                msSQL = " select c.product_code,c.product_name,a.qty_issued,e.branch_gid,d.productuom_name,a.qty_requested, " +
                     " (select ifnull(sum(b.stock_qty+amend_qty-b.damaged_qty-b.issued_qty-transfer_qty),0) as stockqty from ims_trn_tstock b where b.product_gid=a.product_gid" +
                     " and b.uom_gid=a.productuom_gid and b.stock_flag='Y' and b.branch_gid=e.branch_gid) as stock," +
                      " case when a.issuerequestqty<>'0.00'  then a.issuerequestqty else a.qty_issued end as issuerequestqty" +
                     " from ims_trn_tmaterialrequisitiondtl a" +
                     " left join ims_trn_tmaterialrequisition e on e.materialrequisition_gid=a.materialrequisition_gid" +
                     " left join pmr_mst_tproduct c on c.product_gid=a.product_gid" +
                     " LEFT JOIN pmr_mst_tproductuom d on a.productuom_gid = d.productuom_gid" +
                     " where  a.materialrequisition_gid='" + materialrequisition_gid  + "'  and  (a.qty_requested-a.qty_issued) > 0  " +
                     " and e.materialrequisition_status <> 'Issued'"; 


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new materialproduct_list
                        {
                            product_name = dt["product_name"].ToString(),
                            qty_issued = dt["qty_issued"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            issuerequestqty = dt["qty_issued"].ToString(),
                        });
                        values.materialproduct_list = getModuleList;
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

        public void DaGetMIissuedetails(string materialrequisition_gid, MdlImsTrnIssueMaterial values)
        {
            try
            {
                msSQL = "  select materialrequisition_gid,date_format(materialrequisition_date, '%d-%m-%Y') as materialrequisition_date,e.department_name,a.reference_gid,  " +
                        "  branch_name,concat_ws('-', b.user_firstname, b.user_lastname) as user_firstname,d.employee_mobileno,  " +
                        "  concat_ws('-', f.costcenter_name, f.costcenter_gid) as costcenter_name,a.costcenter_gid,format(f.budget_available, 2) as budget_available,e.department_gid,  " +
                        "  a.user_gid,a.materialrequisition_remarks,case when a.approved_by is null then a.reject_reason else a.approver_remarks end as approver_remarks," +
                        "  date_format(a.expected_date, '%d-%m-%y') as expected_date from ims_trn_tmaterialrequisition a  " +
                        "  left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        "  left join hrm_mst_temployee d on d.user_gid = b.user_gid " +
                        "  left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                        "  left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
                        "  left join pmr_mst_tcostcenter f on f.costcenter_gid = a.costcenter_gid " +
                        "  where materialrequisition_gid = '" + materialrequisition_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMIissuedetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetMIissuedetails_list
                        {
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialrequisition_date = dt["materialrequisition_date"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            reference_gid = dt["reference_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            materialrequisition_remarks = dt["materialrequisition_remarks"].ToString(),
                            approver_remarks = dt["approver_remarks"].ToString(),
                            expected_date = dt["expected_date"].ToString(),

                        });
                        values.GetMIissuedetails_list = getModuleList;
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
        public void DaGetMIissuedetailproduct(string branch_gid,string materialrequisition_gid, MdlImsTrnIssueMaterial values)
        {
            try
            {
                msSQL = "  select P.product_gid,P.product_code,P.product_name ,sum(MR_DTL.qty_requested) as req_qty,g.productgroup_name,  "+
                         " case when MR_DTL.issuerequestqty < MR_DTL.qty_issued then(MR_DTL.issuerequestqty + MR_DTL.qty_issued) "+
                         " when MR_DTL.qty_issued<>'0.00' and(MR_DTL.issuerequestqty > MR_DTL.qty_issued) then(MR_DTL.issuerequestqty + MR_DTL.qty_issued) " +
                         " when MR_DTL.issuerequestqty = MR_DTL.qty_issued then(MR_DTL.issuerequestqty + MR_DTL.qty_issued) " +
                         " else MR_DTL.issuerequestqty end as issuerequestqty,   " +
                         " MR.branch_gid,MR_DTL.display_field,MR_DTL.materialrequisition_gid,MR_DTL.productuom_gid,c.productuom_name,   " +
                         " MR_DTL.materialrequisitiondtl_gid,MR_DTL.display_field,p.serial_flag,'' as yarn_knots from " +
                         " ims_trn_tmaterialrequisitiondtl MR_DTL left join ims_trn_tmaterialrequisition MR on MR.materialrequisition_gid = MR_DTL.materialrequisition_gid "+
                         " left join pmr_mst_tproduct P on P.product_gid = MR_DTL.product_gid " +
                         " left  join pmr_mst_tproductuom c on c.productuom_gid = MR_DTL.productuom_gid " +
                         " left join pmr_mst_tproductgroup g on g.productgroup_gid=p.productgroup_gid"+
                         " where MR_DTL.materialrequisition_gid = '"+ materialrequisition_gid + "' and (MR_DTL.qty_requested - MR_DTL.qty_issued) > 0 " +
                         " and MR.materialrequisition_status<> 'Issued'" +
                         " group by MR_DTL.product_gid,MR_DTL.productuom_gid,MR_DTL.display_field";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMIissueproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " SELECT COALESCE(sum(stock_qty+amend_qty-issued_qty-damaged_qty-transfer_qty) ,0)as stock_quantity" +
                                " FROM ims_trn_tstock a   where a.product_gid='"+ dt["product_gid"].ToString() + "'  and a.uom_gid = '"+ dt["productuom_gid"].ToString() + "'" +
                                " and a.branch_gid = '"+ branch_gid + "' and (a.stock_qty-a.issued_qty ) >= 0 and a.stock_flag='Y'  " +
                                " group by a.product_gid, a.uom_gid";
                        //string stock_quantity=objdbconn.GetExecuteScalar(msSQL);
                        string result = objdbconn.GetExecuteScalar(msSQL);
                        string stock_quantity = string.IsNullOrEmpty(result) ? "0" : result;
                        if (string.IsNullOrEmpty(stock_quantity) || stock_quantity == "")
                        {
                            stock_quantity = "0";
                        }

                        msSQL = " SELECT case when a.issuerequestqty=a.qty_issued then '0' else a.qty_issued end as qty_issued   " +
                                " FROM ims_trn_tmaterialrequisitiondtl a  left join ims_trn_tmaterialrequisition b on a.materialrequisition_gid = b.materialrequisition_gid   " +
                                " where a.product_gid='"+ dt["product_gid"].ToString() + "'" +
                                " and a.productuom_gid = '"+ dt["productuom_gid"].ToString() + "'" +
                                " and a.materialrequisition_gid='"+ materialrequisition_gid + "'" +
                                " and b.branch_gid = '"+ branch_gid + "'" +
                                " group by a.product_gid, a.productuom_gid,a.display_field";
                        string qty_issued = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new GetMIissueproduct_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            req_qty = dt["req_qty"].ToString(),
                            issuerequestqty = dt["issuerequestqty"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            materialrequisitiondtl_gid = dt["materialrequisitiondtl_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            stock_quantity = stock_quantity,
                            qty_issued = qty_issued,

                        });
                        values.GetMIissueproduct_list = getModuleList;
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

        public void DaPostissue(string user_gid, string branch_gid, Postissuemetrial values)
        {
            try
            {
                msIssueGID = objcmnfunctions.GetMasterGID("IMIT");
                for (int i = 0; i < values.GetMIissueproduct_list.Count; i++)
                {
                    msSQL = " select distinct a.stock_gid from ims_trn_tstock a  " +
                            " left join pmr_mst_tproduct b on a.product_gid=b.product_gid  " +
                            " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid  " +
                            " left join ims_mst_tlocation d on a.location_gid=d.location_gid  " +
                            " left join ims_mst_tbin k on a.bin_gid=k.bin_gid  " +
                            " where a.product_gid='" + values.GetMIissueproduct_list[0].product_gid + "' and a.uom_gid='" + values.GetMIissueproduct_list[0].productuom_gid + "' and " +
                            " a.stock_flag='Y' and a.branch_gid='"+ branch_gid + "' group by stock_gid";
                        lsstock_gid = objdbconn.GetExecuteScalar(msSQL);
                        double stock = double.Parse(values.GetMIissueproduct_list[i].issuerequestqty);
                        msSQL = " Update ims_trn_tmaterialrequisitiondtl" +
                                " Set qty_issued = qty_issued + '" + stock + "'," +
                                " requeststatus ='N'" +
                                " Where materialrequisitiondtl_gid = '" + values.GetMIissueproduct_list[i].materialrequisitiondtl_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msGetISTP = objcmnfunctions.GetMasterGID("ISTP");
                        msSQL = "insert into ims_trn_tstockdtl(" +
                                "stockdtl_gid," +
                                "stock_gid," +
                                "branch_gid," +
                                "product_gid," +
                                "uom_gid," +
                                "issued_qty," +
                                "amend_qty," +
                                "damaged_qty," +
                                "adjusted_qty," +
                                "transfer_qty," +
                                "return_qty," +
                                "stock_type," +
                                "remarks," +
                                "created_by," +
                                "created_date," +
                                "display_field" +
                                ") values ( " +
                                "'" + msGetISTP + "'," +
                                "'" + lsstock_gid + "'," +
                                "'" + branch_gid + "'," +
                                "'" + values.GetMIissueproduct_list[i].product_gid + "'," +
                                "'" + values.GetMIissueproduct_list[i].productuom_gid + "'," +
                                "'" + values.GetMIissueproduct_list[i].issuerequestqty + "'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'Issued'," +
                                "''," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        mcGetGID = objcmnfunctions.GetMasterGID("IMNL");
                        msSQL = " insert into ims_trn_tnominalregister (" + 
                                " nominal_gid," + 
                                " materialrequisition_gid," + 
                                " product_gid," + 
                                " productuom_gid," + 
                                " issued_qty," + 
                                " remarks, " + 
                                " created_date, " + 
                                " branch_gid, " + 
                                " department_gid, " + 
                                " created_by, " + 
                                " stock_gid ," + 
                                " materialrequisitiondtl_gid " + 
                                " ) values(" + 
                                "'" + mcGetGID + "'," + 
                                "'" + values.materialrequisition_gid + "'," + 
                                "'" + values.GetMIissueproduct_list[i].product_gid + "'," + 
                                "'" + values.GetMIissueproduct_list[i].productuom_gid + "'," + 
                                "'" + values.GetMIissueproduct_list[i].issuerequestqty + "'," + 
                                "'" + values.GetMIissueproduct_list[i].display_field.Replace("'", "\\\'") + "'," + 
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + 
                                "'" + branch_gid + "'," + 
                                "'" + values.department_name + "'," + 
                                "'" + user_gid + "'," + 
                                "'" + lsstock_gid + "'," + 
                                "'" + values.GetMIissueproduct_list[i].materialrequisitiondtl_gid + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult1 == 1)
                        {
                            msSQL = " update ims_trn_tstock set" + 
                                    " issued_qty=issued_qty+'" + stock + "'" +
                                    " where stock_gid='" + lsstock_gid + "' and" +
                                    " product_gid='" + values.GetMIissueproduct_list[i].product_gid + "' and " +
                                    " uom_gid='" + values.GetMIissueproduct_list[i].productuom_gid + "' and " +
                                    " branch_gid='" + branch_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msGetStockTrackerGID = objcmnfunctions.GetMasterGID("ISTK");
                            msSQL = " Insert into ims_trn_tstocktracker (" + 
                                     " stocktracker_gid," + 
                                     " stock_gid, " + 
                                     " branch_gid," + 
                                     " product_gid," + 
                                     " productuom_gid," + 
                                     " display_field," + 
                                     " qty_issued," + 
                                     " stocktype_gid," + 
                                     " created_by," + 
                                     " created_date," + 
                                     " remarks, " + 
                                     " mrdtl_gid," +
                                     " reference_gid," + 
                                     " mr_gid) " + 
                                     " values ( " + 
                                     " '" + msGetStockTrackerGID + "', " + 
                                     " '" + lsstock_gid + "'," + 
                                     " '" + branch_gid + "', " + 
                                     " '" + values.GetMIissueproduct_list[i].product_gid + "', " + 
                                     " '" + values.GetMIissueproduct_list[i].productuom_gid + "', " + 
                                     " '" + values.GetMIissueproduct_list[i].display_field.Replace("'", "\\\'") + "'," + 
                                     " '" + values.GetMIissueproduct_list[i].issuerequestqty + "'," + 
                                     " 'SY0905270006'," + 
                                     " '" + user_gid + "'," + 
                                     " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + 
                                     " 'Material Issued', " + 
                                     " '" + values.GetMIissueproduct_list[i].materialrequisitiondtl_gid + "', " + 
                                     " '" + msIssueGID + "', " + 
                                     " '" + values.materialrequisition_gid + "')";
                            mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Inserting Nominal Register details!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Inserting stock details!";
                    }
                }

                if (mnResult2 == 1)
                {
                    
                    msSQL = " Insert into ims_trn_tmaterialissued (" + 
                            " materialissued_gid, " + 
                            " materialissued_date, " + 
                            " branch_gid, " + 
                            " materialrequisition_gid, " + 
                            " materialrequisition_type, " + 
                            " materialissued_status, " + 
                            " materialissued_reference, " + 
                            " materialissued_remarks, " + 
                            " user_gid, " +
                            " reference_gid, " + 
                            " created_date) " + 
                            " values ( " + 
                            "'" + msIssueGID + "'," + 
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + 
                            "'" + branch_gid + "'," + 
                            "'" + values.materialrequisition_gid + "'," + 
                            "'PT00010001204'," + 
                            "'Issued Accept Pending'," + 
                            "'" + values.issue_remarks.Replace("'", "\\\'") + "'," + 
                            "'" + values.issue_remarks.Replace("'", "\\\'") + "'," + 
                            "'" + user_gid + "'," + 
                            "'" + msIssueGID + "'," + 
                            "'"+ DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if(mnResult == 1)
                    {
                        msSQL = " Select materialrequisitiondtl_gid " +
                                " From ims_trn_tmaterialrequisitiondtl " +
                                " Where qty_requested != qty_issued" +
                                " and materialrequisition_gid = '" + values.materialrequisition_gid + "'";
                        objODBCDataReader = objdbconn.GetDataReader(msSQL);
                        if(objODBCDataReader.HasRows)
                        {
                            msSQL = " Update ims_trn_tmaterialrequisition" +
                                   " Set materialrequisition_status = 'Work In Progress'" +
                                   " Where materialrequisition_gid = '" + values.materialrequisition_gid + "'";
                            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult1 == 1)
                            {
                                values.status = true;
                                values.message = "Material issued successfully";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occured While Updating Material Requisition Status Details";
                            }

                        }
                        else
                        {
                            msSQL = " Update ims_trn_tmaterialrequisition" +
                                    " Set materialrequisition_status = 'Issued'" +
                                    " Where materialrequisition_gid = '" + values.materialrequisition_gid + "'";
                            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult1 == 1)
                            {
                                values.status = true;
                                values.message = "Material issued successfully";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occured While Updating Material Requisition Status Details";
                            }
                        }
                    }
                    else
                    {
                        values.status=false;
                        values.message = "Error While Inserting material issue details!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Inserting Stock Tracker details!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while material Issue !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
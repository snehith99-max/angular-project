using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Media.Media3D;
using System.Reflection;

namespace ems.inventory.DataAccess
{
    public class DaIMSTrnMaterialindent
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        string company_logo_path, authorized_sign_path;
        OdbcDataReader objOdbcDataReader, objODBCDataReader, objOdbcDataReader1;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        Image branch_logo;
        Image company_logo;
        int mnResult, mnResult2, mnResult1, mnResult3;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();
        string lsissuenow_qty, lsqty_issued, lsqty_requested, lsmaterialissued_reference, lsmaterialissued_date, lsavailable;
        public void DaMatrialIndentsummary(MdlImsTrnMaterialindent values)
        {
            try
            {

             msSQL = " select distinct a.materialrequisition_gid,f.branch_prefix,a.materialrequisition_reference, a.materialrequisition_remarks, a.materialrequisition_gid as material,a.priority, " +
                     " a. materialrequisition_status, " +
                     " b.user_firstname, h.qty_requested,(select ifnull(sum(m.stock_qty)+sum(m.amend_qty)-sum(m.damaged_qty)-sum(m.issued_qty)-sum(m.transfer_qty),0) as available_quantity  " +
                     " from ims_trn_tstock m where m.stock_flag='Y' and m.product_gid=h.product_gid and m.branch_gid=a.branch_gid and h.productuom_gid=m.uom_gid) as available_quantity , " +
                     " date_format(a.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date,a.mrbapproval_remarks,g.costcenter_name,  " +
                     " d.department_name, date_format(a.created_date,'%d-%m-%Y') as created_date,date_format(a.expected_date,'%d-%m-%Y') as expected_date  " +
                     " from ims_trn_tmaterialrequisition a  " +
                     " left join pmr_mst_tcostcenter g on a.costcenter_gid = g.costcenter_gid  " +
                     " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                     " left join hrm_mst_temployee c on c.user_gid = b.user_gid  " +
                     " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                     " left join pmr_mst_tproducttype e on a.materialrequisition_type = e.producttype_gid  " +
                     " left join hrm_mst_tbranch f on f.branch_gid = a.branch_gid  " +
                     " left join ims_trn_tmaterialrequisitiondtl h on h.materialrequisition_gid=a.materialrequisition_gid " +
                     " where 1 = 1 group by materialrequisition_gid Order by a.materialrequisition_date desc, a.materialrequisition_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialindent_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        if (int.Parse(dt["qty_requested"].ToString()) < int.Parse(dt["available_quantity"].ToString()))
                        {
                             lsavailable = "Y";
                        }
                        else
                        {
                            lsavailable = "N";
                        }
                        getModuleList.Add(new materialindent_list
                        {
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                            materialrequisition_remarks = dt["materialrequisition_remarks"].ToString(),
                            material = dt["material"].ToString(),
                            materialrequisition_status = dt["materialrequisition_status"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            materialrequisition_date = dt["materialrequisition_date"].ToString(),
                            mrbapproval_remarks = dt["mrbapproval_remarks"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            priority = dt["priority"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            available = lsavailable,
                        });
                        values.materialindentsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Material Indent!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaMatrialIndentApprovalsummary(MdlImsTrnMaterialindent values)
        {
            try
            {

                msSQL = " select distinct a.materialrequisition_gid,f.branch_prefix,a.materialrequisition_reference, a.materialrequisition_remarks, a.materialrequisition_gid as material,a.priority, " +
                        " a. materialrequisition_status, " +
                        " b.user_firstname, h.qty_requested,(select ifnull(sum(m.stock_qty)+sum(m.amend_qty)-sum(m.damaged_qty)-sum(m.issued_qty)-sum(m.transfer_qty),0) as available_quantity  " +
                        " from ims_trn_tstock m where m.stock_flag='Y' and m.product_gid=h.product_gid and m.branch_gid=a.branch_gid and h.productuom_gid=m.uom_gid) as available_quantity , " +
                        " date_format(a.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date,a.mrbapproval_remarks,g.costcenter_name,  " +
                        " d.department_name, date_format(a.created_date,'%d-%m-%Y') as created_date,date_format(a.expected_date,'%d-%m-%Y') as expected_date  " +
                        " from ims_trn_tmaterialrequisition a  " +
                        " left join pmr_mst_tcostcenter g on a.costcenter_gid = g.costcenter_gid  " +
                        " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " left join hrm_mst_temployee c on c.user_gid = b.user_gid  " +
                        " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                        " left join pmr_mst_tproducttype e on a.materialrequisition_type = e.producttype_gid  " +
                        " left join hrm_mst_tbranch f on f.branch_gid = a.branch_gid  " +
                        " left join ims_trn_tmaterialrequisitiondtl h on h.materialrequisition_gid=a.materialrequisition_gid " +
                        " where 1 = 1 and "+
                        "a.materialrequisition_status in ('Pending','Rejected') group by materialrequisition_gid Order by a.materialrequisition_date desc, a.materialrequisition_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialindent_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        if (int.Parse(dt["qty_requested"].ToString()) < int.Parse(dt["available_quantity"].ToString()))
                        {
                            lsavailable = "Y";
                        }
                        getModuleList.Add(new materialindent_list
                        {
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                            materialrequisition_remarks = dt["materialrequisition_remarks"].ToString(),
                            material = dt["material"].ToString(),
                            materialrequisition_status = dt["materialrequisition_status"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            materialrequisition_date = dt["materialrequisition_date"].ToString(),
                            mrbapproval_remarks = dt["mrbapproval_remarks"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            priority = dt["priority"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            available = lsavailable,
                        });
                        values.materialindentsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Material Indent!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public Dictionary<string, object> DaGetmaterialindentRpt(string branch_gid, string materialrequisition_gid, MdlImsTrnMaterialindent values)
        {

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = " select  a.materialrequisition_gid as mrreferenceno,date_format(a.materialrequisition_date,'%d-%m-%Y')as materialrequisition_date , "+
                    " a.materialrequisition_remarks,a.approved_by, date_format(a.approved_date, '%d-%m-%Y') as approved_date,  a.user_gid, "+
                    " date_format(a.created_date, '%d-%m-%Y') as created_date, a.branch_gid,b.user_firstname as approval_name, " +
                    " c.user_firstname as created_name,d.costcenter_name, " +
                    " a.costcenter_gid, a.approver_remarks, e.branch_name, e.branch_logo,date_format(a.expected_date, '%d-%m-%Y') as expected_date " +
                    " from ims_trn_tmaterialrequisition a " +
                    " left join adm_mst_tuser b on a.approved_by = b.user_gid " +
                    " left join adm_mst_tuser c on a.user_gid = c.user_gid " +
                    " left join pmr_mst_tcostcenter d on a.costcenter_gid = d.costcenter_gid " +
                    " left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid " +
                    " where a.materialrequisition_gid ='"+ materialrequisition_gid + "'";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = " select a.materialrequisitiondtl_gid, a.materialrequisition_gid, a.product_gid,a.mr_originalqty, "+
                    " a.qty_requested, a.productuom_gid, a.display_field, b.product_name, a.productgroup_name, a.productuom_name, a.product_code,  "+
                    " (select ifnull(sum(m.stock_qty) + sum(m.amend_qty) - sum(m.damaged_qty) - sum(m.issued_qty) - sum(m.transfer_qty), 0) as availle_quantity" +
                    " from ims_trn_tstock m where m.stock_flag = 'Y' and m.product_gid = a.product_gid and m.branch_gid = e.branch_gid and a.productuom_gid = m.uom_gid) as available_quantity "+
                    " from ims_trn_tmaterialrequisitiondtl a "+
                    " left join ims_trn_tmaterialrequisition e on a.materialrequisition_gid = e.materialrequisition_gid "+
                    " left join pmr_mst_tproduct b on a.product_gid = b.product_gid "+
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid "+
                    " left join pmr_mst_tproductuom d on a.productuom_gid = d.productuom_gid "+
                    " where e.materialrequisition_gid ='"+ materialrequisition_gid + "'";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");

            msSQL = "select  (branch_logo_path) as company_logo  from hrm_mst_tbranch where branch_gid = '" + branch_gid + "' and branch_logo_path is not null";

            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable3.Columns.Add("company_logo", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo"].ToString().Replace("../../", ""));

                    if (System.IO.File.Exists(company_logo_path) == true)
                    {
                        company_logo = System.Drawing.Image.FromFile(company_logo_path);
                        byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));
                        DataTable3.Rows.Add(bytes);
                    }
                }
            }

            dt1.Dispose();
            DataTable3.TableName = "DataTable3";
            myDS.Tables.Add(DataTable3);

            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_inventory2"].ToString(), "ImsCrpMaterialIndent.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Material Indent_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;

        }
        public void DaGetMaterialIndentView(string materialrequisition_gid, MdlImsTrnMaterialindent values)
        {
            try
            {
                msSQL = "  select  a.materialrequisition_gid,i.user_firstname as requested_by, " +
                     " date_format(a.materialrequisition_date, '%d-%m-%Y') as materialrequisition_date, a.approver_remarks, " +
                     " a.materialrequisition_type,a.materialrequisition_remarks,a.materialrequisition_reference,e.branch_name,a.branch_gid, " +
                     " concat(b.user_firstname, ' - ', b.user_lastname) as user_firstname,d.department_name,If(a.priority='N','Low','High') as priority ,  " +
                     " a.costcenter_gid,format(f.budget_available, 2) as budget_available,concat_ws('-', f.costcenter_name, f.costcenter_code) as costcenter_name, " +
                     " g.user_firstname as approvername,date_format(a.approved_date, '%d-%m-%Y') as approved_date,a.materialrequisition_status,date_format(a.expected_date, '%d-%m-%Y') as expected_date " +
                     " from ims_trn_tmaterialrequisition a " +
                     " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                     " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                     " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                     " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid " +
                     " left join pmr_mst_tcostcenter f on f.costcenter_gid = a.costcenter_gid " +
                     " left join adm_mst_tuser g on g.user_gid = a.approved_by " +
                     "  left join ims_trn_tmaterialrequisitiondtl k on a.materialrequisition_gid = k.materialrequisition_gid " +
                     "  left join adm_mst_tuser i on k.requested_by = i.user_gid " +
                     " where a.materialrequisition_gid = '" + materialrequisition_gid + "' group by materialrequisition_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialindentview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new materialindentview_list
                        {
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                            materialrequisition_remarks = dt["materialrequisition_remarks"].ToString(),
                            materialrequisition_status = dt["materialrequisition_status"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            requested_by = dt["requested_by"].ToString(),
                            materialrequisition_date = dt["materialrequisition_date"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            budget_available = dt["budget_available"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            approver_remarks = dt["approver_remarks"].ToString(),
                            approvername = dt["approvername"].ToString(),
                            materialrequisition_type = dt["materialrequisition_type"].ToString(),
                            approved_date = dt["approved_date"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            priority = dt["priority"].ToString(),
                            
                            

                        });
                        values.materialindentview_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Material Indent!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetMaterialIndentViewProduct(string materialrequisition_gid, MdlImsTrnMaterialindent values)
        {
            try
            {
                msSQL = " select a.product_remarks,a.qty_requested, a.qty_issued, (a.qty_requested - a.qty_issued) AS pending_qty,  b.product_name," +
                        " b.product_code, c.productgroup_name, f.productuom_name,  date_format(e.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date," +
                        " a.display_field  from ims_trn_tmaterialrequisitiondtl a  " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid  " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid  " +
                        " left join pmr_mst_tproductuomclass d on d.productuomclass_gid = b.productuomclass_gid " +
                        " left join pmr_mst_tproductuom f on f.productuom_gid = b.productuom_gid  " +
                        " left join ims_trn_tmaterialrequisition e on e.materialrequisition_gid = a.materialrequisition_gid  " +
                        " where a.materialrequisition_gid = '"+ materialrequisition_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialindentviewproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new materialindentviewproduct_list
                        {
                           
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_issued = dt["qty_issued"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            pending_qty = dt["pending_qty"].ToString(),
                        });
                        values.materialindentviewproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Material Indent!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetMIrequest(string materialrequisition_gid, MdlImsTrnMaterialindent values)
        {
            try
            {

                msSQL = "  select  a.materialrequisition_gid,date_format(a.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date, a.approver_remarks," +
                        "  case when a.priority ='Y' then 'High' else 'Low' end as priority," +
                        "  a.materialrequisition_type,a.materialrequisition_remarks,a.materialrequisition_reference,e.branch_name,a.branch_gid, concat(b.user_firstname,' - ',b.user_lastname) as user_firstname," +
                        "  d.department_name, a.costcenter_gid,format(f.budget_available, 2) as budget_available,concat_ws('-',f.costcenter_name,f.costcenter_code) as costcenter_name," +
                        "  g.user_firstname as approvername,date_format(a.approved_date,'%d-%m-%Y' ) as approved_date,a.materialrequisition_status,date_format(a.expected_date,'%d-%m-%Y') as expected_date " +
                        "  from ims_trn_tmaterialrequisition a  " +
                        "  left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        "  left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                        "  left join hrm_mst_tdepartment d on c.department_gid = d.department_gid" +
                        "  left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid" +
                        "  left join pmr_mst_tcostcenter f on f.costcenter_gid = a.costcenter_gid"+
                        "  left join adm_mst_tuser g on g.user_gid = a.approved_by" +
                        "  where a.materialrequisition_gid ='"+ materialrequisition_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<issueequest_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new issueequest_list
                        {
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialrequisition_date = dt["materialrequisition_date"].ToString(),
                            approver_remarks = dt["approver_remarks"].ToString(),
                            materialrequisition_type = dt["materialrequisition_type"].ToString(),
                            materialrequisition_remarks = dt["materialrequisition_remarks"].ToString(),
                            materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            costcenter_gid = dt["costcenter_gid"].ToString(),
                            budget_available = dt["budget_available"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            approvername = dt["approvername"].ToString(),
                            approved_date = dt["approved_date"].ToString(),
                            materialrequisition_status = dt["materialrequisition_status"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            priority = dt["priority"].ToString(),
                        });
                        values.issueequest_list = getModuleList;
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
        public void DaGetproductMIrequest(string branch_gid, string materialrequisition_gid, MdlImsTrnMaterialindent values)
        {
            try
            {
                msSQL = " select MR_DTL.product_gid,MR_DTL.product_code,MR_DTL.product_name ,(MR_DTL.qty_requested) as req_qty,g.productgroup_name,p.product_desc,  " +
                        " MR.branch_gid,MR_DTL.materialrequisition_gid,MR_DTL.productuom_gid,MR_DTL.productuom_name,  " +
                        " MR_DTL.display_field, MR_DTL.materialrequisitiondtl_gid from " +
                        " ims_trn_tmaterialrequisitiondtl MR_DTL " +
                        " left join ims_trn_tmaterialrequisition MR on MR.materialrequisition_gid = MR_DTL.materialrequisition_gid " +
                        " left join pmr_mst_tproduct P on P.product_gid = MR_DTL.product_gid " +
                        " left join pmr_mst_tproductuom c on c.productuom_gid = MR_DTL.productuom_gid " +
                        " left join pmr_mst_tproductgroup g on g.productgroup_gid=p.productgroup_gid"+
                        " where MR.branch_gid = '" + branch_gid + "' and " +
                        " MR.materialrequisition_gid = '" + materialrequisition_gid + "' and(MR_DTL.qty_requested - MR_DTL.qty_issued) > 0 " +
                        " group by MR_DTL.product_gid,MR_DTL.productuom_gid,MR_DTL.display_field ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productrequest_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        msSQL =" SELECT sum(qty_issued) as qty_issued  FROM ims_trn_tmaterialrequisitiondtl a " +
                               " left join ims_trn_tmaterialrequisition b on a.materialrequisition_gid = b.materialrequisition_gid " +
                               " where a.product_gid = '" + dt["product_gid"].ToString() + "' " +
                               " and a.productuom_gid = '"+ dt["productuom_gid"].ToString() + "' " +
                               " and a.materialrequisition_gid = '"+ materialrequisition_gid + "' " +
                               " and b.branch_gid = '"+ branch_gid + "' " +
                               " group by a.product_gid, a.productuom_gid";
                        string qty_issued = objdbconn.GetExecuteScalar(msSQL);
                        if (qty_issued == "")
                        {
                            qty_issued = "0.0";
                        }

                        msSQL = " SELECT sum((a.qty_requested)-(a.qty_issued)) as pending_mr  FROM ims_trn_tmaterialrequisitiondtl a " +
                                " left join ims_trn_tmaterialrequisition b on a.materialrequisition_gid = b.materialrequisition_gid " +
                                " where a.product_gid='"+ dt["product_gid"].ToString() + "'" +
                                " and a.productuom_gid = '"+ dt["productuom_gid"].ToString() + "'" +
                                " and b.branch_gid = '"+ branch_gid + "'" +
                                " group by a.product_gid, a.productuom_gid ";
                        string pending_mr = objdbconn.GetExecuteScalar(msSQL);
                        if (string.IsNullOrEmpty(pending_mr))
                        {
                            pending_mr = "0.0";
                        }
                        msSQL = " SELECT sum((qty_requested)-(qty_received)) as pending_pr  FROM pmr_trn_tpurchaserequisitiondtl a " +
                                " left join pmr_trn_tpurchaserequisition b on a.purchaserequisition_gid = b.purchaserequisition_gid  " +
                                " where a.product_gid='"+ dt["product_gid"].ToString() + "' " +
                                " and a.uom_gid = '"+ dt["productuom_gid"].ToString() + "' " +
                                " and b.branch_gid = '"+ branch_gid + "'" +
                                " and b.purchaserequisition_flag <> 'PR Rejected' " +
                                " and b.purchaseorder_flag <> 'PO Raised'" +
                                " group by a.product_gid, a.uom_gid ";
                        string pending_pr = objdbconn.GetExecuteScalar(msSQL);
                        if (string.IsNullOrEmpty(pending_pr))
                        {
                            pending_pr = "0.0";
                        }
                        msSQL = " SELECT sum(a.stock_qty+amend_qty-a.damaged_qty-a.issued_qty-transfer_qty) as stock_quantity FROM ims_trn_tstock a  " +
                                " where a.product_gid='"+ dt["product_gid"].ToString() + "'" +
                                " and a.uom_gid = '"+ dt["productuom_gid"].ToString() + "'" +
                                " and a.branch_gid = '"+ branch_gid + "' and a.stock_flag='Y' " +
                                " group by a.product_gid, a.uom_gid ";
                        string stock_quantity = objdbconn.GetExecuteScalar(msSQL);
                        if (string.IsNullOrEmpty(stock_quantity))
                        {
                            stock_quantity = "0.0";
                        }
                        double pendingmr = Convert.ToDouble(pending_mr);
                        double pendingpr = Convert.ToDouble(pending_pr);
                        double stockquantity = Convert.ToDouble(stock_quantity);
                        double qty = ((stockquantity + pendingpr) - pendingmr);
                        getModuleList.Add(new productrequest_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            req_qty = dt["req_qty"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            materialrequisitiondtl_gid = dt["materialrequisitiondtl_gid"].ToString(),
                            qty_issued= qty_issued,
                            pending_mr = pending_mr,
                            pending_pr = pending_pr,
                            stock_quantity = stock_quantity,
                            Avl_qty = qty.ToString(),
                        });
                        values.productrequest_list = getModuleList;
                    }

                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading issue Request Product list!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaMIrequest(string user_gid, issuematerialrequest_list values)
        {
            try
            {
                for (int i = 0; i < values.productrequest_list.Count; i++)
                {
                    if (!string.IsNullOrEmpty(values.productrequest_list[i].issuerequestqty))
                    {
                        msSQL = " select qty_requested, qty_issued, issuerequestqty from ims_trn_tmaterialrequisitiondtl a " +
                                " where a.materialrequisitiondtl_gid = '" + values.productrequest_list[i].materialrequisitiondtl_gid + "'" +
                                " and a.product_gid = '" + values.productrequest_list[i].product_gid + "'";
                        objODBCDataReader = objdbconn.GetDataReader(msSQL);
                        if (objODBCDataReader.HasRows)
                        {
                            if (objODBCDataReader.Read())
                            {
                                lsqty_requested = objODBCDataReader["qty_requested"].ToString();
                                lsqty_issued = objODBCDataReader["qty_issued"].ToString();
                                lsissuenow_qty = objODBCDataReader["issuerequestqty"].ToString();
                                objODBCDataReader.Close();
                            }
                        }
                        msSQL = " Update ims_trn_tmaterialrequisition " +
                                " Set issue_status = 'Request For Issue'," +
                                " approver_remarks = '" + values.remarks.Replace("'", "\\\'") + "'" +
                                " where materialrequisition_gid = '" + values.productrequest_list[i].materialrequisition_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = " Update ims_trn_tmaterialrequisitiondtl Set " +
                                    " requeststatus = 'IssueNow', " +
                                    " issuerequestqty = '" + values.productrequest_list[i].issuerequestqty + "'" +
                                    " where materialrequisition_gid = '" + values.productrequest_list[i].materialrequisition_gid + "'" +
                                    " and materialrequisitiondtl_gid = '" + values.productrequest_list[i].materialrequisitiondtl_gid + "'";
                            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                    }
                }
                if (mnResult1 != 0)
                {
                    values.status = true;
                    values.message = "Issue Request Raised Successfully!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While issue Request!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading issue Request updating!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }


        public void DaGetMIViewProduct(string materialrequisition_gid, MdlImsTrnMaterialindent values)
        {
            try
            {

                msSQL = "  SELECT a.materialrequisition_gid,a.materialrequisitiondtl_gid, a.product_gid,a.product_code, a.productuom_name,  " +
                         " a.qty_requested, b.product_name, c.productgroup_name,  " +
                         " (select ifnull(sum(m.stock_qty) + sum(m.amend_qty) - sum(m.damaged_qty) - sum(m.issued_qty) - sum(m.transfer_qty), 0) as available_quantity from ims_trn_tstock m where " +
                         " m.stock_flag = 'Y' and m.product_gid = a.product_gid and m.branch_gid = d.branch_gid and a.productuom_gid = m.uom_gid) as available_quantity " +
                         " FROM ims_trn_tmaterialrequisitiondtl a " +
                         " left join ims_trn_tmaterialrequisition d on d.materialrequisition_gid = a.materialrequisition_gid " +
                         " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                         " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                         " left join pmr_mst_tproductuom g on a.productuom_gid = g.productuom_gid " +
                         " where a.materialrequisition_gid = '" + materialrequisition_gid + "'" +
                         "  order by a.materialrequisitiondtl_gid asc ";
                      

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productsummary_list
                        {
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialrequisitiondtl_gid = dt["materialrequisitiondtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            available_quantity = dt["available_quantity"].ToString(),
                        });
                        values.productsummary_list = getModuleList;
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

        public void DaMaterialIndentApprove(string materialrequisition_gid,string user_gid, MdlImsTrnMaterialindent values)
        {
            try
            {
                msSQL = " update ims_trn_tmaterialrequisition set " +
                        " materialrequisition_status = 'Approved'," +
                        " approved_by='" + user_gid+  "'," +
                        " approved_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " where materialrequisition_gid = '" + materialrequisition_gid + "'";
                    
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " Material Indent Approved Successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while approve material indent.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception error occured while!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaMaterialIndentReject(string materialrequisition_gid, string user_gid, MdlImsTrnMaterialindent values)
        {
            try
            {
                msSQL = " update ims_trn_tmaterialrequisition set " +
                        " materialrequisition_status = 'Rejected'," +
                        " approved_by='" + user_gid + "'," +
                        " approved_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " where materialrequisition_gid = '" + materialrequisition_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " Material Indent Rejected !...";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while reject material indent.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception error occured while!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetMitoPO(string materialrequisition_gid, MdlImsTrnMaterialindent values)
        {
            try
            {
                msSQL = " select  date_format(a.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date, a.purchaserequisition_gid,g.qty_requested,a.purchaserequisition_flag as purchaserequisition_status," +
                        " g.purchaserequisitiondtl_gid from pmr_trn_tpurchaserequisition a  " +
                        " left join ims_trn_tmaterialrequisition b on a.materialrequisition_gid = b.materialrequisition_gid  " +
                        " left join pmr_trn_tpurchaserequisitiondtl g on a.purchaserequisition_gid = g.purchaserequisition_gid  " +
                        " where a.materialrequisition_gid = '"+ materialrequisition_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Mitopo_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Mitopo_list
                        {
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            purchaserequisition_status = dt["purchaserequisition_status"].ToString(),
                            purchaserequisitiondtl_gid = dt["purchaserequisitiondtl_gid"].ToString(),
                        });
                        values.Mitopo_list = getModuleList;
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

        public void DaGetPRtoPayment(string materialrequisition_gid, MdlImsTrnMaterialindent values)
        {
            try
            {
                msSQL = " select a.purchaserequisition_gid, "+
                         " case when b.purchaseorder_gid is null then '--' else b.purchaseorder_gid end as purchaseorder_gid , "+
                         " case when c.grn_gid is null then '--' else c.grn_gid end as grn_gid , " +
                         " case when d.invoice_gid is null then '--' else d.invoice_gid end as invoice_gid , " +
                         " case when e.payment_gid is null then '--' else e.payment_gid end as payment_gid , " +
                         " CASE when a.grn_flag<> 'GRN Pending' then a.grn_flag " +
                         " when a.purchaseorder_flag<> 'PO Pending' then a.purchaseorder_flag " +
                         " else a.purchaserequisition_flag end as 'overall_status' " +
                         " from pmr_trn_tpurchaserequisition a " +
                         " left join pmr_trn_tpurchaseorder b on a.purchaserequisition_gid = b.purchaserequisition_gid " +
                         " left join pmr_trn_tgrn c on b.purchaseorder_gid = c.purchaseorder_gid " +
                         " left join acp_trn_tpo2invoice d on b.purchaseorder_gid = d.purchaseorder_gid " +
                         " left join acp_trn_tinvoice2payment e on d.invoice_gid = e.invoice_gid " +
                         " where a.materialrequisition_gid = '"+ materialrequisition_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Potopayment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Potopayment_list
                        {
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            grn_gid = dt["grn_gid"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            payment_gid = dt["payment_gid"].ToString(),
                            grn_flag = dt["grn_flag"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
  
                        });
                        values.Potopayment_list = getModuleList;
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
        //MI Count

        public void DaGetMICount(MdlImsTrnMaterialindent values)
        {
            try
            {

                msSQL = " SELECT COUNT(CASE  WHEN h.qty_requested <= (SELECT IFNULL(SUM(m.stock_qty) + SUM(m.amend_qty) - SUM(m.damaged_qty) - SUM(m.issued_qty) - SUM(m.transfer_qty), 0) FROM ims_trn_tstock m  WHERE m.stock_flag = 'Y' "+
                        " AND m.product_gid = h.product_gid AND m.branch_gid = a.branch_gid AND h.productuom_gid = m.uom_gid) THEN 1 ELSE NULL END) AS available_count,(select count(materialrequisition_gid)  from ims_trn_tmaterialrequisition)as totalcount, "+
                        "  (select count(materialrequisition_gid)  from ims_trn_tmaterialrequisition where priority = 'Y') as prioritycount " +
                        " FROM ims_trn_tmaterialrequisition a  " +
                        " LEFT JOIN pmr_mst_tcostcenter g ON a.costcenter_gid = g.costcenter_gid " +
                        " LEFT JOIN adm_mst_tuser b ON a.user_gid = b.user_gid " +
                        " LEFT JOIN hrm_mst_temployee c ON c.user_gid = b.user_gid " +
                        " LEFT JOIN hrm_mst_tdepartment d ON c.department_gid = d.department_gid " +
                        " LEFT JOIN pmr_mst_tproducttype e ON a.materialrequisition_type = e.producttype_gid " +
                        " LEFT JOIN hrm_mst_tbranch f ON f.branch_gid = a.branch_gid " +
                        " LEFT JOIN ims_trn_tmaterialrequisitiondtl h ON h.materialrequisition_gid = a.materialrequisition_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<MICount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new MICount
                        {

                            totalcount = dt["totalcount"].ToString(),
                            prioritycount = dt["prioritycount"].ToString(),
                            available_count = dt["available_count"].ToString(),

                        });
                        values.MICount = getModuleList;
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
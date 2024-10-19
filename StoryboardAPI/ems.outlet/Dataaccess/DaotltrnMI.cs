using ems.outlet.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.outlet.Dataaccess
{
    public class DaotltrnMI
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        public void DaMatrialIndentsummary(string user_gid,MdlotltrnMI values)
        {
            try
            {

                msSQL = " select distinct a.materialrequisition_gid,a.materialrequisition_reference, a.materialrequisition_remarks, a.materialrequisition_gid as material, " +
                        " case when a.materialrequisition_status = 'ApproveToIssue' " +
                        " then 'Approved To Issue' else a.materialrequisition_status end as materialrequisition_status,b.user_firstname,  " +
                        " date_format(a.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date,a.mrbapproval_remarks,g.costcenter_name, " +
                        " concat(f.branch_name, '/', d.department_name) as department_name, date_format(a.created_date,'%d-%m-%Y') as created_date,date_format(a.expected_date,'%d-%m-%Y') as expected_date " +
                        " from ims_trn_tmaterialrequisition a " +
                        " left join pmr_mst_tcostcenter g on a.costcenter_gid = g.costcenter_gid " +
                        " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                        " left join pmr_mst_tproducttype e on a.materialrequisition_type = e.producttype_gid " +
                        " left join hrm_mst_tbranch f on f.branch_gid = a.branch_gid " +
                        " where 1 = 1 and a.user_gid='"+ user_gid +"' Order by date(a.materialrequisition_date)desc, a.materialrequisition_date asc, a.materialrequisition_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialindent_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
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
                        });
                        values.MIoutletsummary_list = getModuleList;
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

        public void DaGetMaterialIndentView(string materialrequisition_gid, MdlotltrnMI values)
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
                     " left join ims_trn_tmaterialrequisitiondtl k on a.materialrequisition_gid = k.materialrequisition_gid " +
                     "  left join adm_mst_tuser i on k.requested_by = i.user_gid " +
                     " where a.materialrequisition_gid = '" + materialrequisition_gid + "'  group by materialrequisition_gid ";

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
        public void DaGetMaterialIndentViewProduct(string materialrequisition_gid, MdlotltrnMI values)
        {
            try
            {

                msSQL = " select a.product_remarks, Format(a.qty_requested,2) as qty_requested, Format(a.qty_issued,2) as qty_issued, " +
                " b.product_name, b.product_code, c.productgroup_name, f.productuom_name, " +
                " date_format(e.materialrequisition_date,'%d-%m-%Y') as materialrequisition_date,a.display_field " +
                " from ims_trn_tmaterialrequisitiondtl a " +
                " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                " left join pmr_mst_tproductuomclass d on d.productuomclass_gid = b.productuomclass_gid" +
                " left join pmr_mst_tproductuom f on f.productuom_gid = b.productuom_gid " +
                " left join ims_trn_tmaterialrequisition e on e.materialrequisition_gid = a.materialrequisition_gid " +
               " where a.materialrequisition_gid = '" + materialrequisition_gid + "'";


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




    }
}
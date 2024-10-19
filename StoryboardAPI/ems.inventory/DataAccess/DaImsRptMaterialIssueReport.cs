using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaImsRptMaterialIssueReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsMainbranch_flag, lsbranch_gid;

        public void DaGetImsRptMaterialissuereport(string employee_gid,MdlImsRptMaterialIssueReport values)
        {
            try
            {
                msSQL = " select distinct a.materialissued_gid, a.materialrequisition_gid, a.materialissued_status,h.costcenter_name, " +
                " date_format(a.materialissued_date, '%d-%m-%Y') as materialissued_date,concat(e.user_firstname,'/',d.department_name) as issued_to," +
                " b.user_firstname, d.department_name, a.branch_gid, g.branch_name,f.materialrequisition_reference" +
                " from ims_trn_tmaterialissued a " +
                " Left join ims_trn_tmaterialrequisition f on f.materialrequisition_gid = a.materialrequisition_gid " +
                " Left join pmr_mst_tcostcenter h on h.costcenter_gid = f.costcenter_gid " +
                " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                " left join adm_mst_tuser e on e.user_gid = f.user_gid " +
                " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                " left join hrm_mst_tbranch g on a.branch_gid = g.branch_gid" +
                " where 1=1  and c.employee_gid ='"+ employee_gid+"'  " +
                " order by  date(a.materialissued_date) desc,a.materialissued_date asc, a.materialissued_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialissue_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new materialissue_list
                        {
                            materialissued_gid = dt["materialissued_gid"].ToString(),
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialissued_date = dt["materialissued_date"].ToString(),
                            materialissued_status = dt["materialissued_status"].ToString(),
                            issued_to = dt["issued_to"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            materialrequisition_reference = dt["materialrequisition_reference"].ToString(),
                        });
                        values.materialissue_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Product Issued Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }
}
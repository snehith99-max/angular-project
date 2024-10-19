using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.outlet.Models;

namespace ems.outlet.Dataaccess
{
    public class DaSysMstExpense
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsexpense_code;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        int mnResult6;

        public void DaGetExpenseCategorySummary(MdlSysMstExpense values)
        {
            msSQL = " select a.expense_gid, a.expense_code, a.expense_name, a.expense_desc " +
                    " from sys_mst_texpensecategory a " +
                    " order by expense_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<expensecategory_listdata>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new expensecategory_listdata
                    {
                        expense_gid = dt["expense_gid"].ToString(),
                        expense_code = dt["expense_code"].ToString(),
                        expense_data = dt["expense_name"].ToString(),
                        expense_desc = dt["expense_desc"].ToString(),

                    });
                    values.expensecategory_listdata = getModuleList;
                }
            }
            dt_datatable.Dispose();

        }

        public void DaPostExpense(string user_gid, expensecategory_listdata values)
        {
            try
            {

                msSQL = " select * from sys_mst_texpensecategory where expense_name = '" + values.expense_data + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Expense Name Already Exist !!";
                }
                else
                {

                    msGetGid = objcmnfunctions.GetMasterGID("ECRN");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='ECRN' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lsexpense_code = "ECC" + "000" + lsCode;


                    msGetGid = objcmnfunctions.GetMasterGID("ECRN");

                    msSQL = " insert into sys_mst_texpensecategory(" +
                          " expense_gid," +
                          " expense_code," +
                          " expense_name," +
                          " expense_desc," +
                          " created_by, " +
                          " created_date)" +
                          " values(" +
                          " '" + msGetGid + "'," +
                          " '" + lsexpense_code + "'," +
                          "'" + values.expense_data + "'," +
                          " '" + values.expense_desc + "',";


                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Expense Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Expense";
                    }
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Inserting Expense Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaDeleteExpense(string expense_gid, expensecategory_listdata values)
        {
            try
            {
                msSQL = "select expense_amount from sys_mst_tdaytrackerdtl where expense_gid='"+ expense_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {

                    values.status = false;
                    values.message = "Cannot delete expense detail due to daytracker raised";

                }
                else
                {

                    msSQL = "  delete from sys_mst_texpensecategory where expense_gid='" + expense_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Expense Deleted Successfully";
                    }
                    else
                    {

                        values.status = false;
                        values.message = "Error While Deleting Expense";

                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Expense Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }


    }
}
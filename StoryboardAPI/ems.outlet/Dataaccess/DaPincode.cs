using ems.outlet.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.outlet.Dataaccess
{
    public class DaPincode
    {
        string msSQL, msGetGID, symbol = string.Empty;
        int mnResult;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        DataTable dt_table = new DataTable();
        public void DaPincodeSummary(string branch_gid,MdlPincode values)
        {
            try
            {
                msSQL = " select symbol from crm_trn_tcurrencyexchange where default_currency='Y'";
                symbol = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select a.pincode_code, a.pincode_id, a.created_date,a.branch_gid," +
                    " format(a.deliverycost,2) as deliverycost, a.created_by, b.branch_name as branch_name" +
                    " from adm_mst_tpincode a" +
                    " left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid ";
                dt_table = objdbconn.GetDataTable(msSQL);
                var Getpincode = new List<Getpincode_list>();
                if (dt_table.Rows.Count > 0)
                {
                    foreach(DataRow dr in dt_table.Rows)
                    {
                        Getpincode.Add(new Getpincode_list
                        {
                            pincode_code = dr["pincode_code"].ToString(),
                            pincode_id = dr["pincode_id"].ToString(),
                            branch_gid = dr["branch_gid"].ToString(),
                            deliverycost = dr["deliverycost"].ToString(),
                            branch_name = dr["branch_name"].ToString(),
                            symbol = symbol,
                        });
                        values.Getpincode_list = Getpincode;
                    }
                }
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while loading Pincode data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + 
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + 
                "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + 
                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Pincode/" + "Log" + 
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        
        public void DaGetbranchdetails(MdlPincode values)
        {
            msSQL = " select branch_name, branch_gid from hrm_mst_tbranch";
            dt_table = objdbconn.GetDataTable(msSQL);
            var Getpincode = new List<Getpincode_list>();
            if (dt_table.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_table.Rows)
                {
                    Getpincode.Add(new Getpincode_list
                    {                                                
                        branch_gid = dr["branch_gid"].ToString(),                        
                        branch_name = dr["branch_name"].ToString(),                        
                    });
                    values.Getpincode_list = Getpincode;
                }
            }

        }
        public void DaPostpincode(string user_gid, Pincode_list values)
        {
            try
            {
                msSQL = "insert into adm_mst_tpincode (" +
                   " pincode_code, " +
                   " deliverycost, " +
                   " branch_gid, " +
                   " created_by, " +
                   " created_date " +
                   " ) values (" +
                   "'" + values.pincode + "'," +
                   "'" + values.deliverycost + "'," +
                   "'" + values.branch_gid + "'," +
                   "'" + user_gid + "'," +
                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')"
                   ;
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Pincode add successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while adding Pincode.";
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while loading Pincode submit data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                "***********" + ex.Message.ToString() + "***********" + values.message.ToString() +
                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Pincode/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }            
        } 
        public void Deletepincode(string pincode_id,MdlPincode values)
        {
            try
            {
                msSQL = " delete from adm_mst_tpincode where pincode_id='" + pincode_id + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0) 
                {
                    values.status = true;
                    values.message = " Pincode deleted successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while deleting pincode.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Pincode submit data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                "***********" + ex.Message.ToString() + "***********" + values.message.ToString() +
                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Pincode/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
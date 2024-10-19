using ems.outlet.Models;
using ems.system.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.outlet.Dataaccess
{
    public class DaDeliveryCost
    {
        string msSQL, msGetGID = string.Empty;
        int mnResult;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunction = new cmnfunctions();
        DataTable dt_table = new DataTable();

        public void DaGetDeliveryCostSummary(string branch_gid, MdlDeliveryCost values)
        {
            try
            {
                msSQL = "select a.deliverybase_cost, a.deliverycost_id, concat(b.user_firstname,' ',b.user_lastname)as created_by from adm_mst_tdeliverycost a "+
                    " left join adm_mst_tuser b on b.user_gid=a.created_by" +
                    " where branch_gid='" + branch_gid + "'";
                dt_table = objdbconn.GetDataTable(msSQL);
                var GetDeliverycost = new List<GetDeliverycost_list>();
                if (dt_table.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_table.Rows)
                    {
                        GetDeliverycost.Add(new GetDeliverycost_list
                        {
                            deliverybase_cost = dt["deliverybase_cost"].ToString(),
                            deliverycost_id = dt["deliverycost_id"].ToString(),
                            created_by = dt["created_by"].ToString(),
                        });
                        values.GetDeliverycost_list = GetDeliverycost;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading DeliveryCost data!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                "***********" + ex.Message.ToString() + "***********" + values.message.ToString() +
                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Pincode/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostDeliveryCost(string user_gid,string branch_gid , PostDeliverycost_list values)
        {
            try
            {
                msSQL = " insert into adm_mst_tdeliverycost ( " +
                     " deliverybase_cost," +
                     " branch_gid," +
                     " created_date," +
                     " created_by" +
                     " ) values (" +
                     "'" + values.deliverybase_cost + "'," +
                     "'" + branch_gid + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                     "'" + user_gid + "')"; 
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult ==  1)
                {
                    values.status = true;
                    values.message = "Deliverycost added successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while adding Deliverycost.";
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while Insert DeliveryCost data!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                "***********" + ex.Message.ToString() + "***********" + values.message.ToString() +
                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Pincode/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetPincodeSummaryAssign(string branch_gid, MdlDeliveryCost values)
        {
            try
            {
                msSQL = "select pincode_code, pincode_id, created_date,branch_gid, created_by " +
                  " from adm_mst_tpincode where branch_gid='" + branch_gid + "' and pincode_id" +
                  " not in (select pincode_id from otl_trn_tdeliverycost2pincode where branch_gid='" + branch_gid + "')";
                var GetPincodeSummaryAssign = new List<GetPincodeSummaryAssign>();
                dt_table = objdbconn.GetDataTable(msSQL);
                if(dt_table.Rows.Count > 0)
                {
                    foreach(DataRow row in dt_table.Rows)
                    {
                        GetPincodeSummaryAssign.Add(new Models.GetPincodeSummaryAssign
                        {
                            created_by = row["created_by"].ToString(),
                            pincode_code = row["pincode_code"].ToString(),
                            pincode_id = row["pincode_id"].ToString(),
                            created_date = row["created_date"].ToString(),
                            branch_gid = row["branch_gid"].ToString(),
                        });
                        values.GetPincodeSummaryAssign = GetPincodeSummaryAssign;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Insert DeliveryCost data!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                "***********" + ex.Message.ToString() + "***********" + values.message.ToString() +
                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Pincode/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void PostAssignPincode2deliverycost(string branch_gid, string user_gid, PostAssignPincodedelivery_list values)
        {
            try
            {
                msGetGID = objcmnfunction.GetMasterGID("DCPI");
                for (int i= 0; i < values.deliverypincodeassing.ToArray().Length; i++)
                {
                    msSQL = " insert into otl_trn_tdeliverycost2pincode (" +
                        " deliverycost2pincode_gid," +
                        " branch_gid," +
                        " pincode_id," +
                        " deliverycost_id," +
                        " created_by," +
                        " created_date" +
                        ") values (" +
                        "'" + msGetGID + "'," +
                        "'" + branch_gid + "'," +
                        "'" + values.deliverypincodeassing[i].pincode_id + "'," +
                        "'" + values.deliverycost_id + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Pincode assigned successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Pincode Assign";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Insert DeliveryCost data!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                "***********" + ex.Message.ToString() + "***********" + values.message.ToString() +
                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Pincode/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
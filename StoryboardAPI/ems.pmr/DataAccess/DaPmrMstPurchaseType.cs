using ems.pmr.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
namespace ems.pmr.DataAccess
{
    public class DaPmrMstPurchaseType
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader, objODBCDatareader1;
        DataTable dt_datatable, dt_datatable1;
        string msGetGid, msGetGid1, msGetGid2, mscusconGetGID, msleadbankGetGID, msleadbankconGetGID, msGetGidfil, lsmodule_name, lspurchasetype_gid, lspurchasetype_name, lsasset_status, lsproduct_name, lsassetdtl_gid, lssasset_status;
        int mnResult;

        public void DaGetpurchaseType(string user_gid, MdlPmrMstPurchasetype values)
        {
            msSQL = " Select a.purchasetype_gid,a.purchasetype_code,a.purchasetype_name,b.account_name,b.account_gid " +
                    " from Pmr_trn_tpurchasetype a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " order by purchasetype_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var GetpurchaseType_List = new List<GetpurchaseType_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetpurchaseType_List.Add(new GetpurchaseType_List
                    {
                        purchasetype_gid = dt["purchasetype_gid"].ToString(),
                        purchasetype_code = dt["purchasetype_code"].ToString(),
                        purchasetype_name = dt["purchasetype_name"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.GetpurchaseType_List = GetpurchaseType_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostPmrpurchaseType(string user_gid, GetpurchaseType_List values)
        {
            try
            {
                msSQL = " select purchasetype_name from Pmr_trn_tpurchasetype " +
                       " where purchasetype_name  = '" + values.purchasetype_name.Replace("'", "\\\'") + "'; ";
                //msSQL = " SELECT purchasetype_name FROM Pmr_trn_tpurchasetype " +
                //       " WHERE purchasetype_name = '" + values.purchasetype_name.Replace("'", "''") + "';";

                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows == false)
                {

                    msSQL = " select purchasetype_code from Pmr_trn_tpurchasetype " +
                           " where purchasetype_code  = '" + values.purchasetype_code + "'";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows == true)
                    {
                        values.status = false;
                        values.message = "Purchase Type Code Already Exist !!";
                    }
                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("STPM");
                        msSQL = " insert into Pmr_trn_tpurchasetype  (" +
                                " purchasetype_gid," +
                                " purchasetype_name," +
                                " purchasetype_code," +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                "'" + msGetGid + "',";
                                if(!string.IsNullOrEmpty(values.purchasetype_name) && values.purchasetype_name.Contains("'"))
                                {
                                    msSQL +="'"+values.purchasetype_name.Replace("'","\\\'")+"', ";
                                }
                                else
                                {
                                    msSQL +="'"+values.purchasetype_name+"', ";
                                }
                                msSQL +="'"+values.purchasetype_code+"'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Purchase Type Details Added Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Inserting purchase Type Details !!";
                        }
                    }
                    objODBCDatareader.Close();
                }
                else
                {
                    values.status = false;
                    values.message = "Purchase Type Name Already Exist !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Inserting purchase Type Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdatePmrpurchaseType(string user_gid, GetpurchaseType_List values)
        {
            try
            {
                msSQL = " SELECT purchasetype_name,purchasetype_gid FROM Pmr_trn_tpurchasetype WHERE purchasetype_name='" + values.purchasetype_name.Replace("'", "\\\'") + "'  and purchasetype_gid='" + values.purchasetype_gid + "' ";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    msSQL = " update Pmr_trn_tpurchasetype  set ";
                    if (!string.IsNullOrEmpty(values.purchasetype_name) && values.purchasetype_name.Contains("'"))
                    {
                        msSQL += "purchasetype_name = '" + values.purchasetype_name.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "purchasetype_name = '" + values.purchasetype_name + "', ";
                    }
                    msSQL += " updated_by = '" + user_gid + "'," +
                           " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                           " where purchasetype_gid='" + values.purchasetype_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }

                else
                {

                    msSQL = " SELECT purchasetype_name FROM Pmr_trn_tpurchasetype WHERE purchasetype_name='" + values.purchasetype_name.Replace("'", "\\\'") + " '; ";
                    dt_datatable1 = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable1.Rows.Count > 0)
                    {
                        values.status = false;
                        values.message = "Purchase type name already exist";
                        return;
                    }

                    msSQL = " update Pmr_trn_tpurchasetype  set ";
                    if (!string.IsNullOrEmpty(values.purchasetype_name) && values.purchasetype_name.Contains("'"))
                    {
                        msSQL += "purchasetype_name = '" + values.purchasetype_name.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "purchasetype_name = '" + values.purchasetype_name + "', ";
                    }
                    msSQL += " updated_by = '" + user_gid + "'," +
                           " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                           " where purchasetype_gid='" + values.purchasetype_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Purchase Type Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating purchase Type !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating purchase Type";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetDeletepurchasetype(string purchasetype_gid, GetpurchaseType_List values)
        {
            try
            {
                msSQL = "  delete from Pmr_trn_tpurchasetype where purchasetype_gid='" + purchasetype_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Purchase Type Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting purchase Type";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting purchase Type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }

}

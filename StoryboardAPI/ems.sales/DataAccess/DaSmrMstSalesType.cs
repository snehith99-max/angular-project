using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ems.sales.DataAccess
{
    public class DaSmrMstSalesType
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader, objODBCDatareader1;
        DataTable dt_datatable, dt_datatable1;
        string msGetGid, msGetGid1, msGetGid2, mscusconGetGID, msleadbankGetGID, msleadbankconGetGID, msGetGidfil, lsmodule_name, lssalestype_gid, lssalestype_name, lsasset_status, lsproduct_name, lsassetdtl_gid, lssasset_status;
        int mnResult;

        public void DaGetsalesType(string user_gid, MdlSmrMstSalestype values)
        {
            msSQL = " Select a.salestype_gid,a.salestype_code,a.salestype_name,b.account_name,b.account_gid " +
                    " from smr_trn_tsalestype a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " order by salestype_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var GetSalesType_List = new List<GetSalesType_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetSalesType_List.Add(new GetSalesType_List
                    {
                        salestype_gid = dt["salestype_gid"].ToString(),
                        salestype_code = dt["salestype_code"].ToString(),
                        salestype_name = dt["salestype_name"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.GetSalesType_List = GetSalesType_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostSmrSalesType(string user_gid, GetSalesType_List values)
        {
            try
            {
                msSQL = " select salestype_name from smr_trn_tsalestype " +
                        " where salestype_name  = '" + values.salestype_name + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows == false)
                {

                    msSQL = " select salestype_code from smr_trn_tsalestype " +
                           " where salestype_code  = '" + values.salestype_code + "'";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows == true)
                    {
                        values.status = false;
                        values.message = "Sales Type Code Already Exist !!";
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
                            values.message = "Sales Type Details Added Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Inserting Sales Type Details !!";
                        }
                    }
                    objODBCDatareader.Close();
                }
                else
                {
                    values.status = false;
                    values.message = "Sales Type Name Already Exist !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Inserting Sales Type Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateSmrSalesType(string user_gid, GetSalesType_List values)
        {
            try
            {

                msSQL = " SELECT salestype_name FROM smr_trn_tsalestype WHERE salestype_name='" + values.salestype_name + "' ";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Sales type name already exist";
                    return;
                }

                msSQL = " update smr_trn_tsalestype  set " +
                       " salestype_name = '" + values.salestype_name + "'," +
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
        public void DaGetDeleteSalestype(string salestype_gid, GetSalesType_List values)
        {
            try
            {
                msSQL = "  delete from smr_trn_tsalestype where salestype_gid='" + salestype_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Sales Type Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Sales Type";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Sales Type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}
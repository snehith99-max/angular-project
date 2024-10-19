using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using System.Globalization;

namespace ems.hrm.DataAccess
{
    public class DaHrmMstAssetList
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetGid2, msGetPrivilege_gid, msGetModule2employee_gid, lsassetref_no, lsasset_name;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        int mnResult6;

        public void DaGetAssetListSummary(MdlHrmMstAssetList values)
        {
            try
            {
                msSQL = " select a.asset_gid,a.assetref_no,a.asset_name,a.asset_count, ifnull(a.assigned_count,0) as assigned_count, ifnull(a.available_count,asset_count) as available_count, " +
                        " CONCAT(b.user_firstname,' ',b.user_lastname) as created_by, date_format(a.created_date,'%d-%b-%y') as created_date," +
                        " CONCAT(b.user_firstname,' ',b.user_lastname) as updated_by, a.updated_date," +
                        " CASE WHEN a.active_flag = 'Y' THEN 'Active'  " +
                        " WHEN a.active_flag = 'N' THEN 'Inactive' " +
                        " END as active_flag " +
                        " from hrm_mst_temployeeassetlist a " +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by order by asset_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<asset_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new asset_list
                        {
                            asset_gid = dt["asset_gid"].ToString(),
                            assetref_no = dt["assetref_no"].ToString(),
                            asset_count = dt["asset_count"].ToString(),
                            assigned_count = dt["assigned_count"].ToString(),
                            available_count = dt["available_count"].ToString(),
                            asset_name = dt["asset_name"].ToString(),
                            active_flag = dt["active_flag"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            updated_by = dt["updated_by"].ToString(),
                            updated_date = dt["updated_date"].ToString(),

                        });
                        values.asset_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostAssetList(string user_gid, asset_list values)
        {
            try
            {

                msSQL = " select assetref_no from hrm_mst_temployeeassetlist where assetref_no= '" + values.assetref_no + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                msSQL = " select asset_name from hrm_mst_temployeeassetlist where asset_name= '" + values.asset_name + "' ";
                objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);



                if (objOdbcDataReader.HasRows==true)
                {
                    values.status = false;
                    values.message = "Asset Ref No already Exist";
                    return;

                }
                else if (objOdbcDataReader1.HasRows==true)
                {
                    values.status = false;
                    values.message = "Asset Name already Exist"; 
                    return;
                }




               

                    msGetGid2 = objcmnfunctions.GetMasterGID("EMAL");


                msSQL = " insert into hrm_mst_temployeeassetlist(" +
                            " asset_gid," +
                            " assetref_no," +
                            " asset_count," +
                            " active_flag," +
                            " asset_name," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid2 + "'," +
                            " '" + values.assetref_no + "'," +
                            " '" + values.asset_count + "'," +
                            " 'Y'," +
                            "'" + values.asset_name + "',";
                                
                 msSQL += "'" + user_gid + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";




                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Asset List Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Asset List";
                    }
               
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }



        }

        public void DaUpdatedAssetList(string user_gid, asset_list values)
        {

            try
            {
                string lsactive;

                msSQL = " select assetref_no from hrm_mst_temployeeassetlist where assetref_no= '" + values.assetref_noedit + "' and   asset_gid !='" + values.asset_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " select asset_name from hrm_mst_temployeeassetlist where asset_name= '" + values.asset_nameedit + "' and   asset_gid !='" + values.asset_gid + "' ";
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Asset Ref No already Exist";
                    return;

                }
                else if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Asset Name already Exist";
                    return;
                }
                else
                {
                    if (values.active_flag == "Active")
                    {
                        lsactive = "Y";
                    }
                    else
                    {
                        lsactive = "N";

                    }

                    msSQL = " update  hrm_mst_temployeeassetlist  set " +
                            " assetref_no = '" + values.assetref_noedit + "'," +
                            " active_flag = '" + lsactive + "'," +
                            " asset_name = '" + values.asset_nameedit + "'," +
                            " asset_count = '" + values.asset_countedit + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where asset_gid='" + values.asset_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Asset List Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Asset List";
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }


       //public void DaDeleteAssetList(string asset_gid, asset_list values)
       //{
       //   try
       //   { 
       //     msSQL = "  delete from hrm_mst_temployeeassetlist  where asset_gid='" + asset_gid + "'  ";
       //     mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
       //     if (mnResult != 0)
       //     {
       //         values.status = true;
       //         values.message = "Asset List Deleted Successfully";
       //     }
       //     else
       //     {
       //         {
       //             values.status = false;
       //             values.message = "Error While Deleting Asset List";
       //         }
       //     }
       //   }
       //     catch (Exception ex)
       //     {
       //         values.status = false;

       //         objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
       //          "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
       //     }

       // }
    }
}
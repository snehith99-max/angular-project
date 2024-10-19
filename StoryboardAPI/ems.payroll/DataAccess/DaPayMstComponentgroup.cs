using ems.payroll.Models;
using ems.utilities.Functions;

using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using System.Data.Odbc;
using System.Web.UI.WebControls;




namespace ems.payroll.DataAccess
{
    public class DaPayMstComponentgroup
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, lscomponentgroup_code;

        // Module Master Summary
        public void DaComponentgroupSummary(MdlPayMstComponentgroup values)
        {
            try
            {
                msSQL = " Select componentgroup_gid,statutory,componentgroup_code,componentgroup_name ,group_belongsto,display_name from pay_mst_tcomponentgroupmaster" +
                     " WHERE 1=1 Order by componentgroup_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Componentgroup_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Componentgroup_list
                        {
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_code = dt["componentgroup_code"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            group_belongsto = dt["group_belongsto"].ToString(),
                            display_name = dt["display_name"].ToString(),
                            statutory = dt["statutory"].ToString(),


                        });
                        values.Componentgroup_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component group summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");             
            }
            
        }



        public void DaPostcomponentgroup(string user_gid, Componentgroup_list values)
        {
            try
            {

                msSQL = " SELECT componentgroup_name  FROM " +
                       " pay_mst_tcomponentgroupmaster WHERE componentgroup_name = '" + values.componentgroup_name + "'";

                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);


                msSQL = " SELECT display_name  FROM " +
                       " pay_mst_tcomponentgroupmaster WHERE display_name = '" + values.display_name + "'";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);


              if (dt_datatable1.Rows.Count > 0)
              {
                    values.status = false;
                    values.message = "Component Group name already Exist";
                    return;
              }
                else if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Display Name already Exist";
                    return;
                }           
                           
                   
                    if (values.Code_Generation == "N")
                    {
                        {
                            msSQL = " SELECT componentgroup_code  FROM " +
                                    " pay_mst_tcomponentgroupmaster WHERE componentgroup_code = '" + values.componentgroup_code_manual + "'";

                            dt_datatable = objdbconn.GetDataTable(msSQL);
                        }
                        if (dt_datatable.Rows.Count > 0)
                        {
                            values.status = false;
                            values.message = "Component Group code already Exist";
                            return;

                        }
                    lscomponentgroup_code = values.componentgroup_code_manual;
                    }
                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("CMGP");

                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CMGP' order by finyear desc limit 0,1 ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    lscomponentgroup_code = "CG" + "000" + lsCode;
                    }
                    if (msGetGid == "E")
                    {
                        values.status = false;
                        values.message = "Component Group Gid Not genarated";
                        return;

                    }
                    else
                    {
                        msSQL = " INSERT INTO pay_mst_tcomponentgroupmaster(" +
                            " componentgroup_gid, " +
                            " componentgroup_code, " +
                            " componentgroup_name ," +
                            " display_name," +
                            " group_belongsto ," +
                            " statutory ," +
                            " created_date," +
                            " created_by )" +
                            " VALUES (" +
                                " '" + msGetGid + "'," +
                                " '" + lscomponentgroup_code + "'," +
                                " '" + values.componentgroup_name + "'," +
                                " '" + values.display_name + "'," +
                                " '" + values.group_belongsto + "'," +
                                " '" + values.statutory + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                " '" + user_gid + "')";


                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Component Group Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding component group";
                        }
                    }           

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component group!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
         
        }




        public void DaUpdatecomponentgroup(string user_gid, Componentgroup_list values)
        {
            try
            {
               
                msSQL = " SELECT componentgroup_name  FROM " +
                 " pay_mst_tcomponentgroupmaster WHERE componentgroup_name = '" + values.componentgroup_name + "' and   componentgroup_gid !='" + values.componentgroup_gid + "' ";

                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);


                msSQL = " SELECT display_name  FROM " +
                       " pay_mst_tcomponentgroupmaster WHERE display_name = '" + values.display_name + "' and   componentgroup_gid !='" + values.componentgroup_gid + "' ";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);


                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Component Group name already Exist";
                    return;
                }
                else if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Display Name already Exist";
                    return;
                }
                else
                {


                    msSQL = " update pay_mst_tcomponentgroupmaster set" +


                                " componentgroup_code ='" + values.componentgroup_code + "'," +
                                " componentgroup_name ='" + values.componentgroup_name + "'," +
                                " display_name ='" + values.display_name + "'," +
                                " statutory ='" + values.statutory + "'," +
                                " group_belongsto='" + values.group_belongsto +"'," +
                                " updated_by ='" + user_gid + "'," +
                                " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                " where componentgroup_gid ='" + values.componentgroup_gid + "'";




                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Component Group Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updated component group";
                    }
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Component group!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }

        public void DagetDeleteComponent(string componentgroup_gid, MdlPayMstComponentgroup values)
        {
            try
            {

                msSQL = "  delete from pay_mst_tcomponentgroupmaster where componentgroup_gid='" + componentgroup_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Component Group Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Component";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting component!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }




        }
    }
}
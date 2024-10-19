using ems.system.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Collections.Specialized.BitVector32;


namespace ems.system.DataAccess
{
    public class DaEntity
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1,lsentity_name;

        // Module Master Summary
        public void DaGetEntitySummary(MdlEntity values)
        {
            try
            {
                msSQL = " select  entity_gid,entity_code,entity_prefix, entity_name, entity_description,  a.status as EntityStatus," +
                     " case when a.status = 'Y' then 'Active' when a.status IS NULL OR a.status = '' OR a.status = 'N' then 'InActive' END AS status," +
                        " CONCAT(b.user_firstname,' ',b.user_lastname) as created_by, date_format(a.created_date,'%d-%b-%Y') as created_date " +
                        " from adm_mst_tentity a " +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by order by entity_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<entity_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new entity_lists
                        {
                            entity_gid = dt["entity_gid"].ToString(),
                            entity_code = dt["entity_code"].ToString(),
                            entity_prefix = dt["entity_prefix"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            entity_description = dt["entity_description"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            EntityStatus = dt["status"].ToString(),
                        });
                        values.entity_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostEntity(string user_gid, entity_lists values)
        {
            try
            {

                msSQL = "SELECT entity_code FROM adm_mst_tentity " +
                        "WHERE LOWER(entity_code) = LOWER('" + values.entity_code + "') " +
                        "OR UPPER(entity_code) = UPPER('" + values.entity_code + "')";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable2.Rows.Count > 0)

                {

                    values.status = false;

                    values.message = "Entity Code already exist";

                    return;

                }

                msSQL = "SELECT entity_prefix FROM adm_mst_tentity WHERE entity_prefix='" + values.entity_prefix.Replace("'", "''") + "' " +
                        "OR LOWER(entity_prefix) = LOWER('" + values.entity_prefix.Replace("'", "''") + "') " +
                         "OR UPPER(entity_prefix) = UPPER('" + values.entity_prefix.Replace("'", "''") + "')";
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)

                {

                    values.status = false;

                    values.message = "Entity prefix already exist";

                    return;

                }
                string entity_name = values.entity_name.Replace("'", "\\'");
                msSQL = "SELECT entity_name FROM adm_mst_tentity WHERE entity_name = '" + entity_name + "' " +
                        "OR LOWER(entity_name) = LOWER('" + entity_name + "') " +
                        "OR UPPER(entity_name) = UPPER('" + entity_name + "')";
                DataTable dt_datatable3 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable3.Rows.Count > 0)
                    {

                        values.status = false;

                        values.message = "Entity name already exist";

                        return;

                    }
                        msGetGid = objcmnfunctions.GetMasterGID("CENT");
                        //msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CENT' order by finyear desc limit 0,1 ";
                        //string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        string entity_description;
                        if (values.entity_description == "" || values.entity_description == null)
                        {
                            entity_description = null;
                        }
                        else
                        {
                            entity_description = values.entity_description.Replace("'", "\\'");

                        }

                        //string lsentity_code = "ENT" + "000" + lsCode;

                        msSQL = " insert into adm_mst_tentity(" +
                                " entity_gid," +
                                " entity_code," +
                                  " entity_prefix," +
                                " entity_name," +
                                " entity_description," +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" +values. entity_code + "'," +
                                  " '" + values.entity_prefix.Replace("'", "\\'") + "'," +
                                "'" + entity_name + "'," +
                                "'" + entity_description + "',";
                       
                        msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Entity Added Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Entity !!";
                        }            
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetupdateentitydetails(string user_gid, entity_lists values)
        {
            try
            {
                //msSQL = " select entity_name from adm_mst_tentity where entity_name = '" + values.entityedit_name + "' ";
                //DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable1.Rows.Count > 0)
                //{
                //    values.status = false;
                //    values.message = "Same Entity name already exist";
                //    return;
                //}

                //    msSQL = " select entity_prefix from adm_mst_tentity where entity_prefix = '" + values.entity_prefixedit + "' ";
                //    DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                //    if (dt_datatable2.Rows.Count > 0)
                //    {
                //        values.status = false;
                //        values.message = "Entity prefix already exists";
                //        return;
                //    }
                string entityedit_name = values.entityedit_name.Replace("'", "\\'");


                msSQL = " SELECT entity_name  FROM " +
                      " adm_mst_tentity WHERE entity_name = '" + entityedit_name + "' and   entity_gid !='" + values.entity_gid + "' ";

                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Entity Name already exist";
                    return;
                }


                msSQL = " SELECT entity_prefix  FROM " +
                       " adm_mst_tentity WHERE entity_prefix = '" + values.entity_prefixedit.Replace("'", "\\'") + "' and   entity_gid !='" + values.entity_gid + "' ";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Entity Prefix already exist";
                    return;
                }
                string entityedit_description = values.entityedit_description.Replace("'", "\\'");


                msSQL = " update  adm_mst_tentity set " +
                            " entity_name = '" +entityedit_name + "'," +
                            " entity_prefix = '" + values.entity_prefixedit.Replace("'", "\\'") + "'," +
                            " entity_description = '" + entityedit_description + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where entity_gid='" + values.entity_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                

                //if (values.entity_prefixedit != null)
                //{
                //    msSQL = " update  adm_mst_tentity set " +
                //            " entity_prefix = '" + values.entity_prefixedit + "'," +
                //            " updated_by = '" + user_gid + "'," +
                //            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where entity_gid='" + values.entity_gid + "'  ";
                //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //}

                //if (values.entityedit_description != null)
                //{
                //    msSQL = " update  adm_mst_tentity set " +
                //            " entity_description = '" + values.entityedit_description + "'," +
                //            " updated_by = '" + user_gid + "'," +
                //            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where entity_gid='" + values.entity_gid + "'  ";
                //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //}

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Entity Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Entity !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaEntityActive(string entity_gid, MdlEntity values)
        {
            try
            {
                msSQL = " update adm_mst_tentity set" +
                       " status='Y'" +
                       " where entity_gid = '" + entity_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Entity Active Successfully";

                }
                else
                {

                    values.status = false;
                    values.message = "Error while Entity activeted";

                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaEntityInactive(string entity_gid, MdlEntity values)
        {
            try
            {
                msSQL = " update adm_mst_tentity set" +
                        " status='N'" +
                        " where entity_gid = '" + entity_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Entity Inactive successfully";

                }

                else
                {

                    values.status = false;
                    values.message = "Error while Entity Inactivated";

                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }






        public void DaGetdeleteentitydetails(string entity_gid, entity_lists values)
        {
            try
            {
                msSQL = "  delete from adm_mst_tentity where entity_gid='" + entity_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Entity Deleted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Entity Added Successfully !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
    }
}
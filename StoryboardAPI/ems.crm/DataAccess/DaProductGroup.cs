﻿using ems.system.Models;
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
using System.Runtime.InteropServices;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Collections.Specialized.BitVector32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;






namespace ems.crm.DataAccess
{
    public class DaProductGroup
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsindustry_name, lsproductgroup_code, lsproductgroup_name, lsproductgroup_nameedit, lsproductgroup_gid;
        public void DaProductgroupSummary(MdlProductGroup values)
        {
            try
            {
                 
                msSQL = " select  productgroup_gid, productgroup_name,productgroup_code, CONCAT(b.user_firstname,' ',b.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date " +
                    " from pmr_mst_tproductgroup a " +
                    " left join adm_mst_tuser b on b.user_gid=a.created_by where delete_flag <> 'Y' order by a.productgroup_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productgroup_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productgroup_list
                        {
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.productgroup_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Group Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            } 
        }
        public void DaPostProductgroup(string user_gid, productgroup_list values)

        {

            try
            {
                 
                msSQL = " select productgroup_code from pmr_mst_tproductgroup where productgroup_code = '" + values.productgroup_code + "'";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader .HasRows == true)
                {
                    values.status = false;
                    values.message = "product Group Code Already Exist !!";
                }


                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("PPGM");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPGM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lsproductgroup_code = "PGM" + "000" + lsCode;

                    msSQL = " select productgroup_name from pmr_mst_tproductgroup where productgroup_name = '" + values.productgroup_name.Replace("'", "\\\'") + "'";
                    objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader .HasRows == true)
                    {
                        values.status = false;
                        values.message = "Product Group Name Already Exist !!";
                    }
                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PPGM");

                        msSQL = " insert into pmr_mst_tproductgroup (" +
                                    " productgroup_gid," +
                                    " productgroup_code," +
                                    " productgroup_name," +
                                    " created_by, " +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msGetGid + "'," +
                                    " '" + lsproductgroup_code + "'," +
                                    "'" + values.productgroup_name.Trim().Replace("'", "\\\'") + "'," +
                                    "'" + user_gid + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Group Added Successfully";
                        }
                        else
                        {

                            values.status = false;
                            values.message = "Error While Adding Productgroup";
                        }
                    }

                }
                objOdbcDataReader .Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Product Group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaUpdatedProductgroup(string user_gid, productgroup_list values)

        {

            try
            {
                 
                msSQL = " select productgroup_gid,productgroup_name from pmr_mst_tproductgroup where productgroup_name = '" + values.productgroup_nameedit.Trim().Replace("'", "\\\'") + "'";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader .HasRows)
                {
                    lsproductgroup_gid = objOdbcDataReader ["productgroup_gid"].ToString();
                    lsproductgroup_name = objOdbcDataReader ["productgroup_name"].ToString();
                }

                if (lsproductgroup_gid == values.productgroup_gid)
                {

                    msSQL = " update  pmr_mst_tproductgroup  set " +
                         " productgroup_code = '" + values.productgroup_codeedit + "'," +
                  " productgroup_name = '" + values.productgroup_nameedit.Trim().Replace("'", "\\\'") + "'," +
                  " updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where productgroup_gid='" + values.productgroup_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Product Group Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Productgroup !!";
                    }
                }
                else
                {
                    if (string.Equals(lsproductgroup_name, values.productgroup_nameedit.Trim().Replace("'", "\\\'"), StringComparison.OrdinalIgnoreCase))
                    {
                        values.status = false;
                        values.message = "Product Group with the same name already exists !!";
                    }
                    else
                    {
                        msSQL = " update  pmr_mst_tproductgroup  set " +
                       " productgroup_code = '" + values.productgroup_codeedit + "'," +
                " productgroup_name = '" + values.productgroup_nameedit.Trim().Replace("'", "\\\'") + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where productgroup_gid='" + values.productgroup_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Product Group Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Productgroup !!";
                        }
                    }
                }
                objOdbcDataReader .Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product Group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }         
        }



        public void DadeleteProductgroupSummary(string productgroup_gid, productgroup_list values)
        {

            try
            {
                 
                msSQL = "select product_gid from pmr_mst_tproduct where productgroup_gid='" + productgroup_gid + "';";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader .HasRows)
                {
                    values.status = false;
                    values.message = "Product Group already used hence can't be deleted!!";
                }
                else
                {
                    msSQL = "  delete from pmr_mst_tproductgroup where productgroup_gid='" + productgroup_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Group Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Product Group";
                    }

                }
                objOdbcDataReader .Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Product Group Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}




        

    

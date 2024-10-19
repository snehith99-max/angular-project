using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaImsMstLocation
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable, dt_datatable1;
        string company_logo_path, authorized_sign_path;
        OdbcDataReader objOdbcDataReader, objODBCDataReader;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        int mnResult, mnResult2, mnResult1, mnResult3;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();
        string lsissuenow_qty, lsqty_issued, lsqty_requested, msGetGid;

        public void DaImslocationsummary(MdlImsMstLocation values)
        {
            try
            { 
                msSQL = " select a.branch_gid,a.location_gid,a.location_name,a.location_code,  " +
                        " a.created_date,b.branch_name,concat(c.user_firstname, ' ', c.user_lastname) as created_by " +
                        " from ims_mst_tlocation a " +
                        " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
                        " left join adm_mst_tuser c on a.created_by = c.user_gid " +
                        " where 1 = 1";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<locationsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new locationsummary_list
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            location_gid = dt["location_gid"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            location_code = dt["location_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.locationsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Location summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaImslocationbranch(MdlImsMstLocation values)
        {
            try
            {
                msSQL = " Select branch_name,branch_gid from hrm_mst_tbranch where 1 = 1";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<locationbranch_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new locationbranch_list
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.locationbranch_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading branch data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostLocation(string user_gid, locationadd_list values)
        {
            try
            {
                msSQL= "select branch_name from hrm_mst_tbranch where branch_gid='"+values.branch_name+"'";
                string lsbranch=objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select location_name from ims_mst_tlocation where location_name = '" + values.location_name.Replace("'","\\\'") + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Location Name Already Exist";
                    return;
                }   
                msSQL = "Select location_gid from ims_mst_tlocation where location_code = '" + values.location_code + "'";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Location Code Already Exist";
                    return;
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("LOCN");
                    msSQL = " insert into ims_mst_tlocation (" +
                            " location_gid," +
                            " branch_gid, " +
                            " location_code, " +
                            " location_name, " +
                            " created_by, " +
                            " created_date ) " +
                            "values (" +
                            " '" + msGetGid + "'," +
                            " '" + values.branch_name + "'," +
                            " '" + values.location_code + "'," +
                            " '" + values.location_name.Replace("'","\\\'") + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("ALTM");
                        msSQL = " insert into adm_mst_tlocation " +
                                " (location_gid," +
                                " branch_gid," +
                                " branch_name," +
                                " location_code," +
                                " location_name " +
                                ") values ( " +
                                "'" + msGetGid + "'," +
                                "'" + values.branch_name + "'," +
                                "'" + lsbranch + "'," +
                                "'" + values.location_code + "'," +
                                "'" + values.location_name.Replace("'", "\\\'") + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if(mnResult1 != 0)
                        {
                            values.status = true;
                            values.message = "Location Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Location";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Location";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Location!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaEditlocation(string user_gid, locationadd_list values)
        {
            try
            {
                msSQL = "select branch_name from hrm_mst_tbranch where branch_gid='" + values.branch_name + "'";
                string lsbranch = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " update ims_mst_tlocation set  " +
                        " branch_gid = '" + values.branch_name + "'," +
                        " location_name = '" + values.location_name.Replace("'", "\\\'") + "'," +
                        " updated_by = '" + user_gid + "', " +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'where location_gid='"+values.location_gid+"'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " update adm_mst_tlocation set  " +
                            " branch_gid = '" + values.branch_name + "'," +
                            " branch_name = '" + lsbranch + "'," +
                            " location_name = '" + values.location_name.Replace("'", "\\\'") + "'," +
                            " updated_by = '" + user_gid + "', " +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where location_gid='"+values.location_gid+"'";
                    mnResult1= objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult1 != 0)
                    {
                        values.status = true;
                        values.message = "Location Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Location";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Location";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Location!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }


        public void DaDeletelocationsummary(string location_gid, locationadd_list values)
        {
            try
            {
                msSQL = "select product_gid,location_gid from ims_mst_tproductassign where location_gid='" + location_gid + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "you can not delete this location because products assigned in this location!!";
                    objOdbcDataReader.Close();
                }
                else
                {
                    msSQL = " delete from ims_mst_tlocation where location_gid='" + location_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Location Deleted Successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Location !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }
    }
}
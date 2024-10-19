using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;

namespace ems.crm.DataAccess
{
    public class DaProductUnits
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid;
        int mnResult;
        public void DaGetMktProductUnitSummary(MdlProductUnits values)
        {
            try
            {

                msSQL = " select  productuomclass_gid,productuomclass_code,productuomclass_name, CONCAT(b.user_firstname,' ',b.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date " +
                    " from pmr_mst_tproductuomclass  a " +
                    " left join adm_mst_tuser b on b.user_gid=a.created_by order by productuomclass_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Mktproductunits_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Mktproductunits_list
                        {
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.Mktproductunits_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaPostMktProductUnit(string user_gid, Mktproductunits_list values)
        {
            try
            {

                msSQL = " select productuomclass_name from pmr_mst_tproductuomclass where  productuomclass_name='" + values.productuomclass_name.Trim() + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Product Unit Class Already Exist";
                    return;
                }


                else
                {


                    msGetGid = objcmnfunctions.GetMasterGID("PUCM");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PUCM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string productuomclass_code = "PUC" + "000" + lsCode;

                    msSQL = " insert into pmr_mst_tproductuomclass (" +
                        " productuomclass_gid," +
                        " productuomclass_code," +
                        " productuomclass_name ," +
                        " created_by, " +
                        " created_date)" +
                        " values(" +
                        " '" + msGetGid + "'," +
                        "'" + productuomclass_code + "',";
                    if (values.productuomclass_name.Trim() == null || values.productuomclass_name.Trim() == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.productuomclass_name.Trim().Replace("'", "\\'") + "',";
                    }
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Unit Class Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product Unit";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Marketing Product Unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        // summary grid
        public void DaGetMktProductUnitSummarygrid(string productuomclass_gid, MdlProductUnits values)
        {
            try
            {

                msSQL = "select a.productuom_gid, a.productuom_code, a.productuom_name,a.sequence_level,format(a.convertion_rate, 2) as convertion_rate,a. baseuom_flag, " +
                    " b.productuomclass_gid, b.productuomclass_name from pmr_mst_tproductuom a " +
                    " left join pmr_mst_tproductuomclass b on a.productuomclass_gid = b.productuomclass_gid  left join pmr_mst_tproduct c on a.productuom_gid=c.productuom_gid" +
                    " where a.productuomclass_gid='" + productuomclass_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Mktproductunitgrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Mktproductunitgrid_list
                        {


                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            sequence_level = dt["sequence_level"].ToString(),
                            convertion_rate = dt["convertion_rate"].ToString(),
                            baseuom_flag = dt["baseuom_flag"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),

                        });
                        values.Mktproductunitgrid_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit Summary Grid !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        // Update
        public void DaUpdatedMktProductunit(string user_gid, Mktproductunits_list values)
        {
            try
            {

                msSQL = " select * from pmr_mst_tproductuomclass where  productuomclass_name = '" + values.productuomclassedit_name.Trim() + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Product Unit Name Already Exist";
                    return;
                }

                else
                {

                    msSQL = " update  pmr_mst_tproductuomclass  set " +
              " productuomclass_code = '" + values.productuomclassedit_code + "'," +
              " productuomclass_name = '" + values.productuomclassedit_name.Trim() + "'," +
              " updated_by = '" + user_gid + "'," +
              " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where productuomclass_gid='" + values.productuomclass_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Product Unit Updated Successfully";


                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product Unit";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Sales Product Unit  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DadeleteMktProductunitSummary(string productuomclass_gid, Mktproductunits_list values)
        {
            try
            {

                msSQL = "  delete from pmr_mst_tproductuomclass where productuomclass_gid='" + productuomclass_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Unit Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Product Unit";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Sales Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }
        public void DaGetMktProductunits(string productuomclass_gid, MdlProductUnits values)
        {
            try
            {

                msSQL = "Select productuomclass_gid, productuomclass_code, productuomclass_name" +
                       " from pmr_mst_tproductuomclass where productuomclass_gid='" + productuomclass_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Mktproductunitgrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Mktproductunitgrid_list
                        {


                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),


                        });
                        values.Mktproductunitgrid_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }




        }
        public void PostMktProductunits(string user_gid, Mktproductunitgrid_list values)
        {
            try
            {


                msSQL = " select * from pmr_mst_tproductuom where productuom_code= '" + values.productuomclassadd_code + "'or productuom_name = '" + values.productuomclassadd_name + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Product Unit  Already Exist";
                    return;
                }
                else
                {


                    msGetGid = objcmnfunctions.GetMasterGID("PPMM");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPMM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string productuom_code = "PU" + "000" + lsCode;

                    msSQL = " insert into pmr_mst_tproductuom " +
                        " (productuom_gid, " +
                        " productuom_code," +
                        " productuom_name, " +
                        " sequence_level, " +
                        " convertion_rate, " +
                        " baseuom_flag, " +
                        " productuomclass_gid " +
                        " ) values ( " +
                        " '" + msGetGid + "'," +
                        " '" + productuom_code + "'," +
                        " '" + values.productuomclassadd_name + "'," +
                        " '" + values.sequence_level + "'," +
                        " '" + values.conversion_rate + "'," +
                         " '" + values.batch_flag + "'," +
                        " '" + values.productuomclass_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Unit Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product Unit";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaEditMktProductunits(string user_gid, Mktproductunitgrid_listedit values)
        {
            try
            {


                //msSQL = "select product_gid from pmr_mst_tproduct where productuom_gid='" + values.productuom_gid + "';";
                //objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                //if (objOdbcDataReader.HasRows)
                //{
                //    values.status = false;
                //    values.message = "Product Unit already used hence can't be deleted!!";
                //}
                //else
                //{

                    //msSQL = " insert into pmr_mst_tproductuom " +
                    //    " (productuom_gid, " +
                    //    " productuom_code," +
                    //    " productuom_name, " +
                    //    " sequence_level, " +
                    //    " convertion_rate, " +
                    //    " baseuom_flag, " +
                    //    " productuomclass_gid " +
                    //    " ) values ( " +
                    //    " '" + msGetGid + "'," +
                    //    " '" + productuom_code + "'," +
                    //    " '" + values.productuomclassadd_name + "'," +
                    //    " '" + values.sequence_level + "'," +
                    //    " '" + values.conversion_rate + "'," +
                    //     " '" + values.batch_flag + "'," +
                    //    " '" + values.productuomclass_gid + "')";
                    msSQL = "Update pmr_mst_tproductuom set productuom_name='" + values.productuomedit_name + "', " +
                        "sequence_level='" + values.sequence_leveledit + "' , " +
                        "convertion_rate=(REPLACE('" + values.conversion_rateedit+ "', ',', '')), baseuom_flag='" + values.batch_flagedit + "',"+
                          " updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where productuom_gid='" + values.productuom_gid + "'  ";
                   
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Unit Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product Unit";
                    }
                //}
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaDeletproductuniteMktsummary(string productuom_gid, Mktproductunitgrid_listedit values)
        {
            try
            {

              
                msSQL = "select * from pmr_mst_tproduct where productuom_gid='" + productuom_gid + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Product Unit already used hence can't be deleted!!";
                }
                else
                {
                    msSQL = "  delete from pmr_mst_tproductuom where productuom_gid='" + productuom_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Unit Deleted Successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Sales Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }

    }
}
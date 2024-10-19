using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace ems.sales.DataAccess
{
    public class DaSmrMstProductUnit
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid;
        int mnResult;

        //summary
        public void DaGetSalesProductUnitSummary(MdlSmrMstProductUnit values)
        {
            try
            {

                msSQL = " select  productuom_gid,b.productuomclass_gid,a.productuom_code,productuom_name,b.productuomclass_name,b.productuomclass_code, CONCAT(c.user_firstname, ' ', c.user_lastname) as created_by,date_format(a.created_date, '%d-%m-%Y') as created_date " +
                        " from pmr_mst_tproductuom a " +
                        " left join pmr_mst_tproductuomclass b on b.productuomclass_gid = a.productuomclass_gid " +
                        " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                        " order by productuom_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<salesproductunit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new salesproductunit_list
                    {
                        productuom_gid = dt["productuom_gid"].ToString(),
                        productuomclass_gid = dt["productuomclass_gid"].ToString(),
                        productuomclass_code = dt["productuom_code"].ToString(),
                        productuomclass_name = dt["productuomclass_name"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        created_by = dt["created_by"].ToString(),
                        created_date = dt["created_date"].ToString(),
                    });
                    values.salesproductunit_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }

        //add
        public void DaPostSalesProductUnit(string user_gid, salesproductunit_list values)
        {
            try
            {

                msGetGid = objcmnfunctions.GetMasterGID("PUCM");
                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PUCM' order by finyear desc limit 0,1 ";
                string lsCode = objdbconn.GetExecuteScalar(msSQL);

                string productuomclass_code = "PUC" + "000" + lsCode;

                msSQL = " select * from pmr_mst_tproductuomclass where productuomclass_code= '" + values.productuomclass_code + "'or productuomclass_name='" + values.productuomclass_name.Replace("'","\\\'") + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Product Unit Class Already Exist";
                    return;
                }


                else { 
                msSQL = " insert into pmr_mst_tproductuomclass (" +
                    " productuomclass_gid," +
                    " productuomclass_code," +
                    " productuomclass_name ," +
                    " created_by, " +
                    " created_date)" +
                    " values(" +
                    " '" + msGetGid + "'," +
                    "'" + productuomclass_code + "',";
                if (values.productuomclass_name == null || values.productuomclass_name == "")
                {
                    msSQL += "'',";
                }
                else
                {
                    msSQL += "'" + values.productuomclass_name.Replace("'", "\\\'") + "',";
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
                    values.message = "Error While Adding Product Unit Class";
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Sales Product Unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }

        // summary grid
        public void DaGetSalesProductUnitSummarygrid(string productuomclass_gid, MdlSmrMstProductUnit values)
        {
            try
            {
                
                msSQL = "select a.productuom_gid, a.productuom_code, a.productuom_name,a.sequence_level,format(a.convertion_rate, 2) as convertion_rate, " +
                    "  a.baseuom_flag,b.productuomclass_gid, b.productuomclass_name from pmr_mst_tproductuom a " +
                    " left join pmr_mst_tproductuomclass b on a.productuomclass_gid = b.productuomclass_gid  left join pmr_mst_tproduct c on a.productuom_gid=c.productuom_gid" +
                    " where a.productuomclass_gid='" + productuomclass_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salesproductunitgrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salesproductunitgrid_list
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
                        values.salesproductunitgrid_list = getModuleList;
                    }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit Summary Grid !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Update
        public void DaUpdatedSalesProductunit(string user_gid, salesproductunit_list values)
        {
            try
            {
                msSQL = "select productuomclass_name from pmr_mst_tproductuomclass where productuomclass_name = '" + values.productuomclassedit_name.Replace("'", "\\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
              
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Product Unit Class already exist";
                    objOdbcDataReader.Close();
                    return;
                }
                else
                {


                    msSQL = " update  pmr_mst_tproductuomclass  set " +
              " productuomclass_name = '" + (String.IsNullOrEmpty(values.productuomclassedit_name) ? values.productuomclassedit_name : values.productuomclassedit_name.Replace("'", "\\'"))  + "'," +
              " updated_by = '" + user_gid + "'," +
              " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where productuomclass_gid='" + values.productuomclass_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Product Unit Class Updated Successfully";


                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product Unit Class";
                    }
                }
               


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Sales Product Unit  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        public void DadeleteSalesProductunitSummary(string productuom_gid, salesproductunit_list values)
        {
            try
            {
                msSQL = "select * from pmr_mst_tproduct where productuom_gid = '" + productuom_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "This Product Unit is mapped to the Product!";

                    return;

                }
                else
                {

                    msSQL = "  delete from pmr_mst_tproductuom where productuom_gid='" + productuom_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Unit Class Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Product Unit Class";
                    }
                }
                objOdbcDataReader.Close();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Sales Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            


        }

        // ADD product unit class

        public void DaGetProductunits(string productuomclass_gid,MdlSmrMstProductUnit values)
        {
            try
            {
               
                msSQL = "Select productuomclass_gid, productuomclass_code, productuomclass_name" +
                       " from pmr_mst_tproductuomclass where productuomclass_gid='" + productuomclass_gid + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<salesproductunitgrid_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new salesproductunitgrid_list
                    {


                        productuomclass_gid = dt["productuomclass_gid"].ToString(),
                        productuomclass_code = dt["productuomclass_code"].ToString(),
                        productuomclass_name = dt["productuomclass_name"].ToString(),
                       

                    });
                    values.salesproductunitgrid_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            



        }




        public void PostProductunits( string user_gid, salesproductunitgrid_list values)
        {
            try {


                msSQL = " select * from pmr_mst_tproductuom where productuom_name = '" + values.productuom_name.Replace("'", "\\\'") + "' and productuomclass_gid='"+values.productuomclass_name + "' and productuom_name = '" + values.productuom_code + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Product Unit Name Already Exist";
                    return;
                }
                msSQL = " select * from pmr_mst_tproductuom where productuom_name = '" + values.productuom_code + "' and productuomclass_gid='"+values.productuomclass_name.Replace("'", "\\\'") + "' and productuom_name = '" + values.productuom_name.Replace("'", "\\\'") + "'  ";
      
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Product Unit Code Already Exist";
                    return;
                }
                else { 
              
                   objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    
                    msGetGid = objcmnfunctions.GetMasterGID("PPMM");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPMM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string productuom_code = "PU" + "000" + lsCode;

                    msSQL = " insert into pmr_mst_tproductuom " +
                            " (productuom_gid, " +
                            " productuom_code," +
                            " productuom_name, " +
                            " uomname, " +
                            " productuomclass_gid " +
                            " ) values ( " +
                            " '" + msGetGid + "'," +
                            " '" + values.productuomclass_code + "'," +
                            " '" + (String.IsNullOrEmpty(values.productuom_name) ? values.productuom_name : values.productuom_name.Replace("'", "\\'")) + "'," +
                            " '" + (String.IsNullOrEmpty(values.productuom_name) ? values.productuom_name : values.productuom_name.Replace("'", "\\'")) + "'," +
                            " '" + (String.IsNullOrEmpty(values.productuomclass_name) ? values.productuomclass_name : values.productuomclass_name.Replace("'", "\\'")) + "')";

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
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaEditsalesProductunits(string user_gid, salesproductunitgrid_listedit values)
        {
            try
            {
                

                msSQL = "select * from pmr_mst_tproductuom where productuom_name='"+ values.productuomedit_name.Replace("'", "\\\'") + "'and productuom_code = '"+values.productuomedit_code + "' and productuomclass_gid = '"+values.productuomclass_nameedit+"'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Product Unit already exist";
                    return;
                }
                else {
                    msSQL = " Update pmr_mst_tproductuom set productuom_name='" + (String.IsNullOrEmpty(values.productuomedit_name) ? values.productuomedit_name : values.productuomedit_name.Replace("'", "\\'")) + "', " +
                            " productuomclass_gid = '" + values.productuomclass_nameedit + "' , " +
                             " productuom_code = '" + values.productuomedit_code + "' , " +
                             " uomname = '" + (String.IsNullOrEmpty(values.productuomedit_name) ? values.productuomedit_name : values.productuomedit_name.Replace("'", "\\'")) + "' , " +
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
                }
                objOdbcDataReader.Close();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaDeletproductunitsalessummary(string productuom_gid, salesproductunitgrid_listedit values)
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

        public void DaGetproductunitclassdropdown(MdlSmrMstProductUnit values)
        {
            try
            {
                msSQL = "SELECT productuomclass_gid, productuomclass_name FROM pmr_mst_tproductuomclass Order by productuomclass_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<unitclass_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new unitclass_list
                        {
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),

                        });
                        values.unitclass_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting region !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}
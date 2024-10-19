using System;
using System.Collections.Generic;
using ems.pmr.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;



namespace ems.pmr.DataAccess
{
    public class DaPmrMstProductUnit
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string  msGetGid;
        int mnResult;
        public void DaGetProductUnitSummary(MdlPmrMstProductUnit values)
        {
            try
            {
                
                msSQL =
                msSQL = " select  productuom_gid,b.productuomclass_gid,a.productuom_code,productuom_name,b.productuomclass_name,b.productuomclass_code, CONCAT(c.user_firstname, ' ', c.user_lastname) as created_by,date_format(a.created_date, '%d-%m-%Y') as created_date " +
                        " from pmr_mst_tproductuom a " +
                        " left join pmr_mst_tproductuomclass b on b.productuomclass_gid = a.productuomclass_gid " +
                        " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                        " order by productuom_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productunit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productunit_list
                        {
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.productunit_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Product unit summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaGetProductUnitSummarygrid(string productuomclass_gid, MdlPmrMstProductUnit values)
        {
            try
            {
                
                msSQL = "select a.productuom_gid, a.productuom_code, a.productuom_name,a.sequence_level,format(a.convertion_rate, 2) as convertion_rate,case when a.baseuom_flag ='N' then 'NO' when baseuom_flag='Y' then 'YES' " +
                    " end as baseuom_flag, count(c.product_gid) as total_count,b.productuomclass_gid, b.productuomclass_name from pmr_mst_tproductuom a " +
                    " left join pmr_mst_tproductuomclass b on a.productuomclass_gid = b.productuomclass_gid  left join pmr_mst_tproduct c on a.productuom_gid=c.productuom_gid" +
                    " where b.productuomclass_gid='" + productuomclass_gid + "'  group by productuom_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productunitgrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productunitgrid_list
                        {


                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            sequence_level = dt["sequence_level"].ToString(),
                            convertion_rate = dt["convertion_rate"].ToString(),
                            baseuom_flag = dt["baseuom_flag"].ToString(),
                            total_count = dt["total_count"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),

                        });
                        values.productunitgrid_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Product unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }


        public void DaPostProductUnit(string user_gid, productunit_list values)
        {
            try
            { 
           msSQL = " select productuom_name from pmr_mst_tproductuom where productuom_name = '" + values.productuom_name + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count != 0)
            {
                values.status = false;
                values.message = "Product Unit Name Already Exist";
                return;
            }
            msSQL = " select productuom_code from pmr_mst_tproductuom where productuom_code = '" + values.productuomclass_code + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count != 0)
            {
                values.status = false;
                values.message = "Product Unit Code Already Exist";
                return;
            }
            else
            {

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                msGetGid = objcmnfunctions.GetMasterGID("PPMM");
                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPMM' order by finyear desc limit 0,1 ";
                string lsCode = objdbconn.GetExecuteScalar(msSQL);

                string productuom_code = "PU" + "000" + lsCode;

                msSQL = " insert into pmr_mst_tproductuom " +
                        " (productuom_gid, " +
                        " productuom_code," +
                        " productuom_name, " +
                        " productuomclass_gid " +
                        " ) values ( " +
                        " '" + msGetGid + "'," +
                        " '" + values.productuomclass_code + "'," +
                        " '" + values.productuom_name + "'," +
                        " '" + values.productuomclass_name + "')";

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
                values.message = "Exception occured while adding Product unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaUpdatedProductunit(string user_gid, productunit_list values)
        {
            try
            {
                //msSQL = "select productuomclass_name from pmr_mst_tproductuomclass where productuomclass_name = '" + values.productuomclassedit_name + "'";
                //objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                //if (objOdbcDataReader.HasRows)
                //{
                //    values.status = false;
                //    values.message = "Product Unit Class already exist";
                //    return;
                //}
                //else
                //{


                    msSQL = " update  pmr_mst_tproductuomclass  set " +
              " productuomclass_name = '" + values.productuomclassedit_name + "'," +
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
                //}

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }






        public void DadeleteProductunitSummary(string productuom_gid , productunit_list values)
        {
            try
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
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetProductunits(string productuomclass_gid, MdlPmrMstProductUnit values)
        {
            try
            {
               
                msSQL = "Select productuomclass_gid, productuomclass_code, productuomclass_name" +
                        " from pmr_mst_tproductuomclass where productuomclass_gid='" + productuomclass_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productunitgrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productunitgrid_list
                        {


                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),


                        });
                        values.productunitgrid_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Product unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             

        }
        public void PostProductunits(string user_gid, productunitgrid_list values)
        {
            try
            {


                //msSQL = " select * from pmr_mst_tproductuom where productuom_name = '" + values.productuom_name + "' and productuomclass_gid='" + values.productuomclass_name + "' and productuom_name = '" + values.productuom_code + "' ";
                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable.Rows.Count != 0)
                //{
                //    values.status = false;
                //    values.message = "Product Unit Name Already Exist";
                //    return;
                //}
                msSQL = " select * from pmr_mst_tproductuom where productuom_name = '" + values.productuom_code + "' and productuomclass_gid='" + values.productuomclass_name + "' and productuom_name = '" + values.productuom_name + "'  ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Product Unit Code Already Exist";
                    return;
                }
                else
                {

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    msGetGid = objcmnfunctions.GetMasterGID("PPMM");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPMM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string productuom_code = "PU" + "000" + lsCode;

                    msSQL = " insert into pmr_mst_tproductuom " +
                            " (productuom_gid, " +
                            " productuom_code," +
                            " productuom_name, " +
                            " productuomclass_gid " +
                            " ) values ( " +
                            " '" + msGetGid + "'," +
                            " '" + values.productuomclass_code + "'," +
                            " '" + values.productuom_name + "'," +
                            " '" + values.productuomclass_name + "')";

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
                values.message = "Exception occured while adding Product unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaDeletproductunitsalessummary(string productuom_gid, productunitgrid_listedit values)
        {
            try
            {


                msSQL = "select * from pmr_mst_tproduct where productuom_gid='" + productuom_gid + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    values.status = false;
                    values.message = "Product Unit already used hence can't be deleted!!";
                    objOdbcDataReader.Close();
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
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }

        public void DaEditsalesProductunits(string user_gid, productunitgrid_listedit values)
        {
            try
            {


                msSQL = "select * from pmr_mst_tproductuom where productuom_name='" + values.productuomedit_name + "'and productuom_code = '" + values.productuomedit_code + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Close();
                    values.status = false;
                    values.message = "Product Unit already exist";
                    return;
                }
                else
                {
                    msSQL = " Update pmr_mst_tproductuom set productuom_name='" + values.productuomedit_name + "', " +
                            " productuomclass_gid = '" + values.productuomclass_nameedit + "' , " +
                             " productuom_code = '" + values.productuomedit_code + "' , " +
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

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Unit !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetproductunitclassdropdown(MdlPmrMstProductUnit values)
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
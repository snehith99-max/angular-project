using System.Collections.Generic;
using ems.pmr.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System;


namespace ems.pmr.DataAccess
{
    public class DaPmrMstProductGroup
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetProductGroupSummary(MdlPmrMstProductGroup values)
        {
            try
            {
                
                msSQL = " SELECT  productgroup_gid, productgroup_name,productgroup_code, " +
                  " CONCAT(b.user_firstname,' ',b.user_lastname) " +
                  " as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date " +
                  " from pmr_mst_tproductgroup a " +
                  " left join adm_mst_tuser b on b.user_gid=a.created_by order by a.productgroup_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productgroup_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productgroup_list
                        {
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
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
                values.message = "Exception occured while getting Product group summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }




        public void DaPostProductGroup(string user_gid, productgroup_list values)
        {
            try
            {
               
                msSQL = " select * from pmr_mst_tproductgroup where productgroup_code= '" + values.productgroup_code + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {
                    //values.status = true;
                    values.message = "Product Group code  Already Exist";


                }

                else
                {
                    msSQL = " select * from pmr_mst_tproductgroup where productgroup_name= '" + values.productgroup_name.Replace("'", "\\\'") + "';";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    objOdbcDataReader.Read();
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Close();
                        values.message = "Product Group Name  Already Exist";
                        return;
                        
                    }


                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PPGM");
                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPGM' order by finyear desc limit 0,1 ";
                        string lsCode1 = objdbconn.GetExecuteScalar(msSQL);

                        string lsproductgroup_code = "PGC" + "00" + lsCode1;
                        //msGetGid = objcmnfunctions.GetMasterGID("PPGM");

                        msSQL = " insert into pmr_mst_tproductgroup (" +
                                " productgroup_gid," +
                                " productgroup_code," +
                                " productgroup_name," +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                "'" + msGetGid + "'," +
                                "'" + lsproductgroup_code + "',";
                        if (values.productgroup_name == null || values.productgroup_name == "")
                        {
                            msSQL += "'',";
                        }
                        else
                        {
                            msSQL += "'" + values.productgroup_name.Replace("'", "\\\'") + "',";
                        }

                        msSQL += "'" + user_gid + "'," +
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
                            values.message = "Error While Adding Product Group";
                        }

                    }
                }
                objOdbcDataReader.Close();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured whileAdding Product Group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        public void DaGetUpdatedProductgroup(string user_gid, productgroup_list values)
        {
            try
            {

                // msSQL = " update  pmr_mst_tproductgroup  set " +           
                //         " productgroup_name = '" + values.productgroupedit_name + "'," +
                //         " updated_by = '" + user_gid + "'," +
                //         " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where productgroup_gid='" + values.productgroup_gid + "'  ";


                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //if (mnResult != 0)
                //{

                //    values.status = true;
                //    values.message = "Product Group Updated Successfully";

                //}
                //else
                //{
                //    values.status = false;
                //    values.message = "Error While Updating Product Group";
                //}


                msSQL = "select * from pmr_mst_tproductgroup where productgroup_name = '" + values.productgroupedit_name.Replace("'", "\\\'") + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Close();
                    values.status = true;
                     values.message = "Product Group Updated Successfully";
                     return;
                }
                
                else
                {
                    msSQL = " update  pmr_mst_tproductgroup  set ";
                    if (!string.IsNullOrEmpty(values.productgroupedit_name) && values.productgroupedit_name.Contains("'"))
                    {
                        msSQL += "productgroup_name = '" + values.productgroupedit_name.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "productgroup_name = '" + values.productgroupedit_name + "', ";
                    }
                    msSQL += " updated_by = '" + user_gid + "'," +
                         " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where productgroup_gid='" + values.productgroup_gid + "'  ";


                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Product Group Updated Successfully";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product Group";
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product Group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
         

        }



        public void DaGetDeleteProductSummary(string productgroup_gid, productgroup_list values)

        {
            try
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Product Group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
    }
}
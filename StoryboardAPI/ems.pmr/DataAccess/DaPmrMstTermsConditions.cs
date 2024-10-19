using System;
using System.Collections.Generic;
using ems.pmr.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;



namespace ems.pmr.DataAccess
{
    public class DaPmrMstTermsConditions
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, lsuser_code, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetTermsConditionsSummary(MdlPmrMstTermsConditions values)
        {
            try
            {
                
                msSQL = " select a.termsconditions_gid,a.template_name,b.user_firstname, " +
                " date_format(a.created_date,'%d-%M-%Y') as created_date,a.payment_terms " +
                " from pmr_trn_ttermsconditions a " +
                " left join adm_mst_tuser b on b.user_gid = a.created_by " +
                " order by termsconditions_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettemplate_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Gettemplate_list
                        {
                            termsconditions_gid = dt["termsconditions_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                        });
                        values.Gettemplate_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Terms and Conditions !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
            msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaPostTermsConditions(string user_gid, template_list values)
        {
            try
            {
                msSQL = " select * from pmr_trn_ttermsconditions where template_name= '" + values.template_name.Replace("'", "\\\'")+ "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    values.message = "Template Name Already Exist";
                    objOdbcDataReader.Close();
                        
                }
                else
                //{
                //    msSQL = " select payment_terms from pmr_trn_ttermsconditions where payment_terms= '" + values.payment_terms + "'";
                //    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                //    objOdbcDataReader.Read();
                //    if (objOdbcDataReader.HasRows)
                //    {
                //        values.message = "Payment Terms Already Exist";
                //    }
                //    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PTCP");
                    msSQL = " insert into pmr_trn_ttermsconditions(" +
                         " termsconditions_gid," +
                         " template_name," +
                         " payment_terms," +
                         " template_content," +
                         " created_by," +
                         " created_date)" +
                         " values(" +
                       " '" + msGetGid + "'," +
                       "'" + values.template_name.Replace("'", "\\\'") + "'," +
                       "'" + values.payment_terms + "',";
                    //"'" + values.template_content.Replace("'", "") + "'," +

                    if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                    {
                        msSQL += "'" + values.template_content.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.template_content + "', ";
                    }


                    msSQL +="'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Template Added Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Template !!";
                        }
                    } }
            //}
            catch (Exception ex)
            {
                values.message = "Exception occured while adding  Terms and Conditions!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
            msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetTemplateEditdata(MdlPmrMstTermsConditions values, string termsconditions_gid)
        {
            try
            {
              
                msSQL = " select termsconditions_gid, template_name,payment_terms,template_content from pmr_trn_ttermsconditions " +
            " where termsconditions_gid = '" + termsconditions_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<templateedit_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new templateedit_list
                        {
                            termsconditions_gid = dt["termsconditions_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString(),
                            payment_terms = dt["payment_terms"].ToString()
                        });
                        values.templateedit_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Terms and Conditions!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
            msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaUpdatedTemplate(string user_gid, templateupdate_list values)
        {
            try
            {
                msSQL = " select * from pmr_trn_ttermsconditions where template_name= '" + values.template_name.Replace("'", "") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    values.message = "Template Name Already Exist";
                    objOdbcDataReader.Close();
                }
                else
                {

                    msSQL = " select template_name from pmr_trn_ttermsconditions where template_name = '" + values.template_name.Replace("'", "\\\'") + "' ";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    msSQL = " update pmr_trn_ttermsconditions SET " +
                            " template_name = '" + values.template_name.Replace("'", "\\\'") + "'," +
                            " payment_terms = '" + values.payment_terms + "',";
                    //" template_content = '" + values.template_content.Replace("'", "").Trim() + "'," +
                    if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                    {
                        msSQL += "template_content = '" + values.template_content.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "template_content ='" + values.template_content.Replace("'", "\\\'") + "', ";
                    }
                    msSQL +=" updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where termsconditions_gid = '" + values.termsconditions_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Template Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Template";
                    }
                }

                msSQL = " select * from pmr_trn_ttermsconditions where template_name= '" + values.template_name.Replace("'", "") + "' and termsconditions_gid = '" + values.termsconditions_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)

                {

                    msSQL = " select template_name from pmr_trn_ttermsconditions where template_name = '" + values.template_name.Replace("'", "'") + "' ";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    msSQL = " update pmr_trn_ttermsconditions SET " +
                            " template_name = '" + values.template_name.Replace("'", "") + "'," +
                            " payment_terms = '" + values.payment_terms.Replace("'", "") + "'," +
                            " template_content = '" + values.template_content.Replace("'", "").Trim() + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where termsconditions_gid = '" + values.termsconditions_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Template Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Template";
                    }

                }
                
                else
                {


                    values.message = "Template Name Already Exist";

                  
                }
                objOdbcDataReader.Close();  
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Terms and Conditions!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
            msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }


        public void DaDeleteTemplate(string termsconditions_gid, templatedelete_list values)
        {
            try
            {
                 
                msSQL = " delete from pmr_trn_ttermsconditions where termsconditions_gid='" + termsconditions_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Template Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Template";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Terms and Conditions!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
            msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
    }
}

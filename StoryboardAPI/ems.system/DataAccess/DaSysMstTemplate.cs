using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Web;
using ems.system.Models;
using ems.utilities.Functions;



namespace ems.system.DataAccess{
    public class DaSysMstTemplate
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1;
        int mnResult;
        public void DaTemplateSummary(MdlSysMstTemplate values)
        {
            try
            {
                msSQL = " select a.template_gid, a.template_name, c.templatetype_name, a.status as template_status, " +
                        " case when a.status = 'Y' then 'Active' when a.status IS NULL OR a.status = '' OR a.status = 'N' then 'InActive' END AS status," +
                        " concat_ws(' ', b.user_firstname, b.user_lastname) as created_by, date_format(a.created_on, '%d-%b-%Y') as created_date from adm_mst_ttemplate a " +
                        " left join adm_mst_tuser b on a.created_by = b.user_gid " +
                        " left join adm_mst_ttemplatetype c on a.templatetype_gid = c.templatetype_gid order by a.created_on desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<MdlSysMstTemplateSummarylist>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new MdlSysMstTemplateSummarylist
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            templatetype_name = dt["templatetype_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            TemplateStatus = dt["status"].ToString(),
                        });
                        values.templatesummarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTemplateType(MdlSysMstTemplate values)
        {
            try
            {
                msSQL = " select templatetype_gid, templatetype_name from adm_mst_ttemplatetype ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetTemplateTypedropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTemplateTypedropdown
                        {
                            templatetype_gid = dt["templatetype_gid"].ToString(),
                            templatetype_name = dt["templatetype_name"].ToString(),
                        });
                        values.GetTemplateTypedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
               

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostTemplate(string user_gid, MdlSysMstTemplatelist values)
        {
            try
            {
                string template_name = values.template_name.Replace("'", "\\'");
                string template_content = values.template_content.Replace("'","\\'");



                msSQL = "select template_gid from adm_mst_ttemplate where template_name='" + template_name + "'";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == false)
                {
                    string msGetGID = objcmnfunctions.GetMasterGID("TMPL");

                    msSQL = " insert into adm_mst_ttemplate ( " +
                            " template_gid, " +
                            " template_name, " +
                            " templatetype_gid, " +
                            " template_content, " +
                            " created_by, " +
                            " created_on) " +
                            " values(" +
                            " '" + msGetGID + "'," +
                            " '" + template_name + "'," +
                            " '" + values.templatetype_gid + "'," +
                            " '" + template_content + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Template Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Template";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Template Name Already Exist";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTemplateEditdata(MdlSysMstTemplate values, string template_gid)
        {
            try
            {

                msSQL = " select template_gid, template_name, templatetype_gid, template_content from adm_mst_ttemplate " +
                        " where template_gid = '" + template_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<MdlSysMstTemplateEditlist>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new MdlSysMstTemplateEditlist
                        {
                            template_gid_edit = dt["template_gid"].ToString(),
                            template_name_edit = dt["template_name"].ToString(),
                            templatetype_gid_edit = dt["templatetype_gid"].ToString(),
                            template_content_edit = dt["template_content"].ToString()
                        });
                        values.templateeditlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
               

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdatedTemplate(string user_gid, MdlSysMstTemplateEditlist values)
        {
            try
            {
                string template_name_edit = values.template_name_edit.Replace("'", "\\'");
                string template_content_edit = values.template_content_edit.Replace("'", "\\'");


                msSQL = " SELECT template_name  FROM " +
                     " adm_mst_ttemplate WHERE template_name = '" + template_name_edit + "' and   template_gid !='" + values.template_gid_edit + "' ";

                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Template name already exist";
                    return;
                }


                else
                {
                    msSQL = " update adm_mst_ttemplate SET " +
                            " template_name = '" + template_name_edit + "'," +
                            " templatetype_gid = '" + values.templatetype_gid_edit + "'," +
                            " template_content = '" + template_content_edit + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where template_gid = '" + values.template_gid_edit + "'";

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
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaTemplateActive(string template_gid, MdlSysMstTemplate values)
        {
            try
            {
                msSQL = " update adm_mst_ttemplate set" +
                       " status='Y'" +
                       " where template_gid = '" + template_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Template Activated Successfully";

                }
                else
                {

                    values.status = false;
                    values.message = "Error while Template activeted";

                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaTemplateInactive(string template_gid, MdlSysMstTemplate values)
        {
            try
            {
                msSQL = " update adm_mst_ttemplate set" +
                        " status='N'" +
                        " where template_gid = '" + template_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Template Inactivated Successfully ";

                }

                else
                {

                    values.status = false;
                    values.message = "Error while Template Inactivated";

                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaDeleteTemplate(string template_gid, MdlSysMstTemplateEditlist values)
        {
            try
            {
                msSQL = " delete from adm_mst_ttemplate where template_gid='" + template_gid + "' ";

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
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetassignmodule(MdlSysMstTemplate values)
        {
            try
            {
                msSQL = "select module_gid , module_name from adm_mst_tmoduleangular where module_gid_parent = '$'";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<assignmodule>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assignmodule
                        {
                            module_gid = dt["module_gid"].ToString(),
                            module_name = dt["module_name"].ToString()
                            
                        });
                        values.assignmodule = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {


                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaAssignModuletotemplate(string employee_gid, assignmodule2list values)
        {
            try
            {
                //msSQL = "DELETE FROM adm_trn_ttemplate2module where template_gid = '" + values.template_gid +"'";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                foreach (var data in values.assignmodule)
                {
                    msSQL = " select template2module_gid from adm_trn_ttemplate2module where template_gid='" + values.template_gid + "' and module_gid='"+data.module_gid+"' ";
                    objOdbcDataReader=objdbconn.GetDataReader(msSQL);
                    if(objOdbcDataReader.HasRows)
                    {
                    }
                    else { 
                    msSQL = "INSERT INTO adm_trn_ttemplate2module (template2module_gid, template_gid, module_gid, created_by, created_on) " +
                     "VALUES ('" + objcmnfunctions.GetMasterGID("TMPLDTL") + "', '" + values.template_gid + "', '" + data.module_gid + "', '" + employee_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Module Assigned to the Template successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Module was already assigned";
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGettemplatedetails(string template_gid, MdlSysMstTemplate values)
        {
            try
            {
                msSQL = " select a.module_gid , b.module_name from adm_trn_ttemplate2module a " +
                " left join adm_mst_tmoduleangular b on a.module_gid = b.module_gid " +
                " where template_gid = '" + template_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<assignmodule>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assignmodule
                        {
                            module_gid = dt["module_gid"].ToString(),
                            module_name = dt["module_name"].ToString(),
                           
                        });
                        values.assignmodule = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

    }
}
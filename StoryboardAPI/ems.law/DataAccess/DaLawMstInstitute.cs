using ems.utilities.Functions;
using ems.law.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
namespace ems.law.DataAccess
{
    public class DaLawMstInstitute : ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2;
        string msGetGid, msGetGid1, lsempoyeegid, msGETGID4, lsstatus;
        public void DaGetInstitutesummary(MdlLawMstInstitute values)
        {
            try
            {
                msSQL = " select a.institute_gid,a.institute_code,a.institute_name,concat(a.ins_address1 ,'',a.ins_address2) as institute_location,a.user_code,a.password,  "+
                        " CASE WHEN a.active_flag = 'N' THEN 'Active'  ELSE 'InActive' END as Institute_status,a.institutemail_id,a.mobile,a.contact_person,    " +
                        " concat(c.user_firstname, ' ', c.user_lastname) as created_by  ,     " +
                        " date_format(a.created_date, '%d-%b-%Y') as created_date  " +
                        " from law_mst_tinstitute a  " +
                        " left join adm_mst_tuser c on a.created_by = c.user_gid  " +
                        " order by a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<institute_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new institute_list
                        {
                            institute_gid = dt["institute_gid"].ToString(),
                            institute_code = dt["institute_code"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            password = dt["password"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            institute_location = dt["institute_location"].ToString(),
                            institute_name = dt["institute_name"].ToString(),
                            Institute_status = dt["Institute_status"].ToString(),
                            institutemail_id = dt["institutemail_id"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                        });
                        values.institute_List = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Institute Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetInstitutecountry(MdlLawMstInstitute values)
        {
            msSQL = " Select country_name,country_gid  " +
                    " from adm_mst_tcountry ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<countryList>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new countryList
                    {
                        country_name = dt["country_name"].ToString(),
                        country_gid = dt["country_gid"].ToString(),
                    });
                    values.countryList = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostInstituteAdd(string user_gid, institute_list values)
        {
            try
            {
                msSQL = "SELECT * FROM law_mst_tinstitute WHERE institute_name = '" + values.institute_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    while (objOdbcDataReader.Read())
                    {
                        if (objOdbcDataReader["institute_name"].ToString() == values.institute_name)
                        {
                            values.status = false;
                            values.message = "Institute Name Already Exists!";
                            break;
                        }
                    }
                }
                msSQL = "SELECT * FROM law_mst_tinstitute WHERE  institute_code = '" + values.institute_code + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    while (objOdbcDataReader.Read())
                    {
                        if (objOdbcDataReader["institute_code"].ToString() == values.institute_code)
                        {
                            values.status = false;
                            values.message = "Institute Code Already Exists!";
                            break;
                        }
                    }
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("SUSM");
                    msSQL = " insert into law_mst_tinstitute(" +
                          " institute_gid," +
                          " institute_code," +
                          " institute_name," +
                          " institute_location," +
                          " institutemail_id," +
                          " contact_person," +
                          " mobile," +
                          " password," +
                          " ins_address1," +
                          " ins_address2," +
                          " ins_pincode," +
                          " ins_city," +
                          " ins_state," +
                          " ins_country," +
                          " institute_prefix," +
                          " default_screen," +
                          " created_by, " +
                          " created_date)" +
                          " values(" +
                          " '" + msGetGid + "'," +
                          " '" + values.institute_code + "'," +
                          " '" + values.institute_name + "'," +
                          " '" + values.institute_location + "'," +
                          " '" + values.institutemail_id + "'," +
                          " '" + values.contact_person + "'," +
                          " '" + values.mobile + "'," +
                          " '" + objcmnfunctions.ConvertToAscii(values.password) + "',"+
                          " '" + values.ins_address1 + "',"+
                          " '" + values.ins_address2 + "',"+
                          " '" + values.ins_pincode + "',"+
                          " '" + values.ins_city + "',"+
                          " '" + values.ins_state + "',"+
                          " '" + values.ins_country + "',"+
                          " '" + values.institute_prefix + "',"+
                          " 'LGLINSTCASE'," +
                          "'" + user_gid + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        

                        msSQL = "  select case when  module_gid_parent like '%$%' then 'LGL' else module_gid_parent end as module_gid_parent ," +
                            " module_gid from adm_mst_tmodule where module_code in ('LGLINSTCASE', 'LGLCASE','LGL') ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if(dt_datatable.Rows.Count > 0)
                        {
                            foreach(DataRow row in dt_datatable.Rows)
                            {
                                msGETGID4 = objcmnfunctions.GetMasterGID("SPGM");
                                msSQL = " insert into adm_mst_tprivilege(" +
                            "privilege_gid, " +
                            "module_gid, " +
                            "module_parent_gid, " +
                            "user_gid " +
                            " ) values ( " +
                            "'" + msGETGID4 + "'," +
                            "'" + row["module_gid"].ToString() +"'," +
                            "'" + row["module_gid_parent"].ToString() + "'," +
                            "'" + msGetGid + "')"
                            ;
                           mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }                      
                        values.status = true;
                        values.message = "Institute Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Institute";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Institute Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaPostUpdateinstitute(string user_gid, institute_list values)
        {
            try
            {   
                    msSQL = " update law_mst_tinstitute set" +
                            " institute_name = '" + values.institute_name + "'," +
                            " mobile = '" + values.mobile + "'," +
                            " contact_person = '" + values.contact_person + "'," +
                            " ins_address1 = '" + values.ins_address1 + "'," +
                            " ins_address2 = '" + values.ins_address2 + "'," +
                            " ins_pincode = '" + values.ins_pincode + "'," +
                            " ins_city = '" + values.ins_city + "'," +
                            " ins_state = '" + values.ins_state + "'," +
                            " ins_country = '" + values.ins_country + "'," +
                            " institutemail_id = '" + values.institutemail_id + "'" +
                            " where institute_gid = '" + values.institute_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Institute Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While updating Institute";
                    }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Institute !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetinstituteInactive(string user_gid, Institutioninactivelog_data values)
        {
            try
            {
                if (values.Status == "Active")
                {
                    lsstatus = "N";
                }
                else if (values.Status == "InActive")
                {
                    lsstatus= "Y";   
                }
                msSQL = " update law_mst_tinstitute set" +
                        " active_flag='"+ lsstatus + "'" +
                        " where institute_gid = '" + values.institute_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("ACL");
                    msSQL = " insert into law_mst_tactivelog(" +
                          " log_gid," +
                          " institute_gid," +
                          " institute_name," +
                          " Status," +
                          " remarks," +
                          " updated_by, " +
                          " updated_date)" +
                          " values(" +
                          " '" + msGetGid + "'," +
                          " '" + values.institute_gid + "'," +
                          " '" + values.institute_name + "'," +
                          " '" + values.Status + "'," +
                          " '" + values.remarks + "'," +
                          "'" + user_gid + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    if (mnResult != 0) {
                    values.status = true;
                    values.message = "Institute " + values.Status + "  Successfully";
                    return;
                    }
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Institute " + values.Status + "";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Institute" + values.Status + " !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaGetinstituteActive(string institute_gid, MdlLawMstInstitute values)
        {
            try
            {
                msSQL = " update law_mst_tinstitute set" +
                        " active_flag='N'" +
                        " where institute_gid = '" + institute_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Institute Activated Successfully";
                    return;
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Activating Institute";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Institute Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaPostinstituteresetpassword(string user_gid, institute_list values)
        {
            try
            {
                msSQL = " update law_mst_tinstitute set" +
                        " password = '" + objcmnfunctions.ConvertToAscii(values.password)+ "'" +
                        " where institute_gid = '" + values.institute_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Password Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Password Institute";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Password Institute !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetInstituteEditSummary(string institute_gid, MdlLawMstInstitute values)
        {
            msSQL = " select a.institute_gid,a.institute_code,a.institute_name,a.contact_person,a.mobile,a.institutemail_id,a.ins_address1,a.ins_address2,a.institute_prefix, " +
                    " a.ins_pincode,a.ins_city,a.ins_state,a.ins_country ,b.country_name " +
                    " from law_mst_tinstitute a " +
                    " left join adm_mst_tcountry b on b.country_gid = a.ins_country " +
                    " where a.institute_gid = '" + institute_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<institute_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new institute_list
                    {
                        institute_gid = dt["institute_gid"].ToString(),
                        institute_code = dt["institute_code"].ToString(),
                        institute_name = dt["institute_name"].ToString(),
                        contact_person = dt["contact_person"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        institutemail_id = dt["institutemail_id"].ToString(),
                        ins_address1 = dt["ins_address1"].ToString(),
                        ins_address2 = dt["ins_address2"].ToString(),
                        ins_pincode = dt["ins_pincode"].ToString(),
                        ins_city = dt["ins_city"].ToString(),
                        ins_state = dt["ins_state"].ToString(),
                        ins_country = dt["ins_country"].ToString(),
                        country_name = dt["country_name"].ToString(),
                        institute_prefix = dt["institute_prefix"].ToString(),
                    });
                    values.institute_List = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaInstitutionInactiveHistory(string institute_gid, MdlLawMstInstitute values)
        {
            msSQL = " select a.log_gid,a.institute_gid,a.Status,a.remarks,date_format(a.updated_date, '%d-%b-%Y') as updated_date  ,concat(c.user_firstname, ' ', c.user_lastname) as updated_by " +
                    " from law_mst_tactivelog a   "+
                    " left join adm_mst_tuser c on a.updated_by = c.user_gid  where a.institute_gid = '" + institute_gid + "'"+
                    " order by a.updated_date desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Institutioninactivelog_data>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Institutioninactivelog_data
                    {
                        institute_gid = dt["institute_gid"].ToString(),
                        log_gid = dt["log_gid"].ToString(),
                        Status = dt["Status"].ToString(),
                        updated_date = dt["updated_date"].ToString(),
                        updated_by = dt["updated_by"].ToString(),
                        remarks = dt["remarks"].ToString(),
                    });
                    values.Institutioninactivelog_data = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
    }
}
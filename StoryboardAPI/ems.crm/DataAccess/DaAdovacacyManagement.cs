using ems.system.Models;
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
    public class DaAdovacacyManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsentity_name, lsregion_code, lsregion_name, lsregion_gid, lscity_name_edit, lscity;


        public void DaGetLeaddropdownforadvocacy(MdlAdovacacyManagement values)
        {
            msSQL = "select a.leadbank_gid,a.leadbank_name,b.leadbankbranch_name,b.leadbankcontact_name,b.address1,b.address2,b.city," +
                "b.state,b.pincode,b.mobile,b.email,c.region_name,d.source_name " +
                " from crm_trn_tleadbank a left join  crm_trn_tleadbankcontact  b on a.leadbank_gid=b.leadbank_gid left join crm_mst_tregion c" +
                " on a.leadbank_region=c.region_gid left join crm_mst_tsource d on a.source_gid=d.source_gid where a.leadbank_gid " +
                "not in (select reference_leadbankgid from crm_trn_tadvocacy) and b.main_contact ='Y';";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetLeaddropdownadvocacy_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetLeaddropdownadvocacy_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                        leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                        address1 = dt["address1"].ToString(),
                        address2 = dt["address2"].ToString(),
                        city = dt["city"].ToString(),
                        state = dt["state"].ToString(),
                        pincode = dt["pincode"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        email = dt["email"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        source_name = dt["source_name"].ToString(),


                    });
                    values.GetLeaddropdownadvocacy_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetCustomerdropdown(MdlAdovacacyManagement values)
        {
            try
            {

                msSQL = " select b.leadbank_gid,b.leadbank_name from crm_trn_tappointment a left join crm_trn_tleadbank b " +
                    "on a.leadbank_gid=b.leadbank_gid where Leadstage_gid='6'  and b.leadbank_gid" +
                    " not in (select advocacy_leadbankgid from crm_trn_tadvocacy) group by a.leadbank_gid;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<GetCustomerdropdown_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new GetCustomerdropdown_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),

                        });
                        values.GetCustomerdropdown_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Region Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }


        public void DaPostAdovacacy(string employee_gid, postadovacacy_list values)
        {

            try
            {
                for (int i = 0; i < values.leadtoapi.ToArray().Length; i++)
                {
                    msSQL = "select  reference_leadbankgid from crm_trn_tadvocacy where reference_leadbankgid='" + values.leadtoapi[i].leadbank_gid + "' and advocacy_leadbankgid='" + values.customer_gid + "'";
                    string reference_leadbankgid = objdbconn.GetExecuteScalar(msSQL);
                    if (reference_leadbankgid == null || reference_leadbankgid == "")
                    {
                    
                        msGetGid = objcmnfunctions.GetMasterGID("ADML");
                        msSQL1 = " Insert into crm_trn_tadvocacy ( " +
                                " advocacy_gid, " +
                                " advocacy_leadbankgid, " +
                                " reference_leadbankgid, " +
                                " created_by, " +
                                " created_date)" +
                                " Values ( " +
                                "'" + msGetGid + "'," +
                                "'" + values.customer_gid + "'," +
                                "'" + values.leadtoapi[i].leadbank_gid + "'," +
                                "'" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Advocacy Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occur While Adding Advocacy";
                        }
                    }

                }
             

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Advocacy!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetAdovacacysummary(MdlAdovacacyManagement values)
        {
            try
            {

                msSQL = "SELECT c.advocacy_leadbankgid, a.leadbank_gid, a.leadbank_name,concat(b.leadbankcontact_name,'/', b.mobile,'/', b.email) as lead_details," +
                    " r.region_name, d.source_name FROM crm_trn_tadvocacy c LEFT JOIN crm_trn_tleadbank a ON c.advocacy_leadbankgid = a.leadbank_gid " +
                    "LEFT JOIN crm_trn_tleadbankcontact b ON a.leadbank_gid = b.leadbank_gid LEFT JOIN crm_mst_tregion r ON a.leadbank_region = r.region_gid " +
                    "LEFT JOIN crm_mst_tsource d ON a.source_gid = d.source_gid group by c.advocacy_leadbankgid where b.main_contact ='Y';";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<GetAdovacacysummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new GetAdovacacysummary_list
                        {
                            adovacacy_leadbankgid = dt["advocacy_leadbankgid"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_details = dt["lead_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            source_name = dt["source_name"].ToString(),

                        });
                        values.GetAdovacacysummary_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Advocacy Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetAdvocacyDetails(MdlAdovacacyManagement values,string leadbank_gid)
        {
            try
            {

                msSQL = "SELECT c.advocacy_leadbankgid,c.reference_leadbankgid, a.leadbank_gid, a.leadbank_name," +
                    "concat(b.leadbankcontact_name,'/', b.mobile,'/', b.email) as lead_details, r.region_name," +
                    " d.source_name FROM crm_trn_tadvocacy c LEFT JOIN crm_trn_tleadbank a ON c.reference_leadbankgid = a.leadbank_gid" +
                    " LEFT JOIN crm_trn_tleadbankcontact b ON a.leadbank_gid = b.leadbank_gid LEFT JOIN crm_mst_tregion r " +
                    "ON a.leadbank_region = r.region_gid LEFT JOIN crm_mst_tsource d ON a.source_gid = d.source_gid  " +
                    "where c.advocacy_leadbankgid='"+leadbank_gid+ "' and b.main_contact ='Y';";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<GetAdvocacyDetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new GetAdvocacyDetails_list
                        {
                            adovacacy_leadbankgid = dt["advocacy_leadbankgid"].ToString(),
                            reference_leadbankgid = dt["reference_leadbankgid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_details = dt["lead_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            source_name = dt["source_name"].ToString(),

                        });
                        values.GetAdvocacyDetails_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Advocacy Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

         public void DaDeleteAdvocacy(MdlAdovacacyManagement values,string reference_leadbankgid,string adovacacy_leadbankgid)
        {
            try
            {

                msSQL = "delete from crm_trn_tadvocacy  where advocacy_leadbankgid='"+ adovacacy_leadbankgid + "' and reference_leadbankgid='" + reference_leadbankgid + "';";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;                
                }
                else
                {
                    values.status = false;
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Advocacy ";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }




    }
}
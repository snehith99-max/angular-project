using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.sales.Models;


namespace ems.sales.DataAccess
{
    public class DaSmrTrnSalesManager
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;

        public void DaGetSalesManagerTotal(string employee_gid ,MdlSmrTrnSalesManager values)
        {
            try
            {
               
                msSQL = " Select j.branch_name,b.customer_gid,k.campaign_title,b.customer_name,g.leadbankcontact_gid,c.leadbank_gid," +
                 " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
                 " as contact_details, concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
                 " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
                 " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
                 " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
                 " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid,z.leadstage_name" +
                 " From crm_trn_tenquiry2campaign a" +
                 " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                 " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
                 " left join smr_trn_tcampaign d on a.campaign_gid= d.campaign_gid " +
                 " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
                 " left join hrm_mst_temployee e on b.enquiry_receivedby = e.employee_gid " +
                 " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                 " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                 " left join hrm_mst_tbranch j on b.branch_gid=j.branch_gid " +
                 " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                 " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                 " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                 " left join crm_mst_tenquiry z on z.leadstage_gid=a.leadstage_gid " +
                 " where d.campaign_manager ='"+ employee_gid +"' and a.leadstage_gid in ('6', '3', '4', '5') order by a.enquiry_gid asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<totalall_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new totalall_list
                    {
                        customer_gid = dt["leadbank_gid"].ToString(),
                        customergid = dt["customer_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        customercontact_gid = dt["leadbankcontact_gid"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["customer_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.totalalllist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Sales Manager Total !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }


        public void DaGetSalesManagerComplete(string employee_gid,MdlSmrTrnSalesManager values)
        {
            try
            {
                
                msSQL = " Select j.branch_name,  k.campaign_title,b.customer_name, " +
                  " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
                  " as contact_details, concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
                  " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
                  " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
                  " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
                  " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid" +
                  " From crm_trn_tenquiry2campaign a" +
                  " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                  " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
                  " left join smr_trn_tcampaign d on a.campaign_gid= d.campaign_gid  " + 
                  " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                  " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                  " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
                  " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                  " left join hrm_mst_tbranch j on b.branch_gid=j.branch_gid " +
                  " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                  " left join hrm_mst_temployee x on a.created_by=x.employee_gid" +
                  " left join crm_mst_tenquiry z on z.leadstage_gid=a.leadstage_gid" +
                  " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                  " where d.campaign_manager = '" + employee_gid + "' and a.leadstage_gid ='4' order by a.enquiry_gid asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<complete_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new complete_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["customer_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        //leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.completelist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Manager Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        //Prospect summary

        public void DaGetSalesManagerProspect(string employee_gid,MdlSmrTrnSalesManager values)
        {
            try
            {
                
                msSQL = " Select j.branch_name,  k.campaign_title,b.customer_name, " +
                    " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
                    " as contact_details,concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
                    " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
                    " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
                    " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
                    " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid" +
                    " From crm_trn_tenquiry2campaign a" +
                    " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                    " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
                    " left join smr_trn_tcampaign d on a.campaign_gid= d.campaign_gid  " +
                    " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on b.branch_gid=j.branch_gid " +
                    " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " where d.campaign_manager = '" + employee_gid +"' and a.leadstage_gid ='6' order by a.enquiry_gid asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<prospects_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new prospects_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["customer_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        //leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.prospectslist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Manager Prospect !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        //Potentials summary
        public void DaGetSalesManagerPotential(string employee_gid, MdlSmrTrnSalesManager values)
        {
            try
            {
               
                msSQL = " Select j.branch_name,  k.campaign_title,b.customer_name, " +
                " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
                " as contact_details,concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
                " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
                " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
                " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
                " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid" +
                " From crm_trn_tenquiry2campaign a" +
                " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
                " left join smr_trn_tcampaign d on a.campaign_gid= d.campaign_gid " +
                " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
                " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                " left join hrm_mst_tbranch j on b.branch_gid=j.branch_gid " +
                " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                " where d.campaign_manager = '"+ employee_gid +"' and a.leadstage_gid ='4'  order by a.enquiry_gid asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<potentials_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new potentials_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["customer_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        //leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.potentialslist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Manager Potential !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetSalesManagerDrop(string employee_gid,MdlSmrTrnSalesManager values)
        {
            try
            {
               
                msSQL = " Select j.branch_name,  k.campaign_title,b.customer_name," +
               " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
               " as contact_details, concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
               " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
               " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
               " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
               " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid" +
               " From crm_trn_tenquiry2campaign a" +
               " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
               " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
               " left join smr_trn_tcampaign d on a.campaign_gid= d.campaign_gid  "+
               " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
               " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
               " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
               " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
               " left join hrm_mst_tbranch j on b.branch_gid=j.branch_gid " +
               " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
               " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
               " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
               " where d.campaign_manager = '"+employee_gid+"' and a.leadstage_gid ='3' order by a.enquiry_gid asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<drops_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new drops_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["customer_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        //leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.dropstatuslist = getModuleList;
                }

            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Manager Drop !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        //Count summary

        public void DaGetSmrTrnManagerCount(string employee_gid, string user_gid, MdlSmrTrnSalesManager values)
        {
            try
            {
               
                msSQL = " select(select count(a.lead2campaign_gid) from crm_trn_tenquiry2campaign a " +
                        "  left join smr_trn_tcampaign b on a.campaign_gid = b.campaign_gid " +
                        " where b.campaign_manager = '"+ employee_gid +"' and a.leadstage_gid in ('3','5','8','4')) as employeecount," +
                        " (select count(a.lead2campaign_gid) from crm_trn_tenquiry2campaign a " +
                        " left join smr_trn_tcampaign b on a.campaign_gid = b.campaign_gid " +
                        " where b.campaign_manager = '"+ employee_gid +"' and a.leadstage_gid = '3') as prospect, " +
                        " (select count(a.lead2campaign_gid) from crm_trn_tenquiry2campaign a " +
                        "left join smr_trn_tcampaign b on a.campaign_gid = b.campaign_gid" +
                        " where b.campaign_manager = '"+ employee_gid +"' and a.leadstage_gid = '4') as potential, " +
                        " (select count(a.lead2campaign_gid) from crm_trn_tenquiry2campaign a " +
                        " left join smr_trn_tcampaign b on a.campaign_gid = b.campaign_gid " +
                        " where b.campaign_manager = '"+ employee_gid +"' and a.leadstage_gid = '8') as completed, " +
                        " (select count(a.lead2campaign_gid) from crm_trn_tenquiry2campaign a  " +
                        "left join smr_trn_tcampaign b on a.campaign_gid = b.campaign_gid " +
                        "where b.campaign_manager = '"+ employee_gid +"' and  a.leadstage_gid = '5') as drop_status";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var customercount_list = new List<teammanagercount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    customercount_list.Add(new teammanagercount_list
                    {
                        employeecount = dt["employeecount"].ToString(),
                        prospect = dt["prospect"].ToString(),
                        potential = dt["potential"].ToString(),
                        completed = dt["completed"].ToString(),
                        drop_status = dt["drop_status"].ToString(),
                    });
                    values.teamcount_list = customercount_list;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Manager Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // charts for quotation,enquiry,order and customer

        public void Dacustomercount(string employee_gid,MdlSmrTrnSalesManager values)
        {
            try
            {
                msSQL = " SELECT DATE_FORMAT(a.created_date, '%b') AS Months, COUNT(a.customer_gid) AS count FROM crm_mst_tcustomer a " +
                    " left join crm_trn_tenquiry2campaign b on a.customer_gid = b.customer_gid " +
                    " left join smr_trn_tcampaign c on b.campaign_gid = c.campaign_gid " +
                    "WHERE c.campaign_manager = '" + employee_gid + "' and a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months ORDER BY a.created_date desc; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<chartscounts_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new chartscounts_list1
                        {
                            customermonth = dt["Months"].ToString(),
                            customermonthcount = dt["count"].ToString(),
                        });
                        values.chartscounts_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

        public void Daquotationchartcount(string employee_gid,MdlSmrTrnSalesManager values)
        {
            try
            {
                msSQL = " SELECT DATE_FORMAT(a.created_date,'%b') AS Months, COUNT(a.quotation_gid) AS count FROM smr_trn_treceivequotation a " +
                     " left join crm_trn_tenquiry2campaign b on a.customer_gid = b.customer_gid " +
                    " left join smr_trn_tcampaign c on b.campaign_gid = c.campaign_gid " +
                    " WHERE  c.campaign_manager = '" + employee_gid + "' and a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months ORDER BY a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<chartscounts_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new chartscounts_list1
                        {
                            quotationmonth = dt["Months"].ToString(),
                            quotationmonthcount = dt["count"].ToString(),
                        });
                        values.chartscounts_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

        public void Daenquirychartcount(string employee_gid,MdlSmrTrnSalesManager values)
        {
            try
            {
                msSQL = "SELECT DATE_FORMAT(a.created_date,'%b') AS Months, COUNT(a.enquiry_gid) AS count FROM smr_trn_tsalesenquiry a " +
                    " left join crm_trn_tenquiry2campaign b on a.customer_gid = b.customer_gid " +
                    " left join smr_trn_tcampaign c on b.campaign_gid = c.campaign_gid " +
                    " WHERE  c.campaign_manager = '" + employee_gid + "' and a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months ORDER BY a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<chartscounts_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new chartscounts_list1
                        {
                            enquirymonth = dt["Months"].ToString(),
                            enquirymonthcount = dt["count"].ToString(),
                        });
                        values.chartscounts_list1 = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void Dasaleschartcount(string employee_gid,MdlSmrTrnSalesManager values)
        {
            try
            {
                msSQL = "SELECT DATE_FORMAT(a.created_date,'%b') AS Months, COUNT(a.salesorder_gid) AS count FROM smr_trn_tsalesorder a " +
                     " left join crm_trn_tenquiry2campaign b on a.customer_gid = b.customer_gid " +
                    " left join smr_trn_tcampaign c on b.campaign_gid = c.campaign_gid " +
                    "WHERE c.campaign_manager = '" + employee_gid + "' and a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months ORDER BY a.created_date desc;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<chartscounts_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new chartscounts_list1
                        {
                            salesmonth = dt["Months"].ToString(),
                            salesmonthcount = dt["count"].ToString(),
                        });
                        values.chartscounts_list1 = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DateamactivitySummary(string employee_gid,MdlSmrTrnSalesManager values)
        {
            try
            {
                msSQL = "SELECT COALESCE(t1.Months, t2.enquiryMonths, t3.quote_Months, t4.order_Months) AS Months,COALESCE(t1.leads_count, 0) AS leads_count," +
                    "COALESCE(t2.enquiry_count, 0) AS enquiry_count,COALESCE(t3.quote_count, 0) AS quote_count,COALESCE(t4.order_count, 0) AS order_count," +
                    "COALESCE(t4.salesorder_amount, 0) AS salesorder_amount,COALESCE(t3.quoteorder_amount, 0) AS quoteorder_amount FROM " +
                    "(SELECT DATE_FORMAT(created_date, '%b') AS Months, COUNT(customer_gid) AS leads_count FROM crm_mst_tcustomer WHERE" +
                    " created_date >= DATE_SUB(CURDATE(), INTERVAL 5 MONTH) AND created_date <= CURDATE() GROUP BY Months) AS t1 LEFT JOIN" +
                    " (SELECT DATE_FORMAT(created_date, '%b') AS enquiryMonths, COUNT(enquiry_gid) AS enquiry_count  FROM smr_trn_tsalesenquiry" +
                    " WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 5 MONTH) AND created_date <= CURDATE() " +
                    " GROUP BY enquiryMonths) AS t2 ON t1.Months = t2.enquiryMonths LEFT JOIN (SELECT DATE_FORMAT(created_date, '%b') AS" +
                    " quote_Months, COUNT(quotation_gid) AS quote_count,FORMAT(SUM(total_amount), '2') as quoteorder_amount  FROM smr_trn_treceivequotation " +
                    "WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 5 MONTH)  AND created_date <= CURDATE() GROUP BY quote_Months) AS t3" +
                    " ON t1.Months = t3.quote_Months OR t2.enquiryMonths = t3.quote_Months LEFT JOIN (SELECT DATE_FORMAT(created_date, '%b') AS order_Months" +
                    ", COUNT(salesorder_gid) AS order_count, format(SUM(total_amount), '2') AS salesorder_amount  FROM smr_trn_tsalesorder " +
                    "WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 5 MONTH)  AND created_date <= CURDATE() GROUP BY order_Months) AS t4 " +
                    "ON t1.Months = t4.order_Months OR t2.enquiryMonths = t4.order_Months OR t3.quote_Months = t4.order_Months GROUP BY Months" +
                    " ORDER BY  FIELD(Months,DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 5 MONTH), '%b'),DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 4 MONTH), '%b'), " +
                    "DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 3 MONTH), '%b'),DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 2 MONTH), '%b'), " +
                    "DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 1 MONTH), '%b'),DATE_FORMAT(CURDATE(), '%b')) DESC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<chartscounts_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new chartscounts_list1
                        {
                            Months = dt["Months"].ToString(),
                            customer_count = dt["leads_count"].ToString(),
                            enquiry_count = dt["enquiry_count"].ToString(),
                            quote_count = dt["quote_count"].ToString(),
                            order_count = dt["order_count"].ToString(),
                            salesorder_amount = dt["salesorder_amount"].ToString(),
                            quoteorder_amount = dt["quoteorder_amount"].ToString(),
                        });
                        values.chartscounts_list1 = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }


        public void DaGetSalesTeamSummary(string employee_gid, MdlSmrTrnSalesManager values)
        {

            try
            {

                msSQL = "SELECT a.campaign_gid,a.campaign_title,c.branch_name," +
                    "(SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x WHERE x.campaign_gid = a.campaign_gid and x.leadstage_gid !='') as employeecount, " +
                    " (SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x WHERE x.campaign_gid = a.campaign_gid) as assigned_leads, " +
                    " (SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x WHERE x.so_status <> 'Y' and(x.leadstage_gid = '5') and x.campaign_gid = a.campaign_gid) as potential, " +
                    "(SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x WHERE x.so_status <> 'Y' and(x.leadstage_gid = '6') and x.campaign_gid = a.campaign_gid) as prospect," +
                    "(SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x WHERE(x.leadstage_gid = '3') and x.campaign_gid = a.campaign_gid " +
                    " AND x.customer_gid NOT IN ( SELECT customer_gid FROM crm_trn_tenquiry2campaign GROUP BY customer_gid HAVING COUNT(*) > 1 )) as drop_status, " +
                    "(SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x WHERE(x.leadstage_gid = '4') and x.campaign_gid = a.campaign_gid) as customer " +
                    "FROM smr_trn_tcampaign a LEFT JOIN hrm_mst_tbranch c ON a.campaign_location = c.branch_gid " +
                    " left join crm_trn_tenquiry2campaign y on a.campaign_gid = y.campaign_gid " +
                    " where a.campaign_manager = '"+ employee_gid +"'" +
                    "GROUP BY a.campaign_gid, a.campaign_title, c.branch_name ORDER BY a.campaign_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salesteam_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salesteam_list1
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            employeecount = dt["employeecount"].ToString(),
                            assigned_leads = dt["assigned_leads"].ToString(),
                            prospect = dt["prospect"].ToString(),
                            drop_status = dt["drop_status"].ToString(),
                            customer = dt["customer"].ToString(),
                            potential = dt["potential"].ToString()
                        });
                        values.Salesteam_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

        // overall Count for Chart

        public void DaGetsaleschart(string employee_gid, MdlSmrTrnSalesManager values)
        {
            try
            {
                msSQL = "SELECT Months, SUM(customer_count) AS customer_count, " +
                    "SUM(quotation_count) AS quotation_count," +
                    " SUM(enquiry_count) AS enquiry_count, " +
                    "SUM(order_count) AS order_count " +
                    "FROM (SELECT DATE_FORMAT(a.created_date, '%b') AS Months, " +
                    "COUNT(a.customer_gid) AS customer_count," +
                    " 0 AS quotation_count," +
                    " 0 AS enquiry_count," +
                    " 0 AS order_count FROM crm_mst_tcustomer a " +
                    "LEFT JOIN crm_trn_tenquiry2campaign b ON a.customer_gid = b.customer_gid " +
                    "LEFT JOIN smr_trn_tcampaign c ON b.campaign_gid = c.campaign_gid" +
                    " WHERE c.campaign_manager = '" + employee_gid + "' " +
                    "AND a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months" +
                    " UNION ALL SELECT DATE_FORMAT(a.created_date,'%b') AS Months, 0 AS customer_count, " +
                    "COUNT(a.quotation_gid) AS quotation_count, 0 AS enquiry_count," +
                    " 0 AS order_count FROM smr_trn_treceivequotation a" +
                    " LEFT JOIN crm_trn_tenquiry2campaign b ON a.customer_gid = b.customer_gid " +
                    "LEFT JOIN smr_trn_tcampaign c ON b.campaign_gid = c.campaign_gid" +
                    " WHERE c.campaign_manager = '" + employee_gid + "' " +
                    "AND a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months UNION ALL" +
                    " SELECT DATE_FORMAT(a.created_date,'%b') AS Months, 0 AS customer_count," +
                    " 0 AS quotation_count, COUNT(a.enquiry_gid) AS enquiry_count, 0 AS order_count " +
                    "FROM smr_trn_tsalesenquiry a LEFT JOIN crm_trn_tenquiry2campaign b ON a.customer_gid = b.customer_gid " +
                    "LEFT JOIN smr_trn_tcampaign c ON b.campaign_gid = c.campaign_gid " +
                    "WHERE c.campaign_manager = '" + employee_gid + "' " +
                    "AND a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months UNION ALL " +
                    "SELECT DATE_FORMAT(a.created_date,'%b') AS Months, 0 AS customer_count, 0 AS quotation_count," +
                    " 0 AS enquiry_count, COUNT(a.salesorder_gid) AS order_count FROM smr_trn_tsalesorder a " +
                    "LEFT JOIN crm_trn_tenquiry2campaign b ON a.customer_gid = b.customer_gid" +
                    " LEFT JOIN smr_trn_tcampaign c ON b.campaign_gid = c.campaign_gid " +
                    "WHERE c.campaign_manager = '" + employee_gid + "'" +
                    " AND a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months) AS combined_data GROUP BY Months ORDER BY Months DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<saleschart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new saleschart_list
                        {
                            quotation_count = dt["quotation_count"].ToString(),
                            months = dt["months"].ToString(),
                            enquiry_count = dt["enquiry_count"].ToString(),
                            order_count = dt["order_count"].ToString(),
                            customer_count = dt["customer_count"].ToString(),

                        });
                        values.saleschart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Monthly sales Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}
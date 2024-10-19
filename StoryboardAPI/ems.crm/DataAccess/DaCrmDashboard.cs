using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using ems.crm.Models;
using OfficeOpenXml.Style;



namespace ems.crm.DataAccess
{
    public class DaCrmDashboard
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        string msGetGid;
        int mnResult, mnResult1;

        // My Calls Count
        public void DaGetDashboardCount(string employee_gid, string user_gid, MdlCrmDashboard values)
        {
            try
            {
                 
                msSQL = " select (select count(lead2campaign_gid) from crm_trn_tlead2campaign " +
                    " where leadstage_gid IN (1, 2) and " +
                    " assign_to='" + employee_gid + "') as mycalls_count, " +
                    " (select count(lead2campaign_gid) from crm_trn_tlead2campaign " +
                    " where assign_to='" + employee_gid + "') as myleads_count," +
                    " (select count(schedulelog_gid) from crm_trn_tschedulelog " +
                    " where assign_to='" + employee_gid + "' and schedule_status='Pending' and schedule_date >= curdate()) as myappointments_count," +
                    " (select count(schedulelog_gid) from crm_trn_tschedulelog " +
                    " where assign_to='" + employee_gid + "' and schedule_status='Pending') as assignvisit_count," +
                    " (select count(schedulelog_gid) from crm_trn_tschedulelog " +
                    " where assign_to='" + employee_gid + "' and schedule_status='Closed') as completedvisit_count," +
                    " (select count(proposal_gid) from crm_mst_tproposaltemplate " +
                    " where created_by='" + user_gid + "') as shared_proposal," +
                    " (select count(salesorder_gid) from smr_trn_tsalesorder where" +
                    " salesorder_status<>'SO Amended' and created_by='" + employee_gid + "') as completedorder_count," +
                    " (select count(enquiry_gid) from smr_trn_tsalesenquiry a" +
                    " left join smr_trn_tsalesorder b on b.created_by = a. created_by" +
                    " where a.created_by='" + employee_gid + "' and b.salesorder_status<>'SO Amended') as totalorder_count, " +
                    " (select count(*) from smr_trn_tsalesorder where" +
                    "  created_by='" + employee_gid + "') as total_count," +
                    " (select count(salesorder_gid) from smr_trn_tsalesorder where" +
                    " salesorder_status='Approved' and created_by='" + employee_gid + "') as completedorder_count1," +
                    " (select count(salesorder_gid) from smr_trn_tsalesorder where" +
                    " salesorder_status='Cancelled' and created_by='" + employee_gid + "') as rejected_count," +
                    " (select count(salesorder_gid) from smr_trn_tsalesorder where" +
                    " salesorder_status='Cancelled' and created_by='" + employee_gid + "') as cancel_count," +
                    " (select count(leadbank_gid) from crm_trn_tleadbank where lead_status != 'Pending' and customertype_gid is not null) as total_count1 ," +
                    " (select count(product_gid) from pmr_mst_tproduct) as product_count";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardList = new List<getDashboardCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardList.Add(new getDashboardCount_List
                        {
                            mycalls_count = (dt["mycalls_count"].ToString()),
                            myleads_count = (dt["myleads_count"].ToString()),
                            myappointments_count = (dt["myappointments_count"].ToString()),
                            assignvisit_count = (dt["assignvisit_count"].ToString()),
                            completedvisit_count = (dt["completedvisit_count"].ToString()),
                            shared_proposal = (dt["shared_proposal"].ToString()),
                            completedorder_count = (dt["completedorder_count"].ToString()),
                            totalorder_count = (dt["totalorder_count"].ToString()),
                            total_count = (dt["total_count"].ToString()),
                            completedorder_count1 = (dt["completedorder_count1"].ToString()),
                            rejected_count = (dt["rejected_count"].ToString()),
                            cancel_count = (dt["cancel_count"].ToString()),
                            product_count = (dt["product_count"].ToString()),
                            total_count1 = (dt["total_count1"].ToString()),
                        });
                        values.getDashboardCount_List = getDashboardList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Dashboard count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        // My Calls Count
        public void DaGetDashboardQuotationAmount(string employee_gid, string user_gid, MdlCrmDashboard values)
        {


            try
            {
                 
                //msSQL = "SET GLOBAL sql_mode=\"STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION\"";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //msSQL = "SET SESSION sql_mode=\"STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION\"";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                //msSQL = "SELECT @@GLOBAL.sql_mode";
                //string lsglobal = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = "SELECT @@SESSION.sql_mode";
                //string lssession = objdbconn.GetExecuteScalar(msSQL);


                string YearLabel = DateTime.Now.Year.ToString();

                msSQL = " select year(quotation_date) as year ,MONTHNAME(quotation_date) month_name, " +
                        " round(sum(total_amount), 2) as total_amount from smr_trn_treceivequotation " +
                        " where quotation_date like '%" + YearLabel + "%'  " +
                        " group by year(quotation_date),month(quotation_date) order by year(quotation_date), " +
                        " month(quotation_date) ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<getDashboardQuotationAmt_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new getDashboardQuotationAmt_List
                        {
                            year = (dt["year"].ToString()),
                            month_name = (dt["month_name"].ToString()),
                            total_amount = (dt["total_amount"].ToString()),
                        });
                        values.getDashboardQuotationAmt_List = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Dashboard Quotation Amount!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }
        public void DaGetBarchartMonthlyLead(string employee_gid, string user_gid, MdlCrmDashboard values)
        {

            try
            {
                 
                //msSQL = "SET GLOBAL sql_mode=\"STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION\"";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //msSQL = "SET SESSION sql_mode=\"STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION\"";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                //msSQL = "SELECT @@GLOBAL.sql_mode";
                //string lsglobal = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = "SELECT @@SESSION.sql_mode";
                //string lssession = objdbconn.GetExecuteScalar(msSQL);

                string YearLabel = DateTime.Now.Year.ToString();

                msSQL = " select year(b.created_date) as year ,substring(date_format(b.created_date,'%M'),1,3)as month_name," +
                        " MONTH(b.created_date) AS month_number,count(a.leadbank_gid) as lead_count from crm_trn_tleadbank a  " +
                        " left join crm_trn_tlead2campaign b on b.leadbank_gid = a.leadbank_gid  " +
                        " where b.created_date like '%" + YearLabel + "%'  and b.assign_to= '" + employee_gid + "' GROUP BY year,month_name  order by year,month_name  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<getleadbasedonemployee_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new getleadbasedonemployee_List
                        {
                            lead_year = (dt["year"].ToString()),
                            lead_monthname = (dt["month_name"].ToString()),
                            lead_count = (dt["lead_count"].ToString()),
                        });
                        values.getleadbasedonemployee_List = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting BarChart Monthly Lead!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }        
        }

        //Social Media Lead count

        //public void DaGetsocialmedialeadcount(string employee_gid, string user_gid, MdlCrmDashboard values)
        //{


        //    try
        //    {
        //        objdbconn.OpenConn();
        //        msSQL = "select source_gid from crm_mst_tsource where source_name = 'Whatsapp'";
        //        string whatsapp_gid = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = "select source_gid from crm_mst_tsource where source_name = 'Mail'";
        //        string mail_gid = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = "select source_gid from crm_mst_tsource where source_name ='Shopify'";
        //        string shopify_gid = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = " select(select count(leadbank_gid)from crm_trn_tleadbank where source_gid = '" + null + "') as whatsapp_count," +
        //                " (select count(leadbank_gid)  from crm_trn_tleadbank where source_gid = '" + null + "') as mail_count," +
        //                " (select count(leadbank_gid)  from crm_trn_tleadbank where source_gid = '" + null + "') as shopify_count";

        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getDashboardQuotationList = new List<socialmedialeadcount>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getDashboardQuotationList.Add(new socialmedialeadcount
        //                {
        //                    whatsapp_count = dt["whatsapp_count"].ToString(),
        //                    shopify_count = dt["shopify_count"].ToString(),
        //                    mail_count = dt["mail_count"].ToString(),
        //                });
        //                values.socialmedialeadcount = getDashboardQuotationList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Getting Social Media Lead Count!";
        //    }
        //    finally
        //    {
        //        if (objOdbcDataReader  != null)
        //            objOdbcDataReader .Close();
        //        objdbconn.CloseConn();
        //    }


        //}


        public void DaGetsocialmedialeadcount(MdlCrmDashboard values)
        {
            try
            {


                string whatsappgid = string.Empty;
                string mailgid = string.Empty;
                string shopifygid = string.Empty;
                msSQL = "select source_gid from crm_mst_tsource where source_name = 'Whatsapp'";
                string whatsapp_gid = objdbconn.GetExecuteScalar(msSQL);
                if (whatsapp_gid == null || whatsapp_gid == "")
                {
                    whatsappgid = null;
                }
                else
                {
                    whatsappgid = whatsapp_gid;
                }

                msSQL = "select source_gid from crm_mst_tsource where source_name = 'Mail'";
                string mail_gid = objdbconn.GetExecuteScalar(msSQL);
                if (mail_gid == null || mail_gid == "")
                {
                    mailgid = null;
                }
                else
                {
                    mailgid = mail_gid;
                }

                msSQL = "select source_gid from crm_mst_tsource where source_name ='Shopify'";
                string shopify_gid = objdbconn.GetExecuteScalar(msSQL);
                if (shopify_gid == null || shopify_gid == "")
                {
                    shopifygid = null;
                }
                else
                {
                    shopifygid = shopify_gid;
                }
                msSQL = " select(select count(leadbank_gid)from crm_trn_tleadbank where source_gid = '" + whatsappgid + "') as whatsapp_count," +
                        " (select count(leadbank_gid)  from crm_trn_tleadbank where source_gid = '" + mailgid + "') as mail_count," +
                        " (select count(leadbank_gid)  from crm_trn_tleadbank where source_gid = '" + shopifygid + "') as shopify_count";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<socialmedialeadcount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new socialmedialeadcount
                        {
                            whatsapp_count = dt["whatsapp_count"].ToString(),
                            shopify_count = dt["shopify_count"].ToString(),
                            mail_count = dt["mail_count"].ToString(),
                        });
                        values.socialmedialeadcount = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetappointmentCount(MdlCrmDashboard values)
        {
            try
            {
                msSQL = "SELECT COUNT(leadbank_gid) as appointment_leadcount, DATE_FORMAT(appointment_date, '%d-%b') appointment_month FROM crm_trn_tappointment WHERE appointment_date >= CURDATE() AND appointment_date < CURDATE() + INTERVAL 7 DAY GROUP BY DATE_FORMAT(appointment_date, '%d-%b');";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<appointmentcount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new appointmentcount_list
                        {
                            appointment_leadcount = dt["appointment_leadcount"].ToString(),
                            appointment_month = dt["appointment_month"].ToString(),
                        });
                        values.appointmentcount_list = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGetcrmtilescount(MdlCrmDashboard values)
        {
            try
            {
                msSQL = "SELECT (SELECT COUNT(leadbank_gid) FROM crm_trn_tleadbank) AS total_count," +
                    "(SELECT COUNT(leadbank_gid) FROM crm_trn_tleadbank WHERE MONTH(created_date) = MONTH(CURDATE()) " +
                    "AND YEAR(created_date) = YEAR(CURDATE())) AS mtd_count,(SELECT DATE_FORMAT(CURDATE(), '%M')) AS mtd_month," +
                    "(select DATE_FORMAT(CURDATE(), '%Y')) as ytd_year,(SELECT COUNT(leadbank_gid) FROM crm_trn_tleadbank" +
                    " WHERE YEAR(created_date) = YEAR(CURDATE())) AS ytd_count,(SELECT COUNT(customer_gid) FROM crm_trn_tleadbank)" +
                    " AS customer_count;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<crmtilescount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new crmtilescount_list
                        {
                            total_count = dt["total_count"].ToString(),
                            mtd_count = dt["mtd_count"].ToString(),
                            mtd_month = dt["mtd_month"].ToString(),
                            ytd_count = dt["ytd_count"].ToString(),
                            customer_count = dt["customer_count"].ToString(),
                            ytd_year = dt["ytd_year"].ToString(),
                          
                        });
                        values.crmtilescount_list = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetcrmleadchart(MdlCrmDashboard values)
        {
            try
            {
                msSQL = "select date_format(created_date, '%b') as months, " +
                        "sum(case when customer_gid!='' then 1 else 0 end) as customer_count, " +
                        "sum(case when leadbank_gid!='' then 1 else 0 end) as lead_count " +
                        "from crm_trn_tleadbank where created_date >= date_sub(curdate(), interval 6 month) group by months order by created_date ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<crmleadchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new crmleadchart_list
                        {
                            lead_count = dt["lead_count"].ToString(),
                            months = dt["months"].ToString(),
                            customer_count = dt["customer_count"].ToString(),

                        });
                        values.crmleadchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetsentcampaignsentchart(MdlCrmDashboard values)
        {
            try
            {
                msSQL = "SELECT months,SUM(whatsappsent_count) AS whatsappsent_count, SUM(mailsent_count) AS mailsent_count " +
                        " FROM (SELECT DATE_FORMAT(created_date, '%b-%y') AS months,COUNT(version_id) AS whatsappsent_count, 0 AS mailsent_count " +
                        " FROM crm_trn_twhatsappmessages WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) " +
                        " GROUP BY months UNION ALL SELECT DATE_FORMAT(created_date, '%b-%y') AS months, 0 AS whatsappsent_count, COUNT(temp_mail_gid) AS mailsent_count " +
                        " FROM crm_smm_mailmanagement WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) GROUP BY months) AS combined GROUP BY months ORDER BY STR_TO_DATE(months, '%b-%y')";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<campaignsentchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new campaignsentchart_list
                        {
                            mailsent_count = dt["mailsent_count"].ToString(),
                            whatsappsent_count = dt["whatsappsent_count"].ToString(),
                            months = dt["months"].ToString()

                        });
                        values.campaignsentchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetoverallpropectchart(MdlCrmDashboard values)
        {
            try
            {
                msSQL = "SELECT date_format(created_date, '%b') AS months_detail, " +
                        " SUM(CASE WHEN leadstage_gid = '3' THEN 1 ELSE 0 END) AS prospects, " +
                        " SUM(CASE WHEN leadstage_gid = '4' THEN 1 ELSE 0 END) AS potential, " +
                        " SUM(CASE WHEN leadstage_gid = '5' THEN 1 ELSE 0 END) AS drop_leads, " +
                        " SUM(CASE WHEN leadstage_gid = '6' THEN 1 ELSE 0 END) AS customer " +
                        " FROM crm_trn_tlead2campaign WHERE created_date >= date_sub(curdate(), interval 6 month) GROUP BY months_detail ORDER BY created_date ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<leadstagechart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new leadstagechart_list
                        {
                            months_detail = dt["months_detail"].ToString(),
                            prospects = dt["prospects"].ToString(),
                            customer = dt["customer"].ToString(),
                            drop_leads = dt["drop_leads"].ToString(),
                            potential = dt["potential"].ToString()


                        });
                        values.leadstagechart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetcrmregionchart(MdlCrmDashboard values)
        {
            try
            {
                msSQL = "select count(b.region_name) as region_count,b.region_name from crm_trn_tleadbank a left join crm_mst_tregion b " +
                    " on a.leadbank_region=b.region_gid where a.leadbank_region is not null and a.created_date >= DATE_SUB(CURRENT_DATE(), " +
                    "INTERVAL 6 MONTH) group by b.region_name order by region_count DESC;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<crmregionchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new crmregionchart_list
                        {
                            region_count = dt["region_count"].ToString(),
                            region_name = dt["region_name"].ToString(),

                        });
                        values.crmregionchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetcrmsourcechart(MdlCrmDashboard values)
        {
            try
            {
                msSQL = "select count(b.source_name) as source_count,b.source_name from crm_trn_tleadbank a left join crm_mst_tsource b" +
                    "  on a.source_gid=b.source_gid where a.source_gid is not null and a.created_date >= DATE_SUB(CURRENT_DATE(), " +
                    "INTERVAL 6 MONTH) group by b.source_name order by source_count DESC;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<crmsourcechart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new crmsourcechart_list
                        {
                            source_count = dt["source_count"].ToString(),
                            source_name = dt["source_name"].ToString(),

                        });
                        values.crmsourcechart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}
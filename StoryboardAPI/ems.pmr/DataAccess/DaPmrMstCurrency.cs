
using System;
using System.Collections.Generic;
using ems.pmr.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;



namespace ems.pmr.DataAccess
{
    /// <summary>
    public class DaPmrMstCurrency
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1, objOdbcDataReader2;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetPmrCurrencySummary(MdlPmrMstCurrency values)
        {
            try
            {

                msSQL = " select  currencyexchange_gid,currency_code,format(exchange_rate,2) as  exchange_rate,country as country_name , CONCAT(b.user_firstname,' ',b.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date " +
                    " from crm_trn_tcurrencyexchange a " +
                    " left join adm_mst_tuser b on b.user_gid=a.created_by  GROUP BY a.currency_code  order by a.created_date desc,a.currencyexchange_gid desc";
                // %%%% Exchange rate integration query - DONT DELETE %%%%

//                msSQL = " SELECT e.currencyexchange_gid,e.currency_code,e.exchange_rate,e.country AS country_name,CONCAT(b.user_firstname, ' ', b.user_lastname) AS created_by, " +
//" DATE_FORMAT(e.created_date, '%d-%m-%Y') AS created_date FROM crm_trn_tcurrencyexchange e JOIN (SELECT currency_code, MAX(created_date) AS max_created_date " +
//" FROM crm_trn_tcurrencyexchange GROUP BY currency_code) m ON e.currency_code = m.currency_code AND e.created_date = m.max_created_date " +
//" LEFT JOIN adm_mst_tuser b ON b.user_gid = e.created_by GROUP BY e.currency_code order by e.currency_code asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<currency_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new currency_list
                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            country_name = dt["country_name"].ToString(),
                        });
                        values.currency_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting the currency summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetPmrCountryDtl(MdlPmrMstCurrency values)
        {
            try
            {

                msSQL = " select  country_gid, country_code, country_name " +
                        " from adm_mst_tcountry ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcountrydropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcountrydropdown
                        {
                            country_gid = dt["country_gid"].ToString(),
                            country_name = dt["country_name"].ToString(),
                        });
                        values.GetPmrCountryDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting the country details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaPostPmrCurrency(string user_gid, currency_list values)
        {
            try
            {

                msSQL = " select * from crm_trn_tcurrencyexchange where country_gid= '" + values.country_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows)
                {
                    
                    values.message = "Country Name Already Exist";
                    objOdbcDataReader.Close();
                }
                else
                {
                    msSQL = " select currency_code from crm_trn_tcurrencyexchange where currency_code= '" + values.currency_code + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    
                    if (objOdbcDataReader.HasRows)
                    {
                         
                        values.message = "Currency Code Already Exist";
                        objOdbcDataReader.Close();  
                    }
                    else
                    {

                        msGetGid = objcmnfunctions.GetMasterGID("CUR");
                        msSQL = " Select country_name from adm_mst_tcountry where country_gid= '" + values.country_name + "'";
                        string lscountry_name = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " insert into crm_trn_tcurrencyexchange(" +
                                " currencyexchange_gid," +
                                " currency_code," +
                                " country_gid," +
                                " exchange_rate," +
                                " country," +
                                 "updated_by," +
                                "updated_date," +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + values.currency_code.Trim().Replace("'", "\\\'") + "'," +
                                " '" + values.country_name + "'," +
                                " '" + values.exchange_rate.Replace("'", "\\'") + "',";
                        if (lscountry_name == null || lscountry_name == "")
                        {
                            msSQL += "'',";
                        }
                        else
                        {
                            msSQL += "'" + lscountry_name.Replace("'", "\\\'") + "',";
                        }
                        msSQL += "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'" + user_gid + "'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Currency Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Currency";
                        }
                    }
                }
                objOdbcDataReader.Close();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaPmrCurrencyUpdate(string user_gid, currency_list values)
        {
            try
            {
                msSQL = "select currency_code,exchange_rate,country from crm_trn_tcurrencyexchange where currencyexchange_gid = '" + values.currencyexchange_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (values.currency_codeedit != objOdbcDataReader["currency_code"].ToString() || values.country_nameedit != objOdbcDataReader["country"].ToString() || values.exchange_rateedit != objOdbcDataReader["exchange_rate"].ToString())
                {
                    msSQL = "insert into crm_trn_tcurrencyexchangehistory select * from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currencyexchange_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " Select country_gid from adm_mst_tcountry where country_name= '" + values.country_nameedit + "'";
                        string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                        if (values.currency_codeedit != objOdbcDataReader["currency_code"].ToString())
                        {
                            msSQL = " select currency_code from crm_trn_tcurrencyexchange where currency_code= '" + values.currency_codeedit + "'";
                            objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
                            objOdbcDataReader1.Read();
                            if (objOdbcDataReader1.HasRows)
                            {
                                objOdbcDataReader1.Close();
                                values.status = false;
                                values.message = "Currency Code Already Exist";
                                return;
                               
                            }
                        }
                        if (values.country_nameedit != objOdbcDataReader["country"].ToString())
                        {
                            msSQL = "select country_gid from crm_trn_tcurrencyexchange where country_gid= '" + lscountry_gid + "'";
                            objOdbcDataReader2 = objdbconn.GetDataReader(msSQL);
                            objOdbcDataReader2.Read();
                            if (objOdbcDataReader2.HasRows)
                            {
                                objOdbcDataReader2.Close();
                                values.message = "Country Name Already Exist";
                                return;
                            }
                        }
                        msSQL = " update  crm_trn_tcurrencyexchange set " +
                         " currency_code = '" + values.currency_codeedit + "'," +
                         " exchange_rate = '" + values.exchange_rateedit + "'," +
                         " country = '" + values.country_nameedit + "'," +
                         " country_gid = '" + lscountry_gid + "'," +
                         " updated_by = '" + user_gid + "'," +
                         " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where currencyexchange_gid='" + values.currencyexchange_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update  crm_trn_tcurrencyexchangehistory set " +
                               " user_name = '" + user_gid + "'" +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               " where currencyexchange_gid='" + values.currencyexchange_gid + "'";
                            values.status = true;
                            values.message = "Currency Updated Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Currency";
                        }



                    }
                    else
                    {
                        values.message = "Error While Adding Currency";
                    }

                }
                else
                {
                    values.message = "No changes in the update!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }




        }

        public void DaPmrCurrencySummaryDelete(string currencyexchange_gid, currency_list values)
        {
            try
            {
                bool quotation_flag = false, sales_flag = false;
                msSQL = "SELECT * FROM smr_trn_treceivequotationdtl a  " +
                        " inner join smr_trn_treceivequotation b on a.quotation_gid=b.quotation_gid " +
                        " where '" + currencyexchange_gid + "' in (b.currency_gid) and b.delete_flag='N'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    quotation_flag = true;
                    objOdbcDataReader.Close();
                }
                msSQL = " SELECT * FROM smr_trn_tsalesorderdtl a " +
                        " inner join smr_trn_tsalesorder b on a.salesorder_gid=b.salesorder_gid " +
                        " where '" + currencyexchange_gid + "' in (b.currency_gid) ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    sales_flag = true;
                    objOdbcDataReader.Close();
                }
                if (!(sales_flag || quotation_flag))
                {

                    msSQL = "  delete from crm_trn_tcurrencyexchange where currencyexchange_gid='" + currencyexchange_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Currency Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Currency";
                }
                }
                else
                {
                    values.message = "Cannot delete currency since it is involved in transactions!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetBreadCrumb(string user_gid, string module_gid, MdlPmrMstCurrency values)
        {
            try
            {

                msSQL = "   select a.module_name as module_name3,a.sref as sref3,b.module_name as module_name2 ,b.sref as sref2,c.module_name as module_name1,c.sref as sref1  from adm_mst_tmodule a " +
                        " left join adm_mst_tmodule  b on b.module_gid=a.module_gid_parent" +
                        " left join adm_mst_tmodule  c on c.module_gid=b.module_gid_parent" +
                        " where a.module_gid='" + module_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<breadcrumb_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new breadcrumb_list
                        {


                            module_name1 = dt["module_name1"].ToString(),
                            sref1 = dt["sref1"].ToString(),
                            module_name2 = dt["module_name2"].ToString(),
                            sref2 = dt["sref2"].ToString(),
                            module_name3 = dt["module_name3"].ToString(),
                            sref3 = dt["sref3"].ToString(),

                        });
                        values.breadcrumb_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting BreadCrumb!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                 $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

    }
}
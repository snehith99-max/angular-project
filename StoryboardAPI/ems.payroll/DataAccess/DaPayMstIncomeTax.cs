using ems.payroll.Models;
using ems.utilities.Functions;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using MySql.Data.MySqlClient;

using static System.Collections.Specialized.BitVector32;
using System.Drawing;

namespace ems.payroll.DataAccess
{
    public class DaPayMstIncomeTax
    {
        HttpPostedFile httpPostedFile;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult1, mnResult2, mnResult3;
        string msGetGid, msGetGid1, msGetGid2, msGetDlGID2, msGetGID1, lsempoyeegid, exemployee_code, lsemployeegid, lsbankname, lstax_name, lsincome_tax_rate_code;

        public void DaGetIncomeTaxMasterSummary(MdlPayMstIncomeTax values)
        {
            try
            {
                msSQL = " select tax_regime_gid, tax_name, concat(tax_slabs_fromold, ' to ', tax_slabs_toold) as tax_slab, tax_slabs_fromold, tax_slabs_toold, individuals, resident_senior_citizens, resident_super_senior_citizens, remarks_old " +
                        " from acp_mst_tincometax_regime where tax_name = 'Old Regime' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetIncomeMaster_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetIncomeMaster_list
                        {
                            tax_regime_gid = dt["tax_regime_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_slab = dt["tax_slab"].ToString(),
                            individuals = dt["individuals"].ToString(),
                            resident_senior_citizens = dt["resident_senior_citizens"].ToString(),
                            resident_super_senior_citizens = dt["resident_super_senior_citizens"].ToString(),
                            remarks_old = dt["remarks_old"].ToString(),
                            tax_slabs_fromold = dt["tax_slabs_fromold"].ToString(),
                            tax_slabs_toold = dt["tax_slabs_toold"].ToString(),
                        });
                        values.GetIncomeMaster_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Income Tax summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetIncomeTaxMasterNew(MdlPayMstIncomeTax values)
        {
            try
            {
                msSQL = " select tax_regime_gid, tax_name, concat(tax_slabs_fromnew, ' to ', tax_slabs_tonew) as tax_slabnew, tax_slabs_fromnew, tax_slabs_tonew, income_tax_rates, remarks_new, " +
                        " case when remarks_new <> '' then concat(income_tax_rates, ' ', '( ', remarks_new, ' )') else income_tax_rates end as income_tax_rates1 " +
                        " from acp_mst_tincometax_regime where tax_name = 'New Regime' order by created_date ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetIncomeMasterNew_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetIncomeMasterNew_list
                        {
                            tax_regime_gid = dt["tax_regime_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_slabnew = dt["tax_slabnew"].ToString(),
                            income_tax_rates = dt["income_tax_rates"].ToString(),
                            remarks_new = dt["remarks_new"].ToString(),
                            tax_slabs_fromnew = dt["tax_slabs_fromnew"].ToString(),
                            tax_slabs_tonew = dt["tax_slabs_tonew"].ToString(),
                            income_tax_rates1 = dt["income_tax_rates1"].ToString(),
                           
                        });
                        values.GetIncomeMasterNew_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Income Tax summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostIncomeTaxRates(string user_gid, incometax_list values)
        {
            try
            {

                        msSQL = " insert into acp_mst_tincometax_regime(" +
                                " tax_name," +
                                " tax_slabs_fromold," +
                                " tax_slabs_toold," +
                                " tax_slabs_fromnew," +
                                " tax_slabs_tonew," +
                                " individuals, " +
                                " resident_senior_citizens," +
                                " resident_super_senior_citizens, " +
                                " income_tax_rates, " +
                                " remarks_old, " +
                                " remarks_new, " +
                                " created_by, " +
                                " created_date) " +
                                " values(" +
                                " '" + values.tax_name + "'," +
                                "'" + values.tax_slabs_fromold + "'," +
                                "'" + values.tax_slabs_toold + "'," +
                                "'" + values.tax_slabs_fromnew + "'," +
                                "'" + values.tax_slabs_tonew + "'," +
                                "'" + values.individuals + "'," +
                                " '" + values.resident_senior_citizens + "'," +
                                "'" + values.resident_super_senior_citizens + "'," +
                                " '" + values.income_tax_rates + "'," +
                                " '" + values.remarks_old + "'," +
                                " '" + values.remarks_new + "',";




                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Income Tax Rate Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Income Tax Rate";
                        }
                    
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Income Tax!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DagetUpdatedIncomeTaxRates(string user_gid, incometax_list values)
        {
            try
            {

                msSQL = " update  acp_mst_tincometax_regime set " +
                     " tax_regime_gid = '" + values.tax_regime_gid + "'," +
                     " tax_name = '" + values.tax_nameedit + "'," +
                     " tax_slabs_fromold = '" + values.tax_slabs_fromoldedit + "'," +
                     " tax_slabs_toold = '" + values.tax_slabs_tooldedit + "'," +
                     " tax_slabs_fromnew = '" + values.tax_slabs_fromnewedit + "'," +
                     " tax_slabs_tonew = '" + values.tax_slabs_tonewedit + "'," +
                     " individuals = '" + values.individuals_edit + "'," +
                     " resident_senior_citizens = '" + values.resident_senior_citizensedit + "'," +
                     " resident_super_senior_citizens = '" + values.resident_super_senior_citizensedit + "'," +
                     " income_tax_rates = '" + values.income_tax_ratesedit + "'," +
                     " remarks_old = '" + values.remarksold_edit + "'," +
                     " remarks_new = '" + values.remarksnew_edit + "'," +
                     " updated_by = '" + user_gid + "'," +
                     " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where tax_regime_gid='" + values.tax_regime_gid + "' ";
                     

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Income Tax Rate Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Income Tax Rate";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Income Tax Rate!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
            
            


    }
}
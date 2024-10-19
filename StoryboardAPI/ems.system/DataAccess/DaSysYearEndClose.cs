using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Data.Odbc;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace ems.system.DataAccess
{
    public class DaSysYearEndClose
    {
        string msSQL = string.Empty;
        DataTable dttable;
        dbconn conn = new dbconn();
        cmnfunctions objcmnfn = new cmnfunctions();
        OdbcDataReader odbcreader;
        int lsfyearnow, lsconvertyear, lscurrentyear, lsstartyear, convert_startYr;
        int i, years, lsfyearstart, year, years_new;
        string lscurval, msgetGID, lsfinyear;
        int mnResult;
        public void DaGetYearendactivities(MdlSysYearEndClose values)
        {
            try
            {
                msSQL = " select finyear_gid, date_format(fyear_start,'%m-%Y') as fyear_start, " +
                    " year(fyear_start) as start_year,year(fyear_end) as end_year, " +
                    " date_format(fyear_end,'%m-%Y') as fyear_end, yearendactivity_flag, " +
                    " unauditedclosing_flag, auditedclosing_flag, created_by, created_date, " +
                    " updated_by, updated_date " +
                    " from adm_mst_tyearendactivities where 0=0";
                dttable = conn.GetDataTable(msSQL);
                var GetYearEndDetails = new List<GetYearEndDetails_list>();
                if (dttable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dttable.Rows)
                    {
                        GetYearEndDetails.Add(new GetYearEndDetails_list
                        {
                            finyear_gid = dt["finyear_gid"].ToString(),
                            fyear_start = dt["fyear_start"].ToString(),
                            start_year = dt["start_year"].ToString(),
                            end_year = dt["end_year"].ToString(),
                            fyear_end = dt["fyear_end"].ToString(),
                            yearendactivity_flag = dt["yearendactivity_flag"].ToString(),
                            unauditedclosing_flag = dt["unauditedclosing_flag"].ToString(),
                            auditedclosing_flag = dt["auditedclosing_flag"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.GetYearEndDetails_list = GetYearEndDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfn.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostYearendactivities(string finyear_gid, string start_year, string usergid, MdlSysYearEndClose values)
        {
            try
            {
                lsfinyear = finyear_gid;

                msSQL = " select year(fyear_start) as fyear_start  from adm_mst_tyearendactivities where finyear_gid='" + lsfinyear + "'";
                odbcreader = conn.GetDataReader(msSQL);
                if (odbcreader.HasRows == true)
                {
                    lsfyearstart = Convert.ToInt32(odbcreader["fyear_start"].ToString());
                    odbcreader.Close();
                }
                convert_startYr = (int)Convert.ToInt32(start_year);

                lsfyearnow = DateTime.Now.Year;

                lscurrentyear = lsfyearnow - lsfyearstart;

                for (i = 1; i <= lscurrentyear; i++)
                {
                    msSQL = " select sequence_gid, sequence_code, sequence_name, " +
                           " sequence_format, sequence_curval, sequence_flag, branch_flag, " +
                           " department_flag, year_flag, month_flag, location_flag, company_code, " +
                           " delimeter, runningno_prefix, finyear, carry_forward " +
                           " from adm_mst_tsequence where finyear='" + lsfyearstart + "'";
                    dttable = conn.GetDataTable(msSQL);

                    years = lsfyearstart + i;

                    year = convert_startYr + 1;

                    years_new = years + 1;

                    foreach (DataRow objRow in dttable.Rows)
                    {
                        if (objRow["carry_forward"].ToString() == "N")
                        {
                            lscurval = "0";
                        }
                        else
                        {
                            lscurval = objRow["sequence_curval"].ToString();
                        }

                        msgetGID = objcmnfn.GetMasterGID("SSQM");

                        msSQL = " insert into adm_mst_tsequence( " +
                                    " sequence_gid, " +
                                    " sequence_code, " +
                                    " sequence_name, " +
                                    " sequence_format, " +
                                    " sequence_curval, " +
                                    " sequence_flag, " +
                                    " branch_flag, " +
                                    " department_flag, " +
                                    " year_flag, " +
                                    " month_flag, " +
                                    " location_flag, " +
                                    " company_code, " +
                                    " delimeter, " +
                                    " runningno_prefix, " +
                                    " finyear, " +
                                    " carry_forward)" +
                                    " values( " +
                                    "'" + msgetGID + "', " +
                                    "'" + objRow["sequence_code"] + "', " +
                                    "'" + objRow["sequence_name"] + "', " +
                                    "'" + objRow["sequence_format"] + "', " +
                                    "'" + lscurval + "', " +
                                    "'" + objRow["sequence_flag"] + "', " +
                                    "'" + objRow["branch_flag"] + "', " +
                                    "'" + objRow["department_flag"] + "', " +
                                    "'" + objRow["year_flag"] + "', " +
                                    "'" + objRow["month_flag"] + "', " +
                                    "'" + objRow["location_flag"] + "', " +
                                    "'" + objRow["company_code"] + "', " +
                                    "'" + objRow["delimeter"] + "', " +
                                    "'" + objRow["runningno_prefix"] + "', " +
                                    "'" + years + "', " +
                                    "'" + objRow["carry_forward"] + "') ";
                        mnResult = conn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (years != lsfyearnow)
                    {
                        msSQL = " update adm_mst_tyearendactivities set fyear_end='" + years + "-03-31" + "', " +
                                " yearendactivity_flag='Y', " +
                                " updated_by='" + usergid + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " where finyear_gid='" + lsfinyear + "'";
                        mnResult = conn.ExecuteNonQuerySQL(msSQL);



                        msSQL = "insert into adm_mst_tyearendactivities( " +
                                    " fyear_start," +
                                    " fyear_end," +
                                    " created_by, " +
                                    " created_date)" +
                                    " values( " +
                                    "'" + years + "-04-01" + "', " +
                                    "'" + years_new + "-03-31" + "'," +
                                    "'" + usergid + "', " +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = conn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "insert into adm_mst_tyearendactivities( " +
                                   " fyear_start," +
                                   " created_by, " +
                                   " created_date)" +
                                   " values( " +
                                  "'" + years + "-04-01" + "', " +
                                   "'" + usergid + "', " +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = conn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Successfully updated the year end activities";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occurred while updating year end activities";
                    }
                }
                values.status = false;
                values.message = "Error occurred while updating year end activities";
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfn.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}
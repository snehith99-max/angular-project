using ems.sales.Models;
using ems.utilities.Functions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ems.sales.DataAccess
{
    public class DaSmrMstSequenceCodeCustomizer
    {
        int mnResult;
        DataTable dt_datatable;
        string msSQL = string.Empty;
        cmnfunctions objconn = new cmnfunctions();
        dbconn objdbconn = new dbconn();
        private OdbcDataReader objOdbcDataReader;
        string lsfinyear;

        public void DaGetSequenceCodeCustomizer(MdlSmrMstSequenceCodeCustomizer values)
        {
            try 
            {
                
                    msSQL = " select year(fyear_start) as finyear from adm_mst_tyearendactivities " +
                    " order by finyear_gid desc limit 0,1";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if(objOdbcDataReader.HasRows == true)
                    {
                    
                        lsfinyear = objOdbcDataReader["finyear"].ToString();
                   
                    }
               
               msSQL = " select a.sequencecodecustomizer_gid,a.sequence_code,a.sequence_name,case when a.branch_flag='Y' then 'Yes' else 'No' end as branch_flag," +
                " case when a.businessunit_flag ='Y' then 'Yes' else 'No' end as businessunit_flag," +
                " case when a.department_flag='Y' then 'Yes' else 'No' end as department_flag," +
                " case when a.year_flag='Y' then 'Yes' else 'No' end as year_flag," +
                " a.company_code,a.delimeter," +
                " case when a.location_flag='Y' then 'Yes' else 'No' end as location_flag, " +
                " case when a.month_flag='Y' then 'Yes' else 'No' end as month_flag, a.runningno_prefix,b.branch_name " +
                " from adm_mst_tsequencecodecustomizer a " +
                " left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                " where a.sequence_flag='Y' and a.finyear='" + lsfinyear + "' " +
                "  and a.module_name='RBL' Order by a.sequencecodecustomizer_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<SequenceCodeSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new SequenceCodeSummary
                        {
                            sequencecodecustomizer_gid = dt["sequencecodecustomizer_gid"].ToString(),
                            sequence_code = dt["sequence_code"].ToString(),
                            sequence_name = dt["sequence_name"].ToString(),
                            branch_flag = dt["branch_flag"].ToString(),
                            businessunit_flag = dt["businessunit_flag"].ToString(),
                            department_flag = dt["department_flag"].ToString(),
                            year_flag = dt["year_flag"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            delimeter = dt["delimeter"].ToString(),
                            location_flag = dt["location_flag"].ToString(),
                            month_flag = dt["month_flag"].ToString(),
                            runningno_prefix = dt["runningno_prefix"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.SequenceCodeSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Loading Sequence Code Summary !";
                objconn.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetSequenceCodeCustomizerEdit(string sequencecodecustomizer_gid,MdlSmrMstSequenceCodeCustomizer values)
        {
            try
            {
                msSQL = " select a.sequencecodecustomizer_gid,a.sequence_code,a.sequence_name,a.branch_flag, a.sequence_curval," +
                " a.department_flag,a.year_flag,a.company_code,a.delimeter,a.location_flag, a.month_flag, a.runningno_prefix,a.businessunit_flag  " +
                " from adm_mst_tsequencecodecustomizer a " +
                " where a.sequencecodecustomizer_gid='" + sequencecodecustomizer_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<SequenceCodeSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new SequenceCodeSummary
                        {
                           sequence_curval = dt["sequence_curval"].ToString(),
                            sequence_code = dt["sequence_code"].ToString(),
                            sequence_name = dt["sequence_name"].ToString(),
                            branch_flag = dt["branch_flag"].ToString(),
                            businessunit_flag = dt["businessunit_flag"].ToString(),
                            department_flag = dt["department_flag"].ToString(),
                            year_flag = dt["year_flag"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            delimeter = dt["delimeter"].ToString(),
                            location_flag = dt["location_flag"].ToString(),
                            month_flag = dt["month_flag"].ToString(),
                            runningno_prefix = dt["runningno_prefix"].ToString(),
                           
                        });
                        values.SequenceCodeSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Sequence Code Details!";
                objconn.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetSequenceCodeCustomizerUpdate(SequenceCodeSummary values)
        {
            msSQL = " select sequence_code,finyear from adm_mst_tsequencecodecustomizer where sequencecodecustomizer_gid='" + values.sequencecodecustomizer_gid + "' ";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows ==  true)
            {
              
                string lssequence_code = objOdbcDataReader["sequence_code"].ToString();
                string lsfinyear = objOdbcDataReader["finyear"].ToString();

                msSQL = "select sequence_gid from adm_mst_tsequence where sequence_code='" + lssequence_code + "' and finyear='" + lsfinyear + "' ";
                string lssequence_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update adm_mst_tsequence set sequence_flag='Y', " +
                    " branch_flag='" + values.branch_flag + "', " +
                    " department_flag='" + values.department_flag + "', " +
                    " year_flag='" + values.year_flag + "', " +
                    " month_flag='" + values.month_flag + "', " +
                    " location_flag='" + values.location_flag + "', " +
                    " company_code='" + values.company_code.Replace("'","") + "', " +
                    " delimeter='" +values.delimeter + "', " +
                    " runningno_prefix= '" + values.runningno_prefix + "' " +
                    " where sequence_gid='" + lssequence_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0) 
                {
                    values.status = false;
                    values.message = "Error Occurred while updating the sequence";
                }
               
            }
            msSQL = " update adm_mst_tsequencecodecustomizer set  " +
                " company_code='" + values.company_code.Replace("'","") + "'," +
                " branch_flag='" + values.branch_flag + "'," +
                " businessunit_flag ='" + values.businessunit_flag + "'," +
                " department_flag='" + values.department_flag + "'," +
                " location_flag='" + values.location_flag + "'," +
                " year_flag='" + values.year_flag + "'," +
                " delimeter='" +values.delimeter + "', " +
                " month_flag='" + values.month_flag + "', " +
                " runningno_prefix= '" + values.runningno_prefix + "', " +
                "sequence_curval='" +values.sequence_curval + "' " +
                " where sequencecodecustomizer_gid='" + values.sequencecodecustomizer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Sequence Code Updating Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error Occurred while updating the sequence";
            }
        }
    }
}

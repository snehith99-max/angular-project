using ems.pmr.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.pmr.DataAccess
{
    public class DaPmrTaxSegment
    {
        string msSQL, msGetGid;
        DataTable dt_datatable;
        OdbcDataReader objOdbcDataReader;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        int mnResult;
        public void DaGetTaxSegmentSummary(MdlPmrTaxSegment values)
        {
            try
            {

                msSQL = "select a.taxsegment_gid,a.taxsegment_name,a.taxsegment_code,a.taxsegment_description,count(b.vendor_gid) as assignvendor," +
                        " a.active_flag,a.created_by,a.created_date from acp_mst_ttaxsegment a" +
                        " left join acp_mst_tvendor b on a.taxsegment_gid=b.taxsegment_gid where  reference_type='Vendor'" +
                        " group by taxsegment_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetTaxSummary = new List<PmrTaxSegment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetTaxSummary.Add(new PmrTaxSegment_list
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_code = dt["taxsegment_code"].ToString(),
                            taxsegment_description = dt["taxsegment_description"].ToString(),
                            assignvendor = dt["assignvendor"].ToString(),
                            active_flag = dt["active_flag"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),

                        });
                        values.PmrTaxSegment_list = GetTaxSummary;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetTaxSegmentAssignVendorSummary(MdlPmrTaxSegment values)
        {
            try
            {

                msSQL = "select a.vendor_code,a.vendor_companyname,a.vendor_gid," +
                         " Concat(a.contactperson_name,' / ',a.contact_telephonenumber,' / ',a.email_id) as Contactdetails,b.country,c.region_name" +
                         " from acp_mst_Tvendor a" +
                         " left join crm_trn_Tcurrencyexchange b on a.currencyexchange_gid = b.currencyexchange_gid" +
                         " left join crm_mst_tregion c on a.region_gid=c.region_gid where a.taxsegment_gid is  null";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetTaxSummary = new List<Taxsegment2assignvendorList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetTaxSummary.Add(new Taxsegment2assignvendorList
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            Contactdetails = dt["Contactdetails"].ToString(),
                            country = dt["country"].ToString(),
                            region_name = dt["region_name"].ToString(),
                        });
                        values.Taxsegment2assignvendorList = GetTaxSummary;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetTaxSegmentunAssignVendorSummary(string taxsegment_gid, MdlPmrTaxSegment values)
        {
            try
            {

                msSQL = "select a.vendor_code,a.vendor_companyname,a.vendor_gid," +
                         " Concat(a.contactperson_name,' / ',a.contact_telephonenumber,' / ',a.email_id) as Contactdetails,b.country,c.region_name" +
                         " from acp_mst_Tvendor a" +
                         " left join crm_trn_Tcurrencyexchange b on a.currencyexchange_gid = b.currencyexchange_gid" +
                         " left join crm_mst_tregion c on a.region_gid=c.region_gid where a.taxsegment_gid ='" + taxsegment_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetTaxSummary = new List<Taxsegment2unassignvendorList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetTaxSummary.Add(new Taxsegment2unassignvendorList
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            Contactdetails = dt["Contactdetails"].ToString(),
                            country = dt["country"].ToString(),
                            region_name = dt["region_name"].ToString(),
                        });
                        values.Taxsegment2unassignvendorList = GetTaxSummary;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void PostPmrTaxSegment(string user_gid, PmrTaxSegmentSummary_list values)
        {
            try
            {
                string lsreferencetype = "Vendor";
                msSQL = " select taxsegment_name from acp_mst_ttaxsegment where taxsegment_name= '" + values.taxsegment_name.Replace("'", "\\\'") + "' and reference_type='Vendor'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Tax Segment Name Already Exist";
                    return;
                }
                msSQL = " select taxsegment_code from acp_mst_ttaxsegment where taxsegment_code= '" + values.taxsegment_code.Replace("'", "\\\'") + "'and reference_type = 'Vendor'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Tax Segment Prefix Already Exist";
                    return;
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("TXSG");
                    msSQL = " insert into acp_mst_ttaxsegment(" +
                            " taxsegment_gid, " +
                            " taxsegment_code," +
                            " taxsegment_name," +
                            " taxsegment_description," +
                            " reference_type," +
                            " active_flag," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                    " '" + msGetGid + "'," +
                             " '" + values.taxsegment_code.Replace("'", "\\\'") + "',";
                    if (values.taxsegment_name == null || values.taxsegment_name == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.taxsegment_name.Replace("'", "\\\'") + "',";
                    }
                    if (values.taxsegment_description == null || values.taxsegment_description == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.taxsegment_description.Replace("'", "\\\'") + "',";
                    }
                   
                     msSQL += "'" + lsreferencetype + "'," +
                              "'" + values.active_flag + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Tax Segment Added Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Tax Segment";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
                $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaUpdatedTaxSegmentSummary(string user_gid, PmrTaxSegmentSummary_list values)
        {
            try
            {
                string lsreferencetype = "Vendor";

                msSQL = "SELECT taxsegment_name, taxsegment_code FROM acp_mst_ttaxsegment WHERE taxsegment_gid != '" + values.taxsegment_gid + "' and reference_type = 'Vendor' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                bool nameExists = false;
                bool prefixExists = false;

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    if (dt["taxsegment_name"].ToString() == values.taxsegment_name_edit)
                    {
                        nameExists = true;
                    }
                    if (dt["taxsegment_code"].ToString() == values.taxsegment_code_edit)
                    {
                        prefixExists = true;
                    }
                }

               
                if (nameExists && prefixExists)
                {
                    values.status = false;
                    values.message = "No changes made.";
                    return;
                }
                else if (nameExists)
                {
                    values.status = false;
                    values.message = "Tax Segment name already exists";
                    return;
                }
                else if (prefixExists)
                {
                    values.status = false;
                    values.message = "Tax Segment prefix code already exists";
                    return;
                }

               
                msSQL = "UPDATE acp_mst_ttaxsegment SET " +
                        "taxsegment_name = '" + values.taxsegment_name_edit.Replace("'", "\\\'") + "'," +
                        "taxsegment_description = '" + values.taxsegment_description_edit.Replace("'", "\\\'") + "'," +
                        "reference_type = '" + lsreferencetype + "'," +
                        "active_flag = '" + values.active_flag_edit + "'," +
                        "updated_by = '" + user_gid + "'," +
                        "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        "WHERE taxsegment_gid = '" + values.taxsegment_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Tax Segment Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Tax Segment";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Exception occurred while Updating TaxSegment!";
               
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:" +
                    $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
 
        public void DaPostPmrTaxSegment2Vendor(string user_gid, PostPmrTaxSegment2Vendor_list values)
        {
            try
            {

                foreach(var data in  values.Taxsegment2unassignvendorList) { 

                msSQL = " update  acp_mst_Tvendor set " +
          " taxsegment_gid  = '" + values.taxsegment_gid + "'," +
          " updated_by = '" + user_gid + "'," +
          " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + 
          "' where vendor_gid='" + data.vendor_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Tax Segment Updated Successfully";
                    return;

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Tax Segment";
                    return;
                }
                

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating TaxSegment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaPostPmrTaxSegment2unassignVendor(string user_gid, PostPmrTaxSegment2unassignVendor_list values)
        {
            try
            {

                foreach (var data in values.Taxsegment2assignvendorList)
                {

                    msSQL = " update  acp_mst_Tvendor set " +
              " taxsegment_gid  = null," +
              " updated_by = '" + user_gid + "'," +
              " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
              "' where vendor_gid='" + data.vendor_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Vendor UnAssigned Successfully";
                    return;

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Tax Segment";
                    return;
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating TaxSegment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetTotalWithinState(MdlPmrTaxSegment values)
        {
            try
            {
                msSQL = " SELECT a.vendor_companyname,a.vendor_gid, a.contactperson_name, a.address_gid,concat(c.address1) as vendor_address, a.vendor_code, a.taxsegment_gid, b.taxsegment_name FROM acp_mst_tvendor a " +
                        " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid " +
                        " left join adm_mst_taddress c on a.address_gid = c.address_gid WHERE b.taxsegment_name LIKE 'Within%'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetWithInState = new List<GetPmrWithInState_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetWithInState.Add(new GetPmrWithInState_list
                        {
                            vendor_companyname = wtin["vendor_companyname"].ToString(),
                            vendor_gid = wtin["vendor_gid"].ToString(),
                            contactperson_name = wtin["contactperson_name"].ToString(),
                            vendor_address = wtin["vendor_address"].ToString(),
                            vendor_code = wtin["vendor_code"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetPmrWithInState_list = GetWithInState;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetInterstateSegmentSummary(MdlPmrTaxSegment values)
        {
            try
            {
                msSQL = " SELECT a.vendor_companyname, a.vendor_gid,a.contactperson_name, a.address_gid,concat(c.address1) as vendor_address, a.vendor_code, a.taxsegment_gid, b.taxsegment_name FROM acp_mst_tvendor a " +
                        " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid " +
                        " left join adm_mst_taddress c on a.address_gid = c.address_gid WHERE b.taxsegment_name LIKE 'Inter%'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetWithInState = new List<GetPmrinterstate_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetWithInState.Add(new GetPmrinterstate_list
                        {
                            vendor_companyname = wtin["vendor_companyname"].ToString(),
                            vendor_gid = wtin["vendor_gid"].ToString(),
                            contactperson_name = wtin["contactperson_name"].ToString(),
                            vendor_address = wtin["vendor_address"].ToString(),
                            vendor_code = wtin["vendor_code"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetPmrinterstate_list = GetWithInState;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetOverseasSegmentSummary(MdlPmrTaxSegment values)
        {
            try
            {
                msSQL = " SELECT a.vendor_companyname,a.vendor_gid, a.contactperson_name, a.address_gid,concat(c.address1) as vendor_address, a.vendor_code, a.taxsegment_gid, b.taxsegment_name FROM acp_mst_tvendor a " +
                        " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid " +
                        " left join adm_mst_taddress c on a.address_gid = c.address_gid WHERE b.taxsegment_name LIKE 'Overseas%' or b.taxsegment_name like '% Overseas%'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetWithInState = new List<GetPmrOverSeas_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetWithInState.Add(new GetPmrOverSeas_list
                        {
                            vendor_companyname = wtin["vendor_companyname"].ToString(),
                            vendor_gid = wtin["vendor_gid"].ToString(),
                            contactperson_name = wtin["contactperson_name"].ToString(),
                            vendor_address = wtin["vendor_address"].ToString(),
                            vendor_code = wtin["vendor_code"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetPmrOverSeas_list = GetWithInState;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetOthersSegmentSummary(MdlPmrTaxSegment values)
        {
            try
            {
                msSQL = " SELECT a.vendor_companyname,a.vendor_gid, a.contactperson_name, a.address_gid,concat(c.address1) as vendor_address, a.vendor_code, a.taxsegment_gid, b.taxsegment_name FROM acp_mst_tvendor a " +
                        " left join adm_mst_taddress c on a.address_gid = c.address_gid" +
                        " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid" +
                        "  WHERE b.taxsegment_name not LIKE 'within%' AND b.taxsegment_name NOT LIKE 'inter%' " +
                        "  AND b.taxsegment_name NOT LIKE 'oversea%'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetWithInState = new List<GetPmrothers_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetWithInState.Add(new GetPmrothers_list
                        {
                            vendor_companyname = wtin["vendor_companyname"].ToString(),
                            vendor_gid = wtin["vendor_gid"].ToString(),
                            contactperson_name = wtin["contactperson_name"].ToString(),
                            vendor_address = wtin["vendor_address"].ToString(),
                            vendor_code = wtin["vendor_code"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetPmrothers_list = GetWithInState;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetTotalVendorSegment(MdlPmrTaxSegment values)
        {
            try
            {
                msSQL = " SELECT a.vendor_companyname,a.vendor_gid, a.contactperson_name, " +
                    " case when b.taxsegment_name is null then 'Unassigned' else b.taxsegment_name end as taxsegment_name," +
                    "a.address_gid,concat(c.address1) as vendor_address, a.vendor_code, a.taxsegment_gid, b.taxsegment_name FROM acp_mst_tvendor a " +
                        " left join adm_mst_taddress c on a.address_gid = c.address_gid" +
                        " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetWithInState = new List<GetPmrtotal_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetWithInState.Add(new GetPmrtotal_list
                        {
                            vendor_companyname = wtin["vendor_companyname"].ToString(),
                            vendor_gid = wtin["vendor_gid"].ToString(),
                            contactperson_name = wtin["contactperson_name"].ToString(),
                            vendor_address = wtin["vendor_address"].ToString(),
                            vendor_code = wtin["vendor_code"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetPmrtotal_list = GetWithInState;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetUnassignSummary(MdlPmrTaxSegment values)
        {
            try
            {
                msSQL = " SELECT a.vendor_companyname, a.vendor_gid, a.contactperson_name, a.address_gid, CONCAT(c.address1) AS vendor_address, a.vendor_code, a.taxsegment_gid, CASE WHEN a.taxsegment_gid IS NULL THEN 'Unassigned' ELSE b.taxsegment_name END AS taxsegment_name FROM acp_mst_tvendor a " +
                        " LEFT JOIN adm_mst_taddress c ON a.address_gid = c.address_gid" +
                        " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid" +
                        " WHERE a.taxsegment_gid IS NULL";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetWithInState = new List<GetPmrInassignlist>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetWithInState.Add(new GetPmrInassignlist
                        {
                            vendor_companyname = wtin["vendor_companyname"].ToString(),
                            vendor_gid = wtin["vendor_gid"].ToString(),
                            contactperson_name = wtin["contactperson_name"].ToString(),
                            vendor_address = wtin["vendor_address"].ToString(),
                            vendor_code = wtin["vendor_code"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetPmrInassignlist = GetWithInState;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetTaxSegmentDropDown(string taxsegment_gid, MdlPmrTaxSegment values)
        {
            try
            {
                msSQL = " select taxsegment_name, taxsegment_code, taxsegment_gid from acp_mst_ttaxsegment" +
                    " where active_flag ='Y' and taxsegment_gid <> '" + taxsegment_gid + "'";
                ;

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetTaxSegmentDropDown = new List<GetPmrTaxSegmentDropDown_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetTaxSegmentDropDown.Add(new GetPmrTaxSegmentDropDown_list
                        {
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                        });
                        values.GetPmrTaxSegmentDropDown_list = GetTaxSegmentDropDown;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPmrPostTaxsegmentMoveOn(string user_gid, PmrPostTaxsegment values)
        {

            for (int i = 0; i < values.customer_gid1.ToArray().Length; i++)
            {
                msSQL = " update acp_mst_tvendor set " +
               "taxsegment_gid='" + values.taxsegment_gid + "' where vendor_gid='" + values.customer_gid1[i] + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Tax Segment assign to Vendor !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while occured assigning Vendor to tax segment";
                }
            }
        }
        public void DaGetVendorCount(MdlPmrTaxSegment values)
        {
            try
            {
                msSQL = "SELECT " +
                        " (SELECT COUNT(vendor_gid) FROM acp_mst_tvendor) as total_vendor, " +
                        " (SELECT COUNT(vendor_gid) FROM acp_mst_tvendor a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'within%') as within_vendor, " +
                        " (SELECT COUNT(vendor_gid) FROM acp_mst_tvendor a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'inter%') as interstate_vendor, " +
                        " (SELECT COUNT(vendor_gid) FROM acp_mst_tvendor a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'oversea%') as overseas_vendor, " +
                        " (SELECT COUNT(vendor_gid) FROM acp_mst_tvendor a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name NOT LIKE 'within%' AND b.taxsegment_name NOT LIKE 'inter%' AND b.taxsegment_name NOT LIKE 'oversea%') as other_vendor, " +
                        " (SELECT COUNT(vendor_gid) FROM acp_mst_tvendor WHERE taxsegment_gid IS NULL OR taxsegment_gid = '') as unassign_vendor ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegment_Vendorlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegment_Vendorlist
                        {
                            total_vendor = double.Parse(dt["total_vendor"].ToString()),
                            within_vendor = dt["within_vendor"].ToString(),
                            interstate_vendor = dt["interstate_vendor"].ToString(),
                            overseas_vendor = dt["overseas_vendor"].ToString(),
                            unassign_vendor = double.Parse(dt["unassign_vendor"].ToString()),
                            other_vendor = dt["other_vendor"].ToString(),
                            assign_vendor= (double.Parse(dt["total_vendor"].ToString())- double.Parse(dt["unassign_vendor"].ToString()))


                        });
                        values.TaxSegment_Vendorlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaDeleteTaxSegmentSummary(string taxsegment_gid, MdlPmrTaxSegment values)
        {
            try
            {

                msSQL = "select vendor_gid from acp_mst_tvendor where taxsegment_gid = '" + taxsegment_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Close ();
                    values.status = false;
                    values.message = "This tax segment is mapped to the Vendor!";
                    return;

                }
                msSQL = "select taxsegment_gid from acp_mst_ttax where taxsegment_gid = '" + taxsegment_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Close ();
                    values.status = false;
                    values.message = "Tax segment already mapped!";
                    return;

                }
                else
                {


                    msSQL = "  delete from acp_mst_ttaxsegment where taxsegment_gid='" + taxsegment_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Tax Segment Deleted Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Tax Segment";
                        return;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Tax Segment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}

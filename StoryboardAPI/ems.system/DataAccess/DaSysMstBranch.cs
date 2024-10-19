using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web;
using ems.system.Models;
using ems.utilities.Functions;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.system.DataAccess
{
    public class DaSysMstBranch
    {
        DataTable dt_datatable;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsbranch_code, lsbranch_name, lsbranch_prefix, lsbranch_name_edit, lsbranch_prefix_edit;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        int mnResult6;
        public void DaBranchSummary(MdlSysMstBranch values)
        {
            try
            {
                msSQL = " select a.branch_gid, a.branch_code, a.branch_prefix, a.branch_name, a.branch_logo_path, a.status as branch_status, " +
                       " case when a.status = 'Y' then 'Active' when a.status IS NULL OR a.status = '' OR a.status = 'N' then 'InActive' END AS status," +
                        " concat(b.user_firstname,' ',b.user_lastname) as created_by, " +
                        " date_format(a.created_date,'%d-%b-%Y') as created_date " +
                        " from hrm_mst_tbranch a " +
                        " left join adm_mst_tuser b on b.user_gid = a.created_by " +
                        " order by a.branch_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<branch_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branch_list1
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_code = dt["branch_code"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_logo_path = dt["branch_logo_path"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            BranchStatus = dt["status"].ToString(),
                        });
                        values.branch_list1 = getModuleList;
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
        public void DaBranchSummarydetail(branch_list1 values, string user_gid)
        {
            try
            {
                string Branch_address_add = values.Branch_address_add.Replace("'", "\\'");

                msSQL = " update hrm_mst_tbranch set" +
                        " address1='" + Branch_address_add + "', " +
                        " city='" + values.City.Replace("'", "\\'") + "'," +
                        " state='" + values.State.Replace("'", "\\'") + "'," +
                        " postal_code='" + values.Postal_code + "', " +
                        " contact_number='" + values.Phone_no_add + "'," +
                        " email_id='" + values.Email_address_add + "', " +
                        " gst_no='" + values.GST_no_add + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                        " where branch_gid='" + values.branch_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Branch details updated successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while updating branch details";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostBranch(string user_gid, branch_list1 values)
        {
            try
            {
                string branch_name = values.branch_name.Replace("'", "\\'");

                //msSQL = " select branch_code from hrm_mst_tbranch where branch_code = '" + values.branch_code + "' ";
                //DataTable dt_datatable4 = objdbconn.GetDataTable(msSQL);

                //if (dt_datatable4.Rows.Count > 0)
                //{
                //    values.status = false;
                //    values.message = "Branch code already exist";
                //    return;
                //}

                msSQL = "SELECT branch_code FROM hrm_mst_tbranch " +
                       "WHERE LOWER(branch_code) = LOWER('" + values.branch_code + "') " +
                       "OR UPPER(branch_code) = UPPER('" + values.branch_code + "')";

                DataTable dt_datatable4 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable4.Rows.Count > 0)

                {

                    values.status = false;

                    values.message = "Branch Code already exist";

                    return;

                }


                msSQL = " select branch_name from hrm_mst_tbranch " +
                "WHERE LOWER(branch_name) = LOWER('" + branch_name + "') " +
                "OR UPPER(branch_name) = UPPER('" + branch_name + "')";
                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Branch name already exist";
                    return;
                }

                msSQL = " select branch_prefix from hrm_mst_tbranch " +
                "WHERE LOWER(branch_prefix) = LOWER('" + values.branch_prefix.Replace("'", "\\'") + "') " +
               "OR UPPER(branch_prefix) = UPPER('" + values.branch_prefix.Replace("'", "\\'") + "')";
                DataTable dt_datatable3 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable3.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Branch prefix already exist";
                    return;
                }


                msGetGid1 = objcmnfunctions.GetMasterGID("HBHM");
                msSQL = " insert into hrm_mst_tbranch(" +
                    " branch_gid," +
                    " branch_code," +
                    " branch_name," +
                    " branch_prefix," +
                    " created_by, " +
                    " created_date)" +
                    " values(" +
                    " '" + msGetGid1 + "'," +
                    " '" + values.branch_code + "'," +
                    " '" + branch_name + "'," +
                    " '" + values.branch_prefix.Replace("'", "\\'") + "',";
                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Branch Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Branch";
                }


            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdatedbranchlogo(HttpRequest httpRequest, result values, string user_gid)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;
            string lspath;
            string msGetGid;

            msSQL = " SELECT  a.company_code  FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            string branch_gid = httpRequest.Form[0];
            string branch_code = httpRequest.Form[1];
            string Branch_address = httpRequest.Form[2];
            string City = httpRequest.Form[3];
            string State = httpRequest.Form[4];
            string Postal_code = httpRequest.Form[5];
            string Email_address = httpRequest.Form[6];
            string Phone_no = httpRequest.Form[7];
            string GST_no = httpRequest.Form[8];

            MemoryStream ms = new MemoryStream();
            lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "System" + "/" + "Companydetails" + "/";

            {
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);
            }
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;

                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "System" + "/" + "Companydetails" + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ms.Close();
                        string lspath1 = "erp_documents" + "/" + lscompany_code + "/" + "System" + "/" + "Companydetails" + "/";
                        string final_path = lspath1 + msdocument_gid + FileExtension;



                        msSQL = "update hrm_mst_tbranch set " +
                            " branch_code='" + branch_code + "'," +
                            " address1='" + Branch_address.Replace("'", "\\'") + "'," +
                            " city = '" + City.Replace("'", "\\'") + "'," +
                            " state = '" + State.Replace("'", "\\'") + "'," +
                            " postal_code = '" + Postal_code + "'," +
                            " contact_number = '" + Phone_no + "'," +
                            " email_id = '" + Email_address + "'," +
                            " gst_no = '" + GST_no + "'," +
                            " branch_logo_path='" + final_path + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                            " where branch_gid='" + branch_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Branch Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding branch";
                }
            }

            catch (Exception ex)
            {
                values.message = ex.ToString();
            }
            //return true;

        }
        //public void DagetUpdatedBranch(string user_gid, branch_list1 values)
        //{
        //    try
        //    {
        //        msSQL = " select branch_name from hrm_mst_tbranch where branch_name = '" + values.branch_name_edit + "' ";
        //        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //        if (objOdbcDataReader.HasRows)
        //        {
        //            lsbranch_name = objOdbcDataReader["branch_name"].ToString();
        //        }

        //        msSQL = " select branch_prefix from hrm_mst_tbranch where branch_prefix = '" + values.branch_prefix_edit + "' ";
        //        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //        if (objOdbcDataReader.HasRows)
        //        {
        //            lsbranch_prefix = objOdbcDataReader["branch_prefix"].ToString();
        //        }

        //            if (lsbranch_name != values.branch_name_edit)
        //            {
        //                if (lsbranch_prefix != values.branch_prefix_edit)
        //                {
        //                    msSQL = " update  hrm_mst_tbranch set " +
        //                            " branch_code = '" + values.branch_code_edit + "'," +
        //                            " branch_name = '" + values.branch_name_edit + "'," +
        //                            " branch_prefix = '" + values.branch_prefix_edit + "'," +
        //                            " updated_by = '" + user_gid + "'," +
        //                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where branch_gid='" + values.branch_gid + "'  ";
        //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                    if (mnResult != 0)
        //                    {
        //                        values.status = true;
        //                        values.message = "Branch Updated Successfully";
        //                    }
        //                    else
        //                    {
        //                        values.status = false;
        //                        values.message = "Error While Updating Branch";
        //                    }
        //                }
        //                else
        //                {
        //                    values.status = false;
        //                    values.message = "Branch Prefix Already Exists !!";
        //                }
        //            }
        //            else
        //            {
        //                values.status = false;
        //                values.message = "Branch Name Already Exists !!";
        //            }                           
        //    }
        //    catch (Exception ex)
        //    {
        //        values.status = false;

        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        //         "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }
        //}

        public void DagetUpdatedBranch(string user_gid, branch_list1 values)
        {
            try
            {
                //msSQL = " select branch_prefix from hrm_mst_tbranch where branch_name = '" + values.branch_prefix_edit + "' ";
                //DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable1.Rows.Count > 0)
                //{
                //    values.status = false;
                //    values.message = "Branch prefix already exists";
                //    return;
                //}

                //msSQL = " select branch_name from hrm_mst_tbranch where branch_name = '" + values.branch_name_edit + "' ";
                //DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable2.Rows.Count > 0)
                //{
                //    values.status = false;
                //    values.message = "Branch name already exists";
                //    return;
                //}
                string branch_name_edit = values.branch_name_edit.Replace("'", "\\'");


                msSQL = " SELECT branch_prefix  FROM " +
                        " hrm_mst_tbranch WHERE branch_prefix = '" + values.branch_prefix_edit.Replace("'", "\\'") + "' and   branch_gid !='" + values.branch_gid + "' ";

                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Branch Prefix already exist";
                    return;
                }


                msSQL = " SELECT branch_name  FROM " +
                       " hrm_mst_tbranch WHERE branch_name = '" + branch_name_edit + "' and   branch_gid !='" + values.branch_gid + "' ";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Branch Name already exist";
                    return;
                }

                msSQL = " update  hrm_mst_tbranch set " +
                        " branch_code = '" + values.branch_code_edit + "'," +
                        " branch_name = '" + branch_name_edit + "'," +
                        " branch_prefix = '" + values.branch_prefix_edit.Replace("'", "\\'") + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where branch_gid='" + values.branch_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Branch Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Branch";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaBranchActive(string branch_gid, MdlSysMstBranch objresult)
        {
            try
            {
                msSQL = " update hrm_mst_tbranch set" +
                       " status='Y'" +
                       " where branch_gid = '" + branch_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    objresult.status = true;
                    objresult.message = "Branch Active Successfully";

                }
                else
                {

                    objresult.status = false;
                    objresult.message = "Error while Branch activeted";

                }
            }
            catch (Exception ex)
            {
                objresult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaBranchInactive(string branch_gid, MdlSysMstBranch objresult)
        {
            try
            {
                msSQL = " update hrm_mst_tbranch set" +
                        " status='N'" +
                        " where branch_gid = '" + branch_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    objresult.status = true;
                    objresult.message = "branch Inactive successfully";

                }

                else
                {

                    objresult.status = false;
                    objresult.message = "Error while Branch Inactivated";

                }
            }

            catch (Exception ex)
            {
                objresult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

            public void DaDeleteBranch(string branch_gid, branch_list1 values)
            {
                try
                {

                    bool employeelist = false, employeetypedtl = false, invoice = false, salesorder = false;

                    msSQL = " select branch_gid from hrm_mst_temployee " +
                          " where branch_gid = '" + branch_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        employeelist = true;

                    }
                    msSQL = " select branch_gid from hrm_trn_temployeetypedtl " +
                        " where branch_gid = '" + branch_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        employeetypedtl = true;

                    }
                    msSQL = " select branch_gid from rbl_trn_tinvoice " +
                            " where branch_gid = '" + branch_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        invoice = true;

                    }
                    msSQL = " select branch_gid from smr_trn_tsalesorder " +
                            " where branch_gid = '" + branch_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        salesorder = true;

                    }

                    if (!(employeelist || employeetypedtl || invoice || salesorder))
                    {
                        msSQL = "  delete from hrm_mst_tbranch where branch_gid='" + branch_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Branch Deleted Successfully";
                        }
                        else

                        {
                            values.status = false;
                            values.message = "Error While Deleting Branch";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Cannot delete branch  since it is involved in transactions!";
                    }

                }
                catch (Exception ex)
                {
                    values.status = false;

                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                     "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }

            }
            public void DaPopupBranch(string branch_gid, MdlSysMstBranch values)
            {
                try
                {
                    msSQL = " select branch_code, address1,city,postal_code,contact_number,email_id,gst_no,state,branch_logo_path from hrm_mst_tbranch WHERE branch_gid='" + branch_gid + "'; ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<branch_list1>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new branch_list1
                            {
                                Branch_address_add = dt["address1"].ToString(),
                                branch_code_add = dt["branch_code"].ToString(),
                                city = dt["city"].ToString(),
                                postal_code = dt["postal_code"].ToString(),
                                Phone_no_add = dt["contact_number"].ToString(),
                                Email_address_add = dt["email_id"].ToString(),
                                GST_no_add = dt["gst_no"].ToString(),
                                State = dt["state"].ToString(),
                                branch_logo_path = dt["branch_logo_path"].ToString(),

                            });
                            values.branch_list1 = getModuleList;
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
        }
    }
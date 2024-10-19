using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using System.Globalization;
using Bytescout.Spreadsheet;
using System.Configuration;
using System.IO;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Drawing;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Security.Policy;
using Bytescout.Spreadsheet.COM;
using Bytescout.Spreadsheet.MSODrawing;
using System.IO.Packaging;

namespace ems.hrm.Controllers
{
    public class DaHrmRptEmployeeFormA
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, combine_path, doc, lspath1, lspath2, file_name, file_name1, lsentity_code, final_path, final_path1, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetGid2, msGetPrivilege_gid, msGetModule2employee_gid, lsassetref_no, lsasset_name;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        int mnResult6;
        string EmployeePhoto, EmployeeSign, msdocument_gid, FileName, FileName1, FileExtension, FileName_path, FileName_path1;
        string domain = string.Empty;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        public void DaGetEmployeeFormA(MdlHrmRptEmployeeFormA values)
        {
            try
            {
               msSQL = " Select distinct a.user_gid, case when y.salarycomponent_name ='LWF'  then y.salarycomponent_amount else 'No LWF' end as LWF ,c.identity_no as Aadhar," +
                       " concat(b.entity_prefix,' / ',e.branch_prefix) as entity_name, " +
                       " a.user_code, a.user_firstname, a.user_lastname, a.category_address, a.employment_type, a.servicebookno, a.reason_exit, a.mark_identification, date_format(c.employee_joiningdate,'%d-%m-%Y') as employee_joiningdate, c.employee_gender, c.father_name, date_format(c.employee_dob,'%d-%m-%Y') as employee_dob, " +
                       " c.nationality, c.employee_qualification, c.employee_mobileno, concat(j.address1,' ',j.address2,' / ',j.city,' / ',j.state,' / ',k.country_name,' / ',j.postal_code) as emp_address, c.designation_gid, " +
                       " d.designation_name, c.employee_gid, e.branch_name, c.employee_level, c.useraccess, c.uan_no, c.pan_no, c.esi_no, c.ac_no, c.bank, c.bank_code, c.employee_photo, c.employee_sign, c.remarks, date_format(c.exit_date,'%d-%m-%Y') as exit_date, CASE WHEN a.user_status = 'Y' THEN 'Active' WHEN a.user_status = 'N' THEN 'Inactive' END as user_status, " +
                       " c.department_gid, c.branch_gid, g.department_name, date_format(c.exit_date,'%d-%m-%Y') as exit_date FROM adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                       " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid left join hrm_mst_tdepartment g on g.department_gid = c.department_gid left join adm_mst_taddress j on c.employee_gid = j.parent_gid " +
                       " left join adm_mst_tcountry k on j.country_gid = k.country_gid left join hrm_trn_temployeedtl m on m.permanentaddress_gid = j.address_gid left join adm_mst_tentity b on b.entity_gid = c.entity_gid " +
                       " left join pay_trn_temployee2salarygradetemplate z on c.employee_gid = z.employee_gid " +
                       " left join pay_trn_temployee2salarygradetemplatedtl y on z.employee2salarygradetemplate_gid = y.employee2salarygradetemplate_gid " +
                       " group by c.employee_gid order by c.employee_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employee_listform>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        domain = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");
                        string relativeLogoPath = dt["employee_photo"].ToString();

                        string combinedPath = domain + "/" + relativeLogoPath;

                        domain = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");
                        string relativeLogoPath1 = dt["employee_sign"].ToString();

                        string combinedPath1 = domain + "/" + relativeLogoPath1;

                        getModuleList.Add(new employee_listform
                        {
                            user_gid = dt["user_gid"].ToString(),
                            useraccess = dt["useraccess"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            //user_code = dt["user_code"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            user_lastname = dt["user_lastname"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            father_name = dt["father_name"].ToString(),
                            employee_dob = dt["employee_dob"].ToString(),
                            nationality = dt["nationality"].ToString(),
                            employee_qualification = dt["employee_qualification"].ToString(),
                            uan_no = dt["uan_no"].ToString(),
                            pan_no = dt["pan_no"].ToString(),
                            esi_no = dt["esi_no"].ToString(),
                            ac_no = dt["ac_no"].ToString(),
                            bank = dt["bank"].ToString(),
                            bank_code = dt["bank_code"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            emp_address = dt["emp_address"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            employee_level = dt["employee_level"].ToString(),
                            exit_date = dt["exit_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            employee_photo = combinedPath,
                            employee_sign = combinedPath1,
                            category_address = dt["category_address"].ToString(),
                            employment_type = dt["employment_type"].ToString(),
                            LWF = dt["LWF"].ToString(),
                            Aadhar = dt["Aadhar"].ToString(),
                            servicebookno = dt["servicebookno"].ToString(),
                            reason_exit = dt["reason_exit"].ToString(),
                            mark_identification = dt["mark_identification"].ToString(),
                        });
                        values.employee_listform = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }



        //public void DaGetEmployeeImage(HttpRequest httpRequest, MdlHrmRptEmployeeFormA objResult) 
        //{
        //    HttpFileCollection httpFileCollection;
        //    string lsfilepath = string.Empty;
        //    string lsdocument_gid = string.Empty;
        //    MemoryStream ms_stream = new MemoryStream();
        //    string document_gid = string.Empty;
        //    string lscompany_code = string.Empty;
        //    HttpPostedFile httpPostedFile;

        //    string lspath;
        //    string msGetGid;
        //    msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
        //    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
        //    //string product_name = httpRequest.Form[0];
        //    string Employee_photo_image = httpRequest.Form["Employee_photo"];
        //    string Employee_sign_image = httpRequest.Form["Employee_sign"];
        //    string user_gid1 = httpRequest.Form["user_gid"];

        //    MemoryStream ms = new MemoryStream();

        //    try
        //    {
        //        //msSQL = "select product_gid from pmr_mst_tproduct where product_name = '" + product_name + "'";
        //        //string lsProductgid = objdbconn.GetExecuteScalar(msSQL);

        //        if (httpRequest.Files.Count > 0)
        //        {
        //            string lsfirstdocument_filepath = string.Empty;
        //            httpFileCollection = httpRequest.Files;
        //            for (int i = 0; i < httpFileCollection.Count; i++)
        //            {
        //                 msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
        //               
        //                httpPostedFile = httpFileCollection[i];
        //               
        //                 FileName = httpPostedFile.FileName;
        //                
        //                string lsfile_gid = msdocument_gid;
        //             
        //                string lscompany_document_flag = string.Empty;
        //                FileExtension = Path.GetExtension(FileName).ToLower();
        //                lsfile_gid = lsfile_gid + FileName;
        //                
        //                Stream ls_readStream;
        //                ls_readStream = httpPostedFile.InputStream;
        //                ls_readStream.CopyTo(ms);

        //                bool status1, status2;


        //                if ("EmployeePhoto" == FileName)
        //                {
        //                    status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "HR/Employee/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
        //                    final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "HR/Employee/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
        //                    msSQL = "UPDATE hrm_mst_temployee SET " +
        //                        "employee_photo = '" +
        //                        ConfigurationManager.AppSettings["blob_imagepath1"] +
        //                       final_path +
        //                       msdocument_gid +
        //                       FileExtension +
        //                       ConfigurationManager.AppSettings["blob_imagepath2"] +
        //                       "&" + ConfigurationManager.AppSettings["blob_imagepath3"] +
        //                       "&" + ConfigurationManager.AppSettings["blob_imagepath4"] +
        //                       "&" + ConfigurationManager.AppSettings["blob_imagepath5"] +
        //                       "&" + ConfigurationManager.AppSettings["blob_imagepath6"] +
        //                       "&" + ConfigurationManager.AppSettings["blob_imagepath7"] +
        //                       "&" + ConfigurationManager.AppSettings["blob_imagepath8"] + "'" +
        //                        "WHERE user_gid = '" + user_gid1 + "'";
        //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                }
        //                else if("EmployeeSign" == FileName)
        //                {
        //                    status2 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "HR/EmployeeSignature/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
        //                    final_path1 = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "HR/EmployeeSignature/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
        //                    msSQL = "UPDATE hrm_mst_temployee SET " +
        //                       "employee_sign = '" +
        //                       ConfigurationManager.AppSettings["blob_imagepath1"] +
        //                      final_path1 +
        //                      msdocument_gid +
        //                      FileExtension +
        //                      ConfigurationManager.AppSettings["blob_imagepath2"] +
        //                      "&" + ConfigurationManager.AppSettings["blob_imagepath3"] +
        //                      "&" + ConfigurationManager.AppSettings["blob_imagepath4"] +
        //                      "&" + ConfigurationManager.AppSettings["blob_imagepath5"] +
        //                      "&" + ConfigurationManager.AppSettings["blob_imagepath6"] +
        //                      "&" + ConfigurationManager.AppSettings["blob_imagepath7"] +
        //                      "&" + ConfigurationManager.AppSettings["blob_imagepath8"] + "'" +
        //                       "WHERE user_gid = '" + user_gid1 + "'";
        //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                }
        //                else { }                     
        //            }                                  


        //        }


        //        if (mnResult != 0)
        //        {
        //            objResult.status = true;
        //            objResult.message = "Employee Image and Signature Added Successfully !!";
        //        }
        //        else
        //        {
        //            objResult.status = false;
        //            objResult.message = "Error While Adding Employee Image !!";
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        objResult.message = ex.ToString();
        //    }
        //    //return true;

        //}

        public void DaGetEmployeeImage(HttpRequest httpRequest, MdlHrmRptEmployeeFormA objResult)
        {

            
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            
            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            string Employee_photo_image = httpRequest.Form["Employee_photo"];
            string Employee_sign_image = httpRequest.Form["Employee_sign"];
            string user_gid1 = httpRequest.Form["user_gid"];

            MemoryStream ms = new MemoryStream();
            lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "HR" + "/" + "Employeedetails" + "/";

            {
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);
            }

            try
            {

                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    HttpPostedFile httpPostedFile = httpRequest.Files[i];
                    FileExtension = Path.GetExtension(httpPostedFile.FileName).ToLower();
                    file_name = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
                    msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                    file_name1 = file_name + "-" + objcmnfunctions.ExtractLast4Digits(file_name + msdocument_gid);

                    combine_path = Path.Combine(lspath, file_name1 + FileExtension);

                    if (!Directory.Exists(Path.GetDirectoryName(combine_path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(combine_path));
                    }

                    using (Stream fileStream = httpPostedFile.InputStream)
                    {
                        using (FileStream fs = System.IO.File.Create(combine_path))
                        {
                            fileStream.CopyTo(fs);
                        }
                    }
                    if ("EmployeePhoto" == file_name)
                    {
                        lspath1 = "erp_documents/" + lscompany_code + "/HR/Employeedetails/" + file_name1 + FileExtension;
                        msSQL = " UPDATE hrm_mst_temployee SET " +
                                         " employee_photo = '" + lspath1 + "'" +
                                         " WHERE user_gid = '" + user_gid1 + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else if ("EmployeeSign" == file_name)
                    {
                        lspath2 = "erp_documents/" + lscompany_code + "/HR/Employeedetails/" + file_name1 + FileExtension;
                        msSQL = " UPDATE hrm_mst_temployee SET " +
                                                 " employee_sign = '" + lspath2 + "'" +
                                                 " WHERE user_gid = '" + user_gid1 + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                   
                }


                if (mnResult != 0)
                {
                    objResult.status = true;
                    objResult.message = "Employee Image and Signature Added Successfully !!";
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Adding Employee Image !!";
                }

            }
            catch (Exception ex)
            {
                objResult.message = ex.ToString();
            }
            
        }


        public Dictionary<string, object> DaExportFormAExcel(MdlHrmRptEmployeeFormA values)
        {
            var ls_response = new Dictionary<string, object>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            msSQL = " SELECT a.company_name FROM adm_mst_tcompany a ";
            string lscompany_name = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT a.contact_person FROM adm_mst_tcompany a ";
            string lscontact_person = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);

            Spreadsheet document = new Spreadsheet();
           
            Worksheet sheet = document.Workbook.Worksheets.Add("ExcelSheet1");
            //using (ExcelPackage package = new ExcelPackage())
            //{
            //    ExcelWorksheet sheet1 = package.Workbook.Worksheets.Add("ExcelSheet1");

            //    sheet1.Cells[1, 1].Value = "Name";
            //    sheet1.Cells[1, 2].Value = "Employee ID";
            //    var imagePath = "";
            //    using (Bitmap image = new Bitmap(imagePath))
            //{
            //    var picture = sheet1.Drawings.AddPicture("EmployeePhoto",imagePath);

            //    // Set the position in the "Photo" column
            //    picture.SetPosition(5 - 1, 0,  - 1, 0); // Row and column are 0-based

            //    // Optional: Resize the image to fit within the cell
            //    picture.SetSize(50, 50); // Set the size of the image in pixels
            //}
            //}
            try
            {
                sheet.Range("A11:AD11").Font = new Font("arial", 10, FontStyle.Bold);
                sheet.Range("A1:C1").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("A2").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("A4").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("A5").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Range("A11:AD11").BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                sheet.Range("A11:AD11").TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                sheet.Range("A11:AD11").RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                sheet.Range("A11:AD11").LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                sheet.AutoFitRows();
                
                sheet.Columns[0].Width = 130;
                sheet.Columns[1].Width = 170;
                sheet.Columns[2].Width = 230;
                sheet.Columns[3].Width = 70;
                sheet.Columns[4].Width = 170;
                sheet.Columns[5].Width = 170;
                sheet.Columns[6].Width = 230;
                sheet.Columns[7].Width = 130;
                sheet.Columns[8].Width = 130;
                sheet.Columns[9].Width = 230;
                sheet.Columns[10].Width = 230;
                sheet.Columns[11].Width = 170;
                sheet.Columns[12].Width = 130;
                sheet.Columns[13].Width = 170;
                sheet.Columns[14].Width = 230;
                sheet.Columns[15].Width = 170;
                sheet.Columns[16].Width = 130;
                sheet.Columns[17].Width = 170;
                sheet.Columns[18].Width = 230;
                sheet.Columns[19].Width = 230;
                sheet.Columns[20].Width = 230;
                sheet.Columns[21].Width = 900;
                sheet.Columns[22].Width = 900;
                sheet.Columns[23].Width = 170;
                sheet.Columns[24].Width = 130;
                sheet.Columns[25].Width = 170;
                sheet.Columns[26].Width = 230;
                sheet.Columns[27].Width = 150;
                sheet.Columns[28].Width = 300;
                sheet.Columns[29].Width = 200;
                sheet.Columns[30].Width = 300;
                sheet.ViewOptions.ShowGridLines = false;
                sheet.Range("A1:C1").Merge();
                sheet.Cell(0, 0).AlignmentHorizontal = Bytescout.Spreadsheet.Constants.AlignmentHorizontal.Centered;
                sheet.Range("A2:C2").Merge();
                sheet.Cell(1, 0).AlignmentHorizontal = Bytescout.Spreadsheet.Constants.AlignmentHorizontal.Centered;

                //data inserting in respective cells
                sheet.Cell(0, 0).Value = "FORM A";
                sheet.Cell(1, 0).Value = "EMPLOYEE REGISTER";
                sheet.Cell("A4").Value = "[Part-A: For all Establishments]";
                sheet.Cell("A5").Value = "Name of Establishment :" + lscompany_name;
                sheet.Cell("A7").Value = "Name of Owner :" + lscontact_person;
                sheet.Cell("A8").Value = "LIN :";
                sheet.Cell("A11").Value = "Employee Code";
                sheet.Cell("B11").Value = "Name";
                sheet.Cell("C11").Value = "Surname";
                sheet.Cell("D11").Value = "Gender";
                sheet.Cell("E11").Value = "Father's / Spouse Name";
                sheet.Cell("F11").Value = "Date of Birth";
                sheet.Cell("G11").Value = "Nationality";
                sheet.Cell("H11").Value = "Education Level";
                sheet.Cell("I11").Value = "Date of Joining";
                sheet.Cell("J11").Value = "Designation";
                sheet.Cell("K11").Value = "Category Address *(HS/S/SS/US)";
                sheet.Cell("L11").Value = "Type of Employment";
                sheet.Cell("M11").Value = "Mobile";
                sheet.Cell("N11").Value = "UAN";
                sheet.Cell("O11").Value = "PAN";
                sheet.Cell("P11").Value = "ESIC IP";
                sheet.Cell("Q11").Value = "LWF";
                sheet.Cell("R11").Value = "AADHAAR";
                sheet.Cell("S11").Value = "Bank A/c Number";
                sheet.Cell("T11").Value = "Bank";
                sheet.Cell("U11").Value = "Branch (IFSC)";
                sheet.Cell("V11").Value = "Present Address";
                sheet.Cell("W11").Value = "Permanent Address";
                sheet.Cell("X11").Value = "Service Book No.";
                sheet.Cell("Y11").Value = "Date of Exit";
                sheet.Cell("Z11").Value = "Reason for Exit";
                sheet.Cell("AA11").Value = "Mark of Identification";
                sheet.Cell("AB11").Value = "Photo";
                sheet.Cell("AC11").Value = "Signature";
                sheet.Cell("AD11").Value = "Remarks";

                msSQL = " Select distinct a.user_gid, case when y.salarycomponent_name ='LWF'  then y.salarycomponent_amount else 'No LWF' end as LWF ,c.identity_no as Aadhar," +
                           " concat(b.entity_prefix,' / ',e.branch_prefix) as entity_name, " +
                           " a.user_code, a.user_firstname, a.user_lastname, a.category_address, a.employment_type, a.servicebookno, a.reason_exit, a.mark_identification, date_format(c.employee_joiningdate,'%d-%m-%Y') as employee_joiningdate, c.employee_gender, c.father_name, date_format(c.employee_dob,'%d-%m-%Y') as employee_dob, " +
                           " c.nationality, c.employee_qualification, c.employee_mobileno, concat(j.address1,' ',j.address2,' / ',j.city,' / ',j.state,' / ',k.country_name,' / ',j.postal_code) as emp_address, c.designation_gid, " +
                           " d.designation_name, c.employee_gid, e.branch_name, c.employee_level, c.useraccess, c.uan_no, c.pan_no, c.esi_no, c.ac_no, c.bank, c.bank_code, c.employee_photo, c.employee_sign, c.remarks, date_format(c.exit_date,'%d-%m-%Y') as exit_date, CASE WHEN a.user_status = 'Y' THEN 'Active' WHEN a.user_status = 'N' THEN 'Inactive' END as user_status, " +
                           " c.department_gid, c.branch_gid, g.department_name, date_format(c.exit_date,'%d-%m-%Y') as exit_date FROM adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                           " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid left join hrm_mst_tdepartment g on g.department_gid = c.department_gid left join adm_mst_taddress j on c.employee_gid = j.parent_gid " +
                           " left join adm_mst_tcountry k on j.country_gid = k.country_gid left join hrm_trn_temployeedtl m on m.permanentaddress_gid = j.address_gid left join adm_mst_tentity b on b.entity_gid = c.entity_gid " +
                           " left join pay_trn_temployee2salarygradetemplate z on c.employee_gid = z.employee_gid " +
                           " left join pay_trn_temployee2salarygradetemplatedtl y on z.employee2salarygradetemplate_gid = y.employee2salarygradetemplate_gid " +
                           " group by c.employee_gid order by c.employee_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                //table data inserting based on rows from db
                int cellrowindex;
                if (dt_datatable.Rows.Count != 0)
                {
                    cellrowindex = 11;

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        int cellcloumnindex = 0;
                        uint newRowHeight = 70;

                        sheet.Cell(cellrowindex, 0 + cellcloumnindex).Value = dt["user_code"].ToString();
                        sheet.Cell(cellrowindex, 0 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 0 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 0 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 0 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 1 + cellcloumnindex).Value = dt["user_firstname"].ToString();
                        sheet.Cell(cellrowindex, 1 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 1 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 1 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 1 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 2 + cellcloumnindex).Value = dt["user_lastname"].ToString();
                        sheet.Cell(cellrowindex, 2 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 2 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 2 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 2 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 3 + cellcloumnindex).Value = dt["employee_gender"].ToString();
                        sheet.Cell(cellrowindex, 3 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 3 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 3 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 3 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 4 + cellcloumnindex).Value = dt["father_name"].ToString();
                        sheet.Cell(cellrowindex, 4 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 4 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 4 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 4 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 5 + cellcloumnindex).Value = dt["employee_dob"].ToString();
                        sheet.Cell(cellrowindex, 5 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 5 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 5 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 5 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 6 + cellcloumnindex).Value = dt["nationality"].ToString();
                        sheet.Cell(cellrowindex, 6 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 6 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 6 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 6 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 7 + cellcloumnindex).Value = dt["employee_qualification"].ToString();
                        sheet.Cell(cellrowindex, 7 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 7 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 7 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 7 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 8 + cellcloumnindex).Value = dt["employee_joiningdate"].ToString();
                        sheet.Cell(cellrowindex, 8 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 8 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 8 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 8 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 9 + cellcloumnindex).Value = dt["designation_name"].ToString();
                        sheet.Cell(cellrowindex, 9 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 9 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 9 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 9 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 10 + cellcloumnindex).Value = dt["category_address"].ToString();
                        sheet.Cell(cellrowindex, 10 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 10 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 10 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 10 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 11 + cellcloumnindex).Value = dt["employment_type"].ToString();
                        sheet.Cell(cellrowindex, 11 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 11 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 11 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 11 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 12 + cellcloumnindex).Value = dt["employee_mobileno"].ToString();
                        sheet.Cell(cellrowindex, 12 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 12 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 12 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 12 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;


                        sheet.Cell(cellrowindex, 13 + cellcloumnindex).Value = dt["uan_no"].ToString();
                        sheet.Cell(cellrowindex, 13 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 13 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 13 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 13 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 14 + cellcloumnindex).Value = dt["pan_no"].ToString();
                        sheet.Cell(cellrowindex, 14 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 14 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 14 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 14 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 15 + cellcloumnindex).Value = dt["esi_no"].ToString();
                        sheet.Cell(cellrowindex, 15 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 15 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 15 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 15 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 16 + cellcloumnindex).Value = dt["LWF"].ToString();
                        sheet.Cell(cellrowindex, 16 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 16 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 16 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 16 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 17 + cellcloumnindex).Value = dt["Aadhar"].ToString();
                        sheet.Cell(cellrowindex, 17 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 17 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 17 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 17 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 18 + cellcloumnindex).Value = dt["ac_no"].ToString();
                        sheet.Cell(cellrowindex, 18 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 18 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 18 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 18 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 19 + cellcloumnindex).Value = dt["bank"].ToString();
                        sheet.Cell(cellrowindex, 19 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 19 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 19 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 19 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 20 + cellcloumnindex).Value = dt["bank_code"].ToString();
                        sheet.Cell(cellrowindex, 20 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 20 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 20 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 20 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 21 + cellcloumnindex).Value = dt["emp_address"].ToString();
                        sheet.Cell(cellrowindex, 21 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 21 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 21 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 21 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 22 + cellcloumnindex).Value = dt["emp_address"].ToString();
                        sheet.Cell(cellrowindex, 22 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 22 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 22 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 22 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 23 + cellcloumnindex).Value = dt["servicebookno"].ToString();
                        sheet.Cell(cellrowindex, 23 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 23 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 23 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 23 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 24 + cellcloumnindex).Value = dt["exit_date"].ToString();
                        sheet.Cell(cellrowindex, 24 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 24 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 24 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 24 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 25 + cellcloumnindex).Value = dt["reason_exit"].ToString();
                        sheet.Cell(cellrowindex, 25 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 25 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 25 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 25 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        sheet.Cell(cellrowindex, 26 + cellcloumnindex).Value = dt["mark_identification"].ToString();
                        sheet.Cell(cellrowindex, 26 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 26 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 26 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 26 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        // Assuming cellRowIndex and cellColumnIndex are the indices of the cell where you want to insert the image
                        //int cellRowIndex = 11; // 0-based index, so AB12 is 11 
                        //int cellColumnIndex = 27; // Column AB is the 28th column, so 27 in 0-based index
                        string lspath1 = HttpContext.Current.Server.MapPath("../../../" + dt["employee_photo"].ToString()); // Assuming this is the path to the image file

                        if (!string.IsNullOrEmpty(dt["employee_photo"].ToString()))
                        {

                            sheet.Cell(cellrowindex, 27 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                            sheet.Cell(cellrowindex, 27 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                            sheet.Cell(cellrowindex, 27 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                            sheet.Cell(cellrowindex, 27 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                            IPictureShape picture1 = sheet.Pictures.Add(cellrowindex, 27 + cellcloumnindex, lspath1);

                            // Set the desired image height (for example, 100 units)
                            int desiredImageHeight = 50;

                            // Get the dimensions of the target cell (AB12)
                            double cellWidth = sheet.Columns[cellrowindex].Width;
                            double cellHeight = sheet.Rows[cellcloumnindex].Height;

                            // Set the picture size
                            picture1.Height = desiredImageHeight;

                            // Optionally adjust the width to maintain the aspect ratio
                            double aspectRatio = (double)picture1.Width / picture1.Height;
                            picture1.Width = (int)(desiredImageHeight * aspectRatio);
                        }

                        // Assuming cellRowIndex and cellColumnIndex are the indices of the cell where you want to insert the image
                        //int cellRowIndex1 = 11; // 0-based index for row 12
                        //int cellColumnIndex1 = 28; // Column AC is the 29th column, so 28 in 0-based index
                        string lspath2 = HttpContext.Current.Server.MapPath("../../../" + dt["employee_sign"].ToString()); // Assuming this is the path to the image file

                        if (!string.IsNullOrEmpty(dt["employee_sign"].ToString()))
                        {
                           
                            IPictureShape picture2 = sheet.Pictures.Add(cellrowindex, 28 + cellcloumnindex, lspath2);

                            // Set the desired image height (for example, 100 units)
                            int desiredImageHeight = 50;

                            sheet.Cell(cellrowindex, 28 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                            sheet.Cell(cellrowindex, 28 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                            sheet.Cell(cellrowindex, 28 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                            sheet.Cell(cellrowindex, 28 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                            // Get the dimensions of the target cell (AC12)
                            double cellWidth = sheet.Columns[cellrowindex].Width;
                            double cellHeight = sheet.Rows[cellcloumnindex].Height;

                            // Set the picture size
                            picture2.Height = desiredImageHeight;

                            // Optionally adjust the width to maintain the aspect ratio
                            double aspectRatio = (double)picture2.Width / picture2.Height;
                            picture2.Width = (int)(desiredImageHeight * aspectRatio);

                        }

                        sheet.Cell(cellrowindex, 29 + cellcloumnindex).Value = dt["remarks"].ToString();
                        sheet.Cell(cellrowindex, 29 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 29 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 29 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        sheet.Cell(cellrowindex, 29 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        cellrowindex++;
                        // Set the height of the row
                        sheet.Rows[cellrowindex].Height = newRowHeight;
                    }

                }

                string lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "ExportExcel/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                if (!System.IO.Directory.Exists(lspath))
                {
                    System.IO.Directory.CreateDirectory(lspath);
                }
                try
                {
                    string file_name = "EmployeeFormA_Report" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    string excelFilePath = Path.Combine(lspath, file_name);

                    document.SaveAsXLSX(excelFilePath + ".xlsx");
                    document.Close();

                    ls_response = reportexcelfileStreamDownload(excelFilePath + ".xlsx");
                    return ls_response;
                }
                catch (Exception ex)
                {
                    values.message = ex.Message;
                    ls_response = new Dictionary<string, object>
                {
                   { "status", false },
                   { "message", ex.Message }
                };
                    return ls_response;
                }
            }
            catch(Exception ex)
            {
                values.message = ex.Message;
                ls_response = new Dictionary<string, object>
                {
                   { "status", false },
                   { "message", ex.Message }
                };
                return ls_response;
            }
        }
        public Dictionary<string, object> DaExportFormAExcel1(MdlHrmRptEmployeeFormA values)
        {
            var ls_response = new Dictionary<string, object>();

            // Set the license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;  // Or LicenseContext.Commercial if you have a license

            using (ExcelPackage package = new ExcelPackage())
            {
                // Add a new worksheet to the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                msSQL = "select company_code from adm_mst_Tcompany";
                string lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                var imagePath = ConfigurationManager.AppSettings["importexcelfile1"] + lscompany_code + "/" + "HR/Employeedetails/EmployeePhoto-1552.png";

                // Load the image directly from the file path
                var picture = worksheet.Drawings.AddPicture("Image", new FileInfo(imagePath));

                // Set the position in the worksheet (row 1, column 1)
                picture.SetPosition(0, 0, 0, 0);  // Row and column are 0-based

                // Optional: Resize the image to fit within the cell
                picture.SetSize(100, 100); // Set the size of the image in pixels

                // Construct the file path for the Downloads folder
                var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                var fileName = "formA.xlsx"; // Name your file
                var filePath = Path.Combine(downloadsPath, fileName);

                FileInfo fi = new FileInfo(filePath);
                package.SaveAs(fi);
            }

            Console.WriteLine("Excel file with image created successfully and saved to Downloads folder!");
            return ls_response;
        }

        public Dictionary<string, object> reportexcelfileStreamDownload(string path)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var ls_response = new Dictionary<string, object>();
            string file_name = Path.GetFileName(path);
            string file_format = Path.GetExtension(file_name);
            string file_name_extension = Path.GetFileNameWithoutExtension(file_name);

            // Load the Excel file using EPPlus
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorkbook workbook = package.Workbook;

                // Ensure the workbook is not null and contains at least one worksheet
                if (workbook != null && workbook.Worksheets.Count > 0)
                {
                    // Get the first worksheet
                    ExcelWorksheet firstSheet = workbook.Worksheets.First();


                    // Create a new Excel package for modified content
                    using (ExcelPackage modifiedPackage = new ExcelPackage())
                    {
                        // Add a new worksheet to the modified package
                        ExcelWorksheet modifiedSheet = modifiedPackage.Workbook.Worksheets.Add("Sheet1");

                        // Copy cell values and styles
                        for (int row = 1; row <= firstSheet.Dimension.Rows; row++)
                        {
                            for (int col = 1; col <= firstSheet.Dimension.Columns; col++)
                            {
                                // Copy cell value
                                modifiedSheet.Cells[row, col].Value = firstSheet.Cells[row, col].Value;

                                // Clone cell style
                                modifiedSheet.Cells[row, col].Style.Font.Bold = firstSheet.Cells[row, col].Style.Font.Bold;
                                modifiedSheet.Cells[row, col].Style.Font.Italic = firstSheet.Cells[row, col].Style.Font.Italic;
                                modifiedSheet.Cells[row, col].Style.Font.UnderLine = firstSheet.Cells[row, col].Style.Font.UnderLine;
                                modifiedSheet.Cells[row, col].Style.Fill.PatternType = firstSheet.Cells[row, col].Style.Fill.PatternType;
                                modifiedSheet.Cells[row, col].Style.Border.Top.Style = firstSheet.Cells[row, col].Style.Border.Top.Style;
                                modifiedSheet.Cells[row, col].Style.Border.Bottom.Style = firstSheet.Cells[row, col].Style.Border.Bottom.Style;
                                modifiedSheet.Cells[row, col].Style.Border.Left.Style = firstSheet.Cells[row, col].Style.Border.Left.Style;
                                modifiedSheet.Cells[row, col].Style.Border.Right.Style = firstSheet.Cells[row, col].Style.Border.Right.Style;
                                modifiedSheet.Cells[row, col].Style.Fill.PatternType = firstSheet.Cells[row, col].Style.Fill.PatternType;
                                if (firstSheet.Cells[row, col].Style.Font.Color.Rgb != null)
                                {
                                    OfficeOpenXml.Style.ExcelColor excelFontColor = firstSheet.Cells[row, col].Style.Font.Color;
                                    System.Drawing.Color fontColor = System.Drawing.ColorTranslator.FromHtml(excelFontColor.Rgb);
                                    modifiedSheet.Cells[row, col].Style.Font.Color.SetColor(fontColor);
                                }
                                modifiedSheet.Column(col).Width = firstSheet.Column(col).Width;
                                modifiedSheet.Cells[row, col].Style.WrapText = firstSheet.Cells[row, col].Style.WrapText;
                                modifiedSheet.Cells[row, col].Style.HorizontalAlignment = firstSheet.Cells[row, col].Style.HorizontalAlignment;
                                modifiedSheet.Cells[row, col].Style.VerticalAlignment = firstSheet.Cells[row, col].Style.VerticalAlignment;
                            }
                        }
                        modifiedSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        modifiedSheet.View.ShowGridLines = false;


                        // Save the modified package to a memory stream
                        MemoryStream ms = new MemoryStream();
                        modifiedPackage.SaveAs(ms);

                        // Convert the memory stream to a byte array
                        byte[] bytes = ms.ToArray();


                        ls_response.Add("FileName", file_name);
                        ls_response.Add("FileFormat", file_format);
                        ls_response.Add("FileBytes", bytes);

                        ls_response = objFnazurestorage.ConvertDocumentToByteArray(ms, file_name_extension, file_format);
                    }
                }
            }

            return ls_response;
        }


    }
}
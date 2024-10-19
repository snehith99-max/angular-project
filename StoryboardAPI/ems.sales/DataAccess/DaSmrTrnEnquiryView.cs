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

    public class DaSmrTrnEnquiryView
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        private OdbcDataReader objOdbcDataReader;

        DataTable dt_datatable;

        string closure_date, product_requireddate, lsproductgid1, lsrefno, lscompany_code, EnquiryGID, lsenquiry_type, lsleadbank_gid, lscampaign_gid, lspotential_value, lslead_status, lsleadstage, msGetGid, msGetGid1, msgetlead2campaign_gid;
        int mnResult;

        public void DaGetEnquiryView(string enquiry_gid, string employee_gid, MdlSmrTrnEnquiryView values)
        {
            try
            {
                msSQL = " Select a.enquiry_gid,h.campaign_gid, h.campaign_title,a.customer_gid, date_format(a.enquiry_date,'%d-%m-%Y') as enquiry_date,a.customer_name,b.branch_name,a.contact_number, a.enquiry_referencenumber," +
                    " a.enquiry_remarks,a.contact_email,a.contact_address,a.contact_person,i.customer_rating,CASE WHEN a.closure_date = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.closure_date, '%d-%m-%Y') END AS closure_date,a.landmark,a.customer_requirement, a.enquiry_receivedby," +
                    " concat(f.user_code, ' | ', f.user_firstname, ' ', f.user_lastname) as user" +                 
                    " from smr_trn_tsalesenquiry a" +
                    " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid" +
                    " left join hrm_mst_temployee d on a.enquiry_receivedby = d.employee_gid" +
                    " left join adm_mst_tuser f on d.user_gid = f.user_gid" +
                    " left join crm_trn_tenquiry2campaign g on a.customer_gid = g.customer_gid" +
                    " left join smr_trn_tcampaign h on g.campaign_gid = h.campaign_gid" +
                    " left join crm_trn_tenquiry2campaign i on a.enquiry_gid = i.enquiry_gid" +
                    " where a.enquiry_gid='" + enquiry_gid + "' group by a.enquiry_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEnquiryview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEnquiryview_list
                        {
                            customer_name = dt["customer_name"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString(),
                            customer_gid = dt["customer_name"].ToString(),
                            enquiry_remarks = dt["enquiry_remarks"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            closure_date = dt["closure_date"].ToString(),
                            contact_address = dt["contact_address"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                            landmark = dt["landmark"].ToString(),
                            customer_requirement = dt["customer_requirement"].ToString(),
                            enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                            contact_number = dt["contact_number"].ToString(),
                            contact_mail = dt["contact_email"].ToString(),
                            user = dt["user"].ToString(),
                            customer_rating = dt["customer_rating"].ToString(),
                        });
                        values.GetEnquiryView = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product  Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

public void DaGetEnquiryProductView (string enquiry_gid, string employee_gid, MdlSmrTrnEnquiryView values)
{
            try
            {
                msSQL = " Select a.enquirydtl_gid, b.enquiry_gid,a.product_gid,format(a.qty_enquired, 2) as qty_enquired,a.uom_gid,a.productgroup_gid," +
            " format(a.potential_value, 2) as potential_value,c.product_name, c.product_code, d.productgroup_name, e.productuom_name," +
            " CASE WHEN a.product_requireddate = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.product_requireddate, '%d-%m-%Y') END AS product_requireddate" +
            " from smr_trn_tsalesenquirydtl a" +
            " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid" +
            " left join pmr_mst_tproduct c on a.product_gid = c.product_gid" +
            " left join pmr_mst_tproductgroup d on a.productgroup_gid = d.productgroup_gid" +
            " left join pmr_mst_tproductuom e on a.uom_gid = e.productuom_gid" +
            " where b.enquiry_gid='" + enquiry_gid + "' order by a.enquirydtl_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEnquiryViewProduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEnquiryViewProduct_list
                        {
                            enquirydtl_gid = dt["enquirydtl_gid"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            uom_name = dt["productuom_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            qty_enquired = dt["qty_enquired"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            product_requireddate = dt["product_requireddate"].ToString(),
                           
                        });
                        values.GetEnquiryViewProduct = getModuleList;
                    }
                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                    values.message = "Exception occured while Getting Product  Name !";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                    values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

            }
        }

     }
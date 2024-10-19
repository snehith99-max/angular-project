using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace ems.finance.Models
{
    public class MdlGstManagement
    {
        public List<GetGstManagement_list> GetGstManagement_list { get; set; }
        public List<GetInGstManagement_list> GetInGstManagement_list { get; set; }
        public List<GetOutGstManagement_list> GetOutGstManagement_list { get; set; }
        
    }
    public class GetGstManagement_list : result
    {
        public string month { get; set; }
        public string year { get; set; }
        public string branch_name { get; set; }
        public string gst_no { get; set; }
        public string iptax { get; set; }
        public string optax { get; set; }
        public string tax_payable { get; set; }
        public string taxfilling_date { get; set; }
        public string filling_refno { get; set; }
    }
    public class GetInGstManagement_list : result
    {
    public string invoice_gid { get; set; }
    public string invoice_date { get; set; }
    public string invoice_refno { get; set; }
    public string vendor_companyname { get; set; }
    public string gst_number { get; set; }
    public string SGST_0 { get; set; }
    public string SGST_2point5 { get; set; }
    public string SGST_6 { get; set; }
    public string SGST_9 { get; set; }
    public string SGST_14 { get; set; }
    public string CGST_0 { get; set; }
    public string CGST_2point5 { get; set; }
    public string CGST_6 { get; set; }
    public string CGST_9 { get; set; }
    public string CGST_14 { get; set; }
    public string IGST_0 { get; set; }
    public string IGST_5 { get; set; }
    public string IGST_12 { get; set; }
    public string IGST_18 { get; set; }
    public string IGST_28 { get; set; }
    public string invoice_amount { get; set; }
    public string Taxable_Amount { get; set; }
    public string Non_Taxable_Amount { get; set; }
   
}
    public class GetOutGstManagement_list : result
    {
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string customer_name { get; set; }
        public string gst_number { get; set; }
        public string SGST_0 { get; set; }
        public string SGST_2point5 { get; set; }
        public string SGST_6 { get; set; }
        public string SGST_9 { get; set; }
        public string SGST_14 { get; set; }
        public string CGST_0 { get; set; }
        public string CGST_2point5 { get; set; }
        public string CGST_6 { get; set; }
        public string CGST_9 { get; set; }
        public string CGST_14 { get; set; }
        public string IGST_0 { get; set; }
        public string IGST_5 { get; set; }
        public string IGST_12 { get; set; }
        public string IGST_18 { get; set; }
        public string IGST_28 { get; set; }
        public string invoice_amount { get; set; }
        public string Taxable_Amount { get; set; }
        public string Non_Taxable_Amount { get; set; }

    }
}
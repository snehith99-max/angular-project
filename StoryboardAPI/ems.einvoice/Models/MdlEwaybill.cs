﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.einvoice.Models
{
    public class MdlEwaybill : result
    {
        public List<ewaybillsummary_list> ewaybillsummary_list { get; set; }
        public List<ewaybillinvoicesummary_list> ewaybillinvoicesummary_list { get; set; }
        public List<ewaybillinvoicedata_list> ewaybillinvoicedata_list { get; set; }
        public List<addewaybill_list> addewaybill_list { get; set; }
    }
    public class ewaybillsummary_list : result
    {
        public string ewaybill_gid {  get; set; }
        public string ewaybill_refno { get; set; }
        public string invoice_refno { get; set; }
        public string transporter_name { get; set; }
        public string transport_mode { get; set; }
        public string transport_doc_no { get; set; }
        public string vehicle_no { get; set; }
        public string vehicle_type { get; set; }
        public string invoice_amount { get; set; }
        public DateTime created_date { get; set; }
    }
    public class ewaybillinvoicesummary_list : result
    {
        public string so_referencenumber { get; set; }
        public string irn { get; set; }
        public string invoice_gid { get; set; }
        public DateTime invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string customer_name { get; set; }
        public string invoice_reference { get; set; }
        public string invoice_from { get; set; }
        public string invoice_status { get; set; }
        public string mail_status { get; set; }
        public string invoice_amount { get; set; }
        public string customer_contactperson { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
    }
    public class ewaybillinvoicedata_list : result
    {
        public string invoice_gid { get; set; }
        public string irn { get; set; }
        public string invoice_refno { get; set; }
        public double invoice_amount { get; set; }
    }
    public class addewaybill_list : result
    {
        public string ewaybill_invoice_gid { get; set; }
        public string ewaybill_irn { get; set; }
        public string ewaybill_invoice_ref_no { get; set;}
        public double ewaybill_invoice_amount { get; set; }
        public string ewaybill_transporter_id { get; set; }
        public string ewaybill_transport_mode { get; set; }
        public string ewaybill_transporter_doc_no { get; set; }
        public DateTime ewaybill_transporter_date { get;set; }
        public string ewaybill_transporter_name {  get; set; }
        public string ewaybill_vehicle_no { get; set; }
        public string ewaybill_vehicle_date { get; set; }
        public string ewaybill_vehicle_type { get; set; }
        public string ewaybill_approximate_distance { get; set; }
    }    
    public class generateewaybill : result
    {
        public string Irn { get; set; }
        public string Distance { get; set; }
        public string TransMode { get; set; }
        public string TransId { get; set; }
        public string TransName { get; set; }
        public string TransDocDt { get; set; }
        public string TransDocNo { get; set; }
        public string VehNo { get; set; }
        public string VehType { get; set; }
    }  
}
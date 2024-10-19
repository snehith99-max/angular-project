﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrDashboard : result
    {
        public List<GetPurchaseLiability_List> GetPurchaseLiability_List { get; set; }
        public List<GetExchangeRateAPI_List> GetExchangeRateAPI_List { get; set; } 
        public List<GetExchangeRateAPICredential_List> GetExchangeRateAPICredential_List { get; set; }
        public string total_vendor { get; set; }
        public string total_product { get; set; }
        public string pototalcount { get; set; }
        public string invctotalcount { get; set; }
        public string grntotalcount { get; set; } 
        public string total_payment { get; set; }
        public string total_invoice { get; set; }
        public string cancelled_invoice { get; set; }
        public string pending_invoice { get; set; } 
        public string completed_invoice { get; set; } 
        public string cancelled_payment { get; set; }
        public string pending_payment { get; set; }
        public string completed_payment { get; set; } 
        public int count_date { get; set; } 
       //
       public List<Getpmrdashboard_list> Getpmrdashboard_list { get; set; }
       public List<Getpurchasechart_list> Getpurchasechart_list { get; set; }
       public List<Getinvoicechart_list> Getinvoicechart_list { get; set; }
       public List<Getpaymentchart_list> Getpaymentchart_list { get; set; }
       public List<Getpurchaseorderchart_list> Getpurchaseorderchart_list { get; set; }




    }
    public class Getpmrdashboard_list
    {
        public string pototalcount { get; set; }
        public string total_vendor { get; set; }
        public string month_invoiceamount { get; set; }
        public string month_invoicecount { get; set; }
        public string mtd_invoice { get; set; }
        public string ytd_invoiceamount { get; set; }
        public string ytd_invoice { get; set; }
        public string ytd_invoicecount { get; set; }
        public string currency_symbol { get; set; }
    }
     public class Getpurchasechart_list
    {
        public string payment_count { get; set; }
        public string invoice_count { get; set; }
        public string purchase_count { get; set; }
        public string Months { get; set; }
    }
      public class Getinvoicechart_list
    {
        public string completed_invoice { get; set; }
        public string pending_invoice { get; set; }
        public string cancel_invoice { get; set; }
        public string approved_invoice { get; set; }
    }
    public class Getpaymentchart_list
    {
        public string completed_payment { get; set; }
        public string approved_payment { get; set; }
        public string cancelled_payment { get; set; }
    }
    public class Getpurchaseorderchart_list
    {
        public string po_cancelled { get; set; }
        public string po_pending { get; set; }
        public string po_completed { get; set; }
        public string po_approved { get; set; }
    }



    public class GetExchangeRateAPICredential_List : result
    {
        public string api_url { get; set; }
        public string api_key { get; set; }
        public string api_host { get; set; }
 
    }

        public class GetExchangeRateAPI_List : result
    {
        public string currencyexchangeapihistory_gid { get; set; }
        public string updated_date { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }


        
    }
    public class GetPurchaseLiability_List : result
    {
    
        public string payment_amount { get; set; }
        public string total_amount { get; set; }
        public string invoice_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string purchasemonth { get; set; }
       

    }

    //public class CurrencyExchangeRate_List
    //{
    //    public rates[] rates { get; set; }
    //}
    public class ExchangeRate_List
    {
        public string result { get; set; }
        public string provider { get; set; }
        public string documentation { get; set; }
        public string terms_of_use { get; set; }
        public int time_last_update_unix { get; set; }
        public string time_last_update_utc { get; set; }
        public int time_next_update_unix { get; set; }
        public string time_next_update_utc { get; set; }
        public int time_eol_unix { get; set; }
        public string base_code { get; set; }
        public Rates rates { get; set; }
    }

    public class Rates
    {
        public int USD { get; set; }
        public float AED { get; set; }
        public float AFN { get; set; }
        public float ALL { get; set; }
        public float AMD { get; set; }
        public float ANG { get; set; }
        public float AOA { get; set; }
        public float ARS { get; set; }
        public float AUD { get; set; }
        public float AWG { get; set; }
        public float AZN { get; set; }
        public float BAM { get; set; }
        public int BBD { get; set; }
        public float BDT { get; set; }
        public float BGN { get; set; }
        public float BHD { get; set; }
        public float BIF { get; set; }
        public int BMD { get; set; }
        public float BND { get; set; }
        public float BOB { get; set; }
        public float BRL { get; set; }
        public int BSD { get; set; }
        public float BTN { get; set; }
        public float BWP { get; set; }
        public float BYN { get; set; }
        public int BZD { get; set; }
        public float CAD { get; set; }
        public float CDF { get; set; }
        public float CHF { get; set; }
        public float CLP { get; set; }
        public float CNY { get; set; }
        public float COP { get; set; }
        public float CRC { get; set; }
        public int CUP { get; set; }
        public float CVE { get; set; }
        public float CZK { get; set; }
        public float DJF { get; set; }
        public float DKK { get; set; }
        public float DOP { get; set; }
        public float DZD { get; set; }
        public float EGP { get; set; }
        public int ERN { get; set; }
        public float ETB { get; set; }
        public float EUR { get; set; }
        public float FJD { get; set; }
        public float FKP { get; set; }
        public float FOK { get; set; }
        public float GBP { get; set; }
        public float GEL { get; set; }
        public float GGP { get; set; }
        public float GHS { get; set; }
        public float GIP { get; set; }
        public float GMD { get; set; }
        public float GNF { get; set; }
        public float GTQ { get; set; }
        public float GYD { get; set; }
        public float HKD { get; set; }
        public float HNL { get; set; }
        public float HRK { get; set; }
        public float HTG { get; set; }
        public float HUF { get; set; }
        public float IDR { get; set; }
        public float ILS { get; set; }
        public float IMP { get; set; }
        public float INR { get; set; }
        public float IQD { get; set; }
        public float IRR { get; set; }
        public float ISK { get; set; }
        public float JEP { get; set; }
        public float JMD { get; set; }
        public float JOD { get; set; }
        public float JPY { get; set; }
        public float KES { get; set; }
        public float KGS { get; set; }
        public float KHR { get; set; }
        public float KID { get; set; }
        public float KMF { get; set; }
        public float KRW { get; set; }
        public float KWD { get; set; }
        public float KYD { get; set; }
        public float KZT { get; set; }
        public float LAK { get; set; }
        public int LBP { get; set; }
        public float LKR { get; set; }
        public float LRD { get; set; }
        public float LSL { get; set; }
        public float LYD { get; set; }
        public float MAD { get; set; }
        public float MDL { get; set; }
        public float MGA { get; set; }
        public float MKD { get; set; }
        public float MMK { get; set; }
        public float MNT { get; set; }
        public float MOP { get; set; }
        public float MRU { get; set; }
        public float MUR { get; set; }
        public float MVR { get; set; }
        public float MWK { get; set; }
        public float MXN { get; set; }
        public float MYR { get; set; }
        public float MZN { get; set; }
        public float NAD { get; set; }
        public float NGN { get; set; }
        public float NIO { get; set; }
        public float NOK { get; set; }
        public float NPR { get; set; }
        public float NZD { get; set; }
        public float OMR { get; set; }
        public int PAB { get; set; }
        public float PEN { get; set; }
        public float PGK { get; set; }
        public float PHP { get; set; }
        public float PKR { get; set; }
        public float PLN { get; set; }
        public float PYG { get; set; }
        public float QAR { get; set; }
        public float RON { get; set; }
        public float RSD { get; set; }
        public float RUB { get; set; }
        public float RWF { get; set; }
        public float SAR { get; set; }
        public float SBD { get; set; }
        public float SCR { get; set; }
        public float SDG { get; set; }
        public float SEK { get; set; }
        public float SGD { get; set; }
        public float SHP { get; set; }
        public float SLE { get; set; }
        public float SLL { get; set; }
        public float SOS { get; set; }
        public float SRD { get; set; }
        public float SSP { get; set; }
        public float STN { get; set; }
        public float SYP { get; set; }
        public float SZL { get; set; }
        public float THB { get; set; }
        public float TJS { get; set; }
        public float TMT { get; set; }
        public float TND { get; set; }
        public float TOP { get; set; }
        public float TRY { get; set; }
        public float TTD { get; set; }
        public float TVD { get; set; }
        public float TWD { get; set; }
        public float TZS { get; set; }
        public float UAH { get; set; }
        public float UGX { get; set; }
        public float UYU { get; set; }
        public float UZS { get; set; }
        public float VES { get; set; }
        public float VND { get; set; }
        public float VUV { get; set; }
        public float WST { get; set; }
        public float XAF { get; set; }
        public float XCD { get; set; }
        public float XDR { get; set; }
        public float XOF { get; set; }
        public float XPF { get; set; }
        public float YER { get; set; }
        public float ZAR { get; set; }
        public float ZMW { get; set; }
        public float ZWL { get; set; }
    }


}
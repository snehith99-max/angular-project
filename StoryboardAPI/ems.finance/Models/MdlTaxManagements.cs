using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlTaxManagements : result
    {
        public List<GetTaxManagementSummary_List> GetTaxManagementSummary_List { get; set; }
        public List<GetOutputTaxView_List> GetOutputTaxView_List { get; set; }
        public List<GetInputTaxView_List> GetInputTaxView_List { get; set; }
        public List<GetCreditNoteTaxView_List> GetCreditNoteTaxView_List { get; set; }
        public List<GetTotalTaxView_List> GetTotalTaxView_List { get; set; }
        public List<GetTotalCreditTaxView_List> GetTotalCreditTaxView_List { get; set; }
        public List<GetCompileInputTax_List> GetCompileInputTax_List { get; set; }
        public List<GetCompileOutputTax_List> GetCompileOutputTax_List { get; set; }
        public List<GetCompileCreditTax_List> GetCompileCreditTax_List { get; set; }
        public List<GetCompileDebitTax_List> GetCompileDebitTax_List { get; set; }
        public List<GetTaxCalculation_List> GetTaxCalculation_List { get; set; }
    }
    public class GetTaxCalculation_List
    {
        public string input_tax { get; set; }
        public string output_tax { get; set; }
        public string credit_tax { get; set; }
        public string debit_tax { get; set; }
        public string totaloutputcredit_tax { get; set; }
        public string totalinputdebit_tax { get; set; }
        public string tax_payable { get; set; }
        public string ttotaloutputcredit_tax { get; set; }
        public string lesscarry_forward { get; set; }
        public string totaltax { get; set; }
        public string ttotaltax { get; set; }
    }
    public class GetCompileInputTax_List
    {
        public string tmptaxinput_gid { get; set; }
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
        public string vendor_name { get; set; }
        public string gst_no { get; set; }
    }
    public class GetCompileOutputTax_List
    {
        public string tmptaxoutput_gid { get; set; }
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
        public string customer_name { get; set; }
        public string gst_no { get; set; }
    }
    public class GetCompileCreditTax_List
    {
        public string tmpcredittaxoutput_gid { get; set; }
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
        public string customer_name { get; set; }
        public string gst_no { get; set; }
    }
    public class GetCompileDebitTax_List
    {
        public string tmpdebittaxoutput_gid { get; set; }
        public string invoice_gid { get; set; }
        public string debit_date { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
        public string vendor_name { get; set; }
        public string gst_no { get; set; }
    }
    public class GetTotalCreditTaxView_List
    {
        public string totalcredit_tax { get; set; }
        public string tax_payable { get; set; }
        public string carry_forward { get; set; }
        public double totaloutput_tax { get; set; }

    }
    public class GetTaxManagementSummary_List
    {
        public string fillingref_no { get; set; }
        public string tax_input { get; set; }
        public string tax_output { get; set; }
        public string overalltax_amount { get; set; }
        public string taxfilling_date { get; set; }
        public string month { get; set; }
    }
    public class GetOutputTaxView_List
    {
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string customer_name { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
        public string irn { get; set; }
        public string customer { get; set; }
        public string gst_number { get; set; }
        public string SGST_0 { get; set; }
        public string SGST_25 { get; set; }
        public string SGST_6 { get; set; }
        public string SGST_9 { get; set; }
        public string SGST_14 { get; set; }
        public string CGST_0 { get; set; }
        public string CGST_25 { get; set; }
        public string CGST_6 { get; set; }
        public string CGST_9 { get; set; }
        public string CGST_14 { get; set; }
        public string IGST_0 { get; set; }
        public string IGST_5 { get; set; }
        public string IGST_12 { get; set; }
        public string IGST_18 { get; set; }
        public string IGST_28 { get; set; }
        public string invoiceamount { get; set; }
        public string taxableamount { get; set; }
        public string nontaxable_amount { get; set; }
    }
    public class GetInputTaxView_List
    {
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string vendor_name { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
        public string vendor { get; set; }
        public string gst_number { get; set; }
        public string SGST_0 { get; set; }
        public string SGST_25 { get; set; }
        public string SGST_6 { get; set; }
        public string SGST_9 { get; set; }
        public string SGST_14 { get; set; }
        public string CGST_0 { get; set; }
        public string CGST_25 { get; set; }
        public string CGST_6 { get; set; }
        public string CGST_9 { get; set; }
        public string CGST_14 { get; set; }
        public string IGST_0 { get; set; }
        public string IGST_5 { get; set; }
        public string IGST_12 { get; set; }
        public string IGST_18 { get; set; }
        public string IGST_28 { get; set; }
        public string invoiceamount { get; set; }
        public string taxableamount { get; set; }
        public string nontaxable_amount { get; set; }
    }
    public class GetCreditNoteTaxView_List
    {
        public string invoice_gid { get; set; }
        public string credit_date { get; set; }
        public string creditnote_gid { get; set; }
        public string customer_name { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
    }
    public class GetTotalTaxView_List
    {
        public string lessinput_tax { get; set; }
        public string totaloutputtaxwith_credit { get; set; }
        public string interestcharges_payable { get; set; }
        public string penalty_payable { get; set; }
        public string previouscarry_forward { get; set; }
        public string totalcarry_forward { get; set; }
        public string totaltax_payable { get; set; }
    }
    public class InputTaxSummary : result
    {
        public InputTaxSummary_List[] InputTaxSummary_List;
    }
    public class InputTaxSummary_List
    {
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string vendor_name { get; set; }
        public string gst_number { get; set; }
        public string invoice_amount { get; set; }
        public string total_tax { get; set; }
        public string Taxable_Amount { get; set; }
    }
    public class OutputTaxSummary : result
    {
        public OutputTaxSummary_List[] OutputTaxSummary_List;
    }
    public class OutputTaxSummary_List
    {
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string customer_name { get; set; }
        public string gst_number { get; set; }
        public string invoice_amount { get; set; }
        public string total_tax { get; set; }
        public string Taxable_Amount { get; set; }
    }
    public class CreditNoteTaxSummary : result
    {
        public CreditNoteTaxSummary_List[] CreditNoteTaxSummary_List;
    }
    public class CreditNoteTaxSummary_List
    {
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string credit_date { get; set; }
        public string customer_name { get; set; }
        public string gst_number { get; set; }
        public string invoice_amount { get; set; }
        public string Taxable_Amount { get; set; }
    }
    public class DebitNoteTaxSummary : result
    {
        public DebitNoteTaxSummary_List[] DebitNoteTaxSummary_List;
    }
    public class DebitNoteTaxSummary_List
    {
        public string invoice_gid { get; set; }
        public string debit_date { get; set; }
        public string vendor_name { get; set; }
        public string gst_number { get; set; }
        public string invoice_amount { get; set; }
        public string Taxable_Amount { get; set; }
    } 
   
    public class TaxOverallSubmitDtls : result
    {
        public List<CompileInputTax_List> CompileInputTax_List { get; set; }
        public List<CompileOutputTax_List> CompileOutputTax_List { get; set; }
        public List<CompileCreditTax_List> CompileCreditTax_List { get; set; }
        public List<TaxCalculation_List> TaxCalculation_List { get; set; }
        public string interestcharge_payable { get; set; }
        public string penality_payable { get; set; }
    }
    public class CompileInputTax_List
    {
        public string invoice_gid { get; set; }
        public string tmptaxinput_gid { get; set; }
        public string invoice_date { get; set; }
        public string vendor_name { get; set; }
        public string gst_no { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
    }
    public class CompileOutputTax_List
    {
        public string invoice_gid { get; set; }
        public string tmptaxoutput_gid { get; set; }
        public string invoice_date { get; set; }
        public string customer_name { get; set; }
        public string gst_no { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
    }
    public class CompileCreditTax_List
    {
        public string invoice_gid { get; set; }
        public string tmpcredittaxoutput_gid { get; set; }
        public string invoice_date { get; set; }
        public string customer_name { get; set; }
        public string gst_no { get; set; }
        public string invoice_amount { get; set; }
        public string taxable_amount { get; set; }
    }
    public class TaxCalculation_List
    {
        public string input_tax { get; set; }
        public string output_tax { get; set; }
        public string credit_tax { get; set; }
        public string debit_tax { get; set; }
        public string totaloutputcredit_tax { get; set; }
        public string totalinputdebit_tax { get; set; }
        public string tax_payable { get; set; }
        public string ttotaloutputcredit_tax { get; set; }
        public string lesscarry_forward { get; set; }
        public string totaltax { get; set; }
        public string ttotaltax { get; set; }
    }
}
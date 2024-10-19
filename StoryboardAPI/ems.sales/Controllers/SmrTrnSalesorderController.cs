using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.storage.Models;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrTrnSalesorder")]
    [Authorize]
    public class SmrTrnSalesorderController : ApiController
    {
        string msSQL = string.Empty;
        int mnResult;
        cmnfunctions cmnfunctions = new cmnfunctions();
        dbconn dbconn = new dbconn();

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnSalesorder objsales = new DaSmrTrnSalesorder();

        Fnazurestorage objFnazurestorage = new Fnazurestorage();



        // Module Summary
        [ActionName("GetSmrTrnSalesordersummary")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnSalesordersummary()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetSmrTrnSalesordersummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("checkdeliveryorderforinvoice")]
        [HttpGet]
        public HttpResponseMessage checkdeliveryorderforinvoice(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.Dacheckdeliveryorderforinvoice(salesorder_gid , values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSoDraftsSummary")]
        [HttpGet]
        public HttpResponseMessage GetSoDraftsSummary()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetSoDraftsSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditSODrafts")]
        [HttpGet]
        public HttpResponseMessage GetEditSODrafts(string salesorder_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetEditSODrafts(salesorder_gid, getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("checkinvoice")]
        [HttpGet]
        public HttpResponseMessage checkinvoice(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.checkinvoice( salesorder_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getsalesordersixmonthschart")]
        [HttpGet]
        public HttpResponseMessage Getsalesordersixmonthschart()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.daGetsalesordersixmonthschart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSmrTrnSalesorder2invoicesummary")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnSalesorder2invoicesummary()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetSmrTrnSalesorder2invoicesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSmrTrnSalesorder2invoiceServicessummary")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnSalesorder2invoiceServicessummary()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetSmrTrnSalesorder2invoiceServicessummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetSmrTrnSalesordercount")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnSalesordercount()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetSmrTrnSalesordercount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetViewsalesorderSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewsalesorderSummary(string salesorder_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            objsales.DaGetViewsalesorderSummary(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // Branch dropdown
        [ActionName("GetViewsalesorderdetails")]
        [HttpGet]
        public HttpResponseMessage GetViewsalesorderdetails(string salesorder_gid, string customer_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            objsales.DaGetViewsalesorderdetails(salesorder_gid, customer_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetEditsalesorderSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditsalesorderSummary(string salesorder_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetEditsalesorderSummary(salesorder_gid, getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        // Customer onchange 360

        [ActionName("GetCustomerOnchangeCRM")]
        [HttpGet]
        public HttpResponseMessage GetCustomerOnchangeCRM(string customer_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetCustomerOnchangeCRM(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }




        // Product dropdown CRM

        [ActionName("GetProductNamDtlCRM")]
        [HttpGet]
        public HttpResponseMessage GetProductNamDtlCRM()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetProductNamDtlCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 4 dropdown





        [ActionName("GetOnChangeProductsNames")]
        [HttpGet]

        public HttpResponseMessage GetOnChangeProductsNames(string product_gid)

        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnChangeProductsNames(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetOnChangeProductsNameAmend")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductsNameAmend(string product_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnChangeProductsNameAmend(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        // productadd///

        //Delete product summary//


        //product summary/


        //On change currency
        [ActionName("GetOnChangeCurrency")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCurrency(string currencyexchange_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnchangeCurrency(currencyexchange_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSalesProductdetails")]
        [HttpGet]
        public HttpResponseMessage GetSalesProductdetails(string salesorder_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            objsales.DaGetSalesProductdetails(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("getCancelSalesOrder")]
        [HttpGet]
        public HttpResponseMessage getCancelSalesOrder(string params_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            objsales.DagetCancelSalesOrder(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // CRM product on change
        [ActionName("GetOnChangeProductsNameCRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductsNameCRM(string product_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnChangeProductsNameCRM(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // DIRECT SALES ORDER PRODUCT EDIT

        [ActionName("GetDirectSalesOrderEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDirectSalesOrderEditProductSummary(string tmpsalesorderdtl_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            objsales.DaGetDirectSalesOrderEditProductSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }




        // UPDATE PRODUCT -- DIRECT QUOTATION PRODUCT SUMMARY

        [ActionName("PostUpdateDirectSOProduct")]
        [HttpPost]
        public HttpResponseMessage PostUpdateDirectSOProduct(DirecteditSalesorderList values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostUpdateDirectSOProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getsalesonupdate")]
        [HttpPost]
        public HttpResponseMessage Getsalesonupdate()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetsalesonupdate(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getupdate")]
        [HttpGet]
        public HttpResponseMessage Getupdate()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetupdate(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Customer dropdown 360

        [ActionName("GetCustomerDtl360")]
        [HttpGet]
        public HttpResponseMessage GetCustomerDtl360(string leadbank_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetCustomerDtl360(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // SalesOrder History
        [ActionName("Getsalesorderhistorysummarydata")]
        [HttpGet]
        public HttpResponseMessage Getsalesorderhistorysummarydata(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetsalesorderhistorysummarydata(values, salesorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getsalesorderhistorydata")]
        [HttpGet]
        public HttpResponseMessage Getsalesorderhistorydata(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetsalesorderhistorydata(values, salesorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getsalesorderproductdetails")]
        [HttpGet]
        public HttpResponseMessage Getsalesorderproductdetails(string salesorder_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            objsales.DaGetsalesorderproductdetails(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // product type






        // -----------------------------------------------new sales order ----------------------------------------------------//


        [ActionName("Loadpage")]
        [HttpGet]
        public HttpResponseMessage Loadpage()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            msSQL = "delete from smr_tmp_tsalesorderdtl where employee_gid='" + getsessionvalues.employee_gid + "'";
            mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [ActionName("Getproducttypesales")]
        [HttpGet]
        public HttpResponseMessage Getproducttypesales()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetproducttypesales(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranchDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchDtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetBranchDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerDtl")]
        [HttpGet]
        public HttpResponseMessage GetCustomerDtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetCustomerDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPersonDtl")]
        [HttpGet]
        public HttpResponseMessage GetPersonDtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetPersonDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCurrencyDtl")]
        [HttpGet]
        public HttpResponseMessage GetCurrencyDtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetCurrencyDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTax4Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax4Dtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetTax4Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeProductsName")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductsName(string product_gid, string customercontact_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnChangeProductsName(product_gid, customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeCustomer")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCustomer(string customer_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnChangeCustomer(customer_gid, values); 
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeShipping")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeShipping(string customer_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.Dagetshippingtolist(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductNamDtl")]
        [HttpGet]
        public HttpResponseMessage GetProductNamDtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetProductNamDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostOnAdds")]
        [HttpPost]
        public HttpResponseMessage PostOnAdds(salesorders_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostOnAdds(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ProductSalesSummary")]
        [HttpGet]
        public HttpResponseMessage ProductSalesSummary(string customer_gid, string product_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaProductSalesSummary(getsessionvalues.employee_gid, customer_gid, values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductsearchSummarySales")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummarySales()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetProductsearchSummarySales(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSalesorderdetail")]
        [HttpGet]
        public HttpResponseMessage GetSalesorderdetail(string product_gid)
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            objsales.DaGetSalesorderdetail(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("onbackcleartmpdata")]
        [HttpGet]
        public HttpResponseMessage onbackcleartmpdata()
        {
            MdlSmrTrnSalesorder objresult = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.Daonbackcleartmpdata(getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostSalesOrder")]
        [HttpPost]
        public HttpResponseMessage PostSalesOrder(postsales_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostSalesOrder(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostSalesOrderfileupload")]
        [HttpPost]
        public HttpResponseMessage PostSalesOrderfileupload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objsales.DaPostSalesOrderfileupload(httpRequest, objResult, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("UpdateSalesOrder")]
        [HttpPost]
        public HttpResponseMessage UpdateSalesOrder(postsales_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaUpdateSalesOrder(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateSalesOrderfileupload")]
        [HttpPost]
        public HttpResponseMessage UpdateSalesOrderfileupload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objsales.DaUpdateSalesOrderfileupload(httpRequest, objResult, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("GetSalesOrdersummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesOrdersummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetSalesOrdersummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDeleteDirectSOProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteDirectSOProductSummary(string tmpsalesorderdtl_gid)
        {
            salesorders_list objresult = new salesorders_list();
            objsales.GetDeleteDirectSOProductSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetOnChangeProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductGroup(string productgroup_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnChangeProductGroup(productgroup_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeProductGroupName")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductGroupName(string product_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnChangeProductGroupName(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostProductAdd")]
        [HttpPost]
        public HttpResponseMessage PostProductAdd(PostProduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostProductAdd(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetProductGroup()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetProductGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeBranch")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeBranch(string branch_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOnChangeBranch(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetApprovalSalesOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetApprovalSalesOrderSummary(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetApprovalSalesOrderSummary(salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getapprovalsalesorderdetails")]
        [HttpGet]
        public HttpResponseMessage Getapprovalsalesorderdetails(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetapprovalsalesorderdetails(salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getsalesorderdtlrecords")]
        [HttpGet]
        public HttpResponseMessage Getsalesorderdtlrecords(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetsalesorderdtlrecords(salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        // Tax 1 dropdown

        [ActionName("GetTax1Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax1Dtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetTax1Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 2 dropdown

        [ActionName("GetTax2Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax2Dtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetTax2Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 3 dropdown

        [ActionName("GetTax3Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax3Dtl()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetTax3Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Approvalsalesorder")]
        [HttpGet]
        public HttpResponseMessage Approvalsalesorder(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            msSQL = " update smr_trn_tsalesorder set salesorder_status='Approved'" +
                " where salesorder_gid='"+ salesorder_gid +"'";
            mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Sales Order Approved Successfully.";
            }
            else
            {
                values.status = true;
                values.message = "Something went wrong while approve sales order.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductWithTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductWithTaxSummary(string product_gid, string customer_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetProductWithTaxSummary(product_gid, customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOrderToinvoiceDetailsSummary")]
        [HttpGet]
        public HttpResponseMessage GetOrderToinvoiceDetailsSummary(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetOrderToinvoiceDetailsSummary(salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOrderToInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetOrderToInvoiceProductSummary(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetOrderToInvoiceProductSummary(getsessionvalues.employee_gid, salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeliveryToInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeliveryToInvoiceProductSummary(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetDeliveryToInvoiceProductSummary(getsessionvalues.employee_gid, salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOrderToInvoiceProductDetails")]
        [HttpGet]
        public HttpResponseMessage GetOrderToInvoiceProductDetails(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetOrderToInvoiceProductDetails(getsessionvalues.employee_gid, salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeliveryToInvoiceProductDetails")]
        [HttpGet]
        public HttpResponseMessage GetDeliveryToInvoiceProductDetails(string salesorder_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetDeliveryToInvoiceProductDetails(getsessionvalues.employee_gid, salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostOrderToInvoiceProductAdd")]
        [HttpPost]
        public HttpResponseMessage PostOrderToInvoiceProductAdd(ordertoinvoiceproductsubmit_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.PostOrderToInvoiceProductAdd(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOnsubmitOrdertoInvoice")]
        [HttpPost]
        public HttpResponseMessage PostOnsubmitOrdertoInvoice(ordertoinvoicesubmit values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.PostOnsubmitOrdertoInvoice(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OrdertoInvoiceProductDelete")]
        [HttpGet]
        public HttpResponseMessage OrdertoInvoiceProductDelete(string invoicedtl_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            msSQL = " delete from rbl_tmp_tinvoicedtl where invoicedtl_gid='" + invoicedtl_gid + "'";
            mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Product deleted successfully.";
            }
            else
            {
                values.status = false;
                values.message = "Error while deleting product.";
            }
            return Request.CreateResponse(HttpStatusCode.OK,values);
        }
        [ActionName("DownloadDocument")]
        [HttpPost]
        public HttpResponseMessage download_Collateraldoc(MdlTelecallingDownload values)
        {
            var ls_response = new Dictionary<string, object>();
            //values.file_path = objFnazurestorage.DecryptData(values.file_path);
            ls_response = objFnazurestorage.FnDownloadDocument(values.file_path, values.file_name);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
        [ActionName("GetCourierSerive")]
        [HttpGet]
        public HttpResponseMessage GetCourierSerive()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objsales.DaGetCourierSerive(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //Drafts
        [ActionName("PostSalesOrderDrafts")]
        [HttpPost]
        public HttpResponseMessage PostSalesOrderDrafts(postsales_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostSalesOrderDrafts(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostSalesOrderfileuploadDrafts")]
        [HttpPost]
        public HttpResponseMessage PostSalesOrderfileuploadDrafts()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objsales.DaPostSalesOrderfileuploadDrafts(httpRequest, objResult, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
    }
}

using ems.utilities.Functions;
using ems.utilities.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using ems.sales.Models;
using ems.sales.DataAccess;

namespace ems.sales.Controllers
{

    [RoutePrefix("api/SmrMstTaxSegment")]
    [Authorize]
    public class SmrMstTaxSegmentController:ApiController
    {
        string msSQL = string.Empty;
        int mnResult;
        dbconn dbconn = new dbconn();

            session_values Objgetgid = new session_values();
            logintoken getsessionvalues = new logintoken();
            DaSmrMstTaxSegment objDaSmrMstTaxSegment = new DaSmrMstTaxSegment();
            // Module Summary
            [ActionName("GetTaxSegmentSummary")]
            [HttpGet]
            public HttpResponseMessage GetTaxSegmentSummary()
            {
                MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
                objDaSmrMstTaxSegment.DaGetTaxSegmentSummary(values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }

        [ActionName("GetTaxSegment2ProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegment2ProductSummary(string product_gid)
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetTaxSegment2ProductSummary(values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTax")]
        [HttpGet]
        public HttpResponseMessage GetTax()
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            objDaSmrMstTaxSegment.DaGetTax(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Post Tax
        [ActionName("PostTaxSegment")]
        [HttpPost]
        public HttpResponseMessage PostTaxSegment(TaxSegmentSummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrMstTaxSegment.DaPostTaxSegment(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatedTaxSegmentSummary")]
        [HttpPost]
        public HttpResponseMessage UpdatedTaxSegmentSummary(TaxSegmentSummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrMstTaxSegment.DaUpdatedTaxSegmentSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteTaxSegmentSummary")]
        [HttpGet]
        public HttpResponseMessage DeleteTaxSegmentSummary(string taxsegment_gid)
        {
            TaxSegmentSummary_list objresult = new TaxSegmentSummary_list();
            objDaSmrMstTaxSegment.DaDeleteTaxSegmentSummary(taxsegment_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostTaxSegment2Product")]
        [HttpPost]
        public HttpResponseMessage PostTaxSegment2Product(tax_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrMstTaxSegment.DaPostTaxSegment2Product(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteTaxSegment2Product")]
        [HttpGet]
        public HttpResponseMessage DeleteTaxSegment2Product(string taxsegment2product_gid)
        {
            TaxSegmentSummary_list objresult = new TaxSegmentSummary_list();
            objDaSmrMstTaxSegment.DaDeleteTaxSegment2Product(taxsegment2product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetCustomerUnassignedlist")]
        [HttpGet]
        public HttpResponseMessage GetUnassignedlist(string taxsegment_gid, string customer_gid)
        {
            MdlSmrMstTaxSegment objresult = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetUnassignedlist(taxsegment_gid, customer_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        
        [ActionName("GetCustomerAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetCustomerAssignedlist(string taxsegment_gid)
        {
            MdlSmrMstTaxSegment objresult = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetCustomerAssignedlist(taxsegment_gid,  objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
       
        [ActionName("PostCustomerUnassignedlist")]
        [HttpPost]
        public HttpResponseMessage PostCustomerUnassignedlist(customerassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrMstTaxSegment.DaPostCustomerUnassignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetVendorUnassignedlist")]
        [HttpGet]
        public HttpResponseMessage GetVendorUnassignedlist(string taxsegment_gid, string vendor_gid)
        {
            MdlSmrMstTaxSegment objresult = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetVendorUnassignedlist(taxsegment_gid, vendor_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetVendorAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetVendorAssignedlist(string taxsegment_gid, string vendor_gid)
        {
            MdlSmrMstTaxSegment objresult = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetVendorAssignedlist(taxsegment_gid, vendor_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostVendorUnassignedlist")]
        [HttpPost]
        public HttpResponseMessage PostVendorUnassignedlist(vendorassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrMstTaxSegment.DaPostVendorUnassignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerCount")]
        [HttpGet]
        public HttpResponseMessage GetCustomerCount()
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetCustomerCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetVendorrCount")]
        [HttpGet]
        public HttpResponseMessage GetVendorrCount()
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetVendorCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTotalCustomerSummary")]
        [HttpGet]
        public HttpResponseMessage GetTotalCustomerSummary()
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetTotalCustomerSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTotalCustomerWithinState")]
        [HttpGet]
        public HttpResponseMessage GetTotalCustomerWithinState()
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetTotalCustomerWithinState(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        
        [ActionName("GetTotalCustomerInterState")]
        [HttpGet]
        public HttpResponseMessage GetTotalCustomerInterState()
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetTotalCustomerInterState(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        
        [ActionName("GetTotalCustomerOverSeaState")]
        [HttpGet]
        public HttpResponseMessage GetTotalCustomerOverSeaState()
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetTotalCustomerOverSeaState(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerUnassignSummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerUnassignSummary()
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetCustomerUnassignSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTaxSegmentDropDown")]
        [HttpGet]

        public HttpResponseMessage GetTaxSegmentDropDown(string taxsegment_gid)
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetTaxSegmentDropDown(taxsegment_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostTaxsegmentMoveOn")]
        [HttpPost]

        public HttpResponseMessage PostTaxsegmentMoveOn(PostTaxsegment values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrMstTaxSegment.DaPostTaxsegmentMoveOn(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOtherSegamentSummary")]
        [HttpGet]
        public HttpResponseMessage GetOtherSegamentSummary()
        {
            MdlSmrMstTaxSegment values = new MdlSmrMstTaxSegment();
            objDaSmrMstTaxSegment.DaGetOtherSegamentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UnassignedCustomer")]
        [HttpPost]
        
        public HttpResponseMessage UnassignedCustomer(postunassignedcustomer values)
        {
            for (int i = 0;  i < values.GetCustomerassignedlist.ToArray().Length; i++ )
            {
                msSQL = " delete from acp_mst_ttaxsegment2customer where customer_gid='" + values.GetCustomerassignedlist[i].customer_gid + "'";
                mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " update crm_mst_tcustomer set taxsegment_gid=null where customer_gid='" + values.GetCustomerassignedlist[i].customer_gid + "'";
                    mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Customer unassigned successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occure while unassigning customer";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error occure while unassigning customer";
                }
            }           
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}

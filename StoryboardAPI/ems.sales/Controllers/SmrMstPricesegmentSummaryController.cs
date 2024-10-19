﻿using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using static ems.sales.Models.MdlSmrMstPricesegmentSummary;
using System.Reflection.Emit;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrMstPricesegmentSummary")]
    [Authorize]
    public class SmrMstPricesegmentSummaryController : ApiController
    {
        string msSQL = string.Empty;
        int mnResult;
        dbconn dbconn = new dbconn();

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrMstPricesegmentSummary objsales = new DaSmrMstPricesegmentSummary();

        // Price Segment Summary

        [ActionName("GetSmrMstPricesegmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetSmrMstPricesegmentSummary()
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetSmrMstPricesegmentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostPriceSegment")]
        [HttpPost]
        public HttpResponseMessage PostPriceSegment(Getpricesegment_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostPostPriceSegment(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdatedPriceSegment")]
        [HttpPost]
        public HttpResponseMessage UpdatedPriceSegment(Getpricesegment_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaUpdatePriceSegment(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deletePriceSegmentSummary")]
        [HttpGet]
        public HttpResponseMessage deletePriceSegmentSummary(string pricesegment_gid)
        {
            Getpricesegment_list objresult = new Getpricesegment_list();
            objsales.DadeletePriceSegmentSummary(pricesegment_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetPricesegmentgrid")]
        [HttpGet]
        public HttpResponseMessage GetPricesegmentgrid(string pricesegment_gid)
        {

            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetPricesegmentgrid(pricesegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Product Group Dropdown
        [ActionName("GetSmrGroupDtl")]
        [HttpGet]
        public HttpResponseMessage GetSmrGroupDtl()
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetSmrGroupDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Product Name Dropdown
        [ActionName("GetSmrProductDtl")]
        [HttpGet]
        public HttpResponseMessage GetSmrProductDtl()
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetSmrProductDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Product Unit Dropdown
        [ActionName("GetSmrUnitDtl")]
        [HttpGet]
        public HttpResponseMessage GetSmrUnitDtl()
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetSmrUnitDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Product Assign Summary
        [ActionName("GetSmrMstProductAssignSummary")]
        [HttpGet]
        public HttpResponseMessage GetSmrMstProductAssignSummary(String pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetSmrMstProductAssignSummary(values, pricesegment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSmrMstProductunAssignSummary")]
        [HttpGet]
        public HttpResponseMessage GetSmrMstProductunAssignSummary(String pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetSmrMstProductunAssignSummary(values, pricesegment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UnAssignProduct")]
        [HttpPost]
        public HttpResponseMessage UnAssignProduct(postproductunassign_list values)
        {
            for (int i = 0; i < values.productunassign_list.ToArray().Length; i++)
            {
                msSQL = "delete from smr_trn_tpricesegment2product where " +
                    "pricesegment2product_gid='" + values.productunassign_list[i].pricesegment2product_gid + "'";
                mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Product Unassigned successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while unassigning product";
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //On change Product Name
        [ActionName("GetOnChangeProductName")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductName(string product_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetOnChangeProductName(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Post Product
        [ActionName("PostProduct")]
        [HttpPost]
        public HttpResponseMessage PostProduct(Getproduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostProduct(getsessionvalues.user_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Product Head

        [ActionName("GetSmrMstProductHead")]
        [HttpGet]
        public HttpResponseMessage GetSmrMstProductHead(string pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetSmrMstProductHead(values,pricesegment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeProduct")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProduct(string product_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetOnChangeProduct(values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Update

        [ActionName("GetUpdateProduct")]
        [HttpPost]
        public HttpResponseMessage GetUpdateProduct(Getproduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetUpdateProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Un Assign

        [ActionName("GetUnAssignProduct")]
        [HttpGet]
        public HttpResponseMessage GetUnAssignProduct(string product_gid)
        {
            GetProduct objresult = new GetProduct();
            objsales.DaGetUnAssignProduct(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetUnassignedlists")]
        [HttpGet]
        public HttpResponseMessage GetUnassignedlists(string pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary objresult = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetUnassignedlists(pricesegment_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetAssignedlists")]
        [HttpGet]
        public HttpResponseMessage GetAssignedlists(string pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary objresult = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetAssignedlists(pricesegment_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetUnassigned")]
        [HttpGet]
        public HttpResponseMessage GetUnassigned()
        {
            MdlSmrMstPricesegmentSummary objresult = new MdlSmrMstPricesegmentSummary();
            objsales.DaGetUnassigned( objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostAssignedlist")]
        [HttpPost]
        public HttpResponseMessage PostAssignedlist(campaignassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostAssignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("PostUnassignedlist")]
        [HttpPost]
        public HttpResponseMessage PostUnassignedlist(campaignassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostUnassignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("assignedpricesegmentproducts")]
        [HttpGet]
        public HttpResponseMessage assignedpricesegmentproducts(string pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.daassignedpricesegmentproducts(pricesegment_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("assignedpricesegmentscustomers")]
        [HttpGet]
        public HttpResponseMessage assignedpricesegmentscustomers(string pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.daassignedpricesegmentscustomers(pricesegment_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("assigncustomer")]
        [HttpGet]
        public HttpResponseMessage assigncustomer(string pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.Daassigncustomer(pricesegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("assigncustomerheadercount")]
        [HttpGet]
        public HttpResponseMessage assigncustomerheadercount(string pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.Daassigncustomerheadercount(pricesegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("unassigncustomer")]
        [HttpGet]
        public HttpResponseMessage unassigncustomer(string pricesegment_gid)
        {
            MdlSmrMstPricesegmentSummary values = new MdlSmrMstPricesegmentSummary();
            objsales.Daunassigncustomer(pricesegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("AssignSubmiCustomer")]
        [HttpPost]
        public HttpResponseMessage HolidayAssignSubmitemploye(customerpricesegmentsubmitlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPricesegmentAssignSubmitCustomer(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PriceSegmetUnAssignSubmit")]
        [HttpPost]
        public HttpResponseMessage HolidayUnAssignSubmit(customerpricesegmentunsubmitlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPriceSegmetUnAssignSubmit(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}
            
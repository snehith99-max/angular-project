using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;

namespace ems.crm.Controllers
{
    [RoutePrefix("api/ProductUnits")]
    [Authorize]
    public class ProductUnitsController : ApiController
    {
       
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaProductUnits objsales = new DaProductUnits();
        // Product Unit Summary
        [ActionName("GetMktProductUnitSummary")]
        [HttpGet]
        public HttpResponseMessage GetMktProductUnitSummary()
        {
            MdlProductUnits values = new MdlProductUnits();
            objsales.DaGetMktProductUnitSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostMktProductUnit")]
        [HttpPost]
        public HttpResponseMessage PostMktProductUnit(Mktproductunits_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostMktProductUnit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMktProductUnitSummarygrid")]
        [HttpGet]
        public HttpResponseMessage GetMktProductUnitSummarygrid(string productuomclass_gid)
        {

            MdlProductUnits values = new MdlProductUnits();
            objsales.DaGetMktProductUnitSummarygrid(productuomclass_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdatedMktProductunit")]
        [HttpPost]
        public HttpResponseMessage UpdatedMktProductunit(Mktproductunits_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaUpdatedMktProductunit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deleteMktProductunitSummary")]
        [HttpGet]
        public HttpResponseMessage deleteMktProductunitSummary(string productuomclass_gid)
        {
            Mktproductunits_list objresult = new Mktproductunits_list();
            objsales.DadeleteMktProductunitSummary(productuomclass_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetMktProductunits")]
        [HttpGet]
        public HttpResponseMessage GetMktProductunits(string productuomclass_gid)
        {
            MdlProductUnits values = new MdlProductUnits();
            objsales.DaGetMktProductunits(productuomclass_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostMktProductunits")]
        [HttpPost]
        public HttpResponseMessage PostMktProductunits(Mktproductunitgrid_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.PostMktProductunits(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("EditMktProductunits")]
        [HttpPost]
        public HttpResponseMessage EditMktProductunits(Mktproductunitgrid_listedit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaEditMktProductunits(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeletproductuniteMktsummary")]
        [HttpGet]
        public HttpResponseMessage DeletproductuniteMktsummary(string productuom_gid)
        {
            Mktproductunitgrid_listedit objresult = new Mktproductunitgrid_listedit();
            objsales.DaDeletproductuniteMktsummary(productuom_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}
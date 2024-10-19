using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.sales.Controllers
{
    [Authorize]
    [RoutePrefix("api/SmrProductHsnCode")]
    public class SmrProductHsnCodeController : ApiController
    {
        session_values objgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrProductHsnCode objhsn = new DaSmrProductHsnCode();

        
        [ActionName("ProductSummary")]
        [HttpGet]
        public HttpResponseMessage ProductSummary()
        {
            MdlSmrProductHsnCode values = new MdlSmrProductHsnCode();
            objhsn.DaProductSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateProductHSNCode")]
        [HttpGet]
        public HttpResponseMessage UpdateProductHSNCode(string product_gid, string product_hsncode, 
            string product_hsncode_desc,string product_hsngst)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgid.gettokenvalues(token);
            MdlSmrProductHsnCode values = new MdlSmrProductHsnCode();
            objhsn.DaUpdateProductHSNCode(getsessionvalues.employee_gid, product_gid, product_hsncode, product_hsncode_desc, product_hsngst,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
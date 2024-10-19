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
    [RoutePrefix("api/SmrMstSequenceCodeCustomizer")]
    [Authorize]
    public class SmrMstSequenceCodeCustomizerController : ApiController
    {
        session_values getsesssionvalues  = new session_values();
        logintoken objgid = new logintoken();
        DaSmrMstSequenceCodeCustomizer objseq = new DaSmrMstSequenceCodeCustomizer();

        [ActionName("GetSequenceCodeCustomizer")]
        [HttpGet]
        public HttpResponseMessage GetSequenceCodeCustomizer()
        {
            MdlSmrMstSequenceCodeCustomizer values = new MdlSmrMstSequenceCodeCustomizer();
            objseq.DaGetSequenceCodeCustomizer(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSequenceCodeCustomizerEdit")]
        [HttpGet]
        public HttpResponseMessage GetSequenceCodeCustomizerEdit(string sequencecodecustomizer_gid)
        {
            MdlSmrMstSequenceCodeCustomizer values = new MdlSmrMstSequenceCodeCustomizer();
            objseq.DaGetSequenceCodeCustomizerEdit(sequencecodecustomizer_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSequenceCodeCustomizerUpdate")]
        [HttpPost]
        public HttpResponseMessage GetSequenceCodeCustomizerUpdate(SequenceCodeSummary values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objgid = getsesssionvalues.gettokenvalues(token);
            objseq.DaGetSequenceCodeCustomizerUpdate(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
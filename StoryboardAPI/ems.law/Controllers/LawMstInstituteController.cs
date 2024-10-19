using ems.law.DataAccess;
using ems.law.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
namespace ems.law.Controllers
{
    [Authorize]
    [RoutePrefix("api/LawMstInstitute")]

    public class LawMstInstituteController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLawMstInstitute objinstitute = new DaLawMstInstitute();

        [ActionName("GetInstitutesummary")]
        [HttpGet]
        public HttpResponseMessage GetInstitutesummary()
        {
            MdlLawMstInstitute values = new MdlLawMstInstitute();
            objinstitute.DaGetInstitutesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInstitutecountry")]
        [HttpGet]
        public HttpResponseMessage GetInstitutecountry()
        {
            MdlLawMstInstitute values = new MdlLawMstInstitute();
            objinstitute.DaGetInstitutecountry(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostInstituteAdd")]
        [HttpPost]
        public HttpResponseMessage PostInstituteAdd(institute_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objinstitute.DaPostInstituteAdd(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostUpdateinstitute")]
        [HttpPost]
        public HttpResponseMessage PostUpdateinstitute(institute_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objinstitute.DaPostUpdateinstitute(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetinstituteInactive")]
        [HttpPost]
        public HttpResponseMessage GetinstituteInactive(Institutioninactivelog_data values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objinstitute.DaGetinstituteInactive(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetinstituteActive")]
        [HttpGet]
        public HttpResponseMessage GetinstituteActive(string institute_gid)
        {
            MdlLawMstInstitute values = new MdlLawMstInstitute();
            objinstitute.DaGetinstituteActive(institute_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postinstituteresetpassword")]
        [HttpPost]
        public HttpResponseMessage Postinstituteresetpassword(institute_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objinstitute.DaPostinstituteresetpassword(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetInstituteEditSummary")]
        [HttpGet]
        public HttpResponseMessage GetInstituteEditSummary(string institute_gid)
        {
            MdlLawMstInstitute objresult = new MdlLawMstInstitute();
            objinstitute.DaGetInstituteEditSummary(institute_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("InstitutionInactiveHistory")]
        [HttpGet]
        public HttpResponseMessage InstitutionInactiveHistory(string institute_gid)
        {
            MdlLawMstInstitute values = new MdlLawMstInstitute();
            objinstitute.DaInstitutionInactiveHistory(institute_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }

}

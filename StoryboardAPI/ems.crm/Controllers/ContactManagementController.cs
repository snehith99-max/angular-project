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
using static ems.crm.Models.MdlContactManagement;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/ContactManagement")]
    public class ContactManagementController : ApiController
    {
        DaContactManagement objDacontact = new DaContactManagement();
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();

        [ActionName("ContactAdd")]
        [HttpPost]
        public HttpResponseMessage PostContactAdd(contact objcontact_list)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacontact.DaContactAdd(objcontact_list, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objcontact_list);
        }

        [ActionName("ContactSummary")]
        [HttpGet]
        public HttpResponseMessage GetContactSummary()
        {
            contact_list objcontact_list = new contact_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacontact.DacontactSummary(objcontact_list);
            return Request.CreateResponse(HttpStatusCode.OK, objcontact_list);
        }

        [ActionName("ContactIndividualSummary")]
        [HttpGet]
        public HttpResponseMessage ContactIndividualSummary()
        {
            contact_list objcontact_list = new contact_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacontact.DaContactIndividualSummary(objcontact_list);
            return Request.CreateResponse(HttpStatusCode.OK, objcontact_list);
        }

        [ActionName("ContactCorporateSummary")]
        [HttpGet]
        public HttpResponseMessage ContactCorporateSummary()
        {
            contact_list objcontact_list = new contact_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacontact.DaContactCorporateSummary(objcontact_list);
            return Request.CreateResponse(HttpStatusCode.OK, objcontact_list);
        }

        [ActionName("ContactEditView")]
        [HttpGet]
        public HttpResponseMessage GetContactEditView(string leadbank_gid)
        {
            contact_list objcontact = new contact_list();
            objDacontact.DaContactEditView(objcontact, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objcontact);
        }

        [ActionName("ContactUpdate")]
        [HttpPost]
        public HttpResponseMessage PostContactUpdate(contact objcontact_list)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacontact.DaContactUpdate(getsessionvalues.employee_gid, objcontact_list);
            return Request.CreateResponse(HttpStatusCode.OK, objcontact_list);
        }

        [ActionName("postdocument")]
        [HttpPost]
        public HttpResponseMessage postdocument()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            GeneralDocumentList values = new GeneralDocumentList();
            objDacontact.Dapostdocument(httpRequest, values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteContact")]
        [HttpGet]
        public HttpResponseMessage DeleteContact(string contact_gid, string contact_type)
        {
            result objResult = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacontact.DaDeleteContact(contact_gid, contact_type, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);

        }
        [ActionName("Constitutiondropdown")]
        [HttpGet]
        public HttpResponseMessage Constitutiondropdown()
        {
            contact_list values = new contact_list();
            objDacontact.DaConstitutiondropdown(values); ;
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
    }
}
using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/OfferLetter")]
    public class OfferLetterController :ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOfferLetter objdaoffer = new DaOfferLetter();

        [ActionName("OfferLetterSummary")]
        [HttpGet]
        public HttpResponseMessage OfferLetterSummary()
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaOfferLetterSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Addofferletter")]
        [HttpPost]
        public HttpResponseMessage Addofferletter(AddOfferletter_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoffer.DaAddofferletter(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

       

        [ActionName("Getconfirmationdetails")]
        [HttpGet]
        public HttpResponseMessage Getconfirmationdetails(string offer_gid)
        {
            AddOfferletter_list values = new AddOfferletter_list();
            values = objdaoffer.DaGetconfirmationdetails(offer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Getgradetemplatedropdown")]
        [HttpGet]

        public HttpResponseMessage Getgradetemplatedropdown()
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaGetgradetemplatedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Additionsummary")]
        [HttpGet]
        public HttpResponseMessage Additionsummary(string salarygradetemplate_gid, string salarygradetype)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaAdditionsummary(salarygradetemplate_gid, salarygradetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Deductionsummary")]
        [HttpGet]
        public HttpResponseMessage Deductionsummary(string salarygradetemplate_gid, string salarygradetype)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaDeductionsummary(salarygradetemplate_gid, salarygradetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Otherssummary")]
        [HttpGet]
        public HttpResponseMessage Otherssummary(string salarygradetemplate_gid, string salarygradetype)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaOtherssummary(salarygradetemplate_gid, salarygradetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAdditionalcomponentname")]
        [HttpGet]
        public HttpResponseMessage Getcomponentname(string componentgroup_name)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaGetAdditionalcomponentname(componentgroup_name, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getdeductioncomponentname")]
        [HttpGet]
        public HttpResponseMessage Getdeductioncomponentname(string componentgroup_name)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaGetdeductioncomponentname(componentgroup_name, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Additionsummarybind")]
        [HttpGet]
        public HttpResponseMessage Additionsummarybind(string salarygradetemplategid, string gross_salary)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaAdditionsummarybind(salarygradetemplategid, gross_salary, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deductionsummarybind")]
        [HttpGet]
        public HttpResponseMessage deductionsummarybind(string salarygradetemplategid, string gross_salary)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.Dadeductionsummarybind(salarygradetemplategid, gross_salary, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("otherssummarybind")]
        [HttpGet]
        public HttpResponseMessage otherssummarybind(string salarygradetemplategid, string gross_salary)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaOtherssummary(salarygradetemplategid, gross_salary, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Postemployeeconfirmation")]
        [HttpPost]
        public HttpResponseMessage Postemployeeconfirmation(EmployeedataConfirmation values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoffer.DaConfirmEmployee(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getEmployeelist")]
        [HttpGet]
        public HttpResponseMessage getEmployeelist()
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DagetEmployeelist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOfferletterpdf")]
        [HttpGet]
        public HttpResponseMessage GetOfferletterpdf(string offer_gid)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            var ls_response = new Dictionary<string, object>();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            ls_response = objdaoffer.GetOfferletterpdf(offer_gid, values,getsessionvalues.branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
    
        [ActionName("GetEditEmployeebind")]
        [HttpGet]
        public HttpResponseMessage GetEditEmployeebind(string employee_gid)
        {
            MdlOfferLetter values = new MdlOfferLetter();
            objdaoffer.DaGetEditEmployeebind(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}
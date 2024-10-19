using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{
    [RoutePrefix("api/SysMstTemplate")]
    [Authorize]
    public class SysMstTemplateController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysMstTemplate objDatemplate = new DaSysMstTemplate();

        [ActionName("GetTemplateSummary")]
        [HttpGet]
        public HttpResponseMessage GetTemplateSummary()
        {
            MdlSysMstTemplate values = new MdlSysMstTemplate();
            objDatemplate.DaTemplateSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTemplateType")]
        [HttpGet]
        public HttpResponseMessage GetTemplateType()
        {
            MdlSysMstTemplate values = new MdlSysMstTemplate();
            objDatemplate.DaGetTemplateType(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostTemplate")]
        [HttpPost]
        public HttpResponseMessage PostTemplate(MdlSysMstTemplatelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatemplate.DaPostTemplate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTemplateEditdata")]
        [HttpGet]
        public HttpResponseMessage GetTemplateEditdata(string template_gid)
        {
            MdlSysMstTemplate values = new MdlSysMstTemplate();
            objDatemplate.DaGetTemplateEditdata(values, template_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatedTemplate")]
        [HttpPost]
        public HttpResponseMessage UpdatedTemplate(MdlSysMstTemplateEditlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatemplate.DaUpdatedTemplate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GettemplateActive")]
        [HttpGet]
        public HttpResponseMessage GettemplateActive(string params_gid)
        {
            MdlSysMstTemplate values = new MdlSysMstTemplate();
            objDatemplate.DaTemplateActive(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GettemplateInactive")]
        [HttpGet]
        public HttpResponseMessage GettemplateInactive(string params_gid)
        {
            MdlSysMstTemplate values = new MdlSysMstTemplate();
            objDatemplate.DaTemplateInactive(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("DeleteTemplate")]
        [HttpGet]
        public HttpResponseMessage DeleteTemplate(string params_gid)
        {
            MdlSysMstTemplateEditlist objresult = new MdlSysMstTemplateEditlist();
            objDatemplate.DaDeleteTemplate(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Getassignmodule")]
        [HttpGet]
        public HttpResponseMessage Getassignmodule()
        {
            MdlSysMstTemplate values = new MdlSysMstTemplate();
            objDatemplate.DaGetassignmodule(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Asiignmoduletotemplate")]
        [HttpPost]
        public HttpResponseMessage Asiignmoduletotemplate(assignmodule2list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatemplate.DaAssignModuletotemplate(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Gettemplatedetails")]
        [HttpGet]
        public HttpResponseMessage Gettemplatedetails(string template_gid)
        {
            MdlSysMstTemplate values = new MdlSysMstTemplate();
            objDatemplate.DaGettemplatedetails(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}
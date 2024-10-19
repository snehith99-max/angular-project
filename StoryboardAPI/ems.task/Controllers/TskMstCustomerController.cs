using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.task.Models;

namespace ems.system.Controllers
{
    [RoutePrefix("api/TskMstCustomer")]
    [Authorize]
    public class TskMstCustomerController : ApiController
    {
        DaTskMstCustomer objDacustomer = new DaTskMstCustomer();
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        //Customer master
        [ActionName("CustomerSummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerSummary()
        {
            MdlTskMstCustomer values = new MdlTskMstCustomer();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaCustomerSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Customeradd")]
        [HttpPost]
        public HttpResponseMessage Customeradd(projectlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaCustomeradd(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Customeredit")]
        [HttpGet]
        public HttpResponseMessage Customeredit(string project_gid)
        {
            projectlist values = new projectlist();
            objDacustomer.DaCustomeredit(project_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("projectlist")]
        [HttpGet]
        public HttpResponseMessage projectlist()
        {
            MdlTskMstCustomer values = new MdlTskMstCustomer();
            objDacustomer.Daprojectlist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Updatecustomer")]
        [HttpPost]
        public HttpResponseMessage Updatecustomer(projectlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaUpdatecustomer(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Customerdelete")]
        [HttpGet]
        public HttpResponseMessage Customerdelete(string project_gid)
        {
            projectlist values = new projectlist();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaCustomerdelete(project_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("getassignteam")]
        [HttpPost]
        public HttpResponseMessage getassignteam(projectlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.Dagetassignteam(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUnassignedmodulelist")]
        [HttpGet]
        public HttpResponseMessage GetUnassignedmodulelist(string project_gid)
        {
            MdlTskMstCustomer objresult = new MdlTskMstCustomer();
            objDacustomer.DaGetUnassignedModulelist(project_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetAssignedmodulelist")]
        [HttpGet]
        public HttpResponseMessage GetAssignedmodulelist(string project_gid)
        {
            MdlTskMstCustomer objresult = new MdlTskMstCustomer();
            objDacustomer.DaGetAssignedmodulelist(project_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        //team master
        [ActionName("TeamSummary")]
        [HttpGet]
        public HttpResponseMessage TeamSummary()
        {
            mdltaskteam values = new mdltaskteam();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaTeamSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("TeamList")]
        [HttpGet]
        public HttpResponseMessage TeamList()
        {
            mdltaskteam values = new mdltaskteam();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaTeamList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("teamadd")]
        [HttpPost]
        public HttpResponseMessage teamadd(taskteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.Dateamadd(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("teamedit")]
        [HttpGet]
        public HttpResponseMessage teamedit(string team_gid)
        {
            taskteamlist values = new taskteamlist();
            objDacustomer.Dateamedit(team_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Updateteam")]
        [HttpPost]
        public HttpResponseMessage Updateteam(taskteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaUpdateteam(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("taskdelete")]
        [HttpGet]
        public HttpResponseMessage taskdelete(string team_gid)
        {
            projectlist values = new projectlist();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.Dataskdelete(team_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUnassignedmanagerlist")]
        [HttpGet]
        public HttpResponseMessage GetUnassignedmanagerlist(string team_gid)
        {
            taskteamlist objresult = new taskteamlist();
            objDacustomer.DaGetUnassignedmanagerlist(team_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetAssignedmanagerlist")]
        [HttpGet]
        public HttpResponseMessage GetAssignedmanagerlist(string team_gid)
        {
            taskteamlist objresult = new taskteamlist();
            objDacustomer.DaGetAssignedmanagerlist(team_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Addassignmanager")]
        [HttpPost]
        public HttpResponseMessage Addassignmanager(taskteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaAddassignmanager(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUnassignedlist")]
        [HttpGet]
        public HttpResponseMessage GetUnassignedlist(string team_gid)
        {
            taskteamlist objresult = new taskteamlist();
            objDacustomer.DaGetUnassignedlist(team_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetAssignedlist(string team_gid)
        {
            taskteamlist objresult = new taskteamlist();
            objDacustomer.DaGetAssignedlist(team_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Addassignmember")]
        [HttpPost]
        public HttpResponseMessage Addassignmember(taskteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacustomer.DaAddassignmember(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Manager
        [ActionName("transferlist")]
        [HttpGet]
        public HttpResponseMessage transferlist(string team_gid,string employee_gid)
        {
            taskteamlist values = new taskteamlist();
            objDacustomer.Datransferlist(team_gid, values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("managerlist")]
        [HttpGet]
        public HttpResponseMessage managerlist(string team_gid)
        {
            taskteamlist values = new taskteamlist();
            objDacustomer.Damanagerlist(team_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("editmanager")]
        [HttpGet]
        public HttpResponseMessage editmanager(string team_gid)
        {
            taskteamlist values = new taskteamlist();
            objDacustomer.Daeditmanager(team_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("conditionmember")]
        [HttpGet]
        public HttpResponseMessage conditionmember(string team_gid)
        {
            taskteamlist values = new taskteamlist();
            objDacustomer.Daconditionmember(team_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("levelone_menu")]
        [HttpGet]
        public HttpResponseMessage levelone_menu()
        {
            taskteamlist values = new taskteamlist();
            objDacustomer.Dalevelone_menu(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.system.DataAccess;
using ems.task.DataAccess;
using ems.task.Models;
using ems.storage.Models;

namespace ems.task.Controllers
{
    [RoutePrefix("api/TskTrnTaskManagement")]
    [Authorize]
    public class TskTrnTaskManagementController : ApiController
    {
        DaTskTrnTaskManagement objDatask = new DaTskTrnTaskManagement();
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        [ActionName("Taskadd")]
        [HttpPost]
        public HttpResponseMessage Taskadd(taskaddlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dataskadd(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("completed")]
        [HttpGet]
        public HttpResponseMessage completed()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dacompletedsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("showstoppersummary")]
        [HttpGet]
        public HttpResponseMessage showstoppersummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dashowstoppersummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("nonmandatorytsummary")]
        [HttpGet]
        public HttpResponseMessage nonmandatorytsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Danonmandatorysummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Mandatorysummary")]
        [HttpGet]
        public HttpResponseMessage Mandatorysummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Damandatorysummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("nicetohavesummary")]
        [HttpGet]
        public HttpResponseMessage nicetohavesummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Danicetohavesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("taskedit")]
        [HttpGet]
        public HttpResponseMessage taskedit(string task_gid)
        {
            taskaddlist values = new taskaddlist();
            objDatask.Dataskedit(task_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("tasklist")]
        [HttpGet]
        public HttpResponseMessage tasklist()
        {
            taskteamlist values = new taskteamlist();
            objDatask.Datasklist( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("taskbind")]
        [HttpGet]
        public HttpResponseMessage taskbind(string taskname)
        {
            taskaddlist values = new taskaddlist();
            objDatask.Dataskbind(taskname, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("check")]
        [HttpGet]
        public HttpResponseMessage check(string taskname)
        {
            taskaddlist values = new taskaddlist();
            objDatask.Dacheck(taskname, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("addtaskdocument")]
        [HttpPost]
        public HttpResponseMessage addtaskdocument()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDatask.Daaddtaskdocument(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("taskassigndelete")]
        [HttpGet]
        public HttpResponseMessage taskassigndelete(string task_gid)
        {
            taskaddlist values = new taskaddlist();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dataskassigndelete(task_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("customerlist")]
        [HttpGet]
        public HttpResponseMessage customerlist(string team_gid)
        {
            projectlist values = new projectlist();
            objDatask.Dacustomer_list(team_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Manager
        [ActionName("Managerpendingsummary")]
        [HttpGet]
        public HttpResponseMessage Managerpendingsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaManagerpendingsummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Assignedpendingsummary")]
        [HttpGet]
        public HttpResponseMessage Assignedpendingsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaManagerassignedsummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Assignedcompletedsummary")]
        [HttpGet]
        public HttpResponseMessage Assignedcompletedsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaManagercompletedsummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Assignedtestingsummary")]
        [HttpGet]
        public HttpResponseMessage Assignedtestingsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaManagerintestingsummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Assignedholdsummary")]
        [HttpGet]
        public HttpResponseMessage Assignedholdsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaManagerholdsummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateTask")]
        [HttpPost]
        public HttpResponseMessage UpdateTask(taskteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaUpdateTask(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Addmember")]
        [HttpPost]
        public HttpResponseMessage Addmember(taskteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaAddmember(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("reassign")]
        [HttpPost]
        public HttpResponseMessage reassign(taskteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dareassign(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Memberlivesummary")]
        [HttpGet]
        public HttpResponseMessage Memberlivesummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaMemberlivesummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Membertestsummary")]
        [HttpGet]
        public HttpResponseMessage Membertestsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaMembertestingsummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
           [ActionName("Memberpendingsummary")]
        [HttpGet]
        public HttpResponseMessage Memberpendingsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaMemberpendingsummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Memberprogresssummary")]
        [HttpGet]
        public HttpResponseMessage Memberprogresssummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaMemberprogresssummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Memberholdsummary")]
        [HttpGet]
        public HttpResponseMessage Memberholdsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaMemberHoldsummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUnassignedclientlist")]
        [HttpGet]
        public HttpResponseMessage GetUnassignedclientlist(string team_gid,string task_gid)
        {
            taskteamlist objresult = new taskteamlist();
            objDatask.DaGetUnassignedclientlist(team_gid, task_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetAssignedclientlist")]
        [HttpGet]
        public HttpResponseMessage GetAssignedlist(string team_gid,string task_gid)
        {
            taskteamlist objresult = new taskteamlist();
            objDatask.DaGetAssignedclientlist(team_gid, task_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("getclient")]
        [HttpPost]
        public HttpResponseMessage getclient(projectlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dagetclient(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("count")]
        [HttpGet]
        public HttpResponseMessage count()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dacount(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("managerallcount")]
        [HttpGet]
        public HttpResponseMessage managerallcount()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Damanagerallcount(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("managercount")]
        [HttpGet]
        public HttpResponseMessage managercount()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Damanagercount(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("conditionclient")]
        [HttpGet]
        public HttpResponseMessage conditionclient(string task_gid)
        {
            taskteamlist values = new taskteamlist();
            objDatask.Daconditionclient(task_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("modulelist")]
        [HttpGet]
        public HttpResponseMessage modulelist()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            objDatask.Damodulelist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetHoldtask")]
        [HttpGet]
        public HttpResponseMessage GetHoldAllocation(string task_gid, string taskhold_reason,string status)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            taskteamlist values = new taskteamlist();
            objDatask.DaGetHoldtask(task_gid, taskhold_reason, status,getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getrevoketask")]
        [HttpPost]
        public HttpResponseMessage Getrevoketask(taskteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaGetrevoketask(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("leveltwo_menu")]
        [HttpGet]
        public HttpResponseMessage leveltwo_menu(string module_gid)
        {
            taskteamlist values = new taskteamlist();
            objDatask.Daleveltwo_menu(module_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("task_list")]
        [HttpGet]
        public HttpResponseMessage task_list()
        {
            taskteamlist values = new taskteamlist();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Datask_list(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Tasksheetadd")]
        [HttpPost]
        public HttpResponseMessage Tasksheetadd(taskaddlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaTasksheetadd(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Tasksheetupdate")]
        [HttpPost]
        public HttpResponseMessage Tasksheetupdate(taskaddlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Daupdatetasksheet(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("tasksheetsummary")]
        [HttpGet]
        public HttpResponseMessage tasksheetsummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Datasksheetsummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("tasksheethistorysummary")]
        [HttpGet]
        public HttpResponseMessage tasksheethistorysummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Datasksheethistorysummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("taskwisesummary")]
        [HttpGet]
        public HttpResponseMessage taskwisesummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dataskwisesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deploywisesummary")]
        [HttpGet]
        public HttpResponseMessage deploywisesummary()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dadeploywisesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("teamwisesummary")]
        [HttpGet]
        public HttpResponseMessage teamwisesummary(string module_gid)
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dateamwisesummary(values, module_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("gettask")]
        [HttpGet]
        public HttpResponseMessage gettask(string task_date)
        {
            MdlTskTrnTaskManagement objresult = new MdlTskTrnTaskManagement();
            objDatask.Dagettask(objresult, task_date);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("clientview")]
        [HttpGet]
        public HttpResponseMessage clientview(string task_gid)
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            objDatask.Daclientview(task_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("teamlistsummary")]
        [HttpGet]
        public HttpResponseMessage teamlistsummary()
        {
            mdltaskteam values = new mdltaskteam();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.DaTeamlistSummary(values,getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("detailsview")]
        [HttpGet]
        public HttpResponseMessage detailsview(string employee_gid,string module_gid)
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            objDatask.Dadetailsview(employee_gid, module_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("allmemebrview")]
        [HttpGet]
        public HttpResponseMessage allmemebrview(string assigned_member_gid)
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            objDatask.Daallmemberview(assigned_member_gid ,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("allmember")]
        [HttpGet]
        public HttpResponseMessage allmember()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            objDatask.Daallmember(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("allmembers")]
        [HttpGet]
        public HttpResponseMessage allmembers()
        {
            MdlTskTrnTaskManagement values = new MdlTskTrnTaskManagement();
            objDatask.Daallsmember(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DownloadDocument")]
        [HttpPost]
        public HttpResponseMessage download_Collateraldoc(MdlTelecallingDownload values)
        {
            var ls_response = new Dictionary<string, object>();
            Fnazurestorage objFnazurestorage = new Fnazurestorage();
            //values.file_path = objFnazurestorage.DecryptData(values.file_path);
            ls_response = objFnazurestorage.FnDownloadDocument(values.file_path, values.file_name);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
        [ActionName("linechartcount")]
        [HttpGet]
        public HttpResponseMessage linechartcount(string MonthYear)
        {
            taskteamlist values = new taskteamlist();
            objDatask.Dalinechartcount(MonthYear,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("graphchartcount")]
        [HttpGet]
        public HttpResponseMessage graphchartcount(string module_gid)
        {
            taskteamlist values = new taskteamlist();
            objDatask.Dagraphchartcount(module_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Deployeadd")]
        [HttpPost]
        public HttpResponseMessage Deployeadd(tracker_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dadeployadd(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("addscript")]
        [HttpPost]
        public HttpResponseMessage addscript()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDatask.Dascriptattach(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("deploysummary")]
        [HttpGet]
        public HttpResponseMessage deploysummary()
        {
            tracker_list values = new tracker_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dadeploysummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deployview")]
        [HttpGet]
        public HttpResponseMessage deployview(string deployment_trackergid)
        {
            tracker_list values = new tracker_list();
            objDatask.Dadeployview(deployment_trackergid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Deployeupdate")]
        [HttpPost]
        public HttpResponseMessage Deployeupdate(tracker_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dadeploy(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("versioncheck")]
        [HttpGet]
        public HttpResponseMessage versioncheck(string version_number)
        {
            taskaddlist values = new taskaddlist();
            objDatask.Daversioncheck(version_number, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("versioneditcheck")]
        [HttpGet]
        public HttpResponseMessage versioneditcheck(string version_number,string deployment_trackergid)
        {
            taskaddlist values = new taskaddlist();
            objDatask.Daversioneditcheck(version_number, deployment_trackergid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("scriptdelete")]
        [HttpGet]
        public HttpResponseMessage scriptdelete(string script_gid)
        {
            projectlist values = new projectlist();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDatask.Dascriptdelete(script_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
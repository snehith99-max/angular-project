import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SysMstEmployeeAddComponent } from './component/sys-mst-employee-add/sys-mst-employee-add.component';
import { SysMstMenuMappingComponent } from './component/sys-mst-menu-mapping/sys-mst-menu-mapping.component';
import { SystemDashboardComponent } from './component/system-dashboard/system-dashboard.component';
import { SysMstEmployeePendingSummaryComponent } from './component/sys-mst-employee-pending-summary/sys-mst-employee-pending-summary.component';
import { SysMstEmployeeEditComponent } from './component/sys-mst-employee-edit/sys-mst-employee-edit.component';
import { SysMstEmployeeViewComponent } from './component/sys-mst-employee-view/sys-mst-employee-view.component';
import { SysMstEmployeeSummaryComponent } from './component/sys-mst-employee-summary/sys-mst-employee-summary.component';
import { SysMstEntitySummaryComponent } from './component/sys-mst-entity-summary/sys-mst-entity-summary.component';
import { SysRptEmployeereportComponent } from './component/sys-rpt-employeereport/sys-rpt-employeereport.component';
import { SysMstBranchComponent } from './component/sys-mst-branch/sys-mst-branch.component';
import { SysMstDepartmentComponent } from './component/sys-mst-department/sys-mst-department.component';
import { SysMstDesignationComponent } from './component/sys-mst-designation/sys-mst-designation.component';
import { SysMstUserprofileComponent } from './component/sys-mst-userprofile/sys-mst-userprofile.component';
import { SysMstModulemanagerComponent } from './component/sys-mst-modulemanager/sys-mst-modulemanager.component';
import { SysMstAssignemployeeComponent } from './component/sys-mst-assignemployee/sys-mst-assignemployee.component';
import { SysMstTemplateSummaryComponent } from './component/sys-mst-template-summary/sys-mst-template-summary.component';
import { SysMstTemplateAddComponent } from './component/sys-mst-template-add/sys-mst-template-add.component';
import { SysMstTemplateEditComponent } from './component/sys-mst-template-edit/sys-mst-template-edit.component';
import { SysMstApprovalhierarchyComponent } from './component/sys-mst-approvalhierarchy/sys-mst-approvalhierarchy.component';
import { SysMstOrganisationHierarchyComponent } from './component/sys-mst-organisation-hierarchy/sys-mst-organisation-hierarchy.component';
import { SysMstApprovalassignhierarchyComponent } from './component/sys-mst-approvalassignhierarchy/sys-mst-approvalassignhierarchy.component';
import { SysMstYearendactivitiesComponent } from './component/sys-mst-yearendactivities/sys-mst-yearendactivities.component';
import { SysMstJobtypeComponent } from './component/sys-mst-jobtype/sys-mst-jobtype.component';
import { SysMstCompanyComponent } from './component/sys-mst-company/sys-mst-company.component';
import { SysMstUserprivilegereportComponent } from './component/sys-mst-userprivilegereport/sys-mst-userprivilegereport.component';
import { SysMstErrormanagementComponent } from './component/sys-mst-errormanagement/sys-mst-errormanagement.component';
import { SysRptAuditreportComponent } from './component/sys-rpt-auditreport/sys-rpt-auditreport.component';
import { SysRptAudithistoryComponent } from './component/sys-rpt-audithistory/sys-rpt-audithistory.component';
import { SysMstUsergroupprivilegesummaryComponent } from './component/sys-mst-usergroupprivilegesummary/sys-mst-usergroupprivilegesummary.component';
import { SysMstUsergroupprivilegeaddComponent } from './component/sys-mst-usergroupprivilegeadd/sys-mst-usergroupprivilegeadd.component';
import { SysMstUserprivilegereprtComponent } from './component/sys-mst-userprivilegereprt/sys-mst-userprivilegereprt.component';
import { SysMstUsermanagementSummaryComponent } from './component/sys-mst-usermanagement-summary/sys-mst-usermanagement-summary.component';
import { SysMstUserAddComponent } from './component/sys-mst-user-add/sys-mst-user-add.component';
import { SysMstUserEditComponent } from './component/sys-mst-user-edit/sys-mst-user-edit.component';
import { SysMstUsergroupprivilegeeditComponent } from './component/sys-mst-usergroupprivilegeedit/sys-mst-usergroupprivilegeedit.component';

const routes: Routes = [
  { path: 'SysMstEmployeeAdd', component: SysMstEmployeeAddComponent },
  { path: 'SysMstMenuMapping', component: SysMstMenuMappingComponent },
  { path: 'SystemDashboard', component: SystemDashboardComponent },
  { path: 'SysMstEmployeePendingSummary', component: SysMstEmployeePendingSummaryComponent },
  { path: 'SysMstEmployeeEdit/:employee_gid', component: SysMstEmployeeEditComponent },
  { path: 'SysMstEmployeeView/:employee_gid', component: SysMstEmployeeViewComponent },
  { path: 'SysMstEmployeeSummary', component: SysMstEmployeeSummaryComponent },
  { path: 'SysMstEntitySummary', component: SysMstEntitySummaryComponent },
  { path: 'SysRptEmployeereport', component: SysRptEmployeereportComponent },
  { path: 'SysMstBranch', component: SysMstBranchComponent },
  { path: 'SysMstDepartment', component: SysMstDepartmentComponent },
  { path: 'SysMstDesignation', component: SysMstDesignationComponent },
  { path: 'SysMstUserProfile', component: SysMstUserprofileComponent },
  { path: 'SysMstModuleManager', component: SysMstModulemanagerComponent },
  { path: 'SysMstAssignemployee', component: SysMstAssignemployeeComponent },
  { path: 'SysMstTemplate', component: SysMstTemplateSummaryComponent },
  { path: 'SysMstTemplateAdd', component: SysMstTemplateAddComponent },
  { path: 'SysMstTemplateEdit/:template_gid', component: SysMstTemplateEditComponent },
  { path: 'SysMstApprovalhierarchy', component: SysMstApprovalhierarchyComponent },
  { path: 'SysMstOrganisationHierarchy', component: SysMstOrganisationHierarchyComponent },
  { path: 'SysMstAssignHierarchy/:module_gid', component: SysMstApprovalassignhierarchyComponent },
  { path: 'SysMstYearEndActivities', component: SysMstYearendactivitiesComponent },
  { path: 'SysMstJobtype', component: SysMstJobtypeComponent },
  { path: 'SysMstCompany', component: SysMstCompanyComponent },
  { path: 'SysMstUserprivilegereport', component: SysMstUserprivilegereportComponent },
  { path: 'SysMstErrormanagement', component: SysMstErrormanagementComponent },
  { path: 'SysRptAuditreport', component: SysRptAuditreportComponent },
  { path: 'SysRptAudithistory/:user_gid', component: SysRptAudithistoryComponent },
  { path: 'SysMstusergroupprivilegesummary', component: SysMstUsergroupprivilegesummaryComponent },
  { path: 'SysMstUsergroupprivilegeadd', component: SysMstUsergroupprivilegeaddComponent },
  { path: 'SysMstUserPriRep/:user_gid',component: SysMstUserprivilegereprtComponent},
  { path: 'SysMstUsermanagementSummary',component: SysMstUsermanagementSummaryComponent},
  { path: 'SysMstUserAdd',component: SysMstUserAddComponent},
  { path: 'SysMstUserEdit/:employee_gid',component: SysMstUserEditComponent},
  { path: 'SysMstUsergroupprivilegeedit/:usergrouptemplate_gid',component: SysMstUsergroupprivilegeeditComponent},
  

  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class EmsSystemRoutingModule { }
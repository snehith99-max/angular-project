import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';
import { CKEditorModule } from 'ng2-ckeditor';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { EmsHrmRoutingModule } from './ems.hrm-routing.module';
import { HrmTrnAdmincontrolComponent } from './component/hrm-trn-admincontrol/hrm-trn-admincontrol.component';
import { HrmMstEmployeeviewComponent } from './component/hrm-mst-employeeview/hrm-mst-employeeview.component';
import { HrmTrnEmployeeonboardComponent } from './component/hrm-trn-employeeonboard/hrm-trn-employeeonboard.component';
import { HrmTrnEmployeeonboardaddComponent } from './component/hrm-trn-employeeonboardadd/hrm-trn-employeeonboardadd.component';
import { HrmTrnEmployeeonboardEditComponent } from './component/hrm-trn-employeeonboard-edit/hrm-trn-employeeonboard-edit.component';
import { HrmTrnEmployeeonboardViewComponent } from './component/hrm-trn-employeeonboard-view/hrm-trn-employeeonboard-view.component';
import { HrmTrnEmployeeboardviewpendingComponent } from './component/hrm-trn-employeeboardviewpending/hrm-trn-employeeboardviewpending.component';
import { HrmMstSubfunctionComponent } from './component/hrm-mst-subfunction/hrm-mst-subfunction.component';
import { HrmMstBaselocationComponent } from './component/hrm-mst-baselocation/hrm-mst-baselocation.component';
import { HrmMstBloodgroupComponent } from './component/hrm-mst-bloodgroup/hrm-mst-bloodgroup.component';
import { HrmMstTeammasterComponent } from './component/hrm-mst-teammaster/hrm-mst-teammaster.component';
import { HrmMstUserprofileComponent } from './component/hrm-mst-userprofile/hrm-mst-userprofile.component';
import { HrmTrnEmployeeonboardViewCompletedComponent } from './component/hrm-trn-employeeonboard-view-completed/hrm-trn-employeeonboard-view-completed.component';
import { HrmMstRoleSummaryComponent } from './component/hrm-mst-role-summary/hrm-mst-role-summary.component';
import { HrmMstRoleAddComponent } from './component/hrm-mst-role-add/hrm-mst-role-add.component';
import { HrmMstRoleEditComponent } from './component/hrm-mst-role-edit/hrm-mst-role-edit.component';
import { HrmMstEntityComponent } from './component/hrm-mst-entity/hrm-mst-entity.component';
import { HrmMstBranchSummaryComponent } from './component/hrm-mst-branch-summary/hrm-mst-branch-summary.component';
import { HrmMstDepartmentSummaryComponent } from './component/hrm-mst-department-summary/hrm-mst-department-summary.component';
import { HrmMstDesignationComponent } from './component/hrm-mst-designation/hrm-mst-designation.component';
import { HrmMstHrdocumentComponent } from './component/hrm-mst-hrdocument/hrm-mst-hrdocument.component';
import { HrmMstTaskmasterComponent } from './component/hrm-mst-taskmaster/hrm-mst-taskmaster.component';
import { HrmMemberDashboardComponent } from './component/hrm-member-dashboard/hrm-member-dashboard.component';
import { HrmMemberMyleaveComponent } from './component/hrm-member-myleave/hrm-member-myleave.component';
import { HrmMemberApproveleaveComponent } from './component/hrm-member-approveleave/hrm-member-approveleave.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { HrmTrnProfileComponent } from './component/hrm-trn-profile/hrm-trn-profile.component';
import { HrmTrnIattendanceComponent } from './component/hrm-trn-iattendance/hrm-trn-iattendance.component';
import { HrmTrnProbationperiodComponent } from './component/hrm-trn-probationperiod/hrm-trn-probationperiod.component';
import { HrmTrnProbationhistoryComponent } from './component/hrm-trn-probationhistory/hrm-trn-probationhistory.component';
import { HrmTrnProbationleaveupdateComponent } from './component/hrm-trn-probationleaveupdate/hrm-trn-probationleaveupdate.component';
import { HrmTrnAppointmentorderComponent } from './component/hrm-trn-appointmentorder/hrm-trn-appointmentorder.component';
import { HrmTrnAppointmentordereditComponent } from './component/hrm-trn-appointmentorderedit/hrm-trn-appointmentorderedit.component';
import { HrmTrnAssetcustodianComponent } from './component/hrm-trn-assetcustodian/hrm-trn-assetcustodian.component';
import { HrmTrnAddassetcustodianComponent } from './component/hrm-trn-addassetcustodian/hrm-trn-addassetcustodian.component';
import { HrmTrnCompanypolicyComponent } from './component/hrm-trn-companypolicy/hrm-trn-companypolicy.component';
import { HrmMstEmployeeeditComponent } from './component/hrm-mst-employeeedit/hrm-mst-employeeedit.component';
import { HrmMstEmployeeaddComponent } from './component/hrm-mst-employeeadd/hrm-mst-employeeadd.component';
import { HrmTrnLeavebalancesummaryComponent } from './hrm-trn-leavebalancesummary/hrm-trn-leavebalancesummary.component';
import { HrmTrnLeavebalanceeditComponent } from './hrm-trn-leavebalanceedit/hrm-trn-leavebalanceedit.component';
import { HrmMemberMonthlyattendanceComponent } from './component/hrm-member-monthlyattendance/hrm-member-monthlyattendance.component';
import { HrmMemberOfficecalendarComponent } from './component/hrm-member-officecalendar/hrm-member-officecalendar.component';
import { HrmMstShifttypeComponent } from './component/hrm-mst-shifttype/hrm-mst-shifttype.component';
import { HrmMstAddshifttypeComponent } from './component/hrm-mst-addshifttype/hrm-mst-addshifttype.component';
import { HrmMstLeavetypeComponent } from './component/hrm-mst-leavetype/hrm-mst-leavetype.component';
import { HrmMstLeavegradeComponent } from './component/hrm-mst-leavegrade/hrm-mst-leavegrade.component';
import { HrmMstAddleavegradeComponent } from './component/hrm-mst-addleavegrade/hrm-mst-addleavegrade.component';
import { HrmMstAddholidaygradeComponent } from './component/hrm-mst-addholidaygrade/hrm-mst-addholidaygrade.component';
import { HrmTrnAddholidayasignComponent } from './component/hrm-trn-addholidayasign/hrm-trn-addholidayasign.component';
import { HrmTrnAttendancenonrollformanufacturingComponent } from './component/hrm-trn-attendancenonrollformanufacturing/hrm-trn-attendancenonrollformanufacturing.component';
import { HrmMstShiftassignmentComponent } from './component/hrm-mst-shiftassignment/hrm-mst-shiftassignment.component';
import { ShiftUnAssignmentComponent } from './component/shift-un-assignment/shift-un-assignment.component';
import { HrmTrnAddOfferLetterComponent } from './component/hrm-trn-add-offer-letter/hrm-trn-add-offer-letter.component';
import { HrmTrnOfferletterComponent } from './component/hrm-trn-offerletter/hrm-trn-offerletter.component';
import { HrmRptMonthlyattendanceReportComponent } from './component/hrm-rpt-monthlyattendance-report/hrm-rpt-monthlyattendance-report.component';
import { HrmTrnLeavegradeassign2employeeComponent } from './component/hrm-trn-leavegradeassign2employee/hrm-trn-leavegradeassign2employee.component';
import { HrmTrnLeavegradeunassign2employeeComponent } from './component/hrm-trn-leavegradeunassign2employee/hrm-trn-leavegradeunassign2employee.component';
import { HrmTrnManualregulationComponent } from './component/hrm-trn-manualregulation/hrm-trn-manualregulation.component';
import { HrmMstEmployeeassetlistComponent } from './component/hrm-mst-employeeassetlist/hrm-mst-employeeassetlist.component';
import { HrmMstHolidaygradeManagementComponent } from './component/hrm-mst-holidaygrade-management/hrm-mst-holidaygrade-management.component';
import { HrmMstHolidayassignemployeeComponent } from './component/hrm-mst-holidayassignemployee/hrm-mst-holidayassignemployee.component';
import { HrmMstUnassignemployeeComponent } from './component/hrm-mst-unassignemployee/hrm-mst-unassignemployee.component';
import { HrmDashboardComponent } from './component/hrm-dashboard/hrm-dashboard.component';
import { HrmAttendanceConfigurationComponent } from './component/hrm-attendance-configuration/hrm-attendance-configuration.component';
import { HrmMstEmployeeconfirmationComponent } from './component/hrm-mst-employeeconfirmation/hrm-mst-employeeconfirmation.component';
import { HrmTrnEmployeeexitmanagmentComponent } from './component/hrm-trn-employeeexitmanagment/hrm-trn-employeeexitmanagment.component';
import { HrmTrnEmployeeexitmanagement360Component } from './component/hrm-trn-employeeexitmanagement360/hrm-trn-employeeexitmanagement360.component';
import { HrmForm22Component } from './component/hrm-form22/hrm-form22.component';
import { HrmTrnStatutoryformsComponent } from './component/hrm-trn-statutoryforms/hrm-trn-statutoryforms.component';
import { HrmMstRolegradeComponent } from './component/hrm-mst-rolegrade/hrm-mst-rolegrade.component';
import { HrmMstRoledesignationComponent } from './component/hrm-mst-roledesignation/hrm-mst-roledesignation.component';
import { HrmTrnForm25componentComponent } from './component/hrm-trn-form25component/hrm-trn-form25component.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import { TableModule } from 'primeng/table';
import { HrmTrnDailyattendanceComponent } from './component/hrm-trn-dailyattendance/hrm-trn-dailyattendance.component';
import { HrmTrnAnnualform22Component } from './component/hrm-trn-annualform22/hrm-trn-annualform22.component';
import { HrmMstAssignedholidaygradeviewComponent } from './component/hrm-mst-assignedholidaygradeview/hrm-mst-assignedholidaygradeview.component';
import { HrmMstEditholidaygradeComponent } from './component/hrm-mst-editholidaygrade/hrm-mst-editholidaygrade.component';
import { HrmTrnEmployeeexitrequisitionsummaryComponent } from './component/hrm-trn-employeeexitrequisitionsummary/hrm-trn-employeeexitrequisitionsummary.component';
import { HrmTrnEmployeeexitrequisitionaddComponent } from './component/hrm-trn-employeeexitrequisitionadd/hrm-trn-employeeexitrequisitionadd.component';
import { HrmMstEditshifttypeComponent } from './component/hrm-mst-editshifttype/hrm-mst-editshifttype.component';
import { HrmMstWeekoffmanagementComponent } from './component/hrm-mst-weekoffmanagement/hrm-mst-weekoffmanagement.component';
import { HrmMstWeeklyoffComponent } from './component/hrm-mst-weeklyoff/hrm-mst-weeklyoff.component';
import { HrmMstWeekoffemployeesComponent } from './component/hrm-mst-weekoffemployees/hrm-mst-weekoffemployees.component';
import { HrmMstExperiencelettersummaryComponent } from './component/hrm-mst-experiencelettersummary/hrm-mst-experiencelettersummary.component';
import { HrmMstExperienceletterdirectaddComponent } from './component/hrm-mst-experienceletterdirectadd/hrm-mst-experienceletterdirectadd.component';
import { HrmMstReleivingletterComponent } from './component/hrm-mst-releivingletter/hrm-mst-releivingletter.component';
import { HrmMstAddrelievingletterComponent } from './component/hrm-mst-addrelievingletter/hrm-mst-addrelievingletter.component';
import { HrmTrnAppraisalmanagementComponent } from './component/hrm-trn-appraisalmanagement/hrm-trn-appraisalmanagement.component';
import { HrmTrnAppraisaladdComponent } from './component/hrm-trn-appraisaladd/hrm-trn-appraisaladd.component';
import { HrmTrnEmployee360Component } from './component/hrm-trn-employee360/hrm-trn-employee360.component';
import { HrmTrnAppraisal360Component } from './component/hrm-trn-appraisal360/hrm-trn-appraisal360.component';
import { WeekoffviewComponent } from './component/weekoffview/weekoffview.component';
import { HrmTrnLeavemanagesummaryComponent } from './component/hrm-trn-leavemanagesummary/hrm-trn-leavemanagesummary.component';
import { HrmTrnLeavemanageComponent } from './component/hrm-trn-leavemanage/hrm-trn-leavemanage.component';
import { HrmTrnPromotionsummaryComponent } from './component/hrm-trn-promotionsummary/hrm-trn-promotionsummary.component';
import { HrmTrnPromotionaddComponent } from './component/hrm-trn-promotionadd/hrm-trn-promotionadd.component';
import { HrmTrnPromotionhistoryComponent } from './component/hrm-trn-promotionhistory/hrm-trn-promotionhistory.component';
import { HrmTrnDepromotedetailssummaryComponent } from './component/hrm-trn-depromotedetailssummary/hrm-trn-depromotedetailssummary.component';
import { HrmTrnMemberdashboardComponent } from './component/hrm-trn-memberdashboard/hrm-trn-memberdashboard.component';
import { HrmRptEmployeeFormAComponent } from './component/hrm-rpt-employee-form-a/hrm-rpt-employee-form-a.component';
import { HrmTrnDepromoteaddComponent } from './component/hrm-trn-depromoteadd/hrm-trn-depromoteadd.component';
import { HrmTrnDePromotionhistoryComponent } from './component/hrm-trn-de-promotionhistory/hrm-trn-de-promotionhistory.component';




@NgModule({
  declarations: [
    HrmTrnAdmincontrolComponent,
    HrmMstRolegradeComponent,
    HrmMstRoledesignationComponent,
    HrmForm22Component,
    HrmMstEmployeeconfirmationComponent,
    HrmAttendanceConfigurationComponent,
    HrmTrnAssetcustodianComponent,
    HrmMstHolidaygradeManagementComponent,
    HrmTrnAddassetcustodianComponent,
    HrmTrnProfileComponent,
    HrmTrnCompanypolicyComponent,
    HrmTrnIattendanceComponent,
    HrmTrnProbationperiodComponent,
    HrmRptMonthlyattendanceReportComponent,
    HrmTrnProbationhistoryComponent,
    HrmTrnProbationleaveupdateComponent,
    HrmMstEmployeeviewComponent,
    HrmMstEmployeeeditComponent,
    HrmMstEmployeeaddComponent,
    HrmTrnAppointmentordereditComponent,
    HrmTrnAppointmentorderComponent,
    HrmTrnEmployeeonboardComponent,
    HrmTrnEmployeeonboardComponent,
    HrmTrnEmployeeonboardaddComponent,
    HrmTrnEmployeeonboardEditComponent,
    HrmTrnEmployeeonboardViewComponent,
    HrmTrnEmployeeboardviewpendingComponent,
    HrmMstSubfunctionComponent,
    HrmMstBaselocationComponent,
    HrmMstBloodgroupComponent,
    HrmMstTeammasterComponent,
    HrmMstUserprofileComponent,
    HrmTrnEmployeeonboardViewCompletedComponent,
    HrmMstRoleSummaryComponent,
    HrmMstRoleAddComponent,
    HrmMstRoleEditComponent,
    HrmMstEntityComponent,
    HrmMstBranchSummaryComponent,
    HrmMstDepartmentSummaryComponent,
    HrmMstDesignationComponent,
    HrmMstHrdocumentComponent,
    HrmMstTaskmasterComponent,
    HrmMemberDashboardComponent,
    HrmMemberMyleaveComponent,
    HrmMemberApproveleaveComponent,
    HrmTrnLeavebalancesummaryComponent,
    HrmTrnLeavebalanceeditComponent,
    HrmMemberMonthlyattendanceComponent,
    HrmMemberOfficecalendarComponent,
    HrmMstShifttypeComponent,
    HrmMstAddshifttypeComponent,
    HrmMstLeavetypeComponent,
    HrmMstLeavegradeComponent,
    HrmMstAddleavegradeComponent,
    HrmMstAddholidaygradeComponent,
    HrmTrnAddholidayasignComponent,
    HrmTrnAttendancenonrollformanufacturingComponent,
    HrmMstShiftassignmentComponent,
    ShiftUnAssignmentComponent,
    HrmTrnAddOfferLetterComponent,
    HrmTrnOfferletterComponent,
    HrmTrnLeavegradeassign2employeeComponent,
    HrmTrnLeavegradeunassign2employeeComponent,
    HrmTrnManualregulationComponent,
    HrmMstEmployeeassetlistComponent,
    HrmMstHolidayassignemployeeComponent,
    HrmMstUnassignemployeeComponent,
    HrmMstWeeklyoffComponent,
    HrmMstWeekoffmanagementComponent,
    HrmDashboardComponent,
    HrmForm22Component,
    HrmTrnStatutoryformsComponent,
    HrmDashboardComponent,
    HrmDashboardComponent,
    HrmTrnForm25componentComponent,
    HrmTrnDailyattendanceComponent,
    HrmTrnAnnualform22Component,
    HrmMstAssignedholidaygradeviewComponent,
    HrmMstEditholidaygradeComponent,
    HrmMstEditshifttypeComponent,
    HrmMstWeekoffemployeesComponent,
    HrmTrnEmployeeexitrequisitionsummaryComponent,
    HrmTrnEmployeeexitrequisitionaddComponent,
    HrmMstExperiencelettersummaryComponent,
    HrmMstExperienceletterdirectaddComponent,
    HrmMstReleivingletterComponent,
    HrmMstAddrelievingletterComponent,
    HrmTrnEmployeeexitmanagmentComponent,
    HrmTrnEmployeeexitmanagement360Component,
    HrmTrnAppraisalmanagementComponent,
    HrmTrnAppraisaladdComponent,
        HrmTrnEmployee360Component,
      HrmTrnAppraisal360Component,
      WeekoffviewComponent,
      HrmTrnLeavemanagesummaryComponent,
      HrmTrnLeavemanageComponent,
      HrmTrnPromotionsummaryComponent,
      HrmTrnPromotionaddComponent,
      HrmTrnPromotionhistoryComponent,
      HrmTrnDepromotedetailssummaryComponent,
      HrmTrnMemberdashboardComponent,
      HrmRptEmployeeFormAComponent,
      HrmTrnDepromoteaddComponent,
      HrmTrnDePromotionhistoryComponent
      
  ],


  imports: [
    CommonModule, EmsHrmRoutingModule, FormsModule, ReactiveFormsModule, EmsUtilitiesModule,
    NgApexchartsModule, DataTablesModule,
    NgSelectModule, AngularEditorModule,
    TabsModule, FullCalendarModule, CKEditorModule, TableModule
  ]
})

export class EmsHrmModule { }
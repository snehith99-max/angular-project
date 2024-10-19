import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { PayTrnEmployeebankdetailsComponent } from './component/pay-trn-employeebankdetails/pay-trn-employeebankdetails.component';
import { PayTrnBonussummaryComponent } from './component/pay-trn-bonussummary/pay-trn-bonussummary.component';
import { PayTrnEmployee2bonusComponent } from './component/pay-trn-employee2bonus/pay-trn-employee2bonus.component';
import { PayTrnEmployeeselectComponent } from './component/pay-trn-employeeselect/pay-trn-employeeselect.component';
import { PayTrnSalaryManagementComponent } from './component/pay-trn-salary-management/pay-trn-salary-management.component';
import { PayTrnSalarygradeTemplateComponent } from './component/pay-trn-salarygrade-template/pay-trn-salarygrade-template.component';
import { PayTrnAddsalarygradeTemplateComponent } from './component/pay-trn-addsalarygrade-template/pay-trn-addsalarygrade-template.component';
import { PayRptEmployeesalaryreportComponent } from './component/pay-rpt-employeesalaryreport/pay-rpt-employeesalaryreport.component';
import { PayMstSalarycomponentComponent } from './component/pay-mst-salarycomponent/pay-mst-salarycomponent.component';
import { PayMstSalarycomponentaddComponent } from './component/pay-mst-salarycomponentadd/pay-mst-salarycomponentadd.component';
import { PayMstSalarycomponenteditComponent } from './component/pay-mst-salarycomponentedit/pay-mst-salarycomponentedit.component';
import { PayMstLeavegenerateviewComponent } from './component/pay-mst-leavegenerateview/pay-mst-leavegenerateview.component';
import { PayMstEmployeetemplatesummaryComponent } from './component/pay-mst-employeetemplatesummary/pay-mst-employeetemplatesummary.component';
import { PayMstEmployee2gradeassignComponent } from './component/pay-mst-employee2gradeassign/pay-mst-employee2gradeassign.component';
import { PayTrnLoansummaryComponent } from './component/pay-trn-loansummary/pay-trn-loansummary.component';
import { PayTrnLoanaddComponent } from './component/pay-trn-loanadd/pay-trn-loanadd.component';
import { PayTrnLoaneditComponent } from './component/pay-trn-loanedit/pay-trn-loanedit.component';
import { PayTrnLoanviewComponent } from './component/pay-trn-loanview/pay-trn-loanview.component';
import { PayMstSalarycomponentgroupComponent } from './pay-mst-salarycomponentgroup/pay-mst-salarycomponentgroup.component';
import { PayTrnEditsalarygradeTemplateComponent } from './component/pay-trn-editsalarygrade-template/pay-trn-editsalarygrade-template.component';
import { PayTrnPayrunviewComponent } from './component/pay-trn-payrunview/pay-trn-payrunview.component';
import { PayTrnPaymentsummaryComponent } from './component/pay-trn-paymentsummary/pay-trn-paymentsummary.component';
import { PayTrnMakepaymentComponent } from './component/pay-trn-makepayment/pay-trn-makepayment.component';
import { PayTrnPaymenteditComponent } from './component/pay-trn-paymentedit/pay-trn-paymentedit.component';
import { PayRptPaymentreportsummaryComponent } from './component/pay-rpt-paymentreportsummary/pay-rpt-paymentreportsummary.component';
import { PayMstViewemployee2gradeassignComponent } from './component/pay-mst-viewemployee2gradeassign/pay-mst-viewemployee2gradeassign.component';
import { PayMstEditemployee2gradeassignComponent } from './component/pay-mst-editemployee2gradeassign/pay-mst-editemployee2gradeassign.component';
import { PayMstEmployeegradeconfirmComponent } from './component/pay-mst-employeegradeconfirm/pay-mst-employeegradeconfirm.component';
import { PayMstForm16employeedetailComponent } from './component/pay-mst-form16employeedetail/pay-mst-form16employeedetail.component';
import { PayMstAssessmentsummaryComponent } from './component/pay-mst-assessmentsummary/pay-mst-assessmentsummary.component';
import { PayTrnAssignemployee2form16Component } from './component/pay-trn-assignemployee2form16/pay-trn-assignemployee2form16.component';
import { PayTrnGenerateform16Component } from './component/pay-trn-generateform16/pay-trn-generateform16.component';
import { PayTrnBonuscreateComponent } from './component/pay-trn-bonuscreate/pay-trn-bonuscreate.component';
import { PayTrnGeneratedbonusviewComponent } from './component/pay-trn-generatedbonusview/pay-trn-generatedbonusview.component';
import { PaymstbankmasteraddComponent } from './component/paymstbankmasteradd/paymstbankmasteradd.component';
import { PayMstBankmasterComponent } from './component/pay-mst-bankmaster/pay-mst-bankmaster.component';
import { PaymstbankmastereditComponent } from './component/paymstbankmasteredit/paymstbankmasteredit.component';
import { PayTrnPfmanagementComponent } from './component/pay-trn-pfmanagement/pay-trn-pfmanagement.component';
import { PayTrnPfemployeeassignComponent } from './component/pay-trn-pfemployeeassign/pay-trn-pfemployeeassign.component';
import { PayRptPfEsiFormatComponent } from './component/pay-rpt-pf-esi-format/pay-rpt-pf-esi-format.component';
import { PayMstIncometaxratesComponent } from './component/pay-mst-incometaxrates/pay-mst-incometaxrates.component';
import { PfRptEsiReportComponent } from './component/pf-rpt-esi-report/pf-rpt-esi-report.component';
import { PayMstEmployeeAssessmentsummaryComponent } from './component/pay-mst-employee-assessmentsummary/pay-mst-employee-assessmentsummary.component';
import { PayMstEmployeeForm16detailsComponent } from './component/pay-mst-employee-form16details/pay-mst-employee-form16details.component';
import { PayMstTdsapprovalComponent } from './component/pay-mst-tdsapproval/pay-mst-tdsapproval.component';
import { PayMstTdsapprovalformdetailsComponent } from './component/pay-mst-tdsapprovalformdetails/pay-mst-tdsapprovalformdetails.component';
import { PayTrnSalarypaymentsummaryeditComponent } from './component/pay-trn-salarypaymentsummaryedit/pay-trn-salarypaymentsummaryedit.component';
import { PayMstEmpgradeconfirmComponent } from './component/pay-mst-empgradeconfirm/pay-mst-empgradeconfirm.component';
import { PayMstSalarycomponentviewComponent } from './component/pay-mst-salarycomponentview/pay-mst-salarycomponentview.component';
import { PayTrnPayruneditComponent } from './component/pay-trn-payrunedit/pay-trn-payrunedit.component';
import { PayRptPayrunmailComponent } from './component/pay-rpt-payrunmail/pay-rpt-payrunmail.component';
import { PayTrnPayrunleavereportComponent } from './component/pay-trn-payrunleavereport/pay-trn-payrunleavereport.component';
import { PayMstEditemp2salarygradeComponent } from './component/pay-mst-editemp2salarygrade/pay-mst-editemp2salarygrade.component';
import { PayMstEmployeewisepaymentComponent } from './component/pay-mst-employeewisepayment/pay-mst-employeewisepayment.component';
import { PayRptPfReportComponent } from './component/pay-rpt-pf-report/pay-rpt-pf-report.component';
// import { PayTrnSalarydashboardComponent } from './component/pay-trn-salarydashboard/pay-trn-salarydashboard.component';
// import { PayMstSalarygradeassignComponent } from './component/pay-mst-salarygradeassign/pay-mst-salarygradeassign.component';
// import { PayMstSalarygradeunassignComponent } from './component/pay-mst-salarygradeunassign/pay-mst-salarygradeunassign.component';
// import { PayMstAssignsalarygrade2employeeComponent } from './component/pay-mst-assignsalarygrade2employee/pay-mst-assignsalarygrade2employee.component';




const routes: Routes = [
  { path: 'PayTrnBonus', component: PayTrnBonussummaryComponent },
  { path: 'PayTrnEmployee2bonus/:bonus_gid', component: PayTrnEmployee2bonusComponent },
  { path: 'PayTrnEmployeeBankDetails', component: PayTrnEmployeebankdetailsComponent },
  { path: 'PayTrnEmployeeselect/:monthyear', component: PayTrnEmployeeselectComponent },
  { path: 'PayTrnLeavegenarate/:monthyear', component: PayMstLeavegenerateviewComponent },
  { path: 'PayTrnSalaryManagement', component: PayTrnSalaryManagementComponent },
  { path: 'PayTrnSalaryGradeTemplate', component: PayTrnSalarygradeTemplateComponent },
  { path: 'PayTrnAddSalaryGradeTemplate', component: PayTrnAddsalarygradeTemplateComponent },
  { path: 'PayTrnEditSalaryGradeTemplate/:salarygradetemplate_gid', component: PayTrnEditsalarygradeTemplateComponent },
  { path: 'PayRptEmployeesalaryreport', component: PayRptEmployeesalaryreportComponent },
  { path: 'PayMstSalaryComponent', component: PayMstSalarycomponentComponent },
  { path: 'PayMstSalarycomponentadd', component: PayMstSalarycomponentaddComponent },
  { path: 'PayMstSalarycomponentedit/:salarycomponent_gid', component: PayMstSalarycomponenteditComponent },
  { path: 'PayMstEmployeetemplatesummary', component: PayMstEmployeetemplatesummaryComponent },
  { path: 'PayMstEmployee2gradeassign', component: PayMstEmployee2gradeassignComponent },
  { path: 'PayTrnLoansummary', component: PayTrnLoansummaryComponent },
  { path: 'PayTrnLoanadd', component: PayTrnLoanaddComponent },
  { path: 'PayTrnLoanedit/:loan_gid', component: PayTrnLoaneditComponent },
  { path: 'PayTrnLoanview/:loan_gid', component: PayTrnLoanviewComponent },
  { path: 'PayMstComponentgroup', component: PayMstSalarycomponentgroupComponent },
  { path: 'PayTrnPayrunview/:monthyear', component: PayTrnPayrunviewComponent },
  { path: 'PayTrnPaymentsummary', component: PayTrnPaymentsummaryComponent },
  { path: 'PayTrnMakepayment/:monthyear', component: PayTrnMakepaymentComponent },
  { path: 'PayTrnPaymentedit/:payment_typepayment_datepayment_monthpayment_year', component: PayTrnPaymenteditComponent },
  { path: 'PayRptPaymentreportsummary', component: PayRptPaymentreportsummaryComponent },
  { path: 'PayTrnPaymentsummary', component: PayTrnPaymentsummaryComponent },
  { path: 'PayTrnMakepayment/:loan_gid', component: PayTrnMakepaymentComponent },
  // { path: 'PayTrnPaymentedit', component: PayTrnPaymenteditComponent },
  { path: 'PayTrnPaymentReportSummary', component: PayRptPaymentreportsummaryComponent },
  { path: 'PayMstEmployeegradeconfirm/:employee_gids', component: PayMstEmployeegradeconfirmComponent },
  { path: 'PayMstEditEmployeetosalarygrade/:employee2salarygradetemplate_gid', component: PayMstEditemployee2gradeassignComponent },
  { path: 'PayMstViewEmployeetosalarygrade/:employee2salarygradetemplate_gid', component: PayMstViewemployee2gradeassignComponent },
  { path: 'PayMstAssessmentsummary', component: PayMstAssessmentsummaryComponent },
  { path: 'PayTrnAssignemployee2form16/:assessment_gid', component: PayTrnAssignemployee2form16Component },
  { path: 'PayTrnGenerateform16/:assessment_gid', component: PayTrnGenerateform16Component },
  { path: 'paymstform16employeedetails/:employee_gidassessment_gid', component: PayMstForm16employeedetailComponent },
  { path: 'PayTrnBonuscreate', component: PayTrnBonuscreateComponent },
  { path: 'PayTrnGeneratedbonusview/:bonus_gid', component: PayTrnGeneratedbonusviewComponent },
  { path: 'PayMstBankmaster', component: PayMstBankmasterComponent },
  { path: 'Paymstbankmasteradd', component: PaymstbankmasteraddComponent },
  { path: 'Paymstbankmasteredit/:bank_gid', component: PaymstbankmastereditComponent },
  { path : 'PayTrnPfmanagement', component : PayTrnPfmanagementComponent},
  { path:'PayTrnPfemployeeassign',component:PayTrnPfemployeeassignComponent},
  { path: 'PayRptPfEsiFormat', component: PayRptPfEsiFormatComponent},
  { path: 'PayMstIncometaxrates', component:PayMstIncometaxratesComponent },
  { path:'PayTrnPfemployeeassign',component:PayTrnPfemployeeassignComponent},
  { path: 'PayRptPfEsiFormat', component: PayRptPfEsiFormatComponent},
  { path:  'PayTrnPfemployeeassign',component:PayTrnPfemployeeassignComponent},
  { path: 'PayRptPfEsiFormat', component: PayRptPfEsiFormatComponent},
  { path: 'PayRptESIReport/:monthsal_year', component: PfRptEsiReportComponent},
  { path: 'PayMstEmpAssessmentsummary', component: PayMstEmployeeAssessmentsummaryComponent },
  { path : 'PayTrnPfmanagement', component : PayTrnPfmanagementComponent},
  { path:  'PayTrnPfemployeeassign',component:PayTrnPfemployeeassignComponent},
  { path: 'PayRptPfEsiFormat', component: PayRptPfEsiFormatComponent},
  { path: 'PayRptESIReport/:monthsal_year', component: PfRptEsiReportComponent},
  { path: 'PayMstIncometaxrates', component:PayMstIncometaxratesComponent },
  { path: 'PayMstEmpAssessmentsummary', component: PayMstEmployeeAssessmentsummaryComponent },
  { path: 'PayTrnPfmanagement', component: PayTrnPfmanagementComponent },
  { path: 'PayTrnPfemployeeassign', component: PayTrnPfemployeeassignComponent },
  { path: 'PayRptPfEsiFormat', component: PayRptPfEsiFormatComponent },
  { path: 'PayRptPfReport/:monthsal_year', component: PayRptPfReportComponent },
  { path: 'PayRptESIReport/:monthsal_year', component: PfRptEsiReportComponent },
  { path: 'PayMstIncometaxrates', component: PayMstIncometaxratesComponent },
  { path: 'PayMstEmpAssessmentsummary', component: PayMstEmployeeAssessmentsummaryComponent },
  { path: 'PayMstEmployeeform16details/:assessment_gid', component: PayMstEmployeeForm16detailsComponent },
  { path: 'PayMstEmployeeform16details/:assessment_gid', component: PayMstEmployeeForm16detailsComponent },
  { path: 'payrptpayrunmail/:salary_gid/:month/:year/:to_emailid1', component: PayRptPayrunmailComponent},
  { path: 'PayMstTDSApproval', component: PayMstTdsapprovalComponent },
  { path: 'PayMstTDSApprovalFormDetails/:employee_gidassessment_gid', component: PayMstTdsapprovalformdetailsComponent},
  { path: 'PayMstEmpgradeconfirm/:employee_gid', component:PayMstEmpgradeconfirmComponent},
  { path: 'PayMstSalarycomponentview/:salarycomponent_gid', component: PayMstSalarycomponentviewComponent },
  { path: 'PayTrnPayrunedit/:salary_gid', component: PayTrnPayruneditComponent},
  { path: 'PayTrnLeaveReport/:monthyear', component: PayTrnPayrunleavereportComponent },
  { path: 'PayMstemp2salarygrade/:employee2salarygradetemplate_gid', component: PayMstEditemp2salarygradeComponent},
  { path: 'PayMstEmployeewisepayment', component: PayMstEmployeewisepaymentComponent},
  { path: 'PayTrnSalarypaymentedit/:payment_type/:payment_date/:payment_month/:payment_year', component: PayTrnSalarypaymentsummaryeditComponent},
  // { path: 'PayTrnSalaryDashboard', component: PayTrnSalarydashboardComponent},
  // { path: 'PayMstSalaryGradeAssign/:salarygradetemplate_gid', component: PayMstSalarygradeassignComponent},
  // { path: 'PayMstSalaryGradeUnAssign/:salarygradetemplate_gid', component: PayMstSalarygradeunassignComponent},
  // { path: 'PayMstSalaryGradeAssign2employee/:employee_gids/:salarygradetemplate_gid', component: PayMstAssignsalarygrade2employeeComponent}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class EmsPayrollRoutingModule { }

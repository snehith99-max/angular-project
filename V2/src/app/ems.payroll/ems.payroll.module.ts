import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NgxIntlTelInputModule } from 'ngx-intl-tel-input';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { EmsPayrollRoutingModule } from './ems.payroll-routing.module';
import { PayTrnBonussummaryComponent } from './component/pay-trn-bonussummary/pay-trn-bonussummary.component';
import { PayTrnEmployee2bonusComponent } from './component/pay-trn-employee2bonus/pay-trn-employee2bonus.component';
import { PayTrnEmployeebankdetailsComponent } from './component/pay-trn-employeebankdetails/pay-trn-employeebankdetails.component';
import { PayTrnEmployeeselectComponent } from './component/pay-trn-employeeselect/pay-trn-employeeselect.component';
import { PayTrnSalaryManagementComponent } from './component/pay-trn-salary-management/pay-trn-salary-management.component';
import { PayTrnLoansummaryComponent } from './component/pay-trn-loansummary/pay-trn-loansummary.component';
import { PayTrnLoanaddComponent } from './component/pay-trn-loanadd/pay-trn-loanadd.component';
import { PayTrnLoaneditComponent } from './component/pay-trn-loanedit/pay-trn-loanedit.component';
import { PayTrnLoanviewComponent } from './component/pay-trn-loanview/pay-trn-loanview.component';
import { PayMstLeavegenerateviewComponent } from './component/pay-mst-leavegenerateview/pay-mst-leavegenerateview.component';
import { PayMstEmployeetemplatesummaryComponent } from './component/pay-mst-employeetemplatesummary/pay-mst-employeetemplatesummary.component';
import { PayTrnSalarygradeTemplateComponent } from './component/pay-trn-salarygrade-template/pay-trn-salarygrade-template.component';
import { PayTrnAddsalarygradeTemplateComponent } from './component/pay-trn-addsalarygrade-template/pay-trn-addsalarygrade-template.component';
import { PayMstEmployee2gradeassignComponent } from './component/pay-mst-employee2gradeassign/pay-mst-employee2gradeassign.component';
import { PayRptEmployeesalaryreportComponent } from './component/pay-rpt-employeesalaryreport/pay-rpt-employeesalaryreport.component';
import { PayMstSalarycomponentComponent } from './component/pay-mst-salarycomponent/pay-mst-salarycomponent.component';
import { PayMstSalarycomponentaddComponent } from './component/pay-mst-salarycomponentadd/pay-mst-salarycomponentadd.component';
import { PayMstSalarycomponenteditComponent } from './component/pay-mst-salarycomponentedit/pay-mst-salarycomponentedit.component';
import { PayMstSalarycomponentgroupComponent } from './pay-mst-salarycomponentgroup/pay-mst-salarycomponentgroup.component';
import { PayTrnPayrunviewComponent } from './component/pay-trn-payrunview/pay-trn-payrunview.component';
import { PayTrnPaymentsummaryComponent } from './component/pay-trn-paymentsummary/pay-trn-paymentsummary.component';
import { PayTrnMakepaymentComponent } from './component/pay-trn-makepayment/pay-trn-makepayment.component';
import { PayTrnPaymenteditComponent } from './component/pay-trn-paymentedit/pay-trn-paymentedit.component';
import { PayTrnEditsalarygradeTemplateComponent } from './component/pay-trn-editsalarygrade-template/pay-trn-editsalarygrade-template.component';
import { PayRptPaymentreportsummaryComponent } from './component/pay-rpt-paymentreportsummary/pay-rpt-paymentreportsummary.component';
import { PayMstEmployeegradeconfirmComponent } from './component/pay-mst-employeegradeconfirm/pay-mst-employeegradeconfirm.component';
import { PayMstEditemployee2gradeassignComponent } from './component/pay-mst-editemployee2gradeassign/pay-mst-editemployee2gradeassign.component';
import { PayMstViewemployee2gradeassignComponent } from './component/pay-mst-viewemployee2gradeassign/pay-mst-viewemployee2gradeassign.component';
import { PayTrnBonuscreateComponent } from './component/pay-trn-bonuscreate/pay-trn-bonuscreate.component';
import { PayTrnGeneratedbonusviewComponent } from './component/pay-trn-generatedbonusview/pay-trn-generatedbonusview.component';
import { PayMstAssessmentsummaryComponent } from './component/pay-mst-assessmentsummary/pay-mst-assessmentsummary.component';
import { PayTrnAssignemployee2form16Component } from './component/pay-trn-assignemployee2form16/pay-trn-assignemployee2form16.component';
import { PayTrnGenerateform16Component } from './component/pay-trn-generateform16/pay-trn-generateform16.component';
import { PayMstForm16employeedetailComponent } from './component/pay-mst-form16employeedetail/pay-mst-form16employeedetail.component';
import { PaymstbankmasteraddComponent } from './component/paymstbankmasteradd/paymstbankmasteradd.component';
import { PayMstBankmasterComponent } from './component/pay-mst-bankmaster/pay-mst-bankmaster.component';
import { PaymstbankmastereditComponent } from './component/paymstbankmasteredit/paymstbankmasteredit.component';
import { PayTrnPfmanagementComponent } from './component/pay-trn-pfmanagement/pay-trn-pfmanagement.component';
import { PayTrnPfemployeeassignComponent } from './component/pay-trn-pfemployeeassign/pay-trn-pfemployeeassign.component';
import { PayRptPfEsiFormatComponent } from './component/pay-rpt-pf-esi-format/pay-rpt-pf-esi-format.component';
import { PfRptEsiReportComponent } from './component/pf-rpt-esi-report/pf-rpt-esi-report.component';
import { PayMstIncometaxratesComponent } from './component/pay-mst-incometaxrates/pay-mst-incometaxrates.component';
import { PayMstEmployeeAssessmentsummaryComponent } from './component/pay-mst-employee-assessmentsummary/pay-mst-employee-assessmentsummary.component';
import { PayMstEmployeeForm16detailsComponent } from './component/pay-mst-employee-form16details/pay-mst-employee-form16details.component';
import { PayMstTdsapprovalComponent } from './component/pay-mst-tdsapproval/pay-mst-tdsapproval.component';
import { PayMstTdsapprovalformdetailsComponent } from './component/pay-mst-tdsapprovalformdetails/pay-mst-tdsapprovalformdetails.component';
import { PayTrnSalarypaymentsummaryeditComponent } from './component/pay-trn-salarypaymentsummaryedit/pay-trn-salarypaymentsummaryedit.component';
import { PayMstEmpgradeconfirmComponent } from './component/pay-mst-empgradeconfirm/pay-mst-empgradeconfirm.component';
import { PayMstSalarycomponentviewComponent } from './component/pay-mst-salarycomponentview/pay-mst-salarycomponentview.component';
import { PayTrnPayruneditComponent } from './component/pay-trn-payrunedit/pay-trn-payrunedit.component';
import { TableModule } from 'primeng/table';
import { PayRptPayrunmailComponent } from './component/pay-rpt-payrunmail/pay-rpt-payrunmail.component';
import { PayTrnPayrunleavereportComponent } from './component/pay-trn-payrunleavereport/pay-trn-payrunleavereport.component';
import { PayMstEditemp2salarygradeComponent } from './component/pay-mst-editemp2salarygrade/pay-mst-editemp2salarygrade.component';
import { PayMstEmployeewisepaymentComponent } from './component/pay-mst-employeewisepayment/pay-mst-employeewisepayment.component';
import { PayRptPfReportComponent } from './component/pay-rpt-pf-report/pay-rpt-pf-report.component';
// import { PayTrnSalarydashboardComponent } from './component/pay-trn-salarydashboard/pay-trn-salarydashboard.component';
// import { PayMstSalarygradeassignComponent } from './component/pay-mst-salarygradeassign/pay-mst-salarygradeassign.component';
// import { PayMstSalarygradeunassignComponent } from './component/pay-mst-salarygradeunassign/pay-mst-salarygradeunassign.component';
// import { PayMstAssignsalarygrade2employeeComponent } from './component/pay-mst-assignsalarygrade2employee/pay-mst-assignsalarygrade2employee.component';


@NgModule({
  declarations: [    
    PayTrnEmployee2bonusComponent,
    PayMstEmpgradeconfirmComponent,
    PayMstEmployeeAssessmentsummaryComponent,
    PayMstEmployeeForm16detailsComponent,
    PaymstbankmasteraddComponent,
    PayMstBankmasterComponent,
    PaymstbankmastereditComponent,
    PayMstForm16employeedetailComponent,
    PayTrnEmployeebankdetailsComponent,
    PayTrnEmployeeselectComponent,
    PayTrnLoansummaryComponent,
    PayTrnLoanaddComponent,
    PayTrnLoanviewComponent,
    PayTrnLoaneditComponent,
    PayTrnSalaryManagementComponent,
    PayTrnSalarygradeTemplateComponent,
    PayTrnAddsalarygradeTemplateComponent,
    PayTrnSalaryManagementComponent,
    PayRptEmployeesalaryreportComponent,
    PayMstSalarycomponentComponent,
    PayMstSalarycomponentaddComponent,
    PayMstSalarycomponenteditComponent,
    PayTrnSalaryManagementComponent,
    PayMstLeavegenerateviewComponent,
    PayMstEmployeetemplatesummaryComponent,
    PayMstEmployee2gradeassignComponent,
    PayMstSalarycomponentgroupComponent,
    PayTrnPayrunviewComponent,
    PayTrnPaymentsummaryComponent,
    PayTrnMakepaymentComponent,
    PayTrnPaymenteditComponent,
    PayTrnEditsalarygradeTemplateComponent,
    PayRptPaymentreportsummaryComponent,
    PayMstEmployeegradeconfirmComponent,
    PayMstEditemployee2gradeassignComponent,
    PayMstViewemployee2gradeassignComponent,
    PayTrnBonuscreateComponent,
    PayTrnBonussummaryComponent,
    PayTrnGeneratedbonusviewComponent,
    PayMstAssessmentsummaryComponent,
    PayTrnAssignemployee2form16Component,
    PayTrnGenerateform16Component,
    PayTrnPfmanagementComponent,
    PayTrnPfemployeeassignComponent,
    PayRptPfEsiFormatComponent,
    PayMstIncometaxratesComponent,
    PayMstTdsapprovalComponent,
    PayMstTdsapprovalformdetailsComponent,
    PayTrnSalarypaymentsummaryeditComponent,
    PfRptEsiReportComponent,
    PayMstSalarycomponentviewComponent,
    PayTrnPayruneditComponent,
    PayRptPayrunmailComponent,
    PayTrnPayrunleavereportComponent,
    PayMstEditemp2salarygradeComponent,
    PayMstEmployeewisepaymentComponent,
    PayRptPfReportComponent
    // PayTrnSalarydashboardComponent,
    // PayMstSalarygradeassignComponent,
    // PayMstSalarygradeunassignComponent,
    // PayMstAssignsalarygrade2employeeComponent
  ],



   imports: [
    CommonModule,
    EmsPayrollRoutingModule,
    FormsModule, ReactiveFormsModule,EmsUtilitiesModule,
    NgApexchartsModule,DataTablesModule,
    NgSelectModule,AngularEditorModule,
    TabsModule,NgxIntlTelInputModule,TableModule
  ]
})

export class EmsPayrollModule {}

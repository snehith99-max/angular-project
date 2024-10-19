import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment.development';
import { ExcelService } from 'src/app/Service/excel.service';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { options } from '@fullcalendar/core/preact';
interface IPayrunReport {
  branch_name: string;
  department_name: string;
  branch_gid: string;
  department_gid: string;

}
@Component({
  selector: 'app-pay-rpt-employeesalaryreport',
  templateUrl: './pay-rpt-employeesalaryreport.component.html',
  styleUrls: ['./pay-rpt-employeesalaryreport.component.scss'],
  styles: [`
table thead th, 
.table tbody td { 
 position: relative; 
z-index: 0;
} 
.table thead th:last-child, 

.table tbody td:last-child { 
 position: sticky; 

right: 0; 
 z-index: 0; 

} 
.table td:last-child, 

.table th:last-child { 

padding-right: 50px; 

} 
.table.table-striped tbody tr:nth-child(odd) td:last-child { 

 background-color: #ffffff; 
  
  } 
  .table.table-striped tbody tr:nth-child(even) td:last-child { 
   background-color: #f2fafd; 

} 
`]
})
export class PayRptEmployeesalaryreportComponent {
  reactiveForm!: FormGroup;
  responsedata: any;
  dept_name: any;
  departmentlist: any[] = [];
  branch_name: any;
  department_name: any;
  branchlist: any[] = [];
  additionfilteroptions: any[] = [];
  deductionfilteroptions: any[] = [];
  othersfilteroptions: any[] = [];
  payrun_list: any[] = [];
  monthlist: any[] = [];
  addtionOptions: any[] = [];
  deductionOptions: any[] = [];
  payrunother_list: any[] = [];


  PayrunReport!: IPayrunReport;
  branch_gid: any;
  salary_gid: any;
  department_gid: any;
  month: any;
  company_code: any;
  payrunadd_list: any;
  initialpayrun_list: any;
  year: any;
  payrundeduction_list: any;

  salary_gidmonthyear: any;
  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService,
    private excelService: ExcelService, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.PayrunReport = {} as IPayrunReport;
  }

  ngOnInit(): void {


    const currentdate = new Date();
    const monthindex = currentdate.getMonth();
    const monthnames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    this.month = monthnames[monthindex];
    this.year = currentdate.getFullYear();

    this.Getpayruninitialsummary(this.month, this.year)

    this.reactiveForm = new FormGroup({

      branch_name: new FormControl(''),
      department_name: new FormControl(''),
      month: new FormControl(''),
      year: new FormControl(''),
      branch_gid: new FormControl(''),
      department_gid: new FormControl(''),
    });

    var api = 'PayRptPayrunSummary/GetBranchDtl'
    this.service.get(api).subscribe((result: any) => {
      this.branchlist = result.GetBranchDtl;
      //console.log(this.branchlist)
    });

    var api = 'PayRptPayrunSummary/GetDepartmentDtl'
    debugger;
    this.service.get(api).subscribe((result: any) => {
      this.departmentlist = result.GetDepartmentDtl;
      //console.log(this.branchlist)
    });

    const salary_gidmonthyear = this.route.snapshot.paramMap.get('salary_gidmonthyear');
    this.salary_gidmonthyear = salary_gidmonthyear;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salary_gidmonthyear, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
    debugger;
    const [month, year, salary_gid] = deencryptedParam.split('+');
    this.month = month;
    this.year = year;
    this.salary_gid = salary_gid;


    this.PrintPDF(this.salary_gid, this.month, this.year);

  }



  Getpayruninitialsummary(month: any, year: any) {
    debugger
    const api = 'PayRptPayrunSummary/Getpayruninitialsummary';
    let params = {
      month: month,
      year: year
    }
    this.service.getparams(api, params).subscribe((result: any) => {
      this.payrun_list = result.payrunlist;
      this.addtionOptions = result.addsummary;
      this.deductionOptions = result.dedsummary;
      this.payrunother_list = result.othersummary;

      setTimeout(() => {
        $('#payrunlist').DataTable();
      }, 1);
    });
    console.log(this.payrun_list)
  }
  filterAdditionalOptions(salary_gid: any): any[] {
    return this.addtionOptions.filter(option => option.salary_gid === salary_gid);
  }
  filterDeductionOptions(salary_gid: any): any[] {
    return this.deductionOptions.filter(option => option.salary_gid === salary_gid);
  }
  filterOthersOptions(salary_gid: any): any[] {
    return this.payrunother_list.filter(option => option.salary_gid === salary_gid);
  }
  GetpayrunSummary() {
    const selectedBranch = this.reactiveForm.value.branch_name || 'null';
    const selectedDepartment = this.reactiveForm.value.department_name || 'null';
    const selectmonth = this.reactiveForm.value.month || 'null';
    const selectyear = this.reactiveForm.value.year || 'null';

    for (const control of Object.keys(this.reactiveForm.controls)) {
      this.reactiveForm.controls[control].markAsTouched();
    }
    const params = {
      branch_gid: selectedBranch,
      department_gid: selectedDepartment,
      month: selectmonth,
      year: selectyear
    };

    const url2 = 'PayRptPayrunSummary/Getpayrunsummary';
    this.NgxSpinnerService.show();

    this.service.getparams(url2, params).subscribe((result) => {
      this.NgxSpinnerService.hide();

      this.responsedata = result;
      this.payrun_list = this.responsedata.payrunlist;
      this.addtionOptions = this.responsedata.addsummary;
      this.deductionOptions = this.responsedata.dedsummary;
      this.payrunother_list = this.responsedata.othersummary;
      setTimeout(() => {
        $('#payrun_list').DataTable();
      }, 1);
    });
  }

  openModalpdf() {
  }
  PrintPDF(salary_gid: string, month: string, year: string) {
    debugger
    const api = 'PayRptPayrunSummary/GetPayslipRpt';
    this.NgxSpinnerService.show()
    let param = {
      salary_gid: salary_gid,
      month: month,
      year: year
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
  exportExcel() {
    debugger
    if(this.addtionOptions != null){
    for (const option of this.addtionOptions) {
      for (const item of this.payrun_list) {
        if (item["salary_gid"] == option["salary_gid"]) {
          item["addearned_amount"] = (item["addearned_amount"] ? item["addearned_amount"] + " || " : "") + option["earned_amount"];
          if (!item["addsalarycomponent_name"]) {
            item["addsalarycomponent_name"] = option["salarycomponent_name"];
          } else {
            item["addsalarycomponent_name"] += " || " + option["salarycomponent_name"];
          }
        }
      }
    }
  }
  if(this.deductionOptions !=null){
    for (const option of this.deductionOptions) {
      for (const item of this.payrun_list) {
        if (item["salary_gid"] == option["salary_gid"]) {
          item["dedearned_amount"] = (item["dedearned_amount"] ? item["dedearned_amount"] + " || " : "") + option["earned_amount"];
          if (!item["dedsalarycomponent_name"]) {
            item["dedsalarycomponent_name"] = option["salarycomponent_name"];
          } else {
            item["dedsalarycomponent_name"] += " || " + option["salarycomponent_name"];
          }
        }
      }
    }
  }
  if(this.payrunother_list !=null){
  for (const option of this.payrunother_list) {
    for (const item of this.payrun_list) {
      item["otherearned_amount"] = (item["otherearned_amount"] ? item["otherearned_amount"] + " || " : "") + option["earned_amount"];
      if (!item["othersalarycomponent_name"]) {
        item["othersalarycomponent_name"] = option["salarycomponent_name"];
      } else {
        item["othersalarycomponent_name"] += " || " + option["salarycomponent_name"];
      }
    }
  }
}

    const Employeesalaryreport = this.payrun_list.map(item => ({
      BranchName: item.branch_name || '',
      Department: item.department || '',
      EmployeeCode: item.user_code || '',
      EmployeeName: item.employee_name || '',
      LeaveTaken: item.leave_taken || '',
      LOPDays: item.lop || '',
      TotalDays: item.month_workingdays || '',
      AdditionalComponentName: item.addsalarycomponent_name || '',
      AdditionalEarnedAmount: item.addearned_amount || '',
      WorkingDays: item.actual_month_workingdays || '',
      PublicHolidays: item.public_holidays || '',
      BasicSalary: item.basic_salary || '',
      EarnedBasicSalary: item.earned_basic_salary || '',
      GrossSalary: item.gross_salary || '',
      DeductionComponentName: item.dedsalarycomponent_name || '',
      DeductionalEarnedAmount: item.dedearned_amount || '',
      OtherComponentName:item.othersalarycomponent_name || '',
      OtherEarnedAmount:item.otherearned_amount || '',
      EarnedGrossSalary: item.earned_gross_salary || '',
      NetSalary: item.net_salary || '',
      EarnedNetSalary: item.earned_net_salary || '',
    }));

    this.excelService.exportAsExcelFile(Employeesalaryreport, 'employee_report');

  }
  Mail(salary_gid:string,month:string,year:string,to_emailid1:string){
    const secretKey = 'storyboarderp';
    const param = (salary_gid);
    const param1 = (month);
    const param2 = (year);
    const param3 = (to_emailid1);
    const encryptedParam =AES.encrypt(param,secretKey).toString();
    const encryptedParam1 =AES.encrypt(param1,secretKey).toString();
    const encryptedParam2 =AES.encrypt(param2,secretKey).toString();
    const encryptedParam3 =AES.encrypt(param3,secretKey).toString();
    this.router.navigate(['/payroll/payrptpayrunmail',encryptedParam,encryptedParam1,encryptedParam2,encryptedParam3])

  }

}













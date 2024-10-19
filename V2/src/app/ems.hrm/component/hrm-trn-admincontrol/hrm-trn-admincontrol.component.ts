import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { ExcelService } from 'src/app/Service/excel.service';

interface IEmployee {
  // showOptionsDivId: any;
  password: string;
  confirmpassword: string;
  showPassword: boolean;
  employee_gid: string;
  user_code: string;
  confirmusercode: string;
  deactivation_date: string;
  remarks: string;
}

export type ChartOptions1 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
};

@Component({
  selector: 'app-hrm-trn-admincontrol',
  templateUrl: './hrm-trn-admincontrol.component.html',
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

export class HrmTrnAdmincontrolComponent {
  // showOptionsDivId: any;
  employeereportlist: any[] = [];
  employeereportformalist: any[] = [];
  chartOptions1: any = {};
  empcountbylocation: any;
  Date: string;
  empcountchart: any;

  reactiveForm!: FormGroup;
  file!: File;
  reactiveFormReset!: FormGroup;
  reactiveFormUpdateUserCode!: FormGroup;
  reactiveFormUserDeactivate!: FormGroup;
  reactiveFormdocument!: FormGroup;
  responsedata: any;
  file1!: File;
  file2!: File;
  reset_list: any[] = [];
  employee_list: any[] = [];
  employeeerror_list: any[] = [];
  Document_list: any[] = [];
  Documentdtl_list: any[] = [];
  parameterValuecode: any;
  parameterValueReset: any;
  employee!: IEmployee;
  usercode: any;
  employee_gid: any;
  user_firstname: any;
  branch: any;
  department: any;
  designation: any;
  status: any;
  data: any;
  employee_list_active: any;
  employee_list_inactive: any;

  constructor(private excelService: ExcelService, public service: SocketService, private route: Router, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService,) {
    this.employee = {} as IEmployee;
    this.Date = new Date().toString();

  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    this.reactiveFormReset = new FormGroup({
      password: new FormControl(this.employee.password, [Validators.required, Validators.pattern(/^\S.*$/)]),
      confirmpassword: new FormControl(''),
      employee_gid: new FormControl(''),
    });

    this.reactiveFormdocument = new FormGroup({

      employee_gid: new FormControl(''),
      file1: new FormControl(''),
      file2: new FormControl(''),
    });

    this.reactiveFormUpdateUserCode = new FormGroup({
      user_code: new FormControl(this.employee.user_code, [Validators.required, Validators.pattern(/^\S.*$/)]),
      confirmusercode: new FormControl(''),
      employee_gid: new FormControl(''),
    });

    this.reactiveFormUserDeactivate = new FormGroup({
      deactivation_date: new FormControl(this.employee.deactivation_date, [Validators.required,]),
      employee_gid: new FormControl(''),
      remarks: new FormControl(''),
    });

    this.GetEmployeeSummary();
    this.GetEmployeeActiveSummary();
    this.GetEmployeeInActiveSummary();
  }

  formatDate(date: string): string {
    console.log('Original Date:', date);
    const parts = date.split('-');
    const formattedDate = `${parts[2]}-${parts[1]}-${parts[0]}`;
    console.log('Formatted Date:', formattedDate);
    return formattedDate;
  }

  GetEmployeeSummary() {
    var api1 = 'HrmTrnAdmincontrol/GetEmployeedtlSummary'

    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.employee_list = this.responsedata.employee_list;
      // this.data.employee_joiningdate = this.formatDate(this.responsedata.employee_joiningdate);
      setTimeout(() => {
        $('#employee_list').DataTable();
      }, 1);
    });
  }

  GetEmployeeActiveSummary() {
    var api1 = 'HrmTrnAdmincontrol/GetEmployeeActiveSummary'

    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.employee_list_active = this.responsedata.employee_list_active;
      // this.data.employee_joiningdate = this.formatDate(this.responsedata.employee_joiningdate);
      setTimeout(() => {
        $('#employee_list_active').DataTable();
      }, 1);
    });
  }

  GetEmployeeInActiveSummary() {
    var api1 = 'HrmTrnAdmincontrol/GetEmployeeInActiveSummary'

    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.employee_list_inactive = this.responsedata.employee_list_inactive;
      // this.data.employee_joiningdate = this.formatDate(this.responsedata.employee_joiningdate);
      setTimeout(() => {
        $('#employee_list_inactive').DataTable();
      }, 1);
    });
  }

  get password() {
    return this.reactiveFormReset.get('password')!;
  }

  get user_code() {
    return this.reactiveFormUpdateUserCode.get('user_code')!;
  }

  userpassword(password: any) {
    this.reactiveFormReset.get("confirmpassword")?.setValue(password.value);
  }

  // get deactive_date() {
  //   return this.reactiveFormUserDeactivate.get('deactive_date')!;
  // }

  get deactivation_date() {
    return this.reactiveFormUserDeactivate.get('deactivation_date')!;
  }

  updateusercode(user_code: any) {
    console.log(user_code.value)
    this.reactiveFormUpdateUserCode.get("confirmusercode")?.setValue(user_code.value);
  }

  openModalUpdateCode(parameter: string) {
    this.parameterValuecode = parameter
    console.log(this.parameterValuecode)
    this.usercode = this.parameterValuecode.user_code;
    this.reactiveFormUpdateUserCode.get("employee_gid")?.setValue(this.parameterValuecode.employee_gid);
    this.user_firstname = this.parameterValuecode.user_name;
    this.branch = this.parameterValuecode.branch_name;
    this.department = this.parameterValuecode.department_name;
    this.designation = this.parameterValuecode.designation_name;
  }

  openModaldeactive(parameter: string) {
    this.parameterValuecode = parameter
    console.log(this.parameterValuecode)
    this.usercode = this.parameterValuecode.user_code;
    this.reactiveFormUserDeactivate.get("employee_gid")?.setValue(this.parameterValuecode.employee_gid);
    this.user_firstname = this.parameterValuecode.user_name;
    this.branch = this.parameterValuecode.branch_name;
    this.department = this.parameterValuecode.department_name;
    this.status = this.parameterValuecode.user_status;
    this.designation = this.parameterValuecode.designation_name;
  }

  openModalReset(parameter: string) {
    this.reactiveFormReset.reset();
    this.parameterValueReset = parameter;
    this.reset_list = this.parameterValueReset;
    this.reactiveFormReset.get("employee_gid")?.setValue(this.parameterValueReset.employee_gid);
    this.usercode = this.parameterValueReset.user_code;
    this.user_firstname = this.parameterValueReset.user_name;
  }
  openModalUpdatedocument(parameter: string) {
    this.parameterValueReset = parameter;
    this.reset_list = this.parameterValueReset;
    this.reactiveFormdocument.get("employee_gid")?.setValue(this.parameterValueReset.employee_gid);
    this.usercode = this.parameterValueReset.user_code;
    this.user_firstname = this.parameterValueReset.user_name;
  }
  onChange2(event: any) {
    this.file2 = event.target.files[0];
  }
  onChange11(event: any) {
    this.file1 = event.target.files[0];
  }
  ondelete() {

  }

  onview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/hrm/HrmMstEmployeview', encryptedParam])
  }

  onedit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/hrm/HrmMstEmployeedit', encryptedParam])
  }

  downloadfileformat() {
    debugger;
    let link = document.createElement("a");

    link.download = "Employee Details";
    window.location.href = environment.URL_FILEPATH + "Templates/HR Employee Import.xlsx";

    link.click();
  }

  exportformAExcel(): void {
    this.NgxSpinnerService.show();
    var url = "HrmRptEmployeeFormA/ExportFormAExcel";
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.employeereportformalist = this.responsedata.employee_listform;
      if (result != null) {
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide();
    });
    (error: any) => {
      console.error('Error:', error);
      this.NgxSpinnerService.hide();
    }
  }

  onclose() {
    this.reactiveFormReset.reset();
  }
  onclosedocument() {
    this.reactiveFormdocument.reset();
  }
  oncloseupdatecode() {
    this.reactiveFormUpdateUserCode.reset();
  }

  onupdatereset() {
    debugger
    if (this.reactiveFormReset.value.password != null && this.reactiveFormReset.value.password != '') {
      for (const control of Object.keys(this.reactiveFormReset.controls)) {
        this.reactiveFormReset.controls[control].markAsTouched();
      }

      var url = 'HrmTrnAdmincontrol/Getresetpassword'

      this.service.post(url, this.reactiveFormReset.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.reactiveFormReset.reset();
        }
        else {
          this.ToastrService.success(result.message)
          this.GetEmployeeSummary();
          this.reactiveFormReset.reset();
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.reactiveFormReset.reset();
  }

  onupdateusercode() {
    if (this.reactiveFormUpdateUserCode.value.user_code != null && this.reactiveFormUpdateUserCode.value.user_code != '') {
      for (const control of Object.keys(this.reactiveFormUpdateUserCode.controls)) {
        this.reactiveFormUpdateUserCode.controls[control].markAsTouched();
      }

      var url = 'HrmTrnAdmincontrol/Getupdateusercode'

      this.service.post(url, this.reactiveFormUpdateUserCode.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
          this.GetEmployeeActiveSummary();
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.reactiveFormUpdateUserCode.reset();
  }
  onupdatedocument() {
    debugger
    let formData = new FormData();
    this.employee = this.reactiveFormdocument.value
    if ((this.file1 != null && this.file1 != undefined) || (this.file2 != null && this.file2 != undefined)) {
      if (this.file1 != null || this.file1 != undefined) {
        formData.append("photo", this.file1, this.file1.name);
      }
      if (this.file2 != null || this.file2 != undefined) {
        formData.append("signature", this.file2, this.file2.name);
      }
      formData.append("employee_gid", this.employee.employee_gid);
      console.log(this.employee.employee_gid)
      var url = "HrmTrnAdmincontrol/UpdateEmployeeProfileUpload";
      this.NgxSpinnerService.show()
      this.service.postfile(url, formData).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.reactiveFormdocument.reset()
          this.NgxSpinnerService.hide()
        }
        else {
          this.ToastrService.success(result.message)
          this.reactiveFormdocument.reset()
          this.NgxSpinnerService.hide()
        }
      });

    }
    else {
      this.ToastrService.warning("kindly upload any one of the document")
    }
  }

  onupdateuserdeactivate() {
    if (this.reactiveFormUserDeactivate.value.deactivation_date != null && this.reactiveFormUserDeactivate.value.deactivation_date != '') {
      for (const control of Object.keys(this.reactiveFormUserDeactivate.controls)) {
        this.reactiveFormUserDeactivate.controls[control].markAsTouched();
      }
      var url = 'HrmTrnAdmincontrol/Getupdateuserdeactivate'
      this.service.post(url, this.reactiveFormUserDeactivate.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
          this.GetEmployeeActiveSummary();
          this.GetEmployeeInActiveSummary();

        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!')
    }
    this.reactiveFormUserDeactivate.reset();
  }

  oncloseuserdeactivate() {

  }

  importexcel() {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0,
      });
      formData.append("file", this.file, this.file.name);
      var api = 'HrmTrnAdmincontrol/EmployeeImport'
      this.NgxSpinnerService.show();
      this.service.postfile(api, formData).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetEmployeeSummary();
          this.GetEmployeeActiveSummary();
          this.GetEmployeeInActiveSummary();
        }
        else {
          this.ToastrService.success(result.message)
          this.GetEmployeeSummary();
          this.GetEmployeeActiveSummary();
          this.GetEmployeeInActiveSummary();
        }
      });
    }
  }


  geterorrlog() {
    var api1 = 'HrmTrnAdmincontrol/GetEmployeeerrorlogSummary'

    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.employeeerror_list = this.responsedata.employee_list;
      console.log(this.employee_list)
      setTimeout(() => {
        $('#employee_list').DataTable();
      }, 1);
    });
  }

  onChange1(event: any) {
    this.file = event.target.files[0];
  }

  ondetail(document_name: any) {
    var api1 = 'HrmTrnAdmincontrol/GetDocumentDtllist'
    var param = {
      document_gid: document_name,
    }
    this.service.getparams(api1, param).subscribe((result: any) => {

      this.responsedata = result;
      this.Documentdtl_list = this.responsedata.documentdtl_list;
    });
  }

  //360/

  onopen(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/hrm/HrmTrnEmployee360', encryptedParam])
  }

  getdocumentlist() {
    var api1 = 'HrmTrnAdmincontrol/GetDocumentlist'
    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.Document_list = this.responsedata.document_list;
    });
  }
  personaldatapdf(employee_gid: string) {
    debugger;
    var url = "HrmTrnAdmincontrol/personaldatapdf";
    let param = {
      employee_gid: employee_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
    });
  }

  formapdf(employee_gid: string) {
    debugger;
    var url = "HrmTrnAdmincontrol/formapdf";
    let param = {
      employee_gid: employee_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
    });
  }


  // PrintPDF(assessment_gid: string, employee_gid : String) {
  //   debugger
  //         const api = 'HrmTrnAdmincontrol/Getformapdf';
  //         this.NgxSpinnerService.show()
  //         let param = {
  //           assessment_gid:assessment_gid,
  //           employee_gid:employee_gid,
  //         } 
  //         this.service.getparams(api,param).subscribe((result: any) => {
  //           if(result!=null){
  //             this.service.filedownload1(result);
  //           }
  //           this.NgxSpinnerService.hide()
  //         });

  // }
}
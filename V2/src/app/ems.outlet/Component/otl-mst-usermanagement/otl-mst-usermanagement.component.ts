import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';

interface IEmployee {
  password: string;
  confirmpassword: string;
  showPassword: boolean;
  employee_gid: string;
  user_code: string;
  confirmusercode: string;
  deactivation_date: string;
  remarks: string;
}

@Component({
  selector: 'app-otl-mst-usermanagement',
  templateUrl: './otl-mst-usermanagement.component.html',
  styleUrls: ['./otl-mst-usermanagement.component.scss']
})
export class OtlMstUsermanagementComponent {
  showOptionsDivId: any;
  file!: File;
  reactiveForm!: FormGroup;
  reactiveFormReset!: FormGroup;
  reactiveFormUpdateUserCode!: FormGroup;
  reactiveFormUserDeactivate!: FormGroup;
  responsedata: any;
  reset_list: any[] = [];
  employee_list: any[] = [];
  employeeerror_list:any[]=[];
  parameterValuecode: any;
  parameterValueReset: any;
  employee!: IEmployee;
  usercode: any;
  user_firstname: any;
  branch: any;
  department: any;
  status: any;
  designation: any;

  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService,public NgxSpinnerService: NgxSpinnerService,) {
    this.employee = {} as IEmployee;
  }

  ngOnInit(): void {

    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    this.reactiveFormReset = new FormGroup({

      password: new FormControl(this.employee.password, [Validators.required,]),
      confirmpassword: new FormControl(''),
      employee_gid: new FormControl(''),
    });

    this.reactiveFormUpdateUserCode = new FormGroup({

      user_code: new FormControl(this.employee.user_code, [Validators.required,Validators.pattern(/^\S.*$/)]),
      confirmusercode: new FormControl(''),
      employee_gid: new FormControl(''),
    });
    this.GetEmployeeSummary();

    this.reactiveFormUserDeactivate = new FormGroup({

      deactivation_date: new FormControl(this.employee.deactivation_date, [Validators.required,]),
      employee_gid: new FormControl(''),
      remarks: new FormControl(''),
    });
    this.GetEmployeeSummary();
  }
  toggleOptions(employee_gid: any) {
    if (this.showOptionsDivId === employee_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = employee_gid;
    }
  }
  GetEmployeeSummary() {
    var api1 = 'OutletManage/GetUserSummary'

    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.employee_list = this.responsedata.Outletuser_list;
      console.log(this.employee_list)
      setTimeout(() => {
        $('#employee_list').DataTable();
      }, 1);
    });
  }

  get password() {
    return this.reactiveFormReset.get('password')!;
  }
  get user_code() {
    return this.reactiveFormUpdateUserCode.get('user_code')!;
  }
  get deactivation_date() {
    return this.reactiveFormUserDeactivate.get('deactivation_date')!;
  }

  userpassword(password: any) {
    this.reactiveFormReset.get("confirmpassword")?.setValue(password.value);
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

  openModaldeactive(parameter: string){
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
    this.parameterValueReset = parameter;
    this.reset_list = this.parameterValueReset;
    this.reactiveFormReset.get("employee_gid")?.setValue(this.parameterValueReset.employee_gid);
    this.usercode = this.parameterValueReset.user_code;
    this.user_firstname = this.parameterValueReset.user_name;
  }

  ondelete() {
    
  }

  onview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/outlet/OtlMstUserView', encryptedParam])
  }

  onedit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/outlet/OtlMstUserEdit', encryptedParam])
  }

  onclose() {
    this.reactiveFormReset.reset();
    
  }

  oncloseupdatecode() {
    this.reactiveFormUpdateUserCode.reset();
    
  }

  onupdatereset() {
      if (this.reactiveFormReset.value.password != null && this.reactiveFormReset.value.password != '') {
      for (const control of Object.keys(this.reactiveFormReset.controls)) {
        this.reactiveFormReset.controls[control].markAsTouched();
      }

      var url = 'Employeelist/Getresetpassword'

      this.service.post(url, this.reactiveFormReset.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
          this.GetEmployeeSummary();
         
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!')
    }
    this.reactiveFormReset.reset();
  }

  onupdateusercode() {
    if (this.reactiveFormUpdateUserCode.value.user_code != null && this.reactiveFormUpdateUserCode.value.user_code != '') {
      for (const control of Object.keys(this.reactiveFormUpdateUserCode.controls)) {
        this.reactiveFormUpdateUserCode.controls[control].markAsTouched();
      }
      var url = 'Employeelist/Getupdateusercode'
      this.service.post(url, this.reactiveFormUpdateUserCode.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
          this.GetEmployeeSummary();
        }
      });
      
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!')
    }
    this.reactiveFormUpdateUserCode.reset();
  }

  // onupdateuserdeactivate(){
  //     var url = 'Employeelist/Getupdateuserdeactivate'
  //     this.service.post(url, this.reactiveFormUserDeactivate.value).pipe().subscribe((result: any) => {
  //       this.responsedata = result;
  //       if (result.status == false) {
  //         this.ToastrService.warning(result.message)
  //       }
  //       else {
  //         this.ToastrService.success(result.message)
  //         this.GetEmployeeSummary();
  //       }
  //     });
  //   this.reactiveFormUserDeactivate.reset(); 
  // }
  
  onupdateuserdeactivate(){
    if (this.reactiveFormUserDeactivate.value.deactivation_date != null && this.reactiveFormUserDeactivate.value.deactivation_date != '') {
      for (const control of Object.keys(this.reactiveFormUserDeactivate.controls)) {
        this.reactiveFormUserDeactivate.controls[control].markAsTouched();
      }
      var url = 'Employeelist/Getupdateuserdeactivate'
      this.service.post(url, this.reactiveFormUserDeactivate.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
          this.GetEmployeeSummary();          
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!')
    }
    this.reactiveFormUserDeactivate.reset(); 
  }

  oncloseuserdeactivate(){
   
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  
  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Employee import Excel";
    window.location.href = "https://" + environment.host + "/Templates/Employee import Excel.xlsx";
    link.click();
  }
  importexcel() {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0,
      });
      formData.append("file", this.file, this.file.name);
      var api = 'Employeelist/EmployeeImport'
      this.NgxSpinnerService.show();
      this.service.postfile(api, formData).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        debugger;
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetEmployeeSummary();
        }
        else {
          this.ToastrService.success(result.message)
          this.GetEmployeeSummary();
        }

      });
    }
  }
  
  geterorrlog(){
    var api1 = 'Employeelist/EmployeeerrorlogSummary'

    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.employeeerror_list = this.responsedata.employee_list;
      console.log(this.employee_list)
      setTimeout(() => {
        $('#employee_list').DataTable();
      }, 1);
    });
  }
}

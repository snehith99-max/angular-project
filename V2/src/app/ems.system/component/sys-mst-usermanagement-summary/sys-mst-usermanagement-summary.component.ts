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
  selector: 'app-sys-mst-usermanagement-summary',
  templateUrl: './sys-mst-usermanagement-summary.component.html'
})
export class SysMstUsermanagementSummaryComponent {
  showOptionsDivId: any;
  file!: File;
  reactiveForm!: FormGroup;
  reactiveFormReset!: FormGroup;
  reactiveFormUpdateUserCode!: FormGroup;
  reactiveFormUserDeactivate!: FormGroup;
  responsedata: any;
  reset_list: any[] = [];
  employee_list: any[] = [];
  parameterValuecode: any;
  parameterValueReset: any;
  employee!: IEmployee;
  usercode: any;
  user_firstname: any;
  branch: any;
  parameterValue:any;
  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService,public NgxSpinnerService: NgxSpinnerService,) {
    this.employee = {} as IEmployee;
  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    this.reactiveFormReset = new FormGroup({
      password: new FormControl(this.employee.password, [Validators.required,Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/)]),
      confirmpassword: new FormControl(''),
      employee_gid: new FormControl(''),
    });
    this.GetEmployeeSummary();
  }
  GetEmployeeSummary() {
    this.NgxSpinnerService.show();
    var api1 = 'UserManagementSummary/GetUserSummary'
    this.service.get(api1).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;
      this.employee_list = this.responsedata.usersummary_list;
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
  openModalReset(parameter: string) {
    this.parameterValueReset = parameter;
    this.reset_list = this.parameterValueReset;
    this.reactiveFormReset.get("employee_gid")?.setValue(this.parameterValueReset.employee_gid);
    this.usercode = this.parameterValueReset.user_code;
    this.user_firstname = this.parameterValueReset.user_name;
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    console.log(this.parameterValue);
    var url = 'UserManagementSummary/Getdeleteuser'
    let param = {
      employee_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0, 
        });
        this.ToastrService.warning(result.message)
        this.GetEmployeeSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.GetEmployeeSummary();
      }
      this.GetEmployeeSummary();
      this.NgxSpinnerService.hide();
    });
  }
  onedit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/system/SysMstUserEdit', encryptedParam])
  }
  onclose() {
    this.reactiveFormReset.reset();
  }
}


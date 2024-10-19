
import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import {FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AbstractControl, ValidatorFn } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {
  branchname: string;
  entityname: string;
  first_name: string;
  mobile: string;
  user_type:string;
  country: string;
  user_code: string;
  user_password: string;
  password: string;
  confirmpassword: string;
  showPassword: boolean;
}
@Component({
  selector: 'app-sys-mst-user-add',
  templateUrl: './sys-mst-user-add.component.html'
})
export class SysMstUserAddComponent  implements OnInit {
  employee!: IEmployee;
  reactiveForm: FormGroup | any;
  entity_list: any[] = [];
  branch_list: any[] = [];
  selectedEmpPassword: any;
  selectedEmpAccess: string = 'Yes';
  Emp_Code: any;
  Entity: any;
  responsedata: any;
  formdata = { selectedEmpPassword: "", Confrim_Emp_password: "", Emp_Code: "", Emp_Join_date: "", Emp_mobile_number: "", Email_Address: "" }
  mdlBranchName :any;
  entityList: any;
  branchList: any;
  constructor(public service: SocketService, private route: Router, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {
 this.employee = {} as IEmployee;
  }
  ngOnInit(): void {
    this.reactiveForm = new FormGroup({
      first_name: new FormControl(this.employee.first_name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      user_code: new FormControl(this.employee.user_code, [Validators.required,Validators.pattern(/^\S.*$/)]),
      user_type: new FormControl(''),
      branchname: new FormControl(this.employee.branchname, [ Validators.required,Validators.minLength(1), Validators.maxLength(250)]),
      entityname: new FormControl(this.employee.entityname, [ Validators.required, Validators.minLength(1), Validators.maxLength(250)]),
      mobile: new FormControl(this.employee.mobile, [Validators.required,Validators.minLength(10), Validators.maxLength(15)]),
      password: new FormControl(this.employee.password, [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/)]),
      confirmpassword: new FormControl(''),
    }
    );
    var url = 'Employeelist/Getbranchdropdown';
    this.service.get(url).subscribe((result: any) => {
      this.branchList = result.Getbranchdropdown;
    });
    var api1 = 'Employeelist/Getentitydropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.entity_list = result.Getentitydropdown;
    });
  }
  get branchname() {
    return this.reactiveForm.get('branchname')!;
  }
  get first_name() {
    return this.reactiveForm.get('first_name')!;
  }
  get user_code() {
    return this.reactiveForm.get('user_code')!;
  }
  get mobile() {
    return this.reactiveForm.get('mobile')!;
  }
  get password() {
    return this.reactiveForm.get('password')!;
  }
  get confirmpassword() {
    return this.reactiveForm.get('confirmpassword')!;
  }
  get user_password() {
    return this.reactiveForm.get('user_password')!;
  }
  get entityname() {
    return this.reactiveForm.get('entityname')!;
  }
  userpassword(password: any) {
    this.reactiveForm.get("confirmpassword")?.setValue(password.value);
  }
  public validate(): void {
    console.log(this.reactiveForm.value)
    this.employee = this.reactiveForm.value;
    if (this.employee.entityname != null && this.employee.branchname != null  && this.employee.first_name != null && this.employee.password != null && this.employee.user_type != null  && this.employee.mobile != null) {
      let formData = new FormData(); {
        var api7 = 'UserManagementSummary/Postuserdetails'
        this.NgxSpinnerService.show();
        this.service.post(api7, this.employee).subscribe((result: any) => {
          if (result.status == false) {
           
              this.ToastrService.warning(result.message)
          }
          else {
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide();
            this.route.navigate(['/system/SysMstUsermanagementSummary']);
            this.reactiveForm.reset();
          }
        });
       
      }
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!')
    }
    return;
  }
  noSpaceValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const isSpace = (control.value || '').trim().indexOf(' ') !== -1;
      return isSpace ? { 'noSpace': true } : null;
    };
  }
  togglePasswordVisibility(): void {
    this.employee.showPassword = !this.employee.showPassword;
  } 
}

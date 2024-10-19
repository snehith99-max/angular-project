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
  departmentname: string;
  designationname: string;
  first_name: string;
  last_name: string;
  gender: string;
  mobile: string;
  email: string;
  comp_email:string;
  permanent_address1: string;
  permanent_address2: string;
  permanent_city: string;
  permanent_postal: string;
  permanent_state: string;
  temporary_address1: string;
  temporary_address2: string;
  temporary_city: string;
  temporary_postal: string;
  temporary_state: string;
  country: string;
  countryname: string;
  // reportingto:string;
  usergrouptemplate:string;
  user_code: string;
  user_password: string;
  password: string;
  confirmpassword: string;
  active_flag: string;
  showPassword: boolean;
  
}

@Component({
  selector: 'app-sys-mst-employee-add',
  templateUrl: './sys-mst-employee-add.component.html',
  styleUrls: ['./sys-mst-employee-add.component.scss']
})

export class SysMstEmployeeAddComponent implements OnInit {

  
  file!: File;
  employee!: IEmployee;
  reactiveForm: FormGroup | any;
  entity_list: any[] = [];
  branch_list: any[] = [];
  department_list: any[] = [];
  designation_list: any[] = [];
  country_list: any[] = [];
  country_list2: any[] = [];
  // employeereportingto_list: any[] = [];
  selectedEmpPassword: any;
  Emp_Join_date: any;
  selectedEmpAccess: string = 'Yes';
  Emp_Code: any;
  Email_Address: any;
  Entity: any;
  responsedata: any;
  invalidFileFormat:boolean= false;
  
  formdata = { selectedEmpPassword: "", Confrim_Emp_password: "", Emp_Code: "", Emp_Join_date: "", Emp_mobile_number: "", Email_Address: "" }

  entityList: any;
  branchList: any;
  // ReportingTo: any;
  Usergrouptemplate: any;
  usergrouptemp_list: any[] = [];
  departmentList: any[] = []

  constructor(public service: SocketService, private route: Router, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {
    this.employee = {} as IEmployee;
  }

  ngOnInit(): void {
    debugger
    this.reactiveForm = new FormGroup({
      first_name: new FormControl(this.employee.first_name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      user_code: new FormControl(this.employee.user_code, [Validators.required,Validators.pattern(/^\S.*$/)]),
      file: new FormControl(''),
      permanent_address2: new FormControl(''),
      permanent_state: new FormControl(''),
      active_flag: new FormControl('Y'),
      gender: new FormControl('male'),
      user_phtoto: new FormControl(''),
      last_name: new FormControl(''),
      permanent_postal: new FormControl('', [Validators.minLength(6)]),
      permanent_city: new FormControl(''),
      branchname: new FormControl(this.employee.branchname, [ Validators.required,Validators.minLength(1), Validators.maxLength(250)]),
      // entityname: new FormControl(this.employee.entityname, [ Validators.required, Validators.minLength(1), Validators.maxLength(250)]),
      mobile: new FormControl(this.employee.mobile, [Validators.required,Validators.minLength(10), Validators.maxLength(12)]),
      departmentname: new FormControl(this.employee.departmentname, [Validators.required,Validators.minLength(1)]),
      designationname: new FormControl(this.employee.designationname, [Validators.required,Validators.minLength(1)]),
      countryname: new FormControl(this.employee.countryname, [Validators.minLength(1)]),
      country: new FormControl(this.employee.country, [Validators.minLength(1)]),
      temporary_address2: new FormControl(''),
      temporary_address1: new FormControl(''),
      temporary_postal: new FormControl('', [Validators.minLength(6)]),
      temporary_city: new FormControl(''),
      temporary_state: new FormControl(''),
      permanent_address1: new FormControl(this.employee.permanent_address1, [Validators.maxLength(1000)]),
      email: new FormControl(''),
      // email: new FormControl(this.employee.email, [Validators.required, Validators.pattern(/^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/)]),
      comp_email:new FormControl(this.employee.comp_email,[Validators.required, Validators.pattern(/^[a-z0-9._%+-]+@(?!gmail\.com$)(?!yahoo\.com$)(?!hotmail\.com$)(?!outlook\.com$)(?!live\.com$)[a-z0-9.-]+\.[a-z]{2,100}$/)]),
      password: new FormControl(this.employee.password, [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/)]),
      confirmpassword: new FormControl({ value: '', disabled: false }),
      // reportingto: new FormControl(this.employee.reportingto),
      usergrouptemplate: new FormControl(this.employee.usergrouptemplate),
      employee_photo: new FormControl(''),
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

    var api3 = 'Employeelist/Getdepartmentdropdown'
    this.service.get(api3).subscribe((result: any) => {
      this.departmentList = result.Getdepartmentdropdown;      
    });

    var api4 = 'Employeelist/Getdesignationdropdown'
    this.service.get(api4).subscribe((result: any) => {
      this.designation_list = result.Getdesignationdropdown;      
    });

    var api5 = 'Employeelist/Getcountrydropdown'
    this.service.get(api5).subscribe((result: any) => {
      this.country_list = result.Getcountrydropdown;      
    });

    var api6 = 'Employeelist/Getcountry2dropdown'
    this.service.get(api6).subscribe((result: any) => {
      this.country_list2 = result.Getcountry2dropdown;    
    });

    // var api7 = 'Employeelist/Getreportingtodropdown';
    // this.service.get(api7).subscribe((result: any) => {
    //   this.employeereportingto_list = result.Getreportingtodropdown;
    // });
    debugger
    var url = 'Employeelist/Getusergrouptempdropdown';
    this.service.get(url).subscribe((result: any) => {
      this.usergrouptemp_list = result.Getusergrouptempdropdown;
    });
  }

  onChange2(event: any) {
    debugger
    this.file = event.target.files[0];
    
    const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
   
    if (this.file && validImageTypes.includes(this.file.type)) {
      this.invalidFileFormat = false;
      this.reactiveForm.get('employee_photo')?.setValue(this.file);
    } else {
      this.invalidFileFormat = true;
      this.reactiveForm.get('employee_photo')?.reset();
      event.target.value = ''; // Clear the file input field
    }
  //   if (this.file) {
  //     this.user_phtoto = this.file.name;
  //     this.Images = this.file;
  //     this.form.patchValue({
  //       file_name: this.file.name
  //     });
  //   }
  //  else {
  //     this.file_name = null;
  //     this.Images = null;
  //     this.form.patchValue({
  //       file_name: null
  //     });
  //   }
  }
  

  get permanent_postal() {
    return this.reactiveForm.get('permanent_postal')!;
  }
  get temporary_postal() {
    return this.reactiveForm.get('temporary_postal')!;
  }
  get branchname() {
    return this.reactiveForm.get('branchname')!;
  }
  get departmentname() {
    return this.reactiveForm.get('departmentname')!;
  }
  get designationname() {
    return this.reactiveForm.get('designationname')!;
  }
  get countryname() {
    return this.reactiveForm.get('countryname')!;
  }
  get country() {
    return this.reactiveForm.get('country')!;
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
  get email() {
    return this.reactiveForm.get('email')!;
  }
  get comp_email() {
    return this.reactiveForm.get('comp_email')
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
  get usergrouptemplate() {
    return this.reactiveForm.get('usergrouptemplate')!;
  }
  // get entityname() {
  //   return this.reactiveForm.get('entityname')!;
  // }
  userpassword(password: any) {
    this.reactiveForm.get("confirmpassword")?.setValue(password.value);
  }
  public validate(): void {
    this.NgxSpinnerService.show();
    console.log(this.reactiveForm.value)

    this.employee = this.reactiveForm.value;
    if (this.employee.branchname != null && this.employee.departmentname != null && this.employee.designationname != null && this.employee.first_name != null && this.employee.active_flag != null && this.employee.password != null && this.employee.email != null && this.employee.mobile != null && this.employee.comp_email != null) {
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        formData.append("file", this.file, this.file.name);
        // formData.append("entityname", this.employee.entityname);
        formData.append("branchname", this.employee.branchname);
        formData.append("departmentname", this.employee.departmentname);
        formData.append("designationname", this.employee.designationname);
        formData.append("active_flag", this.employee.active_flag);
        formData.append("user_code", this.employee.user_code);
        formData.append("password", this.employee.password);
        formData.append("confirmpassword", this.employee.confirmpassword);
        formData.append("first_name", this.employee.first_name);
        formData.append("last_name", this.employee.last_name);
        formData.append("gender", this.employee.gender);
        formData.append("email", this.employee.email);
        formData.append("mobile", this.employee.mobile);
        formData.append("permanent_address1", this.employee.permanent_address1);
        formData.append("permanent_address2", this.employee.permanent_address2);
        formData.append("country", this.employee.country);
        formData.append("permanent_city", this.employee.permanent_city);
        formData.append("permanent_state", this.employee.permanent_state);
        formData.append("permanent_postal", this.employee.permanent_postal);
        formData.append("temporary_address1", this.employee.temporary_address1);
        formData.append("temporary_address2", this.employee.temporary_address2);
        formData.append("countryname", this.employee.countryname);
        formData.append("temporary_city", this.employee.temporary_city);
        formData.append("temporary_state", this.employee.temporary_state);
        formData.append("temporary_postal", this.employee.temporary_postal);
         formData.append("usergrouptemplate", this.employee.usergrouptemplate);
        formData.append("comp_email",this.employee.comp_email);


        var api = 'Employeelist/EmployeeProfileUpload'
        this.NgxSpinnerService.show();
        this.service.postfile(api, formData).subscribe((result: any) => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide();

          }
          else {
            
            this.ToastrService.success(result.message)
            this.route.navigate(['/system/SysMstEmployeeSummary']);
            this.NgxSpinnerService.hide();

          }
        });
      }
      else {
        var api7 = 'Employeelist/PostEmployeedetails'
        this.NgxSpinnerService.show();
        this.service.post(api7, this.employee).subscribe((result: any) => {
          if (result.status == false) {           
              this.ToastrService.warning(result.message)
              this.NgxSpinnerService.hide();


          }
          else {
            this.ToastrService.success(result.message)
            this.route.navigate(['/system/SysMstEmployeeSummary']);
            this.reactiveForm.reset();
            this.NgxSpinnerService.hide();


          }
          // this.responsedata = result;
        });
       
      }
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!')
      this.NgxSpinnerService.hide();
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
  copyAddress(): void {
    this.reactiveForm.patchValue({
      temporary_address1: this.reactiveForm.get('permanent_address1').value,
      temporary_address2: this.reactiveForm.get('permanent_address2').value,
      temporary_city: this.reactiveForm.get('permanent_city').value,
      temporary_postal: this.reactiveForm.get('permanent_postal').value,
      temporary_state: this.reactiveForm.get('permanent_state').value,  
      countryname: this.reactiveForm.get('country').value
    });
  }
}

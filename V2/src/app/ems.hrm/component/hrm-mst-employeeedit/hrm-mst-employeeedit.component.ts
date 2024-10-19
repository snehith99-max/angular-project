import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';


interface IEmployee {
  mobileno: string;
  bloodgroup: string;
  differentlyabled: string;
  remarks: string;
  employee_gid: string;
  qualification: string;
  comp_email: string
  workertype: string;
  dob: string;
  role: string;
  aadhar_no: string,
  father_spouse: string,
  shift: string;
  password: string;
  leavegrade: string;
  reportingto: string;
  holidaygrade: string;
  age: string;
  employee_joiningdate: string;
  jobtype: string;
  boimetricid: string;
  tagid: string;
  temporary_addressgid: string;
  permanent_addressgid: string;
  temporary_address1: string;
  temporary_address2: string;
  email: string;
  permanent_address1: string;
  active_flag: string;
  gender: string;
  temporary_postal: string;
  confirmpassword: string;
  user_password: string;
  branchname: string;
  entityname: string;
  country: string;
  countryname: string;
  departmentname: string;
  designationname: string;
  showPassword: boolean;
  permanent_state: string;
  first_name: string;
  user_code: string;
  permanent_address2: string;
  permanent_city: string;
  permanent_postal: string;
  mobile: string;
  last_name: string;
  temporary_state: string;
  temporary_city: string;
}

@Component({
  selector: 'app-hrm-mst-employeeedit',
  templateUrl: './hrm-mst-employeeedit.component.html',
  styleUrls: ['./hrm-mst-employeeedit.component.scss']
})

export class HrmMstEmployeeeditComponent implements OnInit {
  file!: File;
  employee!: IEmployee;
  reactiveForm!: FormGroup;
  entity_list: any[] = [];
  branch_list: any[] = [];
  department_list: any[] = [];
  workertype_list: any[] = [];
  designation_list: any[] = [];
  country_list: any[] = [];
  country_list2: any[] = [];
  selectedEmpPassword: any;
  Emp_Join_date: any;
  selectedEmpAccess: string = 'Yes';
  Emp_Code: any;
  Email_Address: any;
  Entity: any;
  responsedata: any;
  selectedBranch: any;
  selectedDepartment: any;
  selectedEntity: any;
  selectedDesignation: any;
  selectedCountry1: any;
  selectedCountry2: any;
  formdata = { selectedEmpPassword: "", Confrim_Emp_password: "", Emp_Code: "", Emp_Join_date: "", Emp_mobile_number: "", Email_Address: "" }
  entityList: any;
  branchList: any;
  departmentList: any[] = []
  employeegid: any;
  employeeedit_list: any;
  role_list: any [] = [];
  shift_list: any [] = [];
  jobtype_list : any[] = [];
  employeereportingto_list : any[] =[];
  leavegrade_list : any [] = [];
  holidaygrade_list : any [] = [];
  bloodgroup_list : any [] = [];

  mdlWorkType: any;
  mdlholidaygrade: any;
  mdlcountry: any;
  mdlreporting:any;
  mdlcountryname: any;
  mdlentity: any;

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router,public NgxSpinnerService:NgxSpinnerService, private router: ActivatedRoute) {
    this.employee = {} as IEmployee;
  }

  


  ngOnInit(): void {

    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    
    flatpickr('.date-picker', options);

    const employee_gid = this.router.snapshot.paramMap.get('employee_gid');

    this.employeegid = employee_gid;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.employeegid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetEditEmployeeSummary(deencryptedParam)

    this.reactiveForm = new FormGroup({
      first_name: new FormControl(this.employee.first_name, [Validators.required,]),
      user_code: new FormControl(this.employee.user_code, [Validators.required,]),
      file: new FormControl(''),
      permanent_address2: new FormControl(''),
      aadhar_no: new FormControl(''),
      differentlyabled: new FormControl('N'),
      permanent_state: new FormControl(''),
      active_flag: new FormControl('Yes'),
      workertype: new FormControl(this.employee.workertype, [Validators.required]),
      gender: new FormControl('male'),
      user_phtoto: new FormControl(''),
      last_name: new FormControl(''),
      permanent_postal: new FormControl(''),
      permanent_city: new FormControl(''),
      father_spouse: new FormControl(''),
      qualification: new FormControl(''),
      reportingto: new FormControl(this.employee.reportingto),
      dob: new FormControl(this.employee.dob, [Validators.required]),
      age: new FormControl(''),
      role: new FormControl(this.employee.role, [Validators.required]),
      branchname: new FormControl(this.employee.branchname, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      // entityname: new FormControl(this.employee.entityname, [
      //   Validators.required,
      //   Validators.minLength(1),
      //   Validators.maxLength(250),
      // ]),

      mobile: new FormControl(this.employee.mobile, [
        Validators.maxLength(10),
      ]),
      departmentname: new FormControl(this.employee.departmentname, [
        Validators.required,
        Validators.minLength(1),
      ]),
      designationname: new FormControl(this.employee.designationname, [
        Validators.required,
        Validators.minLength(1),
      ]),
      countryname: new FormControl(this.employee.countryname, [
        Validators.minLength(1),
      ]),
      country: new FormControl(this.employee.country, [
        Validators.minLength(1),
      ]),
      employee_gid: new FormControl(''),
      temporary_address2: new FormControl(''),
      shift: new FormControl(this.employee.shift, [Validators.required]),
      leavegrade: new FormControl(this.employee.leavegrade),
      tagid: new FormControl(this.employee.tagid),
      biometricid: new FormControl(this.employee.tagid),
      hide_flag: new FormControl('Y'),
      holidaygrade: new FormControl(this.employee.holidaygrade),
      jobtype: new FormControl(this.employee.jobtype, [Validators.required]),
      employee_joiningdate: new FormControl(this.employee.employee_joiningdate, [Validators.required]),
      boimetricid: new FormControl(this.employee.boimetricid),
      temporary_address1: new FormControl(''),
      temporary_postal: new FormControl(''),
      temporary_city: new FormControl(''),
      temporary_state: new FormControl(''),
      temporary_addressgid: new FormControl(''),
      permanent_addressgid: new FormControl(''),
      bloodgroup: new FormControl(''),
      remarks: new FormControl(''),
      mobileno: new FormControl(this.employee.mobileno, [Validators.maxLength(10)]),
      comp_email: new FormControl('', [Validators.pattern(/^[a-z0-9._%+-]+@(?!gmail\.com$)(?!yahoo\.com$)(?!hotmail\.com$)(?!outlook\.com$)(?!live\.com$)[a-z0-9.-]+\.[a-z]{2,100}$/)]),
      permanent_address1: new FormControl(this.employee.permanent_address1, [
        Validators.maxLength(1000),
      ]),
      email: new FormControl(this.employee.email, [
        Validators.minLength(1),
        Validators.maxLength(250), Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')
      ]),
    }
    

    );

    var url = 'HrmTrnAdmincontrol/Getbranchdropdown';
    this.service.get(url).subscribe((result: any) => {
      this.branchList = result.Getbranchdropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getentitydropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.entity_list = result.Getentitydropdown;
    });

    var api3 = 'HrmTrnAdmincontrol/Getdepartmentdropdown'
    this.service.get(api3).subscribe((result: any) => {
      this.departmentList = result.Getdepartmentdropdown;
    });

    var api4 = 'HrmTrnAdmincontrol/Getdesignationdropdown'
    this.service.get(api4).subscribe((result: any) => {
      this.designation_list = result.Getdesignationdropdown;
    });

    var api5 = 'HrmTrnAdmincontrol/Getcountrydropdown'
    this.service.get(api5).subscribe((result: any) => {
      this.country_list = result.Getcountrydropdown;
    });

    var api6 = 'HrmTrnAdmincontrol/Getcountry2dropdown'
    this.service.get(api6).subscribe((result: any) => {
      this.country_list2 = result.Getcountry2dropdown;
    });

    var url = 'HrmTrnAdmincontrol/Getworkertypedropdown';
    this.service.get(url).subscribe((result: any) => {
      this.workertype_list = result.Getworkertypedropdown;
    });

    var url = 'HrmTrnAdmincontrol/Getroledropdown';
    this.service.get(url).subscribe((result: any) => {
      this.role_list = result.Getroledropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getjobtypedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.jobtype_list = result.Getjobtypenamedropdown;
    });

    var url = 'HrmTrnAdmincontrol/Getreportingtodropdown';
    this.service.get(url).subscribe((result: any) => {
      this.employeereportingto_list = result.Getreportingtodropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getshifttypedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.shift_list = result.Getshifttypenamedropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getleavegradedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.leavegrade_list = result.Getleavegradenamedropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getholidaygradedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.holidaygrade_list = result.Getholidaygradedropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getbloodgroupdropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.bloodgroup_list = result.Getbloodgroupdropdown;
    });
}
  GetEditEmployeeSummary(employee_gid: any) {
    debugger
    var url = 'HrmTrnAdmincontrol/GetEditEmployeeSummary'
    let param = {
      employee_gid: employee_gid
     }

    this.service.getparams(url, param).subscribe((result: any) => {
      this.employeeedit_list = result.GetEditEmployeeSummary;
      console.log(this.employeeedit_list)
      console.log(this.employeeedit_list[0].branch_gid)
      this.selectedBranch = this.employeeedit_list[0].branch_gid;
      this.reactiveForm.get("mobile")?.setValue(this.employeeedit_list[0].employee_personalno);
      this.reactiveForm.get("mobileno")?.setValue(this.employeeedit_list[0].employee_mobileno);
      this.reactiveForm.get("departmentname")?.setValue(this.employeeedit_list[0].department_name);
      this.reactiveForm.get("designationname")?.setValue(this.employeeedit_list[0].designation_name);
      // this.reactiveForm.get("entityname")?.setValue(this.employeeedit_list[0].entity_name);
      this.reactiveForm.get("branchname")?.setValue(this.employeeedit_list[0].branch_name);
      this.reactiveForm.get("workertype")?.setValue(this.employeeedit_list[0].workertype_name);
      this.reactiveForm.get("active_flag")?.setValue(this.employeeedit_list[0].user_status);
      this.reactiveForm.get("user_code")?.setValue(this.employeeedit_list[0].user_code);
      this.reactiveForm.get("first_name")?.setValue(this.employeeedit_list[0].user_firstname);
      this.reactiveForm.get("age")?.setValue(this.employeeedit_list[0].age);
      this.reactiveForm.get("last_name")?.setValue(this.employeeedit_list[0].user_lastname);
      this.reactiveForm.get("permanent_address1")?.setValue(this.employeeedit_list[0].permanent_address1);
      this.reactiveForm.get("permanent_address2")?.setValue(this.employeeedit_list[0].permanent_address2);
      this.reactiveForm.get("permanent_city")?.setValue(this.employeeedit_list[0].permanent_city);
      this.reactiveForm.get("permanent_state")?.setValue(this.employeeedit_list[0].permanent_state);
      this.reactiveForm.get("email")?.setValue(this.employeeedit_list[0].employee_emailid);
      this.reactiveForm.get("comp_email")?.setValue(this.employeeedit_list[0].employee_companyemailid);
      this.reactiveForm.get("employee_gid")?.setValue(this.employeeedit_list[0].employee_gid);
      this.reactiveForm.get("permanent_addressgid")?.setValue(this.employeeedit_list[0].permanent_addressgid);
      this.reactiveForm.get("temporary_addressgid")?.setValue(this.employeeedit_list[0].temporary_addressgid);
      this.reactiveForm.get("permanent_postal")?.setValue(this.employeeedit_list[0].permanent_postalcode);
      this.reactiveForm.get("temporary_address1")?.setValue(this.employeeedit_list[0].temporary_address1);
      this.reactiveForm.get("temporary_address2")?.setValue(this.employeeedit_list[0].temporary_address2);
      this.reactiveForm.get("temporary_city")?.setValue(this.employeeedit_list[0].temporary_city);
      this.reactiveForm.get("temporary_state")?.setValue(this.employeeedit_list[0].temporary_state);
      this.reactiveForm.get("temporary_postal")?.setValue(this.employeeedit_list[0].temporary_postalcode);
      this.reactiveForm.get("gender")?.setValue(this.employeeedit_list[0].employee_gender);
      this.reactiveForm.get("remarks")?.setValue(this.employeeedit_list[0].remarks);
      this.reactiveForm.get("role")?.setValue(this.employeeedit_list[0].role_name);
      this.reactiveForm.get("jobtype")?.setValue(this.employeeedit_list[0].jobtype_name);
      this.reactiveForm.get("employee_joiningdate")?.setValue(this.employeeedit_list[0].employee_joiningdate);
      this.reactiveForm.get("biometricid")?.setValue(this.employeeedit_list[0].biometric_id);
      this.reactiveForm.get("password")?.setValue(this.employeeedit_list[0].user_password);
      this.reactiveForm.get("confirmpassword")?.setValue(this.employeeedit_list[0].user_password);
      this.reactiveForm.get("reportingto")?.setValue(this.employeeedit_list[0].employeereporting_to);
      this.reactiveForm.get("shift")?.setValue(this.employeeedit_list[0].shift);
      this.reactiveForm.get("leavegrade")?.setValue(this.employeeedit_list[0].leavegrade_name);
      this.reactiveForm.get("tagid")?.setValue(this.employeeedit_list[0].tagid);
      this.reactiveForm.get("holidaygrade")?.setValue(this.employeeedit_list[0].holidaygrade_name);
      this.reactiveForm.get("dob")?.setValue(this.employeeedit_list[0].employee_dob);
      this.reactiveForm.get("aadhar_no")?.setValue(this.employeeedit_list[0].identity_no);
      this.reactiveForm.get("father_spouse")?.setValue(this.employeeedit_list[0].father_name);
      this.reactiveForm.get("bloodgroup")?.setValue(this.employeeedit_list[0].bloodgroup_name);
      this.reactiveForm.get("qualification")?.setValue(this.employeeedit_list[0].employee_qualification);
      this.reactiveForm.get("differentlyabled")?.setValue(this.employeeedit_list[0].employee_diffabled);
      this.reactiveForm.get("countryname")?.setValue(this.employeeedit_list[0].temporary_country);
      this.reactiveForm.get("permanent_address1")?.setValue(this.employeeedit_list[0].permanent_address1);
      this.reactiveForm.get("permanent_address2")?.setValue(this.employeeedit_list[0].permanent_address2);
      this.reactiveForm.get("permanent_city")?.setValue(this.employeeedit_list[0].permanent_city);
      this.reactiveForm.get("permanent_postal")?.setValue(this.employeeedit_list[0].permanent_postalcode);
      this.reactiveForm.get("permanent_state")?.setValue(this.employeeedit_list[0].permanent_state);
      this.reactiveForm.get("country")?.setValue(this.employeeedit_list[0].permanent_country);
      this.reactiveForm.get("temporary_address1")?.setValue(this.employeeedit_list[0].temporary_address1);
      this.reactiveForm.get("temporary_address2")?.setValue(this.employeeedit_list[0].temporary_address2);
      this.reactiveForm.get("temporary_city")?.setValue(this.employeeedit_list[0].temporary_city);
      this.reactiveForm.get("temporary_postal")?.setValue(this.employeeedit_list[0].temporary_postalcode);
      this.reactiveForm.get("temporary_state")?.setValue(this.employeeedit_list[0].temporary_state);
    });
    
  }
  onChange2(event: any) {
    this.file = event.target.files[0];
   
  }

  // agecalculation(): number {
  //   debugger;
  //   const today: Date = new Date();
    
  //   const birthDate: Date = new Date(this.reactiveForm.value.dob);
  //   let age: number = today.getFullYear() - birthDate.getFullYear();
  //   const monthDiff: number = today.getMonth() - birthDate.getMonth();

  //   if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
  //     age--;
  //   }
  //   const age_calc = age;
  //   this.reactiveForm.get("age")?.setValue(age_calc);
  //   return age;
  // }

  get branchname() {
    return this.reactiveForm.get('branchname')!;
  }
  get departmentname() {
    return this.reactiveForm.get('departmentname')!;
  }
  get role() {
    return this.reactiveForm.get('role')!;
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
  get employee_joiningdate() {
    return this.reactiveForm.get('employee_joiningdate')!;
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
  // get entityname() {
  //   return this.reactiveForm.get('entityname')!;
  // }
 
  public validate(): void {
    this.employee = this.reactiveForm.value;
    if (
       this.reactiveForm.value.branchname != null && this.reactiveForm.value.branchname !== '' &&
       this.reactiveForm.value.departmentname != null && this.reactiveForm.value.departmentname !== '' &&
       this.reactiveForm.value.role != null && this.reactiveForm.value.role !== '' && 
       this.reactiveForm.value.designationname != null && this.reactiveForm.value.designationname !== '' && 
       this.reactiveForm.value.employee_joiningdate != null && this.reactiveForm.value.employee_joiningdate !== '' && 
       this.reactiveForm.value.shift != null && this.reactiveForm.value.shift !== '' && 
       this.reactiveForm.value.first_name != null && this.reactiveForm.value.first_name !== '' && 
       this.reactiveForm.value.dob != null && this.reactiveForm.value.dob !== '') {
       
       this.reactiveForm.value;
        var api7 = 'HrmTrnAdmincontrol/UpdateEmployeedetails'
        this.NgxSpinnerService.show();
        //console.log(this.file)
        this.service.post(api7, this.employee).subscribe((result: any) => {

          if (result.status == true) {
           
            this.route.navigate(['/hrm/HrmTrnAdmincontrol']);
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide()
          }
          else {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide()
          }
        });
       }
        else {
          this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
        }  
      }
    }
  
  
  




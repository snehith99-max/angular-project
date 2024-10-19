import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe } from '@angular/common';

interface IEmployee {
  entityname: string;
  branchname: string;
  departmentname: string;
  workertype: string;
  role: string;
  jobtype: string;
  designationname: string;
  employee_joiningdate: Date;
  active_flag: string;
  user_code: string;
  biometricid: string;
  password: string;
  confirmpassword: string;
  reportingto: string;
  shift: string;
  leavegrade: string;
  tagid: string;
  hide_flag: string;
  holidaygrade: string;

  first_name: string;
  last_name: string;
  gender: string;
  dob: Date;
  age: string;
  aadhar_no: string;
  father_spouse: string;
  mobile: string;
  mobileno: string;
  email: string;
  comp_email: string;
  bloodgroup: string;
  qualification: string;
  differentlyabled: string;
  remarks: string;
  usergroup: string;

  permanent_address1: string;
  permanent_address2: string;
  permanent_city: string;
  permanent_postal: string;
  permanent_state: string;
  country: string;

  temporary_address1: string;
  temporary_address2: string;
  temporary_city: string;
  temporary_postal: string;
  temporary_state: string;
  countryname: string;
  user_password: string;  
  probationenddate:string;
}

@Component({
  selector: 'app-hrm-mst-employeeadd',
  templateUrl: './hrm-mst-employeeadd.component.html',
  styleUrls: ['./hrm-mst-employeeadd.component.scss']
})

export class HrmMstEmployeeaddComponent implements OnInit {
  file!: File;
  file1!: File;
  file2!: File;
  employee!: IEmployee;
  reactiveForm!: FormGroup;
  entity_list: any;
  branch_list: any[] = [];
  department_list: any[] = [];
  designation_list: any[] = [];
  country_list: any[] = [];
  country_list2: any[] = [];
  bloodgroup_list: any[] = [];

  selectedEmpPassword: any;
  user_gid:any;

  entityList: any;
  showPassword: boolean = false;
  branchList: any;
  departmentList: any[] = []
  workertype_list: any[] = []
  role_list: any[] = []
  Getonchangerolelist:any[]=[]
  jobtype_list: any[] = []
  shift_list: any[] = []
  leavegrade_list: any[] = []
  holidaygrade_list: any[] = []
  employeereportingto_list: any[] = []
  usergroup_list:any[]=[]
  Entity: any;
  Emp_Code: any;
  Emp_Join_date: any;
  selectedEmpAccess: string = 'Yes';
  Email_Address: any;

  responsedata: any;
  role_name: any;  

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  formdata = { selectedEmpPassword: "", Confrim_Emp_password: "", Emp_Code: "", Emp_Join_date: "", Emp_mobile_number: "", Email_Address: "" }

  mdlEntity: any;
  mdlBranch: any;
  mdlDepartment: any;
  mdlWorkerType: any;
  mdlJobType: any;
  mdlDesignation: any;
  mdlReportingTo: any;
  mdlShift: any;
  mdlLeaveGrade: any;
  mdlCountryTemp: any;

  mdlRole: any;
  mdlHolidayGrade: any;
  mdlBloodGroup: any;
  mdlCountry: any;
  mdlusergroup:any;
  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    public NgxSpinnerService:NgxSpinnerService,
    private route: Router,
    private datePipe: DatePipe) {
    this.employee = {} as IEmployee;
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    this.reactiveForm = new FormGroup({
      role_gid: new FormControl(''),
      branchname: new FormControl('', [Validators.required]),
      departmentname: new FormControl('', [Validators.required]),
      workertype: new FormControl('', [Validators.required]),
      role: new FormControl('', [Validators.required]),
      jobtype: new FormControl('', [Validators.required]),
      designationname: new FormControl('', [Validators.required]),
      employee_joiningdate: new FormControl('', [Validators.required]),
      active_flag: new FormControl('Y'),
      user_code: new FormControl('', [Validators.required]),
      biometricid: new FormControl(''),
      password: new FormControl('',  [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/)]),
      confirmpassword: new FormControl('', [Validators.required,Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/)]),
      reportingto: new FormControl(''),
      shift: new FormControl('', [Validators.required]),
      leavegrade: new FormControl(''),
      tagid: new FormControl(''),
      hide_flag: new FormControl('Y'),
      file: new FormControl(''),
      file1: new FormControl(''),
      file2: new FormControl(''),
      holidaygrade: new FormControl(''),
      usergroup:new FormControl(''),
      probation_period: new FormControl(''),
      probationenddate: new FormControl(''),
      employee_photo: new FormControl(''),
      employee_sign: new FormControl(''),

      first_name: new FormControl('', [Validators.required]),
      last_name: new FormControl(''),
      gender: new FormControl('male'),
      dob: new FormControl('', [Validators.required]),
      age: new FormControl(''),
      aadhar_no: new FormControl(''),
      
      father_spouse: new FormControl(''),
      mobile: new FormControl(''),
      mobileno: new FormControl(''),
      
      email: new FormControl(''),
    
      comp_email: new FormControl('', [Validators.pattern(/^[a-z0-9._%+-]+@(?!gmail\.com$)(?!yahoo\.com$)(?!hotmail\.com$)(?!outlook\.com$)(?!live\.com$)[a-z0-9.-]+\.[a-z]{2,100}$/)]),
      user_phtoto: new FormControl('', [Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')]),
      bloodgroup: new FormControl(''),
      qualification: new FormControl(''),
      differentlyabled: new FormControl('N'),
      remarks: new FormControl(''),

      permanent_address1: new FormControl(''),
      permanent_address2: new FormControl(''),
      permanent_city: new FormControl(''),
      permanent_postal: new FormControl(''),
      permanent_state: new FormControl(''),
      country: new FormControl(''),

      temporary_address1: new FormControl(''),
      temporary_address2: new FormControl(''),
      temporary_city: new FormControl(''),
      temporary_postal: new FormControl(''),
      temporary_state: new FormControl(''),
      countryname: new FormControl(''),
    });

    this.reactiveForm.get('password')?.valueChanges.subscribe(value => {
      this.reactiveForm.get('confirmpassword')?.setValue(value);
    });

    var url = 'HrmTrnAdmincontrol/Getbranchdropdown';
    this.service.get(url).subscribe((result: any) => {
      this.branchList = result.Getbranchdropdown;
    });

    var url = 'HrmTrnAdmincontrol/Getreportingtodropdown';
    this.service.get(url).subscribe((result: any) => {
      this.employeereportingto_list = result.Getreportingtodropdown;
    });

    var url = 'HrmTrnAdmincontrol/Getworkertypedropdown';
    this.service.get(url).subscribe((result: any) => {
      this.workertype_list = result.Getworkertypedropdown;
    });

    var url = 'HrmTrnAdmincontrol/Getroledropdown';
    this.service.get(url).subscribe((result: any) => {
      this.role_list = result.Getroledropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getbloodgroupdropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.bloodgroup_list = result.Getbloodgroupdropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getentitydropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.entity_list = result.Getentitydropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getleavegradedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.leavegrade_list = result.Getleavegradenamedropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getjobtypedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.jobtype_list = result.Getjobtypenamedropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getshifttypedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.shift_list = result.Getshifttypenamedropdown;
    });

    var api1 = 'HrmTrnAdmincontrol/Getholidaygradedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.holidaygrade_list = result.Getholidaygradedropdown;
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

    this.reactiveForm.get('employee_joiningdate')?.valueChanges.subscribe(() => {
        this.calculateProbationEndDate();
    });
    // var url = 'HrmTrnAdmincontrol/Getusergroupdropdown';
    // this.service.get(url).subscribe((result: any) => {
    //   this.usergroup_list = result.Getusergrouptemplatedropdown;
    // });

  }

  onChange2(event: any) {
    this.file2 = event.target.files[0];
  }
  onChange1(event: any) {
    this.file1 = event.target.files[0];
  }

 

  get branchname() {
    return this.reactiveForm.get('branchname')!;
  }

  get departmentname() {
    return this.reactiveForm.get('departmentname')!;
  }

  get workertype() {
    return this.reactiveForm.get('workertype')!;
  }

  get role() {
    return this.reactiveForm.get('role')!;
  }

  get jobtype() {
    return this.reactiveForm.get('jobtype')!;
  }

  get designationname() {
    return this.reactiveForm.get('designationname')!;
  }

  get employee_joiningdate() {
    return this.reactiveForm.get('employee_joiningdate')!;
  }

  get user_code() {
    return this.reactiveForm.get('user_code')!;
  }

  get user_password() {
    return this.reactiveForm.get('user_password')!;
  }

  get password() {
    return this.reactiveForm.get('password')!;
  }

  get confirmpassword() {
    return this.reactiveForm.get('confirmpassword')!;
  }

  get shift() {
    return this.reactiveForm.get('shift')!;
  }

  get first_name() {
    return this.reactiveForm.get('first_name')!;
  }
  
  get dob() {
    return this.reactiveForm.get('dob')!;
  }
  
  get mobile() {
    return this.reactiveForm.get('mobile')!;
  }
  
  get mobileno() {
    return this.reactiveForm.get('mobileno')!;
  }
  
  get email() {
    return this.reactiveForm.get('email')!;
  }
  
  get comp_email() {
    return this.reactiveForm.get('comp_email')!;
  }
  
  get country() {
    return this.reactiveForm.get('country')!;
  }
  
  get countryname() {
    return this.reactiveForm.get('countryname')!;
  }

  userpassword(password: any) {
    this.reactiveForm.get("confirmpassword")?.setValue(password.value);
  }
  
  agecalculation(): number {
    debugger;
    const today: Date = new Date();

    const birthDate: Date = new Date(this.reactiveForm.value.dob);
    let age: number = today.getFullYear() - birthDate.getFullYear();
    const monthDiff: number = today.getMonth() - birthDate.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    const age_calc = age;
    this.reactiveForm.get("age")?.setValue(age_calc);
    return age;
  }
  
  public validate(): void {
    debugger;
    console.log(this.reactiveForm.value)

    this.employee = this.reactiveForm.value;
    const dob1 = new Date(this.employee.dob);
    const year = dob1.getFullYear();
    const month = ('0' + (dob1.getMonth() + 1)).slice(-2); // Adding 1 as getMonth() returns zero-based month
    const day = ('0' + dob1.getDate()).slice(-2);
    const dob: string = `${year}-${month}-${day}`;

    const employee_joiningdate1 = new Date(this.employee.employee_joiningdate); // Convert date string to Date object
    const year1 = employee_joiningdate1.getFullYear();
    const month1 = ('0' + (employee_joiningdate1.getMonth() + 1)).slice(-2); // Adding 1 as getMonth() returns zero-based month
    const day1 = ('0' + employee_joiningdate1.getDate()).slice(-2);
    const employee_joiningdate: string = `${year1}-${month1}-${day1}`;

    if ( this.employee.branchname != null && this.employee.departmentname != null && this.employee.designationname != null && this.employee.first_name != null && this.employee.active_flag != null && this.employee.password != null && this.employee.email != null && this.employee.mobile != null) {
      let formData = new FormData();
      if (this.file1 != null && this.file1 != undefined && this.file2 != null && this.file2 != undefined) {
        // formData.append("file", this.file, this.file.name);
        formData.append("employee_photo", this.file1, this.file1.name);
        formData.append("employee_sign", this.file2, this.file2.name);
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
        formData.append("workertype", this.employee.workertype);
        formData.append("role", this.employee.role);
        formData.append("jobtype", this.employee.jobtype);
        formData.append("employee_joiningdate", employee_joiningdate);
        formData.append("reportingto", this.employee.reportingto);
        formData.append("shift", this.employee.shift);
        formData.append("leavegrade", this.employee.leavegrade);
        formData.append("tagid", this.employee.tagid);
        formData.append("holidaygrade", this.employee.holidaygrade);
        formData.append("dob", dob);
        formData.append("age", this.employee.age);
        formData.append("aadhar_no", this.employee.aadhar_no);
        formData.append("father_spouse", this.employee.father_spouse);
        formData.append("mobileno", this.employee.mobileno);
        formData.append("comp_email", this.employee.comp_email);
        formData.append("bloodgroup", this.employee.bloodgroup);
        formData.append("qualification", this.employee.qualification);
        formData.append("remarks", this.employee.remarks);
        formData.append("differentlyabled", this.employee.differentlyabled);
        formData.append("probationenddate", this.employee.probationenddate);
        formData.append("usergroup",this.employee.usergroup)

        var api = 'HrmTrnAdmincontrol/EmployeeProfileUpload'
        this.NgxSpinnerService.show()

        this.service.postfile(api, formData).subscribe((result: any) => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide()
          }
          else {
            this.route.navigate(['/hrm/HrmTrnAdmincontrol']);
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide()
          }
        });
      }
      else {
        var api7 = 'HrmTrnAdmincontrol/PostEmployeedetails'
        this.NgxSpinnerService.show()

        this.service.post(api7, this.employee).subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning('Error Occured While Adding Employee Details')
            this.NgxSpinnerService.hide()
          }
          else {
            this.route.navigate(['/hrm/HrmTrnAdmincontrol']);
            this.ToastrService.success('Employee Details Added Successfully')
            this.NgxSpinnerService.hide()
          }
          this.responsedata = result;
        });
      }
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    return;    
  }

  OnChangeRole() {    
    let role_gid = this.reactiveForm.get("role")?.value;

    let param = {
      role_gid: role_gid
    }

    var url = 'HrmTrnAdmincontrol/GetOnChangeRole';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Getonchangerolelist = this.responsedata.Getonchangerolelist;
      this.reactiveForm.get("probation_period")?.setValue(this.Getonchangerolelist[0].probation_period);
      // this.role_name = this.role_list[0].role_name;
    });
  }
  

  calculateProbationEndDate() {
  debugger
    const joiningDate = new Date(this.reactiveForm.get('employee_joiningdate')?.value);
    const probationPeriod = this.reactiveForm.get('probation_period')?.value;
  
    if (probationPeriod > 0) {
        let probationEndDate = new Date(joiningDate); // Initialize with joining date
        let remainingMonths = probationPeriod;

        // Add probation period iteratively
        while (remainingMonths > 0) {  
            const currentMonth = probationEndDate.getMonth();
            const monthsToAdd = Math.min(remainingMonths, 12 - currentMonth); // Calculate months to add this year

            probationEndDate.setMonth(currentMonth + monthsToAdd); // Add months to the current date

            // Check if the next month will be in the next year
            if (currentMonth + monthsToAdd >= 12) {
                probationEndDate.setFullYear(probationEndDate.getFullYear());
            }

            remainingMonths -= monthsToAdd; // Update remaining months
        }


      const formattedDate = probationEndDate.toISOString().split('T')[0];
      this.reactiveForm.get('probationenddate')?.setValue(formattedDate);
  
      console.log(joiningDate);
      console.log(probationPeriod);
      console.log(probationEndDate);
    } else {      
      this.reactiveForm.get('probationenddate')?.setValue(null);      // Set probation end date to null or empty string
    }
  }
}



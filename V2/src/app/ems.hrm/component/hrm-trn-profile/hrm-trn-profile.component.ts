import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';


interface IProfileManagement {

  // Personal Details
  first_name: string;  
  last_name: string;  
  gender: string;  
  dateof_birth: string;  
  mobile: string;
  personal_no: string;  
  qualification: string;  
  blood_group: string;  
  experience: string;

  firstedit_name: string;
  lastedit_name: string;
  gender_edit: string;
  employeestatus_edit: string;
  dateof_birthedit: string;
  mobile_edit: string;
  personal_noedit: string;
  qualification_edit: string;
  blood_groupedit: string;
  experience_edit: string;
  comp_email: string;
  employee_dateedit: string;
 
  //personal details


  // Change Password
  curr_pwd: string;  
  new_pwd: string;  
  conf_pwd: string; 

  curredit_pwd: string;
  newedit_pwd: string;
  confedit_pwd: string;
  
  showPassword: boolean;
  showCurrentPassword: boolean;
  
  // Work Experience
  empl_prevcomp: string;
  empl_code: string;
  prev_occp: string;
  department: string;
  date_ofjoining: string;
  date_ofreleiving: string;
  work_period: string;
  HR_name: string;
  reason: string;
  report: string;
  rmrks: string;

  // Nomination
  name: string;
  dateofbirth: string;
  age: string;
  mobile_no: string;
  relt_employee: string;
  resign_employee: string;
  residing_town_state: string;
  residing_addr: string;
  nominee_for: string;

  // Statutory
  provident_no: string;
  date_ofjoinPF: string;
  employee_no: string;

  // Emergency Contact
  contact_person: string;
  cont_addr: string;
  cont_no: string;
  cont_emailid: string;
  remarks: string;

  // Dependent
  name_user: string;
  relationship: string;
  date_ofbirth: string;

  // Education
  inst_name: string;
  deg_dip: string;
  field_ofstudy: string;
  date_ofcompletion: string;
  addn_notes: string;
}

@Component({
  selector: 'app-hrm-trn-profile',
  templateUrl: './hrm-trn-profile.component.html',
})

export class HrmTrnProfileComponent {
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
  togglePasswordVisibility1(): void {
    this.showPassword1 = !this.showPassword1;
  }
  togglePasswordVisibility2(): void {
    this.showPassword2 = !this.showPassword2;
  }
 
  // showOptionsDivId: any;
  mdlbloodgroup: any;
  file!: File;
  parameterValue: any;
  temp_list: any;
  showPassword: boolean = false;
  showPassword1: boolean = false;
  showPassword2: boolean = false;
  verificationCompleted1: boolean = false;
  showVerifyDetail: boolean = false;


  
  reactiveForm1!: FormGroup;
  reactiveForm2!: FormGroup;
  reactiveForm3!: FormGroup;
  reactiveForm4!: FormGroup;
  reactiveForm5!: FormGroup;
  reactiveForm6!: FormGroup;

  reactiveForm8Edit!: FormGroup;
  reactiveForm7Edit!: FormGroup;

  profilemanagement!: IProfileManagement;
 
  emp_name: any;
  emp_code: any;
  emp_mobilenumber: any;
  emp_department: any;
  emp_designation: any;
  emp_branch: any;
  emp_Gender: any;
  emp_Joiningdate: any;
  emp_email: any;
  emp_address: any;
  relationshipwith_employee_list: any;
  // policies_list: any[] = [];
  // bloodgroup_list: any;
  workexperiencedtllist: any;
  nominationdtllist: any;
  statutorylist: any;
  emergencylist: any;
  dependentdtllist: any;
  educationdtllist: any;
  employeename_list: any;
  selectedBranch: any;
  selectedEmployee: any;
  employeeedit_list: any;

  responsedata: any;
  parameterValue1: any;
  parameterValue2: any;
  parameterValue3: any;
  parameterValue4: any;
  parameterValue5: any;
  parameterValue6: any;
  parameterValue7: any;
  reside_user: any;
  formdata = { }

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private NgxSpinnerService:NgxSpinnerService, private route: ActivatedRoute, private router: Router,) {
    this.profilemanagement = {} as IProfileManagement;

    // Personal Details
    this.reactiveForm7Edit = new FormGroup({
      firstedit_name: new FormControl(this.profilemanagement.firstedit_name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      lastedit_name: new FormControl(this.profilemanagement.lastedit_name, [Validators.pattern(/^\S.*$/)]),
      dateof_birthedit: new FormControl(this.profilemanagement.dateof_birthedit, [Validators.required]),
      gender_edit: new FormControl(this.profilemanagement.gender_edit, []),
      employeestatus_edit: new FormControl(this.profilemanagement.employeestatus_edit, []),
      mobile_edit: new FormControl(this.profilemanagement.mobile_edit, [Validators.pattern('[0-9]{10}$'), Validators.maxLength(10)]),
      personal_noedit: new FormControl(this.profilemanagement.personal_noedit, [Validators.pattern('[0-9]{10}$'), Validators.maxLength(10)]),
      blood_groupedit: new FormControl(this.profilemanagement.blood_groupedit, []),
      qualification_edit: new FormControl(this.profilemanagement.qualification_edit, [Validators.pattern(/^\S.*$/)]),
      experience_edit: new FormControl(this.profilemanagement.experience_edit, [Validators.pattern(/^\S.*$/)]),
      comp_email: new FormControl(this.profilemanagement.comp_email, [Validators.pattern(/^[a-z0-9._%+-]+@(?!gmail\.com$)(?!yahoo\.com$)(?!hotmail\.com$)(?!outlook\.com$)(?!live\.com$)[a-z0-9.-]+\.[a-z]{2,100}$/)]),
      employee_dateedit: new FormControl(this.profilemanagement.employee_dateedit, []),
      
      
    });

    // Change Password
    this.reactiveForm8Edit = new FormGroup({
      curredit_pwd: new FormControl(this.profilemanagement.curredit_pwd, [Validators.required,]),
      newedit_pwd: new FormControl(this.profilemanagement.newedit_pwd, [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/)]),
      confedit_pwd: new FormControl(this.profilemanagement.confedit_pwd, []),
    });

    this.reactiveForm8Edit.get('newedit_pwd')?.valueChanges.subscribe(value => {
      this.reactiveForm8Edit.get('confedit_pwd')?.setValue(value);
    });

    // Work Experience
    this.reactiveForm6 = new FormGroup({
      empl_prevcomp: new FormControl(this.profilemanagement.empl_prevcomp, [Validators.required,Validators.pattern(/^\S.*$/)]),
      empl_code: new FormControl(this.profilemanagement.empl_code, [Validators.required,Validators.pattern(/^\S.*$/)]),
      prev_occp: new FormControl(this.profilemanagement.prev_occp, [Validators.required,Validators.pattern(/^\S.*$/)]),
      department: new FormControl(this.profilemanagement.department, [Validators.required,Validators.pattern(/^\S.*$/)]),
      date_ofjoining: new FormControl(this.profilemanagement.date_ofjoining, [Validators.required,]),
      date_ofreleiving: new FormControl(this.profilemanagement.date_ofreleiving, [Validators.required,]),
      work_period: new FormControl(this.profilemanagement.work_period, [Validators.required,Validators.pattern(/^\S.*$/)]),
      HR_name: new FormControl(this.profilemanagement.HR_name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      reason: new FormControl(this.profilemanagement.reason, [Validators.required,Validators.pattern(/^\S.*$/)]),
      report: new FormControl(this.profilemanagement.report, []),
      rmrks: new FormControl(this.profilemanagement.rmrks, [Validators.required,Validators.pattern(/^\S.*$/)]),
      
    });

    // Nomination
    this.reactiveForm5 = new FormGroup({
      name: new FormControl(this.profilemanagement.name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      dateofbirth: new FormControl(this.profilemanagement.dateofbirth, [Validators.required,]),
      age: new FormControl(this.profilemanagement.age, [Validators.required,Validators.pattern(/^\S.*$/)]),
      mobile_no: new FormControl(this.profilemanagement.mobile_no, [Validators.pattern('[0-9]{10}$'), Validators.maxLength(10)]),
      relt_employee: new FormControl(this.profilemanagement.relt_employee, [Validators.required,]),
      resign_employee: new FormControl(this.profilemanagement.resign_employee, []),
      residing_town_state: new FormControl(this.profilemanagement.residing_town_state, []),
      residing_addr: new FormControl(this.profilemanagement.residing_addr, []),
      nominee_for: new FormControl(this.profilemanagement.nominee_for, [Validators.required,Validators.pattern(/^\S.*$/)]),
    });

    // Statutory
    this.reactiveForm1 = new FormGroup({
      provident_no: new FormControl(this.profilemanagement.provident_no, [Validators.required,Validators.pattern(/^\S.*$/)]),
      date_ofjoinPF: new FormControl(this.profilemanagement.date_ofjoinPF, [Validators.required,]),
      employee_no: new FormControl(this.profilemanagement.employee_no, [Validators.required,Validators.pattern(/^\S.*$/),Validators.pattern('[0-9]{10}$'), Validators.maxLength(10)]),
    });

    // Emergency Contact
    this.reactiveForm3 = new FormGroup({
      contact_person: new FormControl(this.profilemanagement.contact_person, [Validators.required,Validators.pattern(/^\S.*$/)]),
      cont_addr: new FormControl(this.profilemanagement.cont_addr, [Validators.required,Validators.pattern(/^\S.*$/)]),
      cont_no: new FormControl(this.profilemanagement.cont_no, [Validators.required, Validators.pattern('[0-9]{10}$'), Validators.maxLength(10)]),
      cont_emailid: new FormControl(this.profilemanagement.cont_emailid, [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]),
      remarks: new FormControl(this.profilemanagement.remarks, []),
    });

    // Dependent
    this.reactiveForm2 = new FormGroup({
      name_user: new FormControl(this.profilemanagement.name_user, [Validators.required,Validators.pattern(/^\S.*$/)

      ]),
      relationship: new FormControl(this.profilemanagement.relationship, [Validators.required,Validators.pattern(/^\S.*$/)]),
      date_ofbirth: new FormControl(this.profilemanagement.date_ofbirth, [Validators.required,]),
      dependent_gid: new FormControl(''),
    });

    // Education
    this.reactiveForm4 = new FormGroup({
      inst_name: new FormControl(this.profilemanagement.inst_name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      deg_dip: new FormControl(this.profilemanagement.deg_dip, [Validators.required,Validators.pattern(/^\S.*$/)]),
      field_ofstudy: new FormControl(this.profilemanagement.field_ofstudy, [Validators.required,Validators.pattern(/^\S.*$/)]),
      date_ofcompletion: new FormControl(this.profilemanagement.date_ofcompletion, [Validators.required,]),
      addn_notes: new FormControl(this.profilemanagement.addn_notes, []),
      education_gid: new FormControl(''),
    });

  }

  back() {
    this.router.navigate(['/hrm/HrmMemberDashboard'])
  }

  // Personal Details
  get firstedit_name() {
    return this.reactiveForm7Edit.get('firstedit_name')!;
  }
  get dateof_birthedit() {
    return this.reactiveForm7Edit.get('dateof_birthedit')!;
  }
  get mobile_edit() {
    return this.reactiveForm7Edit.get('mobile_edit')!;
  }
  get personal_noedit() {
    return this.reactiveForm7Edit.get('personal_noedit')!;
  }
 
  //  get blood_groupedit() {
  //    return this.reactiveForm7Edit.get('blood_groupedit')!;
  //  }
  get comp_email() {
    return this.reactiveForm7Edit.get('comp_email')!;
  }
 
  
 

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);  
    this.getEducationSummary();
    this.getdependentSummary();
    this.getemergencysummary();
    this.getstaturarySummary();
    this.getnominationsummary();
    this.getworkexpsummary();
    

   

    var url = 'HrmTrnProfileManagement/employeeList'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.employeename_list = this.responsedata.employeenamelist;
      this.emp_name = this.employeename_list[0].Name;
      this.emp_code = this.employeename_list[0].UserCode;
      this.emp_mobilenumber = this.employeename_list[0].employeemobileNo;
      this.emp_department = this.employeename_list[0].Department;
      this.emp_designation = this.employeename_list[0].Designation;
      this.emp_branch = this.employeename_list[0].Branch;
      this.emp_address = this.employeename_list[0].Address;
      this.emp_Gender = this.employeename_list[0].Gender;
      this.emp_Joiningdate = this.employeename_list[0].Joiningdate;
      this.emp_email = this.employeename_list[0].comp_email;
    });

    // var url = 'HrmTrnProfileManagement/GetCompanyPolicies';
    // this.service.get(url).subscribe((result: any) => {
    //   this.policies_list = result.CompanyPolicy;
    // });

    // var url = 'HrmTrnProfileManagement/GetBloodGroup';
    // this.service.get(url).subscribe((result: any) => {
    //   this.bloodgroup_list = result.bloodgroup_list;
    // });

    var url = 'HrmTrnProfileManagement/Getrelationshipwithemployee';
    this.service.get(url).subscribe((result: any) => {
      this.relationshipwith_employee_list = result.relationshipwith_employee_list;
    });
     this.GetEditEmployee();


     var url = 'HrmTrnProfileManagement/GetCompanyPolicysummary'
     debugger;
    
     this.service.get(url).subscribe((result: any) => {
       this.responsedata = result;
       this.temp_list = this.responsedata.templatelevel_list;
     });
  }

  preprocessPolicyDesc(policyDesc: string): string {
    // Replace the tab character with a line break
    return policyDesc.replace(/\t/g, '<br>');
  }

  getworkexpsummary(){
    var url = 'HrmTrnProfileManagement/GetWorkExperienceSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.workexperiencedtllist = this.responsedata.workexperiencelist;
    });
  }

  getnominationsummary(){
    var url = 'HrmTrnProfileManagement/GetNominationSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.nominationdtllist = this.responsedata.nominationlist;
    });
  }
  
  getstaturarySummary(){
    var url = 'HrmTrnProfileManagement/GetStatutorySummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.statutorylist = this.responsedata.statutorylist;
    });
  }

  getemergencysummary(){
    var url = 'HrmTrnProfileManagement/GetEmergencyContactSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.emergencylist = this.responsedata.emergencycontactlist;
    });
  }

  getdependentSummary(){
    var url = 'HrmTrnProfileManagement/GetDependentSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.dependentdtllist = this.responsedata.dependentlist;
    });
  }

  getEducationSummary(){
    var url = 'HrmTrnProfileManagement/GetEducationSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.educationdtllist = this.responsedata.educationlist;
    });

  }
  GetEditEmployee() {

    var url = 'HrmTrnProfileManagement/GetEditEmployee'
   
    this.service.get(url).subscribe((result: any) => {
    this.responsedata=result;
      this.employeeedit_list = result.GetEditEmployee;
      console.log(this.employeeedit_list)
      console.log(this.employeeedit_list[0].branch_gid)
     
      this.selectedBranch = this.employeeedit_list[0].branch_gid;
      this.selectedEmployee = this.employeeedit_list[0].employee_gid;
      this.reactiveForm7Edit.get("firstedit_name")?.setValue(this.employeeedit_list[0].user_firstname);
      this.reactiveForm7Edit.get("lastedit_name")?.setValue(this.employeeedit_list[0].user_lastname);
      this.reactiveForm7Edit.get("dateof_birthedit")?.setValue(this.employeeedit_list[0].employee_dob);
      this.reactiveForm7Edit.get("blood_groupedit")?.setValue(this.employeeedit_list[0].bloodgroup_name);
      this.reactiveForm7Edit.get("gender_edit")?.setValue(this.employeeedit_list[0].employee_gender);
      this.reactiveForm7Edit.get("employeestatus_edit")?.setValue(this.employeeedit_list[0].user_status);
      this.reactiveForm7Edit.get("mobile_edit")?.setValue(this.employeeedit_list[0].employee_mobileno);
      this.reactiveForm7Edit.get("personal_noedit")?.setValue(this.employeeedit_list[0].employee_personalno);
      this.reactiveForm7Edit.get("qualification_edit")?.setValue(this.employeeedit_list[0].employee_qualification);
      this.reactiveForm7Edit.get("experience_edit")?.setValue(this.employeeedit_list[0].employee_experience);
      this.reactiveForm7Edit.get("comp_email")?.setValue(this.employeeedit_list[0].comp_email);
      this.reactiveForm7Edit.get("employee_dateedit")?.setValue(this.employeeedit_list[0].employee_joingdate);
      
      this.reactiveForm7Edit.get("employee_gid")?.setValue(this.employeeedit_list[0].employee_gid);
    
     
    });
  }

  public detailsupdate(): void {
    debugger;
    let param= {
      firstedit_name: this.reactiveForm7Edit.value.firstedit_name,
      lastedit_name: this.reactiveForm7Edit.value.lastedit_name,
      gender_edit: this.reactiveForm7Edit.value.gender_edit,
      employeestatus_edit: this.reactiveForm7Edit.value.employeestatus_edit,
      dateof_birthedit: this.reactiveForm7Edit.value.dateof_birthedit,
      blood_groupedit: this.reactiveForm7Edit.value.blood_groupedit,
       mobile_edit: this.reactiveForm7Edit.value.mobile_edit,
       personal_noedit: this.reactiveForm7Edit.value.personal_noedit,
       qualification_edit: this.reactiveForm7Edit.value.qualification_edit,
       experience_edit: this.reactiveForm7Edit.value.experience_edit,
       comp_email: this.reactiveForm7Edit.value.comp_email,
       employee_dateedit: this.reactiveForm7Edit.value.employee_dateedit,
       
    }
      
      if (this.reactiveForm7Edit.value.firstedit_name != null && this.reactiveForm7Edit.value.firstedit_name != '') {
      for (const control of Object.keys(this.reactiveForm7Edit.controls)) {
        this.reactiveForm7Edit.controls[control].markAsTouched();
      }
      this.reactiveForm7Edit.value;
      var url22 = 'HrmTrnProfileManagement/UpdatePersonalDetails'
      this.service.postparams(url22, param).pipe().subscribe((result: { status: boolean; message: string | undefined; }) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
        }
       
      });
      setTimeout(function () {
        window.location.reload();
      }, 2000);
    
  }
  else {
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }   
}
    
  
  

  // Change Password
  get curredit_pwd() {
    return this.reactiveForm8Edit.get('curredit_pwd')!;
  }
  get newedit_pwd() {
    return this.reactiveForm8Edit.get('newedit_pwd')!;
  }
  get confedit_pwd() {
    return this.reactiveForm8Edit.get('confedit_pwd')!;
  }
  newpassword(newedit_pwd: any) {
    this.reactiveForm8Edit.get("confedit_pwd")?.setValue(newedit_pwd.value);
  }
  currentpassword(curredit_pwd: any) {
    this.reactiveForm8Edit.get("curredit_pwd")?.setValue(curredit_pwd.value);
  }

  // verifydetail(event: any): void {
  //   debugger;
  //   if (event) {
  //     this.showVerifyDetail = true;
  //   } else {
  //     this.showVerifyDetail = false;
  //   }
  // }

//   verifyforreset(){
//     var url = 'HrmTrnProfileManagement/changepasswordcheck';

//     let param={
//       curredit_pwd: this.reactiveForm8Edit.value.curredit_pwd

//     }
//     if (this.reactiveForm8Edit.value.curredit_pwd != null && this.reactiveForm8Edit.value.curredit_pwd != '') {
//       for (const control of Object.keys(this.reactiveForm8Edit.controls)) {
//         this.reactiveForm8Edit.controls[control].markAsTouched();
//       }
//       this.reactiveForm8Edit.value;
//     this.service.post(url,param).subscribe((result: any) => {

//       if (result.status == false) {
//         this.ToastrService.warning(result.message)
//         // this.showVerifyDetail = result.status;
//       }
//       else {
//         this.ToastrService.success(result.message)    
//         // this.showVerifyDetail = result.status;

//       }
      
//   });
// }
// else {
//   this.ToastrService.warning('Enter Current Password !! ')
// }  

// }

  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveForm8Edit.get("curredit_pwd")?.setValue(this.parameterValue1.curr_pwd);
    this.reactiveForm8Edit.get("newedit_pwd")?.setValue(this.parameterValue1.new_pwd);
    this.reactiveForm8Edit.get("confedit_pwd")?.setValue(this.parameterValue1.conf_pwd);
  }

  public passwordupdate(): void {
      
      var url21 = 'HrmTrnProfileManagement/UpdatePassword'
      this.service.postparams(url21, this.reactiveForm8Edit.value).pipe().subscribe((result: { status: boolean; message: string | undefined; }) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
        }
       
      });
      setTimeout(function () {
        window.location.reload();
      }, 2000);
  }

  // Work Experience
  get empl_prevcomp() {
    return this.reactiveForm6.get('empl_prevcomp')!;
  }
  get empl_code() {
    return this.reactiveForm6.get('empl_code')!;
  }
  get prev_occp() {
    return this.reactiveForm6.get('prev_occp')!;
  }
  get department() {
    return this.reactiveForm6.get('department')!;
  }
  get date_ofjoining() {
    return this.reactiveForm6.get('date_ofjoining')!;
  }
  get date_ofreleiving() {
    return this.reactiveForm6.get('date_ofreleiving')!;
  }
  get work_period() {
    return this.reactiveForm6.get('work_period')!;
  }
  get HR_name() {
    return this.reactiveForm6.get('HR_name')!;
  }
  get reason() {
    return this.reactiveForm6.get('reason')!;
  }
  get report() {
    return this.reactiveForm6.get('report')!;
  }
  get rmrks() {
    return this.reactiveForm6.get('rmrks')!;
  }

  public workexperiencesubmit(): void {
    if (this.reactiveForm6.value.empl_prevcomp != null && this.reactiveForm6.value.empl_prevcomp != '') {
      for (const control of Object.keys(this.reactiveForm6.controls)) {
        this.reactiveForm6.controls[control].markAsTouched();
      }
      this.reactiveForm6.value;
     this.NgxSpinnerService.show();
      var url15 = 'HrmTrnProfileManagement/WorkExperienceSubmit'
      this.service.post(url15, this.reactiveForm6.value).subscribe((result: any) => {
        if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.getworkexpsummary();
        this.NgxSpinnerService.hide();
        }
        else {
          this.NgxSpinnerService.show();

          // this.reactiveForm6.get("empl_prevcomp")?.setValue(null);
          // this.reactiveForm6.get("empl_code")?.setValue(null);
          // this.reactiveForm6.get("prev_occp")?.setValue(null);
          // this.reactiveForm6.get("department")?.setValue(null);
          // this.reactiveForm6.get("date_ofjoining")?.setValue(null);
          // this.reactiveForm6.get("date_ofreleiving")?.setValue(null);
          // this.reactiveForm6.get("work_period")?.setValue(null);
          // this.reactiveForm6.get("HR_name")?.setValue(null);
          // this.reactiveForm6.get("reason")?.setValue(null);
          // this.reactiveForm6.get("report")?.setValue(null);
          // this.reactiveForm6.get("rmrks")?.setValue(null);
          this.ToastrService.success(result.message)
          this.getworkexpsummary()
          this.NgxSpinnerService.hide();

        }
        
      });
     
    }
    setTimeout(function () {
      window.location.reload();
    }, 2000);
   
  }

  openModaldeleteworkexperience(parameter:string){
    this.parameterValue7 = parameter
  }

  ondeleteworkexperience() {
    console.log(this.parameterValue7);
    var url = 'HrmTrnProfileManagement/DeleteWorkExperience'
    this.service.getid(url, this.parameterValue7).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      

    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  }

  // Nomination
  get name() {
    return this.reactiveForm5.get('name')!;
  }
  get dateofbirth() {
    return this.reactiveForm5.get('dateofbirth')!;
  }
  get age() {
    return this.reactiveForm5.get('age')!;
  }
  get mobile_no() {
    return this.reactiveForm5.get('mobile_no')!;
  }
  get relt_employee() {
    return this.reactiveForm5.get('relt_employee')!;
  }
  get resign_employee() {
    return this.reactiveForm5.get('resign_employee')!;
  }
  get residing_town_state() {
    return this.reactiveForm5.get('residing_town_state')!;
  }
  get residing_addr() {
    return this.reactiveForm5.get('residing_addr')!;
  }
  get nominee_for() {
    return this.reactiveForm5.get('nominee_for')!;
  }

  public nominationsubmit(): void {
    
    if (this.reactiveForm5.value.name != null && this.reactiveForm5.value.name != '') {
      for (const control of Object.keys(this.reactiveForm5.controls)) {
        this.reactiveForm5.controls[control].markAsTouched();
      }
      this.reactiveForm5.value;
      var url14 = 'HrmTrnProfileManagement/NominationSubmit'
      this.service.post(url14, this.reactiveForm5.value).subscribe((result: any) => {
        if (result.status == false) {
          this.getnominationsummary()
          this.ToastrService.warning('Error Occured while Adding Nomination')
        }
        else {
          // this.reactiveForm5.get("name")?.setValue(null);
          // this.reactiveForm5.get("dateofbirth")?.setValue(null);
          // this.reactiveForm5.get("age")?.setValue(null);
          // this.reactiveForm5.get("mobile_no")?.setValue(null);
          // this.reactiveForm5.get("relt_employee")?.setValue(null);
          // this.reactiveForm5.get("resign_employee")?.setValue(null);
          // this.reactiveForm5.get("residing_town_state")?.setValue(null);
          // this.reactiveForm5.get("residing_addr")?.setValue(null);
          // this.reactiveForm5.get("nominee_for")?.setValue(null);
          this.getnominationsummary()
          this.ToastrService.success('Nomination Added Successfully')
        }
      });
     
    }
    setTimeout(function () {
      window.location.reload();
    }, 2000);
   
  }

  openModaldeletenomination(parameter: string){
    this.parameterValue6 = parameter
  }
  ondeletenomination(){
    console.log(this.parameterValue6);
    var url = 'HrmTrnProfileManagement/DeleteNomination'
    this.service.getid(url, this.parameterValue6).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      

    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  }

  // Statutory
  get provident_no() {
    return this.reactiveForm1.get('provident_no')!;
  }
  get date_ofjoinPF() {
    return this.reactiveForm1.get('date_ofjoinPF')!;
  }
  get employee_no() {
    return this.reactiveForm1.get('employee_no')!;
  }

  public statutorysubmit(): void {
    
    if (this.reactiveForm1.value.provident_no != null && this.reactiveForm1.value.provident_no != '') {
      this.reactiveForm1.value;
      var url10 = 'HrmTrnProfileManagement/StatutorySubmit'
      this.service.post(url10, this.reactiveForm1.value).subscribe((result: any) => {
        if (result.status == false) {
          this.getstaturarySummary()
          this.ToastrService.warning('Error Occured while Adding Statutory')
        }
        else {
          this.getstaturarySummary()
          this.ToastrService.success('Statutory Added Successfully')         
        }
      });
      
    }  
    setTimeout(function () {
      window.location.reload();
    }, 2000); 
  }

  openModaldeletestatutory(parameter: string){
    this.parameterValue5 = parameter
  }

  ondeletestatutory(){
    console.log(this.parameterValue5);
    var url = 'HrmTrnProfileManagement/DeleteStatutory'
    this.service.getid(url, this.parameterValue5).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      

    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  }
  


  // Emergency Contact
  get contact_person() {
    return this.reactiveForm3.get('contact_person')!;
  }
  get cont_addr() {
    return this.reactiveForm3.get('cont_addr')!;
  }
  get cont_no() {
    return this.reactiveForm3.get('cont_no')!;
  }
  get cont_emailid() {
    return this.reactiveForm3.get('cont_emailid')!;
  }
  get remarks() {
    return this.reactiveForm3.get('remarks')!;
  }

  public emergencysubmit(): void {
   
    if (this.reactiveForm3.value.contact_person != null && this.reactiveForm3.value.contact_person != '') {
      for (const control of Object.keys(this.reactiveForm3.controls)) {
        this.reactiveForm3.controls[control].markAsTouched();
      }
      this.reactiveForm3.value;
      var url12 = 'HrmTrnProfileManagement/EmergencySubmit'
      this.service.post(url12, this.reactiveForm3.value).subscribe((result: any) => {
        
        if (result.status == false) {
          this.getemergencysummary()
        this.ToastrService.warning('Error Occurred While Adding Emergency Contact')
        }
        else {
          // this.reactiveForm3.get("contact_person")?.setValue(null);
          // this.reactiveForm3.get("cont_addr")?.setValue(null);
          // this.reactiveForm3.get("cont_no")?.setValue(null);
          // this.reactiveForm3.get("cont_emailid")?.setValue(null);
          // this.reactiveForm3.get("remarks")?.setValue(null);
          this.getemergencysummary()
          this.ToastrService.success('Emergency Contact Added Successfully')
        }
      });
      
    }
    setTimeout(function () {
      window.location.reload();
    }, 2000);
   
  }
  openModaldeleteemergency(parameter: string){
    this.parameterValue4 = parameter
  }

  ondeleteemergency(){
    console.log(this.parameterValue4);
    var url = 'HrmTrnProfileManagement/DeleteEmergency'
    this.service.getid(url, this.parameterValue4).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      

    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  }

  // Dependent
  get name_user() {
    return this.reactiveForm2.get('name_user')!;
  }
  get relationship() {
    return this.reactiveForm2.get('relationship')!;
  }
  get date_ofbirth() {
    return this.reactiveForm2.get('date_ofbirth')!;
  }

  public dependentsubmit(): void {
    if (this.reactiveForm2.value.name_user != null && this.reactiveForm2.value.name_user != '') {
      for (const control of Object.keys(this.reactiveForm2.controls)) {
        this.reactiveForm2.controls[control].markAsTouched();
      }
      this.reactiveForm2.value;
      var url11 = 'HrmTrnProfileManagement/DependentSubmit'
      this.service.post(url11, this.reactiveForm2.value).subscribe((result: any) => {
        if (result.status == false) {
          this.getdependentSummary()
          this.ToastrService.warning('Error Occured While Adding Dependent')
        }
        else {
          // this.reactiveForm2.get("name_user")?.setValue(null);
          // this.reactiveForm2.get("relationship")?.setValue(null);
          // this.reactiveForm2.get("date_ofbirth")?.setValue(null);
          this.getdependentSummary()
          this.ToastrService.success('Dependent Added Successfully')
        }
      });
      
    }
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  
  }

  openModaldeletedependent(parameter: string){
    this.parameterValue = parameter
  }

  ondeletedependent() {
    console.log(this.parameterValue);
    var url = 'HrmTrnProfileManagement/DeleteDependent'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      

    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  }

  // Education
  get inst_name() {
    return this.reactiveForm4.get('inst_name')!;
  }
  get deg_dip() {
    return this.reactiveForm4.get('deg_dip')!;
  }
  get field_ofstudy() {
    return this.reactiveForm4.get('field_ofstudy')!;
  }
  get date_ofcompletion() {
    return this.reactiveForm4.get('date_ofcompletion')!;
  }
  get addn_notes() {
    return this.reactiveForm4.get('addn_notes')!;
  }

  public educationsubmit(): void {
    if (this.reactiveForm4.value.inst_name != null && this.reactiveForm4.value.inst_name != '') {
      for (const control of Object.keys(this.reactiveForm4.controls)) {
        this.reactiveForm4.controls[control].markAsTouched();
      }
      this.reactiveForm4.value;
      var url13 = 'HrmTrnProfileManagement/EducationSubmit'
      this.service.post(url13, this.reactiveForm4.value).subscribe((result: any) => {
        if (result.status == false) {
          this.getEducationSummary()
          this.ToastrService.warning('Error Occured While Adding Education')
        }
        else {
          // this.reactiveForm4.get("inst_name")?.setValue(null);
          // this.reactiveForm4.get("deg_dip")?.setValue(null);
          // this.reactiveForm4.get("field_ofstudy")?.setValue(null);
          // this.reactiveForm4.get("date_ofcompletion")?.setValue(null);
          // this.reactiveForm4.get("addn_notes")?.setValue(null);
          this.getEducationSummary()
          this.ToastrService.success('Education Added Successfully')
        }
      });
     
    }
    setTimeout(function () {
      window.location.reload();
    }, 2000);
   
  }

  openModaldeleteeducation(parameter: string){
    this.parameterValue3 = parameter
  }

  ondeleteeducation() {
    
    console.log(this.parameterValue3);
    var url = 'HrmTrnProfileManagement/DeleteEducation'
    this.service.getid(url, this.parameterValue3).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      

    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  }
}

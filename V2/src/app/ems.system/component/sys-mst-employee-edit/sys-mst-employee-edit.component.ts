import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';

import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {
  confirmpassword: string | Blob;
  password: string | Blob;
  employee_gid: string;
  temporary_addressgid: string;
  permanent_addressgid: string;
  temporary_address1: string;
  temporary_address2: string;
  email: string;
  comp_email:string;
  permanent_address1: string;
  active_flag: string;
  gender: string;
  temporary_postal: string;
  // password: string;
  // confirmpassword:string;
  user_password: string;
  branchname: string;
  // entityname: string;
  country: string;
  countryname: string;
  // reportingto:string;
  // usergrouptemplate:string;
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
  attach_file_name:string;
  usergrouptemplate:string;
  usergrouptemplate_gid : string
}



@Component({
  selector: 'app-sys-mst-employee-edit',
  templateUrl: './sys-mst-employee-edit.component.html',
  styleUrls: ['./sys-mst-employee-edit.component.scss']
})

export class SysMstEmployeeEditComponent implements OnInit {
  fileName: string | null = null;

  safePdfUrl: string ='';
  attach_file_name : string ='';
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
  // usergrouptemp_list: any[] = [];
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
  ReportingTo: any;
  departmentList: any[] = []
  usergrouptemp_list: any[] = [];
  employeegid: any;
  employeeedit_list: any;
  invalidFileFormat:boolean= false;
  
  @ViewChild('fileInput') fileInput!: ElementRef;
  employee_gid: any;

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, public NgxSpinnerService: NgxSpinnerService,private router: ActivatedRoute) {
    this.employee = {} as IEmployee;
  }


  ngOnInit(): void {


    

    
    const employee_gid = this.router.snapshot.paramMap.get('employee_gid');
    // console.log(termsconditions_gid)
    this.employeegid = employee_gid;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.employeegid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetEditEmployeeSummary(deencryptedParam)

    this.reactiveForm = new FormGroup({

      first_name: new FormControl(this.employee.first_name, [
        Validators.required,Validators.pattern(/^\S.*$/)
      ]),
      user_code: new FormControl(this.employee.user_code, [
        Validators.required,Validators.pattern(/^\S.*$/)
      ]),
      file: new FormControl(''),
      last_name: new FormControl(''),
      gender: new FormControl(''),
      active_flag: new FormControl('Y'),
      //confirmpassword: ['', Validators.required],
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

      mobile: new FormControl(this.employee.mobile, [Validators.required,Validators.minLength(10), Validators.maxLength(12)]),
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
      temporary_postal: new FormControl('', [Validators.minLength(6)]),
      temporary_address2: new FormControl(''),
      temporary_address1: new FormControl(''),
      temporary_city: new FormControl(''),
      temporary_state: new FormControl(''),
      temporary_addressgid: new FormControl(''),
      permanent_state: new FormControl(''),
      permanent_city: new FormControl(''),
      permanent_address2: new FormControl(''),
      // reportingto: new FormControl(this.employee.reportingto),
      // usergrouptemplate: new FormControl(this.employee.usergrouptemplate),
      permanent_postal: new FormControl('', [Validators.minLength(6)]),
      usergrouptemplate: new FormControl(this.employee.usergrouptemplate),
      usergrouptemplate_gid : new FormControl(this.employee.usergrouptemplate_gid),
      permanent_addressgid: new FormControl(''),
      permanent_address1: new FormControl(this.employee.permanent_address1, [
        Validators.maxLength(1000),
      ]),
      // FileName:new FormControl(''),
      // email: new FormControl(this.employee.email, [
      //   Validators.required,
      //   Validators.minLength(1),
      //   Validators.maxLength(250), Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')
      //   // emailValidator(),
      // ]),
      email: new FormControl(this.employee.email, [
        Validators.minLength(1),
        Validators.maxLength(250), Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')
      ]),

      FileName1:new FormControl(''),
      comp_email: new FormControl(this.employee.comp_email, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250), Validators.pattern(/^[a-z0-9._%+-]+@(?!gmail\.com$)(?!yahoo\.com$)(?!hotmail\.com$)(?!outlook\.com$)(?!live\.com$)[a-z0-9.-]+\.[a-z]{2,100}$/)
        // emailValidator(),
      ]),

    }

    );

    var url = 'Employeelist/Getbranchdropdown';
    this.service.get(url).subscribe((result: any) => {
      this.branchList = result.Getbranchdropdown;
    });
    var api1 = 'Employeelist/Getentitydropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.entity_list = result.Getentitydropdown;
      //console.log(this.entity_list)
    });

    var api3 = 'Employeelist/Getdepartmentdropdown'
    this.service.get(api3).subscribe((result: any) => {
      this.departmentList = result.Getdepartmentdropdown;
      //console.log(this.department_list)
    });
    var api4 = 'Employeelist/Getdesignationdropdown'
    this.service.get(api4).subscribe((result: any) => {
      this.designation_list = result.Getdesignationdropdown;
      //console.log(this.designation_list)
    });
    var api5 = 'Employeelist/Getcountrydropdown'
    this.service.get(api5).subscribe((result: any) => {
      this.country_list = result.Getcountrydropdown;
      //console.log(this.country_list)
    });
    var api6 = 'Employeelist/Getcountry2dropdown'
    this.service.get(api6).subscribe((result: any) => {
      this.country_list2 = result.Getcountry2dropdown;
      //console.log(this.branch_list)
    });
    // var api7 = 'Employeelist/Getreportingtodropdown';
    // this.service.get(api7).subscribe((result: any) => {
    //   this.employeereportingto_list = result.Getreportingtodropdown;
    // });
    // var api8 = 'Employeelist/Getusergrouptempdropdown';
    // this.service.get(url).subscribe((result: any) => {
    //   this.usergrouptemp_list = result.Getusergroupdroptempdown;
    // });
    var api7 = 'Employeelist/Getusergrouptempdropdown';
    this.service.get(api7).subscribe((result: any) => {
      this.usergrouptemp_list = result.Getusergrouptempdropdown;
    });
  }
  GetEditEmployeeSummary(employee_gid: any) {
    debugger
    var url = 'Employeelist/GetEditEmployeeSummary'
    let param = {
      employee_gid : employee_gid 
    }
    
    this.service.getparams(url, param).subscribe((result: any) => {
      // this.responsedata=result;
      this.employeeedit_list = result.GetEditEmployeeSummary;
      console.log(this.employeeedit_list)
      console.log(this.employeeedit_list[0].branch_gid)
      this.selectedBranch = this.employeeedit_list[0].branch_gid;
      this.reactiveForm.get("mobile")?.setValue(this.employeeedit_list[0].employee_mobileno);
      this.reactiveForm.get("departmentname")?.setValue(this.employeeedit_list[0].department_name);
      // this.reactiveForm.get("entityname")?.setValue(this.employeeedit_list[0].entity_name);
      this.selectedEntity = this.employeeedit_list[0].entity_gid;
      this.selectedDepartment = this.employeeedit_list[0].department_gid;
      this.selectedDesignation = this.employeeedit_list[0].designation_gid;
      this.selectedCountry1 = this.employeeedit_list[0].permanent_countrygid;
      this.selectedCountry2 = this.employeeedit_list[0].temporary_countrygid;
     //this.reactiveForm.get("active_flag")?.setValue(this.employeeedit_list[0].user_status);
      this.reactiveForm.get("user_code")?.setValue(this.employeeedit_list[0].user_code);
      this.reactiveForm.get("password")?.setValue(this.employeeedit_list[0].user_password);
      this.reactiveForm.get("confirmpassword")?.setValue(this.employeeedit_list[0].user_password);
      this.reactiveForm.get("first_name")?.setValue(this.employeeedit_list[0].user_firstname);
      this.reactiveForm.get("last_name")?.setValue(this.employeeedit_list[0].user_lastname);
      this.reactiveForm.get("permanent_address1")?.setValue(this.employeeedit_list[0].permanent_address1);
      this.reactiveForm.get("permanent_address2")?.setValue(this.employeeedit_list[0].permanent_address2);
      this.reactiveForm.get("permanent_city")?.setValue(this.employeeedit_list[0].permanent_city);
      this.reactiveForm.get("permanent_state")?.setValue(this.employeeedit_list[0].permanent_state);
      this.reactiveForm.get("email")?.setValue(this.employeeedit_list[0].employee_emailid);
      this.reactiveForm.get("comp_email")?.setValue(this.employeeedit_list[0].comp_email);
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
      this.reactiveForm.get("usergrouptemplate")?.setValue(this.employeeedit_list[0].usergrouptemplate_name);
      this.reactiveForm.get("usergrouptemplate_gid")?.setValue(this.employeeedit_list[0].usergrouptemplate_gid)
       this.fileName = (this.employeeedit_list[0].file_name
       );
    });
  }
  onChange2(event: any) {
    this.file = event.target.files[0];
    // var api='Employeelist/EmployeeProfileUpload'
    // //console.log(this.file)
    //   this.service.EmployeeProfileUpload(api,this.file).subscribe((result:any) => {
    //     this.responsedata=result;
    //   });
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
  get usergrouptemplate() {
    return this.reactiveForm.get('usergrouptemplate')!;
  }
  get email() {
    return this.reactiveForm.get('email')!;
  }
  get comp_email() {
    return this.reactiveForm.get('comp_email')!;
  }
  get entityname() {
    return this.reactiveForm.get('entityname')!;
  }
  // userpassword(password:any) {
  //   this.reactiveForm.get("confirmpassword")?.setValue(password.value);
  // }

  copyAddress1(){
    this.reactiveForm.patchValue({
      temporary_address1: this.reactiveForm.get('permanent_address1').value,
      temporary_address2: this.reactiveForm.get('permanent_address2').value,
      temporary_city: this.reactiveForm.get('permanent_city').value,
      temporary_postal: this.reactiveForm.get('permanent_postal').value,
      temporary_state: this.reactiveForm.get('permanent_state').value,  
      countryname: this.reactiveForm.get('country').value
    });
  }

  public validate(): void {
    debugger;
    console.log(this.reactiveForm.value)

    this.employee = this.reactiveForm.value;
    // this.service.Profileupload(this.reactiveForm.value).subscribe(result => {  
    //   this.responsedata=result;
    // });   
    if ( this.employee.branchname != null && this.employee.departmentname != null && this.employee.designationname != null && this.employee.first_name != null && this.employee.active_flag != null && this.employee.email != null && this.employee.mobile != null && this.employee.comp_email != null) {
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {

        this.employee_gid = this.employeeedit_list[0].employee_gid
        formData.append("file", this.file, this.file.name);
        // formData.append("entityname", this.employee.entityname);//0
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
        formData.append("permanent_addressgid", this.employee.permanent_addressgid);
        formData.append("temporary_addressgid", this.employee.temporary_addressgid);
        formData.append("employee_gid", this.employee_gid);
        formData.append("usergrouptemplate", this.employee.usergrouptemplate);
        formData.append("comp_email", this.employee.comp_email);
        formData.append("usergrouptemplate_gid",this.employee.usergrouptemplate_gid)

        // formData.append("reportingto", this.employee.reportingto);
        // formData.append("usergrouptemplate", this.employee.usergrouptemplate);
        var api = 'Employeelist/UpdateEmployeeProfileUpload'
        this.NgxSpinnerService.show();
        //console.log(this.file)
        this.service.post(api, formData).subscribe((result: any) => { 
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide();
          }
          else {
            this.route.navigate(['/system/SysMstEmployeeSummary']);
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide();
          }
        });

      }
      else {
        var api7 = 'Employeelist/UpdateEmployeedetails'
        this.NgxSpinnerService.show();
        //console.log(this.file)
        this.service.post(api7, this.employee).subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning("Error while Updating Employee Detatls")
            this.NgxSpinnerService.hide();
          }
          else {
            this.route.navigate(['/system/SysMstEmployeeSummary']);
            this.ToastrService.success("Employee Updated Successfully")
            this.NgxSpinnerService.hide();
          }
          this.responsedata = result;
        });
      }
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

    // console.info('Name:', this.employee);
    return;



  }

  onFileChange(event: any): void {
    const input = event.target as HTMLInputElement;
    this.file = event.target.files[0];
    const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
   
    if (this.file && validImageTypes.includes(this.file.type)) {
      this.invalidFileFormat = false;
      this.reactiveForm.get('fileName')?.setValue(this.file);
    } else {
      this.invalidFileFormat = true;
      this.reactiveForm.get('fileName')?.reset();
      event.target.value = ''; // Clear the file input field
    }
    if (input.files && input.files.length > 0) {
      this.fileName = input.files[0].name;
      // Optionally, you can set custom label text here
      const label = this.fileInput.nativeElement.previousElementSibling as HTMLLabelElement;
      if (label) {
        label.textContent = this.fileName;
      }
    } else {
      this.fileName = null;
      // Reset custom label text if no file is chosen
      const label = this.fileInput.nativeElement.previousElementSibling as HTMLLabelElement;
      if (label) {
        label.textContent = 'Choose a file';
      }
    }
  }
}


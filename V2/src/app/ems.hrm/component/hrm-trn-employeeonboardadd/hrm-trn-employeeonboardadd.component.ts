import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import {SelectionModel} from '@angular/cdk/collections'; 
interface objInterface { task_gid: string; task_name: string; team_gid: string; team_name: string; 
  hrdocument_gid: string; hrdocument_name: string;  }

@Component({
  selector: 'app-hrm-trn-employeeonboardadd',
  templateUrl: './hrm-trn-employeeonboardadd.component.html',
  styleUrls: ['./hrm-trn-employeeonboardadd.component.scss']
})
export class HrmTrnEmployeeonboardaddComponent {
  confirmPasswordTouched = false;
  EmpAddForm : FormGroup | any;
  TaskInitiationForm : FormGroup | any;
  HRDocumentForm : FormGroup | any; 
cboentity : any;
cbobranch : any;
cbofunction: any;
cbosubfunction: any;
cbodesignation: any;
cborole: any;
txtuser_password: any;
txtconfrim_user_password: any;
rdbemployee_access: string = 'Y';
txtemployee_user_code: any;
txtemployee_joining_date: any;
cboreporting: any;
cbobaselocation: any;
txtofficialemail_address: string = '';
txtofficialmobile_number: any;
txtfirst_name: any;
txtlast_name: string = '';
rdbgender: string = 'Male';
cboblood_group: any;
cbomartial_status: any;
txtpersonal_email_address: string = '';
txtpersonal_phone_number: string = '';

txtpermanent_address_line_one : string = '';
txtpermanent_address_line_two: string = '';
cbopermanent_country: any;
txtpermanent_state: string = '';
txtpermanent_city: string = '';
txtpermanent_postal_code: string = '';
addresult: any;
txttemporary_address_line_one: string = '';
txttemporary_address_line_two: string = '';
cbotemporary_country: any;
txttemporary_state: string = '';
txttemporary_city: string = '';
txttemporary_postal_code: string = '';
txtTaskRemarks: any;
task_list: any;
hrdocument_list : any;
OnboardingTaskList: any[] = [];
hrdocumentData_list: any[] = [];
file_name : any;
 AutoIDkey: any;
TaskInitiationFormdata = {
  cboTask: '',
  cboteam: '',
  txtTaskRemarks: '',
};
HRDocFormdata = {
  cboHRDocument: null 
};
team_list : any; 
formDataObject: FormData = new FormData();

  MartialStatus = [
    { MartialStatus: 'Married', MartialStatus_gid: 'Msts_001'},
    { MartialStatus: 'UnMarried',MartialStatus_gid: 'Msts_002' },
    { MartialStatus: 'Single',MartialStatus_gid: 'Msts_003'},
  ];

  entityList: any;
  branchList: any;
  departmentList: any;
  designationList: any;
  countryList: any;
  subfunction_list: any;
  location_list: any;
  roleList: any;
  reportingtoList: any;
  bloodgroup_list: any;
  maritalstatus_data: any;
  selection = new SelectionModel<objInterface>(true, []);
  constructor(private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService,private SocketService: SocketService,public router:Router,private FormBuilder: FormBuilder) {
    this.Sample();
    this.ValidateTaskinitiation();
    this.ValidateHRDocument(); 
  }
//^\S+$
  Sample(){
    this.EmpAddForm = new FormGroup ({
      txtemployee_user_code : new FormControl(null,
      [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
      ]),
      txtuser_password : new FormControl(null,
        [
          Validators.required,
          Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)
        ]),
      txtconfrim_user_password: new FormControl(null,Validators.required),
      txtofficialmobile_number: new FormControl(null,
        [
          Validators.required,
          Validators.pattern(/^[0-9]+$/),
          Validators.minLength(10),   
        ]),
      txtpersonal_phone_number: new FormControl(null,
          [ 
            Validators.pattern(/^[0-9]+$/),
            Validators.minLength(10),
          ]),
      txtofficialemail_address: new FormControl(null,[
        Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')
        ]),
        txtpersonal_email_address: new FormControl(null,[
        Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')
        ]),
      txtfirst_name: new FormControl(null,[
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
      ]),
      txtemployee_joining_date: new FormControl(null,Validators.required),
      cbobranch : new FormControl(null,Validators.required),
      cbofunction: new FormControl(null,Validators.required),
      cbosubfunction: new FormControl(null,Validators.required),
      cbodesignation: new FormControl(null,Validators.required),
      cborole: new FormControl(null,Validators.required),
      cboreporting: new FormControl(''),
      cbobaselocation: new FormControl(null,Validators.required),
      txtpermanent_postal_code: new FormControl(null,
        [ 
          Validators.pattern(/^[0-9]+$/),
          Validators.minLength(6), 
        ]),
        txttemporary_postal_code: new FormControl(null,
          [ 
            Validators.pattern(/^[0-9]+$/),
            Validators.minLength(6),
          ]),
          txttemporary_address_line_one : new FormControl(null),
          txttemporary_address_line_two: new FormControl(null),
          cbotemporary_country: new FormControl(null),
          txttemporary_state: new FormControl(null),
          txttemporary_city: new FormControl(null),
          txtpermanent_address_line_one: new FormControl(null),
          txtpermanent_address_line_two: new FormControl(null),
          cbopermanent_country: new FormControl(null),
          txtpermanent_state: new FormControl(null),
          txtpermanent_city: new FormControl(null),
          txtlast_name: new FormControl(null),
          cboentity: new FormControl(null),
          rdbemployee_access:new FormControl(null),
          rdbgender:new FormControl(null),
          cbomartial_status:new FormControl(null),
          cboblood_group:new FormControl(null),
    })
  }

  ValidateTaskinitiation(){ 
    this.TaskInitiationForm = this.FormBuilder.group({
      cboTask: [''],
      cboteam: [''],
      txtTaskRemarks: ['',Validators.required],
    });
  }

  ValidateHRDocument(){ 
    this.HRDocumentForm = this.FormBuilder.group({
      cboHRDocument: ['', Validators.required],
      fileInput: ['', Validators.required], 
    });
  }
 
  
  ngOnInit(): void {
    
    const options: Options = {
      dateFormat: 'd-m-Y', 
      
    };

    flatpickr('.date-picker', options);  
//Entity DropDown Values
    var url = 'EmployeeOnboard/PopEntity';
    this.SocketService.get(url).subscribe((result: any) => {
      this.entityList  = result.entity;
    });

    var url = 'EmployeeOnboard/PopBranch';
    this.SocketService.get(url).subscribe((result: any) => {
      this.branchList  = result.employee;
    });

    var url = 'EmployeeOnboard/PopDepartment';
    this.SocketService.get(url).subscribe((result: any) => {
      this.departmentList  = result.employee;
    });

    var url = 'EmployeeOnboard/PopDesignation';
    this.SocketService.get(url).subscribe((result: any) => {
      this.designationList  = result.employee;
    });

    var url = 'EmployeeOnboard/PopRole';
    this.SocketService.get(url).subscribe((result: any) => {
      this.roleList  = result.rolemaster;
    });
  
    var url = 'EmployeeOnboard/PopCountry';
    this.SocketService.get(url).subscribe((result: any) => {
      this.countryList  = result.country;
    });

    var url = 'EmployeeOnboard/PopReportingTo';
    this.SocketService.get(url).subscribe((result: any) => {
      this.reportingtoList  = result.reportingto;
    });

    var url = 'EmployeeOnboard/PopSubfunction';
    this.SocketService.get(url).subscribe((result: any) => {
      this.subfunction_list  = result.employee;
    });

    var url = 'HrmMaster/GetBaseLocationlist';
    this.SocketService.get(url).subscribe((result: any) => {
      this.location_list  = result.location_list;
    });

    var url = 'HrmMaster/GetBloodGroup';
    this.SocketService.get(url).subscribe((result: any) => {
    this.bloodgroup_list  = result.master_list;
    });

   // Get Task and Team Master Data
    var url = 'HrmMaster/GetTaskSummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.task_list  = result.master_list;  
    });

    var url = 'EmployeeOnboard/GetTeamList';
    this.SocketService.get(url).subscribe((result: any) => {
      this.team_list  = result.teamlist;
    });

    // Get HR Document Master Data
    var url = 'HRDocument/GetSysHRDocumentDropDown';
    this.SocketService.get(url).subscribe((result: any) => {
      this.hrdocument_list  = result.hrdocument_list;
    });

    // var url = 'MstApplication360/GetMaritalStatus';
    // this.SocketService.get(url).subscribe((result: any) => {
    //   this.maritalstatus_data  = result.application_list;
    // });
  }

    showPassword: boolean = false;
    showConfrimPassword: boolean = false;
  
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfrimPasswordVisibility(): void {
    this.showConfrimPassword = !this.showConfrimPassword;
  }

  backbutton(){
    this.router.navigate(['/hrm/HrmtrnEmployeeonboard']);
    
  }
  

 isAllSelected() { 
    const numSelected = this.selection.selected.length; 
    const numRows = this.task_list.length; 
    return numSelected === numRows; 
  }
  masterToggle() { 
    
    this.isAllSelected() ? this.selection.clear() : 
      this.task_list.forEach((row: objInterface) => this.selection.select(row)); 
  }

 

  onadd(){
    this.selection.selected.forEach((a: any) => {
          this.OnboardingTaskList.push({ 
            task_gid: a.task_gid,
            task_name: a.task_name,
            team_name: a.team_name,
            team_gid:  a.team_gid, 
            task_remarks: this.TaskInitiationFormdata.txtTaskRemarks
           } 
          ); 
        });
    try { 
      if (this.txtemployee_joining_date.split("-")) 
      this.txtemployee_joining_date = this.txtemployee_joining_date.split("-").reverse().join("-"); 
    } 
    catch (e) {  
      this.txtemployee_joining_date = this.txtemployee_joining_date 
    }
    var params = {
      marital_status : (this.cbomartial_status == undefined) ? "" : this.cbomartial_status.MartialStatus,
      bloodgroup_name :(this.cboblood_group == undefined) ? "" : this.cboblood_group.bloodgroup_name,
      bloodgroup_gid : (this.cboblood_group == undefined) ? "" :this.cboblood_group.bloodgroup_gid,
      marital_status_gid :(this.cbomartial_status == undefined) ? "" : this.cbomartial_status.MartialStatus_gid,
      useraccess: this.rdbemployee_access,
      gender: this.rdbgender,
      company_name: (this.cboentity == undefined) ? "" : this.cboentity.entity_name,
      entity_gid: (this.cboentity == undefined) ? ""  : this.cboentity.entity_gid,
      user_lastname: this.txtlast_name,
      branch_gid:(this.cbobranch == null ? "" : this.cbobranch),
      department_gid:(this.cbofunction == null ? "" : this.cbofunction),
      designation_gid:(this.cbodesignation == null ? "" : this.cbodesignation),
      user_code:this.txtemployee_user_code,
      user_password: this.txtuser_password,
      role_gid:(this.cborole == null ? "" : this.cborole) ,
      employee_reportingto:(this.cboreporting == null ? "" : this.cboreporting) ,
      user_firstname: this.txtfirst_name,
      employee_emailid: this.txtofficialemail_address,





      employee_mobileno: this.txtofficialmobile_number,   
      per_address1: this.txtpermanent_address_line_one,
      per_address2: this.txtpermanent_address_line_two,
      per_country_gid:(this.cbopermanent_country == null ? "" : this.cbopermanent_country),
      per_state: this.txtpermanent_state,
      per_city: this.txtpermanent_city,
      per_postal_code: this.txtpermanent_postal_code,
      temp_address1: this.txttemporary_address_line_one,
      temp_address2: this.txttemporary_address_line_two,
      temp_country_gid:(this.cbotemporary_country == null ? "" : this.cbotemporary_country),
      temp_state: this.txttemporary_state,
      temp_city: this.txttemporary_city,
      temp_postal_code: this.txttemporary_postal_code,
      baselocation_gid: this.cbobaselocation,                 
      joiningdate : this.txtemployee_joining_date,
      personal_phone_no : this.txtpersonal_phone_number,
      personal_emailid: this.txtpersonal_email_address,
      subfunction_gid: this.cbosubfunction, 
      MdlEmployeetasklist: this.OnboardingTaskList
    }  
this.NgxSpinnerService.show();
var url = 'EmployeeOnboard/EmployeeAdd';

this.SocketService.post(url, params).subscribe((result: any) => {
if (result.status == true)
{
   if(this.hrdocumentData_list != null){
    const jsonData = "" + JSON.stringify(this.hrdocumentData_list)+ "";  
    this.formDataObject.append('employee_gid', result.employee_gid);  
    this.formDataObject.append('project_flag', "Default");   
    this.formDataObject.append('hrdocumentList', jsonData);   
    this.addresult = result.status;
    var api='EmployeeOnboard/HRDocumentUpload'
       
    this.SocketService.postfile(api,this.formDataObject).subscribe((result:any) => { 
            if(result.status==true && this.addresult == true){
              this.ToastrService.success("Employee details added successfully");  
            }
            else if(result.status==false && this.addresult == true){
              this.ToastrService.success("Employee details added successfully ")  
            }
            else{
              this.ToastrService.warning(result.message) 
            }
   });
  
  this.NgxSpinnerService.hide();
  this.router.navigate(['/hrm/HrmtrnEmployeeonboard']);
  }

}
else
{
this.ToastrService.warning(result.message)
this.NgxSpinnerService.hide();
}
}
)  


  }  

 

  DeleteTaskClick(index : any){
    if (index >= 0 && index < this.OnboardingTaskList.length) {
      this.OnboardingTaskList.splice(index, 1);
    }  
  }
  DeleteDocumentClick(index : any){
    if (index >= 0 && index < this.hrdocumentData_list.length) {
      this.hrdocumentData_list.splice(index, 1);
    }  
  }

  HrDocumentClick(){
    this.AutoIDkey = this.generateKey(); 
    const fileInput: HTMLInputElement = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      const files: FileList | null = fileInput.files;
      if (files !=null && files.length !=0) { 
        for (let i = 0; i < files.length; i++) {  
          this.formDataObject.append(this.AutoIDkey, files[i]);  
          this.file_name = files[i].name;
        }

        const DocumentInfo = this.hrdocument_list.find((item: objInterface) => item.hrdocument_gid === this.HRDocFormdata.cboHRDocument);
        this.formDataObject.append('hrdocument_gid', this.HRDocFormdata.cboHRDocument==null ? "": this.HRDocFormdata.cboHRDocument); 
        this.formDataObject.append('hrdocument_name', DocumentInfo);  
        this.hrdocumentData_list.push({
          AutoID_Key: this.AutoIDkey,
          hrdocument_name: DocumentInfo.hrdocument_name,
          hrdocument_gid:this.HRDocFormdata.cboHRDocument, 
          file_name: this.file_name
         } 
        );  
        fileInput.value = '';
        this.HRDocumentForm.reset();
        // this.clearFileInput()
      } else {
        this.ToastrService.warning("Kindly Upload the Document") 
      } 
    } 
   
    
  }
   generateKey(): string { 

    return `AutoIDKey${new Date().getTime()}`;
  } 

  btncopy(){
    this.txttemporary_address_line_one = this.txtpermanent_address_line_one;
    this.txttemporary_address_line_two = this.txtpermanent_address_line_two;
    this.cbotemporary_country = this.cbopermanent_country;
    this.txttemporary_state = this.txtpermanent_state;
    this.txttemporary_city = this.txtpermanent_city;
    this.txttemporary_postal_code = this.txtpermanent_postal_code;
 
  }

  passwordsMatch(): boolean {
    const password = this.EmpAddForm.get('txtuser_password').value;
    const confirmPassword = this.EmpAddForm.get('txtconfrim_user_password').value;
    return password === confirmPassword;
  } 
   
}




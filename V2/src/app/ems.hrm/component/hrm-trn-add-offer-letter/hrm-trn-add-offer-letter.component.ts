import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';

interface Iofferletter {
  Offerlettertemplate_content: String;
}

@Component({
  selector: 'app-hrm-trn-add-offer-letter',
  templateUrl: './hrm-trn-add-offer-letter.component.html',
  styleUrls: ['./hrm-trn-add-offer-letter.component.scss']
})

export class HrmTrnAddOfferLetterComponent {
  appointmentorder!: Iofferletter;
  Offerletterform!: FormGroup;
  Existingform!: FormGroup;
  responsedata: any;
  selectedbranch: any;
  branchList: any;
  cbobranch: any;
  department_list: any;
  designationList: any;
  selecteddepartment: any;
  selecteddesignation: any;
  selectedCountry1: any;
  cbodesignation: any;
  cbodepartment: any;
  selectedCountry2: any;
  country_list1: any;
  country_list: any;
  selectedcountry: any;
  Selectedcountry: any;
  appointmentordergid: any;
  editappoinmentorder: any;
  email_address: any;
  mdlTerms: any;
  terms_list: any[] = [];
  templatecontent_list: any;
  txtemployee_joining_date: any;
  Offer_date: any;
  Qualification: any;
  permanent_country: any;
  temp_country: any;
  department_name: any;
  mobile_number: any;
  employeeDropdown:any;
  audioOption1:any;
  audioOption2:any;
  editContent: string = "";
  showDropdown = false;
  
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '12rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  selectedOption: string = 'newCandidate';
  cboselectedEmployee: any;
  employeelist: any[] = [];
  editexistemployee:any[] =[];




  constructor(private renderer: Renderer2, private el: ElementRef, public NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    this.Offerletterform = new FormGroup({
      branch_name: new FormControl(''),
      appointmentorder_gid: new FormControl(''),
      appointment_date: new FormControl(''),
      first_name: new FormControl('',[ Validators.required,Validators.pattern(/^\S.*$/)]),
      last_name: new FormControl(''),
      gender: new FormControl(''),
      Experience: new FormControl(''),
      dob: new FormControl('',[Validators.required]),
      mobile_number: new FormControl('', [Validators.required, Validators.pattern('[0-9]{10}$'), Validators.maxLength(10)]),
      email_address: new FormControl('',[Validators.required, Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')]),
      txtemployee_joining_date: new FormControl('', [Validators.required]),
      Qualification: new FormControl(''),
      cbodesignation: new FormControl(''),
      designation_name: new FormControl(''),
      Salary: new FormControl('', [Validators.required]),
      permanent_address1: new FormControl(''),
      permanent_address2: new FormControl(''),
      permanent_country: new FormControl(''),
      permanent_state: new FormControl(''),
      permanent_city: new FormControl(''),
      permanent_postal: new FormControl(''),
      temporary_address1: new FormControl(''),
      temporary_address2: new FormControl(''),
      temp_country: new FormControl(''),
      temporary_state: new FormControl(''),
      temporary_city: new FormControl(''),
      template_name: new FormControl('', [Validators.required]),
      Offerlettertemplate_content: new FormControl(''),
      cbobranch: new FormControl(''),
      offer_no: new FormControl(''),
      department_name: new FormControl(''),
      temporary_postal: new FormControl(''),
      template_gid: new FormControl(''),
      Offer_date: new FormControl(''),

    });
    this.Existingform = new FormGroup({
      employee_gid: new FormControl(''),
      first_name: new FormControl('', Validators.required),
      last_name: new FormControl(''),
      gender: new FormControl(''),
      Experience: new FormControl(''),
      dob: new FormControl('',[Validators.required]),
      mobile_number: new FormControl('', [Validators.required, Validators.pattern('[0-9]{10}$'), Validators.maxLength(10)]),
      email_address: new FormControl('',[Validators.required]),
      employee_joining_date: new FormControl('', [Validators.required]),
      Qualification: new FormControl(''),
      cbodesignation: new FormControl(''),
      designation_name: new FormControl(''),
      Salary: new FormControl('', [Validators.required]),
      permanent_address1: new FormControl(''),
      permanent_address2: new FormControl(''),
      permanent_country: new FormControl(''),
      permanent_state: new FormControl(''),
      permanent_city: new FormControl(''),
      permanent_postal: new FormControl(''),
      temporary_address1: new FormControl(''),
      temporary_address2: new FormControl(''),
      temp_country: new FormControl(''),
      temporary_state: new FormControl(''),
      temporary_city: new FormControl(''),
      template_name: new FormControl(''),
      Offerlettertemplate_content: new FormControl(''),
      cbobranch: new FormControl(''),
      offer_no: new FormControl(''),
      department_name: new FormControl(''),
      temporary_postal: new FormControl(''),
      template_gid: new FormControl(''),
      Offer_date: new FormControl(''),

    });

    var url = 'EmployeeOnboard/PopBranch';
    this.service.get(url).subscribe((result: any) => {
      this.branchList = result.employee;
    });

    var api2 = 'AppointmentOrder/Getdepartmentdropdown';
    this.service.get(api2).subscribe((result: any) => {
      this.department_list = result.Getdepartmentdropdown;
    });

    var url = 'EmployeeOnboard/PopDesignation';
    this.service.get(url).subscribe((result: any) => {
      this.designationList = result.employee;
    });

    var url = 'AppointmentOrder/TermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.GetAppointmentdropdown;
    });

    var api4 = 'AppointmentOrder/Getcountrydropdown';
    this.service.get(api4).subscribe((result: any) => {
      this.country_list = result.getcountrydropdown;
    });
      var url = 'OfferLetter/getEmployeelist';
  this.service.get(url).subscribe((result: any) => { 
    this.employeelist  = result.Getemployeebind;  
  });


  }
  onsubmit() {
    var params = {
      branch_gid: this.Offerletterform.value.cbobranch,
      offer_refno: this.Offerletterform.value.offer_no,
      offer_date: this.Offerletterform.value.Offer_date,
      first_name: this.Offerletterform.value.first_name,
      last_name: this.Offerletterform.value.last_name,
      gender: this.Offerletterform.value.gender,
      experience_detail: this.Offerletterform.value.Experience,
      dob: this.Offerletterform.value.dob,
      mobile_number: this.Offerletterform.value.mobile_number,
      email_address: this.Offerletterform.value.email_address,
      joiningdate: this.Offerletterform.value.txtemployee_joining_date,
      qualification: this.Offerletterform.value.Qualification,
      department_gid: this.Offerletterform.value.department_name,
      designation_gid: this.Offerletterform.value.cbodesignation,
      employee_salary: this.Offerletterform.value.Salary,
      perm_address1: this.Offerletterform.value.permanent_address1,
      perm_address2: this.Offerletterform.value.permanent_address2,
      perm_city: this.Offerletterform.value.permanent_city,
      perm_pincode: this.Offerletterform.value.permanent_postal,
      perm_state: this.Offerletterform.value.permanent_state,
      perm_country: this.Offerletterform.value.permanent_country,
      temp_address1: this.Offerletterform.value.temporary_address1,
      temp_address2: this.Offerletterform.value.temporary_address2,
      temp_city: this.Offerletterform.value.temporary_city,
      temp_pincode: this.Offerletterform.value.temporary_postal,
      temp_state: this.Offerletterform.value.temporary_state,
      temp_country: this.Offerletterform.value.temp_country,
      template_gid: this.Offerletterform.value.template_name,
      offertemplate_content: this.Offerletterform.value.Offerlettertemplate_content,

    }


    var url = 'OfferLetter/Addofferletter'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
        this.route.navigate(['/hrm/HrmTrnOfferLetter']);
      }

    });

  }


  GetOnChangeTerms() {

    let template_gid = this.Offerletterform.value.template_name;
  
    let param = {
      template_gid: template_gid
    }

    var url = 'AppointmentOrder/GetOnChangeTerms';

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.templatecontent_list = this.responsedata.GetAppointmentdropdown;
      this.Offerletterform.get("Offerlettertemplate_content")?.setValue(this.templatecontent_list[0].template_content);
      // this.Existingform.get("Offerlettertemplate_content")?.setValue(this.templatecontent_list[0].template_content);

      // Fetching values from form controls
      const userFirstName = this.Offerletterform.value.first_name + ' ' + this.Offerletterform.value.last_name;
      const usergender = this.Offerletterform.value.gender.toLowerCase(); 
      const employeedesignation = this.Offerletterform.value.cbodesignation;
      const joiningdate = this.Offerletterform.value.txtemployee_joining_date; // Assuming the property name is txtemployee_joining_date
      const employeesalary = this.Offerletterform.value.Salary; // Assuming the property name is txtemployee_joining_date
      const employeebrench = this.Offerletterform.value.cbobranch; 
      let prefix = '';
       if (this.Offerletterform.value.gender == 'male') {
            prefix = 'Mr.';
       } 
       else if (this.Offerletterform.value.gender  == 'female') {
          prefix = 'Ms.';
        }
        const userFullName = prefix +''+ userFirstName;

      // Replacing placeholders with actual values
      let editContent = this.templatecontent_list[0].template_content;
      editContent = editContent.replace("candidate_name", userFullName);
      editContent = editContent.replace("emp_designation", employeedesignation);
      editContent = editContent.replace("joining_date", joiningdate);
      editContent = editContent.replace("Salary", employeesalary);
      editContent = editContent.replace("brench_name", employeebrench);
      editContent = editContent.replace("brench_name1", employeebrench);


      // Setting the updated content in the form control
      this.Offerletterform.get("Offerlettertemplate_content")?.setValue(editContent);

      // Assuming terms_list contains the template_gid
      this.Offerletterform.value.template_gid = result.terms_list[0].template_gid;
    });
  }
  employeeDetailsFetch() {
   let employee_gid=this.Existingform.get('employee_gid')?.value;
    
    let param = {
      employee_gid: employee_gid
    }

    var api = 'OfferLetter/GetEditEmployeebind';

    console.log(param);
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata=result;
      this.editexistemployee = this.responsedata.getexistemployee;
      this.Existingform.get("cbobranch")?.setValue(this.editexistemployee[0].branch_name);   
      this.Existingform.get("first_name")?.setValue(this.editexistemployee[0].user_firstname);
      this.Existingform.get("last_name")?.setValue(this.editexistemployee[0].user_lastname);
      this.Existingform.get("gender")?.setValue(this.editexistemployee[0].employee_gender);
      this.Existingform.get("Experience")?.setValue(this.editexistemployee[0].employee_experience);
      this.Existingform.get("dob")?.setValue(this.editexistemployee[0].employee_dob);
      this.Existingform.get("mobile_number")?.setValue(this.editexistemployee[0].employee_mobileno);
      this.Existingform.get("email_address")?.setValue(this.editexistemployee[0].employee_emailid);
      this.Existingform.get("employee_joining_date")?.setValue(this.editexistemployee[0].employee_joiningdate);
      this.Existingform.get("Qualification")?.setValue(this.editexistemployee[0].employee_qualification);
      this.Existingform.get("cbodesignation")?.setValue(this.editexistemployee[0].designation_name);
      this.Existingform.get("designation_name")?.setValue(this.editexistemployee[0].email_address);
      this.Existingform.get("department_name")?.setValue(this.editexistemployee[0].department_name);
      this.Existingform.get("Salary")?.setValue(this.editexistemployee[0].state);
      this.Existingform.get("permanent_address1")?.setValue(this.editexistemployee[0].address1);
      this.Existingform.get("permanent_address2")?.setValue(this.editexistemployee[0].address2);
      this.Existingform.get("permanent_country")?.setValue(this.editexistemployee[0].country_gid);
      this.Existingform.get("permanent_state")?.setValue(this.editexistemployee[0].state);
      this.Existingform.get("permanent_city")?.setValue(this.editexistemployee[0].city);
      this.Existingform.get("permanent_postal")?.setValue(this.editexistemployee[0].postal_code);
      this.Existingform.get("temporary_address1")?.setValue(this.editexistemployee[0].address1);
      this.Existingform.get("temporary_address2")?.setValue(this.editexistemployee[0].address2);
      this.Existingform.get("temporary_city")?.setValue(this.editexistemployee[0].city);
      this.Existingform.get("template_name")?.setValue(this.editexistemployee[0].perm_address2);
      this.Existingform.get("temporary_state")?.setValue(this.editexistemployee[0].state);
      this.Existingform.get("temp_country")?.setValue(this.editexistemployee[0].country_gid);
      this.Existingform.get("offer_no")?.setValue(this.editexistemployee[0].perm_pincode);
      this.Existingform.get("temporary_postal")?.setValue(this.editexistemployee[0].postal_code);
      this.Existingform.get("template_gid")?.setValue(this.editexistemployee[0].temp_address2);
      this.Existingform.get("Offer_date")?.setValue(this.editexistemployee[0].temp_country);     
 
    });
  }
  GetOnChange() {

    let template_gid = this.Existingform.value.template_name;
  
    let param = {
      template_gid: template_gid
    }

    var url = 'AppointmentOrder/GetOnChangeTerms';

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.templatecontent_list = this.responsedata.GetAppointmentdropdown;
      this.Existingform.get("Offerlettertemplate_content")?.setValue(this.templatecontent_list[0].template_content);
      // Fetching values from form controls
      const userFirstName = this.Existingform.value.first_name + ' ' + this.Offerletterform.value.last_name;
      const usergender = this.Existingform.value.gender.toLowerCase(); 
      const employeedesignation = this.Existingform.value.cbodesignation;
      const joiningdate = this.Existingform.value.employee_joining_date; // Assuming the property name is txtemployee_joining_date
      const employeesalary = this.Existingform.value.Salary; // Assuming the property name is txtemployee_joining_date
      const employeebrench = this.Existingform.value.cbobranch; 
      let prefix = '';
       if (this.Existingform.value.gender == 'male') {
            prefix = 'Mr.';
       } 
       else if (this.Existingform.value.gender  == 'female') {
          prefix = 'Ms.';
        }
        const userFullName = prefix +''+ userFirstName;

      // Replacing placeholders with actual values
      let editContent = this.templatecontent_list[0].template_content;
      editContent = editContent.replace("candidate_name", userFullName);
      editContent = editContent.replace("emp_designation", employeedesignation);
      editContent = editContent.replace("joining_date", joiningdate);
      editContent = editContent.replace("Salary", employeesalary);
      editContent = editContent.replace("brench_name", employeebrench);
      editContent = editContent.replace("brench_name1", employeebrench);


      // Setting the updated content in the form control
      this.Existingform.get("Offerlettertemplate_content")?.setValue(editContent);

      // Assuming terms_list contains the template_gid

      this.Existingform.value.template_gid = result.terms_list[0].template_gid;
    });
  }
  submit() { 
    debugger
    var params = {
      employee_gid: this.Existingform.value.employee_gid,
      branch_gid: this.Existingform.value.cbobranch,
      offer_refno: this.Existingform.value.offer_no,
      offer_date: this.Existingform.value.Offer_date,
      first_name: this.Existingform.value.first_name,
      last_name: this.Existingform.value.last_name,
      gender: this.Existingform.value.gender,
      experience_detail: this.Existingform.value.Experience,
      dob: this.Existingform.value.dob,
      mobile_number: this.Existingform.value.mobile_number,
      email_address: this.Existingform.value.email_address,
      joiningdate: this.Existingform.value.employee_joining_date,
      qualification: this.Existingform.value.Qualification,
      department_gid: this.Existingform.value.department_name,
      designation_gid: this.Existingform.value.cbodesignation,
      employee_salary: this.Existingform.value.Salary,
      perm_address1: this.Existingform.value.permanent_address1,
      perm_address2: this.Existingform.value.permanent_address2,
      perm_city: this.Existingform.value.permanent_city,
      perm_pincode: this.Existingform.value.permanent_postal,
      perm_state: this.Existingform.value.permanent_state,
      perm_country: this.Existingform.value.permanent_country,
      temp_address1: this.Existingform.value.temporary_address1,
      temp_address2: this.Existingform.value.temporary_address2,
      temp_city: this.Existingform.value.temporary_city,
      temp_pincode: this.Existingform.value.temporary_postal,
      temp_state: this.Existingform.value.temporary_state,
      temp_country: this.Existingform.value.temp_country,
      template_gid: this.Existingform.value.template_name,
      offertemplate_content: this.Existingform.value.Offerlettertemplate_content,

    }

    var url = 'OfferLetter/Addofferletter'
    this.NgxSpinnerService.show();
    this.service.post(url,params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
        this.route.navigate(['/hrm/HrmTrnOfferLetter']);
      }

    });

  }

  back() {
    this.route.navigate(['/hrm/HrmTrnOfferLetter']);
  }

  
}

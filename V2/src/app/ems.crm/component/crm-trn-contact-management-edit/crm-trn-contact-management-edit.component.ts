import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Options } from 'flatpickr/dist/types/options';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { environment } from 'src/environments/environment';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormControl, Validators, FormBuilder } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { ToastrService } from 'ngx-toastr';
import { formatDate } from '@angular/common';
import { Pipe } from '@angular/core';
@Component({
  selector: 'app-crm-trn-contact-management-edit',
  templateUrl: './crm-trn-contact-management-edit.component.html',
  styleUrls: ['./crm-trn-contact-management-edit.component.scss']
})
export class CrmTrnContactManagementEditComponent {
 // new
 contact_gid: any;
 inputValue ="";
 annualincome="";
 monthlyincome="";
 MonthlyincomeValue: string = "";
 txteditfirst_name: any;
 txteditlast_name: any;
 txteditmobile_no: any;
 txteditemail_address: any;
 txteditaddress1: any;
 txteditaddress2: any;
 txteditstate_name: any;
 txteditdistrict_name: any;
 txteditcontact_type: any;
 txteditage: any;
 txteditdob_date: any;
 txteditgender: any;
 txteditaadhaar_no: any;
 txteditindividual_pan_no: any;
 txteditmarital_status: any;
 txteditdesignation_name: any;
 txteditfather_first_name: any;
 txteditfather_last_name: any;
 txteditfather_contact_refno: any;
 txteditmother_first_name: any;
 txteditmother_last_name: any;
 txteditmother_contact_refno: any;
 txteditspouse_first_name: any;
 txteditspouse_last_name: any;
 txteditspouse_contact_refno: any;
 txtediteducation_qualification: any;
 txteditmain_occupation: any;
 txteditannual_income: any;
 txteditmonthly_income: any;
 txteditincome_type: any;
 txtviewcorporate_pan_no: any;
 txtviewofficial_no: any;
 txtviewofficial_mail_address: any;
 txtviewcin_no: any;
 txtviewcertificate_incorporate_no: any;
 txtviewbusiness_start_date: any;
 txtviewyears_in_business: any;
 txtviewmonth_in_business: any;
 txtviewauthorized_person_name: any;
 txtviewgst_registered: any;
 txtviewgst_no: any;
 txtviewgst_state: any;
 txtviewaddress_type: any;
 txtviewcorporate_address1: any;
 txtviewcorporate_address2: any;
 txtviewlandmark: any;
 txtviewprimary_status: any;
 txtviewcorporate_start_date: any;
 txtviewcorporate_end_date: any;
 txtviewescrow: any;
 txtviewlast_year_turnover: any;
 designation_gid: any;
 EditForm!: FormGroup;
 ContactEditIndividualForm: FormGroup | any;
 //new
 designationList: any;
 reportingtoList: any;
 txtemployee_joining_date: any;
 rdbcontacttype:any
 ContactEditForm : FormGroup | any;
 //ContactEditIndividualForm: FormGroup | any;
 ContactEditCompanyForm: FormGroup | any;
 mappingdtlList: any;
 stateList: any;
 value1={
   rdbcontacttype: ''
 }
 selectedstate: any;
 
 DistrictList: any;

 gst_list : any[] = [];
 gstFormData = {
     gst_registered:'Yes',
     gst_no: '',
     gst_state: '',
   }
 address_list: any[] = [];
 addressFormData = {
     addresstype : {
       Addresstype: null
     },
     txt_address1 : '',
     txt_address2 : '',
     rdbpstatus : '',
     txt_landmark : ''
   }
 constitution_list: any;
 txt_Addresstype: any;
 sample: any;
 LocationList: any;
 RegionList:any
 zonalList:any;
 result: any;
 edittxt_dob: any;
 txtedit_enddate: any;
 txtedit_startdate: any;
 amountcomma:any;
 amountwords:any;
 constructor(public router:Router,private SocketService: SocketService,private FormBuilder: FormBuilder,private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService,public ActivatedRoute: ActivatedRoute,) {
   this.Sample();
 }


Sample(){
this.ContactEditIndividualForm = this.FormBuilder.group({
   edittxtage:[null,],
   edittxt_dob:[null,],
   rdbeditgender:['Male',],
   txtedit_aadhaarno: [null, [Validators.required, Validators.pattern(/^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/)]],
   txtedit_pannumber:[null, [Validators.required,Validators.pattern(/^[A-Z]{3}[P]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}$/)]],
   MaritalStatus:[null, [Validators.required]],
   editdesignation_name:[null, [Validators.required]],
   edittxt_father_firstname:[null, [Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
   txtedit_father_lastname:[null,],
   txtedit_father_contactrefno:[null,[Validators.required,Validators.pattern(/^[0-9]+$/),
   Validators.minLength(10),]],
   txtedit_mother_firstname:[null,],
   txtedit_mother_lastname:[null,],
   txtedit_mother_contactrefno:[null,],
   txtedit_spouse_firstname:[null,],
   txtedit_spouse_lastname:[null,],
   txtedit_spouse_contactrefno:[null,],
   txtedit_edu_qualification:[null,[Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
   txtedit_mainoccupation:[null,[Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
   txtedit_annualincome:[null,[Validators.required]],
   txtedit_monthlyincome:[null,[Validators.required]],
   editIncometype:[null,[Validators.required]],
 })

 this.ContactEditCompanyForm = this.FormBuilder.group({
   txtedit_companyname:[null,[Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
   txt_edittradename:[null, [Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
   txtedit_panno:[null, [Validators.required,Validators.pattern(/^[A-Z]{3}[ABCFGHLJTF]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}$/)]],
   txtedit_officialno:[null],
   txtedit_officaialmailid:[null],
     txtedit_cinno:[null],
     txtedit_certificate_incorporation:[null],
     txtedit_business_startdate:[null],
     txtedit_years_business:[null],
     txtedit_month_business:[null],
     txtedit_authorized_person:[null, [Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
     rdbeditgst:['Yes', []],
     txtedit_gst_no:[null, [Validators.pattern(/^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$/)]],
     txtedit_gst_state:[null, [Validators.pattern(/^(?!\s*$).+/)]],
     editAddresstype:[null, [Validators.pattern(/^(?!\s*$).+/)]],
     txtedit_address1:[null, [Validators.pattern(/^(?!\s*$).+/)]],
     txtedit_address2:[null],
     txtedit_landmark:[null],
     rdbeditpstatus:['Yes'],
     txtedit_startdate:[null, [Validators.required,]],
     txtedit_enddate:[null, [Validators.required]],
     editrdbescrow:['Yes'],
     txt_last_yearturnover:[null],
     cboConstitution:[null, [Validators.required,]],
 })
 
 this.ContactEditForm = this.FormBuilder.group ({
   
   txteditfirst_name: [null, [Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
   txteditlast_name:[null, [Validators.pattern(/^(?!\s*$).+/)]],
   txtedit_mobileno:[null, [Validators.required,Validators.pattern(/^[0-9]+$/),
   Validators.minLength(10),]],
   txtedit_emailaddress:[null, [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
   txteditaddress1:[null],
   txteditaddress2:[null],
   editregion_name:[null, [Validators.required]],
   editzonal_name:[null, [Validators.required]],
   editlocation_name:[null, [Validators.required]],
   rdbeditcontacttype:[null, [Validators.required]],
 }) 
 this.ContactEditIndividualForm.get('edittxt_dob').valueChanges.subscribe((dob: string) => {
   if (dob) {
     // Calculate age based on the selected date of birth
     const age = this.calculateAge(dob);
     
     // Update the 'txtage' control with the calculated age
     this.ContactEditIndividualForm.get('edittxtage').setValue(age);
   }
 });
 }

 MaritalStatus = [
   { MaritalStatus: 'Single',MaritalStatus_gid: 'Msts_001'},
   { MaritalStatus: 'Married', MaritalStatus_gid: 'Msts_002'},
   { MaritalStatus: 'Divorced', MaritalStatus_gid: 'Msts_003'},
   { MaritalStatus: 'Widowed', MaritalStatus_gid: 'Msts_004'},
 ];

 
 Incometype = [
   { Incometype: 'Business - Profits and Gains',income_type_gid:'ITY_001' },
   { Incometype: 'Capital Gains',income_type_gid:'ITY_002'},
   { Incometype: 'Commission',income_type_gid:'ITY_003'},
   { Incometype: 'Interest',income_type_gid:'ITY_004'},
   { Incometype: 'Investment',income_type_gid:'ITY_005'},
   { Incometype: 'Other Sources',income_type_gid:'ITY_006'},
 ];


 Addresstype = [
   { Addresstype: 'Permanent Address',addresstype_gid: 'Msts_003'},
   { Addresstype: 'Temporary Address',addresstype_gid: 'Msts_003'},
   { Addresstype: 'Branch Office', addresstype_gid: 'Msts_001'},
   { Addresstype: 'Registered Office',addresstype_gid: 'Msts_002' },
     ];
  
     txtedit_business_startdate:any
 ngOnInit(): void {
   
//    this.NgxSpinnerService.show();
//    this.ActivatedRoute.queryParams.subscribe((params) => {
//      const urlparams = params['hash'];
//      if (urlparams) {
//        const decryptedParam = AES.decrypt(
//          urlparams,
//          environment.secretKey
//        ).toString(enc.Utf8);
//        const paramvalues = decryptedParam.split('&');
//        this.contact_gid = paramvalues[0];
//      }
//      var url = 'ManageEmployee/PopDesignation';
//    this.SocketService.get(url).subscribe((result: any) => {
//      this.designationList  = result.employee;
//    }); 
//      var url = "CmnMstDropdown/GetConstitutionList";
//      this.SocketService.get(url).subscribe((result:any)=>{
//        this.constitution_list=result.constitution_list;   
//        if(result.status==true){
//          var param = {
//            contact_gid: this.contact_gid,
//          };
//          this.NgxSpinnerService.show();
//          var url = 'CmnMstContactManagement/ContactEditView'; 
//          this.SocketService.getparams(url, param).subscribe((result: any) => {
//            this.result = result; 
//            // var business = new Date(result.business_start_date);
//           // this.edittxt_dob = new Date(result.dob_date);
//            // var start  = new Date(result.corporate_start_date);
//            // var end  = new Date(result.corporate_end_date);
   
//            this.ContactEditForm.get('txteditfirst_name').setValue( (result.first_name == null) ? "" : result.first_name);
//            this.ContactEditForm.get('txteditlast_name').setValue((result.last_name == null) ? "" : result.last_name);
//            this.ContactEditForm.get('txtedit_mobileno').setValue((result.mobile_no == null) ? "" : result.mobile_no);
//            this.ContactEditForm.get('txtedit_emailaddress').setValue((result.email_address == null) ? "" : result.email_address);
//            this.ContactEditForm.get('txteditaddress1').setValue((result.address1== null) ? "" : result.address1);
//            this.ContactEditForm.get('txteditaddress2').setValue((result.address2== null) ? "" : result.address2);
//            this.ContactEditForm.get('editregion_name').setValue({region_name: result.region_name, region_gid : result.region_gid});
//            this.ContactEditForm.get('editzonal_name').setValue({zonal_name: result.zonal_name, zonal_gid : result.zonal_gid});
//            this.ContactEditForm.get('editlocation_name').setValue({location_name: result.location_name, location_gid : result.location_gid});
          
//            this.ContactEditForm.get('rdbeditcontacttype').setValue((result.contact_type== null) ? "" : result.contact_type);
//            this.ContactEditIndividualForm.get('edittxtage').setValue((result.age== null) ? "" : result.age);
//            this.ContactEditIndividualForm.get('edittxt_dob').setValue((result.dob_date== null) ? "" : result.dob_date);
//            this.edittxt_dob = this.ContactEditIndividualForm.value.edittxt_dob;
//            // this.edittxt_dob = result.dob_date
//            //this.ContactEditIndividualForm.get('edittxt_dob').setValue((formatDate(dob,'dd-MM-yyyy','en-US') == null) ? "" : formatDate(dob,'dd-MM-yyyy','en-US') );
//            this.ContactEditIndividualForm.get('rdbeditgender').setValue((result.gender== null) ? "" : result.gender);
//            this.ContactEditIndividualForm.get('txtedit_aadhaarno').setValue((result.aadhaar_no== null) ? "" : result.aadhaar_no);
//            this.ContactEditIndividualForm.get('txtedit_pannumber').setValue((result.individual_pan_no== null) ? "" : result.individual_pan_no);
//            this.ContactEditIndividualForm.get('MaritalStatus').setValue({MaritalStatus: result.marital_status, MaritalStatus_gid : result.marital_status_gid});
//            this.ContactEditIndividualForm.get('editdesignation_name').setValue({designation_name: result.designation_name, designation_gid : result.designation_gid});
//            this.ContactEditIndividualForm.get('edittxt_father_firstname').setValue((result.father_first_name== null) ? "" : result.father_first_name);
//            this.ContactEditIndividualForm.get('txtedit_father_lastname').setValue((result.father_last_name== null) ? "" : result.father_last_name);
//            this.ContactEditIndividualForm.get('txtedit_father_contactrefno').setValue((result.father_contact_refno== null) ? "" : result.father_contact_refno);
//            this.ContactEditIndividualForm.get('txtedit_mother_firstname').setValue((result.mother_first_name== null) ? "" : result.mother_first_name);
//            this.ContactEditIndividualForm.get('txtedit_mother_lastname').setValue((result.mother_last_name== null) ? "" : result.mother_last_name);
//            this.ContactEditIndividualForm.get('txtedit_mother_contactrefno').setValue((result.mother_contact_refno== null) ? "" : result.mother_contact_refno);
//            this.ContactEditIndividualForm.get('txtedit_spouse_firstname').setValue((result.spouse_first_name== null) ? "" : result.spouse_first_name);
//            this.ContactEditIndividualForm.get('txtedit_spouse_lastname').setValue((result.spouse_last_name== null) ? "" : result.spouse_last_name);
//            this.ContactEditIndividualForm.get('txtedit_spouse_contactrefno').setValue((result.spouse_contact_refno== null) ? "" : result.spouse_contact_refno);
//            this.ContactEditIndividualForm.get('txtedit_edu_qualification').setValue((result.education_qualification== null) ? "" : result.education_qualification);
//            this.ContactEditIndividualForm.get('txtedit_mainoccupation').setValue((result.main_occupation== null) ? "" : result.main_occupation);
//            this.ContactEditIndividualForm.get('txtedit_annualincome').setValue((result.annual_income== null) ? "" : result.annual_income);
//            this.ContactEditIndividualForm.get('txtedit_monthlyincome').setValue((result.monthly_income== null) ? "" : result.monthly_income);
//            this.ContactEditIndividualForm.get('editIncometype').setValue({Incometype: result.income_type, income_type_gid : result.income_type_gid});
//            this.ContactEditCompanyForm.get('txtedit_companyname').setValue((result.company_name == null) ? "" : result.company_name);
//            this.ContactEditCompanyForm.get('txt_edittradename').setValue((result.trade_name== null) ? "" : result.trade_name);
//            this.ContactEditCompanyForm.get('txtedit_panno').setValue((result.corporate_pan_no== null) ? "" : result.corporate_pan_no);
//            this.ContactEditCompanyForm.get('txtedit_officialno').setValue((result.official_no== null) ? "" : result.official_no);
//            this.ContactEditCompanyForm.get('txtedit_officaialmailid').setValue((result.official_mail_address== null) ? "" : result.official_mail_address);
//            this.ContactEditCompanyForm.get('txtedit_cinno').setValue((result.cin_no== null) ? "" : result.cin_no);
//            this.ContactEditCompanyForm.get('txtedit_certificate_incorporation').setValue((result.certificate_incorporate_no== null) ? "" : result.certificate_incorporate_no);
//           // this.ContactEditCompanyForm.get('txtedit_business_startdate').setValue((result.business_start_date== null) ? "" : result.business_start_date);
//            this.ContactEditCompanyForm.get('txtedit_business_startdate').setValue((result.business_start_date== null) ? "" : result.business_start_date);
//            this.txtedit_business_startdate = this.ContactEditCompanyForm.value.txtedit_business_startdate;
//           //this.ContactEditCompanyForm.get('txtedit_business_startdate').setValue((formatDate(business,'dd-MM-yyyy','en-US') == null) ? "" : formatDate(business,'dd-MM-yyyy','en-US'));
//           this.txtedit_business_startdate = result.business_start_date;
//            this.ContactEditCompanyForm.get('txtedit_years_business').setValue((result.years_in_business== null) ? "" : result.years_in_business);
//            this.ContactEditCompanyForm.get('txtedit_month_business').setValue((result.month_in_business== null) ? "" : result.month_in_business);
//            this.ContactEditCompanyForm.get('txtedit_authorized_person').setValue((result.authorized_person_name== null) ? "" : result.authorized_person_name);
//            this.ContactEditCompanyForm.get('rdbeditgst').setValue((result.gst_registered== null) ? "" : result.gst_registered);
//            this.ContactEditCompanyForm.get('txtedit_gst_no').setValue((result.gst_no== null) ? "" : result.gst_no);
//            this.ContactEditCompanyForm.get('txtedit_gst_state').setValue((result.gst_state== null) ? "" : result.gst_state);
//            this.ContactEditCompanyForm.get('editAddresstype').setValue((result.address_type== null) ? "" : result.address_type);
//            this.ContactEditCompanyForm.get('txtedit_address1').setValue((result.corporate_address1== null) ? "" : result.corporate_address1);
//            this.ContactEditCompanyForm.get('txtedit_address2').setValue((result.corporate_address2== null) ? "" : result.corporate_address2);
//            this.ContactEditCompanyForm.get('txtedit_landmark').setValue((result.landmark== null) ? "" : result.landmark);
//            this.ContactEditCompanyForm.get('rdbeditpstatus').setValue((result.primary_status== null) ? "" : result.primary_status);
//            this.ContactEditCompanyForm.get('cboConstitution').setValue({constitution_gid: result.constitution_gid, constitution_name : result.constitution_name});
   
//            this.ContactEditIndividualForm.get('edittxt_dob').setValue((result.dob_date== null) ? "" : result.dob_date);
//            this.ContactEditCompanyForm.get('txtedit_startdate').setValue((result.corporate_start_date== null) ? "" : result.corporate_start_date);
//            this.ContactEditCompanyForm.get('txtedit_enddate').setValue((result.corporate_end_date== null) ? "" : result.corporate_end_date);
//            this.txtedit_startdate = result.corporate_start_date;
//               this.txtedit_enddate = result.corporate_end_date;
          
//            this.ContactEditCompanyForm.get('txt_last_yearturnover').setValue((result.last_year_turnover== null) ? "" : result.last_year_turnover);
   
//            this.gst_list = (result.gst_list== null) ? [] : result.gst_list;
//            this.address_list = (result.address_list== null) ? [] : result.address_list; 
     
//            this.NgxSpinnerService.hide(); 
//          }); 
//        }
//      });
    
//    });
//    const options: Options = {
//      dateFormat: 'd-m-Y',  
//    };

//    flatpickr('.date-picker', options); 
//     var url = 'ManageEmployee/PopReportingTo';
//     this.SocketService.get(url).subscribe((result: any) => {
//     this.reportingtoList  = result.reportingto;
// });   
  
//    var url = 'CmnMstDropdown/GetRegionList';
//    this.SocketService.get(url).subscribe((result: any) => {
//     this.RegionList= result.region_list;  
    
//   }); 
 
//   this.NgxSpinnerService.hide();
 
}
 onedit()
 {

   var params = {
     first_name: this.ContactEditForm.value.txtfirst_name,
     last_name: this.ContactEditForm.value.txtlast_name,
     mobile_no: this.ContactEditForm.value.txt_mobileno,
     email_address: this.ContactEditForm.value.txt_emailaddress,
     
     region_gid: this.ContactEditForm.value.editregion_name.region_gid,
     region_name: this.ContactEditForm.value.editregion_name.region_name,
     zonal_gid: this.ContactEditForm.value.editzonal_name.zonal_gid,
     zonal_name: this.ContactEditForm.value.editzonal_name.zonal_name,
     location_gid: this.ContactEditForm.value.editlocation_name.location_gid,
     location_name: this.ContactEditForm.value.editlocation_name.location_name,
   
     contact_type:this.ContactEditForm.value.rdbcontacttype,
     address1: this.ContactEditForm.value.txtaddress1,
     address2: this.ContactEditForm.value.txtaddress2,
     //Individual
     age: this.ContactEditForm.value.txtage,
     dob_date : this.ContactEditForm.value.txt_dob,
     gender: this.ContactEditForm.value.rdbgender,
     aadhaar_no: this.ContactEditForm.value.txt_aadhaarno,
     individual_pan_no: this.ContactEditForm.value.txt_pannumber,
     marital_status_gid: this.ContactEditForm.value.MaritalStatus.MaritalStatus_gid,

     marital_status:(this.ContactEditForm.value.MaritalStatus.MaritalStatus == undefined) ? "" : this.ContactEditForm.value.MaritalStatus.MaritalStatus,
     designation_gid:(this.ContactEditForm.value.designation_name.designation_gid == undefined) ? "" : this.ContactEditForm.value.designation_name.designation_gid,
     designation_name:(this.ContactEditForm.value.designation_name.designation_name == undefined) ? "" : this.ContactEditForm.value.designation_name.designation_name,
     father_first_name: this.ContactEditForm.value.txt_father_firstname,
     father_last_name: this.ContactEditForm.value.txt_father_lastname,
     father_contact_refno: this.ContactEditForm.value.txt_father_contactrefno,
     mother_first_name: this.ContactEditForm.value.txt_mother_firstname,
     mother_last_name: this.ContactEditForm.value.txt_mother_lastname,
     mother_contact_refno: this.ContactEditForm.value.txt_mother_contactrefno,
     spouse_first_name: this.ContactEditForm.value.txt_spouse_firstname,
     spouse_last_name: this.ContactEditForm.value.txt_spouse_lastname,
     spouse_contact_refno: this.ContactEditForm.value.txt_spouse_contactrefno,
     education_qualification: this.ContactEditForm.value.txt_edu_qualification,
     main_occupation: this.ContactEditForm.value.txt_mainoccupation,
     annual_income: this.ContactEditForm.value.txt_annualincome,
     monthly_income: this.ContactEditForm.value.txt_monthlyincome,
     income_type: (this.ContactEditForm.value.Incometype.Incometype == undefined) ? "" : this.ContactEditForm.value.Incometype.Incometype,
     income_type_gid: this.ContactEditForm.value.editIncometype.income_type_gid,

     //Corporate

     company_name:this.ContactEditForm.value.txt_companyname,
     trade_name:this.ContactEditForm.value.txt_tradename,
     corporate_pan_no:this.ContactEditForm.value.txt_panno,
     official_no:this.ContactEditForm.value.txt_officialno,
     official_mail_address:this.ContactEditForm.value.txt_officaialmailid,
     cin_no:this.ContactEditForm.value.txt_cinno,
     certificate_incorporate_no:this.ContactEditForm.value.txt_certificate_incorporation,
     business_start_date:this.ContactEditForm.value.txt_business_startdate,
     years_in_business:this.ContactEditForm.value.txt_years_business,
     month_in_business:this.ContactEditForm.value.txt_month_business,
     authorized_person_name:this.ContactEditForm.value.txt_authorized_person,
     gst_registered:this.ContactEditForm.value.rdbgst,
     gst_no:this.ContactEditForm.value.txt_gst_no,
     gst_state:this.ContactEditForm.value.txt_gst_state,
     
     address_type:(this.ContactEditForm.value.Addresstype.Addresstype == undefined) ? "" : this.ContactEditForm.value.Addresstype.Addresstype,

     corporate_address1:this.ContactEditForm.value.txt_address1,
     corporate_address2:this.ContactEditForm.value.txt_address2,
     landmark:this.ContactEditForm.value.txt_landmark,
     primary_status:this.ContactEditForm.value.rdbpstatus,
     
     corporate_start_date:this.ContactEditForm.value.txt_startdate,
     corporate_end_date:this.ContactEditForm.value.txt_enddate,
     escrow:this.ContactEditForm.value.rdbescrow,
     last_year_turnover:this.ContactEditForm.value.txt_last_year_turnover
    
 
     
  }



  var url = 'CmnMstContactManagement/ContactAdd';
  this.SocketService.post(url,params).subscribe((result: any) => {
    this.NgxSpinnerService.hide();
    if (result.status == true) {
      this.ToastrService.success(result.message);
    } else {
      this.ToastrService.warning(result.message);
    } 
    this.router.navigate(['/cmn/CmnMstContactManagement']);
  
  });

 }

 


 backbutton(){
   this.router.navigate(['/cmn/CmnMstContactManagement']);
   
 }
 
 radioClick(type : string){
   if(this.value1.rdbcontacttype != type){
     this.value1.rdbcontacttype = type;
   
   }
   
 }

 change(){
   var parm = {
 region_gid:this.ContactEditForm.value.editregion_name.region_gid
}
 var url = 'CmnMstContactManagement/Getzonal';
  this.SocketService.getparams(url,parm).subscribe((result: any) => {
   this.zonalList= result.selectedzonaldtl;   
 }); 
}


changezonal(){
var parm = {
zonal_gid:this.ContactEditForm.value.editzonal_name.zonal_gid
}
var url = 'CmnMstContactManagement/Getlocation';
this.SocketService.getparams(url,parm).subscribe((result: any) => {
this.LocationList= result.selectedlocationdtl;   
}); 
}




 
 addGST(){
   this.gst_list.push({
     gst_registered: this.gstFormData.gst_registered,
     gst_no: this.gstFormData.gst_no,
     gst_state: this.gstFormData.gst_state
   });
   this.gstFormData.gst_registered='Yes',
   this.gstFormData.gst_no= '',
   this.gstFormData.gst_state= ''
 }
 deleteGST(index: number){
   this.gst_list.splice(index, 1);
 }

 addAddress(){
   this.address_list.push({
     address1: this.addressFormData.txt_address1,
     address2: this.addressFormData.txt_address2,
     landmark: this.addressFormData.txt_landmark,
     primary_status: this.addressFormData.rdbpstatus,
     addresstype: this.addressFormData.addresstype.Addresstype,
   });
   this.addressFormData.addresstype.Addresstype = null,
   this.addressFormData.txt_address1 = '',
   this.addressFormData.txt_address2 = '',
   this.addressFormData.txt_landmark = '',
   this.addressFormData.rdbpstatus = 'Yes'
 }

 deleteAddress(index: number){
   this.address_list.splice(index, 1);
 }

 get isAddButtonDisabled(): boolean {
   // Check if all three fields are empty
   return !this.gstFormData.gst_registered || !this.gstFormData.gst_no || !this.gstFormData.gst_state;
 }

 get isAddAddressDisabled(): boolean {
   // Check if all three fields are empty
   return !this.addressFormData.addresstype || !this.addressFormData.txt_address1 || !this.addressFormData.rdbpstatus;
 }

 oncorporateedit()
 {

   try { 
     if (this.ContactEditCompanyForm.value.txtedit_business_startdate.split("-")) 
     this.txtemployee_joining_date = this.ContactEditCompanyForm.value.txtedit_business_startdate.split("-").reverse().join("-"); 
   } 
   catch (e) {  
     this.txtemployee_joining_date = this.ContactEditCompanyForm.value.txtedit_business_startdate
   }
   try { 
     if (this.ContactEditCompanyForm.value.txtedit_enddate.split("-")) 
     this.txtedit_enddate = this.ContactEditCompanyForm.value.txtedit_enddate.split("-").reverse().join("-"); 
   } 
   catch (e) {  
     this.txtedit_enddate = this.ContactEditCompanyForm.value.txtedit_enddate
   }
   try { 
     if (this.ContactEditCompanyForm.value.txtedit_startdate.split("-")) 
     this.txtedit_startdate = this.ContactEditCompanyForm.value.txtedit_startdate.split("-").reverse().join("-"); 
   } 
   catch (e) {  
     this.txtedit_startdate = this.ContactEditCompanyForm.value.txtedit_startdate
   }
   

   var params = {
     contact_gid: this.contact_gid,
     // Basic Details
     first_name: this.ContactEditForm.value.txteditfirst_name,
     last_name: this.ContactEditForm.value.txteditlast_name,
     mobile_no: this.ContactEditForm.value.txtedit_mobileno,
     email_address: this.ContactEditForm.value.txtedit_emailaddress,

  

     region_gid: this.ContactEditForm.value.editregion_name.region_gid,
     region_name: this.ContactEditForm.value.editregion_name.region_name,
     zonal_gid: this.ContactEditForm.value.editzonal_name.zonal_gid,
     zonal_name: this.ContactEditForm.value.editzonal_name.zonal_name,
     location_gid: this.ContactEditForm.value.editlocation_name.location_gid,
     location_name: this.ContactEditForm.value.editlocation_name.location_name,

     contact_type:this.ContactEditForm.value.rdbeditcontacttype,
     address1: this.ContactEditForm.value.txteditaddress1,
     address2: this.ContactEditForm.value.txteditaddress2,

     // Corporate Details
     company_name:this.ContactEditCompanyForm.value.txtedit_companyname,
     trade_name:this.ContactEditCompanyForm.value.txt_edittradename,
     corporate_pan_no:this.ContactEditCompanyForm.value.txtedit_panno,
     official_no:this.ContactEditCompanyForm.value.txtedit_officialno,
     official_mail_address:this.ContactEditCompanyForm.value.txtedit_officaialmailid,
     cin_no:this.ContactEditCompanyForm.value.txtedit_cinno,
     certificate_incorporate_no:this.ContactEditCompanyForm.value.txtedit_certificate_incorporation,
     business_start_date:this.ContactEditCompanyForm.value.txtedit_business_startdate,
     years_in_business:this.ContactEditCompanyForm.value.txtedit_years_business,
     month_in_business:this.ContactEditCompanyForm.value.txtedit_month_business,
     authorized_person_name:this.ContactEditCompanyForm.value.txtedit_authorized_person,
     gst_registered:this.ContactEditCompanyForm.value.rdbeditgst,
     gst_no:this.ContactEditCompanyForm.value.txtedit_gst_no,
     gst_state:this.ContactEditCompanyForm.value.txtedit_gst_state,
     address_type:(this.ContactEditCompanyForm.value.editAddresstype.Addresstype == undefined) ? "" : this.ContactEditCompanyForm.value.editAddresstype.Addresstype,
     corporate_address1:this.ContactEditCompanyForm.value.txtedit_address1,
     corporate_address2:this.ContactEditCompanyForm.value.txtedit_address2,
     landmark:this.ContactEditCompanyForm.value.txtedit_landmark,
     primary_status:this.ContactEditCompanyForm.value.rdbeditpstatus,
     corporate_start_date:this.ContactEditCompanyForm.value.txtedit_startdate,
     corporate_end_date:this.ContactEditCompanyForm.value.txtedit_enddate,
     escrow:this.ContactEditCompanyForm.value.editrdbescrow,
     last_year_turnover:this.ContactEditCompanyForm.value.txt_last_yearturnover,
     constitution_gid:this.ContactEditCompanyForm.value.cboConstitution.constitution_gid,
     constitution_name:this.ContactEditCompanyForm.value.cboConstitution.constitution_name,
     // Multiple add Values 
     gst_list : this.gst_list,
     address_list : this.address_list, 
   }
   var url = 'CmnMstContactManagement/ContactUpdate';
   this.SocketService.post(url,params).subscribe((result: any) => {
     this.NgxSpinnerService.hide();
     if (result.status == true) {
       this.ToastrService.success(result.message);
     } else {
       this.ToastrService.warning(result.message);
     } 
     this.router.navigate(['/cmn/CmnMstContactManagement']);
   });
 }

 onindividualedit()
 {
   var params = {
     contact_gid: this.contact_gid,
     // Basic Details
     first_name: this.ContactEditForm.value.txteditfirst_name,
     last_name: this.ContactEditForm.value.txteditlast_name,
     mobile_no: this.ContactEditForm.value.txtedit_mobileno,
     email_address: this.ContactEditForm.value.txtedit_emailaddress,
    
     region_gid: this.ContactEditForm.value.editregion_name.region_gid,
     region_name: this.ContactEditForm.value.editregion_name.region_name,
     zonal_gid: this.ContactEditForm.value.editzonal_name.zonal_gid,
     zonal_name: this.ContactEditForm.value.editzonal_name.zonal_name,
     location_gid: this.ContactEditForm.value.editlocation_name.location_gid,
     location_name: this.ContactEditForm.value.editlocation_name.location_name,

     contact_type:this.ContactEditForm.value.rdbeditcontacttype,
     address1: this.ContactEditForm.value.txteditaddress1,
     address2: this.ContactEditForm.value.txteditaddress2,

     //Individual
     age: this.ContactEditIndividualForm.value.edittxtage,
     dob_date : this.ContactEditIndividualForm.value.edittxt_dob,
     gender: this.ContactEditIndividualForm.value.rdbeditgender,
     aadhaar_no: this.ContactEditIndividualForm.value.txtedit_aadhaarno,
     individual_pan_no: this.ContactEditIndividualForm.value.txtedit_pannumber,
     marital_status_gid: this.ContactEditIndividualForm.value.MaritalStatus.MaritalStatus_gid,

     marital_status:(this.ContactEditIndividualForm.value.MaritalStatus.MaritalStatus == undefined) ? "" : this.ContactEditIndividualForm.value.MaritalStatus.MaritalStatus,    
     // designation_gid:(this.ContactEditIndividualForm.value.editdesignation_name.designation_gid == undefined) ? "" : this.ContactEditIndividualForm.value.editdesignation_name.designation_gid,
     // designation_name:(this.ContactEditIndividualForm.value.editdesignation_name.designation_name == undefined) ? "" : this.ContactEditIndividualForm.value.editdesignation_name.designation_name,
      designation_gid:this.ContactEditIndividualForm.value.editdesignation_name.designation_gid,
     designation_name:this.ContactEditIndividualForm.value.editdesignation_name.designation_name,
     father_first_name: this.ContactEditIndividualForm.value.edittxt_father_firstname,
     father_last_name: this.ContactEditIndividualForm.value.txtedit_father_lastname,
     father_contact_refno: this.ContactEditIndividualForm.value.txtedit_father_contactrefno,
     mother_first_name: this.ContactEditIndividualForm.value.txtedit_mother_firstname,
     mother_last_name: this.ContactEditIndividualForm.value.txtedit_mother_lastname,
     mother_contact_refno: this.ContactEditIndividualForm.value.txtedit_mother_contactrefno,
     spouse_first_name: this.ContactEditIndividualForm.value.txtedit_spouse_firstname,
     spouse_last_name: this.ContactEditIndividualForm.value.txtedit_spouse_lastname,
     spouse_contact_refno: this.ContactEditIndividualForm.value.txtedit_spouse_contactrefno,
     education_qualification: this.ContactEditIndividualForm.value.txtedit_edu_qualification,
     main_occupation: this.ContactEditIndividualForm.value.txtedit_mainoccupation,
     annual_income: this.ContactEditIndividualForm.value.txtedit_annualincome,
     monthly_income: this.ContactEditIndividualForm.value.txtedit_monthlyincome,
     income_type_gid: this.ContactEditIndividualForm.value.editIncometype.income_type_gid,

     income_type: (this.ContactEditIndividualForm.value.editIncometype.Incometype == undefined) ? "" : this.ContactEditIndividualForm.value.editIncometype.Incometype,
   }
   var url = 'CmnMstContactManagement/ContactUpdate';
   this.SocketService.post(url,params).subscribe((result: any) => {
     this.NgxSpinnerService.hide();
     if (result.status == true) {
       this.ToastrService.success(result.message);
     } else {
       this.ToastrService.warning(result.message);
     } 
     this.router.navigate(['/cmn/CmnMstContactManagement']);
   });
 }
 calculateAge(dob: string): number {
   const today = new Date();
   const birthDate = new Date(dob);
   let age = today.getFullYear() - birthDate.getFullYear();

   // Check if birthday has occurred this year
   const isBirthdayPassed = today.getMonth() > birthDate.getMonth() ||
     (today.getMonth() === birthDate.getMonth() && today.getDate() >= birthDate.getDate());

   if (!isBirthdayPassed) {
     age--;
   }

   return age;
 }

}

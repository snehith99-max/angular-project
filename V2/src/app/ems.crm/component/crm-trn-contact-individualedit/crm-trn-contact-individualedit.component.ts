import { Component } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { param } from 'jquery';
import { Location } from '@angular/common';
import {CountryISO,SearchCountryField,} from "ngx-intl-tel-input";
import { environment } from 'src/environments/environment';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-crm-trn-contact-individualedit',
  templateUrl: './crm-trn-contact-individualedit.component.html',
  styleUrls: ['./crm-trn-contact-individualedit.component.scss']
})
export class CrmTrnContactIndividualeditComponent {
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  nomineeyes:boolean=false;
  filesWithId: { file: File; AutoIDkey: string; file_name: string }[] = [];
  // filesWithId: { file: File, AutoIDkey: string }[] | any = [];
  filesWithIds: { file: File, AutoIDkey: string }[] | any = [];
  formDataObject: FormData = new FormData();
  MobileList: any[] = []
  BasicEmaillist: any[] = []
  GentralDOClist: any[] = []
  inputValue: string = "";
  MonthlyincomeValue: string = "";
  lastyearValue: string = "";
  designationList: any;
  reportingtoList: any;
  txtemployee_joining_date: any;
  rdbcontacttype: any
  ContactAddForm: FormGroup | any;
  GeneralDocumentForm: FormGroup | any;
  ContactEditIndividualForm: FormGroup | any;
  ContactAddCompanyForm: FormGroup | any;
  ContactBasicForm: FormGroup | any;
  ContactBasicEmailForm: FormGroup | any;
  mappingdtlList: any;
  stateList: any;
  temporary_country: any;
  permanent_country: any;
  value1 = {
    rdbcontacttype: 'Individual'
  }
  selectedstate: any;

  DistrictList: any;
  constitution_list: any;
  gst_list: any[] = [];
  gstFormData = {
    gst_registered: 'Yes',
    gst_no: '',
    gst_state: '',
  }
  address_list: any[] = [];
  addressFormData = {
    addresstype: {
      Addresstype: null
    },
    address1: '',
    address2: '',
    primary_status: '',
    landmark: ''
  }

  txt_Addresstype: any;
  employee_list: any;
  locationzonal_list: any;
  zonalList: any;
  RegionList: any;
  LocationList: any;
  AutoIDkey: any;
  file_name: any;
  isReadonly = true;
  countryList: any;
  txttemporary_postal_code:any
  txttemporary_address_line_one: any;
  txtpermanent_address_line_one: any;
  txttemporary_address_line_two: any;
  txtpermanent_address_line_two: any;
  txtpermanent_country: any;
  txtpermanent_state: any;
  txttmppermanent_city: any;
  txtpermanent_city: any;
  txttmppermanent_postal_code: any;
  txtpermanent_postal_code: any;
  temp_Latitude: any;
  txtlatitude: any;
  temp_Longitude: any;
  txtlongitude: any;
  txttemp_state: any;
  txttemp_country: any;
  addresult: any;
  salutationlist: any;
  contact_gid: any;
  contact_name: any;
  contact_ref_no: any;
  contact_type: any;
  pan_no: any;
  aadhar_no: any;
  individual_dob: any;
  age: any;
  regionnamelist: any[] = []
  source_list: any[] = []
  GetLeaddropdown_list: any[] = [];
  gender_name: any;
  designation_name: any;
  maritalstatus_name: any;
  physicalstatus_name: any;
  address1: any;
  address2: any;
  city: any;
  state: any;
  postal_code: any;
  country_name: any;
  latitude: any;
  longitude: any;
  tempaddress1: any;
  tempaddress2: any;
  tempcity: any;
  tempstate: any;
  temppostal_code: any;
  tempcountry_name: any;
  templatitude: any;
  templongitude: any;
  father_name: any;
  fathercontact_no: any;
  mother_name: any;
  mothercontact_no: any;
  spouse_name: any;
  spousecontact_no: any;
  educationalqualification_name: any;
  main_occupation: any;
  annual_income: any;
  monthly_income: any;
  incometype_name: any;
  mobile_list: any;
  email_list: any;
  DocumentList: any;
  salutation: any;
  first_name: any;
  middle_name: any;
  last_name: any;
  father_firstname: any;
  father_lastname: any;
  mother_firstname: any;
  mother_lastname: any;
  spouse_firstname: any;
  spouse_lastname: any;
  gender_gid: any;
  designation_gid: any;
  maritalstatus_gid: any;
  physicalstatus_gid: any;
  incometype_gid: any;
  salutation_gid: any;
  country_gid: any;
  tempcountry_gid: any;
  
  constructor(private Location: Location, public router: Router, private SocketService: SocketService, private FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService,private ActivatedRoute:ActivatedRoute) {
    this.Sample();
    this.ValidateGeneralocument();
    this.BasicForm();
    this.BasicEmailForm();
  }
  Generaldocumentlist: any[] = [
    { file_name: 'PAN DOC', document_name: 'PAN' }
  ]
  rdbcustomer_yes  () {
    this.nomineeyes = true;
 }
 rdbcustomer_no  () {
  this.nomineeyes = false;

 }
 moveToTab(nextTabId: string) {
  const currentTab = document.querySelector('.tab-pane.show.active');
  const nextTab = document.getElementById(nextTabId);
  
  if (currentTab && nextTab) {
    // Remove active class from current tab
    currentTab.classList.remove('show', 'active');
    // Add active class to next tab
    nextTab.classList.add('show', 'active');

    // Update tab borders
    const tabLinks = document.querySelectorAll('.nav-link');
    tabLinks.forEach(link => link.classList.remove('active'));

    // Find the index of the next tab link
    const nextTabIndex = Array.from(tabLinks).findIndex(link => link.getAttribute('href') === `#${nextTabId}`);
    if (nextTabIndex !== -1) {
      tabLinks[nextTabIndex].classList.add('active');
      this.NgxSpinnerService.show()
      window.scrollTo({
        top: 0,
      });
      this.NgxSpinnerService.hide()

    }
  }
}

moveToPreviousTab(nextTabId: string) {
  const currentTab = document.querySelector('.tab-pane.show.active');
  const nextTab = document.getElementById(nextTabId);
  if (currentTab && nextTab) {
    // Remove active class from current tab
    currentTab.classList.remove('show', 'active');
    // Add active class to next tab
    nextTab.classList.add('show', 'active');

    // Update tab borders
    const tabLinks = document.querySelectorAll('.nav-link');
    tabLinks.forEach(link => link.classList.remove('active'));

    // Find the index of the next tab link
    const nextTabIndex = Array.from(tabLinks).findIndex(link => link.getAttribute('href') === `#${nextTabId}`);
    if (nextTabIndex !== -1) {
      tabLinks[nextTabIndex].classList.add('active');
    }
    window.scrollTo({
      top: 0,
    });
  }

}
  ValidateGeneralocument() {
    this.GeneralDocumentForm = this.FormBuilder.group({
      cboGeneralDocument: ['', Validators.required],
      fileInput: ['', Validators.required],
    });
  }
  BasicForm() {
    this.ContactBasicForm = this.FormBuilder.group({
      rdbprimarystatus: [null,[Validators.required]],
      txt_mobileno: [null, [Validators.required]],

    })
  }
  BasicEmailForm() {
    this.ContactBasicEmailForm = this.FormBuilder.group({
      rdbstatus: [null,[Validators.required]],
      txt_emailaddress: [null, [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
    })
  }
  Sample() {


    this.ContactEditIndividualForm = this.FormBuilder.group({
      txtage: [{ value: null, disabled: false }],
      txt_dob: [null,[Validators.required]],
      gender: [null, [Validators.required]],
      rdbnominee: [null],
      temporary_country: [null],
      txtrelation_type: [null],
      txtnomineefirst_name: [null],
      txtnomineemiddle_name: [null],
      txtnomineelast_name: [null],
      txt_aadhaarno: [null, [Validators.required, Validators.pattern(/^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/)]],
      txt_pannumber: [null, [Validators.required, Validators.pattern(/^[A-Z]{3}[P]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}$/)]],
      txtfirst_name: [{ value: null, disabled: false },[Validators.required]],
      txtmiddle_name: [{ value: null, disabled: false },[Validators.required]],
      txtlast_name: [{ value: null, disabled: false },[Validators.required]],
      MaritalStatus: [null, [Validators.required]],
      PhysicalStatus: [null, [Validators.required]],
      designation_name: [null, [Validators.required]],
      salutation_name: [null,[Validators.required]],
      txt_father_firstname: [null, [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txt_father_lastname: [null],
      txt_father_contactrefno: [null, [Validators.required, Validators.pattern(/^[0-9]+$/),
      Validators.minLength(10),]],
      txt_mother_firstname: [null,],
      txt_mother_lastname: [null,],
      txt_mother_contactrefno: [null,],
      txt_spouse_firstname: [null,],
      txt_spouse_lastname: [null,],
      txttemporary_postal_code: [null,],
      txt_spouse_contactrefno: [null,],
      txt_edu_qualification: [null, [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txt_mainoccupation: [null, [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txt_annualincome: [null, [Validators.required]],
      txt_monthlyincome: [null, [Validators.required]],
      Incometype: [null, [Validators.required]],
      txtpermanent_address_line_one: [null],
      txtpermanent_address_line_two: [null],
      txtpermanent_postal_code: [null],
      txtpermanent_city: [{ value: null, disabled: false }],
      permanent_country: [null],
      txtlatitude: [{ value: null, disabled: false }],
      txtlongitude: [{ value: null, disabled: false }],
      txttemporary_address_line_one: [null],
      txttemporary_address_line_two: [null],
      txttmppermanent_postal_code: [null],
      txttmppermanent_city: [{ value: null, disabled: false }],
      txtpermanent_country: [null],
      temp_Latitude: [{ value: null, disabled: false }],
      temp_Longitude: [{ value: null, disabled: false }],
      txttemppermanent_state: [{ value: null, disabled: false }],
      txtpermanent_state: [{ value: null, disabled: false }],
      txttemp_state: [{ value: null, disabled: false }],
      regionname: [null],
      sourcename: [null],
      referred_by: [null],

    })
    this.ContactEditIndividualForm.get('txt_dob').valueChanges.subscribe((dob: string) => {
      if (dob) {
        // Calculate age based on the selected date of birth
        const age = this.calculateAge(dob);

        // Update the 'txtage' control with the calculated age
        this.ContactEditIndividualForm.get('txtage').setValue(age);
      }
    });
  }

  btncopy(){
    this.address1 = this.tempaddress1;
    this.address2 = this.tempaddress2;
    this.permanent_country = this.tempcountry_name;
    this.state = this.tempstate;
    this.city = this.tempcity;
    this.postal_code = this.temppostal_code;
    this.latitude = this.templatitude;
    this.longitude = this.templongitude;
 
  }


  MaritalStatus = [
    { MaritalStatus: 'Single', MaritalStatus_gid: 'Msts_001' },
    { MaritalStatus: 'Married', MaritalStatus_gid: 'Msts_002' },
    { MaritalStatus: 'Divorced', MaritalStatus_gid: 'Msts_003' },
    { MaritalStatus: 'Widowed', MaritalStatus_gid: 'Msts_004' },
  ];
  PhysicalStatus = [
    { PhysicalStatus: 'Active', PhysicalStatus_gid: 'Phys_001' },
    { PhysicalStatus: 'Disabled', PhysicalStatus_gid: 'Phys_002' }
  ];
  Gender = [
    { Gender: 'Male', Gender_gid: 'GEN_001' },
    { Gender: 'Female', Gender_gid: 'GEN_002' },
    { Gender: 'Transgender', Gender_gid: 'GEN_002' }
  ];


  Incometype = [
    { Incometype: 'Business - Profits and Gains', income_type_gid: 'ITY_001' },
    { Incometype: 'Capital Gains', income_type_gid: 'ITY_002' },
    { Incometype: 'Commission', income_type_gid: 'ITY_003' },
    { Incometype: 'Interest', income_type_gid: 'ITY_004' },
    { Incometype: 'Investment', income_type_gid: 'ITY_005' },
    { Incometype: 'Other Sources', income_type_gid: 'ITY_006' },
  ];

  Addresstype = [
    { Addresstype: 'Permanent Address', addresstype_gid: 'Msts_003' },
    { Addresstype: 'Temporary Address', addresstype_gid: 'Msts_003' },
    { Addresstype: 'Branch Office', addresstype_gid: 'Msts_001' },
    { Addresstype: 'Registered Office', addresstype_gid: 'Msts_002' },
  ];
  Salutations = [
    { salutation: 'Mr', salutation_gid: 'SAL_001' },
    { salutation: 'Mrs', salutation_gid: 'SAL_002' },
    { salutation: 'Master', salutation_gid: 'SAL_003' },
    { salutation: 'Miss', salutation_gid: 'SAL_004' },
    { salutation: 'Ms', salutation_gid: 'SAL_005' },
    { salutation: 'Dr', salutation_gid: 'SAL_006' },
    { salutation: 'Chief', salutation_gid: 'SAL_006' },

  ];







  ngOnInit(): void {
    this.NgxSpinnerService.show();
    const contact_gid = this.ActivatedRoute.snapshot.paramMap.get('leadbank_gid');
    this.contact_gid = contact_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.contact_gid, secretKey).toString(enc.Utf8);
    this.contact_gid = deencryptedParam;
      var param = {
        leadbank_gid: this.contact_gid,
      }
      this.NgxSpinnerService.show();

      var url= 'ContactManagement/ContactEditView';
      this.SocketService.getparams(url,param).subscribe((result:any)=>{
        this.contact_name = result.contact_name;
        this.contact_ref_no = result.contact_ref_no;
        this.contact_type = result.contact_type;
        this.pan_no = result.pan_no;
        this.aadhar_no = result.aadhar_no;
        this.salutation = result.salutation ? result.salutation : null;
        this.first_name = result.first_name;
        this.middle_name = result.middle_name;
        this.last_name = result.last_name;
        this.individual_dob = result.individual_dob;
        this.age = result.age;
        this.gender_name = result.gender_name ? result.gender_name : null;
        this.designation_name = result.designation_name ? result.designation_name : null;
        this.maritalstatus_name = result.maritalstatus_name ? result.maritalstatus_name : null;
        this.physicalstatus_name = result.physicalstatus_name ? result.physicalstatus_name : null;
        this.address1 = result.address1;
        this.address2 = result.address2;
        this.city = result.city;
        this.state = result.state;
        this.postal_code = result.postal_code;
        this.permanent_country = result.country_name ? result.country_name : null;
        this.country_gid = result.country_gid ? result.country_gid : null;
        this.latitude = result.latitude;
        this.longitude = result.longitude;
        this.tempaddress1 = result.tempaddress1;
        this.tempaddress2 = result.tempaddress2;
        this.tempcity = result.tempcity;
        this.tempstate = result.tempstate;
        this.temppostal_code = result.temppostal_code;
        this.tempcountry_name = result.tempcountry_name ? result.tempcountry_name : null;;
        this.tempcountry_gid = result.tempcountry_gid;
        this.templatitude = result.templatitude;
        this.templongitude = result.templongitude;
        this.father_name = result.father_name;
        this.fathercontact_no = result.fathercontact_no;
        this.mother_name = result.mother_name;
        this.mothercontact_no = result.mothercontact_no;
        this.spouse_name = result.spouse_name;
        this.spousecontact_no = result.spousecontact_no;
        this.educationalqualification_name = result.educationalqualification_name;
        this.main_occupation = result.main_occupation;
        this.annual_income = result.annual_income;
        this.monthly_income = result.monthly_income;
        this.incometype_name = result.incometype_name ? result.incometype_name : null;;
        this.father_firstname = result.father_firstname;
        this.father_lastname = result.father_lastname;
        this.mother_firstname = result.mother_firstname;
        this.mother_lastname = result.mother_lastname;
        this.spouse_firstname = result.spouse_firstname;
        this.spouse_lastname = result.spouse_lastname;
        this.gender_gid = result.gender_gid;
        
        this.designation_gid = result.designation_gid;
        this.maritalstatus_gid = result.maritalstatus_gid;
        this.physicalstatus_gid = result.physicalstatus_gid;
        this.incometype_gid = result.incometype_gid;
        this.salutation_gid = result.salutation_gid;

        this.MobileList = result.mobile_list;
        this.BasicEmaillist = result.email_list;
        this.GentralDOClist = result.DocumentList;
        this.NgxSpinnerService.hide();

      });
      
    // var url = 'BsoMstSalutation/SalutationSummary';
    // this.SocketService.get(url).subscribe((result: any) => {
    //   this.salutationlist = result.salutationlist;
    // });
    // var url = 'ManageEmployee/PopReportingTo';
    // this.SocketService.get(url).subscribe((result: any) => {
    //   this.reportingtoList = result.reportingto;
    // });
    // var url = 'ManageEmployee/PopDesignation';
    // this.SocketService.get(url).subscribe((result: any) => {
    //   this.designationList = result.employee;
    // });
    // var url = "CmnMstDropdown/GetConstitutionList";
    // this.SocketService.get(url).subscribe((result: any) => {
    //   this.constitution_list = result.constitution_list;
    // });
    // var url = 'CmnMstDropdown/GetRegionList';
    // this.SocketService.get(url).subscribe((result: any) => {
    //   this.RegionList = result.region_list;
    // });
    // var url = 'ManageEmployee/PopCountry';
    // this.SocketService.get(url).subscribe((result: any) => {
    //   this.countryList  = result.country;
    // });
    var api5 = 'Leadbank/Getcountrynamedropdown'
    this.SocketService.get(api5).subscribe((result: any) => {
      this.countryList = result.country_list;
    });
    var api1 = 'Leadbank/Getsourcedropdown'
    this.SocketService.get(api1).subscribe((result: any) => {
      this.source_list = result.source_list;
      //console.log(this.source_list)
    });

    var api2 = 'Leadbank/Getregiondropdown'
    this.SocketService.get(api2).subscribe((result: any) => {
      this.regionnamelist = result.regionname_list;
    });
    var api = 'AppointmentManagement/GetLeaddropdown';
    this.SocketService.get(api).subscribe((result: any) => {
      this.GetLeaddropdown_list = result.GetLeaddropdown_list;
    });
    const options: Options = {
      dateFormat: 'd-m-Y',

    };
    flatpickr('.date-picker', options);
  }

  onEdit() {
   
    var params = { 
      contact_gid:this.contact_gid,
      country_gid: (this.ContactEditIndividualForm.value.permanent_country == undefined) ? "" : this.ContactEditIndividualForm.value.permanent_country.country_gid,
      country_name: (this.ContactEditIndividualForm.value.permanent_country == undefined) ?"" : this.ContactEditIndividualForm.value.permanent_country.country_name,
      email_list: this.BasicEmaillist,
      mobile_list: this.MobileList,
      salutation_gid: (this.ContactEditIndividualForm.value.salutation_name == undefined) ?  "" : this.ContactEditIndividualForm.value.salutation_name.salutation_gid,
      salutation: (this.ContactEditIndividualForm.value.salutation_name == undefined) ? "" : this.ContactEditIndividualForm.value.salutation_name.salutation,
      first_name: this.ContactEditIndividualForm.value.txtfirst_name,
      middle_name: this.ContactEditIndividualForm.value.txtmiddle_name,
      last_name: this.ContactEditIndividualForm.value.txtlast_name,
      physicalstatus_gid: (this.ContactEditIndividualForm.value.PhysicalStatus == undefined) ? "" : this.ContactEditIndividualForm.value.PhysicalStatus.PhysicalStatus_gid,
      physicalstatus_name: (this.ContactEditIndividualForm.value.PhysicalStatus == undefined) ? "" : this.ContactEditIndividualForm.value.PhysicalStatus.PhysicalStatus,
      age: this.ContactEditIndividualForm.value.txtage,
      individual_dob: this.ContactEditIndividualForm.value.txt_dob,
      gender_gid: (this.ContactEditIndividualForm.value.gender == undefined) ?  "" : this.ContactEditIndividualForm.value.gender.Gender_gid,
      gender_name: (this.ContactEditIndividualForm.value.gender == undefined) ? "" : this.ContactEditIndividualForm.value.gender.Gender,
      aadhar_no: this.ContactEditIndividualForm.value.txt_aadhaarno,
      pan_no: this.ContactEditIndividualForm.value.txt_pannumber,
      marital_status_gid: (this.ContactEditIndividualForm.value.MaritalStatus == undefined) ? "" :this.ContactEditIndividualForm.value.MaritalStatus.MaritalStatus_gid ,
      maritalstatus_name: (this.ContactEditIndividualForm.value.MaritalStatus == undefined) ? "" : this.ContactEditIndividualForm.value.MaritalStatus.MaritalStatus,
      designation_gid: (this.ContactEditIndividualForm.value.designation_name == undefined) ?  "" : this.ContactEditIndividualForm.value.designation_name.designation_gid,
      designation_name: (this.ContactEditIndividualForm.value.designation_name == undefined) ? "" : this.ContactEditIndividualForm.value.designation_name.designation_name,
      //address
      address1: this.ContactEditIndividualForm.value.txtpermanent_address_line_one,
      address2: this.ContactEditIndividualForm.value.txtpermanent_address_line_two,
      state: this.ContactEditIndividualForm.value.txtpermanent_state,
      city: this.ContactEditIndividualForm.value.txtpermanent_city,
      postal_code: this.ContactEditIndividualForm.value.txtpermanent_postal_code,
      latitude:this.ContactEditIndividualForm.value.txtlatitude,
      longitude:this.ContactEditIndividualForm.value.txtlongitude,
      tempaddress1: this.ContactEditIndividualForm.value.txttemporary_address_line_one,
      tempaddress2: this.ContactEditIndividualForm.value.txttemporary_address_line_two,
      tempcountry_gid: (this.ContactEditIndividualForm.value.temporary_country == undefined) ?  "" : this.ContactEditIndividualForm.value.temporary_country.country_gid,
      tempcountry_name: (this.ContactEditIndividualForm.value.temporary_country == undefined) ? "" : this.ContactEditIndividualForm.value.temporary_country.country_name,
      tempstate: this.ContactEditIndividualForm.value.txttemp_state,
      tempcity: this.ContactEditIndividualForm.value.txttmppermanent_city,
      temppostal_code: this.ContactEditIndividualForm.value.txttemporary_postal_code,
      templatitude:this.ContactEditIndividualForm.value.temp_Latitude,
      templongitude:this.ContactEditIndividualForm.value.temp_Longitude,
      contact_type: 'Individual', 
      father_firstname: this.ContactEditIndividualForm.value.txt_father_firstname,
      fathercontact_no: this.ContactEditIndividualForm.value.txt_father_contactrefno,
      father_lastname: (this.ContactEditIndividualForm.value.txt_father_lastname == undefined) ? "" : this.ContactEditIndividualForm.value.txt_father_lastname,
      mother_firstname: (this.ContactEditIndividualForm.value.txt_mother_firstname == undefined) ? "" : this.ContactEditIndividualForm.value.txt_mother_firstname,
      mother_lastname: (this.ContactEditIndividualForm.value.txt_mother_lastname == undefined) ? "" : this.ContactEditIndividualForm.value.txt_mother_lastname,
      mothercontact_no: (this.ContactEditIndividualForm.value.txt_mother_contactrefno == undefined) ? "" : this.ContactEditIndividualForm.value.txt_mother_contactrefno,
      spouse_firstname: (this.ContactEditIndividualForm.value.txt_spouse_firstname == undefined) ? "" : this.ContactEditIndividualForm.value.txt_spouse_firstname,
      spouse_lastname: (this.ContactEditIndividualForm.value.txt_spouse_lastname == undefined) ? "" : this.ContactEditIndividualForm.value.txt_spouse_lastname,
      spousecontact_no: (this.ContactEditIndividualForm.value.txt_spouse_contactrefno == undefined) ? "" : this.ContactEditIndividualForm.value.txt_spouse_contactrefno,
      educationalqualification_name: this.ContactEditIndividualForm.value.txt_edu_qualification,
      main_occupation: this.ContactEditIndividualForm.value.txt_mainoccupation,
      annual_income: this.ContactEditIndividualForm.value.txt_annualincome,
      monthly_income: this.ContactEditIndividualForm.value.txt_monthlyincome,
      incometype_gid: (this.ContactEditIndividualForm.value.Incometype == undefined) ?  "" : this.ContactEditIndividualForm.value.Incometype.incometype_gid,
      incometype_name: (this.ContactEditIndividualForm.value.Incometype == undefined) ? "" : this.ContactEditIndividualForm.value.Incometype.Incometype,
      region_gid: (this.ContactEditIndividualForm.value.regionname == undefined) ? "" : this.ContactEditIndividualForm.value.regionname.region_gid,
      source_gid: (this.ContactEditIndividualForm.value.sourcename == undefined) ? "" : this.ContactEditIndividualForm.value.sourcename.source_gid,
      referred_by: (this.ContactEditIndividualForm.value.referred_by == undefined) ? "" : this.ContactEditIndividualForm.value.referred_by,

    }

    this.NgxSpinnerService.show();
    var url = 'ContactManagement/ContactUpdate';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == true) {
        if (this.GentralDOClist != null && this.GentralDOClist.length > 0) {
          const jsonData = "" + JSON.stringify(this.GentralDOClist) + "";
          this.formDataObject.append('contact_gid', result.contact_gid);  
          this.formDataObject.append('project_flag', "Default");
          this.formDataObject.append('GentralDOClist', jsonData);
          this.addresult = result.status;
          var api = 'CmnMstContactManagement/postdocument'
          this.SocketService.postfile(api, this.formDataObject).subscribe((result: any) => {
            if (result.status == true && this.addresult == true) {
              this.NgxSpinnerService.hide();
              this.ToastrService.success("Contact Details Updated Successfully");
              this.router.navigate(['/crm/CrmTrnIndividualContactSummary']);
            }
  
            else if (result.status == false && this.addresult == true) {
              this.NgxSpinnerService.hide();
              this.ToastrService.success("Contact Details Updated Successfully")
              this.router.navigate(['/crm/CrmTrnIndividualContactSummary']);
  
            }
          });
        }
          else {
            this.ToastrService.success("Contact Details Added Successfully");
            this.router.navigate(['/crm/CrmTrnIndividualContactSummary']);
          }
        } else {
          this.ToastrService.warning(result.message);
          this.router.navigate(['/crm/CrmTrnIndividualContactSummary']);
        }

    });

  }


  backbutton() {
    this.Location.back()
  }

  radioClick(type: string) {
    if (this.value1.rdbcontacttype != type) {
      this.value1.rdbcontacttype = type;
    }
  }


  change() {
    var parm = {
      region_gid: this.ContactAddForm.value.region_name.region_gid
    }
    var url = 'CmnMstContactManagement/Getzonal';
    this.SocketService.getparams(url, parm).subscribe((result: any) => {
      this.zonalList = result.selectedzonaldtl;
    });
  }


  changezonal() {
    var parm = {
      zonal_gid: this.ContactAddForm.value.zonal_name.zonal_gid
    }
    var url = 'CmnMstContactManagement/Getlocation';
    this.SocketService.getparams(url, parm).subscribe((result: any) => {
      this.LocationList = result.selectedlocationdtl;
    });
  }




  addGST() {
    this.gst_list.push({
      gst_registered: this.gstFormData.gst_registered,
      gst_no: this.gstFormData.gst_no,
      gst_state: this.gstFormData.gst_state
    });
    this.gstFormData.gst_registered = 'Yes',
      this.gstFormData.gst_no = '',
      this.gstFormData.gst_state = ''
  }
  deleteGST(index: number) {
    this.gst_list.splice(index, 1);
  }

  addAddress() {
    this.address_list.push({
      address1: this.addressFormData.address1,
      address2: this.addressFormData.address2,
      landmark: this.addressFormData.landmark,
      primary_status: this.addressFormData.primary_status,
      addresstype: this.addressFormData.addresstype.Addresstype,
    });
    this.addressFormData.addresstype.Addresstype = null,
      this.addressFormData.address1 = '',
      this.addressFormData.address2 = '',
      this.addressFormData.landmark = '',
      this.addressFormData.primary_status = 'Yes'
  }

  deleteAddress(index: number) {
    this.address_list.splice(index, 1);
  }

  get isAddButtonDisabled(): boolean {
    // Check if all three fields are empty
    return !this.gstFormData.gst_registered || !this.gstFormData.gst_no || !this.gstFormData.gst_state;
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

  BasicClick() {
    const mobileNumber = this.ContactBasicForm.value.txt_mobileno.e164Number;
    const primaryStatus = this.ContactBasicForm.value.rdbprimarystatus;
    const exists = this.MobileList.some(item => item.mobile_no === mobileNumber);
  
    if (!exists) {
      this.MobileList.push({
        mobile_no: mobileNumber,
        primary_status: primaryStatus
      });
    } else {    
      this.ToastrService.warning("Mobile number already exists in the list.");
      window.scrollTo({
        top: 0,
      });
    }
  
    // Reset the form
    this.ContactBasicForm.reset();
  }
  BasicEmailClick() {
    const emailAddress = this.ContactBasicEmailForm.value.txt_emailaddress;
    const primaryStatus = this.ContactBasicEmailForm.value.rdbstatus;
    const exists = this.BasicEmaillist.some(item => item.email_address === emailAddress);
    if (!exists) {
      this.BasicEmaillist.push({
        email_address: emailAddress,
        primary_status: primaryStatus
      });
    } else {
      this.ToastrService.warning("Email address already exists in the list.");
      window.scrollTo({
        top: 0,
      });
    }
    this.ContactBasicEmailForm.reset();
  }
  

  DeleteDocumentClick(index: any) {
    if (index >= 0 && index < this.MobileList.length) {
      this.MobileList.splice(index, 1);
    }
  }
  DeleteEmailClick(index: any) {
    if (index >= 0 && index < this.BasicEmaillist.length) {
      this.BasicEmaillist.splice(index, 1);
    }
  }
  generateKey(): string {
    return `AutoIDKey${new Date().getTime()}`;
  }
  GentralDOCClick() {
    this.AutoIDkey = this.generateKey();
    const fileInput: HTMLInputElement | null = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      const files: FileList | null = fileInput.files;
      if (files && files.length > 0) {
        for (let i = 0; i < files.length; i++) {
          const file: File = files[i];
          const fileType: string = file.type;
          const fileName: string = file.name;
          const disallowedExtensions: string[] = ['.zip', '.exe', '.sql'];
          if (disallowedExtensions.some(ext => fileName.toLowerCase().endsWith(ext)) || fileType === 'application/zip') {
            this.ToastrService.warning("Unsupported File Format");
            break;
          } else {
            this.formDataObject.append(this.AutoIDkey, file);
            this.file_name = fileName;
            this.formDataObject.append('document_name', this.file_name);
            this.GentralDOClist.push({
              AutoID_Key: this.AutoIDkey,
              document_name: this.GeneralDocumentForm.value.cboGeneralDocument,
              FileName: this.GeneralDocumentForm.value.fileInput,
            });
            const filesArray: { file: File; AutoIDkey: string; file_name: string }[] = Array.from(files).map((file) => ({
              file,
              AutoIDkey: this.AutoIDkey,
              file_name: file.name,
            }));
            this.filesWithId = filesArray;
          }
        }
        fileInput.value = '';
        this.GeneralDocumentForm.reset();
      } else {
        this.ToastrService.warning("Kindly Upload the Document");
      }
    }
  }
  
  viewFiles(AutoIDkey: string): void {
    const fileObject = this.filesWithId.find((fileObj) => fileObj.AutoIDkey === AutoIDkey);
  
    if (fileObject) {
      const file = fileObject.file;
      const contentType = this.getFileContentType(file);
  
      if (contentType) {
        const blob = new Blob([file], { type: contentType });
        const fileUrl = URL.createObjectURL(blob);
        const newTab = window.open(fileUrl, '_blank');
  
        if (newTab) {
          newTab.focus();
        }
  
        setTimeout(() => {
          URL.revokeObjectURL(fileUrl);
        }, 60000);
      } 
    } else {
      console.error('File not found for AutoIDkey:', AutoIDkey);
    }
  }
  

  viewFile(path: string, name: string) {
  const lowerCaseFileName = name.toLowerCase();
    if (!(lowerCaseFileName.endsWith('.pdf') ||
          lowerCaseFileName.endsWith('.jpg') ||
          lowerCaseFileName.endsWith('.jpeg') ||
          lowerCaseFileName.endsWith('.png') ||
          lowerCaseFileName.endsWith('.txt'))){
            window.scrollTo({
              top: 0,
            });
        this.ToastrService.warning('Unsupported file format');
    }
    else {
      var params = {
        file_path: path,
        file_name: name
      }
      var url = 'TskTrnTaskManagement/DownloadDocument';
      this.SocketService.post(url, params).subscribe((result: any) => {
            if (result != null) {
              this.SocketService.fileviewer(result);
            }
          });
        }
  }
  
  downloadFile(AutoIDkey: string, file_name: string): void {
    const fileObject = this.filesWithId.find((fileObj) => fileObj.AutoIDkey === AutoIDkey);
  
    if (fileObject) {
      const file = fileObject.file;
      const fileUrl = URL.createObjectURL(file);
      const a = document.createElement('a');
      a.href = fileUrl;
      a.download = file_name;
      a.click();
      URL.revokeObjectURL(fileUrl);
    } else {
      // Handle the case where the file object is not found
      console.error('File not found for AutoIDkey:', AutoIDkey);
    }
  }
  downloadFiles(path: string, name: string) {
    var params = {
      file_path: path,
      file_name: name
    }
    var url = 'TskTrnTaskManagement/DownloadDocument';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if(result != null){
        this.SocketService.filedownload1(result);
      }
    });
  }
  viewFilesAndFile(autoID: any, path: string, fileName: string) {
    this.viewFile(path, fileName);
    this.viewFiles(autoID);
}
downloadFilesAndFile(autoID: any, path: string, fileName: string) {
  this.downloadFiles(path, fileName);
  this.downloadFile(autoID, fileName);
}

  DeleteGentralDocumentClick(index: any) {
    if (index >= 0 && index < this.GentralDOClist.length) {
      this.GentralDOClist.splice(index, 1);
    }
  }


  getFileContentType(file: File): string | null {
    const lowerCaseFileName = file.name.toLowerCase();
  
    if (lowerCaseFileName.endsWith('.pdf')) {
      return 'application/pdf';
    } else if (lowerCaseFileName.endsWith('.jpg') || lowerCaseFileName.endsWith('.jpeg')) {
      return 'image/jpeg';
    } else if (lowerCaseFileName.endsWith('.png')) {
      return 'image/png';
    } else if (lowerCaseFileName.endsWith('.txt')) {
      return 'text/plain';
    }
  
    return null;
  }
}

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
@Component({
  selector: 'app-crm-trn-contact-management-add',
  templateUrl: './crm-trn-contact-management-add.component.html',
  styleUrls: ['./crm-trn-contact-management-add.component.scss']
})
export class CrmTrnContactManagementAddComponent {
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
  regionnamelist: any[] = []
  source_list: any[] = []
  responsedata:any;
  reportingtoList: any;
  txtemployee_joining_date: any;
  rdbcontacttype: any
  ContactAddForm: FormGroup | any;
  GeneralDocumentForm: FormGroup | any;
  ContactAddIndividualForm: FormGroup | any;
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
  GetLeaddropdown_list: any[] = [];
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
  
  constructor(private Location: Location, public router: Router, private SocketService: SocketService, private FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {
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
      txt_mobileno: [null],

    })
  }
  BasicEmailForm() {
    this.ContactBasicEmailForm = this.FormBuilder.group({
      rdbstatus: [null,[Validators.required]],
      txt_emailaddress: [null, [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
    })
  }
  Sample() {


    this.ContactAddIndividualForm = this.FormBuilder.group({
      txtage: [{ value: null, disabled: true }],
      txt_dob: ['',[]],
      gender: [null, []],
      rdbnominee: [null],
      temporary_country: [null],
      txtrelation_type: [null],
      txtnomineefirst_name: [null],
      txtnomineemiddle_name: [null],
      txtnomineelast_name: [null],
      txt_aadhaarno: [null, [ Validators.pattern(/^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/)]],
      txt_pannumber: [null, [ Validators.pattern(/^[A-Z]{3}[P]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}$/)]],
      txtfirst_name: [{ value: null, disabled: false },[]],
      txtmiddle_name: [{ value: null, disabled: false },[]],
      txtlast_name: [{ value: null, disabled: false },[]],
      MaritalStatus: [null, []],
      PhysicalStatus: [null, []],
      Salutations: [null, []],
      designation_name: [null, []],
      salutation_name: [null,[]],
      txt_father_firstname: [null, [Validators.pattern(/^(?!\s*$).+/)]],
      txt_father_lastname: [null],
      txt_father_contactrefno: [null, [Validators.pattern(/^[0-9]+$/),
      Validators.minLength(10),]],
      txt_mother_firstname: [null,],
      txt_mother_lastname: [null,],
      txt_mother_contactrefno: [null,],
      txt_spouse_firstname: [null,],
      txt_spouse_lastname: [null,],
      txttemporary_postal_code: [null,],
      txt_spouse_contactrefno: [null,],
      txt_edu_qualification: [null, [Validators.pattern(/^(?!\s*$).+/)]],
      txt_mainoccupation: [null, [Validators.pattern(/^(?!\s*$).+/)]],
      txt_annualincome: [null, []],
      txt_monthlyincome: [null, []],
      Incometype: [null, []],
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
    // this.ContactAddIndividualForm.get('txt_dob').valueChanges.subscribe((dob: string) => {
    //   if (dob) {
    //     // Calculate age based on the selected date of birth
    //     const age = this.calculateAge(dob);

    //     // Update the 'txtage' control with the calculated age
    //     this.ContactAddIndividualForm.get('txtage').setValue(age);
    //   }
    // });
  }

  btncopy(){
    this.txttemporary_address_line_one = this.txtpermanent_address_line_one;
    this.txttemporary_address_line_two = this.txtpermanent_address_line_two;
    this.txttemp_country = this.permanent_country;
    this.txttemp_state = this.txtpermanent_state;
    this.txttmppermanent_city = this.txtpermanent_city;
    this.txttemporary_postal_code = this.txtpermanent_postal_code;
    this.temp_Latitude = this.txtlatitude;
    this.temp_Longitude = this.txtlongitude;
 
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
  Salutations = [
    { salutation: 'Mr', salutation_gid: 'SAL_001' },
    { salutation: 'Mrs', salutation_gid: 'SAL_002' },
    { salutation: 'Master', salutation_gid: 'SAL_003' },
    { salutation: 'Miss', salutation_gid: 'SAL_004' },
    { salutation: 'Ms', salutation_gid: 'SAL_005' },
    { salutation: 'Dr', salutation_gid: 'SAL_006' },
    { salutation: 'Chief', salutation_gid: 'SAL_006' },

  ];

  Addresstype = [
    { Addresstype: 'Permanent Address', addresstype_gid: 'Msts_003' },
    { Addresstype: 'Temporary Address', addresstype_gid: 'Msts_003' },
    { Addresstype: 'Branch Office', addresstype_gid: 'Msts_001' },
    { Addresstype: 'Registered Office', addresstype_gid: 'Msts_002' },
  ];

  ngOnInit(): void {
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
     var api5 = 'Leadbank/Getcountrynamedropdown'
    this.SocketService.get(api5).subscribe((result: any) => {
      this.countryList = result.country_list;
    });
    var api4 = 'Employeelist/Getdesignationdropdown'
    this.SocketService.get(api4).subscribe((result: any) => {
    this.designationList = result.Getdesignationdropdown;      
  });
  var api1 = 'Leadbank/Getsourcedropdown'
    this.SocketService.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.source_list = result.source_list;
      //console.log(this.source_list)
    });

    var api2 = 'Leadbank/Getregiondropdown'
    this.SocketService.get(api2).subscribe((result: any) => {
      this.responsedata = result;
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
  get txtfirst_name() {

    return this.ContactAddIndividualForm.get('txtfirst_name')!;

  };
  get txt_mobileno() {

    return this.ContactBasicForm.get('txt_mobileno')!;

  };
  onadd() {
    if (this.ContactAddIndividualForm.value.txtfirst_name != null) {
    var params = {
      country_name: (this.permanent_country == undefined) ? '' : this.permanent_country.country_name,
      country_gid: (this.permanent_country == undefined) ? '' : this.permanent_country.country_gid,
      email_list: this.BasicEmaillist,
      countryList: this.countryList,
      mobile_list: this.MobileList,
      salutation_gid: (this.ContactAddIndividualForm.value.Salutations == undefined) ? "" : this.ContactAddIndividualForm.value.Salutations.salutation_gid,
      salutation: (this.ContactAddIndividualForm.value.Salutations == undefined) ? "" : this.ContactAddIndividualForm.value.Salutations.salutation,
      first_name: this.ContactAddIndividualForm.value.txtfirst_name,
      middle_name: this.ContactAddIndividualForm.value.txtmiddle_name,
      last_name: this.ContactAddIndividualForm.value.txtlast_name,
      physicalstatus_gid: (this.ContactAddIndividualForm.value.PhysicalStatus == undefined) ? "" : this.ContactAddIndividualForm.value.PhysicalStatus.PhysicalStatus_gid,
      physicalstatus_name: (this.ContactAddIndividualForm.value.PhysicalStatus == undefined) ? "" : this.ContactAddIndividualForm.value.PhysicalStatus.PhysicalStatus,
      age: this.ContactAddIndividualForm.value.txtage,
      dob_date: this.ContactAddIndividualForm.value.txt_dob,
      gender: (this.ContactAddIndividualForm.value.gender == undefined) ? "" : this.ContactAddIndividualForm.value.gender.Gender,
      gender_gid: (this.ContactAddIndividualForm.value.gender == undefined) ? "" : this.ContactAddIndividualForm.value.gender.Gender_gid,
      aadhar_no: this.ContactAddIndividualForm.value.txt_aadhaarno,
      pan_no: this.ContactAddIndividualForm.value.txt_pannumber,
      marital_status_gid: (this.ContactAddIndividualForm.value.MaritalStatus == undefined) ? "" : this.ContactAddIndividualForm.value.MaritalStatus.MaritalStatus_gid,
      marital_status: (this.ContactAddIndividualForm.value.MaritalStatus == undefined) ? "" : this.ContactAddIndividualForm.value.MaritalStatus.MaritalStatus,
      designation_gid: (this.ContactAddIndividualForm.value.designation_name == undefined) ? "" : this.ContactAddIndividualForm.value.designation_name.designation_gid,
      designation_name: (this.ContactAddIndividualForm.value.designation_name == undefined) ? "" : this.ContactAddIndividualForm.value.designation_name.designation_name,
      address1: this.ContactAddIndividualForm.value.txtpermanent_address_line_one,
      address2: this.ContactAddIndividualForm.value.txtpermanent_address_line_two,
      state: this.ContactAddIndividualForm.value.txtpermanent_state,
      city: this.ContactAddIndividualForm.value.txtpermanent_city,
      postal_code: this.ContactAddIndividualForm.value.txtpermanent_postal_code,
      latitude: this.ContactAddIndividualForm.value.txtlatitude,
      longitude: this.ContactAddIndividualForm.value.txtlongitude,
      tempaddress1: this.ContactAddIndividualForm.value.txttemporary_address_line_one,
      tempaddress2: this.ContactAddIndividualForm.value.txttemporary_address_line_two,
      tempcountry_name: (this.ContactAddIndividualForm.value.temporary_country == undefined) ? "" : this.ContactAddIndividualForm.value.PhysicalStatus.country_name,
      tempcountry_gid: (this.ContactAddIndividualForm.value.temporary_country == undefined) ? "" : this.ContactAddIndividualForm.value.PhysicalStatus.country_gid,
      tempstate: this.ContactAddIndividualForm.value.txttemp_state,
      tempcity: this.ContactAddIndividualForm.value.txttmppermanent_city,
      temppostal_code: this.ContactAddIndividualForm.value.txttemporary_postal_code,
      templatitude: this.ContactAddIndividualForm.value.temp_Latitude,
      templongitude: this.ContactAddIndividualForm.value.temp_Longitude,
      contact_type: 'Individual',
      // Individual  
      father_first_name: this.ContactAddIndividualForm.value.txt_father_firstname,
      father_contact_refno: this.ContactAddIndividualForm.value.txt_father_contactrefno,
      father_last_name: (this.ContactAddIndividualForm.value.txt_father_lastname == undefined) ? "" : this.ContactAddIndividualForm.value.txt_father_lastname,
      mother_first_name: (this.ContactAddIndividualForm.value.txt_mother_firstname == undefined) ? "" : this.ContactAddIndividualForm.value.txt_mother_firstname,
      mother_last_name: (this.ContactAddIndividualForm.value.txt_mother_lastname == undefined) ? "" : this.ContactAddIndividualForm.value.txt_mother_lastname,
      mother_contact_refno: (this.ContactAddIndividualForm.value.txt_mother_contactrefno == undefined) ? "" : this.ContactAddIndividualForm.value.txt_mother_contactrefno,
      spouse_first_name: (this.ContactAddIndividualForm.value.txt_spouse_firstname == undefined) ? "" : this.ContactAddIndividualForm.value.txt_spouse_firstname,
      spouse_last_name: (this.ContactAddIndividualForm.value.txt_spouse_lastname == undefined) ? "" : this.ContactAddIndividualForm.value.txt_spouse_lastname,
      spouse_contact_refno: (this.ContactAddIndividualForm.value.txt_spouse_contactrefno == undefined) ? "" : this.ContactAddIndividualForm.value.txt_spouse_contactrefno,
      education_qualification: this.ContactAddIndividualForm.value.txt_edu_qualification,
      main_occupation: this.ContactAddIndividualForm.value.txt_mainoccupation,
      annual_income: this.ContactAddIndividualForm.value.txt_annualincome,
      monthly_income: this.ContactAddIndividualForm.value.txt_monthlyincome,
      income_type_gid: (this.ContactAddIndividualForm.value.Incometype == undefined) ? "" : this.ContactAddIndividualForm.value.Incometype.income_type_gid,
      income_type: (this.ContactAddIndividualForm.value.Incometype == undefined) ? "" : this.ContactAddIndividualForm.value.Incometype.Incometype,
      region_gid: (this.ContactAddIndividualForm.value.regionname == undefined) ? "" : this.ContactAddIndividualForm.value.regionname.region_gid,
      source_gid: (this.ContactAddIndividualForm.value.sourcename == undefined) ? "" : this.ContactAddIndividualForm.value.sourcename.source_gid,
      referred_by: (this.ContactAddIndividualForm.value.referred_by == undefined) ? "" : this.ContactAddIndividualForm.value.referred_by,

    };
  
    this.NgxSpinnerService.show();
    var url = 'ContactManagement/ContactAdd';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      
      if (result.status == true) {
        if (this.GentralDOClist != null && this.GentralDOClist.length > 0) {
          const jsonData = "" + JSON.stringify(this.GentralDOClist) + "";
          this.formDataObject.append('contact_gid', result.leadbank_gid);
          this.formDataObject.append('project_flag', "Default");
          this.formDataObject.append('GentralDOClist', jsonData);
          this.addresult = result.status;
  
          var api = 'ContactManagement/postdocument';
          this.SocketService.postfile(api, this.formDataObject).subscribe((fileResult: any) => {
            this.NgxSpinnerService.hide();
            if (fileResult.status == true && this.addresult == true) {
              this.ToastrService.success("Contact Details Added Successfully");
            } else {
              this.ToastrService.success("Contact Details Added Successfully without document");
            }
            this.router.navigate(['/crm/CrmTrnIndividualContactSummary']);
          });
        } else {
          this.ToastrService.success("Contact Details Added Successfully");
          this.router.navigate(['/crm/CrmTrnIndividualContactSummary']);
        }
      } else {
        this.ToastrService.warning(result.message);
        this.router.navigate(['/crm/CrmTrnIndividualContactSummary']);
      }
    });
  }
    else {
    
      window.scrollTo({
  
        top: 0, // Code is used for scroll top after event done
  
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
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


  // calculateAge(dob: string): number {
  //   const today = new Date();
  //   const birthDate = new Date(dob);
  //   let age = today.getFullYear() - birthDate.getFullYear();

  //   // Check if birthday has occurred this year
  //   const isBirthdayPassed = today.getMonth() > birthDate.getMonth() ||
  //     (today.getMonth() === birthDate.getMonth() && today.getDate() >= birthDate.getDate());

  //   if (!isBirthdayPassed) {
  //     age--;
  //   }

  //   return age;
  // }
  calculateAge() {
    const dobString = this.ContactAddIndividualForm.get('txt_dob').value;
    if (dobString && /^\d{2}-\d{2}-\d{4}$/.test(dobString)) {
      const [day, month, year] = dobString.split('-').map(Number);
      const dob = new Date(year, month - 1, day);
      console.log("Parsed Date:", dob);
      
      const today = new Date();
      const age = today.getFullYear() - dob.getFullYear();
      const m = today.getMonth() - dob.getMonth();
      if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) {
        this.ContactAddIndividualForm.get('txtage').setValue(age - 1);
      } else {
        this.ContactAddIndividualForm.get('txtage').setValue(age);
      }
    } else {
      this.ContactAddIndividualForm.get('txtage').setValue('');
    }
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
          this.formDataObject.append(this.AutoIDkey, files[i]);
          this.file_name = files[i].name;
             const file: File = files[i];
          const fileType: string = file.type;
          const fileName: string = file.name;
          const disallowedExtensions: string[] = ['.zip', '.exe', '.sql'];
          if (disallowedExtensions.some(ext => fileName.toLowerCase().endsWith(ext)) || fileType === 'application/zip') {
            this.ToastrService.warning("Unsupported File Format");
            break;
          } else {
          // const DocumentInfo = this.file_name
          // this.formDataObject.append('document_name', DocumentInfo);
          this.GentralDOClist.push({
            AutoID_Key: this.AutoIDkey,
            document_name: this.GeneralDocumentForm.value.cboGeneralDocument,
            file_name: this.GeneralDocumentForm.value.fileInput,
          });
          const filesArray: { file: File; AutoIDkey: string; file_name: string }[] = Array.from(files).map((file) => ({
            file,
            AutoIDkey: this.AutoIDkey,
            file_name: file.name,
          }));
  
          this.filesWithId.push(...filesArray);
        }
        }

        fileInput.value = '';
        this.GeneralDocumentForm.reset();
      } else {
        this.ToastrService.warning("Kindly Upload the Document");
      }
    }


  }

  
  

  downloadFiles(AutoIDkey: string, file_name: string): void {
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
  DeleteGentralDocumentClick(index: any) {
    if (index >= 0 && index < this.GentralDOClist.length) {
      this.GentralDOClist.splice(index, 1);
    }
  }
  viewFile(AutoIDkey: string): void {
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
      } else {
        this.ToastrService.warning('Unsupported file format');
      }
    } else {
      console.error('File not found for AutoIDkey:', AutoIDkey);
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

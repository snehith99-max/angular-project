import { Component } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';
import {CountryISO,SearchCountryField,} from "ngx-intl-tel-input";
import { environment } from 'src/environments/environment';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-crm-trn-contact-corporateedit',
  templateUrl: './crm-trn-contact-corporateedit.component.html',
  styleUrls: ['./crm-trn-contact-corporateedit.component.scss']
})
export class CrmTrnContactCorporateeditComponent {
  hide: boolean = true;
  showreply: boolean = false;
  hide1: boolean = true;
  showreply1: boolean = false;
  showreply2: boolean = false;
  showreply3: boolean = false;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
  CountryISO.India,
  CountryISO.India
  ];
  GentralDOClist: any[] = []
  Basiclist: any[] = []
  BasicEmaillist: any[] = []
  AddressList: any[] = []
  PromoterList: any[] = []
  DirectorList: any[] = []
  GeneralDocumentForm: FormGroup | any;
  GSTForm: FormGroup | any;
  inputValue: string ="";
  MonthlyincomeValue: string = "";
  lastyearValue: string = "";
  designationList: any;
  reportingtoList: any;
  txtemployee_joining_date: any;
  rdbcontacttype:any
  ContactAddForm : FormGroup | any;
  ContactAddIndividualForm: FormGroup | any;
  ContactAddCompanyForm: FormGroup | any;
  AddressCompanyForm: FormGroup | any;
  mappingdtlList: any;
  stateList: any;
  regionname: any;
  sourcename: any;
  referred_by: any;
  value1={
    rdbcontacttype: 'Individual'
  }
  selectedstate: any;
  
  DistrictList: any;
  constitution_list: any;
  gst_list : any[] = [];
  source_list : any[] = [];
  regionnamelist : any[] = [];
  GetLeaddropdown_list : any[] = [];
  gstFormData = {
      gst:'',
      gst_no: '',
      gst_state: '',
    }
  address_list: any[] = [];
  addressFormData = {
      addresstype : {
        Addresstype: null
      },
      address1 : '',
      address2 : '',
      primary_status : '',
      landmark : ''
    }

  txt_Addresstype: any;
  employee_list: any;
  locationzonal_list: any;
  zonalList: any;
  RegionList: any;
  LocationList: any;
  ContactBasicForm: FormGroup | any;
  ContactBasicEmailForm: FormGroup | any;
  ContactPromoterAddForm: FormGroup | any;
  ContactDirectorAddForm: FormGroup | any;
  AutoIDkey: any;
  formDataObject: FormData = new FormData();
  file_name: any;
  filesWithId: { file: File; AutoIDkey: string; file_name: string }[] = [];txtpermanent_address_line_one : string = '';
  // filesWithId: { file: File, AutoIDkey: string }[] | any = [];
  countryList: any;
  tabContent1: boolean = true;
  tabContent2: boolean = false;
  tabContent3: boolean = false;
  tabContent4: boolean = false;
  tabContent5: boolean = false;
  tabContent6: boolean = false;
  addresult: any;
  Drp_country: any;
  salutationlist: any;
  txt_location: any;
  contact_gid: any;
  lgltrade_name: any;
  corporate_pan_no: any;
  contact_type: any;
  lei: any;
  cin: any;
  cin_date: any;
  constitution: any;
  businessstart_date: any;
  businesss_vintage: any;
  tan: any;
  tan_state: any;
  kin: any;
  udhayam_registration: any;
  category_aml: any;
  category_business: any;
  last_year_turnover: any;
  contact_ref_no: any;
  constitution_gid: any;
  amlcategory_gid: any;
  businesscategory_gid: any;
  constitutiondropdown_list: any;

  constructor(private Location:Location,public router:Router,private SocketService: SocketService,private FormBuilder: FormBuilder,private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService,private ActivatedRoute:ActivatedRoute) {
    this.Sample();
    this.BasicForm();
    this.ValidateGeneralocument();
    this.BasicEmailForm();
    this.BasicGSTForm();
    this.PromoterForm();
    this.DirectorForm();
    this.AddressForm();
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
  AddressForm() {
    this.AddressCompanyForm = this.FormBuilder.group({
      rdbpstatus: ['Yes', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      Addresstype: [null, [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txt_address1: [null, [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txt_address2: [null],
      txt_city: [null, [Validators.required,,Validators.pattern(/^(?!\s*$).+/)]],
      txt_state: [{ value: null, disabled: false }, [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      Drp_country: [null, [Validators.required]],
      Longitude: [{ value: null, disabled: false }, [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      Latitude: [{ value: null, disabled: false }, [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      // txtpermanent_postal_code: new FormControl(null, [ Validators.pattern(/^[0-9]+$/),Validators.minLength(6) ]),
      txtpermanent_postal_code: [null, [Validators.required,Validators.pattern(/^[0-9]+$/)]],
      txt_emailaddress: [null, [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
      txt_mobileno: [null, [Validators.required, Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")]],
    });
  }
  PromoterForm() {
    this.ContactPromoterAddForm = this.FormBuilder.group({
      txt_aadhaarno: [null,[Validators.required, Validators.pattern(/^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/)]],
      txt_pannumber:[null, [Validators.required,Validators.pattern(/^[A-Z]{3}[P]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}$/)]],  
      txtfirst_name: [{ value: null, disabled: false },[Validators.required]],
      txtmiddle_name: [{ value: null, disabled: false },[Validators.required]],
      txtlast_name:[{ value: null, disabled: false },[Validators.required]],
      designation_name:[null, [Validators.required]],
      txt_emailaddress: [null, [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
      txt_mobileno: [null, [Validators.required,Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")]],
      salutation_name: [null, [Validators.required]],
    });
  }
  DirectorForm() {
    this.ContactDirectorAddForm = this.FormBuilder.group({
      txt_aadhaarno: [null,[Validators.required, Validators.pattern(/^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/)]],
      txt_pannumber:[null, [Validators.required,Validators.pattern(/^[A-Z]{3}[P]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}$/)]],  
      txtfirst_name: [{ value: null, disabled: false },[Validators.required]],
      txtmiddle_name: [{ value: null, disabled: false },[Validators.required]],
      txtlast_name:[{ value: null, disabled: false },[Validators.required]],
      designation_name:[null, [Validators.required]],
      txt_emailaddress: [null, [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
      txt_mobileno: [null, [Validators.required,Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")]],
      salutation_name: [null, [Validators.required]],
    });
  }
  BasicGSTForm() {
    this.GSTForm = this.FormBuilder.group({
      txt_gst_no: [null, [Validators.required, Validators.pattern(/^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$/)]],
      txt_gst_state: [null, [Validators.required,Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txt_location:[null, [Validators.required,Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]]
    });
  }
  BasicForm() {
    this.ContactBasicForm = this.FormBuilder.group({
      rdbprimarystatus: [null,[Validators.required]],
      txt_mobileno: [null, [Validators.required,Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")]],

    })
  }
  BasicEmailForm() {
    this.ContactBasicEmailForm = this.FormBuilder.group({
      rdbstatus: [null,[Validators.required]],
      txt_emailaddress: [null, [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
    })
  }
Sample(){

 
  this.ContactAddIndividualForm = this.FormBuilder.group({
    txtage:[null],
    txt_dob:[null],
    rdbgender:['Male'],
    txt_aadhaarno: [null,[Validators.required, Validators.pattern(/^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/)]],
    txt_pannumber:[null, [Validators.required,Validators.pattern(/^[A-Z]{3}[P]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}$/)]],
   
    MaritalStatus:[null, [Validators.required]],
    designation_name:[null, [Validators.required]],
    txt_father_firstname:[null, [Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
    txt_father_lastname:[null],
    txt_father_contactrefno:[null,[Validators.required,Validators.pattern(/^[0-9]+$/),
    Validators.minLength(10),]],
    txt_mother_firstname:[null,],
    txt_mother_lastname:[null,],
    txt_mother_contactrefno:[null,],
    txt_spouse_firstname:[null,],
    txt_spouse_lastname:[null,],
    txt_spouse_contactrefno:[null,],
    txt_edu_qualification:[null,[Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
    txt_mainoccupation:[null,[Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
    txt_annualincome:[null,[Validators.required]],
    txt_monthlyincome:[null,[Validators.required]],
    Incometype:[null,[Validators.required]],
  })

  this.ContactAddCompanyForm = this.FormBuilder.group({
    txt_tradename:[{ value: null, disabled: false },[Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
    txt_legalentityname:[{ value: null, disabled: false }],
    txt_panno:[null, [Validators.required,Validators.pattern(/^[A-Z]{3}[ABCFGHLJTF]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}$/)]],
    txt_cinno: [null, [Validators.required, Validators.pattern(/^([LUu]{1})([0-9]{5})([A-Za-z]{2})([0-9]{4})([A-Za-z]{3})([0-9]{6})$/)]],      txt_businessvintage:[{ value: null, disabled: false }],
      txt_TAN_state:[{ value: null, disabled: false }],
      txt_TAN:[null],
      CIN_date:[{ value: null, disabled: false }],
      txt_KIN:[null],
      txt_Udhayam_Registration:[null], 
      txt_AML_Category:[null],
      txt_AML_Business :[null,[]],
      txt_business_startdate:[null,[]],
     txt_last_yearturnover: [null, [Validators.pattern]],
      cboConstitution: [null,],
      regionname: [null],
      sourcename: [null],
      referred_by: new FormControl(null),
  })
  
  this.ContactAddForm = this.FormBuilder.group ({
    
    txtfirst_name: [null, [Validators.required,Validators.pattern(/^(?!\s*$).+/)]],
    txtmiddle_name: [null],
    txtlast_name:[null],
    drobdesignation_name: [null, [Validators.required]],
  }) 
  this.ContactAddIndividualForm.get('txt_dob').valueChanges.subscribe((dob: string) => {
    if (dob) {
      // Calculate age based on the selected date of birth
      const age = this.calculateAge(dob);
      
      // Update the 'txtage' control with the calculated age
      this.ContactAddIndividualForm.get('txtage').setValue(age);
    }
  });
    }
    get txt_cinno() {
      return this.ContactAddCompanyForm.get('txt_cinno');
    }
    get txt_gst_no() {
      return this.GSTForm.get('txt_gst_no');
    }
    get txt_gst_state() {
      return this.GSTForm.get('txt_gst_state');
    }
    AddPromoter() {
      this.showreply = true;
      this.hide = false;
  
    }
    cancelAdd() {
      this.showreply = false;
      this.hide = true;
  
    }
    EditPromoter() {
      this.showreply2 = true;
      this.hide = false;
  
    }
    EditcancelAdd() {
      this.showreply2 = false;
      this.hide = true;
  
    }
    AddDirector() {
      this.showreply1 = true;
      this.hide1 = false;
  
    }
    DirectorcancelAdd() {
      this.showreply1 = false;
      this.hide1 = true;
  
    }
    EditDirector() {
      this.showreply3 = true;
      this.hide1 = false;
  
    }
    DirectorEditcancelAdd() {
      this.showreply3 = false;
      this.hide1 = true;
  
    }
    BasicClick() {
      const params = {
        txt_mobileno: this.ContactBasicForm.value.txt_mobileno.e164Number,
        rdbprimarystatus: this.ContactBasicForm.value.rdbprimarystatus
      }
      // this.Basiclist=this.Basiclist || []
      this.Basiclist.push({
        txt_mobileno: this.ContactBasicForm.value.txt_mobileno.e164Number,
        rdbprimarystatus: this.ContactBasicForm.value.rdbprimarystatus
  
      });
      this.ContactBasicForm.reset();
    }
    BasicEmailClick() {
      const params = {
        txt_emailaddress: this.ContactBasicEmailForm.value.txt_emailaddress,
        rdbstatus: this.ContactBasicEmailForm.value.rdbstatus
      }
      // this.Basiclist=this.Basiclist || []
      this.BasicEmaillist.push({
        txt_emailaddress: this.ContactBasicEmailForm.value.txt_emailaddress,
        rdbstatus: this.ContactBasicEmailForm.value.rdbstatus
  
      });
      this.ContactBasicEmailForm.reset();
    }
    DeleteDocumentClick(index: any) {
      if (index >= 0 && index < this.Basiclist.length) {
        this.Basiclist.splice(index, 1);
      }
    }
    DeleteEmailClick(index: any) {
      if (index >= 0 && index < this.BasicEmaillist.length) {
        this.BasicEmaillist.splice(index, 1);
      }
    }
    Category_AML = [
      { Category_AML: 'High',Category_AML_gid: 'CGYAML_001'},
      { Category_AML: 'Medium', Category_AML_gid: 'CGYAML_002'},
      { Category_AML: 'Low', Category_AML_gid: 'CGYAML_003'},
      { Category_AML: 'Not Applicable', Category_AML_gid: 'CGYAML_004'},
    ];
    addresslists = [
      { addresstype_name: 'Registered Office',addressline1: '1/2 Balaji Nager',addressline2: '1st street,Kalaimagal nagar , Ekkatuthangal',state: 'Tamil Nadu',country: 'India',postal_code: '600032',latitude: '13.0263166',longitude: ' 80.20633549999999',primary_status:'Yes'},
    ];
    Category_Business = [
      { Category_Business: 'Others',Category_Business_gid: 'CGYBS_001'},
      { Category_Business: 'Large', Category_Business_gid: 'CGYBS_002'},
      { Category_Business: 'Medium', Category_Business_gid: 'CGYBS_003'},
      { Category_Business: 'MSME', Category_Business_gid: 'CGYBS_004'},
      { Category_Business: 'SME', Category_Business_gid: 'CGYBS_005'},
      { Category_Business: 'Micro', Category_Business_gid: 'CGYBS_006'},
    ];
    MaritalStatus = [
    { MaritalStatus: 'Single',MaritalStatus_gid: 'Msts_001'},
    { MaritalStatus: 'Married', MaritalStatus_gid: 'Msts_002'},
    { MaritalStatus: 'Divorced', MaritalStatus_gid: 'Msts_003'},
    { MaritalStatus: 'Widowed', MaritalStatus_gid: 'Msts_004'},
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
        this.lgltrade_name = result.lgltrade_name;
        this.corporate_pan_no = result.corporate_pan_no;
        this.contact_type = result.contact_type;
        this.lei = result.lei;
        this.cin = result.cin;
        this.cin_date = result.cin_date;
        this.constitution = result.constitution;
        this.businessstart_date = result.businessstart_date;
        this.businesss_vintage = result.businesss_vintage;
        this.tan = result.tan;
        this.tan_state = result.tan_state;
        this.kin = result.kin;
        this.udhayam_registration = result.udhayam_registration;
        this.category_aml = result.category_aml;
        this.category_business = result.category_business;
        this.last_year_turnover = result.last_year_turnover;
        this.contact_ref_no = result.contact_ref_no;
        this.constitution_gid = result.constitution_gid;
        this.amlcategory_gid = result.amlcategory_gid;
        this.businesscategory_gid = result.businesscategory_gid;
        this.gst_list = result.gst_list;
        this.AddressList = result.address_list;
        this.PromoterList = result.promoter_list;
        this.DirectorList = result.director_list;
        this.GentralDOClist = result.DocumentList;
        this.sourcename = result.source_name;
        this.regionname = result.region_name;
        this.referred_by = result.referred_by;
        this.NgxSpinnerService.hide();
      });
//     var url = 'BsoMstSalutation/SalutationSummary';
//     this.SocketService.get(url).subscribe((result: any) => {
//       this.salutationlist = result.salutationlist;
//     });
//     var url = 'ManageEmployee/PopCountry';
//     this.SocketService.get(url).subscribe((result: any) => {
//       this.countryList = result.country;
//     });
//      var url = 'ManageEmployee/PopReportingTo';
//      this.SocketService.get(url).subscribe((result: any) => {
//      this.reportingtoList  = result.reportingto; 
// });  
//    var url = 'ManageEmployee/PopDesignation';
//     this.SocketService.get(url).subscribe((result: any) => {
//       this.designationList  = result.employee;
//     });
//     var url = "CmnMstDropdown/GetConstitutionList";
//     this.SocketService.get(url).subscribe((result:any)=>{
//       this.constitution_list=result.constitution_list;   
//     });
//     var url = 'CmnMstDropdown/GetRegionList';
//     this.SocketService.get(url).subscribe((result: any) => {
//      this.RegionList= result.region_list;   
//    });  
//    const options: Options = {
//     dateFormat: 'd-m-Y', 
    
//   }; 
//   flatpickr('.date-picker', options); 
var api = 'ContactManagement/Constitutiondropdown' 
this.SocketService.get(api).subscribe((result: any) => {
  this.constitutiondropdown_list=result.Constitutiondropdown_list;
});
var api4 = 'Employeelist/Getdesignationdropdown'
this.SocketService.get(api4).subscribe((result: any) => {
this.designationList = result.Getdesignationdropdown;  
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
});
const options: Options = {
  dateFormat: 'd-m-Y', 
  
}; 
flatpickr('.date-picker', options);


  }


  oncorporateEdit()
  {
    this.NgxSpinnerService.show();
    var params = {
      gst_list : this.gst_list,
     address_list : this.AddressList,
     promoter_list : this.PromoterList,
     director_list : this.DirectorList,
     contact_gid:this.contact_gid,
     region_gid: (this.ContactAddCompanyForm.value.regionname == undefined) ? "" : this.ContactAddCompanyForm.value.regionname.region_gid,
     region_name: (this.ContactAddCompanyForm.value.regionname == undefined) ? "" : this.ContactAddCompanyForm.value.regionname.region_name,
     source_gid: (this.ContactAddCompanyForm.value.sourcename == undefined) ? "" : this.ContactAddCompanyForm.value.sourcename.source_gid,
     source_name: (this.ContactAddCompanyForm.value.sourcename == undefined) ? "" : this.ContactAddCompanyForm.value.sourcename.source_name,
     referred_by: (this.ContactAddCompanyForm.value.referred_by == undefined) ? "" : this.ContactAddCompanyForm.value.referred_by,
     corporate_pan_no: this.ContactAddCompanyForm.value.txt_panno,
     lgltrade_name: this.ContactAddCompanyForm.value.txt_tradename,
     lei: (this.ContactAddCompanyForm.value.txt_legalentityname == undefined) ? "" : this.ContactAddCompanyForm.value.txt_legalentityname,
     cin: this.ContactAddCompanyForm.value.txt_cinno,
     business_start_date: this.ContactAddCompanyForm.value.txt_business_startdate,
     cin_date: (this.ContactAddCompanyForm.value.CIN_date == undefined) ? "" : this.ContactAddCompanyForm.value.CIN_date,
     constitution_gid: (this.ContactAddCompanyForm.value.cboConstitution.constitution_gid == undefined) ?  this.constitution_gid : this.ContactAddCompanyForm.value.cboConstitution.constitution_gid,
     constitution: (this.ContactAddCompanyForm.value.cboConstitution.constitution_name == undefined) ? this.ContactAddCompanyForm.value.cboConstitution : this.ContactAddCompanyForm.value.cboConstitution.constitution_name,
     businesss_vintage: (this.ContactAddCompanyForm.value.txt_businessvintage == undefined) ? "" : this.ContactAddCompanyForm.value.txt_businessvintage,
    tan: (this.ContactAddCompanyForm.value.txt_TAN == undefined) ? "" : this.ContactAddCompanyForm.value.txt_TAN,
    tan_state: (this.ContactAddCompanyForm.value.txt_TAN_state == undefined) ? "" : this.ContactAddCompanyForm.value.txt_TAN_state,
     contact_type:'Corporate',
     kin: (this.ContactAddCompanyForm.value.txt_KIN == undefined) ? "" : this.ContactAddCompanyForm.value.txt_KIN,
    udhayam_registration: (this.ContactAddCompanyForm.value.txt_Udhayam_Registration == undefined) ? "" : this.ContactAddCompanyForm.value.txt_Udhayam_Registration,
    amlcategory_gid: (this.ContactAddCompanyForm.value.txt_AML_Category.Category_AML_gid == undefined) ?  this.amlcategory_gid : this.ContactAddCompanyForm.value.txt_AML_Category.Category_AML_gid,
    category_aml: (this.ContactAddCompanyForm.value.txt_AML_Category.Category_AML == undefined) ? this.ContactAddCompanyForm.value.txt_AML_Category : this.ContactAddCompanyForm.value.txt_AML_Category.Category_AML,
    businesscategory_gid: (this.ContactAddCompanyForm.value.txt_AML_Business.Category_Business_gid == undefined) ?  this.businesscategory_gid : this.ContactAddCompanyForm.value.txt_AML_Business.Category_Business_gid,
    category_business: (this.ContactAddCompanyForm.value.txt_AML_Business.Category_Business == undefined) ? this.ContactAddCompanyForm.value.txt_AML_Business : this.ContactAddCompanyForm.value.txt_AML_Business.Category_Business,
     last_year_turnover:this.ContactAddCompanyForm.value.txt_last_yearturnover,
 
  } 
  this.NgxSpinnerService.show();
  var url = 'ContactManagement/ContactUpdate';
  this.SocketService.post(url,params).subscribe((result: any) => {
    this.NgxSpinnerService.hide();
    if (result.status == true) {{
      if (result.status == true) {
        if (this.GentralDOClist != null && this.GentralDOClist.length > 0) {
        const jsonData = "" + JSON.stringify(this.GentralDOClist) + "";
        this.formDataObject.append('contact_gid', result.contact_gid);  
        this.formDataObject.append('project_flag', "Default");
        this.formDataObject.append('GentralDOClist', jsonData);
        this.addresult = result.status;
        var api = 'ContactManagement/postdocument'
        this.SocketService.postfile(api, this.formDataObject).subscribe((result: any) => {
          if (result.status == true && this.addresult == true) {
            this.NgxSpinnerService.hide();
            this.ToastrService.success("Contact Details Updated Successfully");
            this.router.navigate(['/crm/CrmTrnCorporateContactSummary']);
          }

          else if (result.status == false && this.addresult == true) {
            this.NgxSpinnerService.hide();
            this.ToastrService.success("Contact Details Updated Successfully")
            this.router.navigate(['/crm/CrmTrnCorporateContactSummary']);

          }
        });
      }
      else {
        this.ToastrService.success("Contact Details Added Successfully");
        this.router.navigate(['/crm/CrmTrnCorporateContactSummary']);
      }
    } else {
      this.ToastrService.warning(result.message);
      this.router.navigate(['/crm/CrmTrnCorporateContactSummary']);
    }
    }
    }
    else {
      this.NgxSpinnerService.hide();
      this.ToastrService.warning(result.message)
      this.router.navigate(['/crm/CrmTrnCorporateContactSummary']);

    }

  });
}
   
  backbutton(){
    this.Location.back()
  }
  
  radioClick(type : string){
    if(this.value1.rdbcontacttype != type){
      this.value1.rdbcontacttype = type; 
    } 
  }


  change(){
        var parm = {
      region_gid:this.ContactAddForm.value.region_name.region_gid
    }
      var url = 'CmnMstContactManagement/Getzonal';
       this.SocketService.getparams(url,parm).subscribe((result: any) => {
        this.zonalList= result.selectedzonaldtl;   
      }); 
  }


  changezonal(){
    var parm = {
  zonal_gid:this.ContactAddForm.value.zonal_name.zonal_gid
}
  var url = 'CmnMstContactManagement/Getlocation';
   this.SocketService.getparams(url,parm).subscribe((result: any) => {
    this.LocationList= result.selectedlocationdtl;   
  }); 
}



  
  addGST(){debugger
    this.gst_list.push({
      gst_location: this.GSTForm.value.txt_location,
      gst_no: this.GSTForm.value.txt_gst_no,
      gst_state: this.GSTForm.value.txt_gst_state,
      // gst_location: this.gstFormData.gst,
      // gst_no: this.gstFormData.gst_no,
      // gst_state: this.gstFormData.gst_state
    });
    // this.gstFormData.gst='',
    // this.gstFormData.gst_no= '',
    // this.gstFormData.gst_state= ''
    this.GSTForm.reset()
  }
  deleteGST(index: number){
    this.gst_list.splice(index, 1);
  }

  addAddress(){
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

  deleteAddress(index: number){
    this.address_list.splice(index, 1);
  }

  get isAddButtonDisabled(): boolean {
    // Check if all three fields are empty
    return !this.gstFormData.gst || !this.gstFormData.gst_no || !this.gstFormData.gst_state;
  }

  get isAddAddressDisabled(): boolean {
    // Check if all three fields are empty
    return !this.addressFormData.addresstype || !this.addressFormData.address1 || !this.addressFormData.primary_status;
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
  AddressClick() {debugger
    const params = {
      addresstype_gid: this.AddressCompanyForm.value.Addresstype.addresstype_gid,
      addresstype: this.AddressCompanyForm.value.Addresstype.Addresstype,
      address1: this.AddressCompanyForm.value.txt_address1,
      address2: this.AddressCompanyForm.value.txt_address2,
      city: this.AddressCompanyForm.value.txt_city,
      state: this.AddressCompanyForm.value.txt_state,
      postal_code: this.AddressCompanyForm.value.txtpermanent_postal_code,
      country_name: this.AddressCompanyForm.value.Drp_country.country_name,
      country_gid: this.AddressCompanyForm.value.Drp_country.country_gid,
      latitude: this.AddressCompanyForm.value.Latitude,
      longitude: this.AddressCompanyForm.value.Longitude,
      email_address: this.AddressCompanyForm.value.txt_emailaddress,
      mobile_no: this.AddressCompanyForm.value.txt_mobileno.e164Number,
      primary_status: this.AddressCompanyForm.value.rdbpstatus
    }
    // this.MobileList=this.MobileList || []
    this.AddressList.push({
      addresstype_gid: this.AddressCompanyForm.value.Addresstype.addresstype_gid,
      addresstype: this.AddressCompanyForm.value.Addresstype.Addresstype,
      address1: this.AddressCompanyForm.value.txt_address1,
      address2: this.AddressCompanyForm.value.txt_address2,
      city: this.AddressCompanyForm.value.txt_city,
      state: this.AddressCompanyForm.value.txt_state,
      postal_code: this.AddressCompanyForm.value.txtpermanent_postal_code,
      country_name: this.AddressCompanyForm.value.Drp_country.country_name,
      country_gid: this.AddressCompanyForm.value.Drp_country.country_gid,
      latitude: this.AddressCompanyForm.value.Latitude,
      longitude: this.AddressCompanyForm.value.Longitude,
      email_address: this.AddressCompanyForm.value.txt_emailaddress,
      mobile_no: this.AddressCompanyForm.value.txt_mobileno.e164Number,
      primary_status: this.AddressCompanyForm.value.rdbpstatus

    });
    this.AddressCompanyForm.reset();
  }
  AddressDeleteClick(index: any) {
    if (index >= 0 && index < this.AddressList.length) {
      this.AddressList.splice(index, 1);
    }
  }

  PromoterAdd() {
    const panNumber = this.ContactPromoterAddForm.value.txt_pannumber;
    const mobileNumber = this.ContactPromoterAddForm.value.txt_mobileno.e164Number;
    const emailAddress = this.ContactPromoterAddForm.value.txt_emailaddress;
  
    // Check if the PAN number already exists in the PromoterList array
    const exists = this.PromoterList.some(item => 
      item.pan_no === panNumber
      
    );
  
    if (!exists) {
      // PAN number doesn't exist, so add the promoter to the PromoterList array
      this.PromoterList.push({
        pan_no: panNumber,
        aadhar_no: this.ContactPromoterAddForm.value.txt_aadhaarno,
        first_name: this.ContactPromoterAddForm.value.txtfirst_name,
        middle_name: this.ContactPromoterAddForm.value.txtmiddle_name,
        last_name: this.ContactPromoterAddForm.value.txtlast_name,
        designation: this.ContactPromoterAddForm.value.designation_name.designation_name,
        email: emailAddress,
        mobile: mobileNumber,
        salutation: this.ContactPromoterAddForm.value.salutation_name.salutation_name,
        salutation_gid: this.ContactPromoterAddForm.value.salutation_name.salutation_gid
      });
    } else {
      // PAN number already exists, you can show a message or handle it as per your requirement
      this.ToastrService.warning("Promoter with the same PAN number already exists in the list.");
      window.scrollTo({
        top: 0,
      });
    }
  
    // Reset the form
    this.ContactPromoterAddForm.reset();
    this.showreply = false;
    this.hide = true;
  }
  
  
  PromoterDeleteClick(index: any) {
    if (index >= 0 && index < this.PromoterList.length) {
      this.PromoterList.splice(index, 1);
    }
  }

  DirectorAdd() {
    const panNumber = this.ContactDirectorAddForm.value.txt_pannumber;
    const mobileNumber = this.ContactDirectorAddForm.value.txt_mobileno.e164Number;
    const emailAddress = this.ContactDirectorAddForm.value.txt_emailaddress;
    const exists = this.DirectorList.some(item => 
      item.pan_no === panNumber
    );
  
    if (!exists) {
      this.DirectorList.push({
        pan_no: panNumber,
        aadhar_no: this.ContactDirectorAddForm.value.txt_aadhaarno,
        first_name: this.ContactDirectorAddForm.value.txtfirst_name,
        middle_name: this.ContactDirectorAddForm.value.txtmiddle_name,
        last_name: this.ContactDirectorAddForm.value.txtlast_name,
        designation: this.ContactDirectorAddForm.value.designation_name.designation_name,
        email: emailAddress,
        mobile: mobileNumber,
        salutation: this.ContactDirectorAddForm.value.salutation_name.salutation_name,
        salutation_gid: this.ContactDirectorAddForm.value.salutation_name.salutation_gid
      });
    } else {
      this.ToastrService.warning("Director with the same PAN number already exists in the list.");
      window.scrollTo({
        top: 0,
      });
    }
  
    // Reset the form
    this.ContactDirectorAddForm.reset();
    this.showreply1 = false;
    this.hide1 = true;
  }
  
  DirectorDeleteClick(index: any) {
    if (index >= 0 && index < this.DirectorList.length) {
      this.DirectorList.splice(index, 1);
    }
  }
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  // viewFile(AutoIDkey: string): void {
  //   const fileObject = this.filesWithId.find((fileObj) => fileObj.AutoIDkey === AutoIDkey);
  
  //   if (fileObject) {
  //     const file = fileObject.file;
  //     const contentType = this.getFileContentType(file);
  
  //     if (contentType) {
  //       const blob = new Blob([file], { type: contentType });
  //       const fileUrl = URL.createObjectURL(blob);
  //       const newTab = window.open(fileUrl, '_blank');
  
  //       if (newTab) {
  //         newTab.focus();
  //       }
  
  //       setTimeout(() => {
  //         URL.revokeObjectURL(fileUrl);
  //       }, 60000);
  //     } else {
  //       this.ToastrService.warning('Unsupported file format');
  //     }
  //   } else {
  //     console.error('File not found for AutoIDkey:', AutoIDkey);
  //   }
  // }
  
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
  // downloadFiles(AutoIDkey: string, file_name: string): void {
  //   const fileObject = this.filesWithId.find((fileObj) => fileObj.AutoIDkey === AutoIDkey);
  
  //   if (fileObject) {
  //     const file = fileObject.file;
  //     const fileUrl = URL.createObjectURL(file);
  //     const a = document.createElement('a');
  //     a.href = fileUrl;
  //     a.download = file_name;
  //     a.click();
  //     URL.revokeObjectURL(fileUrl);
  //   } else {
  //     // Handle the case where the file object is not found
  //     console.error('File not found for AutoIDkey:', AutoIDkey);
  //   }
  // }
}

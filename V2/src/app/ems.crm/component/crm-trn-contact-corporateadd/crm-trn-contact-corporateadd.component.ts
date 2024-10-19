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
@Component({
  selector: 'app-crm-trn-contact-corporateadd',
  templateUrl: './crm-trn-contact-corporateadd.component.html',
  styleUrls: ['./crm-trn-contact-corporateadd.component.scss']
})
export class CrmTrnContactCorporateaddComponent {
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
  regionnamelist: any[] = []
  source_list: any[] = []
  responsedata:any;
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
  value1={
    rdbcontacttype: 'Individual'
  }
  selectedstate: any;
  
  DistrictList: any;
  constitution_list: any;
  gst_list : any[] = [];
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
  constitutiondropdown_list: any;

  GetLeaddropdown_list: any[] = [];
  
  
	
	
	  
	  
  constructor(private Location:Location,public router:Router,private SocketService: SocketService,private FormBuilder: FormBuilder,private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService) {
    this.Sample();
    this.BasicForm();
    this.ValidateGeneralocument();
    this.BasicEmailForm();
    this.BasicGSTForm();
    this.PromoterForm();
    this.DirectorForm();
    this.AddressForm();
    this.leaddropdown();
   
	
  }

  leaddropdown(){
    var api = 'AppointmentManagement/GetLeaddropdown';
    this.SocketService.get(api).subscribe((result: any) => {
      this.GetLeaddropdown_list = result.GetLeaddropdown_list;
    });
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
      txt_mobileno: [null],
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
      txt_mobileno: [null],
      salutation_name: [null, []],
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
      txt_mobileno: [null],
      salutation_name: [null, []],
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
      txt_mobileno: [null],

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
      txt_AML_Business :[null,[Validators.required]],
      txt_business_startdate:[null,[Validators.required]],
     txt_last_yearturnover: [null, [Validators.required, Validators.pattern]],
      cboConstitution: [null,[Validators.required]],
      regionname: [null,[Validators.required]],
      sourcename: [null,[Validators.required]],
      referred_by: new FormControl(null, [Validators.required]),
      
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
    	  
  get referred_by() {
    return this.ContactAddCompanyForm.get('referred_by')!;
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

  
  Incometype = [
    { Incometype: 'Business - Profits and Gains',income_type_gid:'ITY_001' },
    { Incometype: 'Capital Gains',income_type_gid:'ITY_002'},
    { Incometype: 'Commission',income_type_gid:'ITY_003'},
    { Incometype: 'Interest',income_type_gid:'ITY_004'},
    { Incometype: 'Investment',income_type_gid:'ITY_005'},
    { Incometype: 'Other Sources',income_type_gid:'ITY_006'},
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
    { Addresstype: 'Permanent Address',addresstype_gid: 'Msts_003'},
    { Addresstype: 'Temporary Address',addresstype_gid: 'Msts_003'},
    { Addresstype: 'Branch Office', addresstype_gid: 'Msts_001'},
    { Addresstype: 'Registered Office',addresstype_gid: 'Msts_002' },
      ];

    
    
         




  ngOnInit(): void {  
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
  var api4 = 'Employeelist/Getdesignationdropdown'
  this.SocketService.get(api4).subscribe((result: any) => {
  this.designationList = result.Getdesignationdropdown;      
});


  var api5 = 'Leadbank/Getcountrynamedropdown'
  this.SocketService.get(api5).subscribe((result: any) => {
  this.countryList = result.country_list;
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
   const options: Options = {
    dateFormat: 'd-m-Y', 
    
  }; 
  flatpickr('.date-picker', options);
  
  var api = 'ContactManagement/Constitutiondropdown' 
  this.SocketService.get(api).subscribe((result: any) => {
    this.constitutiondropdown_list=result.Constitutiondropdown_list;
  });
  }
  get txt_tradename() {

    return this.ContactAddCompanyForm.get('txt_tradename')!;

  };
  oncorporateadd() {
    debugger;
    if (this.ContactAddCompanyForm.value.txt_tradename != null) {
    // Prepare parameters
    var params = {
      gst_list: this.gst_list,
      address_list: this.AddressList,
      promoter_list: this.PromoterList,
      director_list: this.DirectorList,
      corporate_pan_no: this.ContactAddCompanyForm.value.txt_panno,
      lgltrade_name: this.ContactAddCompanyForm.value.txt_tradename,
      lei: (this.ContactAddCompanyForm.value.txt_legalentityname == undefined) ? "" : this.ContactAddCompanyForm.value.txt_legalentityname,
      cin: this.ContactAddCompanyForm.value.txt_cinno,
      cin_date: (this.ContactAddCompanyForm.value.CIN_date == undefined) ? "" : this.ContactAddCompanyForm.value.CIN_date,
      businessstart_date: (this.ContactAddCompanyForm.value.txt_business_startdate == undefined) ? "" : this.ContactAddCompanyForm.value.txt_business_startdate,
      constitution: (this.ContactAddCompanyForm.value.cboConstitution == undefined) ? "" : this.ContactAddCompanyForm.value.cboConstitution.constitution_name,
      constitution_gid: (this.ContactAddCompanyForm.value.cboConstitution == undefined) ? "" : this.ContactAddCompanyForm.value.cboConstitution.constitution_gid,
      region_gid: (this.ContactAddCompanyForm.value.regionname == undefined) ? "" : this.ContactAddCompanyForm.value.regionname.region_gid,
      region_name: (this.ContactAddCompanyForm.value.regionname == undefined) ? "" : this.ContactAddCompanyForm.value.regionname.region_name,
      source_gid: (this.ContactAddCompanyForm.value.sourcename == undefined) ? "" : this.ContactAddCompanyForm.value.sourcename.source_gid,
      source_name: (this.ContactAddCompanyForm.value.sourcename == undefined) ? "" : this.ContactAddCompanyForm.value.sourcename.source_name,
      referred_by: (this.ContactAddCompanyForm.value.referred_by == undefined) ? "" : this.ContactAddCompanyForm.value.referred_by,
      businesss_vintage: (this.ContactAddCompanyForm.value.txt_businessvintage == undefined) ? "" : this.ContactAddCompanyForm.value.txt_businessvintage,
      tan: (this.ContactAddCompanyForm.value.txt_TAN == undefined) ? "" : this.ContactAddCompanyForm.value.txt_TAN,
      tan_state: (this.ContactAddCompanyForm.value.txt_TAN_state == undefined) ? "" : this.ContactAddCompanyForm.value.txt_TAN_state,
      contact_type: 'Corporate',
      kin: (this.ContactAddCompanyForm.value.txt_KIN == undefined) ? "" : this.ContactAddCompanyForm.value.txt_KIN,
      udhayam_registration: (this.ContactAddCompanyForm.value.txt_Udhayam_Registration == undefined) ? "" : this.ContactAddCompanyForm.value.txt_Udhayam_Registration,
      category_aml: (this.ContactAddCompanyForm.value.txt_AML_Category == undefined) ? "" : this.ContactAddCompanyForm.value.txt_AML_Category.Category_AML,
      amlcategory_gid: (this.ContactAddCompanyForm.value.txt_AML_Category == undefined) ? "" : this.ContactAddCompanyForm.value.txt_AML_Category.Category_AML_gid,
      category_business: (this.ContactAddCompanyForm.value.txt_AML_Business == undefined) ? "" : this.ContactAddCompanyForm.value.txt_AML_Business.Category_Business,
      businesscategory_gid: (this.ContactAddCompanyForm.value.txt_AML_Business == undefined) ? "" : this.ContactAddCompanyForm.value.txt_AML_Business.Category_Business_gid,
      last_year_turnover: this.ContactAddCompanyForm.value.txt_last_yearturnover,
    };
    console.log('urefkewmdlwed',params)
    this.NgxSpinnerService.show();
      var url = 'ContactManagement/ContactAdd';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      
      if (result.status == true) {
        if (this.GentralDOClist != null && this.GentralDOClist.length > 0) {
          const jsonData = "" + JSON.stringify(this.GentralDOClist) + "";
          this.formDataObject.append('contact_gid', result.contact_gid);
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
            this.router.navigate(['/crm/CrmTrnCorporateContactSummary']);
          });
        } else {
          this.ToastrService.success("Contact Details Added Successfully");
          this.router.navigate(['/crm/CrmTrnCorporateContactSummary']);
        }
      } else {
        this.ToastrService.warning(result.message);
        this.router.navigate(['/crm/CrmTrnCorporateContactSummary']);
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
  get txt_mobileno() {
    return this.AddressCompanyForm.get('txt_mobileno')!;
  }
  
  get txt_mobilenopromoter() {
    return this.ContactPromoterAddForm.get('txt_mobileno')!;
  }
  
  get txt_mobilenodirector() {
    return this.ContactDirectorAddForm.get('txt_mobileno')!;
  }
  
  
  PromoterAdd() {
    debugger;
    
    const panNumber = this.ContactPromoterAddForm.value.txt_pannumber || null;
    const mobileNumber = this.ContactPromoterAddForm.value.txt_mobileno ? this.ContactPromoterAddForm.value.txt_mobileno.e164Number : null;
    const emailAddress = this.ContactPromoterAddForm.value.txt_emailaddress || null;
  
    // Check if the PAN number already exists in the PromoterList array
    const exists = this.PromoterList.some(item => item.pan_no === panNumber);
  
    if (!exists) {
      // PAN number doesn't exist, so add the promoter to the PromoterList array, handling possible null values
      this.PromoterList.push({
        pan_no: panNumber,
        aadhar_no: this.ContactPromoterAddForm.value.txt_aadhaarno || null,
        first_name: this.ContactPromoterAddForm.value.txtfirst_name || null,
        middle_name: this.ContactPromoterAddForm.value.txtmiddle_name || null,
        last_name: this.ContactPromoterAddForm.value.txtlast_name || null,
        designation: this.ContactPromoterAddForm.value.designation_name ? this.ContactPromoterAddForm.value.designation_name.designation_name : null,
        email: emailAddress,
        mobile: mobileNumber,
        salutation: this.ContactPromoterAddForm.value.salutation_name ? this.ContactPromoterAddForm.value.salutation_name.salutation : null,
        salutation_gid: this.ContactPromoterAddForm.value.salutation_name ? this.ContactPromoterAddForm.value.salutation_name.salutation_gid : null
      });
    } else {
      // PAN number already exists, show a warning message
      this.ToastrService.warning("Promoter with the same PAN number already exists in the list.");
      window.scrollTo({
        top: 0,
      });
    }
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
    const panNumber = this.ContactDirectorAddForm.value.txt_pannumber || null;
    const mobileNumber = this.ContactDirectorAddForm.value.txt_mobileno ? this.ContactDirectorAddForm.value.txt_mobileno.e164Number : null;
    const emailAddress = this.ContactDirectorAddForm.value.txt_emailaddress || null;
  
    // Check if the PAN number already exists in the DirectorList array
    const exists = this.DirectorList.some(item => item.pan_no === panNumber);
  
    if (!exists) {
      // PAN number doesn't exist, so add the director to the DirectorList array, handling possible null values
      this.DirectorList.push({
        pan_no: panNumber,
        aadhar_no: this.ContactDirectorAddForm.value.txt_aadhaarno || null,
        first_name: this.ContactDirectorAddForm.value.txtfirst_name || null,
        middle_name: this.ContactDirectorAddForm.value.txtmiddle_name || null,
        last_name: this.ContactDirectorAddForm.value.txtlast_name || null,
        designation: this.ContactDirectorAddForm.value.designation_name ? this.ContactDirectorAddForm.value.designation_name.designation_name : null,
        email: emailAddress,
        mobile: mobileNumber,
        salutation: this.ContactDirectorAddForm.value.salutation_name ? this.ContactDirectorAddForm.value.salutation_name.salutation : null,
        salutation_gid: this.ContactDirectorAddForm.value.salutation_name ? this.ContactDirectorAddForm.value.salutation_name.salutation_gid : null
      });
    } else {
      // PAN number already exists, show a warning message
      this.ToastrService.warning("Director with the same PAN number already exists in the list.");
      window.scrollTo({
        top: 0,
      });
    }
  
    // Reset the form after adding the director
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
}

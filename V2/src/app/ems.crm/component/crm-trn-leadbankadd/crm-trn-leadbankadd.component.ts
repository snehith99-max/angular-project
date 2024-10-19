import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";
import flatpickr from 'flatpickr';


interface ILeadbank {
  company_website: string;
  phone2: string;
  phone1: string;
  fax: string;
  email: string;
  leadbank_address1: string;
  active_flag: string;
  addtocustomer: string;
  status: string;
  region_name: string;
  source_name: string;
  country_name: string;
  leadbank_name: string;
  value: string;
  phone: string;
  wedding_day:string;
  birth_day:string;
  interests:string;
  designation: string;
  leadbank_state: string;
  leadbankcontact_name: string;
  user_code: string;
  remarks: string;
  referred_by: string;
  leadbank_address2: string;
  leadbank_city: string;
  leadbank_pin: string;
  mobile: string;
  country_code1: string;
  customer_type:string;
  lead_type:string;
}

@Component({
  selector: 'app-crm-trn-leadbankadd',
  templateUrl: './crm-trn-leadbankadd.component.html',
  styleUrls: ['./crm-trn-leadbankadd.component.scss']
})
export class CrmTrnLeadbankaddComponent implements OnInit{
  //file!:File;
  leadbank!: ILeadbank;
  reactiveForm!: FormGroup;
  entity_list: any[] = [];
  source_list: any[] = [];
  GetLeaddropdown_list: any[] = [];
  //industryList: any[] = [];
  regionnamelist: any[] = [];
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];


  country_list: any[] = [];
  Email_Address: any;
  responsedata: any;
  lspage: any;
  formData: any = {};
  isSubmitting = false;
  customertype_list: any[] = [];
  leadtype_list: any[] = [];
  constructor(private renderer: Renderer2, private el: ElementRef,private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private route: Router,private router:ActivatedRoute) {
    this.leadbank = {} as ILeadbank;
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'Y-m-d',    
    };
    flatpickr('.date-picker', options);  

    const lspage = this.router.snapshot.paramMap.get('lspage');
    this.lspage = lspage;
    const secretKey = 'storyboarderp';
    const deencryptedParam1 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);
    
    this.lspage = deencryptedParam1;

     this.reactiveForm = new FormGroup({
      leadbankcontact_name: new FormControl(this.leadbank.leadbankcontact_name, [
       Validators.minLength(1),
       Validators.maxLength(250)

      ]),
      leadbank_name: new FormControl(this.leadbank.leadbank_name, [
        Validators.required,
      ]),
      referred_by: new FormControl(this.leadbank.referred_by, [
        
      ]),
      customer_type: new FormControl(this.leadbank.customer_type, [
        
      ]),
      lead_type: new FormControl(this.leadbank.lead_type, [
        
      ]),
      leadbank_gid:new FormControl(''),
      wedding_day: new FormControl(this.leadbank.wedding_day, [
      ]),
      birth_day: new FormControl(this.leadbank.birth_day, [
        
      ]),
      interests: new FormControl(this.leadbank.interests, [
        
      ]),

      leadbank_address2: new FormControl(''),
      leadbank_state: new FormControl(''),
      remarks: new FormControl(''),
      status: new FormControl('Y'),
      active_flag: new FormControl('Y'),
      //leadbank_pin: new FormControl(''),
      leadbank_city: new FormControl(''),
      addtocustomer: new FormControl('N'),
      country_code1: new FormControl(''),
      region_name: new FormControl(this.leadbank.region_name, [
       
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      source_name: new FormControl(this.leadbank.source_name, [
       
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),

      phone2: new FormControl(this.leadbank.phone2, [
        Validators.minLength(7),
        Validators.maxLength(15),
        Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")
      ]),
      phone1: new FormControl(this.leadbank.phone1, [
        Validators.minLength(7),
        Validators.maxLength(15),
        Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")
      ]),
      fax: new FormControl(this.leadbank.fax, [
      Validators.minLength(6),
        Validators.maxLength(10),
        Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")
      ]),

      phone: new FormControl(this.leadbank.phone),
      mobile: new FormControl(''),
      value: new FormControl(this.leadbank.value, [
      ]),
      
      

      designation: new FormControl(this.leadbank.designation, [
       //Validators.pattern("^[A-Za-z\s.-]*$"),
       Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      country_name: new FormControl(this.leadbank.country_name, [
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      leadbank_address1: new FormControl(this.leadbank.leadbank_address1, [
        Validators.maxLength(1000),
      ]),
      // email: new FormControl(this.leadbank.email, [
        
      //   Validators.maxLength(320),
      //    Validators.pattern('^([a-z0-9-]+|[a-z0-9-]+([.][a-z0-9-]+)*)@([a-z0-9-]+\.[a-z]{2,20}(\.[a-z]{2})?|\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\]|localhost)$')
      // ]),
      
      email: new FormControl(this.leadbank.email, [
        Validators.maxLength(320),
        Validators.email  // Angular's built-in email validator
      ]),
      
      company_website: new FormControl(this.leadbank.company_website, [
       
        Validators.minLength(1),
        Validators.maxLength(250),
        Validators.pattern('^(http(s)?:\/\/)?(www\.)?([a-zA-Z0-9-]+(\.[a-zA-Z]{2,})+)')
      ]),

      leadbank_pin: new FormControl(this.leadbank.leadbank_pin, [
        Validators.minLength(5),
        Validators.maxLength(12),      ]),
      
      
    });

    var api1 = 'Leadbank/Getsourcedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.source_list = result.source_list;
      //console.log(this.source_list)
    });

    var api2 = 'Leadbank/Getregiondropdown'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.regionnamelist = result.regionname_list;
    });

    var api3 = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list1;
    });

    var api5 = 'Leadbank/Getcountrynamedropdown'
    this.service.get(api5).subscribe((result: any) => {
      this.responsedata = result;
      this.country_list = result.country_list;
    });
    var api5 = 'Leadbank/Getleadtypedropdown'
    this.service.get(api5).subscribe((result: any) => {
      this.responsedata = result;
      this.leadtype_list = result.leadtype_list;
    });
    var api = 'AppointmentManagement/GetLeaddropdown';
    this.service.get(api).subscribe((result: any) => {
      this.GetLeaddropdown_list = result.GetLeaddropdown_list;
    });
  }
  get customer_type() {
    return this.reactiveForm.get('customer_type')!;
  }

  get branchname() {
    return this.reactiveForm.get('branchname')!;
  }
  get designation() {
    return this.reactiveForm.get('designation')!;
  }
  get country_name() {
    return this.reactiveForm.get('country_name')!;
  }
  get region_name() {
    return this.reactiveForm.get('region_name')!;
  }

  get leadbank_name() {
    return this.reactiveForm.get('leadbank_name')!;
  }

  get leadbankcontact_name() {
    return this.reactiveForm.get('leadbankcontact_name')!;
  }
  get referred_by() {
    return this.reactiveForm.get('referred_by')!;
  }
  get mobile() {
    return this.reactiveForm.get('mobile')!;
  }
  get country_code1(){
    return this.reactiveForm.get('country_code1')!;
  }
  get phone1() {
    return this.reactiveForm.get('phone1')!;
  }
  get wedding_day() {
    return this.reactiveForm.get('wedding_day')!;
  }
  get birth_day(){
    return this.reactiveForm.get('birth_day')!;
  }
  get interests() {
    return this.reactiveForm.get('interests')!;
  }
  get phone2() {
    return this.reactiveForm.get('phone2')!;
  }

  get email() {
    return this.reactiveForm.get('email')!;
  }

  get password() {
    return this.reactiveForm.get('password')!;
  }
  get fax() {
    return this.reactiveForm.get('fax')!;
  }
  get company_website() {
    return this.reactiveForm.get('company_website')!;
  }

  get addtocustomer() {
    return this.reactiveForm.get('addtocustomer')!;
  }

  get status() {
    return this.reactiveForm.get('status')!;
  }

  get source_name() {
    return this.reactiveForm.get('source_name')!;
  }

  get remarks() {
    return this.reactiveForm.get('remarks')!;
  }
  get leadbank_pin() {
    return this.reactiveForm.get('leadbank_pin')!;
  }
  get lead_type() {
    return this.reactiveForm.get('lead_type')!;
  }
  get phone() {
    return this.reactiveForm.get('phone')!;
  }
  
  

  public validate(): void {
    if (!this.reactiveForm.value.phone) {
      this.reactiveForm.value.phone = { e164Number: null }; // Or any default value you prefer
    }
    this.leadbank = this.reactiveForm.value;
    
    if (this.leadbank.customer_type != null && this.leadbank.leadbank_name != null ) {
      // Disable the submit button to prevent multiple clicks
      this.isSubmitting = true;
    
      this.NgxSpinnerService.show();
      // Continue with form submission logic
      var api7 = 'Leadbank/Postleadbank'
      this.service.post(api7, this.leadbank).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          this.route.navigate(['/crm/CrmTrnLeadbanksummary']); 
          this.ToastrService.warning(result.message);
        } else {
          this.route.navigate(['/crm/CrmTrnLeadbanksummary']); 
          this.ToastrService.success(result.message);
        }
        this.responsedata = result;
        
        // Re-enable the submit button after form submission is complete
        this.isSubmitting = false;
      });
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
 
  back() {
    if (this.lspage == 'Registerlead'){
      this.route.navigate(['/crm/CrmTrnLeadMasterSummary']);
    }
    else if (this.lspage == 'LeadBankdistributor') {
      this.route.navigate(['/crm/CrmTrnLeadbanksummary']); 
    } 
    else if (this.lspage == 'LeadBankcorporate') {
      this.route.navigate(['/crm/CrmTrnCorporateLeadBank']); 
    } 
    else if (this.lspage == 'LeadBankretailer') {
      this.route.navigate(['/crm/CrmTrnRetailerLeadBank']); 
    } 
    else {
      this.route.navigate(['/crm/CrmTrnNewtask']); 
    }  
}
  
}

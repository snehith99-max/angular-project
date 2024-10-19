import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

interface ILeadmaster {
  value: string;
  phone: string;
  customer_type: string;
  company_website: string;
  email: string;
  leadbank_address1: string;
  active_flag: string;
  addtocustomer: string;
  status: string;
  region_name: string;
  source_name: string;
  country_name: string;
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
  leadbank_gid: string;
  
}

@Component({
  selector: 'app-crm-trn-retaileradd',
  templateUrl: './crm-trn-retaileradd.component.html',
  styleUrls: ['./crm-trn-retaileradd.component.scss']
})
export class CrmTrnRetaileraddComponent implements OnInit {
  leadmaster!: ILeadmaster;
  reactiveForm!: FormGroup;
  source_list: any[] = [];
  regionnamelist: any[] = [];

  country_list: any[] = [];
  Email_Address: any;
  responsedata: any;
  formData: any = {};
  isSubmitting = false;
  lspage: any;

  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute) {
    this.leadmaster = {} as ILeadmaster;
  }

  ngOnInit(): void {
    const lspage = this.router.snapshot.paramMap.get('lspage');

    this.lspage = lspage;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);

    this.lspage = deencryptedParam;
    console.log("Redirected page:" + this.lspage);

    this.reactiveForm = new FormGroup({
      leadbankcontact_name: new FormControl(this.leadmaster.leadbankcontact_name, [
        Validators.minLength(1),
        Validators.maxLength(250),
        Validators.required,
      ]),

      referred_by: new FormControl(this.leadmaster.referred_by, [
      ]),

      leadbank_address2: new FormControl(''),
      leadbank_state: new FormControl(''),
      remarks: new FormControl(''),
      status: new FormControl('Y'),
      active_flag: new FormControl('Y'),
      //leadbank_pin: new FormControl(''),
      leadbank_city: new FormControl(''),

      region_name: new FormControl(this.leadmaster.region_name, [

        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      source_name: new FormControl(this.leadmaster.source_name, [

        Validators.minLength(1),
        Validators.maxLength(250),
      ]),

      phone: new FormControl(this.leadmaster.phone, [
        Validators.required,]),
      mobile: new FormControl(''),
      value: new FormControl(this.leadmaster.value, [
        Validators.required,
      ]),
      
      designation: new FormControl(this.leadmaster.designation, [
        //Validators.pattern("^[A-Za-z\s.-]*$"),
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      country_name: new FormControl(this.leadmaster.country_name, [
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      leadbank_address1: new FormControl(this.leadmaster.leadbank_address1, [
        Validators.maxLength(1000),
      ]),
      email: new FormControl(this.leadmaster.email, [

        Validators.maxLength(320),
        Validators.email// Angular's built-in email validator
     
      ]),
      company_website: new FormControl(this.leadmaster.company_website, [

        Validators.minLength(1),
        Validators.maxLength(250),
        Validators.pattern('^(http(s)?:\/\/)?(www\.)?([a-zA-Z0-9-]+(\.[a-zA-Z]{2,})+)')
      ]),

      leadbank_pin: new FormControl(this.leadmaster.leadbank_pin, [
        Validators.minLength(5),
        Validators.maxLength(12),
        Validators.pattern('^[A-Za-z0-9_-]+$') // Allow letters, digits, underscore, and hyphen
      ]),
      customer_type: new FormControl('Retailer')
    });

    var api1 = 'registerlead/GetSourcetypedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.source_list = result.GetSourcetypedropdown;
      //console.log(this.source_list)
    });

    var api2 = 'registerlead/Getregiondropdown1'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.regionnamelist = result.Getregiondropdown1;
    });


    var api5 = 'registerlead/Getcountrynamedropdown'
    this.service.get(api5).subscribe((result: any) => {
      this.responsedata = result;
      this.country_list = result.Getcountrynamedropdown;
    });
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
  get leadbankcontact_name() {
    return this.reactiveForm.get('leadbankcontact_name')!;
  }
  get referred_by() {
    return this.reactiveForm.get('referred_by')!;
  }
  get mobile() {
    return this.reactiveForm.get('mobile')!;
  }
  get email() {
    return this.reactiveForm.get('email')!;
  }
  get password() {
    return this.reactiveForm.get('password')!;
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

  back() {
    if (this.lspage == 'Registerlead') {
      this.route.navigate(['/crm/CrmTrnLeadMasterSummary']);
    }
    else if (this.lspage == 'Registerleaddistributor') {
      this.route.navigate(['/crm/CrmTrnLeadMasterSummary']);
    }
    else if (this.lspage == 'Registerleadcorporate') {
      this.route.navigate(['/crm/CrmTrnCorporateRegisterLead']);
    }
    else if (this.lspage == 'Registerleadretailer') {
      this.route.navigate(['/crm/CrmTrnRetailerRegisterLead']);
    }
    else if (this.lspage == 'MyLeadsNewTasks') {
      this.route.navigate(['/crm/CrmTrnNewtask']);
    }
    else if (this.lspage == 'LeadBankretailer'){
      this.route.navigate(['/crm/CrmTrnRetailerLeadBank']);
    }
    else {
      this.route.navigate(['/crm/CrmTrnNewtask']);
    }
  }

  public validate(): void {
    console.log(this.reactiveForm.value);
    if (!this.reactiveForm.value.phone) {
      this.reactiveForm.value.phone = { e164Number: null }; // Or any default value you prefer
    }
    this.leadmaster = this.reactiveForm.value;
  
    if (this.leadmaster.active_flag != null && this.leadmaster.leadbankcontact_name != null) {
      // Disable the submit button to prevent multiple clicks
      this.isSubmitting = true;
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }

      // Continue with form submission logic
      var api7 = 'Leadbank/RetailerPostleadbank'
      this.service.post(api7, this.leadmaster).subscribe((result: any) => {
        if (result.status == false) {

          this.ToastrService.warning(result.message);

        } else {
          // Navigating to another page or displaying success message
          // this.route.navigate(['/crm/CrmTrnLeadMasterSummary']);
          if (this.lspage == 'Registerleadretailer') {
            this.route.navigate(['/crm/CrmTrnRetailerRegisterLead']);
          }
          else {
            this.route.navigate(['/crm/CrmTrnRetailerLeadBank']);
          }

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
}


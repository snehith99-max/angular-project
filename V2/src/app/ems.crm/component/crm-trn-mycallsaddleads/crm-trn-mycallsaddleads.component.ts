import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router,ActivatedRoute } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";
import { NgxSpinnerService } from 'ngx-spinner';
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
  team_name: string;
  source_name: string;
  country_name: string;
  leadbank_name: string;
  value: string;
  phone: string;

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
  selector: 'app-crm-trn-mycallsaddleads',
  templateUrl: './crm-trn-mycallsaddleads.component.html',
  styleUrls: ['./crm-trn-mycallsaddleads.component.scss']
})
export class CrmTrnMycallsaddleadsComponent {
  reactiveForm!: FormGroup;
  leadbank!: ILeadbank;
  country_list: any[] = [];
  Email_Address: any;
  responsedata: any;
  lspage: any;
  formData: any = {};
  isSubmitting = false;
  customertype_list: any[] = [];
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  source_list: any[] = [];
  regionnamelist: any[] = [];
  assignedteamdropdown_list: any[] = [];
  leadtype_list: any[] = [];
  GetLeaddropdown_list: any[] = [];
  constructor(public service: SocketService,private route: Router,private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService,private router:ActivatedRoute){
    this.leadbank = {} as ILeadbank;
  }
  ngOnInit(): void {

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
      lead_type: new FormControl(this.leadbank.lead_type
      ),

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
      team_name: new FormControl(this.leadbank.team_name, [
       
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),

      phone2: new FormControl(this.leadbank.phone2, [
        Validators.minLength(7),
        Validators.maxLength(15),
        Validators.pattern("^\\+?[0-9]+(?:[\\s-]?[0-9]+)*$")
      ]),
      phone1: new FormControl(this.leadbank.phone1, [
        Validators.minLength(7),
        Validators.maxLength(15),
        Validators.pattern("^\\+?[0-9]+(?:[\\s-]?[0-9]+)*$")
      ]),
      fax: new FormControl(this.leadbank.fax, [
      Validators.minLength(6),
        Validators.maxLength(10),
        Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")
      ]),

      phone: new FormControl(this.leadbank.phone, [
        Validators.required,]),
      mobile: new FormControl(''),
      value: new FormControl(this.leadbank.value, [
        Validators.required,
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
        Validators.maxLength(12),
        Validators.pattern('^[A-Za-z0-9_-]+$') // Allow letters, digits, underscore, and hyphen
      ]),
      
      
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
    var api2 = 'Mycalls/GetAssignedteamdropdown'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.assignedteamdropdown_list = result.assignedteamdropdown_list;
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
  get team_name() {
    return this.reactiveForm.get('team_name')!;
  }
  get lead_type() {
    return this.reactiveForm.get('lead_type')!;
  }
  get customer_type() {
    return this.reactiveForm.get('customer_type')!;
  }

  validate(){
    this.NgxSpinnerService.show()
    if (!this.reactiveForm.value.phone) {
      this.reactiveForm.value.phone = { e164Number: null }; // Or any default value you prefer
    }
    this.leadbank = this.reactiveForm.value;
    if (this.leadbank.active_flag != null && this.leadbank.leadbank_name != null  ) {
      this.isSubmitting = true;
      var api7 = 'Mycalls/Postmycallleadbank'
      this.service.post(api7, this.leadbank).subscribe((result: any) => {
        if (result.status == false) {
         
          this.ToastrService.warning(result.message);
        } else {
          if (this.lspage == 'Mycalls-schedule'){
            this.route.navigate(['/crm/CrmTrnTelemycampaignNew']);
          }
         else if (this.lspage == 'Mycalls-Upcoming'){
            this.route.navigate(['/crm/CrmTrnTelemycampaignNew']);
          }
         else if (this.lspage == 'Mycalls-allleads'){
            this.route.navigate(['/crm/CrmTrnTelemycampaignNew']);
          }
         else if (this.lspage == 'Mycalls-newlead'){
            this.route.navigate(['/crm/CrmTrnTelemycampaignNew']);
          }
         else if (this.lspage == 'Mycalls-pending'){
            this.route.navigate(['/crm/CrmTrnTelemycampaignNew']);
          }
         else if (this.lspage == 'Mycalls-followup'){
            this.route.navigate(['/crm/CrmTrnTelemycampaignNew']);
          }
         else if (this.lspage == 'Mycalls-prospect'){
            this.route.navigate(['/crm/CrmTrnTelemycampaignNew']);
          }
          else {
            this.route.navigate(['/crm/CrmTrnTelemycampaign']); 
          } 
         
          this.ToastrService.success(result.message);
        }
        this.NgxSpinnerService.hide()
        this.responsedata = result;
        
        // Re-enable the submit button after form submission is complete
        this.isSubmitting = false;
      });
    }
    else {
      this.NgxSpinnerService.hide()
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
  back(){
    debugger
    if (this.lspage == 'Mycalls-schedule'){
      this.route.navigate(['/crm/CrmTrnTelemycampaign']);
    }
   else if (this.lspage == 'Mycalls-Upcoming'){
      this.route.navigate(['/crm/CrmTrnUpcomingtelecaller']);
    }
   else if (this.lspage == 'Mycalls-allleads'){
      this.route.navigate(['/crm/CrmTrnTelemycampaignAll']);
    }
   else if (this.lspage == 'Mycalls-newlead'){
      this.route.navigate(['/crm/CrmTrnTelemycampaignNew']);
    }
   else if (this.lspage == 'Mycalls-pending'){
      this.route.navigate(['/crm/CrmTrnTelemycampaignPending']);
    }
   else if (this.lspage == 'Mycalls-followup'){
      this.route.navigate(['/crm/CrmTrnTelemycampaignFollowup']);
    }
   else if (this.lspage == 'Mycalls-prospect'){
      this.route.navigate(['/crm/CrmTrnTelemycampaignProspect']);
    }
    else{
      this.route.navigate(['/crm/CrmTrnTelemycampaign']); 
    }
  }
}

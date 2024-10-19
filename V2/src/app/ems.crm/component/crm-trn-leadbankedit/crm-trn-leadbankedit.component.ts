import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

interface ILeadbankedit {
  leadbank_gid: string;
  leadbankcontact_gid: string;
  company_website: string;
  phone2: string;
  phone1: string;
  fax: string;
  email: string;
  leadbank_address1: string;
  leadbank_address2: string;
  leadbank_city: string;
  leadbank_state: string;
  leadbank_pin: string;
  active_flag: string;
  addtocustomer: string;
  status: string;
  leadbank_region: string;
  region_name: string;
  source_name: string;
  country_name: string;
  leadbank_name: string;
  categoryindustry_name: string;
  designation: string;
  leadbankcontact_name: string;
  user_code: string;
  remarks: string;
  referred_by: string;
  mobile: string;
 customer_type: string;
  value: string;
  phone: string;
  wedding_day:any;
  birth_day:any;
  interests:string;
  lead_type:any;
}
@Component({
  selector: 'app-crm-trn-leadbankedit',
  templateUrl: './crm-trn-leadbankedit.component.html',
  styleUrls: ['./crm-trn-leadbankedit.component.scss']
})
export class CrmTrnLeadbankeditComponent implements OnInit {
  leadbankeditlist!: ILeadbankedit;
  leadbankedit_list11!: ILeadbankedit;
  leadbankedit_list2!: ILeadbankedit;
  reactiveForm!: FormGroup;
  entity_list: any[] = [];
  source_list: any[] = [];
  industryList: any[] = [];
  regionnamelist: any[] = [];
  designation_list: any[] = [];
  country_list: any[] = [];
  Email_Address: any;
  responsedata: any;
  selectedsource_name: any;
  selectedregion_name: any;
  selectedcategoryindustry_name: any;
  selectedcountry_name: any;
  selectedCustomerType: any;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  customertype_list: any[] = [];
  GetLeaddropdown_list: any[] = [];
  leadbank_gid: any;
  leadbankcontact_gid: any;
  leadbankedit_list: any;
  lspage: any;
  isSubmitting = false;

  leadtype_list: any[] = [];
  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {
    this.leadbankeditlist = {} as ILeadbankedit;
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-y',    
    };
    flatpickr('.date-picker', options);  
    const leadbank_gid = this.router.snapshot.paramMap.get('leadbank_gid');
    const lspage = this.router.snapshot.paramMap.get('lspage');
    // console.log(termsconditions_gid)
    this.leadbank_gid = leadbank_gid;
    this.lspage = lspage;
    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam1 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);

    this.lspage = deencryptedParam1;

    // console.log("Redirecting="+deencryptedParam);
    // console.log("Redirecting="+deencryptedParam1);

    this.GetleadbankeditSummary(deencryptedParam)

    this.reactiveForm = new FormGroup({
      leadbankcontact_name: new FormControl(this.leadbankeditlist.leadbankcontact_name, [
        Validators.required,
      ]),
      leadbank_name: new FormControl(this.leadbankeditlist.leadbank_name, [
        Validators.required,
      ]),
      customer_type: new FormControl(this.leadbankeditlist.customer_type, [
        Validators.required,
      ]),
      referred_by: new FormControl(null),
      wedding_day: new FormControl(this.leadbankeditlist.wedding_day, [
      ]),
      birth_day: new FormControl(this.leadbankeditlist.birth_day, [
        
      ]),
      interests: new FormControl(this.leadbankeditlist.interests, [
        
      ]),
      leadbank_gid: new FormControl(''),
      leadbankcontact_gid: new FormControl(''),
      leadbank_address2: new FormControl(''),
      leadbank_state: new FormControl(''),
      remarks: new FormControl(''),
      status: new FormControl('Y'),
      active_flag: new FormControl('Y'),
      //leadbank_pin: new FormControl(''),
      leadbank_city: new FormControl(''),
      addtocustomer: new FormControl('N'),
      region_name: new FormControl(null),
      source_name: new FormControl(null),
      lead_type: new FormControl(null),

      phone2: new FormControl(this.leadbankeditlist.phone2, [
        Validators.minLength(7),
        Validators.maxLength(15),
        Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")
      ]),
      phone1: new FormControl(this.leadbankeditlist.phone1, [
        Validators.minLength(7),
        Validators.maxLength(15),
        Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")
      ]),
      fax: new FormControl(this.leadbankeditlist.fax, [
        Validators.minLength(6),
        Validators.maxLength(10),
        Validators.pattern("^([+x()])?[0-9]+(?:[+()]?[0-9]+)*$")
      ]),


      phone: new FormControl(this.leadbankeditlist.phone, [
        Validators.required,]),
      mobile: new FormControl(''),
      value: new FormControl(this.leadbankeditlist.value, [
        Validators.required,
      ]),


      categoryindustry_name: new FormControl(this.leadbankeditlist.categoryindustry_name, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      designation: new FormControl(this.leadbankeditlist.designation, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      country_name: new FormControl(this.leadbankeditlist.country_name, [
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      leadbank_address1: new FormControl(this.leadbankeditlist.leadbank_address1, [
        Validators.maxLength(1000),
      ]),
      email: new FormControl(this.leadbankeditlist.email, [
        Validators.required,
        Validators.maxLength(320),
        Validators.email  // Angular's built-in email validator
      ]),
      company_website: new FormControl(this.leadbankeditlist.company_website, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
        Validators.pattern('^(http(s)?:\/\/)?(www\.)?([a-zA-Z0-9-]+(\.[a-zA-Z]{2,})+)')
      ]),
      leadbank_pin: new FormControl(this.leadbankeditlist.leadbank_pin, [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(12),
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
  get categoryindustry_name() {
    return this.reactiveForm.get('categoryindustry_name')!;
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
  get wedding_day() {
    return this.reactiveForm.get('wedding_day')!;
  }
  get birth_day(){
    return this.reactiveForm.get('birth_day')!;
  }
  get interests() {
    return this.reactiveForm.get('interests')!;
  }
  get lead_type() {
    return this.reactiveForm.get('lead_type')!;
  }

  GetleadbankeditSummary(leadbank_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'Leadbank/GetleadbankeditSummary'
    let param = {
      leadbank_gid: leadbank_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      // this.responsedata=result;
      this.leadbankedit_list = result.leadbankedit_list;
      // console.log(this.leadbankedit_list)
      // console.log(this.leadbankedit_list[0].branch_gid)
      this.reactiveForm.get("customer_type")?.setValue(this.leadbankedit_list[0].customer_type);
      this.reactiveForm.get("phone")?.setValue(this.leadbankedit_list[0].mobile);
      this.reactiveForm.get("region_name")?.setValue(
        this.leadbankedit_list[0]?.region_gid ? this.leadbankedit_list[0].region_gid : null
      );
      
      this.reactiveForm.get("source_name")?.setValue(
        this.leadbankedit_list[0]?.source_gid ? this.leadbankedit_list[0].source_gid : null
      );
      
      this.reactiveForm.get("categoryindustry_name")?.setValue(this.leadbankedit_list[0].categoryindustry_name);
      this.selectedregion_name = this.leadbankedit_list[0].region_gid;
      this.selectedsource_name = this.leadbankedit_list[0].source_gid;
      this.selectedcategoryindustry_name = this.leadbankedit_list[0].categoryindustry_gid;
      this.selectedcountry_name = this.leadbankedit_list[0].country_gid;
      this.selectedCustomerType = this.leadbankedit_list[0].customertype_gid;
      this.reactiveForm.get("leadbank_name")?.setValue(this.leadbankedit_list[0].leadbank_name);
      this.reactiveForm.get("active_flag")?.setValue(this.leadbankedit_list[0].active_flag);
      this.reactiveForm.get("leadbankcontact_name")?.setValue(this.leadbankedit_list[0].leadbankcontact_name);
      this.reactiveForm.get("leadbank_address1")?.setValue(this.leadbankedit_list[0].leadbank_address1);
      this.reactiveForm.get("leadbank_address2")?.setValue(this.leadbankedit_list[0].leadbank_address2);
      this.reactiveForm.get("leadbank_city")?.setValue(this.leadbankedit_list[0].leadbank_city);
      this.reactiveForm.get("leadbank_state")?.setValue(this.leadbankedit_list[0].leadbank_state);
      this.reactiveForm.get("email")?.setValue(this.leadbankedit_list[0].email);
      this.reactiveForm.get("leadbank_pin")?.setValue(this.leadbankedit_list[0].leadbank_pin);
      this.reactiveForm.get("designation")?.setValue(this.leadbankedit_list[0].designation);
      this.reactiveForm.get("phone1")?.setValue(this.leadbankedit_list[0].phone1);
      this.reactiveForm.get("phone2")?.setValue(this.leadbankedit_list[0].phone2);
      this.reactiveForm.get("fax")?.setValue(this.leadbankedit_list[0].fax);
      this.reactiveForm.get("remarks")?.setValue(this.leadbankedit_list[0].remarks);
      this.reactiveForm.get("status")?.setValue(this.leadbankedit_list[0].status);
      this.reactiveForm.get("company_website")?.setValue(this.leadbankedit_list[0].company_website);
      this.reactiveForm.get("leadbank_gid")?.setValue(this.leadbankedit_list[0].leadbank_gid);
      this.reactiveForm.get("leadbankcontact_gid")?.setValue(this.leadbankedit_list[0].leadbankcontact_gid);
      this.reactiveForm.get("wedding_day")?.setValue(this.leadbankedit_list[0].wedding_day);
      this.reactiveForm.get("birth_day")?.setValue(this.leadbankedit_list[0].birth_day);
      this.reactiveForm.get("interests")?.setValue(this.leadbankedit_list[0].interests);
      this.reactiveForm.get("country_name")?.setValue(this.leadbankedit_list[0].country_gid);
      this.reactiveForm.get("referred_by")?.setValue(
        this.leadbankedit_list[0]?.referred_by ? this.leadbankedit_list[0].referred_by : null
      );
      this.reactiveForm.get("lead_type")?.setValue(
        this.leadbankedit_list[0]?.lead_type ? this.leadbankedit_list[0].lead_type : null
      );
            console.log('emklwmedw',this.leadbankedit_list)
      this.NgxSpinnerService.hide();

      console.log('elkmdew',this.leadbankedit_list);

    });
  }

  public validate(): void {
    //console.log(this.reactiveForm.value);
    if (!this.reactiveForm.value.phone) {
      this.reactiveForm.value.phone = { e164Number: null }; // Or any default value you prefer
    }
    this.leadbankeditlist = this.reactiveForm.value;
    console.log('iuhiu',this.leadbankeditlist)
    {
      this.isSubmitting = true;

      if (this.leadbankeditlist.leadbank_name != null ) {
        this.NgxSpinnerService.show();

        var api7 = 'Leadbank/Updateleadbank';
        this.service.post(api7, this.leadbankeditlist).subscribe((result: any) => {
          this.NgxSpinnerService.hide();

          if (result.status == false) {
            this.route.navigate(['/crm/CrmTrnLeadbanksummary']); 
            this.ToastrService.warning(result.message);
          }
          else {
            this.route.navigate(['/crm/CrmTrnLeadbanksummary']); 
            this.ToastrService.success(result.message);

          }
          this.responsedata = result;
        });
        this.isSubmitting = true;
      }
      else {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
      }
      return;
    }
  }

  back() {
    //console.log("Back button:" + this.lspage);

    if (this.lspage == 'Leadbank') {
      this.route.navigate(['/crm/CrmTrnLeadbanksummary']);
    } else if (this.lspage == 'Registerlead') {
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
    else if (this.lspage == 'LeadBankdistributor') {
      this.route.navigate(['/crm/CrmTrnLeadbanksummary']);
    }
    else if (this.lspage == 'LeadBankcorporate') {
      this.route.navigate(['/crm/CrmTrnCorporateLeadBank']);
    }
    else if (this.lspage == 'LeadBankretailer') {
      this.route.navigate(['/crm/CrmTrnRetailerLeadBank']);
    }
  }

}

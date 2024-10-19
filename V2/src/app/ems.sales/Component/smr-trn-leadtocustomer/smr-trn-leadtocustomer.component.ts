import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgSelectModule } from '@ng-select/ng-select';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import {CountryISO,SearchCountryField} from "ngx-intl-tel-input";
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';

interface ICustomer {
  taxsegment_name: any;
  country_code: any;
  area_code: any;
  mobiles: any;
  user_gid :string;
  countryname: string;
  taxname: string;
  customer_state: string;
  fax: string;
  fax_area_code: string;
  fax_country_code: string;
  customer_city: string;
  mobile: string;
  customer_id: string;
  email: string;
  billemail:string;
  postal_code: string;
  address1: string;
  address2: string;
  leadbank_name: string;
  tax_no:string;
  leadbankcontact_name: string;
  gst_number: string;
  designation: string;
  region_name: string;
  sales_person:string;
  currency:string;
  company_website :string;
  currencyexchange_gid : String;
  currency_code:string;
  customer_gid : string;
  customer_code : string;
  credit_days : string;
  customer_type:string;
  pricesegment_name:string;
  
}

@Component({
  selector: 'app-smr-trn-leadtocustomer',
  templateUrl: './smr-trn-leadtocustomer.component.html',
  styleUrls: ['./smr-trn-leadtocustomer.component.scss']
})
export class SmrTrnLeadtocustomerComponent {


  file!: File;
  customer!: ICustomer;
  reactiveForm!: FormGroup;
  country_list: any[] = [];
  currency_list: any[] = [];
  salesperson_list: any[] = [];
  tax_list: any[] = [];
  region_list: any[] = [];
  pricesegment_list : any[] = [];
  Email_Address: any;
  responsedata: any;
  customer_id1:any;
  customertype_list:any[] = [];
  mdlType:any;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  taxsegment_list: any;
  leadbank_gid: any;
  lead_list : any[]=[];

  constructor(private renderer: Renderer2,private router : ActivatedRoute, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, public NgxSpinnerService:NgxSpinnerService ) {
    this.customer = {} as ICustomer;
  }
  ngOnInit(): void {

    const leadbank_gid = this.router.snapshot.paramMap.get('leadbank_gid');
    this.leadbank_gid = leadbank_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    this.leadbank_gid = deencryptedParam;
    console.log(deencryptedParam)
    this.getlead(this.leadbank_gid)
    
    this.reactiveForm = new FormGroup({

      
      customer_gid: new FormControl(''),
      credit_days: new FormControl(''),

      area_code: new FormControl(this.customer.area_code, []),

      country_code: new FormControl(this.customer.country_code, []),
      
      fax: new FormControl(this.customer.fax, [
       

      ]),
      customer_city: new FormControl(this.customer.customer_city, []),
      address1: new FormControl(this.customer.address1, [
        Validators.required,
      ]),
      address2: new FormControl(this.customer.address2, []),
      customer_id: new FormControl(''),
      tax_no: new FormControl(''),
      leadbank_name: new FormControl(this.customer.leadbank_name, [
      ]),
      mobiles: new FormControl(this.customer.mobiles, []),
      
      region_name: new FormControl(this.customer.region_name, []),
      sales_person: new FormControl(this.customer.sales_person, [
        Validators.required,

      ]),
      currency: new FormControl(this.customer.currency, [
        Validators.required,

      ]),
      taxsegment_name: new FormControl(this.customer.taxsegment_name, [
        Validators.required,

      ]),
     pricesegment_name: new FormControl(this.customer.pricesegment_name, []),

      email: new FormControl(''),
      billemail: new FormControl(''),
      postal_code: new FormControl(''),
      leadbankcontact_name: new FormControl(''),
      customer_type: new FormControl(this.customer.customer_type, []),
      countryname: new FormControl(this.customer.countryname, [
        Validators.required,
      ]),
    });

    //country drop down//
    var url = 'SmrTrnCustomerSummary/Getcountry'
    this.service.get(url).subscribe((result: any) => {
      this.country_list = result.Getcountry;
    });
    var url = 'SmrTrnCustomerSummary/Getsalespersondtl'
    this.service.get(url).subscribe((result: any) => {
      this.salesperson_list = result.Getsalespersondtl_list;
    });
    var url = 'SmrTrnCustomerSummary/getcurrencydtl'
    this.service.get(url).subscribe((result: any) => {
      this.currency_list = result.getcurrencydtl_list;
    });
    // price segment dropdown
    var url = 'SmrTrnCustomerSummary/Getpricesegment'
    this.service.get(url).subscribe((result: any) => {
      this.pricesegment_list = result.pricesegment_list;
    });

    var url = 'SmrTrnCustomerSummary/GetCustomerTypeSummary'
    this.service.get(url).subscribe((result: any) => {
      this.customertype_list = result.customertype_list1;
    });
    //regionname dropdown//

    var url = 'SmrTrnCustomerSummary/Getregion'
    this.service.get(url).subscribe((result: any) => {
      this.region_list = result.Getregion;
    });

    var url = 'SmrMstTaxSegment/GetTaxSegmentSummary'
    this.service.get(url).subscribe((result: any) => {
      this.taxsegment_list = result.TaxSegmentSummary_list;
    });
  }
  

  onChange2(event: any) {
    this.file = event.target.files[0];

  }

  get countryname() {
    return this.reactiveForm.get('countryname')!;
  }


  get customer_type() {
    return this.reactiveForm.get('customer_type')!;
  }

  get country_code() {
    return this.reactiveForm.get('country_code')!;
  }
 

  get address1() {
    return this.reactiveForm.get('address1')!;
  }



  get customer_name() {
    return this.reactiveForm.get('customer_name')!;
  }
  get tax_no() {
    return this.reactiveForm.get('tax_no')!;
  }
  get customercontact_name() {
    return this.reactiveForm.get('customercontact_name')!;
  }
 
  get email() {
    return this.reactiveForm.get('email')!;
  }
  get billemail() {
    return this.reactiveForm.get('billemail')!;
  }
  get region_name() {
    return this.reactiveForm.get('region_name')!;
  }
  get sales_person() {
    return this.reactiveForm.get('sales_person')!;
  }
  get currency() {
    return this.reactiveForm.get('currency')!;
  }
  get taxsegment_name() {
    return this.reactiveForm.get('taxsegment_name')!;
  }
  get pricesegment_name() {
    return this.reactiveForm.get('pricesegment_name')!;
  }

  getlead(deencryptedParam : any){

    var url='/SmrTrnCustomerEnquiry/GetLeadSummary'
    let params = {
      leadbank_gid :deencryptedParam
    }
    this.service.getparams(url,params).subscribe((result : any)=>{
      this.responsedata = result;
      this.lead_list = result.cusenquiry_list;
      this.reactiveForm.get("customer_id")?.setValue(this.lead_list[0].enquiry_referencenumber)
      this.reactiveForm.get("leadbank_name")?.setValue(this.lead_list[0].leadbank_name)
      this.reactiveForm.get("leadbankcontact_name")?.setValue(this.lead_list[0].leadbankcontact_name)
      this.reactiveForm.get("mobiles")?.setValue(this.lead_list[0].contact_number)
      this.reactiveForm.get("email")?.setValue(this.lead_list[0].contact_email)
      this.reactiveForm.get("address1")?.setValue(this.lead_list[0].address1)
      this.reactiveForm.get("address2")?.setValue(this.lead_list[0].address2)
      this.reactiveForm.get("customer_city")?.setValue(this.lead_list[0].leadbank_city)
      this.reactiveForm.get("postal_code")?.setValue(this.lead_list[0].leadbank_pin)
      this.reactiveForm.get("customer_gid")?.setValue(this.lead_list[0].customer_gid);
    })
  }

 validate() {
  debugger
  
    this.customer = this.reactiveForm.value;

      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        debugger
        formData.append("customer_state", this.customer.customer_state);
        formData.append("customer_city", this.customer.customer_city);
        formData.append("address1", this.customer.address1);
        formData.append("mobiles", this.customer.mobiles);
        formData.append("customer_id", this.customer_id1 );
        formData.append("customer_code", this.customer.customer_code );
        formData.append("leadbank_name", this.customer.leadbank_name);
        formData.append("tax_no", this.customer.tax_no);
        formData.append("leadbankcontact_name", this.customer.leadbankcontact_name);
        formData.append("email", this.customer.email);
        formData.append("billemail", this.customer.billemail);
        formData.append("postal_code", this.customer.postal_code);
        formData.append("address2", this.customer.address2);
        formData.append("countryname", this.customer.countryname);
        formData.append("taxname", this.customer.taxname);
        formData.append("region_name", this.customer.region_name);
        formData.append("sales_person", this.customer.sales_person);
        formData.append("currency", this.customer.currency);
        formData.append("area_code", this.customer.area_code);
        formData.append("credit_days", this.customer.credit_days);
        formData.append("country_code", this.customer.country_code);
        formData.append("fax", this.customer.fax);
        formData.append("company_website", this.customer.company_website);
        formData.append("designation", this.customer.designation);
        formData.append("taxsegment_name", this.customer.taxsegment_name);
        formData.append("pricesegment_name", this.customer.pricesegment_name);
        formData.append("customer_gid", this.customer.customer_gid);
    
        var api = 'SmrTrnCustomerEnquiry/Postlead'
        this.NgxSpinnerService.show();
        this.service.postfile(api, formData).subscribe((result: any) => {
          debugger
          this.responsedata = result;
          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
          }
          else {
            this.NgxSpinnerService.hide();
            this.route.navigate(['/smr/SmrTrnCustomerenquirySummary']);
            this.ToastrService.success(result.message)
          }
        });

      }
      else {
        var api7 = 'SmrTrnCustomerEnquiry/Postlead'
        this.NgxSpinnerService.show();
        this.service.post(api7, this.customer).subscribe((result: any) => {

          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
          }
          else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message)
            this.route.navigate(["/smr/SmrTrnCustomerenquirySummary"])
          }
          this.responsedata = result;
        });
      }
    
    
    }
   
  onclose(){
    this.route.navigate(['/smr/SmrTrnCustomerenquirySummary/'])
  }
}


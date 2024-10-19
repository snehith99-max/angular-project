import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgSelectModule } from '@ng-select/ng-select';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import {CountryISO,SearchCountryField} from "ngx-intl-tel-input";
import { NgxSpinnerService } from 'ngx-spinner';

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
  customer_name: string;
  tax_no:string;
  customercontact_name: string;
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
  selector: 'app-smr-trn-customeradd',
  templateUrl: './smr-trn-customeradd.component.html',
  styleUrls: ['./smr-trn-customeradd.component.scss']
})
export class SmrTrnCustomeraddComponent {
  
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

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, public NgxSpinnerService:NgxSpinnerService ) {
    this.customer = {} as ICustomer;
  }
  ngOnInit(): void {
    
    this.reactiveForm = new FormGroup({

      
      customer_gid: new FormControl(''),
      credit_days: new FormControl(''),

      area_code: new FormControl(this.customer.area_code, [
        

      ]),

      country_code: new FormControl(this.customer.country_code, [
        

      ]),
      
      fax: new FormControl(this.customer.fax, [
       

      ]),
      customer_city: new FormControl(this.customer.customer_city, [
        

      ]),
      address1: new FormControl(this.customer.address1, [
        Validators.required,
      ]),
      customer_state: new FormControl(this.customer.customer_state),
      address2: new FormControl(this.customer.address2, [
        
      ]),
      customer_id: new FormControl(''),
      tax_no: new FormControl(''),
      customer_name: new FormControl(this.customer.customer_name, [
       

      ]),
      mobiles: new FormControl(this.customer.mobiles, [
        

      ]),
      
      region_name: new FormControl(this.customer.region_name, [
        

      ]),
      sales_person: new FormControl(this.customer.sales_person, [
        Validators.required,

      ]),
      currency: new FormControl(this.customer.currency, [
        Validators.required,

      ]),
      taxsegment_name: new FormControl(this.customer.taxsegment_name, [
        Validators.required,

      ]),
     pricesegment_name: new FormControl(this.customer.pricesegment_name, [
        

      ]),


      
      
      

      email: new FormControl(''),
      billemail: new FormControl(''),
      postal_code: new FormControl(''),
      customercontact_name: new FormControl(''),

      customer_type: new FormControl(this.customer.customer_type, [
       

      ]),

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

  // get customer_state() {
  //   return this.reactiveForm.get('customer_state')!;
  // }

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
  // get mobiles(){
  //   return this.reactiveForm.get('mobiles')!;
  // }









 validate() {
  debugger
  if(this.reactiveForm.value.customer_name != null && this.reactiveForm.value.customercontact_name != null &&
    this.reactiveForm.value.email != null && this.reactiveForm.value.sales_person != null && this.reactiveForm.value.currency != null && this.reactiveForm.value.taxsegment_name != null &&
    this.reactiveForm.value.address1 != null && this.reactiveForm.value.countryname != null ){
    this.customer = this.reactiveForm.value;

        var api7 = 'SmrTrnCustomerSummary/Postcustomer'
        this.NgxSpinnerService.show();
        this.service.post(api7, this.customer).subscribe((result: any) => {

          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
          }
          else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message)
            this.route.navigate(["/smr/SmrTrnCustomerSummary"])
          }
        });
      }
    else
    {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("Kindly fill all mandatory fields")
    }
    }
   
  onclose(){
    this.route.navigate(['/smr/SmrTrnCustomerSummary/'])
  }
}

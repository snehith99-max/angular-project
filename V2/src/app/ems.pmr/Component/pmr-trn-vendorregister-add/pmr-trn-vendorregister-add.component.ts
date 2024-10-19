import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgSelectModule } from '@ng-select/ng-select';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import {CountryISO,SearchCountryField,} from "ngx-intl-tel-input";
import { NgxSpinnerService } from 'ngx-spinner';
interface IVendor {
  taxsegment_name: any;
  mobile: any;
  countryname:string;
  currencyname:string;
  taxname:string;
  state_name: string;
  fax_name: string;
  city: string;
  tax_number: string;

  address: string;
  contact_telephonenumber: string;
  vendor_code: string;
  email_address: string;
  billingemail_address: string;

  postal_code: string;
  address2: string;
  vendor_companyname: string;
  contactperson_name: string;
  averageleadtime: string;

}

@Component({
  selector: 'app-pmr-trn-vendorregister-add',
  templateUrl: '/pmr-trn-vendorregister-add.component.html',
})

export class PmrTrnVendorregisterAddComponent {

  file!:File;
  vendor!: IVendor;
  reactiveForm!: FormGroup;
  country_list: any[] = [];
  currency_list: any[] = [];
  region_list:any[]=[];
  tax_list: any[] = [];
  Email_Address: any;
  billingEmail_Address: any;

  responsedata: any;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  taxsegment_list: any;
  taxsegment_gid: any;

  constructor(private renderer: Renderer2, private el: ElementRef,public service :SocketService,private NgxSpinnerService:NgxSpinnerService,private ToastrService: ToastrService,private route:Router ) {
    this.vendor = {} as IVendor;
  }

  
  ngOnInit(): void {
   
    this.reactiveForm = new FormGroup({
  
       
      // state_name: new FormControl(this.vendor.state_name, [
      //   Validators.required,
      
      // ]),
      // fax_name: new FormControl(this.vendor.fax_name, [
      //   Validators.required,
        
      // ]),
      city:new FormControl(''),
      tax_number: new FormControl(''),
      tax_segment:new FormControl(''),
      region:  new FormControl(''),
      address: new FormControl(this.vendor.address, [
        Validators.required,
      ]),
      vendor_code: new FormControl(this.vendor.vendor_code),  
      vendor_companyname: new FormControl(this.vendor.vendor_companyname, [
        Validators.required,

      ]),
      mobile: new FormControl(''),
      taxsegment_name: new FormControl(this.vendor.taxsegment_name, [
        Validators.required,

      ]),
      email_address: new FormControl(''),
      averageleadtime: new FormControl(''),

      billingemail_address: new FormControl(''),

      postal_code: new FormControl(''),
      address2: new FormControl(''),
      contactperson_name :new FormControl(''),
          countryname : new FormControl(this.vendor.countryname, [
            Validators.required,        
            ]),
            currencyname : new FormControl(this.vendor.currencyname, [
              Validators.required,    
              ]),
              paymentterms:new FormControl(''),
              creditdays:new FormControl('')
             
 }

    );


var api5='PmrMstVendorRegister/Getcountry'
this.service.get(api5).subscribe((result:any)=>{
  this.country_list = result.Getcountry;
});

var api4='PmrMstVendorRegister/Getcurency'
this.service.get(api4).subscribe((result:any)=>{
  this.currency_list = result.Getcurency;
});

var api3='PmrMstVendorRegister/Gettax'
this.service.get(api3).subscribe((result:any)=>{
  this.tax_list = result.Gettax;
});
var api3='PmrMstVendorRegister/GetRegion'
this.service.get(api3).subscribe((result:any)=>{
  this.region_list = result.VendorRegion_list;
});
var url = 'PmrMstVendorRegister/GetTaxSegmentSummary'
this.service.get(url).subscribe((result: any) => {
  this.taxsegment_list = result.TaxSegmentSummary_list;
});
  
  }

  onChange2(event:any) {
    this.file =event.target.files[0];

    }
  
    get countryname() {
      return this.reactiveForm.get('countryname')!;
    }
    get currencyname() {
      return this.reactiveForm.get('currencyname')!;
    }
    // get taxname() {
    //   return this.reactiveForm.get('taxname')!;
    // }
    get country() {
      return this.reactiveForm.get('country')!;
    }  
   
    // get state_name() {
    //   return this.reactiveForm.get('state_name')!;
    // }
    // get fax_name() {
    //   return this.reactiveForm.get('fax_name')!;
    // }
    get city() {
      return this.reactiveForm.get('city')!;
    }
    get region() {
      return this.reactiveForm.get('region')!;
    }
    get address() {
      return this.reactiveForm.get('address')!;
    }
    get mobile() {
      return this.reactiveForm.get('mobile')!;
    }
    get tax_number() {
      return this.reactiveForm.get('tax_number')!;
    }
    // get vendor_code() {
    //   return this.reactiveForm.get('vendor_code')!;
    // }
    get vendor_companyname() {
      return this.reactiveForm.get('vendor_companyname')!;
    }
    get contactperson_name() {
      return this.reactiveForm.get('contactperson_name')!;
    }
    get postal() {
      return this.reactiveForm.get('postal_code')!;
    }
    get email() {
      return this.reactiveForm.get('email_address')!;
    }
    get billingemail() {
      return this.reactiveForm.get('billingemail_address')!;
    }

  get taxsegment_name() {
    return this.reactiveForm.get('taxsegment_name')!;
  }
  // get paymentterms() {
  //   return this.reactiveForm.get('paymentterms')!;
  // }
  get averageleadtime() {
    return this.reactiveForm.get('averageleadtime')!;
  }
  
 
    
    public validate(): void {
      debugger
      if (this.reactiveForm.invalid) {
        // Mark all fields as touched to trigger validation messages
        Object.values(this.reactiveForm.controls).forEach(control => {
          control.markAsTouched();
        });

        return;
      }
      else{
        this.vendor = this.reactiveForm.value;
        
          let formData = new FormData();
          if(this.file !=null &&  this.file != undefined){

          
        //  formData.append("state_name", this.vendor.state_name);
        //  formData.append("fax_name", this.vendor.fax_name);
         formData.append("city", this.vendor.city);
         formData.append("address", this.vendor.address);
         formData.append("contact_telephonenumber", this.vendor.mobile.e164Number);
         formData.append("vendor_code", this.vendor.vendor_code);
         formData.append("vendor_companyname", this.vendor.vendor_companyname);
         formData.append("contactperson_name", this.vendor.contactperson_name);
         formData.append("email_address", this.vendor.email_address);
         formData.append("postal_code", this.vendor.postal_code);
         formData.append("address2", this.vendor.address2);
         formData.append("countryname", this.vendor.countryname);
         formData.append("currencyname", this.vendor.currencyname);
        //  formData.append("taxname", this.vendor.taxname);
         formData.append("taxsegment_name", this.vendor.taxsegment_name);
          var api='PmrMstVendorRegister/PostVendorRegister'
          this.NgxSpinnerService.show()
            this.service.postfile(api,formData).subscribe((result:any) => {
              this.responsedata=result;
              if(result.status ==false){
                this.ToastrService.warning(result.message)
              }
              else{
                this.route.navigate(['/pmr/PmrMstVendorregister']);
                this.ToastrService.success(result.message)
                this.NgxSpinnerService.hide()
                
              }
            });
        
        }
        else{
          var api7='PmrMstVendorRegister/PostVendorRegister'
          this.NgxSpinnerService.show()
      
            this.service.post(api7,this.reactiveForm.value).subscribe((result:any) => {

              if(result.status ==false){
                this.ToastrService.warning(result.message)
                this.NgxSpinnerService.hide()
              }
              else{
                this.route.navigate(['/pmr/PmrMstVendorregister']);
                this.ToastrService.success(result.message)
                this.NgxSpinnerService.hide()
              }
              this.responsedata=result;
            });
        }
      }
      

    }
}

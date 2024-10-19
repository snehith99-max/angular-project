import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, MinLengthValidator, Validators } from '@angular/forms';
import { Router } from '@angular/router'; 
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES , enc} from 'crypto-js';
import {CountryISO,SearchCountryField,} from "ngx-intl-tel-input";
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
interface IVendor {
  taxsegment_gid: string | Blob;
  taxsegment_name: string | Blob;
  country_name:string;
  currency_code:string;
  vendor_gid:string;
  tax_name:string;
  state: string;
  fax: string;
  city: string;
  address1: string;
  contact_telephonenumber: string;
  vendor_code: string;
  email_id: string;
  postal_code: string;
  address2: string;
  vendor_companyname: string;
  contactperson_name: string;
  address_gid: string;
}

@Component({
  selector: 'app-pmr-trn-vendorregister-edit',
  templateUrl: './pmr-trn-vendorregister-edit.component.html',
})
export class PmrTrnVendorregisterEditComponent {
  file!:File;
  vendor!: IVendor;
  reactiveForm!: FormGroup;
  country_list: any[] = [];
  currency_list: any[] = [];
  tax_list: any[] = [];
  region_list:any[]=[];
  
  responsedata: any;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  selectedVendorcode:any;
  vendor_gid:any;
  vendorregisteredit_list:any;
  taxsegment_list: any;
  MdlTaxsegment: any;
  vendorgid: any;
  constructor(private renderer: Renderer2, private el: ElementRef,public service :SocketService,private ToastrService: ToastrService,private NgxSpinnerService:NgxSpinnerService,private route:Router,private router: ActivatedRoute ) {
    this.vendor = {} as IVendor;
  }

  
  ngOnInit(): void {
    debugger
    const vendorregister_gid = this.router.snapshot.paramMap.get('vendor_gid');
    // console.log(termsconditions_gid)
    this.vendor_gid = vendorregister_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.vendor_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetVendorRegisterDetail(deencryptedParam)
   this.vendorgid=deencryptedParam
    this.reactiveForm = new FormGroup({
  
       
      state: new FormControl(''),
      fax: new FormControl(''),
      city: new FormControl(''),
      email_address:new FormControl(''),
      billingemail_address:new FormControl(''),
      tax_number:new FormControl(''),
      taxsegment_name:new FormControl('',Validators.required),
      averageleadtime:new FormControl(''),
      address:new FormControl(''),
      vendor_gid:new FormControl(''),
      address_gid:new FormControl(''),
      address1: new FormControl(this.vendor.address1, [
        Validators.required,
      ]),
      address2:new FormControl(''),
      vendor_code: new FormControl(this.vendor.vendor_code, [
        Validators.required,
      ]),
      vendor_companyname: new FormControl(this.vendor.vendor_companyname, [
        Validators.required,

      ]),
      contact_telephonenumber: new FormControl(this.vendor.contact_telephonenumber, [
        Validators.required,
        
      ]),
      email_id: new FormControl(this.vendor.email_id, [
        Validators.required,
        
      ]),
      // email_address: new FormControl(this.vendor.email_address, [
      //   Validators.required,
        
      // ]),
      postal_code:new FormControl(''),
      country_name:new FormControl(''),
      currencyname:new FormControl(''),
      creditdays:new FormControl(''),
      contactperson_name: new FormControl(this.vendor.contactperson_name, [
        Validators.required,
        
      ]),   
      region:new FormControl(''),  
      paymentterms:new FormControl(''), 
     taxsegment_gid: new FormControl(''),
      vendorregister_gid: new FormControl(this.vendor.vendor_gid),
      
            currency_code : new FormControl(this.vendor.currency_code, [
              Validators.required,    
              ]),
              tax_name : new FormControl(''),
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
var api3='PmrMstVendorRegister/GetRegion'
this.service.get(api3).subscribe((result:any)=>{
  this.region_list = result.VendorRegion_list;
});
var api3='PmrMstVendorRegister/Gettax'
this.service.get(api3).subscribe((result:any)=>{
  this.tax_list = result.Gettax;
});
var url = 'PmrMstVendorRegister/GetTaxSegmentSummary'
this.service.get(url).subscribe((result: any) => {
  this.taxsegment_list = result.TaxSegmentSummary_list;
});
  }

  GetVendorRegisterDetail(vendor_gid: any) {
  debugger
  this.NgxSpinnerService.show();
  var url1='PmrMstVendorRegister/GetVendorRegisterDetail'
  let param = {
    vendor_gid : vendor_gid 
  }
  this.service.getparams(url1, param).subscribe((result: any) => {
    // this.responsedata=result;
    this.vendorregisteredit_list = result.editvendorregistersummary_list;
    console.log(this.vendorregisteredit_list)
    // console.log(this.vendorregisteredit_list[0].vendorregister_gid)
    // this.selectedVendorcode = this.vendorregisteredit_list[0].vendorregister_gid;
    this.reactiveForm.get("vendor_gid")?.setValue(this.vendorregisteredit_list[0].vendorregister_gid);
    this.reactiveForm.get("state")?.setValue(this.vendorregisteredit_list[0].state);
    this.reactiveForm.get("fax")?.setValue(this.vendorregisteredit_list[0].fax);
    this.reactiveForm.get("city")?.setValue(this.vendorregisteredit_list[0].city);
    this.reactiveForm.get("creditdays")?.setValue(this.vendorregisteredit_list[0].credit_days);
    this.reactiveForm.get("billingemail_address")?.setValue(this.vendorregisteredit_list[0].billing_email);
    this.reactiveForm.get("tax_number")?.setValue(this.vendorregisteredit_list[0].tax_number);
    this.reactiveForm.get("taxsegment_name")?.setValue(this.vendorregisteredit_list[0].taxsegment_name);
    this.reactiveForm.get("averageleadtime")?.setValue(this.vendorregisteredit_list[0].average_leadtime);
    this.reactiveForm.get("country_name")?.setValue(this.vendorregisteredit_list[0].country_name);
    this.reactiveForm.get("region")?.setValue(this.vendorregisteredit_list[0].region_gid);
    this.reactiveForm.get("currencyname")?.setValue(this.vendorregisteredit_list[0].currency_code);
    this.reactiveForm.get("paymentterms")?.setValue(this.vendorregisteredit_list[0].payment_terms);

    this.reactiveForm.get("email_address")?.setValue(this.vendorregisteredit_list[0].email_id);
    this.reactiveForm.get("address")?.setValue(this.vendorregisteredit_list[0].address1);
    this.reactiveForm.get("address2")?.setValue(this.vendorregisteredit_list[0].address2);
    this.reactiveForm.get("contact_telephonenumber")?.setValue(this.vendorregisteredit_list[0].contact_telephonenumber);
    this.reactiveForm.get("vendor_code")?.setValue(this.vendorregisteredit_list[0].vendor_code);
    this.reactiveForm.get("vendor_companyname")?.setValue(this.vendorregisteredit_list[0].vendor_companyname);
    this.reactiveForm.get("contactperson_name")?.setValue(this.vendorregisteredit_list[0].contactperson_name);
    this.reactiveForm.get("email_id")?.setValue(this.vendorregisteredit_list[0].email_id);
    this.reactiveForm.get("postal_code")?.setValue(this.vendorregisteredit_list[0].postal_code);
    this.reactiveForm.get("address2")?.setValue(this.vendorregisteredit_list[0].address2);
    this.reactiveForm.get("address_gid")?.setValue(this.vendorregisteredit_list[0].address_gid);
    this.reactiveForm.get("country_name")?.setValue(this.vendorregisteredit_list[0].country_name);
    this.reactiveForm.get("currency_code")?.setValue(this.vendorregisteredit_list[0].currency_code);
    this.reactiveForm.get("tax_name")?.setValue(this.vendorregisteredit_list[0].tax_name);
    this.MdlTaxsegment = this.vendorregisteredit_list[0].taxsegment_name;
    this.reactiveForm.get("taxsegment_name")?.setValue(this.vendorregisteredit_list[0].taxsegment_name);
    this.NgxSpinnerService.hide();
  });
}
  onChange2(event:any) {
    this.file =event.target.files[0];

    }
    get currencyname() {
      return this.reactiveForm.get('currencyname')!;
    }
    get country_name() {
      return this.reactiveForm.get('country_name')!;
    }
    get currency_code() {
      return this.reactiveForm.get('currency_code')!;
    }
    get tax_name() {
      return this.reactiveForm.get('tax_name')!;
    }
    get country() {
      return this.reactiveForm.get('country')!;
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
   
   
    get state() {
      return this.reactiveForm.get('state')!;
    }
    get fax() {
      return this.reactiveForm.get('fax')!;
    }
    get city() {
      return this.reactiveForm.get('city')!;
    }
    get address1() {
      return this.reactiveForm.get('address1')!;
    }
    get contact_telephonenumber() {
      return this.reactiveForm.get('contact_telephonenumber')!;
    }
    get vendor_code() {
      return this.reactiveForm.get('vendor_code')!;
    }
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
      return this.reactiveForm.get('email_id')!;
    }
  
    get taxsegment_gid() {
      return this.reactiveForm.get('taxsegment_gid')!;
    }
    get billingemail() {
      return this.reactiveForm.get('billingemail_address')!;
    }

  get taxsegment_name() {
    return this.reactiveForm.get('taxsegment_name')!;
  }
  get paymentterms() {
    return this.reactiveForm.get('paymentterms')!;
  }
  get averageleadtime() {
    return this.reactiveForm.get('averageleadtime')!;
  }

  
 
    
    public validate(): void {
    debugger
      console.log(this.reactiveForm.value)
        this.vendor = this.reactiveForm.value;

          let formData = new FormData();
          if(this.file !=null &&  this.file != undefined){

        formData.append("vendorregister_gid", this.vendor.vendor_gid);
         formData.append("state", this.vendor.state);
         formData.append("fax", this.vendor.fax);
         formData.append("city", this.vendor.city);
         formData.append("address1", this.vendor.address1);
         formData.append("contact_telephonenumber", this.vendor.contact_telephonenumber);
         formData.append("vendor_code", this.vendor.vendor_code);
         formData.append("vendor_companyname", this.vendor.vendor_companyname);
         formData.append("contactperson_name", this.vendor.contactperson_name);
         formData.append("email_id", this.vendor.email_id);
         formData.append("postal_code", this.vendor.postal_code);
         formData.append("address2", this.vendor.address2);
         formData.append("country_name", this.vendor.country_name);
         formData.append("currency_code", this.vendor.currency_code);
         formData.append("tax_name", this.vendor.tax_name);
         formData.append("taxsegment_name", this.vendor.taxsegment_name);
         formData.append("taxsegment_gid", this.vendor.taxsegment_gid);
          var api='PmrMstVendorRegister/PostVendorRegisterUpdate'
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
          var api7='PmrMstVendorRegister/PostVendorRegisterUpdate'
          this.NgxSpinnerService.show()
            this.service.post(api7,this.vendor).subscribe((result:any) => {

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
        
        
        return;
      

    }
}

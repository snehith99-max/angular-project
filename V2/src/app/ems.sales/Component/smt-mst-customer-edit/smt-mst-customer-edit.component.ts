import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import {CountryISO,SearchCountryField,} from "ngx-intl-tel-input";
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smt-mst-customer-edit',
  templateUrl: './smt-mst-customer-edit.component.html',
  styleUrls: ['./smt-mst-customer-edit.component.scss']
})
export class SmtMstCustomerEditComponent {
EditForm : FormGroup | any;
region_list : any[]=[];
country_list: any[]=[];
taxsegment_list: any[]=[];
pricesegment_list: any[]=[];
currency_list : any[]=[];
salesperson_list : any[]=[];
GetCustomerList: any;
customer_list: any;
customeredit: any;
mdlpricesegment:any;
responsedata: any;
customertype_list: any[]=[];
MdlType:any;
lspage:any;
SearchCountryField = SearchCountryField;
CountryISO = CountryISO;
preferredCountries: CountryISO[] = [
  CountryISO.India,
  CountryISO.India
];
customer_gid:any;
  
  MdlTaxsegment: any;
constructor(private http:HttpClient,public NgxSpinnerService:NgxSpinnerService, private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService) {
 
}
ngOnInit(): void
{
  this.EditForm = new FormGroup
  ({
     customer_gid: new FormControl(''),
     customer_id: new FormControl(''),
     customer_name: new FormControl(''),
     customercontact_name: new FormControl(''),
     mobiles: new FormControl(''),
     pricesegment_name: new FormControl(''),
     taxsegment_name: new FormControl('',[Validators.required]),
     email: new FormControl(''),
     billemail: new FormControl(''),
     customer_type: new FormControl(''),
     address1: new FormControl(''),
     address2: new FormControl(''),
     city: new FormControl(''),
     postal_code: new FormControl(''),
     country_name: new FormControl('',Validators.required),
     customer_state: new FormControl(''),
     gst_number: new FormControl(''),
     tax_no: new FormControl(''),
     country_code: new FormControl(''),
     sales_person: new FormControl('',Validators.required),
     currency: new FormControl('',Validators.required),
     credit_days: new FormControl(''),
     area_code: new FormControl(''),
     fax_number: new FormControl(''),
     designation: new FormControl(''),
     company_website: new FormControl(''),
     region_name: new FormControl(''),
     taxsegment_gid: new FormControl(''),
  })
  
  this.customeredit = this.route.snapshot.paramMap.get('customer_gid');
  const lspage1 = this.route.snapshot.paramMap.get('lspage');
  this.lspage= lspage1
  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.customeredit, secretKey).toString(enc.Utf8);
  //const deencryptedParam1 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);
 this.customer_gid=(deencryptedParam);
 //this.lspage=(deencryptedParam1);

 console.log(this.lspage)
  this.GetCustomerEditSummary(deencryptedParam);

//country drop down//
var url = 'SmrTrnCustomerSummary/Getcountry'
this.service.get(url).subscribe((result: any) => {
  this.country_list = result.Getcountry;
  //  this.EditForm.get("countryname")?.setValue(this.GetCustomerList[0].customer_gid);

});

//region dropdown//
  var url = 'SmrTrnCustomerSummary/Getregion'
  this.service.get(url).subscribe((result: any) => {
    this.region_list = result.Getregion;
  });

  // Type dropdown//
  var url = 'SmrTrnCustomerSummary/GetCustomerTypeSummary'
  this.service.get(url).subscribe((result: any) => {
    this.customertype_list = result.customertype_list1;
  });
  var url = 'SmrMstTaxSegment/GetTaxSegmentSummary'
    this.service.get(url).subscribe((result: any) => {
      this.taxsegment_list = result.TaxSegmentSummary_list;
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
}
get country_name() {
  return this.EditForm.get('country_name')!;
}

get customer_type() {
  return this.EditForm.get('customer_type')!;
}

get mobiles() {
  return this.EditForm.get('mobiles')!;
}
get address1() {
  return this.EditForm.get('address1')!;
}


get customer_name() {
  return this.EditForm.get('customer_name')!;
}
get customercontact_name() {
  return this.EditForm.get('customercontact_name')!;
}
// get customer_state() {
//   return this.EditForm.get('customer_state')!;
// }
get email() {
  return this.EditForm.get('email')!;
}
get tax_no() {
  return this.EditForm.get('tax_no')!;
}
get billemail() {
  return this.EditForm.get('billemail')!;
}
get region_name() {
  return this.EditForm.get('region_name')!;
}
get taxsegment_name() {
  return this.EditForm.get('taxsegment_name')!;
}
get sales_person() {
  return this.EditForm.get('sales_person')!;
}
get currency() {
  return this.EditForm.get('currency')!;
}
get credit_days() {
  return this.EditForm.get('credit_days')!;
}
get pricesegment_name() {
  return this.EditForm.get('pricesegment_name')!;
}
get taxsegment_gid() {
  return this.EditForm.get('taxsegment_gid')!;
}
GetCustomerEditSummary(customer_gid: any) {    

          var url = 'SmrTrnCustomerSummary/GetEditCustomer'      
          this.NgxSpinnerService.show();
          let param = {      
            customer_gid: customer_gid      
          }      
          this.service.getparams(url, param).subscribe((result: any) => {
      
            this.GetCustomerList = result.GetCustomerList;
            this.EditForm.get("customer_id")?.setValue(this.GetCustomerList[0].customer_id);
            this.EditForm.get("customer_gid")?.setValue(this.GetCustomerList[0].customer_gid);
            this.EditForm.get("customer_name")?.setValue(this.GetCustomerList[0].customer_name);
            this.EditForm.get("customercontact_name")?.setValue(this.GetCustomerList[0].customercontact_name);
            //this.EditForm.get("customer_type")?.setValue(this.GetCustomerList[0].customer_type);
            this.MdlType=this.GetCustomerList[0].customer_type;
            this.EditForm.get("mobiles")?.setValue(this.GetCustomerList[0].mobile_number);
            this.EditForm.get("address1")?.setValue(this.GetCustomerList[0].address1);
            this.EditForm.get("address2")?.setValue(this.GetCustomerList[0].address2);
            this.EditForm.get("city")?.setValue(this.GetCustomerList[0].city);
            this.EditForm.get("email")?.setValue(this.GetCustomerList[0].email);
            this.EditForm.get("postal_code")?.setValue(this.GetCustomerList[0].postal_code);
            this.EditForm.get("country_name")?.setValue(this.GetCustomerList[0].country_name);
            this.EditForm.get("customer_state")?.setValue(this.GetCustomerList[0].customer_state);
            this.EditForm.get("tax_no")?.setValue(this.GetCustomerList[0].gst_number);
            this.EditForm.get("region_name")?.setValue(this.GetCustomerList[0].region_name);
            this.EditForm.get("company_website")?.setValue(this.GetCustomerList[0].company_website);
            this.EditForm.get("country_code")?.setValue(this.GetCustomerList[0].country_code);
            this.EditForm.get("area_code")?.setValue(this.GetCustomerList[0].area_code);
            this.EditForm.get("fax_number")?.setValue(this.GetCustomerList[0].fax_number);
            this.EditForm.get("designation")?.setValue(this.GetCustomerList[0].designation);
            this.EditForm.get("taxsegment_name")?.setValue(this.GetCustomerList[0].taxsegment_name);
            this.EditForm.get("taxsegment_gid")?.setValue(this.GetCustomerList[0].taxsegment_gid);
            this.EditForm.get("sales_person")?.setValue(this.GetCustomerList[0].sales_person);
            this.EditForm.get("salesperson_gid")?.setValue(this.GetCustomerList[0].salesperson_gid);
            this.EditForm.get("credit_days")?.setValue(this.GetCustomerList[0].credit_days);
            this.EditForm.get("billemail")?.setValue(this.GetCustomerList[0].billing_email);
            this.EditForm.get("currency")?.setValue(this.GetCustomerList[0].currency);
            this.EditForm.get("currency_gid")?.setValue(this.GetCustomerList[0].currency_gid);
            this.EditForm.get("pricesegment_name")?.setValue(this.GetCustomerList[0].pricesegment_name);
            this.EditForm.get("pricesegment_gid")?.setValue(this.GetCustomerList[0].pricesegment_gid);
            this.MdlTaxsegment = this.GetCustomerList[0].taxsegment_gid;
            this.EditForm.get("taxsegment_name")?.setValue(this.MdlTaxsegment);
            this.NgxSpinnerService.hide();
          })
}
  validate() {
    if(this.EditForm.value.customer_name != null && this.EditForm.value.customercontact_name != null &&
      this.EditForm.value.email != null && this.EditForm.value.sales_person != null && this.EditForm.value.currency != null && this.EditForm.value.taxsegment_name != null &&
      this.EditForm.value.address1 != null && this.EditForm.value.country_name != null ){
    var params = {
      customer_id: this.EditForm.value.customer_id,
      customer_name: this.EditForm.value.customer_name,
      customer_gid: this.EditForm.value.customer_gid,
      customercontact_name: this.EditForm.value.customercontact_name,
      customer_type: this.EditForm.value.customer_type,
      mobiles: this.EditForm.value.mobiles,
      email: this.EditForm.value.email,
      address1: this.EditForm.value.address1,
      address2: this.EditForm.value.address2,
      city: this.EditForm.value.city,
      postal_code: this.EditForm.value.postal_code,
      customer_state: this.EditForm.value.customer_state,
      country_name: this.EditForm.value.country_name,
      region_name: this.EditForm.value.region_name,
      designation: this.EditForm.value.designation,
      country_code: this.EditForm.value.country_code,
      area_code: this.EditForm.value.area_code,
      fax_number: this.EditForm.value.fax_number,
      company_website: this.EditForm.value.company_website,
      taxsegment_name: this.EditForm.value.taxsegment_name,
      taxsegment_gid: this.EditForm.value.taxsegment_gid,
      sales_person: this.EditForm.value.sales_person,
      pricesegment_name: this.EditForm.value.pricesegment_name,
      currency: this.EditForm.value.currency,
      credit_days: this.EditForm.value.credit_days,
      tax_no:this.EditForm.value.tax_no,
      billemail:this.EditForm.value.billemail
    }

    

    var url = 'SmrTrnCustomerSummary/UpdateCustomerEdit'
    this.NgxSpinnerService.show();
    this.service.postparams(url,params).subscribe((result: any) => {
    
      if (result.status == false) {
        this.NgxSpinnerService.hide()
        this.ToastrService.warning(result.message)
    
      }
      else{
        this.ToastrService.success(result.message)
         this.EditForm.reset(); 
         this.router.navigate(["/smr/SmrTrnCustomerSummary"])
         this.NgxSpinnerService.hide()
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
    if(this.lspage == '/smr/SmrTrnCustomerRetailer')
    {
      this.router.navigate(['/smr/SmrTrnCustomerRetailer']);

    }
   else if(this.lspage == '/smr/SmrTrnCustomerDistributor')
    {
      this.router.navigate(['/smr/SmrTrnCustomerDistributor']);

    }
  
    else if(this.lspage == '/smr/SmrTrnCustomerCorporate')
    {
      this.router.navigate(['/smr/SmrTrnCustomerCorporate']);

    }
else

{
  this.router.navigate(['/smr/SmrTrnCustomerSummary'])
}
  }
  onKeyPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      event.preventDefault(); // Prevent form submission
    }
  }
}

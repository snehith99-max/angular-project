import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { CountryISO, SearchCountryField } from 'ngx-intl-tel-input';
import { NgxSpinnerService } from 'ngx-spinner';
interface ICustomer {
  mobiles: any;
  country_name: string;
  customer_state: string;
  customer_city: string;
  mobile: string;
  email: string;
  customer_pin: string;
  address1: string;
  address2: string;
  customerbranch_name: string;
  designation: string;
  region_name: string;
  customer_gid : string;
  customercontact_name : string; 
}
@Component({
  selector: 'app-smr-trn-customerbranch',
  templateUrl: './smr-trn-customerbranch.component.html',
  styleUrls: ['./smr-trn-customerbranch.component.scss']
})
export class SmrTrnCustomerbranchComponent {
  file!: File;
  customer!: ICustomer;
  smrcustomerbranch_list: any[] = [];
  reactiveForm!: FormGroup;
  country_list: any[] = [];
  MdlCountry:any;
  region_list: any[] = [];
  responsedata: any;
  customer_gid: any
  customer_gids :any;
  smrcustomer_list:any;
  lspage:any;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  Form: any;
  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router,private router:ActivatedRoute, public NgxSpinnerService:NgxSpinnerService) {
    this.customer = {} as ICustomer;
  }
  ngOnInit(): void {

    //this.GetSmrTrnCustomerBranchSummary();
    const customer_gid =this.router.snapshot.paramMap.get('customer_gid');
    const lspage1 = this.router.snapshot.paramMap.get('lspage');
    this.lspage = lspage1
    this.customer_gid= customer_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.customer_gid,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetSmrTrnCustomerBranchSummary(deencryptedParam);
    this.reactiveForm = new FormGroup({

       customer_state: new FormControl(this.customer.customer_state),
      
       customer_gid:new FormControl(this.customer.customer_gid, [
        Validators.required,
        
      ]),

       customer_city: new FormControl(this.customer.customer_city, [
        Validators.required,

      ]),

       address1: new FormControl(this.customer.address1, [
        Validators.required,
      ]),

       address2: new FormControl(this.customer.address2),

       customerbranch_name: new FormControl(this.customer.customerbranch_name, [
        Validators.required,

      ]),

      mobiles: new FormControl(this.customer.mobiles),
     
       region_name: new FormControl(this.customer.region_name, [

      ]),

       designation: new FormControl(this.customer.designation),

       email: new FormControl(''),

       customer_pin: new FormControl(''),

       customercontact_name: new FormControl(''),

       country_name: new FormControl(this.customer.country_name),
      

    }

    );
    //country drop down//
    var url = 'SmrTrnCustomerSummary/Getcountry'
    this.service.get(url).subscribe((result: any) => {
      this.country_list = result.Getcountry;
    });
    //regionname dropdown//

    var url = 'SmrTrnCustomerSummary/Getregion'
    this.service.get(url).subscribe((result: any) => {
      this.region_list = result.Getregion;
    });

    var url = 'SmrTrnCustomerSummary/GetSmrTrnCustomerSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#smrcustomer_list').DataTable().destroy();
       this.responsedata = result;
       this.smrcustomer_list = this.responsedata.smrcustomer_list;

       this.reactiveForm.get("customer_gid")?.setValue(this.smrcustomer_list[0].customergroup_gid);
    });

  }
  
  GetSmrTrnCustomerBranchSummary(customer_gid: any) {
  
    var url = 'SmrTrnCustomerSummary/GetSmrTrnCustomerBranchSummary'
    
    let param = {
      customer_gid : customer_gid
        }
    this.service.getparams(url,param).subscribe((result: any) => {
       this.responsedata = result;
       this.smrcustomerbranch_list = this.responsedata.smrcustomerbranch_list;
    });
  }
  
  get country_name() {
    return this.reactiveForm.get('country_name')!;
  }

  get address1() {
    return this.reactiveForm.get('address1')!;
  }

  get customerbranch_name() {
    return this.reactiveForm.get('customerbranch_name')!;
  }

  get customercontact_name() {
    return this.reactiveForm.get('customercontact_name')!;
  }
 
  get email() {
    return this.reactiveForm.get('email')!;
  }

  get region_name() {
    return this.reactiveForm.get('region_name')!;
  }
 
  get mobiles() {
    return this.reactiveForm.get('mobiles')!;
  }

  validated() {

    console.log(this.reactiveForm)
    const customer_gid =this.router.snapshot.paramMap.get('customer_gid');
    this.customer_gid= customer_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.customer_gid,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.reactiveForm.get("customer_gid")?.setValue(deencryptedParam);
    this.customer_gids = deencryptedParam;
 
    if (
        this.country_name != null
       && this.address1.value != null 
        ) {
 
    var api ='SmrTrnCustomerSummary/PostCustomerbranch';
    this.NgxSpinnerService.show();
    this.service.post(api, this.reactiveForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Kindly fill all mandatory fields!!')
      }
         else {
       
        this.ToastrService.success('Address Updated Successfully!');
        this.reactiveForm.reset();
        this.NgxSpinnerService.hide();
        this.GetSmrTrnCustomerBranchSummary(this.customer_gids);
         }
    });
  }
  else{
    this.ToastrService.warning('Kindly fill all mandatory fields!!');

  }

  }
  onclose(){
    if(this.lspage == '/smr/SmrTrnCustomerRetailer')
    {
      this.route.navigate(['/smr/SmrTrnCustomerRetailer']);

    }
    else if(this.lspage == '/smr/SmrTrnCustomerDistributor')
    {
      this.route.navigate(['/smr/SmrTrnCustomerDistributor']);

    }
    else if(this.lspage == '/smr/SmrTrnCustomerCorporate')
    {
      this.route.navigate(['/smr/SmrTrnCustomerCorporate']);

    }
    else
    {
    this.route.navigate(['/smr/SmrTrnCustomerSummary/'])
    }
  }
  onedit(){}

  onKeyPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      event.preventDefault(); // Prevent form submission
    }
  }
}

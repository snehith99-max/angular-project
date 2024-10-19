import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from 'jquery';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-acc-trn-glcodedebitoradd',
  templateUrl: './acc-trn-glcodedebitoradd.component.html',
  styleUrls: ['./acc-trn-glcodedebitoradd.component.scss']
})
export class AccTrnGlcodedebitoraddComponent {
  reactiveform: FormGroup | any;
  RegionName_List: any;
  CountryName_List: any;
  responsedata: any;

  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router,
    private FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService, private datePipe: DatePipe,) {
  }

  ngOnInit(): void {
    var url = 'GLCode/GetRegionDtls'
    this.service.get(url).subscribe((result: any) => {
      this.RegionName_List = result.GetRegionName_List;
    });

    var url = 'GLCode/GetCountryDtls'
    this.service.get(url).subscribe((result: any) => {
      this.CountryName_List = result.GetCountryName_List;
    });

    this.reactiveform = new FormGroup({
      customer_code: new FormControl(null,Validators.required),
      customer_name: new FormControl(null,Validators.required),
      address_line1: new FormControl(null,Validators.required),
      address_line2: new FormControl(null),
      region_name: new FormControl(null,Validators.required),
      city_name: new FormControl(null,Validators.required),
      state_name: new FormControl(null,Validators.required),
      pincode: new FormControl(null),
      country_name: new FormControl(null,Validators.required),
      contactperson_name: new FormControl(null,Validators.required),
      mobile_number: new FormControl('' , [ Validators.pattern('[0-9]{12}$'), Validators.maxLength(12)]),
      designation: new FormControl(null),
      email_address: new FormControl(null,Validators.required),
      company_website: new FormControl(null),
      contactcountry_code: new FormControl(null),
      countryarea_code: new FormControl(null),
      countrycontact_number: new FormControl(null),
      faxcountry_code: new FormControl(null),
      faxarea_code: new FormControl(null),
      fax_number: new FormControl(null),
    });
  }

  get customer_code() {
    return this.reactiveform.get('customer_code')!;
  }

  get customer_name() {
    return this.reactiveform.get('customer_name')!;
  }

  get address_line1() {
    return this.reactiveform.get('address_line1')!;
  }

  get region_name() {
    return this.reactiveform.get('region_name')!;
  }

  get city_name() {
    return this.reactiveform.get('city_name')!;
  }

  get state_name() {
    return this.reactiveform.get('state_name')!;
  }

  get country_name() {
    return this.reactiveform.get('country_name')!;
  }

  get contactperson_name() {
    return this.reactiveform.get('contactperson_name')!;
  }

  get mobile_number() {
    return this.reactiveform.get('mobile_number')!;
  }

  get email_address() {
    return this.reactiveform.get('email_address')!;
  }

  SubmitDebitor_Glcode() {
    this.reactiveform.value;
    if (this.reactiveform.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'GLCode/PostDebitorGLCode';
      this.service.post(url, this.reactiveform.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
  
            top: 0, // Code is used for scroll top after event done
  
          });
          this.ToastrService.warning(result.message)           
          this.reactiveform.reset();          
        }
        else {
          window.scrollTo({
  
            top: 0, // Code is used for scroll top after event done
  
          });
          this.ToastrService.success(result.message)                 
          this.reactiveform.reset();
          this.route.navigate(['/finance/AccMstGlcodeSummary']);
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else { }
  }
  
  Back(){   
    this.route.navigate(['/finance/AccMstGlcodeSummary']);
  }

}

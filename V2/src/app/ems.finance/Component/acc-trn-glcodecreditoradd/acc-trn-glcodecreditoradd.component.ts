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
  selector: 'app-acc-trn-glcodecreditoradd',
  templateUrl: './acc-trn-glcodecreditoradd.component.html',
  styleUrls: ['./acc-trn-glcodecreditoradd.component.scss']
})
export class AccTrnGlcodecreditoraddComponent {
  reactiveform: FormGroup | any;
  responsedata: any;
  CountryName_List: any;

  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router,
    private FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService, private datePipe: DatePipe,) {
  }

  ngOnInit(): void {

    var url = 'GLCode/GetCountryDtls'
    this.service.get(url).subscribe((result: any) => {
      this.CountryName_List = result.GetCountryName_List;
    });

    this.reactiveform = new FormGroup({
      vendor_code: new FormControl(null,Validators.required),
      vendorcompany_name: new FormControl(null,Validators.required),
      contactperson_name: new FormControl(null,Validators.required),
      contact_number: new FormControl('' , [ Validators.pattern('[0-9]{10}$'), Validators.maxLength(10)]),
      address_line1: new FormControl(null,Validators.required),
      address_line2: new FormControl(null),
      city_name: new FormControl(null,Validators.required),
      state_name: new FormControl(null,Validators.required),
      pincode: new FormControl(null,Validators.required),
      country_name: new FormControl(null,Validators.required),
      email_address: new FormControl(null,Validators.required),
      fax_number: new FormControl(null)
    });
  }

  get vendor_code() {
    return this.reactiveform.get('vendor_code')!;
  }

  get vendorcompany_name() {
    return this.reactiveform.get('vendorcompany_name')!;
  }

  get contactperson_name() {
    return this.reactiveform.get('contactperson_name')!;
  }

  get contact_number() {
    return this.reactiveform.get('contact_number')!;
  }

  get address_line1() {
    return this.reactiveform.get('address_line1')!;
  }

  get address_line2() {
    return this.reactiveform.get('address_line2')!;
  }

  get city_name() {
    return this.reactiveform.get('city_name')!;
  }

  get state_name() {
    return this.reactiveform.get('state_name')!;
  }

  get pincode() {
    return this.reactiveform.get('pincode')!;
  }

  get country_name() {
    return this.reactiveform.get('country_name')!;
  }

  get email_address() {
    return this.reactiveform.get('email_address')!;
  }

  get fax_number() {
    return this.reactiveform.get('fax_number')!;
  }
  
  SubmitCreditor_Glcode() {
    this.reactiveform.value;
    if (this.reactiveform.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'GLCode/PostCreditorGLCode';
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

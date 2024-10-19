import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CountryISO, SearchCountryField, } from "ngx-intl-tel-input";

interface Iinstitute  {
  institute_location: string;
  institute_name: string;
  user_code:string;
  created_by:string;
  created_date: string;
  institute_gid:string;
  Institute_status:string;
  contact_person:string;
  mobile:string;
  showPassword: boolean;
  ins_address1 :string;
  ins_address2 :string;
  ins_pincode :string;
  ins_city :string;
  ins_state :string;
  ins_country:string;

}
@Component({
  selector: 'app-law-mst-instituteadd',
  templateUrl: './law-mst-instituteadd.component.html',
  styleUrls: ['./law-mst-instituteadd.component.scss']
})
export class LawMstInstituteaddComponent implements OnInit {
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  InsAddForm: FormGroup | any;
  Institute!: Iinstitute;
  country_list: any[] = [];
  institute_gid: any;
  showhide: boolean=true;
  confirmPasswordTouched = false;
  password: string | any;
  confirmpassword: any;
  institutemail_id:any;
  institute_code: any;
  institute_prefix:any;
  constructor(private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService, private ToastrService: ToastrService,private route: Router) {
    this.Institute = {} as Iinstitute;
  }

  ngOnInit(): void {

    this.InsAddForm = new FormGroup({
      institute_location: new FormControl(''),
      institute_code:  new FormControl(null,
        [
          Validators.required,
          Validators.pattern(/^(?!\s*$).+/),
        ]),
        institute_prefix:  new FormControl(null,
          [
            Validators.required,
            Validators.pattern(/^(?!\s*$).+/),
          ]),
      institute_name: new FormControl(''),
      password: new FormControl(null,
        [
          Validators.required,
          Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)
        ]),
      institute_gid: new FormControl(''),
      contact_person: new FormControl(null, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
      ]),
      institutemail_id: new FormControl(null, [
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')
      ]),
      mobile: new FormControl(null,
        [
          Validators.required, 
        ]),
      confirmpassword: new FormControl(null,this.showPassword ? Validators.required : null),
      ins_address1 :new FormControl(''),
      ins_address2 :new FormControl(''),
      ins_pincode :new FormControl(null,
        [
          Validators.required, 
          Validators.pattern(/^[0-9]+$/),
          Validators.minLength(6),
        ]),
      ins_city :new FormControl(''),
      ins_state :new FormControl(''),
      ins_country :new FormControl(''),
      

    });
    debugger
    var api = 'LawMstInstitute/GetInstitutecountry'
    this.SocketService.get(api).subscribe((result: any) => {
      this.country_list = result.countryList;      
    });

  }
  get institute_location() {
    return this.InsAddForm.get('institute_location')!;
  }
  get institute_name() {
    return this.InsAddForm.get('institute_name')!;
  }
  get contact_person() {
    return this.InsAddForm.get('contact_person')!;
  }
  get mobile() {
    return this.InsAddForm.get('mobile')!;
  }
  get ins_address1() {
    return this.InsAddForm.get('ins_address1')!;
  }
  get ins_address2() {
    return this.InsAddForm.get('ins_address2')!;
  }
  get ins_pincode() {
    return this.InsAddForm.get('ins_pincode')!;
  }
  get ins_city() {
    return this.InsAddForm.get('ins_city')!;
  }
  get ins_state() {
    return this.InsAddForm.get('ins_state')!;
  }
  get ins_country() {
    return this.InsAddForm.get('ins_country')!;
  }
  validate(){
  var params = {
    institute_name: this.InsAddForm.value.institute_name,
    institute_code: this.InsAddForm.value.institute_code,
    institute_prefix: this.InsAddForm.value.institute_prefix,
    password: this.InsAddForm.value.password,
    mobile: this.InsAddForm.value.mobile.e164Number,
    contact_person: this.InsAddForm.value.contact_person,
    ins_address1 :this.InsAddForm.value.ins_address1,
    ins_address2 :this.InsAddForm.value.ins_address2,
    ins_pincode :this.InsAddForm.value.ins_pincode,
    ins_city :this.InsAddForm.value.ins_city,
    ins_state :this.InsAddForm.value.ins_state,
    ins_country:this.InsAddForm.value.ins_country,
    institutemail_id:this.InsAddForm.value.institutemail_id,

  }
  debugger
  this.NgxSpinnerService.show();
  var url = 'LawMstInstitute/PostInstituteAdd';

  this.SocketService.postparams(url, params).subscribe((result: any) => {
    if (result.status == true) {
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      this.route.navigate(['/legal/LglMstInstitute']);
    }
    else {
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();
    }
  }
  )

  }
  backbutton(){
    this.route.navigate(['/legal/LglMstInstitute']);
  }
  showPassword: boolean = false;
  showConfrimPassword: boolean = false;

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfrimPasswordVisibility(): void {
    this.showConfrimPassword = !this.showConfrimPassword;
  }
  passwordsMatch(): boolean {
    const password = this.InsAddForm.get('password').value;
    const confirmPassword = this.InsAddForm.get('confirmpassword').value;
    return password === confirmPassword;
  }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { CountryISO, SearchCountryField, } from "ngx-intl-tel-input";
import { AES, enc } from 'crypto-js';

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
  selector: 'app-law-mst-instituteedit',
  templateUrl: './law-mst-instituteedit.component.html',
  styleUrls: ['./law-mst-instituteedit.component.scss']
})
export class LawMstInstituteeditComponent implements OnInit {
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  InseEditForm: FormGroup | any;
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
  institutegid: any;
  InstituteList: any;
  constructor(private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService, private ToastrService: ToastrService,private route: Router,private router: ActivatedRoute) {
    this.Institute = {} as Iinstitute;
  }

  ngOnInit(): void {
    debugger
    const institute_gid = this.router.snapshot.paramMap.get('institute_gid');
    this.institutegid=institute_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.institutegid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetInstituteEditSummary(deencryptedParam)

    this.InseEditForm = new FormGroup({
      institute_location: new FormControl(''),
      institute_code: new FormControl({ value: null, disabled: true }), 
      institute_prefix: new FormControl({ value: null, disabled: true }), 
      institute_name: new FormControl(''),
      institute_gid: new FormControl(''),
      contact_person: new FormControl(null, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
      ]),
      institutemail_id: new FormControl(null, [
        Validators.required,
        Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')
      ]),
      mobile: new FormControl(null,
        [
          Validators.required, 
        ]),
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

  GetInstituteEditSummary(institute_gid:any){
debugger
    var url = 'LawMstInstitute/GetInstituteEditSummary'
    let param = {
      institute_gid : institute_gid 
    }
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.InstituteList = result.institute_List;
      this.InseEditForm.get("mobile")?.setValue(this.InstituteList[0].mobile);
      this.InseEditForm.get("institute_code")?.setValue(this.InstituteList[0].institute_code);
      this.InseEditForm.get("institute_prefix")?.setValue(this.InstituteList[0].institute_prefix);
      this.InseEditForm.get("institute_name")?.setValue(this.InstituteList[0].institute_name);
      this.InseEditForm.get("institute_location")?.setValue(this.InstituteList[0].institute_location);
      this.InseEditForm.get("institutemail_id")?.setValue(this.InstituteList[0].institutemail_id);
      this.InseEditForm.get("contact_person")?.setValue(this.InstituteList[0].contact_person);
      this.InseEditForm.get("ins_address1")?.setValue(this.InstituteList[0].ins_address1);
      this.InseEditForm.get("ins_address2")?.setValue(this.InstituteList[0].ins_address2);
      this.InseEditForm.get("ins_pincode")?.setValue(this.InstituteList[0].ins_pincode);
      this.InseEditForm.get("ins_city")?.setValue(this.InstituteList[0].ins_city);
      this.InseEditForm.get("ins_state")?.setValue(this.InstituteList[0].ins_state);
      this.InseEditForm.get("ins_country")?.setValue(this.InstituteList[0].ins_country);
      this.InseEditForm.get("institute_gid")?.setValue(this.InstituteList[0].institute_gid);
    });
  }
  get institute_location() {
    return this.InseEditForm.get('institute_location')!;
  }
  get institute_name() {
    return this.InseEditForm.get('institute_name')!;
  }
  get contact_person() {
    return this.InseEditForm.get('contact_person')!;
  }
  get mobile() {
    return this.InseEditForm.get('mobile')!;
  }
  get ins_address1() {
    return this.InseEditForm.get('ins_address1')!;
  }
  get ins_address2() {
    return this.InseEditForm.get('ins_address2')!;
  }
  get ins_pincode() {
    return this.InseEditForm.get('ins_pincode')!;
  }
  get ins_city() {
    return this.InseEditForm.get('ins_city')!;
  }
  get ins_state() {
    return this.InseEditForm.get('ins_state')!;
  }
  get ins_country() {
    return this.InseEditForm.get('ins_country')!;
  }
  validate(){
  var params = {
    institute_gid: this.InseEditForm.value.institute_gid,
    institute_name: this.InseEditForm.value.institute_name,
    institute_code: this.InseEditForm.value.institute_code,
    mobile: this.InseEditForm.value.mobile.e164Number,
    contact_person: this.InseEditForm.value.contact_person,
    ins_address1 :this.InseEditForm.value.ins_address1,
    ins_address2 :this.InseEditForm.value.ins_address2,
    ins_pincode :this.InseEditForm.value.ins_pincode,
    ins_city :this.InseEditForm.value.ins_city,
    ins_state :this.InseEditForm.value.ins_state,
    ins_country:this.InseEditForm.value.ins_country,
    institutemail_id:this.InseEditForm.value.institutemail_id,
  }
  debugger
  this.NgxSpinnerService.show();
  var url = 'LawMstInstitute/PostUpdateinstitute';
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
}

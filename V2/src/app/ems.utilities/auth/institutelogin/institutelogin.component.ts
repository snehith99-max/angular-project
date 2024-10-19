import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { SocketService } from '../../services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';


interface ILogin {
  password: string;
  confirmpassword: string;
  showPassword: boolean;
  companyid: string;
  institute_code: string;
  confirmusercode: string;
  mobile: string;

}
interface IReset {
  password: string;
  confirmpassword_reset: string;
  showPassword: boolean;
  showPassword1: boolean;
  companyid_reset: string;
  usercode_reset: string;
  old_password: string;

}
@Component({
  selector: 'app-institutelogin',
  templateUrl: './institutelogin.component.html',
  styleUrls: ['./institutelogin.component.scss']
})
export class InstituteloginComponent  implements OnInit, OnDestroy {
  reactiveFormforogot!: FormGroup;
  reactiveFormreset!: FormGroup;
  login!: ILogin;
  reset!: IReset;
  defaultAuth: any = {

  };
  loginForm: FormGroup | any;
  hasError?: boolean;
  returnUrl?: string;
  companycode: string | any;
  company_code: string | any;
  submitted = false;
  cc_flag: boolean = false;
  company: any;
  current_domain: any;
  // private fields
  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/


  constructor(
    private fb: FormBuilder,

    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService



  ) {
    this.login = {} as ILogin;
    this.reset = {} as IReset;

  }

  ngOnInit(): void {
    localStorage.clear();
    sessionStorage.removeItem('CRM_LEADBANK_GID_ENQUIRY');
    sessionStorage.removeItem('CRM_APPOINTMENT_GID');
    sessionStorage.removeItem('CRM_CUSTOMER_GID_QUOTATION');
    sessionStorage.removeItem('CRM_TOMAILID');
    //sessionStorage.clear();
    this.initForm();
    this.reactiveFormforogot = new FormGroup({

      password: new FormControl(this.login.password, [
        Validators.required,
      ]),
      institute_code: new FormControl(this.login.institute_code, [
        Validators.required,
      ]),
      companyid: new FormControl(this.login.companyid, [
        Validators.required,
      ]),
      mobile: new FormControl(this.login.mobile, [
        Validators.required,
        Validators.maxLength(10),
      ]),
      confirmpassword: new FormControl(''),


    });
    this.reactiveFormreset = new FormGroup({

      password: new FormControl(this.reset.password, [
        Validators.required,
      ]),
      usercode_reset: new FormControl(this.reset.usercode_reset, [
        Validators.required,
      ]),
      companyid_reset: new FormControl(this.reset.companyid_reset, [
        Validators.required,
      ]),
      old_password: new FormControl(this.reset.old_password, [
        Validators.required,
      ]),
      confirmpassword_reset: new FormControl(''),


    });
    this.current_domain = window.location.hostname;
    if (this.current_domain == 'lawyer.storyboardsystems.com') {
      this.loginForm.get("company_code")?.setValue('saha');
      this.reactiveFormforogot.get("companyid")?.setValue('saha');
      this.reactiveFormreset.get("companyid_reset")?.setValue('saha');
      localStorage.setItem('c_code','saha')
      this.companycode = 'saha'
      this.cc_flag = true
    }
            
    else {
      this.companycode = 'default'
    }

    this.returnUrl =
      this.route.snapshot.queryParams['returnUrl'.toString()] || '/';
  }
  
  get password() {
    return this.reactiveFormforogot.get('password')!;
  }
  get companyid() {
    return this.reactiveFormforogot.get('companyid')!;
  }
  get institute_code() {
    return this.reactiveFormforogot.get('institute_code')!;
  }
  get password_reset() {
    return this.reactiveFormreset.get('password')!;
  }
  get companyid_reset() {
    return this.reactiveFormreset.get('companyid_reset')!;
  }
  get usercode_reset() {
    return this.reactiveFormreset.get('usercode_reset')!;
  }
  get old_password() {
    return this.reactiveFormreset.get('old_password')!;
  }
  userpassword(password: any) {
    this.reactiveFormforogot.get("confirmpassword")?.setValue(password.value);
  }
  userrresetpassword(password_reset: any) {
    this.reactiveFormreset.get("confirmpassword_reset")?.setValue(password_reset.value);
  }
  get mobile() {
    return this.reactiveFormforogot.get('mobile')!;
  }
  
  get f() {
    return this.loginForm.controls;
  }

  initForm() {
    this.loginForm = this.fb.group({

      institute_code: [
        this.defaultAuth.institute_code,
        Validators.compose([

          Validators.required,

          Validators.minLength(4),
          Validators.maxLength(100), // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
          Validators.pattern(/^[a-zA-Z0-9]+$/)
        ]),
      ],
      user_password: [
        this.defaultAuth.user_password,
        Validators.compose([
          Validators.required,
          Validators.minLength(1),
          Validators.maxLength(20),
        ]),
      ],
      company_code: [
        this.defaultAuth.company_code,
        Validators.compose([
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(25),

        ]),
      ]
    });
  }

  submit() {
   
    if (this.loginForm.value.company_code == null || this.loginForm.value.company_code == "" || this.loginForm.value.institute_code == null || this.loginForm.value.institute_code == ""
      || this.loginForm.value.user_password == null || this.loginForm.value.user_password == "") {
      this.ToastrService.warning("Kindly fill in all the login details")
    }
    else {
      this.NgxSpinnerService.show();
      var api = 'InstituteLogin/IntUserLogin';
      this.service.post(api, this.loginForm.value).subscribe((result: any) => {
        if (result != null) {
          if (result.institute_gid == null || result.institute_gid == "") {
            this.NgxSpinnerService.hide();
            this.ToastrService.error("Invalid credentials. Kindly enter valid credentials.")
          }


          else if ((result.institute_gid != null || result.institute_gid != "") && (result.sref != null && result.sref != "")) {
            debugger
            localStorage.setItem('token', result.token);
            localStorage.setItem('user_gid', result.institute_gid);
            localStorage.setItem('c_code', result.c_code);
            this.router.navigate([result.sref]);
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message)
          }
        }
      }, (error: any) => {
        this.NgxSpinnerService.hide();
        if (error.status === 401) {
          this.router.navigate(['auth/401'])
          this.ToastrService.warning("Unauthorized")
        }
        else if (error.status === 404) {
          this.router.navigate(['auth/404'])
          this.ToastrService.warning("Not Found")
        }
        else if (error.status === 500) {
          this.router.navigate(['auth/500'])
          this.ToastrService.warning("Internal Server Error")
        }
      });

    }

  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
  onsubmit() {
    this.submitted = true
  }
  /////////////////////Forgot Popup update event//////////////
  onforgot() {
    //console.log(this.reactiveFormforogot.value)
    this.reactiveFormforogot.get("companyid")?.setValue(localStorage.getItem('c_code'));
    console.log(this.reactiveFormforogot)
    var url = 'Login/UserForgot';

    this.service.post(url, this.reactiveFormforogot.value).subscribe((result: any) => {

      if (result.status == false) {
        this.reactiveFormforogot.reset();
        this.ToastrService.warning(result.message)
      }
      else {
        this.reactiveFormforogot.reset();
        this.ToastrService.success(result.message)
      }

    });
  }
  onclose() {
    this.reactiveFormforogot.reset();
    this.reactiveFormforogot.get("companyid")?.setValue(localStorage.getItem('c_code'));
  }
  /////////////////////Reset Popup update event//////////////
  onreset() {
    this.reactiveFormreset.get("companyid_reset")?.setValue(localStorage.getItem('c_code'));
    console.log(this.reactiveFormreset)
    var url = 'Login/UserReset';

    this.service.post(url, this.reactiveFormreset.value).subscribe((result: any) => {

      if (result.status == false) {
        this.reactiveFormreset.reset();
        this.ToastrService.warning(result.message)
      }
      else {
        this.reactiveFormreset.reset();
        this.ToastrService.success(result.message)
      }

    });
  }
  onclosereset() {
    this.reactiveFormreset.reset();
    this.reactiveFormreset.get("companyid_reset")?.setValue(localStorage.getItem('c_code'));
  }
}

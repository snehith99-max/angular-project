import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { AbstractControl, ValidatorFn } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { SocketService } from '../../services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from '../../../Service/notification.service';

interface ILogin {
  password: string;
  confirmpassword: string;
  showPassword: boolean;
  companyid: string;
  usercode: string;
  confirmusercode: string;
}
interface IReset {
  password: string;
  confirmpassword_reset: string;
  confirmpassword_forgot: string;
  showPassword: boolean;
  showPassword1: boolean;
  companyid_reset: string;
  usercode_reset: string;
  old_password: string;
 showPassword2: boolean;

}
interface Iforgot {

  showPassword: boolean;
  showPassword1: boolean;
  showPassword2: boolean;
}
@Component({
  selector: 'app-vcxcontrollerlogin',
  templateUrl: './vcxcontrollerlogin.component.html',
  styleUrls: ['./vcxcontrollerlogin.component.scss']
})
export class VcxcontrollerloginComponent implements OnInit, OnDestroy {
  reactiveFormforogot!: FormGroup;
  reactiveFormreset!: FormGroup;
  login!: ILogin;
  reset!: IReset;
  forgot!: Iforgot;
  otpreactiveFormforogot!: FormGroup;

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
  show: boolean = false;
  email: any;
  verificationCompleted: boolean = false;
  showTooltip = false;
  showpasswordemail:boolean=true;
  otpresetpassword:boolean=true;
  otpresetbutton:boolean=false;
  verificationCompleted1: boolean = false;
  user_code:any;
  showPassword1: boolean = false;
  showPassword: boolean = false;
  showPassword2: boolean = false;
  otpverificationCompleted: boolean = false;
  showPassword4: boolean = false;
  // private fields
  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/
  password: any;
  code:any;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService,
    private notificationService: NotificationService


  ) {
    this.login = {} as ILogin;
    this.reset = {} as IReset;
    this.forgot = {} as Iforgot;

  }
  
  notification() {
    this.notificationService.loginNotification();
  }
  onsubmit() {
    this.submitted = true
  }
  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
  ngOnInit(): void {
    this.initForm();

    this.reactiveFormforogot = this.fb.group({
      usercode: new FormControl(this.login.usercode, [
        Validators.required, Validators.pattern(/^\S.*$/)
      ]),
      companyid: new FormControl(this.login.companyid, [
        Validators.required, Validators.pattern(/^\S.*$/)
      ]),
      forgotportal_emailid: new FormControl('', Validators.pattern(/^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/)),
      confirmpassword_forgot: new FormControl('', [Validators.required,
      Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/)]),
      forgot_pwd: new FormControl('', [
        Validators.required,
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/)]),

      code: new FormControl('', [Validators.required, Validators.minLength(6)]),
    },
      {
        validators: passwordMatchValidator('forgot_pwd', 'confirmpassword_forgot'),
      });

    this.reactiveFormreset =this.fb.group({

      password: new FormControl(this.reset.password, [
        Validators.required,
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/)]),

      usercode_reset: new FormControl(this.reset.usercode_reset, [
        Validators.required, Validators.pattern(/^\S.*$/)
      ]),
      companyid_reset: new FormControl(this.reset.companyid_reset, [
        Validators.required, Validators.pattern(/^\S.*$/)
      ]),
      old_password: new FormControl(this.reset.old_password, [
        Validators.required, Validators.pattern(/^\S.*$/), 
      ]),
      confirmpassword_reset: new FormControl('',[Validators.required,
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/)]),

    }, {
      validators: passwordMatchValidator1('password', 'confirmpassword_reset'),
    });


    this.otpreactiveFormforogot = this.fb.group({
      usercode: new FormControl(this.login.usercode, [
        Validators.required, Validators.pattern(/^\S.*$/)
      ]),
      companyid: new FormControl(this.login.companyid, [
        Validators.required, Validators.pattern(/^\S.*$/)
      ]),
      
      otpcode: new FormControl('', [Validators.required, Validators.minLength(6)]),
    },
      {
        validators: passwordMatchValidator('forgot_pwd', 'confirmpassword_forgot'),
      });

      this.companycode = 'default'
    

    // get return url from route parameters or default to '/'
    this.returnUrl =
      this.route.snapshot.queryParams['returnUrl'.toString()] || '/';



  }
  get forgot_pwd() {
    return this.reactiveFormforogot.get('forgot_pwd')!;
  }
  get confirmpassword_forgot() {
    return this.reactiveFormforogot.get('confirmpassword_forgot')!;
  }
  get confirmpassword_reset() {
    return this.reactiveFormreset.get('confirmpassword_reset')!;
  }
  get companyid() {
    return this.reactiveFormforogot.get('companyid')!;
  }
  get usercode() {
    return this.reactiveFormforogot.get('usercode')!;
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
  
  userforgotpassword(password_reset: any) {
    this.reactiveFormreset.get("confirmpassword_reset")?.setValue(password_reset.value);
  }

  userrresetpassword(password_reset: any) {
    this.reactiveFormreset.get("confirmpassword_reset")?.setValue(password_reset.value);
  }
  get forgotportal_emailid() {
    return this.reactiveFormforogot.get('forgotportal_emailid')!;
  }
  get code_forgot() {
    return this.reactiveFormforogot.get('code')!;
  }
  get otpcode() {
    return this.otpreactiveFormforogot.get('otpcode')!;
  }
  get f() {
    return this.loginForm.controls;
  }
  initForm() {
    this.loginForm = this.fb.group({

      user_code: [
        this.defaultAuth.user_code,
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
  togglePasswordVisibility2(): void {
    this.showPassword1 = !this.showPassword1;
  }
  togglePasswordVisibility1(): void {
    this.showPassword = !this.showPassword;
  }
  togglePasswordVisibility3(): void {
    this.showPassword2 = !this.showPassword2;
  } 
  PasswordVisibility1(): void {
    this.showPassword1 = !this.showPassword1;
  } 
  PasswordVisibility2(): void {
    this.showPassword = !this.showPassword;
  } 
  PasswordVisibility3(): void {
    this.showPassword2 = !this.showPassword2;
  } 
  togglePasswordVisibility4(): void {
    this.showPassword4 = !this.showPassword4;
  } 
  submit() {
    this.loginForm.value.company_code = localStorage.getItem('c_code') == null ? this.loginForm.value.company_code : localStorage.getItem('c_code')
    // if (environment.test_environment == 'N')
    //   this.loginForm.value.company_code = environment.company_code;
    if (this.loginForm.value.company_code == null || this.loginForm.value.company_code == "" || this.loginForm.value.user_code == null || this.loginForm.value.user_code == ""
      || this.loginForm.value.user_password == null || this.loginForm.value.user_password == "") {
      this.ToastrService.warning("Kindly fill in all the login details")
      
    }  


    else {
      this.NgxSpinnerService.show();
      localStorage.clear();
    sessionStorage.removeItem('CRM_LEADBANK_GID_ENQUIRY');
    sessionStorage.removeItem('CRM_APPOINTMENT_GID');
    sessionStorage.removeItem('CRM_CUSTOMER_GID_QUOTATION');
    sessionStorage.removeItem('CRM_TOMAILID');
      var api = 'Login/VcxControllerUserLogin';
      this.service.post(api, this.loginForm.value).subscribe((result: any) => {
        if (result != null) {
          if (result.user_gid == null || result.user_gid == "") {
            this.NgxSpinnerService.hide();
            this.ToastrService.error("Invalid credentials. Kindly enter valid credentials.")
          }          
         
          else if ((result.sref == null || result.sref == "") && (result.user_gid != null || result.user_gid != "")) {
            this. otpverificationCompleted=true;
            this.notificationService.loginNotification();

            this.NgxSpinnerService.hide();
             this.loginForm.reset();
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
  toggleTooltip() {
    this.showTooltip = !this.showTooltip;
  }
  
  codebind(event: any) {
    this.code = event.target.value;

  }

  passwordbind(event: any) {
    this.password = event.target.value;
  }
  send() {
    let param = {
      usercode: this.reactiveFormforogot.value.usercode,
      companyid: this.reactiveFormforogot.value.companyid
    }


    var url = 'Login/forgormailtrigger';

    this.service.post(url, param).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }

    });
  }

  verify() {
    let param = {
      user_code: this.reactiveFormforogot.value.usercode,
      company_code: this.reactiveFormforogot.value.companyid,
      code: this.reactiveFormforogot.value.code
    }

    var url = 'Login/verifypassword';

    this.service.post(url, param).subscribe((result: any) => {

      if (result.status == true) {
        this.show = true;
        this.ToastrService.success(result.message)
        this.showpasswordemail=false;
       this. verificationCompleted=true;
      }

      else {
        this.show = false;
        this.ToastrService.warning(result.message)
      }

    });

  }
  otpverify() {
    let param = {
      user_code: this.otpreactiveFormforogot.value.usercode,
      company_code: this.otpreactiveFormforogot.value.companyid,
      otp_value: this.otpreactiveFormforogot.value.otpcode
    }

    var url = 'Login/otpverifyvcxcontroller';

    this.service.post(url, param).subscribe((result: any) => {

      if (result.status == true) {
       
       localStorage.setItem('token', result.token);
            localStorage.setItem('user_gid', result.user_gid);
            localStorage.setItem('c_code', result.c_code);
             this.router.navigate(['/auth/WelcomePage']);
            this.notificationService.loginNotification();
            this. otpverificationCompleted=true;
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message);
      }

      else {
        this.show = false;
        this.ToastrService.warning(result.message)
      }

    });

  }
  onforgot() {


    if (this.reactiveFormforogot.invalid) {
      // Trigger validation
      this.reactiveFormforogot.markAllAsTouched();
      return;
    }
    let param = {
      usercode: this.reactiveFormforogot.value.usercode,
      companyid: this.reactiveFormforogot.value.companyid,
      forgotportal_emailid: this.reactiveFormforogot.value.forgotportal_emailid,
      confirmpassword_reset: this.reactiveFormforogot.value.confirmpassword_forgot,
      forgot_pwd: this.reactiveFormforogot.value.forgot_pwd

    }
    var url = 'Login/submitforgot';
    this.NgxSpinnerService.show();
    this.service.post(url, param).subscribe((result: any) => {
      this.reactiveFormforogot.reset();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      this.NgxSpinnerService.hide();
       window.location.reload();
    });
  }
  onclose() {
    this.reactiveFormforogot.reset();
    this.reactiveFormforogot.get("companyid")?.setValue(localStorage.getItem('c_code'));
  }
  get passwordMismatch() {
    return this.reactiveFormforogot.errors?.['passwordMismatch'];
  }
  get passwordMismatch1() {
    return this.reactiveFormreset.errors?.['passwordMismatch1'];
  }
  /////////////////////Reset Popup update event//////////////
  onreset() {

    var url = 'Login/UserReset';
    this.NgxSpinnerService.show();
    this.service.post(url, this.reactiveFormreset.value).subscribe((result: any) => {

      if (result.status == false) {
        this.reactiveFormreset.reset();
        this.ToastrService.warning(result.message)
      }
      else {
        this.reactiveFormreset.reset();
        this.ToastrService.success(result.message)
      }
      this.NgxSpinnerService.hide();
      window.location.reload();


    });
  }
  onclosereset() {
    this.reactiveFormreset.reset();
    this.reactiveFormreset.get("companyid_reset")?.setValue(localStorage.getItem('c_code'));
  }
  bind() {
    var url = 'Login/Getemail';
    if (this.reactiveFormforogot.value.companyid != null && this.reactiveFormforogot.value.usercode != null) {
      let param = {
        company_code: this.reactiveFormforogot.value.companyid,
        user_code: this.reactiveFormforogot.value.usercode
      }
      this.service.post(url, param).subscribe((result: any) => {
        this.email = result.email
      });
    }

  }


  verifyforreset(){

    var url = 'Login/resetpasswordcheck';

    let param={
      usercode_reset: this.reactiveFormreset.value.usercode_reset,
      companyid_reset: this.reactiveFormreset.value.companyid_reset,
      old_password: this.reactiveFormreset.value.old_password

    }
    this.NgxSpinnerService.show()
    this.service.post(url,param).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.success(result.message)    
        this.NgxSpinnerService.hide()
        this.otpresetpassword=false;
        this.otpresetbutton=true;
        this. verificationCompleted1=true;

      }

    });


  }
 


  asnpdf() {
    //this.service.msASN_PDF()
  }
}
export function passwordMatchValidator(passwordField: string, confirmPasswordField: string): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const password = control.get(passwordField)?.value;
    const confirmPassword = control.get(confirmPasswordField)?.value;

    if (password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  };
}


export function passwordMatchValidator1(passwordField: string, confirmPasswordField: string): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const password = control.get(passwordField)?.value;
    const confirmPassword = control.get(confirmPasswordField)?.value;

    if (password !== confirmPassword) {
      return { passwordMismatch1: true };
    }
    return null;
  };
}

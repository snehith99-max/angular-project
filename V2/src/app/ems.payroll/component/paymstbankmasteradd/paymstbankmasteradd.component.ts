import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';


@Component({
  selector: 'app-paymstbankmasteradd',
  templateUrl: './paymstbankmasteradd.component.html',
  styleUrls: ['./paymstbankmasteradd.component.scss']
})
export class PaymstbankmasteraddComponent {

  

  reactiveform: FormGroup | any;
  bank_list: any;
  accounttype_list: any;
  accountgroup_list: any;
  branchname_list: any;

  mdlBankName: any;
  mdlAccountType: any;
  mdlAccountGroup: any;
  mdlBranchName:any;

  constructor(private formBuilder: FormBuilder, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    
    this.reactiveform = new FormGroup({
      bank_code: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      bank_name: new FormControl('', Validators.required),
      account_no: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      ifsc_code: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      neft_code: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      swift_code: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      remarks: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      accountgroup_gid: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      accountgroup_name: new FormControl('',Validators.required),
      account_type: new FormControl('',Validators.required),
      openning_balance: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      created_date: new FormControl(''),
    })
  }

  get bank_code() {
    return this.reactiveform.get('bank_code')!;
  }
  get bank_name() {
    return this.reactiveform.get('bank_name')!;
  }
  get branch_name() {
    return this.reactiveform.get('branch_name')!;
  }
  get account_no() {
    return this.reactiveform.get('account_no')!;
  }
  get openning_balance() {
    return this.reactiveform.get('openning_balance')!;
  }

  get account_type() {
    return this.reactiveform.get('account_type')!;
  }
  get accountgroup_name() {
    return this.reactiveform.get('accountgroup_name')!;
  }



  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };

    flatpickr('.date-picker', options);
    
    ////Drop Down///
    var url = 'PayMstBankMaster/GetBankName'
    this.service.get(url).subscribe((result: any) => {
      this.bank_list = result.GetBankName;
    });
    var url = 'PayMstBankMaster/GetAccountType'
    this.service.get(url).subscribe((result: any) => {
      this.accounttype_list = result.GetAccountType;
    });
    var url = 'PayMstBankMaster/GetAccountGroup'
    this.service.get(url).subscribe((result: any) => {
      this.accountgroup_list = result.GetAccountGroup;
    });
    var url = 'PayMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
    });
  }
  ////Submit Function////
  public onsubmit(): void {

    if (this.reactiveform.value.bank_name != null && this.reactiveform.value.bank_name != null) {
      for (const control of Object.keys(this.reactiveform.controls)) {
        this.reactiveform.controls[control].markAsTouched();
      }
      this.reactiveform.value;
      var api = 'PayMstBankMaster/PostBankMaster';
      this.service.post(api, this.reactiveform.value).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning("Error While Adding Bank Master")
          // this.BranchSummary();
        }
        else {
          this.reactiveform.get("bank_code")?.setValue(null);
          this.reactiveform.get("bank_name")?.setValue(null);
          this.reactiveform.get("account_gid")?.setValue(null);
          this.reactiveform.get("account_no")?.setValue(null);
          this.reactiveform.get("ifsc_code")?.setValue(null);
          this.reactiveform.get("neft_code")?.setValue(null);
          this.reactiveform.get("swift_code")?.setValue(null);
          this.reactiveform.get("accountgroup_gid")?.setValue(null);
          this.reactiveform.get("openning_balance")?.setValue(null);
          this.reactiveform.get("branch_gid")?.setValue(null);
          this.reactiveform.get("created_date")?.setValue(null);
          this.reactiveform.get("remarks")?.setValue(null);
          this.ToastrService.success("Bank Master Added Successfully");
          // this.BranchSummary();

        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.router.navigate(['/payroll/PayMstBankmaster'])
    setTimeout(function() {
      window.location.reload();
  }, 3000); // 3000 milliseconds = 3 seconds
  }

  onadd() { }
  redirecttolist() {
    this.router.navigate(['/payroll/PayMstBankmaster'])
  }


}

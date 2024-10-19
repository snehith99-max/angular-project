import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
interface IBankMaster{
  bank_gid:any;
  bank_name:string;
  account_no:string;
  ifsc_code:string;
  neft_code:string;
  swift_code:string;
  remarks:string;
  branch_name:string;
  accountgroup_name:string;
  account_type:string;
  openning_balance:string;
  created_date:string;
}

@Component({
  selector: 'app-paymstbankmasteredit',
  templateUrl: './paymstbankmasteredit.component.html',
  styleUrls: ['./paymstbankmasteredit.component.scss']
})
export class PaymstbankmastereditComponent {

  bankmaster!: IBankMaster;
  reactiveform!: FormGroup;
  bank_list: any;
  accounttype_list: any;
  accountgroup_list: any;
  branchname_list: any;
  bank_gid: any;
  bankmasteredit_list:any;
  bankmasters:any;

  mdlBankName: any;
  mdlAccountType: any;
  mdlAccountGroup: any;


  constructor(private formBuilder: FormBuilder, private route: Router, private ToastrService: ToastrService, public service: SocketService,private router: ActivatedRoute) {
    this.bankmaster = {} as IBankMaster;
  }

  get bank_code() {
    return this.reactiveform.get('bank_code')!;
  }
  get bank_name() {
    return this.reactiveform.get('bank_name')!;
  }
  get accountgroup_name() {
    return this.reactiveform.get('accountgroup_name')!;
  }
  get account_type() {
    return this.reactiveform.get('account_type')!;
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
  get created_date() {
    return this.reactiveform.get('created_date')!;
  }

  ngOnInit(): void {
    debugger
    this.bankmasters= this.router.snapshot.paramMap.get('bank_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.bankmasters,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetBankMasterDetail(deencryptedParam);

    this.reactiveform = new FormGroup({
      bank_gid: new FormControl(''),
      bank_code: new FormControl(''),
      bank_name: new FormControl('', Validators.required),
      account_no: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      ifsc_code: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      neft_code: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      swift_code: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      remarks: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      accountgroup_gid: new FormControl(''),
      account_gid: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      branch_gid: new FormControl(''),
      accountgroup_name: new FormControl('', Validators.required),
      account_type: new FormControl('', Validators.required),
      openning_balance: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      created_date: new FormControl(''),
    })


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

  ////Edit////
  GetBankMasterDetail(bank_gid: any) {
    debugger
    var url='PayMstBankMaster/GetBankMasterDetail'
    let param = {
      bank_gid : bank_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.bankmasteredit_list = result.GetEditBankMaster_list;
      console.log(this.bankmasteredit_list)

    this.reactiveform.get("bank_gid")?.setValue(this.bankmasteredit_list[0].bank_gid);
    this.reactiveform.get("account_gid")?.setValue(this.bankmasteredit_list[0].account_gid);
    this.reactiveform.get("accountgroup_gid")?.setValue(this.bankmasteredit_list[0].accountgroup_gid);
    this.reactiveform.get("branch_gid")?.setValue(this.bankmasteredit_list[0].branch_gid);
    this.reactiveform.get("bank_code")?.setValue(this.bankmasteredit_list[0].bank_code);
    this.reactiveform.get("bank_name")?.setValue(this.bankmasteredit_list[0].bank_name);
    this.reactiveform.get("account_type")?.setValue(this.bankmasteredit_list[0].account_type);
    this.reactiveform.get("account_no")?.setValue(this.bankmasteredit_list[0].account_no);
    this.reactiveform.get("ifsc_code")?.setValue(this.bankmasteredit_list[0].ifsc_code);
    this.reactiveform.get("neft_code")?.setValue(this.bankmasteredit_list[0].neft_code);
    this.reactiveform.get("swift_code")?.setValue(this.bankmasteredit_list[0].swift_code);
    this.reactiveform.get("accountgroup_name")?.setValue(this.bankmasteredit_list[0].accountgroup_name);
    this.reactiveform.get("openning_balance")?.setValue(this.bankmasteredit_list[0].openning_balance);
    this.reactiveform.get("branch_name")?.setValue(this.bankmasteredit_list[0].branch_name);
    this.reactiveform.get("created_date")?.setValue(this.bankmasteredit_list[0].created_date);
    this.reactiveform.get("remarks")?.setValue(this.bankmasteredit_list[0].remarks);
  });
  }  
  public onupdate():void {
    debugger
    console.log(this.reactiveform.value)
    this.bankmaster = this.reactiveform.value;
    if (this.reactiveform.value.bank_name != null && this.reactiveform.value.bank_name != null){
      const api = 'PayMstBankMaster/PostBankMasterUpdate';
      this.service.post(api, this.reactiveform.value).subscribe(
        (result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            this.ToastrService.success(result.message)
            this.route.navigate(['/payroll/PayMstBankmaster']);
          }
        });
      }
   }
   redirecttolist() {
    this.route.navigate(['/payroll/PayMstBankmaster'])
  }


}
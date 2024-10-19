import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
interface IBankMaster{
  bank_gid:any;
  bank_name:string;
  account_no:string;
  ifsc_code:string;
  neft_code:string;
  swift_code:string;
  remarks:string;
  accountgroup_gid:string;
  branch_name:string;
  accountgroup_name:string;
  account_type:string;
  openning_balance:string;
  created_date:string;
}
@Component({
  selector: 'app-acc-mst-bankmaster-edit',
  templateUrl: './acc-mst-bankmaster-edit.component.html',
  styleUrls: ['./acc-mst-bankmaster-edit.component.scss']
})
export class AccMstBankmasterEditComponent {

  bankmaster!: IBankMaster;
  reactiveform!: FormGroup;
  bankmaster_list: any;
  accounttype_list: any;
  accountgroup_list: any;
  branchname_list: any;
  bank_gid: any;
  bankmasteredit_list:any;
  bankmasters:any;
  txtcreated_date:any;
  

  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private route: Router, private ToastrService: ToastrService, public service: SocketService,private router: ActivatedRoute) {
    this.bankmaster = {} as IBankMaster;
  }

  

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options); 
    this.accounttype_list = [
      { id: 'Current Account', value: 'Current Account' },
      { id: 'Savings Account', value: 'Savings Account' },
      { id: 'Recurring Deposit', value: 'Recurring Deposit' },
      { id: 'Fixed Deposit', value: 'Fixed Deposit' },
      { id: 'OD Account', value: 'OD Account' }
    ];
    this.bankmasters= this.router.snapshot.paramMap.get('bank_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.bankmasters,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetBankMasterDetail(deencryptedParam);

    this.reactiveform = new FormGroup({
      bank_gid: new FormControl(''),
      bank_code: new FormControl(''),
      bank_name: new FormControl('', Validators.required),
      account_no: new FormControl('', Validators.required),
      ifsc_code: new FormControl(''),
      neft_code: new FormControl(''),
      swift_code: new FormControl(''),
      remarks: new FormControl(''),
      accountgroup_gid: new FormControl(''),
      account_gid: new FormControl(''),
      branch_name: new FormControl('',Validators.required),
      branch_gid: new FormControl(''),
      accountgroup_name: new FormControl('',Validators.required),
      account_type: new FormControl('',Validators.required),
      openning_balance: new FormControl('',[
        Validators.required,
       // Pattern to allow numbers and decimals (up to 2 decimal places)
      ]),
      created_date: new FormControl(this.getCurrentDate(), Validators.required),
    })


    ////Drop Down///
    // var url = 'AccMstBankMaster/GetAccountType'
    // this.service.get(url).subscribe((result: any) => {
    //   this.accounttype_list = result.GetAccountType;
    // });
    
    var url = 'AccMstBankMaster/GetAccountGroup'
    this.service.get(url).subscribe((result: any) => {
      this.accountgroup_list = result.GetAccountGroup;
    });
    var url = 'AccMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
  
    return dd + '-' + mm + '-' + yyyy;
  }
  ////Edit////
  GetBankMasterDetail(bank_gid: any) {
    //debugger
    var url='AccMstBankMaster/GetBankMasterDetail'
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
    //this.reactiveform.get("created_date")?.setValue(this.bankmasteredit_list[0].created_date);
    this.reactiveform.get("remarks")?.setValue(this.bankmasteredit_list[0].remarks);
    this.txtcreated_date = this.bankmasteredit_list[0].created_date
  });
  }  
  
  get bank_code() {
    return this.reactiveform.get('bank_code')!;
  }
  get bank_name() {
    return this.reactiveform.get('bank_name')!;
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
  get branch_name() {
    return this.reactiveform.get('branch_name')!;
  }
  get accountgroup_name() {
    return this.reactiveform.get('accountgroup_name')!;
  }
  get account_type() {
    return this.reactiveform.get('account_type')!;
  }

  public onupdate():void {
    this.bankmaster = this.reactiveform.value;
    if (this.reactiveform.status =='VALID'){
      this.NgxSpinnerService.show();
      const api = 'AccMstBankMaster/PostBankMasterUpdate';
      this.service.post(api, this.reactiveform.value).subscribe(
        (result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
          }
          else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message)
            this.route.navigate(['/finance/AccMstBankMasterSummary']);
          }
        });
      }
      else {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
   }
   redirecttolist() {
    this.route.navigate(['/finance/AccMstBankMasterSummary'])
  }
  validateNumericInput(event: KeyboardEvent): void {
    const inputChar = String.fromCharCode(event.charCode);
    
    // Allow only numeric input and decimal point
    if (!/[0-9.]/.test(inputChar)) {
      event.preventDefault();
    }
  }
  formatCurrency(event: any): void {
    let input = event.target.value;
    if (input === '') {
      event.target.value = '';
      return;
    }
    const cursorPosition = event.target.selectionStart;
    let cleanedInput = input.replace(/[^0-9.]/g, '');
    const parts = cleanedInput.split('.');
    if (parts.length > 2) {
      cleanedInput = `${parts[0]}.${parts.slice(1).join('')}`;
    }
    const integerPart = parts[0];
    let formattedInteger = parseInt(integerPart, 10).toLocaleString('en-US');
    let formattedInput = formattedInteger;
    if (parts.length > 1) {
      const fractionalPart = parts[1];
      formattedInput += '.' + fractionalPart.slice(0, 2);
    }
    event.target.value = formattedInput;
    const newCursorPosition = cursorPosition + (formattedInput.length - input.length);
    event.target.setSelectionRange(newCursorPosition, newCursorPosition);
  }
}

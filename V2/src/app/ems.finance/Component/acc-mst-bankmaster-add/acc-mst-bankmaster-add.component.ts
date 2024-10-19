import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-acc-mst-bankmaster-add',
  templateUrl: './acc-mst-bankmaster-add.component.html',
  styleUrls: ['./acc-mst-bankmaster-add.component.scss']
})
export class AccMstBankmasterAddComponent {

  reactiveform: FormGroup | any;
  bankmaster_list: any;
  accounttype_list: any;
  accountgroup_list: any;
  branchname_list: any;
  mdlAccountType:any;
  mdlAccountGroup:any;
  mdlBranchName:any;

  constructor(private formBuilder: FormBuilder, private router: Router, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.reactiveform = new FormGroup({
      bank_code: new FormControl(''),
      bank_name: new FormControl('', [
        Validators.required,
        Validators.pattern("(?=.*[a-zA-Z0-9]).+$"),
      ]),
      account_no: new FormControl('', Validators.required),
      ifsc_code: new FormControl('',[Validators.maxLength(11), Validators.minLength(11)]),
      neft_code: new FormControl(''),
      swift_code: new FormControl('',[Validators.maxLength(11), Validators.minLength(11)]),
      remarks: new FormControl(''),
      accountgroup_gid: new FormControl(''),
      branch_name: new FormControl('',Validators.required),
      accountgroup_name: new FormControl('',Validators.required),
      account_type: new FormControl('',Validators.required),
      openning_balance:  new FormControl('', [Validators.required,]),
      created_date: new FormControl(this.getCurrentDate(), Validators.required),
    })
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



  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options); 

    ////Drop Down///
    // var url = 'AccMstBankMaster/GetAccountType'
    // this.service.get(url).subscribe((result: any) => {
    //   this.accounttype_list = result.GetAccountType;
    // });
    this.accounttype_list = [
      { id: 'Current Account', value: 'Current Account' },
      { id: 'Savings Account', value: 'Savings Account' },
      { id: 'Recurring Deposit', value: 'Recurring Deposit' },
      { id: 'Fixed Deposit', value: 'Fixed Deposit' },
      { id: 'OD Account', value: 'OD Account' }
    ];
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
  ////Submit Function////
  public onsubmit(): void {

    if (this.reactiveform.status == 'VALID') {
      this.NgxSpinnerService.show();
      var api = 'AccMstBankMaster/PostBankMaster';
      this.service.post(api, this.reactiveform.value).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          // this.BranchSummary();
        }
        else{
          this.router.navigate(['/finance/AccMstBankMasterSummary'])
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
          // this.BranchSummary();

        }

      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  onadd() { }
  redirecttolist() {
    this.router.navigate(['/finance/AccMstBankMasterSummary'])
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

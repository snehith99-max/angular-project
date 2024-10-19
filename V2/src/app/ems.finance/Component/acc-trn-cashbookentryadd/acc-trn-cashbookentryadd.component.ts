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

@Component({
  selector: 'app-acc-trn-cashbookentryadd',
  templateUrl: './acc-trn-cashbookentryadd.component.html',
  styleUrls: ['./acc-trn-cashbookentryadd.component.scss']
})
export class AccTrnCashbookentryaddComponent {
  CashBookEntryForm: FormGroup | any;
  branch_gid: any;
  lsbranch_gid: any;
  finyear_gid:any;
  responsedata:any;
  CashBookEntryView_List:any;
  CashBookEnterBy_List:any;
  AccountGroup_List: any;
  Account_List: any;
  cashbookmultipleform: FormGroup | any;
  CashBookMulAddDeleteForm: FormGroup | any;
  CashAccountMulAdd_List:any;
  parameterValue1:any;
  showOptionsDivId:any;

  constructor(public service: SocketService, private router: ActivatedRoute,private route: Router, private FormBuilder: FormBuilder,private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService) {
   
  }

 
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options); 
    const branch_gid = this.router.snapshot.paramMap.get('branch_gid');
    this.branch_gid = branch_gid;
   
    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.branch_gid, secretKey).toString(enc.Utf8);
    this.lsbranch_gid = deencryptedParam;

    const finyear_gid = this.router.snapshot.paramMap.get('finyear_gid');
    this.finyear_gid = finyear_gid;

    this.NgxSpinnerService.show();
    var url = 'AccTrnCashBookSummary/GetCashBookEntryView'
    let param = {
      branch_gid: this.lsbranch_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.CashBookEntryView_List = result.GetCashBookEntryView_List;
      this.NgxSpinnerService.hide();
    });

    this.NgxSpinnerService.show();
    var url = 'AccTrnCashBookSummary/GetCashBookEnterBy'    
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.CashBookEnterBy_List = result.GetCashBookEnterBy_List;
      this.NgxSpinnerService.hide();
    });

    var url = 'AccTrnBankbooksummary/GetAccountGroupList'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.AccountGroup_List = result.GetAccountGroup_List;
    });

    var url = 'AccTrnBankbooksummary/GetAccountNameList'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Account_List = result.GetAccount_List;
    });

    this.cashbookmultipleform = new FormGroup({
      accountgroup_name: new FormControl(null),
      account_name: new FormControl(null, Validators.required),
      transaction_type: new FormControl(null),
      transaction_amount: new FormControl(null, Validators.required),
      txtremarks: new FormControl(null),
    });

    this.CashBookMulAddDeleteForm = new FormGroup({
      session_id: new FormControl(null),
    });

    this.CashBookEntryForm = new FormGroup({
      acct_refno: new FormControl(null,Validators.required),
      transaction_date: new FormControl(null, Validators.required),
      direct_remarks: new FormControl(null),
      branch_gid: new FormControl(this.lsbranch_gid),
    });

    this.AccountMultpleAddSummary();

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

  }

  AccountMultpleAddSummary() {
    this.NgxSpinnerService.show();
    var url = 'AccTrnCashBookSummary/GetCashAccountMulAddDtl'
    debugger    
    this.service.get(url).subscribe((result: any) => {
      $('#CashAccountMulAdd_List').DataTable().destroy();
      this.responsedata = result;
      this.CashAccountMulAdd_List = this.responsedata.GetCashAccountMulAdd_List;
      this.NgxSpinnerService.hide();      
    });
  }

  get account_name() {
    return this.cashbookmultipleform.get('account_name')!;
  }
  get transaction_amount() {
    return this.cashbookmultipleform.get('transaction_amount')!;
  }

  get acct_refno() {
    return this.CashBookEntryForm.get('acct_refno')!;
  }
  get transaction_date() {
    return this.CashBookEntryForm.get('transaction_date')!;
  }

  transactiontypelist = [
    { trans_type: 'Deposit' },
    { trans_type: 'Withdraw' }
  ]

  submit() {
    this.cashbookmultipleform.value;
    if (this.cashbookmultipleform.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'AccTrnCashBookSummary/PostAccountMulAddDtls';
      this.service.post(url, this.cashbookmultipleform.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.cashbookmultipleform.get("accountgroup_name")?.setValue(null);
          this.cashbookmultipleform.get("account_name")?.setValue(null);
          this.cashbookmultipleform.get("transaction_type")?.setValue(null);
          this.cashbookmultipleform.get("transaction_amount")?.setValue(null);
          this.cashbookmultipleform.get("txtremarks")?.setValue(null);
          this.AccountMultpleAddSummary();
          this.cashbookmultipleform.reset();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.cashbookmultipleform.get("accountgroup_name")?.setValue(null);
          this.cashbookmultipleform.get("account_name")?.setValue(null);
          this.cashbookmultipleform.get("transaction_type")?.setValue(null);
          this.cashbookmultipleform.get("transaction_amount")?.setValue(null);
          this.cashbookmultipleform.get("txtremarks")?.setValue(null);
          this.AccountMultpleAddSummary();
          this.cashbookmultipleform.reset();
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else { }

  }
  
  delete(parameter: any) {
    this.parameterValue1 = parameter
    console.log(this.parameterValue1,'this.parameterValue1bank');
    this.CashBookMulAddDeleteForm.get("session_id")?.setValue(this.parameterValue1.session_id);
  }

  submit_delete() {   
    this.CashBookMulAddDeleteForm.value;     
    let param = {
      session_id: this.CashBookMulAddDeleteForm.value.session_id
    }  
      this.NgxSpinnerService.show();
      var url = 'AccTrnCashBookSummary/GetDeleteMulBankDtls'
      this.service.getparams(url, param).subscribe((result: any) => {       
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.AccountMultpleAddSummary();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.AccountMultpleAddSummary();
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
  }

  submit_cashbookbook() {
    this.CashBookEntryForm.value;
    if (this.CashBookEntryForm.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'AccTrnCashBookSummary/PostDirectCashBookEntry';
      this.service.post(url, this.CashBookEntryForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.CashBookEntryForm.get("acct_refno")?.setValue(null);
          this.CashBookEntryForm.get("transaction_date")?.setValue(null);
          this.AccountMultpleAddSummary();
          this.CashBookEntryForm.reset();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          const secretKey = 'storyboarderp';
          const branch_gid = AES.encrypt(this.lsbranch_gid, secretKey).toString();
          const finyear_gid = this.finyear_gid;
          this.route.navigate(['/finance/AccTrnCashbookSelect', branch_gid, finyear_gid]);
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else { }

  }


  back(){   
    const secretKey = 'storyboarderp';
    const branch_gid = AES.encrypt(this.lsbranch_gid, secretKey).toString();
    const finyear_gid = this.finyear_gid;
    this.route.navigate(['/finance/AccTrnCashbookSelect',branch_gid,finyear_gid]);
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }
  
}

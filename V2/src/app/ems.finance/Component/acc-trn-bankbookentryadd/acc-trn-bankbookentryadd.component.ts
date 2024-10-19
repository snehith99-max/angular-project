import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from 'jquery';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';

@Component({
  selector: 'app-acc-trn-bankbookentryadd',
  templateUrl: './acc-trn-bankbookentryadd.component.html',
  styleUrls: ['./acc-trn-bankbookentryadd.component.scss']
})
export class AccTrnBankbookentryaddComponent {
  BankBookEntryForm!: FormGroup;
  bankbookmultipleform: FormGroup | any;
  BankBookMulAddDeleteForm: FormGroup | any;
  bank_gid: any;
  lsbank_gid: any;
  finyear_gid: any;
  responsedata: any;
  BankBookEntryView_List: any;
  AccountGroup_List: any;
  Account_List: any;
  AccountMulAdd_List: any[] = [];
  parameterValue1: any;
  showOptionsDivId:any;

  bankbook_list: any;
  GetAccountNameDropdown: any;
  Getledgername_List: any;
  
  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService,private Location: Location) {
   
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    const bank_gid = this.router.snapshot.paramMap.get('bank_gid');
    this.bank_gid = bank_gid;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.bank_gid, secretKey).toString(enc.Utf8);
    this.lsbank_gid = deencryptedParam;

    const finyear_gid = this.router.snapshot.paramMap.get('finyear_gid');
    this.finyear_gid = finyear_gid;

    this.BankBookEntryForm = new FormGroup({
      acct_refno: new FormControl(null,Validators.required),
      transaction_date: new FormControl(null, Validators.required),
      direct_remarks: new FormControl(null),
      bank_gid: new FormControl(this.lsbank_gid),
    });

    this.bankbookmultipleform = new FormGroup({
      accountgroup_name: new FormControl(null),
      account_name: new FormControl(null, Validators.required),
      transaction_type: new FormControl(null),
      transaction_amount: new FormControl(null, Validators.required),
      txtremarks: new FormControl(null),
    });

    this.BankBookMulAddDeleteForm = new FormGroup({
      session_id: new FormControl(null),
    });

    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetBankBookEntryView'
    let param = {
      bank_gid: this.lsbank_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.BankBookEntryView_List = result.GetBankBookEntryView_List;
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

    var url = 'AccTrnBankbooksummary/GetAccountGroupNameDropdown'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.GetAccountNameDropdown = result.GetAccountNameDropdown;
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
    var url = 'AccTrnBankbooksummary/GetAccountMulAddDtl'   
    this.service.get(url).subscribe((result: any) => {
      $('#AccountMulAdd_List').DataTable().destroy();
      this.responsedata = result;
      this.AccountMulAdd_List = this.responsedata.GetAccountMulAdd_List;
      this.NgxSpinnerService.hide();      
    });
  }

  get account_name() {
    return this.bankbookmultipleform.get('account_name')!;
  }
  get transaction_amount() {
    return this.bankbookmultipleform.get('transaction_amount')!;
  }

  get acct_refno() {
    return this.BankBookEntryForm.get('acct_refno')!;
  }
  get transaction_date() {
    return this.BankBookEntryForm.get('transaction_date')!;
  }
  
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }
  
  transactiontypelist = [
    { trans_type: 'Deposit' },
    { trans_type: 'Withdraw' }
  ]

  submit() {
    this.bankbookmultipleform.value;
    if (this.bankbookmultipleform.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'AccTrnBankbooksummary/PostAccountMulAddDtls';
      this.service.post(url, this.bankbookmultipleform.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.bankbookmultipleform.get("accountgroup_name")?.setValue(null);
          this.bankbookmultipleform.get("account_name")?.setValue(null);
          this.bankbookmultipleform.get("transaction_type")?.setValue(null);
          this.bankbookmultipleform.get("transaction_amount")?.setValue(null);
          this.bankbookmultipleform.get("txtremarks")?.setValue(null);
          this.AccountMultpleAddSummary();
          this.bankbookmultipleform.reset();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.bankbookmultipleform.get("accountgroup_name")?.setValue(null);
          this.bankbookmultipleform.get("account_name")?.setValue(null);
          this.bankbookmultipleform.get("transaction_type")?.setValue(null);
          this.bankbookmultipleform.get("transaction_amount")?.setValue(null);
          this.bankbookmultipleform.get("txtremarks")?.setValue(null);
          this.AccountMultpleAddSummary();
          this.bankbookmultipleform.reset();
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
    this.BankBookMulAddDeleteForm.get("session_id")?.setValue(this.parameterValue1.session_id);
  }

  submit_delete() {   
    this.BankBookMulAddDeleteForm.value;     
    let param = {
      session_id: this.BankBookMulAddDeleteForm.value.session_id
    }  
      this.NgxSpinnerService.show();
      var url = 'AccTrnBankbooksummary/GetDeleteMulBankDtls'
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

  submit_bankbook() {
    this.BankBookEntryForm.value;
    if (this.BankBookEntryForm.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'AccTrnBankbooksummary/PostDirectBankBookEntry';
      this.service.post(url, this.BankBookEntryForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.BankBookEntryForm.get("acct_refno")?.setValue(null);
          this.BankBookEntryForm.get("transaction_date")?.setValue(null);
          this.AccountMultpleAddSummary();
          this.BankBookEntryForm.reset();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.Location.back()
          this.allbankbook()
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else { }

  }

  allbankbook() {
    var url = 'AccTrnBankbooksummary/GetBankBookSummary'
      this.service.get(url).subscribe((result: any) => {
        $('#bankbook_list').DataTable().destroy();
        this.responsedata = result;
        this.bankbook_list = this.responsedata.Getbankbook_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#bankbook_list').DataTable(
            {
              // code by snehith for customized pagination
              "pageLength": 50, // Number of rows to display per page
              "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
            }
          );
        }, 1);
      });
  }

  back() {
      this.Location.back()
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }
  Select_Ledger(){
    let Subgroup_gid = this.bankbookmultipleform.get("accountgroup_name")?.value;
    let param = {
      Subgroup_gid : Subgroup_gid
    }
    var url = 'AccTrnBankbooksummary/GetLedgerNameDropDownList'
    this.service.getparams(url,param).subscribe((result: any) =>{
      this.Getledgername_List = result.Getledgername_List;
    });
  }
}

import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { data } from 'jquery';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-mst-creditcardmaster-summary',
  templateUrl: './acc-mst-creditcardmaster-summary.component.html',
  styleUrls: ['./acc-mst-creditcardmaster-summary.component.scss'],
  styles: [`
table thead th, 
.table tbody td { 
 position: relative; 
z-index: 0;
} 
.table thead th:last-child, 

.table tbody td:last-child { 
 position: sticky; 

right: 0; 
 z-index: 0; 

} 
.table td:last-child, 

.table th:last-child { 

padding-right: 50px; 

} 
.table.table-striped tbody tr:nth-child(odd) td:last-child { 

 background-color: #ffffff; 
  
  } 
  .table.table-striped tbody tr:nth-child(even) td:last-child { 
   background-color: #f2fafd; 

} 
`]
})
export class AccMstCreditcardmasterSummaryComponent {
  CreditCardForm: FormGroup | any;
  CreditCardEditForm: FormGroup | any;
  branchname_list: any;
  acctgroupname_List: any;
  responsedata:any;
  creditcard_List: any[] = [];
  parameterValue1: any;
  editbank_code: any;
  editopening_balance: any;
  parameterValue: any;
  showOptionsDivId:any;
  mdlDesignation: any;

  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService) {
  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',

    };
    flatpickr('.date-picker', options);
   
    this.CreditCardForm = new FormGroup({
      bank_code: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      bank_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      cardholder_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      card_number: new FormControl('', [Validators.required]),
      opening_balance: new FormControl(null, Validators.required),
      branch_name: new FormControl(null, Validators.required),
      account_group: new FormControl(null, Validators.required),
      transaction_type: new FormControl(null, Validators.required),
      date_value: new FormControl(this.getCurrentDate(), Validators.required),
      remarks: new FormControl(null)
    });

    this.CreditCardEditForm = new FormGroup({
      editbank_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      editcardholder_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      editcard_number: new FormControl('', [Validators.required]),
      default_account: new FormControl(null),
      bank_gid: new FormControl(null),
    });

    var url = 'AccMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
    });

    var url = 'AccMstCreditCard/GetAccountGroupName'
    this.service.get(url).subscribe((result: any) => {
      this.acctgroupname_List = result.Getacctgroupname_List;
    });

    this.creditcardsummary();

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  creditcardsummary() {
    this.NgxSpinnerService.show();
    var url = 'AccMstCreditCard/GetCreditCardSummary'
    debugger    
    this.service.get(url).subscribe((result: any) => {
      $('#creditcard_List').DataTable().destroy();
      this.responsedata = result;
      this.creditcard_List = this.responsedata.Getcreditcard_List;
      //console.log(this.creditcard_List)
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#creditcard_List').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1); 
    });
  }

  get bank_code() {
    return this.CreditCardForm.get('bank_code')!;
  }
  get bank_name() {
    return this.CreditCardForm.get('bank_name')!;
  }
  get cardholder_name() {
    return this.CreditCardForm.get('cardholder_name')!;
  }
  get card_number() {
    return this.CreditCardForm.get('card_number')!;
  }
  get opening_balance() {
    return this.CreditCardForm.get('opening_balance')!;
  }
  get branch_name() {
    return this.CreditCardForm.get('branch_name')!;
  }
  get account_group() {
    return this.CreditCardForm.get('account_group')!;
  }
  get transaction_type() {
    return this.CreditCardForm.get('transaction_type')!;
  }
  get date_value() {
    return this.CreditCardForm.get('date_value')!;
  }
  get editcard_number() {
    return this.CreditCardEditForm.get('editcard_number')!;
  }
  get editcardholder_name() {
    return this.CreditCardEditForm.get('editcardholder_name')!;
  }
  get editbank_name() {
    return this.CreditCardEditForm.get('editbank_name')!;
  }

  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }

  submit() {
    this.CreditCardForm.value;
    if (this.CreditCardForm.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'AccMstCreditCard/PostCreditCardDetails';
      this.service.post(url, this.CreditCardForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.CreditCardForm.get("bank_code")?.setValue(null);
          this.CreditCardForm.get("bank_name")?.setValue(null);
          this.CreditCardForm.get("cardholder_name")?.setValue(null);
          this.CreditCardForm.get("card_number")?.setValue(null);
          this.CreditCardForm.get("opening_balance")?.setValue(null);
          this.CreditCardForm.get("branch_name")?.setValue(null);
          this.CreditCardForm.get("account_group")?.setValue(null);
          this.CreditCardForm.get("transaction_type")?.setValue(null);
          this.CreditCardForm.get("date_value")?.setValue(null);
          this.CreditCardForm.get("remarks")?.setValue(null);
          this.CreditCardForm.reset();
          this.creditcardsummary();
          this.getCurrentDate();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.CreditCardForm.get("bank_code")?.setValue(null);
          this.CreditCardForm.get("bank_name")?.setValue(null);
          this.CreditCardForm.get("cardholder_name")?.setValue(null);
          this.CreditCardForm.get("card_number")?.setValue(null);
          this.CreditCardForm.get("opening_balance")?.setValue(null);
          this.CreditCardForm.get("branch_name")?.setValue(null);
          this.CreditCardForm.get("account_group")?.setValue(null);
          this.CreditCardForm.get("transaction_type")?.setValue(null);
          this.CreditCardForm.get("date_value")?.setValue(null);
          this.CreditCardForm.get("remarks")?.setValue(null);
          this.CreditCardForm.reset();
          this.creditcardsummary();
          this.getCurrentDate();
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else { }

  }

  edit(parameter: any) {
    this.parameterValue1 = parameter
    console.log(this.parameterValue1,'this.parameterValue1credit');
    this.editbank_code =this.parameterValue1.bank_code;
    this.CreditCardEditForm.get("editbank_name")?.setValue(this.parameterValue1.bank_name);
    this.CreditCardEditForm.get("editcardholder_name")?.setValue(this.parameterValue1.cardholder_name);
    this.CreditCardEditForm.get("editcard_number")?.setValue(this.parameterValue1.creditcard_no);
    this.editopening_balance =this.parameterValue1.openning_balance;
    this.CreditCardEditForm.get("bank_gid")?.setValue(this.parameterValue1.bank_gid);
    this.CreditCardEditForm.get("default_account")?.setValue(this.parameterValue1.default_flag);
  }

  update() {   
    this.CreditCardEditForm.value;   
      this.NgxSpinnerService.show();
      var url = 'AccMstCreditCard/Getupdatecreditcarddtls';
      this.service.post(url, this.CreditCardEditForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.CreditCardEditForm.get("bank_name")?.setValue(null);
          this.CreditCardEditForm.get("cardholder_name")?.setValue(null);
          this.CreditCardEditForm.get("card_number")?.setValue(null);
          this.CreditCardEditForm.get("default_account")?.setValue(null);
          this.CreditCardEditForm.reset();
          this.creditcardsummary();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.CreditCardEditForm.get("bank_name")?.setValue(null);
          this.CreditCardEditForm.get("cardholder_name")?.setValue(null);
          this.CreditCardEditForm.get("card_number")?.setValue(null);
          this.CreditCardEditForm.get("default_account")?.setValue(null);
          this.CreditCardEditForm.reset();
          this.creditcardsummary();
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
  }

  transactiontypelist = [
    { transaction_type: 'Debit' },
    { transaction_type: 'Credit' }
  ]

  openModalinactive(parameter: string){
    this.parameterValue = parameter
  }
  oninactive(){
    //console.log(this.parameterValue);
    let param = {
      status_flag:"Y",
      bank_gid: this.parameterValue,
    }
      var url3 = 'AccMstCreditCard/PostCreditCardStatus'
      this.service.getparams(url3, param).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning(result.message)
        }
        else {
          this.creditcardsummary();
         this.ToastrService.success(result.message)
          }
       
      });
  }
  
  openModalactive(parameter: string){
    this.parameterValue = parameter
  }
  
  onactive(){
    //console.log(this.parameterValue);
    let param = {
      status_flag:"N",
      bank_gid: this.parameterValue,
    }
      var url3 = 'AccMstCreditCard/PostCreditCardStatus'
      this.service.getparams(url3, param).subscribe((result: any) => {
  
        if ( result.status == false) {
          this.ToastrService.warning(result.message)
         }
         else {
          this.creditcardsummary();
          this.ToastrService.success(result.message)
           }
      
      });
  }
  
  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }

  onclose(){
    this.CreditCardForm.reset();
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

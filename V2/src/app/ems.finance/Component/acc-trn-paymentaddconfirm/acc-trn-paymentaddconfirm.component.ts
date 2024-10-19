

import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SelectionModel } from '@angular/cdk/collections';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';


interface ISinglePaymentReport {
  payment_date: string;
  entered_by: string;
  priority: string;
  textbox: string;
  email: string;
  contact_no: string;
  vendor_name: string;
  vendor_contact: string;
  vendor_address: string;
  fax: string;
  currency: string;
  exchange_rate: string;
  payment_remarks: string;
  payment_note: string;
  payment_mode: string;
  bank_name: string;
  cheque_no: string;
  branch_name: string;
  transaction_refno: string;
  transaction_no: string;
  card_name: string;
  dd_no: string;
}

@Component({
  selector: 'app-acc-trn-paymentaddconfirm',
  templateUrl: './acc-trn-paymentaddconfirm.component.html',
  styleUrls: ['./acc-trn-paymentaddconfirm.component.scss']
})
export class AccTrnPaymentaddconfirmComponent {


  // showInput: boolean = false;
   showInput1: boolean = false;
  // showInput2: boolean = false;
  // showInput3: boolean = false;
  // showInput4: boolean = false;
  // showInput5: boolean = false;
  selectedPaymentMode = '';
  showInput = false;
  showInput2 = false;
  showInput3 = false;
  showInput4 = false;
  showInput5 = false;
  reactiveForm!: FormGroup;
  expense_gid: any;
  selection = new SelectionModel<ISinglePaymentReport>(true, []);
  employeelist: any[] = [];
  singlepayment_list: any[] = [];
  singlepayment_list1: any[] = [];
  bankdetailslist: any[] = [];
  carddetailslist: any[] = [];
  vendor_lists: any[] = [];
  SinglePaymentReport!: ISinglePaymentReport;
  responsedata: any;
  singlepayment_list4: any[] = [];

  bank_name1:any;
  branch_namedd:any;
  branchche:any;
  cardname:any;
  name:any;
  vendor_companyname:any;
  vendorcontactdetails:any;
  email_id:any;
  contact_telephonenumber:any;
  vendoraddress:any;
  exchange_rate:any;
  currency_code:any;
  Vendor_details:any;
  branchnameneft:any;
  vendor_gid_key:any;
  expense_gid_key:any;


  constructor(private formBuilder: FormBuilder,
    private spinnerService: NgxSpinnerService,
    private route: ActivatedRoute,
    private router: Router,
    private ToastrService: ToastrService,
    public service: SocketService,
    public NgxSpinnerService:NgxSpinnerService

  ) {
    this.SinglePaymentReport = {} as ISinglePaymentReport;
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    this.reactiveForm = new FormGroup({
      payment_date: new FormControl(''),
      payment_date1: new FormControl(''),
      priority: new FormControl(''),
      textbox: new FormControl(''),
      payment_remarks: new FormControl(''),
      payment_note: new FormControl(''),
      payment_mode: new FormControl(''),
      bank_name: new FormControl(''),
      bank_gid: new FormControl(''),
      cheque_no: new FormControl(''),
      branch_name: new FormControl(''),
      transaction_refno: new FormControl(''),
      transaction_no: new FormControl(''),
      card_name: new FormControl(''),
      dd_no: new FormControl(''),
      advance: new FormControl(''),
      payment_amount: new FormControl(''),
      balancepo_advance: new FormControl(''),
      grand_total: new FormControl(''),
      tds_amount: new FormControl(''),
      final_amount: new FormControl(''),
      remark: new FormControl(''),
      totalpo_advance: new FormControl(''),
      contact_telephonenumber : new FormControl(''),
      vendorcontactdetails: new FormControl(''),
      vendor_companyname: new FormControl(''),
      email_id: new FormControl(''),
      vendoraddress: new FormControl(''),
      currency_code: new FormControl(''),
      name: new FormControl(''),
      exchange_rate: new FormControl(''),
      vendor_gid: new FormControl(''),
      singlepayment_list: this.formBuilder.array([])
    });
debugger

    const expense_gid = this.route.snapshot.paramMap.get('expense_gid');
  const vendor_gid = this.route.snapshot.paramMap.get('vendor_gid');
  const secretKey = 'storyboarderp';

  this.expense_gid_key = expense_gid;
  this.vendor_gid_key = vendor_gid;
  const expense_gid1 = AES.decrypt(this.expense_gid_key, secretKey).toString(enc.Utf8);
  const vendor_gid1 = AES.decrypt(this.vendor_gid_key, secretKey).toString(enc.Utf8);;
   this.GetsingleSummary(expense_gid1,vendor_gid1);
    debugger
    this.reactiveForm.get('vendor_gid')?.setValue(vendor_gid1)
    var api = 'AccTrnPayment/GetBankDetail'
    this.service.get(api).subscribe((result: any) => {
      this.bankdetailslist = result.GetBankNameVle;
    });

    var api = 'AccTrnPayment/GetCardDetail'
    this.service.get(api).subscribe((result: any) => {
      this.carddetailslist = result.GetCardNameVle;
    });

    
  }

  GetsingleSummary(expense_gid:any,vendor_gid: any) {
    debugger
    var param = {
        vendor_gid: vendor_gid
    };
    var url = 'AccTrnPayment/Getmultipleexpense2employeedtl';
    this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.vendor_lists = this.responsedata.Getmultipleexpense2employeedtl;
        this.name = this.vendor_lists[0].name;
        this.vendor_companyname = this.vendor_lists[0].vendor_companyname;
        this.vendoraddress = this.vendor_lists[0].vendoraddress;
        this.vendorcontactdetails = this.vendor_lists[0].vendorcontactdetails;
        this.email_id = this.vendor_lists[0].email_id;
        this.currency_code = this.vendor_lists[0].currency_code;
        this.exchange_rate = this.vendor_lists[0].exchange_rate;

        // Concatenate vendor contact details and email ID with newlines
        this.Vendor_details = `${this.vendorcontactdetails}\n${this.email_id}`;
    });

    var param1= {
      expense_gid: expense_gid
  };
  debugger
    var url = 'AccTrnPayment/GetSingleaddPaymentSummary'
    this.NgxSpinnerService.show();
    this.service.getparams(url, param1).subscribe((result: any) => {
      this.responsedata = result;
      this.singlepayment_list = this.responsedata.singleexpenselist;
      this.NgxSpinnerService.hide();
      // this.reactiveForm.get("advance")?.setValue(result.GetBranch[0].advance_amount);
      // // this.reactiveForm.get("payment_amount")?.setValue(result.GetBranch[0].payed_amount);
      // this.reactiveForm.get("remark")?.setValue(result.GetBranch[0].expense_remarks);
      for (let i = 0; i < this.singlepayment_list.length; i++) {
        this.reactiveForm.addControl(`advance${i}`, new FormControl(this.singlepayment_list[i].advance));
        this.reactiveForm.addControl(`payment_amount_${i}`, new FormControl(this.singlepayment_list[i].payment_amount));
        this.reactiveForm.addControl(`balancepo_advance_${i}`, new FormControl(this.singlepayment_list[i].balancepo_advance));
        this.reactiveForm.addControl(`totalpo_advance_${i}`, new FormControl(this.singlepayment_list[i].totalpo_advance));
        this.reactiveForm.addControl(`grand_total_${i}`, new FormControl(this.singlepayment_list[i].grand_total));
        this.reactiveForm.addControl(`tds_amount_${i}`, new FormControl(this.singlepayment_list[i].tds_amount));
        this.reactiveForm.addControl(`final_amount_${i}`, new FormControl(this.singlepayment_list[i].final_amount));
        this.reactiveForm.addControl(`remark_${i}`, new FormControl(this.singlepayment_list[i].remark));
      }
     
    });
    const currentDate = new Date().toISOString().split('T')[0];
    this.reactiveForm.get('payment_date')?.setValue(currentDate);
  }

  prodtotalcal(i: any) {
    debugger;
    // const payment_amount = parseFloat(this.singlepayment_list[i].payment_amount) || 0;
    // const tds_amount = parseFloat(this.singlepayment_list[i].tds_amount);
    // const grand_total = payment_amount;
    // const final_amount = !isNaN(tds_amount) ? (grand_total + tds_amount) : grand_total;
    // this.singlepayment_list[i].grand_total = grand_total.toFixed(2);
    // this.singlepayment_list[i].final_amount = final_amount.toFixed(2);
    const payment_amount_str: string = this.singlepayment_list[i].payment_amount ? this.singlepayment_list[i].payment_amount.replace(/,/g, '') : '0'; // Remove commas
    const payment_amount: number = parseFloat(payment_amount_str) || 0;
    const tds_amount_str: string = this.singlepayment_list[i].tds_amount ? this.singlepayment_list[i].tds_amount.replace(/,/g, '') : '0'; // Ensure it's a string or '0'
    const tds_amount: number = parseFloat(tds_amount_str) || 0;
    const grand_total: number = payment_amount;
    const final_amount: number = grand_total + tds_amount;

    this.singlepayment_list[i].grand_total = grand_total.toFixed(2);
    this.singlepayment_list[i].final_amount = final_amount.toFixed(2);
  }
  onKeyPress(event: any) {
    // Get the pressed key
    const key = event.key;

    if (!/^[0-9.]$/.test(key)) {
      // If not a number or dot, prevent the default action (key input)
      event.preventDefault();
    }
  }
  get bank_name() {
    return this.reactiveForm.get('bank_name')!;
  }
  get card_name() {
    return this.reactiveForm.get('card_name')!;
  }
  get payment_mode() {
    return this.reactiveForm.get('payment_mode')!;
  }


  onsubmit() {
  
    debugger;
    if (this.reactiveForm.value.payment_mode == "" || this.reactiveForm.value.payment_mode == null || this.reactiveForm.value.payment_mode == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Payment mode!');
      return
    }
    this.NgxSpinnerService.show();
    var api = 'AccTrnPayment/Postsinglepayment';
    const formValues = this.reactiveForm.value; 
    const params = {
      Getmultipleexpense2employeedtl: this.vendor_lists,
      GetMultipleexpenseSummary: this.singlepayment_list,
      bankname:this.reactiveForm.value.bank_name,
      cheque_no:this.reactiveForm.value.cheque_no,
      branch_name:this.reactiveForm.value.branch_name,
      payment_date1:this.reactiveForm.value.payment_date,
      dd_no:this.reactiveForm.value.dd_no,
      card_name:this.reactiveForm.value.card_name,
      transaction_no:this.reactiveForm.value.transaction_no,
      payment_mode:this.reactiveForm.value.payment_mode,
      payment_remarks:this.reactiveForm.value.payment_remarks,
      payment_note:this.reactiveForm.value.payment_note,
      vendor_gid:this.reactiveForm.value.vendor_gid,
    };
    debugger;

    this.service.postparams(api, params).subscribe((result: any) => {
      if (result.status === false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        
        this.ToastrService.warning(result.message);
      } else {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message);
        this.router.navigate(['/finance/AccTrnPaymentSummary'])
      }
    
    });
  }
   

  showTextBox(selectedValue: string) {
    this.showInput = selectedValue === 'Cheque';
    this.showInput2 = selectedValue === 'DD';
    this.showInput3 = selectedValue === 'Credit Card';
    this.showInput4 = selectedValue === 'NEFT';
    this.showInput5 = selectedValue === 'Cash';
  }

  showTextBox2(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput1 = target.value === 'High';
  }

 
 }

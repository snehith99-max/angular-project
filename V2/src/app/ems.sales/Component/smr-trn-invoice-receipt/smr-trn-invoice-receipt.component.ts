import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';

interface ISinglePaymentReport {
  payment_date: string;
  cheque_date:string;
  neft_date:string;
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
  selector: 'app-smr-trn-invoice-receipt',
  templateUrl: './smr-trn-invoice-receipt.component.html',
  styleUrls: ['./smr-trn-invoice-receipt.component.scss']
})
export class SmrTrnInvoiceReceiptComponent implements OnInit {
  showInput: boolean = false;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInput3: boolean = false;
  showInput4: boolean = false;
  showInput5: boolean = false;

  customer_gid: any;
  addgrnform!: FormGroup;
  productform!: FormGroup;
  grn_lists: any;
  mdlBranchName:any;
  branch_list: any;
  tableData: any[] = [];
  summary_list: any[] = [];
  invoicereceiptsummary_list: any[] = [];
  bankdetailslist: any[] = [];
  productgroup_list: any[] = [];
  userlist: any[] = [];
  summary_list1: any[] = [];
  file!: File;
  pick: Array<any> = [];
  invoicegid: any;
  qtyReceivedAs: string = '';
  parameter: any;
  parameterValue1: any;
  singlepayment_list: any[] = [];
  singlepayment_list1: any[] = [];
  singlepayment_list4: any[] = [];
  carddetailslist: any[] = [];
  assignvisitlist: any[] = [];
  responsedata: any;
  IGrn: any;
  purchaseorder_list: any;
  invoicereceipt_list :any;
  payment_days: any;
  delivery_days: any;
  total_tax: any;
  discount_amount: any;
  addon_amount: any;
  freight_charges: any;
  packing_charges:any;
  roundoff:any;
  insurance_charges:any;
  total_discount_amount:any;
  selection = new SelectionModel<ISinglePaymentReport>(true, []);
  SinglePaymentReport!: ISinglePaymentReport;
  grand_total: any;
  txtshipto:any;
  currency_code: any;
  customer_address:any;
  branch_name:any;
  customer_details:any;
  customer_email:any;
  exchange_rate:any;
  customer_contactnumber:any;
  customer_name:any;
  salesorder_gid : any;
  totalPaymentAmount: number = 0;
  receive_amount: number = 0;
  tdsreceivale_amount:number=0;
  total_amount: any;
  exchange_gain:any;
  exchange_loss:any;
  branchnameneft:any;
  foreigncurrencycode:any;
  defaultcurrencycode:any;
  cardname:any;
  branch_namedd:any;
  branchche:any;
  branch:any;
  cusbank_name:any;
  payment_mode:any;
  bank_name1:any;
  trancsaction_no:any;

  paymentModes = [
    { label: '--Select an option--', value: '' },
    { label: 'Cash', value: 'Cash' },
    { label: 'Cheque', value: 'Cheque' },
    { label: 'Credit Card', value: 'CREDITCARD' },

    { label: 'DD', value: 'DD' },
    { label: 'NEFT', value: 'NEFT' },
    { label: 'Advance Receipt', value: 'Advance Receipt' }
  ];
  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private FormBuilder: FormBuilder, private router: ActivatedRoute,public NgxSpinnerService:NgxSpinnerService)
  {
    this.SinglePaymentReport = {} as ISinglePaymentReport;
  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options)
    const customer_gid = this.router.snapshot.paramMap.get('customer_gid');
    this.customer_gid = customer_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.customer_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.Getaddgrnsummary(deencryptedParam);
    this.Getsummaryaddgrn(deencryptedParam);
    this.addgrnform = new FormGroup({
      customer_gid: new FormControl(''),
      branch_name: new FormControl(''),
      customer_details: new FormControl(''),
      receipt_date: new FormControl(this.getCurrentDate()),
      payment_date: new FormControl(''),
      cheque_date: new FormControl(''),
      neft_date: new FormControl(''),
      customer_address: new FormControl(''),
      customer_contactnumber:new  FormControl(''),
      customer_email:new FormControl(''),
      payment_mode:new FormControl('',[Validators.required]),
      cheque_no:new  FormControl(''),
      creditcard_number:new  FormControl(''),

      bank_name:new FormControl(''),
      cusbank_name:new FormControl(''),
      trancsaction_no:new FormControl(''),
      payment_remarks:new FormControl(''),

    });
    var api = 'SmrReceipt/GetBankDetail'
    this.service.get(api).subscribe((result: any) => {
      this.bankdetailslist = result.GetBankNameVle;
    });
    var api = 'SmrReceipt/GetCardDetail'
    this.service.get(api).subscribe((result: any) => {
      this.carddetailslist = result.GetCardNameVle;
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  Getaddgrnsummary(customer_gid: any) {
    debugger
    var url = 'SmrReceipt/Getinvoicereceipt'
    this.NgxSpinnerService.show()
    let param = {
      customer_gid: customer_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.invoicereceipt_list = result.invoicereceipt_list;
      this.customer_address=this.invoicereceipt_list[0].customer_address;
      this.branch_name=this.invoicereceipt_list[0].branch_name;
      this.customer_email = this.invoicereceipt_list[0].customer_email;
      this.customer_contactnumber = this.invoicereceipt_list[0].customer_contactnumber;
      this.customer_name = this.invoicereceipt_list[0].customer_name;
      this.currency_code = this.invoicereceipt_list[0].currency_code;
      this.exchange_rate = this.invoicereceipt_list[0].exchange_rate;
      this.salesorder_gid = this.invoicereceipt_list[0].salesorder_gid;
      console.log(this.salesorder_gid)
      this.NgxSpinnerService.hide();
    });
  }
  Getsummaryaddgrn(customer_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'SmrReceipt/Getinvoicereceiptsummary'
    let param = {
      customer_gid: customer_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
        this.invoicereceiptsummary_list = this.responsedata.invoicereceiptsummary_list;
        this.defaultcurrencycode = this.responsedata.defaultcurrency;
        this.foreigncurrencycode = this.responsedata.invoicereceiptsummary_list.currency_code;
        const currentDate = new Date().toISOString().split('T')[0];
        this.addgrnform.get('payment_date')?.setValue(currentDate);
        this.addgrnform.get('cheque_date')?.setValue(currentDate);
        this.addgrnform.get('neft_date')?.setValue(currentDate);
      this.NgxSpinnerService.hide();
    });
  }
  onChange2(event: any) {
    this.file = event.target.files[0];
  }
  get receipt_date() {
    return this.addgrnform.get('receipt_date')!;
  }
  get payment_date() {
    return this.addgrnform.get('payment_date')!;
  }
  get cheque_date() {
    return this.addgrnform.get('cheque_date')!;
  }
  get neft_date() {
    return this.addgrnform.get('neft_date')!;
  }
  get bank_name() {
    return this.addgrnform.get('bank_name')!;
  }
  get contact_telephonenumber() {
    return this.addgrnform.get('contact_telephonenumber')!;
  }
  get email_id() {
    return this.addgrnform.get('email_id')!;
  }
  get address() {
    return this.addgrnform.get('address')!;
  }

onsubmit() {
  debugger
    const requestData = {
      updatereceipt_list: this.invoicereceiptsummary_list,
      receipt_date: this.addgrnform.value.receipt_date,
      customer_contactnumber: this.addgrnform.value.customer_contactnumber,
      payment_mode: this.addgrnform.value.payment_mode,

      payment_date: this.addgrnform.value.payment_date,
      cheque_date: this.addgrnform.value.payment_date,
      neft_date: this.addgrnform.value.payment_date,
      cheque_no: this.addgrnform.value.cheque_no,
      bank_name: this.addgrnform.value.bank_name, 
      branch_name: this.branch_name, 
      cusbank_name: this.addgrnform.value.cusbank_name,
      trancsaction_no: this.addgrnform.value.trancsaction_no,
      invoice_from : this.addgrnform.value.invoice_from,
      creditcard_number : this.addgrnform.value.trancsaction_no,

      salesorder_gid :this.invoicereceipt_list[0].salesorder_gid
    };
    console.log(requestData);
    if(this.addgrnform.value.payment_mode==""||this.addgrnform.value.payment_mode==null){
      debugger;
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning("Kindly Select the Receipt Mode");


    }
    else{

    
    const url = 'SmrReceipt/UpdatedMakeReceipt';
    this.NgxSpinnerService.show(); 
    this.service.post(url, requestData).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      window.scrollTo({
        top: 0, 
      });
      if (result.status === false) {
        this.ToastrService.warning(result.message);
      } 
      else {
        this.ToastrService.success(result.message);
        this.route.navigate(['/smr/SmrTrnReceiptsummary']);
      }
    });
  }
 
}
  prodtotalcal(i: any) {
    const receive_amount = parseFloat(this.invoicereceiptsummary_list[i].receive_amount) || 0;
    const tdsreceivale_amount = parseFloat(this.invoicereceiptsummary_list[i].tdsreceivale_amount) || 0;
    const adjust_amount = parseFloat(this.invoicereceiptsummary_list[i].adjust_amount) || 0;
    const exchange_rate = parseFloat(this.invoicereceiptsummary_list[i].exchange_rate) || 1;
    const subtotal = receive_amount + tdsreceivale_amount + adjust_amount;
    const total_amount = subtotal;
    const payment_amount = receive_amount * exchange_rate;
    this.invoicereceiptsummary_list[i].received_in_bank=payment_amount.toFixed(2);
    this.invoicereceiptsummary_list[i].payment_amount = payment_amount.toFixed(2);
    this.invoicereceiptsummary_list[i].total_amount = total_amount.toFixed(2);
    const value = this.total_amount.value;
    const formattedValue = parseFloat(value).toFixed(2);
    this.total_amount.setValue(formattedValue, { emitEvent: false });
  }
  calexchangelossgain(i:any){
    debugger
    const payment_amount = parseFloat(this.invoicereceiptsummary_list[i].payment_amount) || 0;
    const received_in_bank  = parseFloat(this.invoicereceiptsummary_list[i].received_in_bank) || 0;
    const bank_charge = parseFloat(this.invoicereceiptsummary_list[i].bank_charges) || 0;
     
    if(payment_amount > (received_in_bank + bank_charge) ){
      this.exchange_loss = payment_amount - received_in_bank;
      this.invoicereceiptsummary_list[i].exchange_gain = 0;
      this.invoicereceiptsummary_list[i].exchange_loss = this.exchange_loss.toFixed(2);
    }
    else if(payment_amount == (received_in_bank + bank_charge)){
      this.invoicereceiptsummary_list[i].exchange_loss = 0;
      this.invoicereceiptsummary_list[i].exchange_gain = 0;
    }
    else{
      this.exchange_gain = (received_in_bank + bank_charge) - payment_amount;
      this.invoicereceiptsummary_list[i].exchange_loss = 0;
      this.invoicereceiptsummary_list[i].exchange_gain = this.exchange_gain.toFixed(2);
    }


  }
  showTextBox(event: any) {
    const selectedValue = event.value;;
    this.showInput = selectedValue === 'Cheque';
    this.showInput2 = selectedValue === 'DD';
    this.showInput3 = selectedValue === 'CREDITCARD';
    this.showInput4 = selectedValue === 'NEFT';
    this.showInput5 = selectedValue === 'Cash';
  }
  showTextBox2(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput1 = target.value === 'High';
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.singlepayment_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.singlepayment_list.forEach((row: ISinglePaymentReport) => this.selection.select(row));
  }
}
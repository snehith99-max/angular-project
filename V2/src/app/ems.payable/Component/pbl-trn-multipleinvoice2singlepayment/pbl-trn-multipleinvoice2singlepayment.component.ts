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
  selector: 'app-pbl-trn-multipleinvoice2singlepayment',
  templateUrl: './pbl-trn-multipleinvoice2singlepayment.component.html',
  styleUrls: ['./pbl-trn-multipleinvoice2singlepayment.component.scss']
})

export class PblTrnMultipleinvoice2singlepaymentComponent {
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
  vendor_gid: any;
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
  currency:any;


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
      singlepayment_list: this.formBuilder.array([]),
      vendor_gid: new FormControl(''),
      currency: new FormControl(''),
      
    });

    const vendor_gid = this.route.snapshot.paramMap.get('vendor_gid');
    this.vendor_gid = vendor_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.vendor_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.Getmultipleinvoice2employee(deencryptedParam)
    this.reactiveForm.get('vendor_gid')?.setValue(deencryptedParam)
    debugger
    var api = 'PblTrnPaymentRpt/GetBankDetail'
    this.service.get(api).subscribe((result: any) => {
      this.bankdetailslist = result.GetBankNameVle;
    });

    var api = 'PblTrnPaymentRpt/GetCardDetail'
    this.service.get(api).subscribe((result: any) => {
      this.carddetailslist = result.GetCardNameVle;
    });

    
  }

  Getmultipleinvoice2employee(params: any) {
    var param = {
        vendor_gid: params
    };
    var url = 'PblTrnPaymentRpt/Getmultipleinvoice2employeedtl';
    this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.vendor_lists = this.responsedata.Getmultipleinvoice2employeedtl;
        this.name = this.vendor_lists[0].name;
        this.vendor_companyname = this.vendor_lists[0].vendor_companyname;
        this.vendoraddress = this.vendor_lists[0].vendoraddress;
        this.vendorcontactdetails = this.vendor_lists[0].vendorcontactdetails;
        this.email_id = this.vendor_lists[0].email_id;
        this.currency_code = this.vendor_lists[0].currency_code;
        this.exchange_rate = this.vendor_lists[0].exchange_rate;
        this.currency = this.vendor_lists[0].currency;

        // Concatenate vendor contact details and email ID with newlines
        this.Vendor_details = `${this.vendorcontactdetails}\n${this.email_id}`;
    });


    var url = 'PblTrnPaymentRpt/GetpaymentInvoiceSummary'
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.singlepayment_list = this.responsedata.GetMultipleInvoiceSummary;
      this.NgxSpinnerService.hide();

      for (let i = 0; i < this.singlepayment_list.length; i++) {
        this.reactiveForm.addControl(`advance${i}`, new FormControl(this.singlepayment_list[i].advance));
        this.reactiveForm.addControl(`payment_amount_${i}`, new FormControl(this.singlepayment_list[i].payment_amount));
        this.reactiveForm.addControl(`balancepo_advance_${i}`, new FormControl(this.singlepayment_list[i].balancepo_advance));
        this.reactiveForm.addControl(`totalpo_advance_${i}`, new FormControl(this.singlepayment_list[i].totalpo_advance));
        this.reactiveForm.addControl(`grand_total_${i}`, new FormControl(this.singlepayment_list[i].grand_total));
        this.reactiveForm.addControl(`tds_amount_${i}`, new FormControl(this.singlepayment_list[i].tds_amount));
        this.reactiveForm.addControl(`bankcharges_${i}`, new FormControl(this.singlepayment_list[i].bankcharges));
        this.reactiveForm.addControl(`exchangegain_${i}`, new FormControl(this.singlepayment_list[i].exchangegain));
        this.reactiveForm.addControl(`exchangeloss_${i}`, new FormControl(this.singlepayment_list[i].exchangeloss));
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
    // Ensure that payment_amount and tds_amount are numbers or empty strings before processing
    // const payment_amount_str: string = this.singlepayment_list[i].payment_amount ? this.singlepayment_list[i].payment_amount.replace(/,/g, '') : '0'; // Remove commas
    // const payment_amount: number = parseFloat(payment_amount_str) || 0;
    // const tds_amount_str: string = this.singlepayment_list[i].tds_amount ? this.singlepayment_list[i].tds_amount.replace(/,/g, '') : '0'; // Ensure it's a string or '0'
    // const tds_amount: number = parseFloat(tds_amount_str) || 0;
    // const grand_total: number = payment_amount;
    // const final_amount: number = grand_total + tds_amount;

    // // this.singlepayment_list[i].grand_total = grand_total.toFixed(2);
    // this.singlepayment_list[i].final_amount = final_amount.toFixed(2);
    debugger
    // const payed_amount_str: string = this.singlepayment_list[i].payed_amount ? this.singlepayment_list[i].payed_amount.replace(/,/g, '') : '0'; 
    // const payed_amount: number = parseFloat(payed_amount_str) || 0;
    // const invoice_amount_str: string = this.singlepayment_list[i].invoice_amount ? this.singlepayment_list[i].invoice_amount.replace(/,/g, '') : '0'; 
    // const invoice_amount: number = parseFloat(invoice_amount_str) || 0;
    // const outstanding_str: string = this.singlepayment_list[i].outstanding ? this.singlepayment_list[i].outstanding.replace(/,/g, '') : '0'; 
    // const outstanding: number = parseFloat(outstanding_str) || 0;
    const exchange = parseFloat(this.vendor_lists[0].exchange_rate) || 0;

    const outstanding = parseFloat(this.singlepayment_list[i].outstanding) || 0;
    const payment_amount_str: string = this.singlepayment_list[i].payment_amount ? this.singlepayment_list[i].payment_amount.replace(/,/g, '') : '0'; 
    const payment_amount: number = parseFloat(payment_amount_str) || 0;
    const tds_amount_str: string = this.singlepayment_list[i].tds_amount ? this.singlepayment_list[i].tds_amount.replace(/,/g, '') : '0'; 
    const tds_amount: number = parseFloat(tds_amount_str) || 0;
    const bankcharges_str: string = this.singlepayment_list[i].payment_amount ? this.singlepayment_list[i].bankcharges.replace(/,/g, '') : '0'; 
    const bankcharges: number = parseFloat(bankcharges_str) || 0;
    const exchangeloss_str: string = this.singlepayment_list[i].exchangeloss ? this.singlepayment_list[i].exchangeloss.replace(/,/g, '') : '0'; 
    const exchangeloss: number = parseFloat(exchangeloss_str) || 0;
    const exchangegain_str: string = this.singlepayment_list[i].exchangegain ? this.singlepayment_list[i].exchangegain.replace(/,/g, '') : '0'; 
    const exchangegain: number = parseFloat(exchangegain_str) || 0;
    
    
    // const loss : number = (tds_amount + payment_amount + bankcharges) - (outstanding * exchange ) ;
    const loss1: number =(tds_amount + payment_amount);
    const loss2:number =(outstanding * exchange );
    const loss3:number =payment_amount + tds_amount  - loss2
if (this.currency != this.currency_code ){
  if (loss1 <= loss2) {
         
    this.singlepayment_list[i].exchangegain = Math.abs(loss3).toFixed(2);
    this.singlepayment_list[i].exchangeloss = 0.00.toFixed(2); 

} 
else {  this.singlepayment_list[i].exchangeloss = Math.abs(loss3).toFixed(2);
        this.singlepayment_list[i].exchangegain = 0.00.toFixed(2);


}
}else{}
        
    const grand_total: number = payment_amount + tds_amount + bankcharges ;
    const final_amount: number = grand_total;
    
     this.singlepayment_list[i].final_amount = final_amount.toFixed(2);
    //  this.singlepayment_list[i].exchangeloss = loss.toFixed(2);
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
    var api = 'PblTrnPaymentRpt/Postmultipleinvoice2singlepayment';
    const formValues = this.reactiveForm.value; 
    const params = {
      Getmultipleinvoice2employeedtl: this.vendor_lists,
      GetMultipleInvoiceSummary: this.singlepayment_list,
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
        this.router.navigate(['/payable/PblTrnPaymentsummary'])
      }
    
    });
  }
   

  // showTextBox(event: Event) {
  //   debugger
  //   const target = event.target as HTMLInputElement;
  //   this.showInput = target.value === 'Cheque';
  //   this.showInput2 = target.value === 'DD';
  //   this.showInput3 = target.value === 'Credit Card';
  //   this.showInput4 = target.value === 'NEFT';
  //   this.showInput5 = target.value === 'Cash';
  // }


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

  // isAllSelected() {
  //   const numSelected = this.selection.selected.length;
  //   const numRows = this.singlepayment_list.length;
  //   return numSelected === numRows;
  // }

//   masterToggle() {
//     this.isAllSelected() ?
//       this.selection.clear() :
//       this.singlepayment_list.forEach((row: ISinglePaymentReport) => this.selection.select(row));
//   }
 }
import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-rbl-mst-makereceipt',
  templateUrl: './rbl-mst-makereceipt.component.html',
  styleUrls: ['./rbl-mst-makereceipt.component.scss']
})
export class RblMstMakereceiptComponent {

  makereceiptform: FormGroup | any;
  receiptmode_list: any;
  customer_gid: any;
  responsedata: any;
  makereceiptlist: any;
  makereceiptdata: any;
  modeofpayment_list: any;
  received_amount: any;
  total_amount: any;
  payment_amount: any;

  mdlReceiptmode: any;

  constructor(private route: Router, private router: ActivatedRoute, private fb: FormBuilder, private service: SocketService, private ToastrService: ToastrService) {
  }
  ngOnInit() {

    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    
    flatpickr('.date-picker', options);

    const customer_gid = this.router.snapshot.paramMap.get('customer_gid');
    this.customer_gid = customer_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.customer_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
   

    this.makereceiptform = new FormGroup({
      receipt_customer_name: new FormControl(''),
      receipt_customer_address: new FormControl(''),
      receipt_mobile_no: new FormControl(''),
      receipt_email_id: new FormControl(''),
      receipt_paymentdate: new FormControl('',[Validators.required]),
      receipt_branch_name: new FormControl(''),
      receipt_invoice_id: new FormControl(''),
      currency_code: new FormControl(''),
      receipt_invoice_amount: new FormControl(''),
      advance_amount: new FormControl(''),
      invoice_gid: new FormControl(''),
      os_amount: new FormControl(''),
      received_amount: new FormControl(''),
      tds_receivable: new FormControl(''),
      adjust_amount: new FormControl(''),
      receipt_payment_amount: new FormControl(''),
      receipt_total_amount: new FormControl(''),
      receipt_towards: new FormControl(''),
      receipt_mode: new FormControl('', [Validators.required]),
      cheque_date: new FormControl(''),
      cash_date: new FormControl(''),
      neft_date: new FormControl(''),
      invoice_from: new FormControl(''),
      payment_type: new FormControl('', [Validators.required]),
    })

    var api1 = 'Receipt/Getmodeofpayment';
    this.service.get(api1).subscribe((result: any) => {
      this.modeofpayment_list = result.Getmodeofpaymentlist;
    });
    this.GetMakeReceipt(deencryptedParam)

  }
  
  receivedamount (){
    this.payment_amount = this.received_amount;
    this.total_amount = this.received_amount;
  }
  get receiptdateControl() {
    return this.makereceiptform.get('receipt_paymentdate');
  }

  get receiptmodecontrol() {
    return this.makereceiptform.get('payment_type');
  }

 
  GetMakeReceipt(customer_gid: any) {
    var url = 'Receipt/GetMakeReceiptdata'
    debugger
    let param = { customer_gid: customer_gid }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.makereceiptdata = result.makereceipt_list;
      
      this.makereceiptform.get("invoice_gid")?.setValue(this.makereceiptdata[0].invoice_gid);
      this.makereceiptform.get("receipt_customer_name")?.setValue(this.makereceiptdata[0].customer_name);      
      this.makereceiptform.get("receipt_customer_address")?.setValue(this.makereceiptdata[0].customer_address);
      this.makereceiptform.get("receipt_mobile_no")?.setValue(this.makereceiptdata[0].customer_contactnumber);
      this.makereceiptform.get("receipt_email_id")?.setValue(this.makereceiptdata[0].customer_email);
      this.makereceiptform.get("receipt_paymentdate")?.setValue(this.makereceiptdata[0].payment_date);
      this.makereceiptform.get("receipt_branch_name")?.setValue(this.makereceiptdata[0].branch_name);
      this.makereceiptform.get("receipt_invoice_id")?.setValue(this.makereceiptdata[0].invoice_id);
      this.makereceiptform.get("currency_code")?.setValue(this.makereceiptdata[0].currency_code);
      this.makereceiptform.get("receipt_invoice_amount")?.setValue(this.makereceiptdata[0].invoice_amount);
      this.makereceiptform.get("advance_amount")?.setValue(this.makereceiptdata[0].advance_amount);
      this.makereceiptform.get("os_amount")?.setValue(this.makereceiptdata[0].os_amount);
      this.makereceiptform.get("received_amount")?.setValue(this.makereceiptdata[0].received_amount);
      this.makereceiptform.get("tds_receivable")?.setValue(this.makereceiptdata[0].tds_amount);
      this.makereceiptform.get("adjust_amount")?.setValue(this.makereceiptdata[0].adjust_amount);
      this.makereceiptform.get("receipt_payment_amount")?.setValue(this.makereceiptdata[0].payment_amount);
      this.makereceiptform.get("receipt_total_amount")?.setValue(this.makereceiptdata[0].total_amount);      
      this.makereceiptform.get("receipt_towards")?.setValue(this.makereceiptdata[0].receipt_towards);
      this.makereceiptform.get("payment_type")?.setValue(this.makereceiptdata[0].payment_type);      
      this.makereceiptform.get("cheque_date")?.setValue(this.makereceiptdata[0].cheque_date);
      this.makereceiptform.get("cash_date")?.setValue(this.makereceiptdata[0].cash_date);
      this.makereceiptform.get("neft_date")?.setValue(this.makereceiptdata[0].neft_date);
      this.makereceiptform.get("invoice_from")?.setValue(this.makereceiptdata[0].invoice_from);
  });
  }

  updated() {
    console.log(this.makereceiptform.value)
    console.log(this.makereceiptform.invalid)
    const api = 'Receipt/UpdatedMakeReceipt';
    this.service.post(api, this.makereceiptform.value).subscribe((result: any) => {


      if (result.status == false) {
        this.ToastrService.warning(result.message,undefined, { timeOut: 10000 ,  positionClass: 'toast-top-right',
      })

      }
      else {
        this.route.navigate(['einvoice/ReceiptSummary']);

        this.ToastrService.success(result.message,undefined, { timeOut: 10000 ,  positionClass: 'toast-top-right',
      })      }
      this.responsedata = result;

    } );
   

  }

  redirecttolist() {
    this.route.navigate(['/einvoice/ReceiptAdd']);
  }
}

import { Component } from '@angular/core';
import { FormBuilder, FormGroup, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-receiptapproval',
  templateUrl: './smr-trn-receiptapproval.component.html',
  styleUrls: ['./smr-trn-receiptapproval.component.scss']
})
export class SmrTrnReceiptapprovalComponent {

  receipt_list: any[]=[];
  showOptionsDivId: any;
  responsedata:any;
  parameterValue:any;
  Product_list: any[]=[];
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
  }

 
  ngOnInit(): void {
    this.GetReceiptApprovalSummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  GetReceiptApprovalSummary() {
    var url = 'SmrReceipt/GetReceiptApprovalSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#receipt_list').DataTable().destroy();
      this.responsedata = result;
      this.receipt_list = this.responsedata.receiptapprovallist;
      setTimeout(() => {
        $('#receipt_list').DataTable();
      }, 1);


    })


  }
  Details(invoice_gid: any){
    var url = 'SmrReceipt/Getreceiptdetails'
    let param = {
      invoice_gid : invoice_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Product_list = result.invoice_list;
    });
  }
  
  onupdate(){}

  onApprove(){
  debugger
    let param = {
      payment_gid : this.receipt_list[0].payment_gid,
      payment_date : this.receipt_list[0].payment_date,
      customer_name : this.receipt_list[0].customer_name,
      approval_status: this.receipt_list[0].approval_status,
      //amount: this.receipt_list[0].amount,
      payment_type: this.receipt_list[0].payment_type,
      payment_mode: this.receipt_list[0].payment_mode,
      mobile: this.receipt_list[0].mobile,
      total_amount: this.receipt_list[0].total_amount,
      adjust_amount: this.receipt_list[0].adjust_amount,
      bank_charge: this.receipt_list[0].bank_charge,
      exchange_loss: this.receipt_list[0].exchange_loss,
      exchange_gain: this.receipt_list[0].exchange_gain,
      tds_amount: this.receipt_list[0].tds_amount,
      invoice_gid : this.receipt_list[0].invoice_gid,
      payment_remarks : this.receipt_list[0].payment_remarks,
      dbank_gid : this.receipt_list[0].dbank_gid,
      customer_gid : this.receipt_list[0].customer_gid,
      paid_amount : this.receipt_list[0].amount

    }
    var url = 'SmrReceipt/PostReceiptApprove'
    this.NgxSpinnerService.show();
    this.service.post(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
      }
    });
  }
}

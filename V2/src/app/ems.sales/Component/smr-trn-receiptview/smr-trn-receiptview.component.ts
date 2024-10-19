import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-smr-trn-receiptview',
  templateUrl: './smr-trn-receiptview.component.html',
  styleUrls: ['./smr-trn-receiptview.component.scss']
})
export class SmrTrnReceiptviewComponent {

  showInput: boolean = false;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInput3: boolean = false;
  showInput4: boolean = false;
  showInput5: boolean = false;

  payment_gid : any;
  paymentgid : any;
  response_data: any;
  receiptview_list : any[] =[];
  receiptview : any[] =[];
  viewBankSummary : any[]=[];
  receipt_date : any;
  branch:any;
  customer_name : any;
  email: any;
  mobile: any;
  address : any;
  receipt_mode : any;
  date: any;
  transactionref : any;
  depositedbank : any;
  defaultcurrencycode : any;
  cash_date : any;
  cheque_number : any

  constructor(private router : ActivatedRoute , private service : SocketService){}
  ngOnInit(): void {

    const payment_gid = this.router.snapshot.paramMap.get('payment_gid');
       this.payment_gid = payment_gid;
       const secretKey = 'storyboarderp';
       const deencryptedParam = AES.decrypt(this.payment_gid, secretKey).toString(enc.Utf8);
       this.paymentgid = deencryptedParam;
       console.log(deencryptedParam)
       this.GetReceiptViewSummary();
       this.GetReceiptViewProdSummary();
       this.GeViewBankReceiptSummary();

      
       
  }
  GetReceiptViewSummary(){
    var url = "SmrReceipt/GetViewReceipt";
    let param = {
        payment_gid : this.paymentgid
    }
    this.service.getparams(url,param).subscribe((result : any)=>{
      this.response_data = result;
      this.receiptview_list = result.ReceiptView_list;
      this.receipt_date = this.receiptview_list[0].payment_date;
      this.branch = this.receiptview_list[0].branch_name;
      this.email = this.receiptview_list[0].email;
      this.mobile = this.receiptview_list[0].mobile;
      this.customer_name = this.receiptview_list[0].customer_name,
      this.address = this.receiptview_list[0].customer_address;
      this.defaultcurrencycode = this.receiptview_list[0].currency_code;
      
    })
  }

  GetReceiptViewProdSummary(){
    var url = "SmrReceipt/GetViewReceiptSummary";
    let param = {
        payment_gid : this.paymentgid
    }
    this.service.getparams(url,param).subscribe((result : any)=>{
      this.response_data = result;
      this.receiptview = result.receiptapprovallist;
    })
  }
  GeViewBankReceiptSummary(){
    var url = "SmrReceipt/GetViewReceiptBankSummary";
    let param = {
      payment_gid : this.paymentgid
    }
    this.service.getparams(url,param).subscribe((result : any)=>{
      this.response_data = result;
      this.viewBankSummary = result.ReceiptView_list;
      this.receipt_mode = this.viewBankSummary[0].payment_mode;
      this.date = this.viewBankSummary[0].cheque_date;
      this.transactionref = this.viewBankSummary[0].neft_transcationid;
      this.depositedbank = this.viewBankSummary[0].bank_name;
      this.cheque_number = this.viewBankSummary[0].cheque_number;
      this.cash_date = this.viewBankSummary[0].cash_date;
      this.branch = this.viewBankSummary[0].branch;
      
     
    })
  }

  onback(){
    window.history.back();
  }
}

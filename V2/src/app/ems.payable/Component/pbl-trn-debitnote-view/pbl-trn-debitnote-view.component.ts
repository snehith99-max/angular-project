import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-pbl-trn-debitnote-view',
  templateUrl: './pbl-trn-debitnote-view.component.html',
})
export class PblTrnDebitnoteViewComponent {

  GetStockReturnDebit_list: any[]=[];
  GetStockReturnproduct_list: any[]=[];  
  invoicegid: any;
  invoice_gid: any;

  constructor(
    private router : Router,
    private route : ActivatedRoute,
    private NgxSpinnerService : NgxSpinnerService,
    private service : SocketService
  ){}

  ngOnInit() : void {
    const key = 'storyboard';
    this.invoicegid = this.route.snapshot.paramMap.get('invoicegid');
    this.invoice_gid = AES.decrypt(this.invoicegid,key).toString(enc.Utf8);
    this.GetDebitNoteView(this.invoice_gid);
    this.GetDebitNoteViewProduct(this.invoice_gid);
  }
  GetDebitNoteView(invoice_gid: any){
    this.NgxSpinnerService.show();
    let param = { invoice_gid : invoice_gid };
    var summaryapi = 'PblDebitNote/GetStockReturnDebitSummary';
    this.service.getparams(summaryapi,param).subscribe((result : any)=>{
      this.GetStockReturnDebit_list = result.GetStockReturnDebit_list;
      this.NgxSpinnerService.hide();
    });
  }
  GetDebitNoteViewProduct(invoice_gid: any){
    this.NgxSpinnerService.show();
    let param = { invoice_gid : invoice_gid};
    var productapi = 'PblDebitNote/GetStockReturnDebitProductSummary';
    this.service.getparams(productapi,param).subscribe((result: any)=>{
      this.GetStockReturnproduct_list = result.GetStockReturnproduct_list;
      this.NgxSpinnerService.hide();
    });
  }
}

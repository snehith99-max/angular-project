import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-pbl-trn-debitnote-summary',
  templateUrl: './pbl-trn-debitnote-summary.component.html',
})
export class PblTrnDebitnoteSummaryComponent {

  Getdebitnote_list: any[]=[];
  GetDebitdtl_list: any[]=[];
  hidebutton : boolean = false;
  selectedDebitnoteGid: any;
  selectedPaymentGid: any;

  constructor(private service: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    private router : Router,
    private ToastrService: ToastrService
  ){}

  ngOnInit() : void {
    this.NgxSpinnerService.show();
    var summaryapi = 'PblDebitNote/GetDebitNoteSummary';
    this.service.get(summaryapi).subscribe((result: any)=>{
      this.Getdebitnote_list = result.Getdebitnote_list;
      this.GetDebitdtl_list = result.GetDebitdtl_list;
      console.log('list',this.GetDebitdtl_list)
      setTimeout(() => {
        $('#Getdebitnote_list').DataTable();
      }, 1); 
      this.NgxSpinnerService.hide();   
    });    
  }
  stockreturn(invoice_gid: any, debitnote_gid: any){
    const key = 'storyboard';
    const param = invoice_gid;
    const param1 = debitnote_gid;
    const invoicegid =  AES.encrypt(param,key).toString();
    const debitnotegid =  AES.encrypt(param1,key).toString();
    this.router.navigate(['/payable/PblTrnStockReturn',invoicegid,debitnotegid])
  }
  DebitPDF(debitnote_gid : any, invoice_gid: any){
    this.NgxSpinnerService.show();
    let param = { debitnote_gid: debitnote_gid, invoice_gid : invoice_gid};
    var pdfapi = 'PblDebitNote/DebitPDF';
    this.service.getparams(pdfapi,param).subscribe((result: any)=>{
      if(result.status == true){
        this.service.filedownload1(result);
        this.NgxSpinnerService.hide();
      }
      else{
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }
  DebitView(invoice_gid: any){
    const key = 'storyboard';
    const param = invoice_gid;
    const invoicegid = AES.encrypt(param,key).toString();
    this.router.navigate(['/payable/PblTrnDebitNoteView',invoicegid]);
  }
  buttonhide(debitnote_gid: any){  
    return this.tohidebutton(this.GetDebitdtl_list, debitnote_gid)   
  }
  tohidebutton(items: any[],debitnote_gid: any){
   return items.some(item => item.debitnote_gid === debitnote_gid);  
  }

  openModaldelete(debitnote_gid: any, payment_gid: any) {
    this.selectedDebitnoteGid = debitnote_gid;
    this.selectedPaymentGid = payment_gid;
  }
  DeleteDebit() {
    this.NgxSpinnerService.show();
    let param = { debitnote_gid: this.selectedDebitnoteGid, payment_gid: this.selectedPaymentGid };
    var deleteapi = 'PblDebitNote/DeleteDebitNote';
    
    this.service.getparams(deleteapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      } else {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();      
        this.ngOnInit();  // Reload the component to refresh the data
      }
    });
  }
}

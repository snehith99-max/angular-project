import { Component } from '@angular/core';
import { FormBuilder, FormGroup, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-tcreditnnote-summary',
  templateUrl: './smr-trn-tcreditnnote-summary.component.html',
  styleUrls: ['./smr-trn-tcreditnnote-summary.component.scss']
})
export class SmrTrnTcreditnnoteSummaryComponent {

  creditnotesummary_list : any[] =[];
  GetCreditdtl_list : any[]=[];
  responsedata: any;
  showOptionsDivId: any;
  creditnote_no : any;
  receipt_gid : any

  ngOnInit(): void {
    this.GetCreditNoteSummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
  }

  toggleOptions(invoice_gid: any) {
    debugger
    if (this.showOptionsDivId === invoice_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = invoice_gid;
    }
  }
  sortColumn(columnKey: string): void {
    return this.service.sortColumn(columnKey);
  }
  getSortIconClass(columnKey: string) {
    return this.service.getSortIconClass(columnKey);
  }

  //// Summary Grid//////
  GetCreditNoteSummary() {
    var url = 'SmrTrnCreditNote/GetCreditNoteSummary';
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#creditnotesummary_list').DataTable().destroy();
      this.responsedata = result;
      this.creditnotesummary_list = this.responsedata.creditnotesummary_list;
      this.GetCreditdtl_list = result.GetCreditdtl_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#creditnotesummary_list').DataTable();
      }, 1);
    })
  }
  buttonhide(creditnote_gid: any){  
    return this.tohidebutton(this.GetCreditdtl_list, creditnote_gid)   
  }
  tohidebutton(items: any[],creditnote_gid: any){
   return items.some(item => item.creditnote_gid === creditnote_gid);  
  }
  onview(invoice_gid:any){
    const secretKey = 'storyboarderp';
    const invoicegid = (invoice_gid);
    const encryptedParam = AES.encrypt(invoicegid, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnTcreditnoteview', encryptedParam]);
  }
  onStockReturn(invoice_gid:any){
    const secretKey = 'storyboarderp';
    const invoicegid = (invoice_gid);
    const encryptedParam = AES.encrypt(invoicegid, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnStockReturn', encryptedParam]);
  }
  openModaldelete(creditnote_gid:any,receipt_gid : any) {
    debugger
    this.creditnote_no = creditnote_gid,
    this.receipt_gid = receipt_gid
  }
  ondelete(){
    debugger
    var url = 'SmrTrnCreditNote/GetDeleteCreditNote';
    let param = {
      creditnote_gid : this.creditnote_no,
      receipt_gid : this.receipt_gid
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this. GetCreditNoteSummary() ;
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.success(result.message)
        this. GetCreditNoteSummary() ;
        this.NgxSpinnerService.hide()
      }
    });
  }
  //pdf
  CreditPDF(creditnote_gid : any, invoice_gid: any){
    this.NgxSpinnerService.show();
    let param = { creditnote_gid: creditnote_gid, invoice_gid : invoice_gid};
    var pdfapi = 'SmrTrnCreditNote/CreditPDF';
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
}

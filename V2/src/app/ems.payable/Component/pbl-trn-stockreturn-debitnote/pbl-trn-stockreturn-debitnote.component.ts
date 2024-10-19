import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-pbl-trn-stockreturn-debitnote',
  templateUrl: './pbl-trn-stockreturn-debitnote.component.html',
})
export class PblTrnStockreturnDebitnoteComponent {

  StockReturnForm! : FormGroup;
  Mdlcreditinvoice_amount: any;
  invoicegid: any;
  debitnotegid: any;
  invoice_gid: any;
  debitnote_gid: any;
  GetStockReturnproduct_list: any[]=[];
  GetStockReturnDebit_list: any[]=[];

  constructor(private route: ActivatedRoute,
    private router : Router,
    private service : SocketService,
    private NgxSpinnerService : NgxSpinnerService,
    private ToastrService : ToastrService
  ){}

  ngOnInit() :void {

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    const key = 'storyboard';
    this.invoicegid = this.route.snapshot.paramMap.get('invoicegid');
    this.debitnotegid = this.route.snapshot.paramMap.get('debitnotegid');
    this.invoice_gid = AES.decrypt(this.invoicegid, key).toString(enc.Utf8);
    this.debitnote_gid = AES.decrypt(this.debitnotegid, key).toString(enc.Utf8);
    this.GetStockReturnDebitnoteSummary(this.invoice_gid);
    this.GetStockReturnProductSummary(this.invoice_gid);

    this.StockReturnForm = new FormGroup({
      invoice_refno : new FormControl(''),
      invoice_date : new FormControl(this.getCurrentDate()),
      orderref_no : new FormControl(''),
      company_details : new FormControl(''),
      invoice_remarks : new FormControl(''),
      vendor_address : new FormControl(''),
      vendor_companyname : new FormControl(''),
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }

  GetStockReturnDebitnoteSummary(invoice_gid: any){
    this.NgxSpinnerService.show();
    let param = { invoice_gid : invoice_gid};
    var summaryapi = 'PblDebitNote/GetStockReturnDebitSummary';
    this.service.getparams(summaryapi,param).subscribe((result: any)=>{
      this.GetStockReturnDebit_list = result.GetStockReturnDebit_list;
      this.StockReturnForm.get('invoice_refno')?.setValue(result.GetStockReturnDebit_list[0].invoice_refno);
      this.StockReturnForm.get('invoice_date')?.setValue(result.GetStockReturnDebit_list[0].invoice_date);
      this.StockReturnForm.get('orderref_no')?.setValue(result.GetStockReturnDebit_list[0].invoice_reference);
      this.StockReturnForm.get('vendor_companyname')?.setValue(result.GetStockReturnDebit_list[0].vendor_companyname);
      this.StockReturnForm.get('invcoie_remarks')?.setValue(result.GetStockReturnDebit_list[0].remarks);
      this.StockReturnForm.get('vendor_address')?.setValue(result.GetStockReturnDebit_list[0].vendor_address);

      const company_details = `${result.GetStockReturnDebit_list[0].email_id}\n${result.GetStockReturnDebit_list[0].mobile}\n${result.GetStockReturnDebit_list[0].contactperson_name}`;
      this.StockReturnForm.get('company_details')?.setValue(company_details);
      this.NgxSpinnerService.hide();
    });
  }
  GetStockReturnProductSummary(invoice_gid: any){
    this.NgxSpinnerService.show();
    let param = { invoice_gid : invoice_gid};
    var productapi = 'PblDebitNote/GetStockReturnDebitProductSummary';
    this.service.getparams(productapi,param).subscribe((result: any)=>{
      this.GetStockReturnproduct_list = result.GetStockReturnproduct_list;
      this.NgxSpinnerService.hide();
    });
  }
  qtycal(data: any){

  }
  onSubmit(){
    this.NgxSpinnerService.show();
    let param = {
      GetStockReturnproduct_list : this.GetStockReturnproduct_list,
      debitnote_gid: this.debitnote_gid
    };
    var postapi = 'PblDebitNote/PostStockReturnDebit';
    this.service.post(postapi, param).subscribe((result: any)=>{
      if(result.status == false){
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
      else{
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.router.navigate(['/payable/PblTrnDebitNote']);
      }
    });
  }
}

import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective } from '@angular/forms';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-pbl-trn-raisedebitnote-add',
  templateUrl: './pbl-trn-raisedebitnote-add.component.html',
})
export class PblTrnRaisedebitnoteAddComponent {

  invoicegid: any;
  invoice_gid: any;
  DeditForm!: FormGroup;
  GetDebitProduct_list: any[] = [];
  GetRaiseDebitNoteAdd_list: any[] = [];
  Mdlcreditinvoice_amount: any;

  constructor(private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private service: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService
  ) { }

  ngOnInit(): void {

    const key = 'storyboard';
    this.invoicegid = this.route.snapshot.paramMap.get('invoicegid');
    this.invoice_gid = AES.decrypt(this.invoicegid, key).toString(enc.Utf8);
    this.GetRaiseDebitnoteSummary(this.invoice_gid);
    this.GetRaiseDebitProductSummary(this.invoice_gid);

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    this.DeditForm = new FormGroup({
      branch_name: new FormControl(''),
      invoice_refno: new FormControl(''),
      invoice_date: new FormControl(''),
      invoice_reference: new FormControl(''),
      vendor_companyname: new FormControl(''),
      company_details: new FormControl(''),
      payment_term: new FormControl(''),
      payment_date: new FormControl(''),
      currency_code: new FormControl(''),
      exchange_rate: new FormControl(''),
      invoice_remarks: new FormControl(''),
      vendor_address: new FormControl(''),
      vendor_refnodate: new FormControl(''),
      costcenter_name: new FormControl(''),
      reasondebit: new FormControl(''),
      outstanding: new FormControl(''),
      branch_gid: new FormControl(''),
      vendor_gid: new FormControl(''),
      debitnote_date: new FormControl(this.getCurrentDate()),
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  GetRaiseDebitnoteSummary(invoice_gid: any) {
    this.NgxSpinnerService.show();
    let param = { invoice_gid: invoice_gid }
    var summaryapi = 'PblDebitNote/GetRaiseDebitNoteAdd';
    this.service.getparams(summaryapi, param).subscribe((result: any) => {
      this.GetRaiseDebitNoteAdd_list = result.GetRaiseDebitNoteAdd_list;
      this.DeditForm.get('branch_name')?.setValue(result.GetRaiseDebitNoteAdd_list[0].branch_name);
      this.DeditForm.get('invoice_refno')?.setValue(result.GetRaiseDebitNoteAdd_list[0].invoice_refno);
      this.DeditForm.get('invoice_date')?.setValue(result.GetRaiseDebitNoteAdd_list[0].invoice_date);
      this.DeditForm.get('invoice_reference')?.setValue(result.GetRaiseDebitNoteAdd_list[0].invoice_reference);
      this.DeditForm.get('payment_term')?.setValue(result.GetRaiseDebitNoteAdd_list[0].payment_term);
      this.DeditForm.get('payment_date')?.setValue(result.GetRaiseDebitNoteAdd_list[0].payment_date);
      this.DeditForm.get('vendor_companyname')?.setValue(result.GetRaiseDebitNoteAdd_list[0].vendor_companyname);
      this.DeditForm.get('currency_code')?.setValue(result.GetRaiseDebitNoteAdd_list[0].currency_code);
      this.DeditForm.get('exchange_rate')?.setValue(result.GetRaiseDebitNoteAdd_list[0].exchange_rate);
      this.DeditForm.get('invoice_remarks')?.setValue(result.GetRaiseDebitNoteAdd_list[0].invoice_remarks);
      this.DeditForm.get('vendor_address')?.setValue(result.GetRaiseDebitNoteAdd_list[0].vendor_address);
      this.DeditForm.get('vendor_refnodate')?.setValue(result.GetRaiseDebitNoteAdd_list[0].vendor_refnodate);
      this.DeditForm.get('costcenter_name')?.setValue(result.GetRaiseDebitNoteAdd_list[0].costcenter_name);
      this.DeditForm.get('branch_gid')?.setValue(result.GetRaiseDebitNoteAdd_list[0].branch_gid);
      this.DeditForm.get('vendor_gid')?.setValue(result.GetRaiseDebitNoteAdd_list[0].vendor_gid);      

      const company_detaisl = `${result.GetRaiseDebitNoteAdd_list[0].email_id}\n${result.GetRaiseDebitNoteAdd_list[0].mobile}\n${result.GetRaiseDebitNoteAdd_list[0].contactperson_name}`;
      this.DeditForm.get('company_details')?.setValue(company_detaisl);
      this.NgxSpinnerService.hide();
    });
  }
  GetRaiseDebitProductSummary(invoice_gid: any) {
    this.NgxSpinnerService.show();
    let param = { invoice_gid: invoice_gid }
    var productapi = 'PblDebitNote/GetDebitProductSummary';
    this.service.getparams(productapi, param).subscribe((result: any) => {
      this.GetDebitProduct_list = result.GetDebitProduct_list;
      this.NgxSpinnerService.hide();
    });
  }
  qtycal(data: any) {
    debugger
    const invoice_gid = data.invoice_gid;
    const outstanding = parseFloat(data.outstanding.toString().replace(/,/g, '')); 
    const debit_amount = parseFloat(this.Mdlcreditinvoice_amount.toString().replace(/,/g, '')); 
    
    if(debit_amount === 0.00)
    {
      return 'Debit Amount should not be Zero'
    }
    else (debit_amount > outstanding) 
    {
      return 'Debit Amount should be less than or equal to Outstanding Amount';
    }
    
    return '';
  }
  
  onSubmit() {
debugger
    for (let data of this.GetRaiseDebitNoteAdd_list) {
      let warning = this.qtycal(data);
      if (warning != '') {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(warning);
        return
      }
    }
    this.NgxSpinnerService.show();
    let param =
    { 
      invoice_gid : this.invoice_gid,
      outstanding: this.DeditForm.value.outstanding,
      creditinvoice_amount : this.Mdlcreditinvoice_amount,
      reasondebit : this.DeditForm.value.reasondebit,
      debitnote_date : this.DeditForm.value.debitnote_date,
      exchange_rate : this.DeditForm.value.exchange_rate,
      branch_gid : this.DeditForm.value.branch_gid,
      vendor_gid : this.DeditForm.value.vendor_gid,
      invoice_refno : this.DeditForm.value.invoice_refno,
    }
    var postapi = 'PblDebitNote/PostDebitnote';
    this.service.post(postapi, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/payable/PblTrnDebitNote']);
        this.NgxSpinnerService.hide();
      }
    });
  }
}

import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { environment } from '../../../../environments/environment.development';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-rbl-trn-tinvoice',
  templateUrl: './rbl-trn-tinvoice.component.html',
  styleUrls: ['./rbl-trn-tinvoice.component.scss']
})

export class RblTrnTinvoiceComponent {
  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/

  invoicesummarylist: any[] = [];
  response_data: any;
  company_code: any;
  cancelirnForm!: FormGroup;
  parameterValue2: any;
  parameterValue: any;
  responsedata:any;


  creditnoteForm!: FormGroup;

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
     private route: ActivatedRoute, private router: Router, private service: SocketService,
     public NgxSpinnerService:NgxSpinnerService) {
    this.cancelirnForm = new FormGroup({
      invoice_refno_cancel: new FormControl(''),
      invoice_date_cancel: new FormControl(''),
      invoice_custname_cancel: new FormControl(''),
      invoice_amount_cancel: new FormControl(''),
      irn_no_cancel: new FormControl(''),
      invoice_gid: new FormControl(''),

    })

    this.creditnoteForm = new FormGroup({
      invoice_refno_creditnote: new FormControl(''),
      invoice_date_creditnote: new FormControl(''),
      invoice_custname_creditnote: new FormControl(''),
      invoice_amount_creditnote: new FormControl(''),
      irn_no_creditnote: new FormControl(''),
      invoice_gid: new FormControl(''),

    })
  }

  ngOnInit(): void {
    this.EinvoiceSummary();
  }
  EinvoiceSummary() {
    var api = 'Einvoice/einvoiceSummary';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.response_data = result;
      this.invoicesummarylist = this.response_data.invoicesummary_list;
     
      setTimeout(() => {
        $('#invoice').DataTable();
      }, 1);
    });
  }

  onadd() {
    this.router.navigate(['/einvoice/AddInvoice'])
  }

  onraiseinvoice() {
    this.router.navigate(['/einvoice/SalesInvoiceSummary'])
  }

  PrintPDF(invoice_gid: string) {
    debugger
          const api = 'Einvoice/GetInvoicePDF';
          this.NgxSpinnerService.show()
          let param = {
            invoice_gid:invoice_gid
          } 
          this.service.getparams(api,param).subscribe((result: any) => {
            if(result!=null){
              this.service.filedownload1(result);
            }
            this.NgxSpinnerService.hide()
          });
    
  }

  editinvoice(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/einvoice/Invoice-Edit', encryptedParam])
  }
  viewinvoice(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/einvoice/Invoiceview', encryptedParam])
  }
  
  einvoice(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/einvoice/EInvoice', encryptedParam])
  }

  onsubmit() {

  var url = 'Einvoice/PostCancelIRN'
      this.service.postparams(url, this.cancelirnForm.value).pipe().subscribe(result => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.EinvoiceSummary();
        }
        else {
          this.ToastrService.success("Update successfully")
          this.EinvoiceSummary();
        }
      });
  }
  creditnotesubmit() {
    

    var url = 'Einvoice/PostCreditNote'
        this.service.postparams(url, this.creditnoteForm.value).pipe().subscribe(result => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.EinvoiceSummary();
          }
          else {
            this.ToastrService.success("IRN Cancelled Sucessfully")
            this.EinvoiceSummary();
          }
        });
    }
  myModaladddetails(parameter: string) {
    this.parameterValue2 = parameter
    this.cancelirnForm.get("invoice_refno_cancel")?.setValue(this.parameterValue2.invoice_refno);
    this.cancelirnForm.get("invoice_date_cancel")?.setValue(this.parameterValue2.invoice_date);
    this.cancelirnForm.get("invoice_custname_cancel")?.setValue(this.parameterValue2.customer_name);
    this.cancelirnForm.get("invoice_amount_cancel")?.setValue(this.parameterValue2.invoice_amount);
    this.cancelirnForm.get("irn_no_cancel")?.setValue(this.parameterValue2.irn);
    this.cancelirnForm.get("invoice_gid")?.setValue(this.parameterValue2.invoice_gid);




  }
  creditnoteModaldetails(parameter: string) {
    this.parameterValue = parameter
    this.creditnoteForm.get("invoice_refno_creditnote")?.setValue(this.parameterValue.invoice_refno);
    this.creditnoteForm.get("invoice_date_creditnote")?.setValue(this.parameterValue.invoice_date);
    this.creditnoteForm.get("invoice_custname_creditnote")?.setValue(this.parameterValue.customer_name);
    this.creditnoteForm.get("invoice_amount_creditnote")?.setValue(this.parameterValue.invoice_amount);
    this.creditnoteForm.get("irn_no_creditnote")?.setValue(this.parameterValue.irn);
    this.creditnoteForm.get("invoice_gid")?.setValue(this.parameterValue.invoice_gid);

 }

  onclose() {
    this.cancelirnForm.reset();
  };
  back(){
    this.creditnoteForm.reset();


  }

  ondelete() {

  }
}
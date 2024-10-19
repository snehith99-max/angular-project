import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable, ReplaySubject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../../environments/environment.development';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-rbl-trn-proforma-invoice',
  templateUrl: './rbl-trn-proforma-invoice.component.html',
  styleUrls: ['./rbl-trn-proforma-invoice.component.scss']
})
export class RblTrnProformaInvoiceComponent {
  response_data: any;
  proformainvoice: any;
  company_code: any;
  parameterValue1: any;
  invoice_gid: any;
  proformaproduct_list: any[] = [];
  parameterValue: any;



  constructor(private fb: FormBuilder,public NgxSpinnerService:NgxSpinnerService,
     private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService) { }

  ngOnInit(): void {
    var api = 'ProformaInvoice/GetProformaInvoiceSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.proformainvoice = this.response_data.proformainvoicesummary_list;
      setTimeout(() => {
        $('#proformainvoice').DataTable();
      }, 1);
    });
  }

  PrintPDF(invoice_gid: string) {
    debugger
    debugger
    this.NgxSpinnerService.show();
    let params = { invoice_gid : invoice_gid}    
    var pdfapi = 'ProformaInvoice/GetProformaInvoicePDF';
    this.service.getparams(pdfapi, params).subscribe((apiresponse : any) =>{
      if(apiresponse.status == false){
        this.ToastrService.warning(apiresponse.message);
      }
      else{
        this.service.filedownload1(apiresponse);
      }
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.hide();  
  }

  raiseproforma() {
    this.router.navigate(['/einvoice/ProformaInvoiceAdd'])
  }
  editproformainvoice(params: string) {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/einvoice/ProformaInvoiceEdit', encryptedParam])
  }
  Mail(params: string) {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/einvoice/ProformaInvoiceMail', encryptedParam])
  }

  proformaadvancereceipt(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/einvoice/ProformaInvoiceAdvanceReceipt', encryptedParam])
  }
  proformainvoiceview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/einvoice/ProformaInvoiceView', encryptedParam])
  }

  Details(parameter: string, invoice_gid: string) {
    this.parameterValue1 = parameter;
    this.invoice_gid = parameter;

    var url = 'ProformaInvoice/GetProductdetails'
    let param = {
      invoice_gid: invoice_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.response_data = result;
      this.proformaproduct_list = result.proformaproduct_list;
    });
  }

  GetProductdetails() {

    var url = 'ProformaInvoice/GetProductdetails'
    this.service.get(url).subscribe((result: any) => {
      $('#proformaproduct_list').DataTable().destroy();
      this.response_data = result;
      this.proformaproduct_list = this.response_data.proformaproduct_list;
      setTimeout(() => {
        $('#proformaproduct_list').DataTable();
      }, 1);


    })
  }

  ondelete() {
    debugger
    var deleteapi = 'ProformaInvoice/DeleteProformaInvoice';
    this.service.getparams(deleteapi, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else{
        this.ToastrService.success(result.message)
      }
    });
  }
  openModaldelete(invoice_gid: string, invoice_reference: string, invoice_amount: string) {
    debugger
    let param = {
      invoice_gid: invoice_gid,
      invoice_reference: invoice_reference,
      invoice_amount: invoice_amount
    }
    this.parameterValue = param
  }



}
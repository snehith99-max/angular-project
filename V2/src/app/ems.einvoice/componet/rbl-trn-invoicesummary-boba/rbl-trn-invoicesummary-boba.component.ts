import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { environment } from '../../../../environments/environment.development';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-rbl-trn-invoicesummary-boba',
  templateUrl: './rbl-trn-invoicesummary-boba.component.html',
  styleUrls: ['./rbl-trn-invoicesummary-boba.component.scss']
})
export class RblTrnInvoicesummaryBobaComponent {

  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/

  invoicesummarylist: any[] = [];
  response_data: any;
  company_code: any;
  parameterValue2: any;
  parameterValue: any;
  responsedata: any;
  lspage: any;

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
    private route: ActivatedRoute, private router: Router, private service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {
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
    const lspage ="Invoice";

    this.router.navigate(['/einvoice/Invoice-Add',lspage])
  }

  onraiseinvoice() {

    this.router.navigate(['/einvoice/SalesInvoiceSummary'])
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
    this.router.navigate(['/einvoice/Invoice-View', encryptedParam])
  }

  PrintPDF(invoice_gid: string) {
    debugger
    this.NgxSpinnerService.show();
          const api = 'Einvoice/GetInvoicePDF';
          let param = {
            invoice_gid:invoice_gid
          } 
          this.service.getparams(api,param).subscribe((result: any) => {
            if(result!=null){
              this.service.filedownload1(result);
              
            this.NgxSpinnerService.hide();
            }
            else{  
              this.ToastrService.warning(result.message)
            }
          });
    
  }
}

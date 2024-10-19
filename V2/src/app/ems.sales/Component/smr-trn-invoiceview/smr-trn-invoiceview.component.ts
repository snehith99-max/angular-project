import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-smr-trn-invoiceview',
  templateUrl: './smr-trn-invoiceview.component.html',
  styleUrls: ['./smr-trn-invoiceview.component.scss']
})
export class SmrTrnInvoiceviewComponent {

  invoice_gid: any;
  responsedata: any;
  viewinvoicelist: any;
  grand_total: any;
  directinvoiceproductsummary_list:any;
  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private route: Router, private router: ActivatedRoute) {}
  ngOnInit(): void {
    const invoice_gid = this.router.snapshot.paramMap.get('invoice_gid');
    this.invoice_gid = invoice_gid
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.viewinvoice(deencryptedParam);
  }
  onclickgoback(){
    window.history.back()
  }
  viewinvoice(invoice_gid: any) {
    debugger
    var url = 'SmrRptInvoiceReport/viewinvoice';
    this.invoice_gid = invoice_gid
    var params={
      invoice_gid : invoice_gid
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.viewinvoicelist = result.Salesviewinvoice_list;
      console.log(this.viewinvoicelist);
    });
      var url1='SmrRptInvoiceReport/viewinvoiceproduct'
      this.service.getparams(url1, params).subscribe((result: any) => {
        this.responsedata = result;
        this.directinvoiceproductsummary_list = result.directinvoiceproductsummary_list;
        console.log(this.viewinvoicelist);
      });
  }
}



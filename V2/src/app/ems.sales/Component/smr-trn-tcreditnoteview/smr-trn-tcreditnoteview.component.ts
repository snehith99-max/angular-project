import { HttpClient } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-smr-trn-tcreditnoteview',
  templateUrl: './smr-trn-tcreditnoteview.component.html',
  styleUrls: ['./smr-trn-tcreditnoteview.component.scss']
})
export class SmrTrnTcreditnoteviewComponent {
  invoice_gid : any;
  combinedFormData : FormGroup | any;
  productform : FormGroup | any;
  tax_name: any;
  tax_name2 : any;
  tax_name3 : any;
  tax_amount : any;
  tax_amount2:any;
  tax_amount3 : any;
  responsedata:any;
  cnsummary_list : any[] = [];
  cnprodsummary_list : any[] = [];
  customer_address : any;
  viewinvoicelist: any;
  directinvoiceproductsummary_list:any;
  constructor(
    private fb: FormBuilder, private renderer: Renderer2,
    private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService,) {
    
  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
 
    const invoice_gid = this.router.snapshot.paramMap.get('invoice_gid'); 
    this.invoice_gid = invoice_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);
    this.GetCnSummary(deencryptedParam);
    this.GetProdSummary(deencryptedParam);
   
  }
  GetCnSummary(params:any){
     debugger
    var url='SmrTrnCreditNote/GetViewAddSelectSummary'
    let param = {
      invoice_gid : params
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.cnsummary_list = this.responsedata.addselectCNsummary_list;
    const address = this.cnsummary_list[0].customer_address;
    const mobile = this.cnsummary_list[0].mobile;
    const contactperson = this.cnsummary_list[0].customer_contactperson;
    const customer_Address = `${contactperson}\n${mobile}\n${address}`; 
    this.customer_address =  customer_Address;
    });
  }

  GetProdSummary(params:any){
    
    debugger
    var url = 'SmrRptInvoiceReport/viewinvoice';
    var param={
      invoice_gid : params
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.viewinvoicelist = result.Salesviewinvoice_list;
      console.log(this.viewinvoicelist);
    });
      var url1='SmrRptInvoiceReport/viewinvoiceproduct'
      this.service.getparams(url1, param).subscribe((result: any) => {
        this.responsedata = result;
        this.directinvoiceproductsummary_list = result.directinvoiceproductsummary_list;
        console.log(this.viewinvoicelist);
      });
  }
}

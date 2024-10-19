import { Component, DebugEventListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-renewal-invoice',
  templateUrl: './smr-trn-renewal-invoice.component.html',
  styleUrls: ['./smr-trn-renewal-invoice.component.scss']
})
export class SmrTrnRenewalInvoiceComponent {

  getrenewalinvoice: any[] = [];
  getrenewalinvoice2: any[] = [];
  getrenewalinvoice3: any[] = [];
  getrenewalinvoice4: any[] = [];
  getrenewalinvoice5: any[] = [];
  responsedata: any;
  RenewalCountList: any[] = [];
  parameterValue: any; 
   paramvalue: any;
   renewal_gid:any;
constructor(public service:SocketService,private router:Router,private route:Router, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {
 
  }

  ngOnInit(): void {
    this.GetRenewalInvoiceSummary();
    this.getrenewalinvoice.sort((a,b) => {
      return new (b.created_date) - new (a.created_date); 
    });
    this.GetRenewalInvoiceSummaryless30();
    this.getrenewalinvoice2.sort((a,b) => {
      return new (b.created_date) - new (a.created_date); 
    });
    this.GetRenewalInvoiceSummaryexpired();
    this.getrenewalinvoice3.sort((a,b) => {
      return new (b.created_date) - new (a.created_date); 
    });
    this.GetRenewalInvoiceSummarydrop();
    this.getrenewalinvoice4.sort((a,b) => {
      return new (b.created_date) - new (a.created_date); 
    });
    this.GetRenewalInvoiceSummaryall();
    this.getrenewalinvoice5.sort((a,b) => {
      return new (b.created_date) - new (a.created_date); 
    });


    var url = 'SmrTrnRenewalInvoiceSummary/GetCount';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.RenewalCountList = this.responsedata.GetRenewalInvoiceSummary_lists;

    });
  }


  GetRenewalInvoiceSummary(){
    this.NgxSpinnerService.show();
    debugger
    var url = 'SmrTrnRenewalInvoiceSummary/GetRenewalInvoiceSummary'
    this.service.get(url).subscribe((result: any) => {
     $('#getrenewalinvoice').DataTable().destroy();
     this.responsedata = result;
     this.getrenewalinvoice = this.responsedata.GetRenewalInvoiceSummary_lists;
     setTimeout(() => {
       $('#getrenewalinvoice').DataTable();
     }, 1);
     this.NgxSpinnerService.hide();
   });
  }

  GetRenewalInvoiceSummaryless30(){
    this.NgxSpinnerService.show();
    debugger
    var url = 'SmrTrnRenewalInvoiceSummary/GetRenewalInvoiceSummaryless30'
    this.service.get(url).subscribe((result: any) => {
     $('#getrenewalinvoice2').DataTable().destroy();
     this.responsedata = result;
     this.getrenewalinvoice2 = this.responsedata.GetRenewalInvoiceSummary_lists2;
     setTimeout(() => {
       $('#getrenewalinvoice2').DataTable();
     }, 1);
     this.NgxSpinnerService.hide();
   });
  }

  GetRenewalInvoiceSummaryexpired(){
    this.NgxSpinnerService.show();
    debugger
    var url = 'SmrTrnRenewalInvoiceSummary/GetRenewalInvoiceSummaryexpired'
    this.service.get(url).subscribe((result: any) => {
     $('#getrenewalinvoice3').DataTable().destroy();
     this.responsedata = result;
     this.getrenewalinvoice3 = this.responsedata.GetRenewalInvoiceSummary_lists3;
     setTimeout(() => {
       $('#getrenewalinvoice3').DataTable();
     }, 1);
     this.NgxSpinnerService.hide();
   });
  }

  GetRenewalInvoiceSummarydrop(){
    this.NgxSpinnerService.show();
    debugger
    var url = 'SmrTrnRenewalInvoiceSummary/GetRenewalInvoiceSummarydrop'
    this.service.get(url).subscribe((result: any) => {
     $('#getrenewalinvoice4').DataTable().destroy();
     this.responsedata = result;
     this.getrenewalinvoice4 = this.responsedata.GetRenewalInvoiceSummary_lists4;
     setTimeout(() => {
       $('#getrenewalinvoice4').DataTable();
     }, 1);
     this.NgxSpinnerService.hide();
   });
  }

  GetRenewalInvoiceSummaryall(){
    this.NgxSpinnerService.show();
    debugger
    var url = 'SmrTrnRenewalInvoiceSummary/GetRenewalInvoiceSummaryall'
    this.service.get(url).subscribe((result: any) => {
     $('#getrenewalinvoice5').DataTable().destroy();
     this.responsedata = result;
     this.getrenewalinvoice5 = this.responsedata.GetRenewalInvoiceSummary_lists5;
     setTimeout(() => {
       $('#getrenewalinvoice5').DataTable();
     }, 1);
     this.NgxSpinnerService.hide();
   });
  }
  open(renewal_gid: any) {
    this.paramvalue = renewal_gid;
  }
  openModalrenew(parameter: string) {
    this.parameterValue = parameter
  
  }
  RaisetoOrder(params: any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
     this.route.navigate(['/smr/SmrTrnRenewaltoInvoice',encryptedParam]) 
  }
  ondrop() {
    debugger
 // API endpoint URL
  this.NgxSpinnerService.show()
    var url = 'SmrTrnRenewalInvoiceSummary/GetDroprenewal'
    let param = {
      renewal_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else{
           
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        window.location.reload();
    }
  });
}

}

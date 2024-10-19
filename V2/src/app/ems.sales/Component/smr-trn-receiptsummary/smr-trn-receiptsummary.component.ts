import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../../environments/environment.development';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-smr-trn-receiptsummary',
  templateUrl: './smr-trn-receiptsummary.component.html',
  styleUrls: ['./smr-trn-receiptsummary.component.scss']
})
export class SmrTrnReceiptsummaryComponent {

  receipt: any;
  responsedata : any;
  parameterValue : any;
  company_code : any;
  parameterValue1: any;
  invoice_list : any[] = [];
  invoice_gid: any;
  showOptionsDivId:any;


  constructor(private fb: FormBuilder, private route: ActivatedRoute, 
    public NgxSpinnerService:NgxSpinnerService,private router: Router, private service: SocketService, private ToastrService: ToastrService) { }

  ngOnInit(): void {
    this.NgxSpinnerService.show();
    var api = 'SmrReceipt/GetReceiptSummary1';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.receipt = this.responsedata.receiptsummary_list;
      setTimeout(() => {
        $('#receipt').DataTable();
      }, 1);
    });
    this.NgxSpinnerService.hide();
  }


  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    console.log(this.parameterValue);
    var url3 = 'SmrReceipt/deleteReceiptSummary'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      this.ngOnInit();

    });
  }


  Details(parameter: string,payment_gid: string){
    this.parameterValue1 = parameter;
    this.invoice_gid = parameter;
  
    var url='SmrReceipt/Getreceiptdetails'
      let param = {
        payment_gid : payment_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.invoice_list = result.invoice_list;   
      });
    
  }

  Getreceiptdetails() {
    
    var url = 'SmrReceipt/Getreceiptdetails'
    this.service.get(url).subscribe((result: any) => {
      $('#invoice_list').DataTable().destroy();
      this.responsedata = result;
      this.invoice_list = this.responsedata.invoice_list;
      setTimeout(() => {
        $('#invoice_list').DataTable();
      }, 1);
  
  
    })
  }
  

 addreceipt() {
    this.router.navigate(['/smr/SmrTrnAddReceipt'])
  }

//  PrintPDF(payment_gid: string,payment_type:string) {
//     this.company_code = localStorage.getItem('c_code')
//     window.location.href = "http://" + environment.host + "/Print/EMS_print/rbl_mst_paymentsummaryreceiptrpt.aspx?payment_gid=" + payment_gid + "&companycode=" + this.company_code+ "&payment_type=" + payment_type
//   }
PrintPDF(payment_gid: string, payment_type:string) {
        const api = 'SmrReceipt/GetReceiptPDF';
        this.NgxSpinnerService.show()
        let param = {
          payment_gid:payment_gid,
          payment_type : payment_type
        } 
        this.service.getparams(api,param).subscribe((result: any) => {
          if(result!=null){
            this.service.filedownload1(result);
          }
          this.NgxSpinnerService.hide()
        });
  
}

View(payment_gid : string){
  const secretKey = 'storyboarderp';
  const param = (payment_gid);
  const encryptedParam = AES.encrypt(param, secretKey).toString();
  this.router.navigate(['/smr/SmrTrnReceiptview', encryptedParam])
}
}

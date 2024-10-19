import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { en } from '@fullcalendar/core/internal-common';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-acc-trn-sundrypayment-view',
  templateUrl: './acc-trn-sundrypayment-view.component.html',
  styleUrls: ['./acc-trn-sundrypayment-view.component.scss']
})
export class AccTrnSundrypaymentViewComponent {

  GetSundryPayment_list : any[]=[];
  GetSundryPaymentDetails_list : any[]=[];
  paymentgid: any;
  payment_gid: any;

  constructor(private route : ActivatedRoute,
  private service: SocketService
  ){}

  ngOnInit():void {
    debugger
    const key = 'storyboarderp';
    this.paymentgid = this.route.snapshot.paramMap.get('paymentgid');
    this.payment_gid = AES.decrypt(this.paymentgid, key).toString(enc.Utf8);
    this.GetSundryPaymentProduct(this.payment_gid);
    this.GetSundryPaymentDetails(this.payment_gid);
  }
  GetSundryPaymentProduct(payment_gid: any){
    let param = {
      payment_gid : payment_gid
    }
    var getapi = 'AccTrnPayment/GetSundryPaymentView';
    this.service.getparams(getapi,param).subscribe((result:any)=>{
      this.GetSundryPayment_list = result.GetSundryPayment_list;
    });
  }
  GetSundryPaymentDetails(payment_gid:any){
    let param = {
      payment_gid : payment_gid
    }
    var getapi = 'AccTrnPayment/GetSundryPaymentViewDetails';
    this.service.getparams(getapi,param).subscribe((result:any)=>{
      this.GetSundryPaymentDetails_list = result.GetSundryPaymentDetails_list;
    });
  }
}

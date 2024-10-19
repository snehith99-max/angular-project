import { Component } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';


@Component({
  selector: 'app-ims-trn-sales-history',
  templateUrl: './ims-trn-sales-history.component.html',
  styleUrls: ['./ims-trn-sales-history.component.scss']
})
export class ImsTrnSalesHistoryComponent {
  saleshistory_list: any;
  Getsaleshistory_list:any[]=[];
  customergid:any;
  productgid:any;
  constructor(private route: Router,
    private router: ActivatedRoute,
    private NgxSpinnerService: NgxSpinnerService,
    private service: SocketService
  ) { }

ngOnInit():void{
    debugger
    const key = 'storyboard';
    this.customergid = this.router.snapshot.paramMap.get('customergid');
    const customer_gid = AES.decrypt(this.customergid, key).toString(enc.Utf8);
    this.productgid = this.router.snapshot.paramMap.get('productgid');
    const product_gid = AES.decrypt(this.productgid, key).toString(enc.Utf8);
  this.Getsalescustomer(customer_gid);
  this.GetSalesorder_history(customer_gid,product_gid);
}
Getsalescustomer(customer_gid:any){
  debugger
  let param = { 
    customer_gid: customer_gid 
  }
  this.NgxSpinnerService.show();
  var Api = 'ImsRptStockreport/GetSalescustomer';
  this.service.getparams(Api, param).subscribe((response: any) =>{
    this.saleshistory_list = response.GetStockcustomer_list;
    this.NgxSpinnerService.hide();
  });
}

GetSalesorder_history(customer_gid:any,product_gid:any){
debugger;
let param={
customer_gid:customer_gid,
product_gid:product_gid
}
this.NgxSpinnerService.show();
var Api='ImsRptStockreport/GetSalesorder_history';
this.service.getparams(Api,param).subscribe((reponse:any)=>{
this.Getsaleshistory_list=reponse.Getsaleshistory_list;
this.NgxSpinnerService.hide();
});
}
onview(params:any){
  debugger
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/ims/ImsTrnSalesHistoryview',encryptedParam])
}
}

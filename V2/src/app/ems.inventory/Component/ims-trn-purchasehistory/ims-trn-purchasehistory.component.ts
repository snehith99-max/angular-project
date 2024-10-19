import { Component } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';


@Component({
  selector: 'app-ims-trn-purchasehistory',
  templateUrl: './ims-trn-purchasehistory.component.html',
  styleUrls: ['./ims-trn-purchasehistory.component.scss']
})
export class ImsTrnPurchasehistoryComponent {
  purchasehistory_list: any;
  purchaseorder_list:any[]=[];
  vendorgid:any;
  productgid:any;
  constructor(private route: Router,
    private router: ActivatedRoute,
    private NgxSpinnerService: NgxSpinnerService,
    private service: SocketService
  ) { }

ngOnInit():void{
    debugger
    const key = 'storyboard';
    this.vendorgid = this.router.snapshot.paramMap.get('vendorgid');
    const vendor_gid = AES.decrypt(this.vendorgid, key).toString(enc.Utf8);
    this.productgid = this.router.snapshot.paramMap.get('productgid');
    const product_gid = AES.decrypt(this.productgid, key).toString(enc.Utf8);
  this.GetPurchasevendor(vendor_gid);
  this.GetPurchaseorder_history(vendor_gid,product_gid);
}

GetPurchasevendor(vendor_gid:any){
  debugger
  let param = { 
    vendor_gid: vendor_gid 
  }
  this.NgxSpinnerService.show();
  var Api = 'ImsRptStockreport/GetPurchasevendor';
  this.service.getparams(Api, param).subscribe((response: any) =>{
    this.purchasehistory_list = response.GetStockvendor_list;
    this.NgxSpinnerService.hide();
  });
}
GetPurchaseorder_history(vendor_gid:any,product_gid:any){
debugger;
let param={
vendor_gid:vendor_gid,
product_gid:product_gid
}
this.NgxSpinnerService.show();
var Api='ImsRptStockreport/GetPurchaseorder_history';
this.service.getparams(Api,param).subscribe((reponse:any)=>{
this.purchaseorder_list=reponse.Getpurchaseorder_history;
this.NgxSpinnerService.hide();
});
}
onview(params:any){
  debugger
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/ims/ImsTrnPurchaseHistoryview',encryptedParam])
}
}

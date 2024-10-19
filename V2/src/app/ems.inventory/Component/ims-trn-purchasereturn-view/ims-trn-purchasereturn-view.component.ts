import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-ims-trn-purchasereturn-view',
  templateUrl: './ims-trn-purchasereturn-view.component.html',
  styleUrls: ['./ims-trn-purchasereturn-view.component.scss']
})
export class ImsTrnPurchasereturnViewComponent {

  GetPurchaseReturnViewDetails_list: any[]=[];
  GetPurchaseReturnView_list: any[]=[];
  purchasereturngid : any;



  constructor(private router: Router,
    private route: ActivatedRoute,
    private service: SocketService
  ){}

  ngOnInit(): void{
    const key = 'storyboard';
    this.purchasereturngid = this.route.snapshot.paramMap.get('purchasereturngid');
    const purchasereturn_gid = AES.decrypt(this.purchasereturngid, key).toString(enc.Utf8);

    this.GetPurchaseReturnViewSummary(purchasereturn_gid);
    this.GetPurchaseReturnViewDetailsSummary(purchasereturn_gid);
  }
  GetPurchaseReturnViewSummary(purchasereturn_gid: any){
    let param = { purchasereturn_gid: purchasereturn_gid}
    var summaryapi = 'PurchaseReturn/GetPurchaseReturnView';
    this.service.getparams(summaryapi,param).subscribe((result: any)=>{
    this.GetPurchaseReturnView_list = result.GetPurchaseReturnView_list;
    });
  }
  GetPurchaseReturnViewDetailsSummary(purchasereturn_gid: any){
  let param = { purchasereturn_gid : purchasereturn_gid }
  var summaryapi = 'PurchaseReturn/GetPurchaseReturnViewDetails';
  this.service.getparams(summaryapi,param).subscribe((result: any)=>{
  this.GetPurchaseReturnViewDetails_list = result.GetPurchaseReturnViewDetails_list;
  });
  }
}

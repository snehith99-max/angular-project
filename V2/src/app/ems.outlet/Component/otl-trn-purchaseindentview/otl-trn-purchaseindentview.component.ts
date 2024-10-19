import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-otl-trn-purchaseindentview',
  templateUrl: './otl-trn-purchaseindentview.component.html',
  styleUrls: ['./otl-trn-purchaseindentview.component.scss']
})
export class OtlTrnPurchaseindentviewComponent {
  purchaserequest_list: any [] = [];
  productdetails_list: any [] = [];
  requestview:any;
  responsedata: any;
  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService:NgxSpinnerService) {
  }
  ngOnInit(): void {
  const requestview =this.route.snapshot.paramMap.get('purchaserequisition_gid');
  this.requestview= requestview;
  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.requestview,secretKey).toString(enc.Utf8);
  console.log(deencryptedParam)
  this.GetViewPurchaseSummary(deencryptedParam);
  }

  GetViewPurchaseSummary(purchaserequisition_gid: any) {
    var url='OtlTrnPurchaseIndent/GetPurchaseRequisitionView'
    this.NgxSpinnerService.show();
    let param = {
      purchaserequisition_gid : purchaserequisition_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.purchaserequest_list = result.purchaserequestitionview; 
    this.NgxSpinnerService.hide();

    });
  
   var url1='OtlTrnPurchaseIndent/GetPurchaseRequisitionproduct'
   this.NgxSpinnerService.show();
    this.service.getparams(url1,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.productdetails_list = result.purchaserequestitionview
    this.NgxSpinnerService.hide();
    });
  }
  onback()
  {
    this.router.navigate(['/pmr/PmrTrnPurchaseRequisition']);
  }
}


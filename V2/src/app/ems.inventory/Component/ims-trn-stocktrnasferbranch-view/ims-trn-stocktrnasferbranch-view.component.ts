import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-ims-trn-stocktrnasferbranch-view',
  templateUrl: './ims-trn-stocktrnasferbranch-view.component.html',
  styleUrls: ['./ims-trn-stocktrnasferbranch-view.component.scss']
})
export class ImsTrnStocktrnasferbranchViewComponent {

    stock: any;
    stocktransferbranchview:any;
    responsedata: any;
  
    constructor(private formBuilder: FormBuilder,private route:Router,private router:ActivatedRoute,public service :SocketService,private NgxSpinnerService:NgxSpinnerService) { }
  
    ngOnInit(): void {
     const stocktransfer_gid =this.router.snapshot.paramMap.get('stocktransfer_gid');
      this.stock= stocktransfer_gid;
      const secretKey = 'storyboarderp';
      const deencryptedParam = AES.decrypt(this.stock,secretKey).toString(enc.Utf8);
      console.log(deencryptedParam)
      this.GetBranchWiseView(deencryptedParam);
    }
    GetBranchWiseView(stocktransfer_gid: any) {
      this.NgxSpinnerService.show();
      var url='ImsTrnStockTransferSummary/GetBranchWiseView'
      let param = {
        stocktransfer_gid : stocktransfer_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.stocktransferbranchview = result.stocktransferbranchview;   
      this.NgxSpinnerService.hide();
      });
    }
  
  
  
  
}

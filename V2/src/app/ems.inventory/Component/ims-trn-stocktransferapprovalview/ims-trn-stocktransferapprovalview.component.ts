import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-ims-trn-stocktransferapprovalview',
  templateUrl: './ims-trn-stocktransferapprovalview.component.html',
  styleUrls: ['./ims-trn-stocktransferapprovalview.component.scss']
})
export class ImsTrnStocktransferapprovalviewComponent {

  locationview_list: any [] = [];
  productdetails_list: any [] = [];
  requestview:any;
  responsedata: any;
  stocktransfer_gid:any;
  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService:NgxSpinnerService) {
  }
  ngOnInit(): void {
  const requestview =this.route.snapshot.paramMap.get('stocktransfer_gid');
  this.requestview= requestview;
  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.requestview,secretKey).toString(enc.Utf8);
  console.log(deencryptedParam)
  this.stocktransfer_gid = deencryptedParam;
  this.GetViewLocationSummary(deencryptedParam);
  }

  GetViewLocationSummary(stocktransfer_gid: any) {
    debugger;
    var url='ImsTrnStockTransferSummary/GetImsRptStocktransferapprovalview'
    this.NgxSpinnerService.show();
    let param = {
      stocktransfer_gid : stocktransfer_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.locationview_list = result.stocktransferapproval_list; 
    this.NgxSpinnerService.hide();

    });
  
   var url1='ImsTrnStockTransferSummary/GetStocktransferProductView'
   this.NgxSpinnerService.show();
    this.service.getparams(url1,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.productdetails_list = result.stocktransferlocationproductview
    this.NgxSpinnerService.hide();
    });
  }
  onback()
  {
    this.router.navigate(['/ims/ImsTrnStockTransferApprovalSummary']);
  }
  onSubmit() {
    debugger;
    var approvalapi = 'ImsTrnStockTransferSummary/StockTransferApproval';
    let param = {
      stocktransfer_gid: this.stocktransfer_gid,
    }
    this.service.getparams(approvalapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['ims/ImsTrnStockTransferApprovalSummary']);
      }
    });
  }
  onreject(){
    var rejectapi = 'ImsTrnStockTransferSummary/StockTransferReject';
    let param = {
      stocktransfer_gid:this.stocktransfer_gid,
    }
    this.service.getparams(rejectapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['ims/ImsTrnStockTransferApprovalSummary']);
      }
    });
  }

}

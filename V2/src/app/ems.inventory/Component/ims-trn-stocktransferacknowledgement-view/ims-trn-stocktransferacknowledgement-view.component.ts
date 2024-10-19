import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-ims-trn-stocktransferacknowledgement-view',
  templateUrl: './ims-trn-stocktransferacknowledgement-view.component.html',
  styleUrls: ['./ims-trn-stocktransferacknowledgement-view.component.scss']
})
export class ImsTrnStocktransferacknowledgementViewComponent {
  StocktransferAck: FormGroup | any;
  locationview_list: any [] = [];
  productdetails_list: any [] = [];
  requestview:any;
  responsedata: any;
  stocktransfer_gid:any;
  getCurrentDate: any;
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
  this.StocktransferAck = new FormGroup({

    stocktranferack_date: new FormControl(this.getCurrentDate()),

  });
  const options: Options = {
    dateFormat: 'd-m-Y',    
  };
  flatpickr('.date-picker', options);
  }

  GetViewLocationSummary(stocktransfer_gid: any) {
    debugger;
    var url='ImsTrnStockTransferSummary/GetImsRptStocktransferackview'
    this.NgxSpinnerService.show();
    let param = {
      stocktransfer_gid : stocktransfer_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.locationview_list = result.stocktransferapproval_list; 
    this.NgxSpinnerService.hide();

    });
  
   var url1='ImsTrnStockTransferSummary/GetStocktransferackProductView'
   this.NgxSpinnerService.show();
    this.service.getparams(url1,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.productdetails_list = result.stocktransferlocationproductview
    this.NgxSpinnerService.hide();
    });
  }
  onback()
  {
    this.router.navigate(['/ims/ImsTrnStockTransferAcknowledgementSummary']);
  }
  onSubmit() {
    debugger;
    var approvalapi = 'ImsTrnStockTransferSummary/StockTransferAckApproval';
    let param = {
      stocktransfer_gid: this.stocktransfer_gid,
    }
    this.service.getparams(approvalapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['ims/ImsTrnStockTransferAcknowledgementSummary']);
      }
    });
  }
  onreject(){
    var rejectapi = 'ImsTrnStockTransferSummary/StockTransferAckReject';
    let param = {
      stocktransfer_gid:this.stocktransfer_gid,
    }
    this.service.getparams(rejectapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/ims/ImsTrnStockTransferAcknowledgementSummary']);
      }
    });
  }

}

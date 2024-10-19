import { Component } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-ims-trn-salesreturn-view',
  templateUrl: './ims-trn-salesreturn-view.component.html',
  
})
export class ImsTrnSalesreturnViewComponent {

  salesreturngid: any;
  salesreturn_gid: any;
  GetSalesReturnView_list: any[]=[];
  GetSalesReturnViewDetails_list: any[]=[];

  constructor(private route: Router,
    private router : ActivatedRoute,
    private service :  SocketService,
    private NgxSpinnerService: NgxSpinnerService
  ){}

  ngOnInit() :void {
    debugger
    const key = 'storyboard';
    this.salesreturngid = this.router.snapshot.paramMap.get('salesreturngid');
    this.salesreturn_gid = AES.decrypt(this.salesreturngid, key).toString(enc.Utf8);
    this.GetSalesReturnViewSummary(this.salesreturn_gid);   
  }
  GetSalesReturnViewSummary(salesreturn_gid: any){
    let params = { salesreturn_gid: salesreturn_gid}
    this.NgxSpinnerService.show();
    var viewapi = 'SalesReturn/GetSalesReturnViewSummary';
    this.service.getparams(viewapi, params).subscribe((result : any)=>{
    this.GetSalesReturnView_list = result.GetSalesReturnView_list;
    this.GetSalesReturnViewDetails(this.salesreturn_gid,result.GetSalesReturnView_list[0].directorder_gid);    
    });
  }
  GetSalesReturnViewDetails(salesreturn_gid: any, directorder_gid: any){
    let params = { salesreturn_gid: salesreturn_gid, directorder_gid: directorder_gid}
    this.NgxSpinnerService.show();
    var viewapi = 'SalesReturn/GetSalesReturnViewDetails';
    this.service.getparams(viewapi, params).subscribe((result : any)=>{
    this.GetSalesReturnViewDetails_list = result.GetSalesReturnViewDetails_list;
    this.NgxSpinnerService.hide();
    });
  }
}
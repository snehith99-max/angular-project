import { Component } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-ims-trn-stockstatement-view',
  templateUrl: './ims-trn-stockstatement-view.component.html',
})
export class ImsTrnStockstatementViewComponent {

  productgid: any;
  branchgid:any;
  product_name: any;
  product_code: any;
  GetStockStatement_list: any[] = [];
  stockreport_list:any[]=[];
  GetStockCustomer_list: any[] = [];
  StockpurchasecustomerList: any[] = [];
  GetStockpurchaseState_list: any[] = [];

  constructor(private route: Router,
    private router: ActivatedRoute,
    private NgxSpinnerService: NgxSpinnerService,
    private service: SocketService
  ) { }

  ngOnInit(): void {
    debugger
    const key = 'storyboard';
    this.productgid = this.router.snapshot.paramMap.get('productgid');
    const product_gid = AES.decrypt(this.productgid, key).toString(enc.Utf8);
    this.branchgid = this.router.snapshot.paramMap.get('branchgid');
    const branch_gid = AES.decrypt(this.branchgid, key).toString(enc.Utf8);
    this.GetStockStatementProduct(product_gid,branch_gid);
    // this.GetStockStatemetSummary(product_gid);
    // this.GetStockStatementPurchaseSummary(product_gid);
    this.GetStockStatementPurchase(product_gid);
    this.GetStockStatementSales(product_gid);
  }
  GetStockStatementProduct(product_gid: any,branch_gid:any){
    debugger
    let param = { 
      product_gid: product_gid ,
      branch_gid:branch_gid
    }
    this.NgxSpinnerService.show();
    var Api = 'ImsRptStockreport/GetStockStatementProduct';
    this.service.getparams(Api, param).subscribe((response: any) =>{
      this.stockreport_list = response.stockreport_list;
      this.NgxSpinnerService.hide();
    });
  }
  GetStockStatementPurchase(product_gid: any){
    debugger
    let param = { 
      product_gid: product_gid ,
    }
    this.NgxSpinnerService.show();
    var Api = 'ImsRptStockreport/GetStockStatementPurchase';
    this.service.getparams(Api, param).subscribe((response: any) =>{
      this.GetStockpurchaseState_list = response.GetStockpurchaseState_list;
      this.NgxSpinnerService.hide();
    });
  }
  GetStockStatementSales(product_gid: any){
    debugger
    let param = { 
      product_gid: product_gid ,
    }
    this.NgxSpinnerService.show();
    var Api = 'ImsRptStockreport/GetStockStatementSales';
    this.service.getparams(Api, param).subscribe((response: any) =>{
      this.GetStockStatement_list = response.GetStockStatement_list;
      this.NgxSpinnerService.hide();
    });
  }
  GetStockStatemetSummary(product_gid: any) {
    let param = { product_gid: product_gid }
    this.NgxSpinnerService.show();
    var summaryapi = 'ImsRptStockreport/GetStockStatementSummary';
    this.service.getparams(summaryapi, param).subscribe((apiresponse: any) => {
      this.GetStockStatement_list = apiresponse.GetStockState_list;
      this.product_name = apiresponse.GetStockState_list[0].product_name; 
      this.product_code = apiresponse.GetStockState_list[0].product_code; 
      this.GetStockCustomer_list = apiresponse.StockcustomerList;
      this.NgxSpinnerService.hide();
    });
  }
  GetStockStatementPurchaseSummary(product_gid: any){
    let param = { product_gid: product_gid }
    this.NgxSpinnerService.show();
    var summaryapi = 'ImsRptStockreport/GetStockStatementPurchaseSummary';
    this.service.getparams(summaryapi, param).subscribe((apiresponse: any) => {
      this.GetStockpurchaseState_list = apiresponse.GetStockpurchaseState_list;
      this.StockpurchasecustomerList = apiresponse.StockpurchasecustomerList;
      this.NgxSpinnerService.hide();
    });
  }
  Getstockproduct(customer_gid: any) {
    return this.filterproduct(this.GetStockStatement_list, customer_gid)
  }
  filterproduct(items: any[], customer_gid: any) {
    return items.filter(item => item.customer_gid === customer_gid);
  }
  GetPurchasestock(vendor_gid: any){
    return this.filterpurchasestock(this.GetStockpurchaseState_list, vendor_gid)
  }
  filterpurchasestock(items: any[], vendor_gid: any){
    return items.filter(item => item.vendor_gid === vendor_gid);
  }
  purchase_history(vendor_gid:any,product_gid:any){
    debugger
    const key = 'storyboard';
    const param1 = (vendor_gid);
    const param2 = (product_gid);
    const vendorgid = AES.encrypt(param1, key).toString();
    const productgid = AES.encrypt(param2, key).toString();
    this.route.navigate(['/ims/ImsTrnPurchasehistory', vendorgid,productgid]);
  }
  sales_history(customer_gid:any,product_gid:any){
    debugger
    const key = 'storyboard';
    const param1 = (customer_gid);
    const param2 = (product_gid);
    const customergid = AES.encrypt(param1, key).toString();
    const productgid = AES.encrypt(param2, key).toString();
    this.route.navigate(['/ims/ImsTrnSalesHistory', customergid,productgid]);
  }

}

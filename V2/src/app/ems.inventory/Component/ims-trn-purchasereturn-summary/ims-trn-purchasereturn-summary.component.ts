import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-ims-trn-purchasereturn-summary',
  templateUrl: './ims-trn-purchasereturn-summary.component.html',
  styleUrls: ['./ims-trn-purchasereturn-summary.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class ImsTrnPurchasereturnSummaryComponent {

  GetPurchaseReturn_list: any[] = [];
  showOptionsDivId: any;
  getGetViewSRProduct_list:any;
  responsedata: any;

  constructor(private service: SocketService,
    private router: Router,
    private route: Router,
    private ToastrService: ToastrService,private NgxSpinnerService:NgxSpinnerService
  ) { }

  ngOnInit(): void {
    var summaryapi = 'PurchaseReturn/GetPurchaseReturnSummary';
    this.service.get(summaryapi).subscribe((result: any) => {
      this.GetPurchaseReturn_list = result.GetPurchaseReturn_list;
      setTimeout(() => {
        $('#GetPurchaseReturn_list').DataTable();
      }, 1);
    });
  }
  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }
  PurchasereturnView(purchasereturn_gid: any) {
    const key = 'storyboard';
    const param = purchasereturn_gid;
    const purchasereturngid = AES.encrypt(param, key).toString();
    this.router.navigate(['/ims/ImsTrnPurchaseReturnView', purchasereturngid])
  }
  PrintPDF(purchasereturn_gid: any) {
    // API endpoint URL
    const api = 'PurchaseReturn/GetPurchaseReturnRpt';
    this.NgxSpinnerService.show()
    let param = {
      purchasereturn_gid:purchasereturn_gid
    } 
    this.service.getparams(api,param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
  PurchaseReturnCancel(purchasereturn_gid: any) {
    this.NgxSpinnerService.show();
    let param = { purchasereturn_gid: purchasereturn_gid }
    var cencelapi = 'PurchaseReturn/PurchaseReturnCancel';
    this.service.getparams(cencelapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
    });
  }

  Getproduct(purchasereturn_gid: any){
    let param = {
      purchasereturn_gid:purchasereturn_gid
    } 
    var url = 'PurchaseReturn/GetViewSRProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#getGetViewSRProduct_list').DataTable().destroy();
      this.responsedata = result;
      this.getGetViewSRProduct_list = this.responsedata.getProduct_list;
      setTimeout(() => {
        $('#getGetViewSRProduct_list').DataTable();
      }, 1);
    });
  }
}

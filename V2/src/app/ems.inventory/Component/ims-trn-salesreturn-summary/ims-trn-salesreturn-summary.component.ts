import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-ims-trn-salesreturn-summary',
  templateUrl: './ims-trn-salesreturn-summary.component.html',
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
export class ImsTrnSalesreturnSummaryComponent {

  GetSalesReturn_list: any[] = [];
  showOptionsDivId: any;
  getGetViewSRProduct_list:any;
  responsedata: any;
  constructor(private service: SocketService,
    private router : Router,
    private route :ActivatedRoute,
    private NgxSpinnerService: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    this.NgxSpinnerService.show();
    var summaryapi = 'SalesReturn/GetSalesReturnSummary';
    this.service.get(summaryapi).subscribe((result: any) => {
      this.GetSalesReturn_list = result.GetSalesReturn_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#GetSalesReturn_list').DataTable();
      }, 1);
    });    
  }
  toggleOptions(data:any){
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }
  SalesReturnView(salesreturn_gid: any){
    debugger
    const key = 'storyboard';
    const param = salesreturn_gid;
    const salesreturngid = AES.encrypt(param,key).toString();
    this.router.navigate(['/ims/ImsTrnSalesReturnView',salesreturngid])
  }
  Getproduct(salesreturn_gid: any){
    let param = {
      salesreturn_gid:salesreturn_gid
    } 
    var url = 'SalesReturn/GetViewSRProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#getGetViewSRProduct_list').DataTable().destroy();
      this.responsedata = result;
      this.getGetViewSRProduct_list = this.responsedata.getGetViewSRProduct_list;
      setTimeout(() => {
        $('#getGetViewSRProduct_list').DataTable();
      }, 1);
    });
  }
}

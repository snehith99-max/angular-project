import { Component } from '@angular/core';
import { setRef } from '@fullcalendar/core/internal';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-ims-trn-salesreturn-add',
  templateUrl: './ims-trn-salesreturn-add.component.html',
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
export class ImsTrnSalesreturnAddComponent {

  GetSalesReturnAdd_list: any[]=[];
  responsedata:any;
  GetViewreturnProduct_list:any;

  constructor(private service: SocketService, 
    private NgxSpinnerService: NgxSpinnerService,
    private route: Router,
    private router : ActivatedRoute
  ){}


  ngOnInit() : void {
    this.NgxSpinnerService.show();
    var summaryapi = 'SalesReturn/GetSalesReturnAddSummary';
    this.service.get(summaryapi).subscribe((result: any)=>{
      this.GetSalesReturnAdd_list = result.GetSalesReturnAdd_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#GetSalesReturnAdd_list').DataTable();
      }, 1);
    });
  }
  addsalesreturn(directorder_gid: any){
    const key = 'storyboard'
    const param = directorder_gid;
    const directordergid = AES.encrypt(param,key).toString();
    this.route.navigate(['/ims/ImsTrnSalesReturnAddSelect',directordergid])
  }

  Getproduct(directorder_gid: any){
    let param = {
      directorder_gid:directorder_gid
    } 
    var url = 'SalesReturn/GetViewreturnProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#GetViewreturnProduct_list').DataTable().destroy();
      this.responsedata = result;
      this.GetViewreturnProduct_list = this.responsedata.GetViewreturnProduct_list;
      setTimeout(() => {
        $('#GetViewreturnProduct_list').DataTable();
      }, 1);
    });
  }
}

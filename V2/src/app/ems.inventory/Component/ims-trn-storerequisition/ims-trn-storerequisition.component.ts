import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-ims-trn-storerequisition',
  templateUrl: './ims-trn-storerequisition.component.html',
  styleUrls: ['./ims-trn-storerequisition.component.scss'],
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
export class ImsTrnStorerequisitionComponent {

  srsummary_list:any;
  responsedata: any;
  showOptionsDivId: any;
  productsummary_list:any;

  constructor(public service :SocketService,private router:Router,private route:Router,private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {  
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSRSummary();
  }
  GetSRSummary(){
    var url = 'ImsTrnStoreRequisition/GetSRSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#srsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.srsummary_list = this.responsedata.srsummary_list;
      setTimeout(() => {
        $('#srsummary_list').DataTable();
      }, 1);
    });
  }
 
  onadd(){
    this.router.navigate(['/ims/ImsTrnStorerequisitionadd']);
  }
  onview(params:any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnStorerequisitionview',encryptedParam]);
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  Getproduct(storerequisition_gid: any){
    let param = {
      storerequisition_gid:storerequisition_gid
    } 
    var url = 'ImsTrnStoreRequisition/GetSRViewProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#productsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.productsummary_list = this.responsedata.storeRequisitionproduct_list;
      setTimeout(() => {
        $('#productsummary_list').DataTable();
      }, 1);
    });
  }
}
